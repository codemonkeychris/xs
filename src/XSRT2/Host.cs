using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace XSRT2
{
    public sealed class Host
    {
        XSRT.JScriptRuntime jsrt = null;
        XSRT2.StateManager state = new XSRT2.StateManager();
        Diff diff;
        string lastProgram = "";
        bool autoCheckUpdates = false;
        bool overwriteIfExists = true;
        DispatcherTimer dt;
        Type appType;
        string programFileName;
        const string defaultPath = "xs-program.js";
        const string defaultProgram = @"
var App;
(function (App) {
    App.render = function() {
        return { type:'TextBlock', text:'Hello from JS!' };
    }
    
})(App || (App = {}));
";

        public Host(ContentControl displayControl, Type appType, string programFileName)
        {
            this.appType = appType;
            this.programFileName = programFileName;
            diff = new Diff(state, displayControl);
            Handler.Command += delegate (object sender, CommandEventArgs e)
            {
                state.NotifyCommand(e);
                RenderIfNeeded();
            };
        }

        public bool StressReload { get; set; }

        public StateManager State { get { return state; } }

        public bool AutoCheckUpdates
        {
            get { return autoCheckUpdates; }
            set { autoCheckUpdates = value; UpdateAutoTimer(); }
        }

        public bool OverwriteIfExists
        {
            get { return overwriteIfExists; }
            set { overwriteIfExists = value; }
        }

        void UpdateAutoTimer()
        {
            if (dt != null)
            {
                dt.Stop();
                dt = null;
            }

            if (autoCheckUpdates)
            {
                dt = new DispatcherTimer();
                dt.Interval = TimeSpan.FromMilliseconds(250);
                dt.Tick += autoTimer_Tick;
                dt.Start();
            }
        }

        void autoTimer_Tick(object sender, object e)
        {
            CheckForUpdates();
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
                Display(state.RenderIfNeeded());
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
            return await ReadText(file);
        }

        // This is here purely as a debugging aid for hard coding content of the file in cases
        // where there is a problem
        // 
        async Task<string> ReadText(StorageFile file)
        {
            try
            {
                var str = await FileIO.ReadTextAsync(file);
                return str;
            }
            catch (UnauthorizedAccessException)
            {
                // need wait
                return @"var App;
                (function(App) {
                    App.render = function() {
                        return { type:'TextBlock', text:'Failed to load file (access denied)' };
                    }

                })(App || (App = { }));";
            }
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
            if (jsrt != null)
            {
                state.ReleaseEventHandlers();
                jsrt.ClearActive();
                jsrt.ClearTimers();
                jsrt = null;
            }
            state.NotifyChanged();

            jsrt = new XSRT.JScriptRuntime();
            jsrt.SetActive();
            jsrt.StartDebugging();
            // must call AddWinRTNamespace before AddHostObject
            //
            jsrt.AddWinRTNamespace("Windows");
            jsrt.AddWinRTNamespace("XSRT2");
            jsrt.AddHostObject("state", state);
            jsrt.AddHostObject("helpers", new Helpers());
            jsrt.Eval(program + "\r\n" + runtime);
        }

        void Display(RenderEventArgs renderEventArgs)
        {
            if (renderEventArgs != null && renderEventArgs.View != null)
            {
                diff.Process(renderEventArgs.View.ToString());
            }
        }
    }

    public sealed class Helpers
    {
        public bool? GetIsChecked(object v) { return ((ToggleButton)v).IsChecked; }
    }
}
