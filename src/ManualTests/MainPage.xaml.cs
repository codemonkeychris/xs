using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XSRT2;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ManualTests
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        UIAwareHost host;

        public MainPage()
        {
            this.InitializeComponent();
            this.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            this.VerticalContentAlignment = VerticalAlignment.Stretch;
            this.Content = new ContentControl()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch
            };
            host = new UIAwareHost((ContentControl)this.Content, typeof(MainPage), "tests.js");
            host.AutoCheckUpdates = true;        // will future changes to app.js in RoaminState directory will be shown?
            host.OverwriteIfExists = true;       // will embedded app.js always be overwriten on program start?
            host.Runtime.PreserveStateOnReload = false;  // will state be kept alive between program reloads
            host.Runtime.Initialized += Host_Initialized;
            host.Startup();
        }

        async void Host_Initialized(object sender, InitializedEventArgs e)
        {
            await Dispatcher.RunIdleAsync((IdleDispatchedHandler)async delegate (IdleDispatchedHandlerArgs ignore)
            {
                await host.Runtime.RunAllTests();
                var state = (IDictionary <string, object>)(host.Runtime.SaveState());
                state["mode"] = "results";
                host.Runtime.LoadState(state);
                host.Runtime.RenderIfNeeded();
                ApplicationView.GetForCurrentView().Title = "{succeeded:" + host.Runtime.GetTestSummary() + "}";
            });
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            host.Runtime.Close();
            host = null;
            base.OnNavigatingFrom(e);
        }
    }
}
