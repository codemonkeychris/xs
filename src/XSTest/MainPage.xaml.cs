﻿//#define STRESS_RELOAD
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
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

namespace XSTest
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
            this.Content = new ContentControl() {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalContentAlignment=HorizontalAlignment.Stretch,
                VerticalContentAlignment=VerticalAlignment.Stretch
            };
            host = new UIAwareHost((ContentControl)this.Content, typeof(MainPage), "app.js");

            host.AutoCheckUpdates = true; // will future changes to app.js in RoaminState directory will be shown?
            host.OverwriteIfExists = true; // will embedded app.js always be overwriten on program start?
            host.Runtime.Rendered += Host_Rendered;
            host.Startup();

        }
        private void Host_Rendered(object sender, DiffStats e)
        {
            ApplicationView.GetForCurrentView().Title = "{props:" + e.PropertySetCount + ", objs:" + e.ObjectCreateCount + ", elapsed:" + e.ElapsedMilliseconds.ToString("#")+ "ms }";
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            host.Runtime.Close();
            host = null;
            base.OnNavigatingFrom(e);
        }
    }


}
