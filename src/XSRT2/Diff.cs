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

namespace XSRT2
{
    public sealed class Diff
    {
        ContentControl control;
        JObject lastState;


        public Diff(ContentControl control)
        {
            this.control = control;
        }
        public void Process(string state)
        {
            var newState = JObject.Parse(state);

            control.Content = CreateFromState(newState);
        }

        static TextBlock CreateTextBlock(JObject obj)
        {
            TextBlock t = new TextBlock();
            SetTextBlockProperties(t, obj);
            return t;
        }
        static void SetTextBlockProperties(TextBlock t, JObject obj)
        {
            TrySet(obj, "text", t, (target, x) => target.Text = x.ToString());
            TrySet(obj, "fontSize", t, (target, x) => target.FontSize= x.Value<double>());
        }
        delegate void Setter<T>(T target, JToken value);
        static void TrySet<T>(JObject obj, string name, T target, Setter<T> setter)
        {
            JToken tok;
            if (obj.TryGetValue(name, out tok)) setter(target, tok);
        }

        static void SetFrameworkElementProperties(FrameworkElement t, JObject obj)
        {

        }
        static void SetPanelChildren(Panel t, JObject obj)
        {
            SetPanelChildrenWorker(t, obj["children"].AsJEnumerable());
        }
        static void SetPanelChildrenWorker(Panel t, IJEnumerable<JToken> items) { 
            foreach (var child in items)
            {
                if (child.Type == JTokenType.Array)
                {
                    SetPanelChildrenWorker(t, child.AsJEnumerable());
                }
                else
                {
                    var instance = Diff.CreateFromState((JObject)child);
                    t.Children.Add(instance);
                }
            }
        }
        static void SetPanelProperties(Panel t, JObject obj)
        {
            SetFrameworkElementProperties(t, obj);
            SetPanelChildren(t, obj);
        }
        static StackPanel CreateStackPanel(JObject obj)
        {
            StackPanel t = new StackPanel();
            SetPanelProperties(t, obj);
            return t;
        }
        static T Creator<T, V>(JObject obj, IEnumerable<PropSetter<V>> props) where T:V,new()
        {
            T value = new T();
            foreach (var s in props) { s.Set(value, obj); }
            return value;
        }
        delegate FrameworkElement CreateCallback(JObject obj);

        static Dictionary<string, CreateCallback> handlers;
        static Dictionary<string, CreateCallback> GetHandlers()
        {
            if (handlers == null)
            {
                handlers = new Dictionary<string, CreateCallback>();
                handlers["StackPanel"] = CreateStackPanel;
                handlers["TextBlock"] = CreateTextBlock ;
            }
            return handlers;
        }


        internal static FrameworkElement CreateFromState(JObject item)
        {
            var type = item["type"].ToString();
            CreateCallback create;
            if (GetHandlers().TryGetValue(type, out create))
            {
                return create(item);
            }
            return new TextBlock() { FontSize = 48, Text = "Not found" };
        }
    }
    abstract class PropSetter<ObjectType>
    {
        public PropSetter() { }
        public abstract void Set(ObjectType target, JObject parentValue);
    }
}
