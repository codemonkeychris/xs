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
        XSRT.JScriptRuntime jsrt = null;
        XSRT2.StateManager state = new XSRT2.StateManager();

        public MainPage()
        {
            this.InitializeComponent();
            Initialize();
        }

        void Initialize()
        {
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
            jsrt.Eval(@"
host.state.addEventListener('render', function(ev) { 
    ev.view = 'hello world!';
}); 
");
            Display(state.RenderIfNeeded());
        }

        private void Display(RenderEventArgs renderEventArgs)
        {
            this.Content = new ContentControl() { FontSize=48, Margin=new Thickness(5), Content = renderEventArgs.View };
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            jsrt.ClearActive();
            jsrt = null;
            base.OnNavigatingFrom(e);
        }
    }


}
