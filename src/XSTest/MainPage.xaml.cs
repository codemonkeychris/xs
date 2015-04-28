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
        public MainPage()
        {
            this.InitializeComponent();
            XSRT2.Test.Try("from C#!");

            var x = new XSRT.JScriptRuntime();
            x.EchoNotify += X_EchoNotify;
            var f = new Test2();
            x.AddWinRTNamespace("XSRT2"); // must be first
            x.AddHostObject("foo", f);
            x.Eval("host.echo('echo from JS!');");
            x.Eval("host.foo.echo('from JS!');");
            x.Eval("XSRT2.Test.try('from JS!');");
        }

        private void X_EchoNotify(string message)
        {
            var x = message;
        }
    }


}
