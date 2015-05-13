// #define STRESS_RELOAD
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XSRT2;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace XSTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        XSRT.JScriptRuntime jsrt = null;
        XSRT2.StateManager state = new XSRT2.StateManager();
        Diff diff;
        DispatcherTimer dt;
        int frame;
        string lastProgram = "";

        public MainPage()
        {
            this.InitializeComponent();
            this.Content = new ContentControl();
            diff = new Diff(state, (ContentControl)this.Content);
            Diff.Command += delegate (object sender, CommandEventArgs e)
            {
                state.NotifyCommand(e);
            };
            Startup();
            
        }
        async void Startup()
        {
            state.SetState("frame", (frame++).ToString());
            await InitFile();
            await CheckFile();
            dt = new DispatcherTimer();
#if STRESS_RELOAD
            dt.Interval = TimeSpan.FromMilliseconds(16);
#else
            dt.Interval = TimeSpan.FromMilliseconds(250);
#endif
            dt.Tick += dt_Tick;
            dt.Start();
        }

        private void dt_Tick(object sender, object e)
        {
            state.SetState("frame", (frame++).ToString());
            CheckFile();
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
        const string path = "xs-program.js";
        const string defaultApp = @"
var App;
(function (App) {
    App.render = function() {
        return { type:'TextBlock', text:'Hello from JS!' };
    }
    
})(App || (App = {}));
";
        async Task<string> InitFile()
        {

            StorageFile file;
            try
            {
                var found = await Windows.Storage.ApplicationData.Current.RoamingFolder.TryGetItemAsync(path);
                if (found == null)
                {
                    file = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFileAsync(path, Windows.Storage.CreationCollisionOption.FailIfExists);
                    await FileIO.WriteTextAsync(file, defaultApp);
                }
                else
                {
                    file = (StorageFile)found;
                }
            }
            catch
            {
                file = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFileAsync(path, CreationCollisionOption.OpenIfExists);
            }
            return await ReadText(file);
        }

        // This is here purely as a debugging aid for hard coding content of the file in cases
        // where there is a problem
        // 
        async Task<string> ReadText(StorageFile file)
        {
            var str = await FileIO.ReadTextAsync(file);
            return str;
        }

        async Task<string> CheckFile()
        {
            var file = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFileAsync(path, Windows.Storage.CreationCollisionOption.OpenIfExists);
            //var file = await Windows.Storage.KnownFolders.DocumentsLibrary.CreateFileAsync(path, Windows.Storage.CreationCollisionOption.OpenIfExists);
            var contents = await ReadText(file);
            bool changed = lastProgram != contents;
#if STRESS_RELOAD
            var b = new byte[1];
            new Random().NextBytes(b);
            changed = changed || (b[0] < 25); // 10% chance of random reload
#endif
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
                jsrt = null;
            }
            state.NotifyChanged();

            jsrt = new XSRT.JScriptRuntime();
            jsrt.SetActive();
            // must call AddWinRTNamespace before AddHostObject
            //
            jsrt.AddWinRTNamespace("Windows"); 
            jsrt.AddWinRTNamespace("XSRT2"); 
            jsrt.AddHostObject("state", state);
            jsrt.Eval(program + "\r\n" + runtime);
        }

        private void Display(RenderEventArgs renderEventArgs)
        {
            if (renderEventArgs != null)
            {
                diff.Process(renderEventArgs.View.ToString());
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            jsrt.ClearActive();
            jsrt = null;
            base.OnNavigatingFrom(e);
        }
    }


}
