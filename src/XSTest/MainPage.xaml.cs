﻿using System;
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
            dt.Interval = TimeSpan.FromMilliseconds(1000);
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

        async Task<string> InitFile()
        {
            StorageFile file;
            try
            {
                var found = await Windows.Storage.ApplicationData.Current.RoamingFolder.TryGetItemAsync(path);
                if (found == null)
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
                else
                {
                    file = (StorageFile)found;
                }
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
            try
            {
                jsrt.Eval(program);
            }
            catch (Exception e)
            {
                Display(new RenderEventArgs() { View = "{ type:'TextBlock', text:'Error:"+e.ToString().Replace("\'", "\"")+"' }" });
                return;
            }
            jsrt.Eval(@"
function render(ev) {
    ev.view = (App && App.render) ? JSON.stringify(App.render()) : 'not found';
}
function command(ev) {
    var handler = App && App.eventHandlers && App.eventHandlers[ev.commandHandlerToken];
    if (handler) { handler(ev.sender, ev.eventArgs); }
}
host.state.addEventListener('render', render); 
host.state.addEventListener('command', command); 
if (App && App.setInitialState) { App.setInitialState(); }
");
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
