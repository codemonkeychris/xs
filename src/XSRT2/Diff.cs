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
            control.Content = CreateFromState(newState, lastState);
            lastState = newState;
        }

        TextBlock CreateTextBlock(JObject obj, JObject lastObj)
        {
            TextBlock t = CreateOrGetLast<TextBlock>(obj);
            SetTextBlockProperties(t, obj, lastObj);
            return t;
        }
        TextBox CreateTextBox(JObject obj, JObject lastObj)
        {
            TextBox t = CreateOrGetLast<TextBox>(obj);
            SetTextBoxProperties(t, obj, lastObj);
            return t;
        }
        Button CreateButton(JObject obj, JObject lastObj)
        {
            Button t = CreateOrGetLast<Button>(obj);
            SetButtonProperties(t, obj, lastObj);
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
        void SetTextBoxProperties(TextBox t, JObject obj, JObject lastObj)
        {
            SetControlProperties(t, obj, lastObj);
            TrySet(obj, lastObj, "text", t, (target, x, lastX) => target.Text = x.ToString());
        }
        void SetButtonProperties(Button t, JObject obj, JObject lastObj)
        {
            SetControlProperties(t, obj, lastObj);
            TrySet(obj, lastObj, "content", t, (target, x, lastX) => target.Content = CreateFromState((JObject)x, (JObject)lastX));
        }
        void SetControlProperties(Control t, JObject obj, JObject lastObj)
        {
            SetFrameworkElementProperties(t, obj, lastObj);
            TrySet(obj, lastObj, "background", t, (target, x, lastX) => target.Background = XamlStringParse<Brush>(x));
            TrySet(obj, lastObj, "foreground", t, (target, x, lastX) => target.Foreground = XamlStringParse<Brush>(x));
            TrySet(obj, lastObj, "fontFamily", t, (target, x, lastX) => target.FontFamily = new FontFamily(x.ToString()));
            TrySet(obj, lastObj, "fontSize", t, (target, x, lastX) => target.FontSize = x.Value<double>());
            TrySet(obj, lastObj, "fontWeight", t, (target, x, lastX) => target.FontWeight = ParseEnum<FontWeight>(x));
        }
        void SetTextBlockProperties(TextBlock t, JObject obj, JObject lastObj)
        {
            SetFrameworkElementProperties(t, obj, lastObj);
            TrySet(obj, lastObj, "text", t, (target, x, lastX) => target.Text = x.ToString());
            TrySet(obj, lastObj, "fontFamily", t, (target, x, lastX) => target.FontFamily = new FontFamily(x.ToString()));
            TrySet(obj, lastObj, "fontSize", t, (target, x, lastX) => target.FontSize= x.Value<double>());
            TrySet(obj, lastObj, "fontWeight", t, (target, x, lastX) => target.FontWeight = ParseEnum<FontWeight>(x));
        }
        delegate void Setter<T>(T target, JToken value, JToken lastValue);
        static void TrySet<T>(JObject obj, JObject last, string name, T target, Setter<T> setter)
        {
            JToken tok;
            JToken tokLast = null;
            if (obj.TryGetValue(name, out tok))
            {
                if (last != null && last.TryGetValue(name, out tokLast))
                {
                    if (tokLast.ToString() == tok.ToString())
                    {
                        return; // bail early if old & new are the same
                    }
                }
                setter(target, tok, tokLast);
            }
        }

        static T ParseEnum<T>(JToken v) 
        {
            return (T)Enum.Parse(typeof(T), v.ToString());
        }
        static T XamlStringParse<T>(JToken v)
        {
            return (T)Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(T), v.ToString());
        }
        void SetFrameworkElementProperties(FrameworkElement t, JObject obj, JObject lastObj)
        {
            TrySet(obj, lastObj, "horizontalAlignment", t, (target, x, lastX) => target.HorizontalAlignment =  ParseEnum<HorizontalAlignment>(x));
            TrySet(obj, lastObj, "verticalAlignment", t, (target, x, lastX) => target.VerticalAlignment = ParseEnum<VerticalAlignment>(x));
            TrySet(obj, lastObj, "margin", t, (target, x, lastX) => target.Margin = XamlStringParse<Thickness>(x));
            TrySet(obj, lastObj, "name", t, (target, x, lastX) => {
                target.Name = x.ToString();
                namedObjectMap[target.Name] = target;
            });
        }
        void SetPanelChildren(Panel t, JObject obj, JObject lastObj)
        {
            SetFrameworkElementProperties(t, obj, lastObj);
            List<UIElement> children = new List<UIElement>();
            IJEnumerable<JToken> lastChildren = null;
            JToken last;
            if (lastObj != null && lastObj.TryGetValue("children", out last))
            {
                lastChildren = last.AsJEnumerable();
            }
            CollectPanelChildrenWorker(t, obj["children"].AsJEnumerable(), lastChildren, children);
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
        void CollectPanelChildrenWorker(Panel t, IJEnumerable<JToken> items, IEnumerable<JToken> lastItems, List<UIElement> children)
        {
            IEnumerator<JToken> enumerator = null;
            if (lastItems != null)
            {
                enumerator = lastItems.GetEnumerator();
                enumerator.Reset();
            }
            foreach (var child in items)
            {
                JToken lastChild = null;
                if (enumerator != null && enumerator.MoveNext()) { lastChild = enumerator.Current; }

                if (child.Type == JTokenType.Array)
                {
                    CollectPanelChildrenWorker(t, child.AsJEnumerable(), lastChild != null ? lastChild.AsJEnumerable() : null, children);
                }
                else
                {
                    var instance = CreateFromState((JObject)child, lastChild as JObject);
                    children.Add(instance);
                }
            }
        }
        void SetPanelProperties(Panel t, JObject obj, JObject lastObj)
        {
            SetFrameworkElementProperties(t, obj, lastObj);
            SetPanelChildren(t, obj, lastObj);
        }
        StackPanel CreateStackPanel(JObject obj, JObject lastObj)
        {
            StackPanel t = CreateOrGetLast<StackPanel>(obj);
            SetPanelProperties(t, obj, lastObj);
            return t;
        }
        delegate FrameworkElement CreateCallback(JObject obj, JObject lastObj);

        Dictionary<string, CreateCallback> handlers;
        Dictionary<string, CreateCallback> GetHandlers()
        {
            if (handlers == null)
            {
                handlers = new Dictionary<string, CreateCallback>();
                handlers["StackPanel"] = CreateStackPanel;
                handlers["TextBlock"] = CreateTextBlock ;
                handlers["TextBox"] = CreateTextBox;
                handlers["Button"] = CreateButton;
            }
            return handlers;
        }


        internal FrameworkElement CreateFromState(JObject item, JObject lastItem)
        {
            var type = item["type"].ToString();
            CreateCallback create;
            if (GetHandlers().TryGetValue(type, out create))
            {
                return create(item, lastItem);
            }
            return new TextBlock() { FontSize = 48, Text = "'"+type+"'Not found" };
        }
    }
}
