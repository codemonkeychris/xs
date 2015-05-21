using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Text;
using System.Reflection;
using Windows.UI.Xaml.Controls.Primitives;

namespace XSRT2
{
    public sealed class Diff
    {
        ContentControl control;
        StateManager stateManager;
        JObject lastUI;
        Dictionary<string, object> namedObjectMap = new Dictionary<string, object>();

        public Diff(StateManager state, ContentControl control)
        {
            this.stateManager = state;
            this.control = control;
        }
        public void Process(string ui)
        {
            var newUI = JObject.Parse(ui);
            var defer = new List<Handler.DeferSetter>();
            control.Content = Handler.CreateFromState(newUI, lastUI, namedObjectMap, defer);
            foreach (var d in defer)
            {
                d.Do();
            }
            lastUI = newUI;
        }
   }
}
