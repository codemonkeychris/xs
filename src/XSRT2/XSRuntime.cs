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
    public sealed class XSRuntime
    {
        static string runtimeCache = null;

        JSRT.JScriptRuntime jsrt = null;
        XSRT2.JScriptXSRuntimeProjection hostProjection;
        Diff diff;
        string lastProgram = "";
        string program = "";
        UInt64 lastSeenVersion = UInt64.MaxValue;
        string runningTest = "n/a";
        List<string> tests = new List<string>();
        List<LogEntry> testLogs = new List<LogEntry>();

        public XSRuntime(ContentControl displayControl)
        {
            PreserveStateOnReload = true;
            hostProjection = new XSRT2.JScriptXSRuntimeProjection(this);
            diff = new Diff(displayControl);
            Handler.Command += delegate (object sender, CommandEventArgs e)
            {
                hostProjection.CallCommand(e);
                RenderIfNeeded();
            };
        }

        public string Program
        {
            get { return program; }
            set { program = value; SetProgram(value, false); } 
        }

        public void ForceCleanReload()
        {
            SetProgram(lastProgram, true);
        }


        public void SizeChanged(double width, double height)
        {
            // UNDONE: design question - should resize force a recalc? Here we push in some
            // new state which will force a re-render... hmm... 
            //
            if (jsrt != null)
            {
                var state = (IDictionary<string, object>)jsrt.SaveState();
                state["clientHeight"] = height;
                state["clientWidth"] = width;
                jsrt.LoadState(state);
                RenderIfNeeded();
            }
        }

        public event EventHandler<DiffStats> Rendered;
        public event EventHandler<InitializedEventArgs> Initialized; 

        public DependencyObject LastGeneratedView { get { return diff.LastGeneratedView; } }

        public bool PreserveStateOnReload { get; set; }
        
        public void RegisterTests([ReadOnlyArray] string[] names)
        {
            tests.AddRange(names);
        }
        public object SaveState() { return jsrt.SaveState(); }
        public void LoadState(object value) { jsrt.LoadState(value); }

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


        internal static string ProgramWithMessage(string message)
        {
            return @"var App;
                (function(App) {
                    App.render = function() {
                        return { type:'TextBlock', text:'" + message.Replace('\'', '\"') + @"' };
                    }

                })(App || (App = { }));";
        }

        internal static string ProgramWithException(Exception x)
        {
            return ProgramWithMessage(x.ToString());
        }

        internal async Task<string> SetProgram(string contents, bool forceReload)
        {
            bool changed = forceReload || lastProgram != contents;

            if (changed)
            {
                lastProgram = contents;
                string runtime = runtimeCache;
                if (runtime == null)
                {
                    runtime = runtimeCache = await RuntimeHelpers.GetRuntimeJavaScript();
                }
                Initialize(contents, runtime, forceReload);
            }
            return contents;
        }


        void Initialize(string program, string runtime, bool forceReload)
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

            if (!PreserveStateOnReload || forceReload)
            {
                hostProjection.IsInitialized = false;
                testLogs.Clear(); // only clear logs if we want to start fresh
            }
            try
            {
                jsrt.Eval(program);
#if DEBUG
                // force active transition to make sure every survives across sessions
                jsrt.ClearActive();
                jsrt.SetActive();
#endif
                jsrt.Eval(runtime);
            }
            catch (Exception x)
            {
                jsrt.Eval(ProgramWithException(x) + "\r\n" + runtime);
            }
            bool restoreState = !forceReload && PreserveStateOnReload && lastState != null;
            if (restoreState)
            {
                jsrt.LoadState(lastState);
            }
            if (forceReload)
            {
                diff.Reset();
            }
            if (Initialized != null)
            {
                Initialized(this, new InitializedEventArgs() { RestoredState = restoreState });
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
