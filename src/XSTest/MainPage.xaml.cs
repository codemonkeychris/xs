using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        XSRT.JScriptRuntime x = new XSRT.JScriptRuntime();
        XSRT2.StateManager state = new XSRT2.StateManager();

        public MainPage()
        {
            this.InitializeComponent();

            x.EchoNotify += X_EchoNotify;
            x.AddWinRTNamespace("XSRT2"); // must be first
            x.AddHostObject("state", state);
            
            // this works, yeah!
            //
            x.Eval("host.state.addEventListener('render', function() { host.echo('hit'); }); host.state.notifyChanged(); host.state.renderIfNeeded();");

            // this doesn't work, booh!
            //
            x.Eval("host.state.notifyChanged();");
            state.RenderIfNeeded();
        }

        private void X_EchoNotify(string message)
        {
            var x = message;
        }
    }


}
