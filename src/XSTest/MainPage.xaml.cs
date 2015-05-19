//#define STRESS_RELOAD
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
        Host host;

        public MainPage()
        {
            this.InitializeComponent();
            this.Content = new ContentControl();
            host = new Host((ContentControl)this.Content, typeof(MainPage), "app.js") {
                AutoCheckUpdates = true, // will future changes to app.js in RoaminState directory will be shown?
                OverwriteIfExists = true // will embedded app.js always be overwriten on program start?
            };
            host.Startup();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            host.Close();
            host = null;
            base.OnNavigatingFrom(e);
        }
    }


}
