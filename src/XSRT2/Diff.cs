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
    public struct DiffStats
    {
        public int EventSetCount;
        public int PropertySetCount;
        public int ObjectCreateCount;
        public double ElapsedMilliseconds;
    }
    public sealed class Diff
    {
        ContentControl control;
        JObject lastUI;
        Dictionary<string, object> namedObjectMap = new Dictionary<string, object>();

        public Diff(ContentControl control)
        {
            this.control = control;
        }
        public DiffStats Process(string ui)
        {
            var newUI = JObject.Parse(ui);
            var context = new Handler.DiffContext(namedObjectMap) {
                Defer = new List<Handler.DeferSetter>(),
                Start = DateTime.Now,
                PropertySetCount = 0,
                ObjectCreateCount = 0,
                EventSetCount = 0,
                HostElement = control
            };
            control.Content = Handler.CreateFromState(newUI, lastUI, context);
            foreach (var d in context.Defer)
            {
                d.Do();
            }
            context.End = DateTime.Now;
            namedObjectMap = context.GetNamedObjectMap();
            lastUI = newUI;
            return new DiffStats
            {
                PropertySetCount = context.PropertySetCount,
                ObjectCreateCount = context.ObjectCreateCount,
                ElapsedMilliseconds = (context.End -context.Start).TotalMilliseconds
            };
        }
   }
}
