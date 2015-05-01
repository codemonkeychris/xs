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
        string lastProgram = "";

        public MainPage()
        {
            this.InitializeComponent();
            this.Content = new ContentControl();
            diff = new Diff((ContentControl)this.Content);
            Startup();
        }
        async void Startup()
        {
            await InitFile();
            await CheckFile();
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(500);
            dt.Tick += dt_Tick;
            dt.Start();

        }

        private void dt_Tick(object sender, object e)
        {
            CheckFile();
        }
        const string path = "xs-program.js";

        async Task<string> InitFile()
        {
            StorageFile file;
            try
            {
                file = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFileAsync(path, Windows.Storage.CreationCollisionOption.FailIfExists);
                await FileIO.WriteTextAsync(file, @"
var App;
(function (App) {
    
    App.render = function() {
        return { type:'TextBlock', text:'Hello from JS!' };
    }
    
})(App || (App = {}));
");
            }
            catch
            {
                file = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFileAsync(path, CreationCollisionOption.OpenIfExists);
            }
            var str = await FileIO.ReadTextAsync(file);
            var pp = file.Path;
            return str;

        }

        async Task<string> CheckFile()
        {
            var file = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFileAsync(path, Windows.Storage.CreationCollisionOption.OpenIfExists);
            //var file = await Windows.Storage.KnownFolders.DocumentsLibrary.CreateFileAsync(path, Windows.Storage.CreationCollisionOption.OpenIfExists);
            var contents = await FileIO.ReadTextAsync(file);
            if (lastProgram != contents)
            {
                Initialize(contents);
            }
            return contents;
        }

        void Initialize(string program)
        {
            lastProgram = program;
            if (jsrt != null)
            {
                jsrt.ClearActive();
                jsrt = null;
            }
            state.NotifyChanged();

            jsrt = new XSRT.JScriptRuntime();
            jsrt.SetActive();
            jsrt.AddWinRTNamespace("XSRT2"); // must be first
            jsrt.AddHostObject("state", state);
            jsrt.Eval(program);
            jsrt.Eval(@"
host.state.addEventListener('render', function(ev) { 
    ev.view = App ? JSON.stringify(App.render()) : 'not found';
}); 
");
            Display(state.RenderIfNeeded());
        }

        private void Display(RenderEventArgs renderEventArgs)
        {

            diff.Process(renderEventArgs.View.ToString());
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            jsrt.ClearActive();
            jsrt = null;
            base.OnNavigatingFrom(e);
        }
    }


}
