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
        DispatcherTimer dt;
        int frame;

        public MainPage()
        {
            this.InitializeComponent();
            this.Content = new ContentControl();
            host = new Host((ContentControl)this.Content) { AutoCheckUpdates = true };
            host.State.SetState("frame", (frame++).ToString());
            host.Startup();

            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(1000);
            dt.Tick += dt_Tick;
            dt.Start();
        }

        private void dt_Tick(object sender, object e)
        {
            host.State.SetState("frame", (frame++).ToString());
            host.RenderIfNeeded();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            host.Close();
            host = null;
            base.OnNavigatingFrom(e);
        }
    }


}
