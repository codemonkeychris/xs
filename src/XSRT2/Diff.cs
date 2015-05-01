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
        static TextBox CreateTextBox(JObject obj)
        {
            TextBox t = new TextBox();
            SetTextBoxProperties(t, obj);
            return t;
        }
        static void SetTextBoxProperties(TextBox t, JObject obj)
        {
            SetControlProperties(t, obj);
            TrySet(obj, "text", t, (target, x) => target.Text = x.ToString());
        }
        static void SetControlProperties(Control t, JObject obj)
        {
            SetFrameworkElementProperties(t, obj);
            TrySet(obj, "background", t, (target, x) => target.Background = XamlStringParse<Brush>(x));
            TrySet(obj, "foreground", t, (target, x) => target.Foreground = XamlStringParse<Brush>(x));
            TrySet(obj, "fontFamily", t, (target, x) => target.FontFamily = new FontFamily(x.ToString()));
            TrySet(obj, "fontSize", t, (target, x) => target.FontSize = x.Value<double>());
            TrySet(obj, "fontWeight", t, (target, x) => target.FontWeight = ParseEnum<FontWeight>(x));
        }
        static void SetTextBlockProperties(TextBlock t, JObject obj)
        {
            SetFrameworkElementProperties(t, obj);
            TrySet(obj, "text", t, (target, x) => target.Text = x.ToString());
            TrySet(obj, "fontFamily", t, (target, x) => target.FontFamily = new FontFamily(x.ToString()));
            TrySet(obj, "fontSize", t, (target, x) => target.FontSize= x.Value<double>());
            TrySet(obj, "fontWeight", t, (target, x) => target.FontWeight = ParseEnum<FontWeight>(x));
        }
        delegate void Setter<T>(T target, JToken value);
        static void TrySet<T>(JObject obj, string name, T target, Setter<T> setter)
        {
            JToken tok;
            if (obj.TryGetValue(name, out tok)) setter(target, tok);
        }

        static T ParseEnum<T>(JToken v) 
        {
            return (T)Enum.Parse(typeof(T), v.ToString());
        }
        static T XamlStringParse<T>(JToken v)
        {
            return (T)Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(T), v.ToString());
        }
        static void SetFrameworkElementProperties(FrameworkElement t, JObject obj)
        {
            TrySet(obj, "horizontalAlignment", t, (target, x) => target.HorizontalAlignment =  ParseEnum<HorizontalAlignment>(x));
            TrySet(obj, "verticalAlignment", t, (target, x) => target.VerticalAlignment = ParseEnum<VerticalAlignment>(x));
            TrySet(obj, "margin", t, (target, x) => target.Margin = XamlStringParse<Thickness>(x));
        }
        static void SetPanelChildren(Panel t, JObject obj)
        {
            SetFrameworkElementProperties(t, obj);
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
                handlers["TextBox"] = CreateTextBox;
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
            return new TextBlock() { FontSize = 48, Text = "'"+type+"'Not found" };
        }
    }
    abstract class PropSetter<ObjectType>
    {
        public PropSetter() { }
        public abstract void Set(ObjectType target, JObject parentValue);
    }
}
