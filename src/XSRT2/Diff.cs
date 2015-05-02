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
        Dictionary<string, object> namedObjectMap = new Dictionary<string, object>();

        public Diff(ContentControl control)
        {
            this.control = control;
        }
        public void Process(string state)
        {
            var newState = JObject.Parse(state);
            control.Content = CreateFromState(newState);
        }

        TextBlock CreateTextBlock(JObject obj)
        {
            TextBlock t = CreateOrGetLast<TextBlock>(obj);
            SetTextBlockProperties(t, obj);
            return t;
        }
        TextBox CreateTextBox(JObject obj)
        {
            TextBox t = CreateOrGetLast<TextBox>(obj);
            SetTextBoxProperties(t, obj);
            return t;
        }
        T CreateOrGetLast<T>(JObject obj) where T:new()
        {
            JToken name;
            if (obj.TryGetValue("name", out name))
            {
                object value;
                if (namedObjectMap.TryGetValue(name.ToString(), out value))
                {
                    if (value != null && value is T)
                    {
                        return (T)value;
                    }
                }
            }
            return new T();
        }
        void SetTextBoxProperties(TextBox t, JObject obj)
        {
            SetControlProperties(t, obj);
            TrySet(obj, "text", t, (target, x) => target.Text = x.ToString());
        }
        void SetControlProperties(Control t, JObject obj)
        {
            SetFrameworkElementProperties(t, obj);
            TrySet(obj, "background", t, (target, x) => target.Background = XamlStringParse<Brush>(x));
            TrySet(obj, "foreground", t, (target, x) => target.Foreground = XamlStringParse<Brush>(x));
            TrySet(obj, "fontFamily", t, (target, x) => target.FontFamily = new FontFamily(x.ToString()));
            TrySet(obj, "fontSize", t, (target, x) => target.FontSize = x.Value<double>());
            TrySet(obj, "fontWeight", t, (target, x) => target.FontWeight = ParseEnum<FontWeight>(x));
        }
        void SetTextBlockProperties(TextBlock t, JObject obj)
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
        void SetFrameworkElementProperties(FrameworkElement t, JObject obj)
        {
            TrySet(obj, "horizontalAlignment", t, (target, x) => target.HorizontalAlignment =  ParseEnum<HorizontalAlignment>(x));
            TrySet(obj, "verticalAlignment", t, (target, x) => target.VerticalAlignment = ParseEnum<VerticalAlignment>(x));
            TrySet(obj, "margin", t, (target, x) => target.Margin = XamlStringParse<Thickness>(x));
            TrySet(obj, "name", t, (target, x) => {
                target.Name = x.ToString();
                namedObjectMap[target.Name] = target;
            });
        }
        void SetPanelChildren(Panel t, JObject obj)
        {
            SetFrameworkElementProperties(t, obj);
            List<UIElement> children = new List<UIElement>();
            CollectPanelChildrenWorker(t, obj["children"].AsJEnumerable(), children);
            var setChildrenNeeded = false;
            if (t.Children.Count == children.Count)
            {
                for (int i=0; i< children.Count; i++)
                {
                    if (!object.ReferenceEquals(children[i], t.Children[i]))
                    {
                        setChildrenNeeded = true;
                    }
                }
            }
            else
            {
                setChildrenNeeded = true;
            }

            if (setChildrenNeeded)
            {
                t.Children.Clear();
                foreach (var child in children) { t.Children.Add(child); }
            }
        }
        void CollectPanelChildrenWorker(Panel t, IJEnumerable<JToken> items, List<UIElement> children) { 
            foreach (var child in items)
            {
                if (child.Type == JTokenType.Array)
                {
                    CollectPanelChildrenWorker(t, child.AsJEnumerable(), children);
                }
                else
                {
                    var instance = CreateFromState((JObject)child);
                    children.Add(instance);
                }
            }
        }
        void SetPanelProperties(Panel t, JObject obj)
        {
            SetFrameworkElementProperties(t, obj);
            SetPanelChildren(t, obj);
        }
        StackPanel CreateStackPanel(JObject obj)
        {
            StackPanel t = CreateOrGetLast<StackPanel>(obj);
            SetPanelProperties(t, obj);
            return t;
        }
        delegate FrameworkElement CreateCallback(JObject obj);

        Dictionary<string, CreateCallback> handlers;
        Dictionary<string, CreateCallback> GetHandlers()
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


        internal FrameworkElement CreateFromState(JObject item)
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
}
