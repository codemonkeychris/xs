using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace XSRT2
{
    public sealed class Host
    {
        JSRT.JScriptRuntime jsrt = null;
        XSRT2.JScriptHostProjection hostProjection;
        Diff diff;
        string lastProgram = "";
        bool autoCheckUpdates = false;
        bool overwriteIfExists = true;
        DispatcherTimer fileWatcherUpdateTimer;
        Type appType;
        string programFileName;
        UInt64 lastSeenVersion = UInt64.MaxValue;
        string runningTest = "n/a";
        List<string> tests = new List<string>();
        List<LogEntry> testLogs = new List<LogEntry>();
        const string defaultPath = "xs-program.js";

        public Host(ContentControl displayControl, Type appType, string programFileName)
        {
            PreserveStateOnReload = true;
            this.appType = appType;
            this.programFileName = programFileName;
            hostProjection = new XSRT2.JScriptHostProjection(this);
            diff = new Diff(displayControl);
            Handler.Command += delegate (object sender, CommandEventArgs e)
            {
                hostProjection.CallCommand(e);
                RenderIfNeeded();
            };
        }

        public event EventHandler<DiffStats> Rendered;
        public event EventHandler<InitializedEventArgs> Initialized; 

        public bool StressReload { get; set; }

        internal DependencyObject LastGeneratedView { get { return diff.LastGeneratedView; } }

        public bool AutoCheckUpdates
        {
            get { return autoCheckUpdates; }
            set { autoCheckUpdates = value; UpdateAutoTimer(); }
        }
        public bool PreserveStateOnReload { get; set; }
        public bool OverwriteIfExists
        {
            get { return overwriteIfExists; }
            set { overwriteIfExists = value; }
        }
        
        public void RegisterTests([ReadOnlyArray] string[] names)
        {
            tests.AddRange(names);
        }
        public object SaveState() { return jsrt.SaveState(); }
        public void LoadState(object value) { jsrt.LoadState(value); }

        void UpdateAutoTimer()
        {
            if (fileWatcherUpdateTimer != null)
            {
                fileWatcherUpdateTimer.Stop();
                fileWatcherUpdateTimer = null;
            }

            if (autoCheckUpdates)
            {
                fileWatcherUpdateTimer = new DispatcherTimer();
                fileWatcherUpdateTimer.Interval = TimeSpan.FromMilliseconds(250);
                fileWatcherUpdateTimer.Tick += autoTimer_Tick;
                fileWatcherUpdateTimer.Start();
            }
        }

        void autoTimer_Tick(object sender, object e)
        {
            CheckForUpdates();
        }
        public void LogAssert(bool result, string message)
        {
            testLogs.Add(new LogEntry() { Result = result, Message = message, Test = runningTest });
        }
        public LogEntry[] GetLogs()
        {
            return testLogs.ToArray();
        }
        public bool GetTestSummary()
        {
            return testLogs.All(e => e.Result);
        }
        public IAsyncOperation<int> RunTest(string test)
        {
            runningTest = test;
            hostProjection.RaiseTestSetup(test);
            RenderIfNeeded();
            hostProjection.RaiseTestReady(test);
            runningTest = "n/a";
            return Task<int>.FromResult(0).AsAsyncOperation();
        }
        public IAsyncOperation<int> RunAllTests()
        {
            return RunAllTestsWorker().AsAsyncOperation();
        }
        async Task<int> RunAllTestsWorker()
        {
            int r = 0;
            foreach (var t in tests)
            {
                r += await RunTest(t);
            }
            return r;
        }
        public async void Startup()
        {
            await InitFile();
            await CheckFile();
            RenderIfNeeded();
        }
        public async void CheckForUpdates()
        {
            await CheckFile();
            RenderIfNeeded();
        }
        public void Close()
        {
            jsrt.ClearActive();
            jsrt = null;
        }
        public void RenderIfNeeded()
        {
            try
            {
                var lastVer = jsrt.GetStateVersion();
                if (lastVer != lastSeenVersion)
                {
                    lastSeenVersion = lastVer;

                    Display(hostProjection.CallRender());
                }
            }
            catch (Exception x)
            {
                Display(new RenderEventArgs() { View = "{ type:'TextBlock', text:'Error:" + x.ToString().Replace("\'", "\"") + "' }" });
                return;
            }
        }

        async Task<string> InitFile()
        {

            StorageFile file;
            try
            {
                var found = await Windows.Storage.ApplicationData.Current.RoamingFolder.TryGetItemAsync(programFileName);
                if (found == null)
                {
                    var program = await RuntimeHelpers.GetResource(appType, programFileName);
                    file = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFileAsync(programFileName, Windows.Storage.CreationCollisionOption.FailIfExists);
                    await FileIO.WriteTextAsync(file, program);
                }
                else
                {
                    file = (StorageFile)found;
                    if (overwriteIfExists)
                    {
                        var program = await RuntimeHelpers.GetResource(appType, programFileName);
                        await FileIO.WriteTextAsync(file, program);
                    }
                }
            }
            catch
            {
                file = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFileAsync(programFileName, CreationCollisionOption.OpenIfExists);
            }

            if (overwriteIfExists)
            {
                return await RuntimeHelpers.GetResource(appType, programFileName);
            }
            else
            {
                return await ReadText(file);
            }
        }

        // This is here purely as a debugging aid for hard coding content of the file in cases
        // where there is a problem
        // 
        static async Task<string> ReadText(StorageFile file)
        {
            try
            {
                var str = await FileIO.ReadTextAsync(file);
                return str;
            }
            catch (UnauthorizedAccessException)
            {
                // need wait
                return ProgramWithMessaage("Failed to load file (access denied)");
            }
        }

        static string ProgramWithMessaage(string message)
        {
            return @"var App;
                (function(App) {
                    App.render = function() {
                        return { type:'TextBlock', text:'" + message.Replace('\'', '\"') + @"' };
                    }

                })(App || (App = { }));";
        }

        static string ProgramWithException(Exception x)
        {
            return ProgramWithMessaage(x.ToString());
        }

        async Task<string> CheckFile()
        {
            var file = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFileAsync(programFileName, Windows.Storage.CreationCollisionOption.OpenIfExists);
            //var file = await Windows.Storage.KnownFolders.DocumentsLibrary.CreateFileAsync(path, Windows.Storage.CreationCollisionOption.OpenIfExists);
            var contents = await ReadText(file);
            bool changed = lastProgram != contents;

            if (StressReload)
            {
                var b = new byte[1];
                new Random().NextBytes(b);
                changed = changed || (b[0] < 50); // 20% chance of random reload
            }
            if (changed)
            {
                lastProgram = contents;
                var runtime = await RuntimeHelpers.GetRuntimeJavaScript();
                Initialize(contents, runtime);
            }
            return contents;
        }

        void Initialize(string program, string runtime)
        {
            object lastState = null;
            if (jsrt != null)
            {
                lastState = jsrt.SaveState();
                hostProjection.ReleaseEventHandlers();
                jsrt.ClearActive();
                jsrt.ClearTimers();
                jsrt = null;
            }
            lastSeenVersion = ulong.MinValue;

            jsrt = new JSRT.JScriptRuntime();
            jsrt.SetActive();
            jsrt.StartDebugging();
            // must call AddWinRTNamespace before AddHostObject
            //
            jsrt.AddWinRTNamespace("Windows");
            jsrt.AddWinRTNamespace("XSRT2");
            jsrt.AddGlobalObject("xsrt", hostProjection);
            tests.Clear(); // always clear tests, they get re-registered

            if (!PreserveStateOnReload)
            {
                hostProjection.IsInitialized = false;
                testLogs.Clear(); // only clear logs if we want to start fresh
            }
            try
            {
                jsrt.Eval(program + "\r\n" + runtime);
            }
            catch (Exception x)
            {
                jsrt.Eval(ProgramWithException(x) + "\r\n" + runtime);
            }
            if (PreserveStateOnReload && lastState != null)
            {
                jsrt.LoadState(lastState);
            }
            if (Initialized != null)
            {
                Initialized(this, new InitializedEventArgs() { RestoredState = PreserveStateOnReload && lastState != null });
            }
        }

        void Display(RenderEventArgs renderEventArgs)
        {
            if (renderEventArgs != null && renderEventArgs.View != null)
            {
                var stats = diff.Process(renderEventArgs.View.ToString());
                if (Rendered != null)
                {
                    Rendered(this, stats);
                }
            }
        }
    }

    public sealed class InitializedEventArgs
    {
        public bool RestoredState { get; set; }
    }

    public struct LogEntry
    {
        public bool Result;
        public string Message;
        public string Test;
    }
}
