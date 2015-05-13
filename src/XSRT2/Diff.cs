﻿using System;
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
        static DependencyProperty eventMap = DependencyProperty.RegisterAttached("XSEventMap", typeof(Dictionary<string, string>), typeof(FrameworkElement), PropertyMetadata.Create((object)null));
        static Dictionary<string, CreateCallback> handlers;
        ContentControl control;
        StateManager stateManager;
        JObject lastUI;
        Dictionary<string, object> namedObjectMap = new Dictionary<string, object>();

        public Diff(StateManager state, ContentControl control)
        {
            this.stateManager = state;
            this.control = control;
        }
        public static event EventHandler<CommandEventArgs> Command;
        public void Process(string ui)
        {
            var newUI = JObject.Parse(ui);
            control.Content = CreateFromState(newUI, lastUI, namedObjectMap);
            lastUI = newUI;
        }
        static Tuple<bool, T> CreateOrGetLast<T>(JObject obj, Dictionary<string, object> namedObjectMap) where T:new()
        {
            JToken name;
            if (obj.TryGetValue("name", out name))
            {
                object value;
                if (namedObjectMap.TryGetValue(name.ToString(), out value))
                {
                    if (value != null && value is T)
                    {
                        return new Tuple<bool, T>(true, (T)value);
                    }
                }
            }
            return new Tuple<bool, T>(false, new T());
        }
        static void SetTextBoxProperties(TextBox t, JObject obj, JObject lastObj, Dictionary<string, object> namedObjectMap)
        {
            ControlDiff.SetControlProperties(t, obj, lastObj, namedObjectMap);
            TrySet(obj, lastObj, "text", t, (target, x, lastX) => target.Text = x.ToString());
        }
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
        static void TrySetEvent<T>(JObject obj, JObject last, string name, T target, Setter<T> setter)
        {
            JToken tok;
            JToken tokLast = null;
            if (obj.TryGetValue("$" + name, out tok))
            {
                if (last != null && last.TryGetValue("$" + name, out tokLast))
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
        static Dictionary<string, CreateCallback> GetHandlers()
        {
            if (handlers == null)
            {
                handlers = new Dictionary<string, CreateCallback>();
                handlers["StackPanel"] = StackPanelDiff.CreateStackPanel;
                handlers["TextBlock"] = TextBlockDiff.CreateTextBlock ;
                handlers["TextBox"] = TextBoxDiff.CreateTextBox;
                handlers["Button"] = ButtonDiff.CreateButton;
            }
            return handlers;
        }
        static FrameworkElement CreateFromState(JObject item, JObject lastItem, Dictionary<string, object> namedObjectMap)
        {
            var type = item["type"].ToString();
            CreateCallback create;
            if (GetHandlers().TryGetValue(type, out create))
            {
                return create(item, lastItem, namedObjectMap);
            }
            return new TextBlock() { FontSize = 48, Text = "'"+type+"'Not found" };
        }

        delegate void Setter<T>(T target, JToken value, JToken lastValue);
        delegate void EventSetter<T>(T target, JToken value, JToken lastValue);
        delegate FrameworkElement CreateCallback(JObject obj, JObject lastObj, Dictionary<string, object> namedObjectMap);

        static class TextBlockDiff
        {
            internal static TextBlock CreateTextBlock(JObject obj, JObject lastObj, Dictionary<string, object> namedObjectMap)
            {
                var t = CreateOrGetLast<TextBlock>(obj, namedObjectMap);
                SetTextBlockProperties(t.Item2, obj, t.Item1 ? lastObj : null, namedObjectMap);
                return t.Item2;
            }
            static void SetTextBlockProperties(TextBlock t, JObject obj, JObject lastObj, Dictionary<string, object> namedObjectMap)
            {
                FrameworkElementDiff.SetFrameworkElementProperties(t, obj, lastObj, namedObjectMap);
                TrySet(obj, lastObj, "text", t, (target, x, lastX) => target.Text = x.ToString());
                TrySet(obj, lastObj, "fontFamily", t, (target, x, lastX) => target.FontFamily = new FontFamily(x.ToString()));
                TrySet(obj, lastObj, "fontSize", t, (target, x, lastX) => target.FontSize = x.Value<double>());
                TrySet(obj, lastObj, "fontWeight", t, (target, x, lastX) => target.FontWeight = ParseEnum<FontWeight>(x));
            }
        }
        static class FrameworkElementDiff
        {
            internal static void SetFrameworkElementProperties(FrameworkElement t, JObject obj, JObject lastObj, Dictionary<string, object> namedObjectMap)
            {
                TrySet(obj, lastObj, "horizontalAlignment", t, (target, x, lastX) => target.HorizontalAlignment = ParseEnum<HorizontalAlignment>(x));
                TrySet(obj, lastObj, "verticalAlignment", t, (target, x, lastX) => target.VerticalAlignment = ParseEnum<VerticalAlignment>(x));
                TrySet(obj, lastObj, "margin", t, (target, x, lastX) => target.Margin = XamlStringParse<Thickness>(x));
                TrySet(obj, lastObj, "name", t, (target, x, lastX) => {
                    target.Name = x.ToString();
                    namedObjectMap[target.Name] = target;
                });
            }
        }
        static class PanelDiff
        {
            static void SetPanelChildren(Panel t, JObject obj, JObject lastObj, Dictionary<string, object> namedObjectMap)
            {
                FrameworkElementDiff.SetFrameworkElementProperties(t, obj, lastObj, namedObjectMap);
                List<UIElement> children = new List<UIElement>();
                IJEnumerable<JToken> lastChildren = null;
                JToken last;
                if (lastObj != null && lastObj.TryGetValue("children", out last))
                {
                    lastChildren = last.AsJEnumerable();
                }
                CollectPanelChildrenWorker(t, obj["children"].AsJEnumerable(), lastChildren, children, namedObjectMap);
                var setChildrenNeeded = false;
                if (t.Children.Count == children.Count)
                {
                    for (int i = 0; i < children.Count; i++)
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
            static void CollectPanelChildrenWorker(Panel t, IJEnumerable<JToken> items, IEnumerable<JToken> lastItems, List<UIElement> children, Dictionary<string, object> namedObjectMap)
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
                        CollectPanelChildrenWorker(t, child.AsJEnumerable(), lastChild != null ? lastChild.AsJEnumerable() : null, children, namedObjectMap);
                    }
                    else
                    {
                        var instance = CreateFromState((JObject)child, lastChild as JObject, namedObjectMap);
                        children.Add(instance);
                    }
                }
            }
            internal static void SetPanelProperties(Panel t, JObject obj, JObject lastObj, Dictionary<string, object> namedObjectMap)
            {
                FrameworkElementDiff.SetFrameworkElementProperties(t, obj, lastObj, namedObjectMap);
                SetPanelChildren(t, obj, lastObj, namedObjectMap);
            }

        }
        static class StackPanelDiff
        {
            internal static StackPanel CreateStackPanel(JObject obj, JObject lastObj, Dictionary<string, object> namedObjectMap)
            {
                var t = CreateOrGetLast<StackPanel>(obj, namedObjectMap);
                PanelDiff.SetPanelProperties(t.Item2, obj, t.Item1 ? lastObj : null, namedObjectMap);
                return t.Item2;
            }
        }
        static class TextBoxDiff
        {
            internal static TextBox CreateTextBox(JObject obj, JObject lastObj, Dictionary<string, object> namedObjectMap)
            {
                var t = CreateOrGetLast<TextBox>(obj, namedObjectMap);
                SetTextBoxProperties(t.Item2, obj, t.Item1 ? lastObj : null, namedObjectMap);
                return t.Item2;
            }
        }
        static class ButtonDiff
        {
            internal static Button CreateButton(JObject obj, JObject lastObj, Dictionary<string, object> namedObjectMap)
            {
                var t = CreateOrGetLast<Button>(obj, namedObjectMap);
                SetButtonProperties(t.Item2, obj, t.Item1 ? lastObj : null, namedObjectMap);
                return t.Item2;
            }
            static void SetButtonProperties(Button t, JObject obj, JObject lastObj, Dictionary<string, object> namedObjectMap)
            {
                ControlDiff.SetControlProperties(t, obj, lastObj, namedObjectMap);
                TrySet(obj, lastObj, "content", t, (target, x, lastX) => target.Content = CreateFromState((JObject)x, (JObject)lastX, namedObjectMap));
                TrySetEvent(obj, lastObj, "click", t, (target, x, lastX) => SetClickEventHandler(x.ToString(), target));
            }
            static void ClickRouter(object sender, RoutedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["Click"], Sender = sender, EventArgs = e });
                }
            }

            static void SetClickEventHandler(string handlerName, ButtonBase element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["Click"] = handlerName;
                // remove to avoid duplicates   
                //
                element.Click -= ClickRouter;
                element.Click += ClickRouter;
            }
        }
        static class ControlDiff
        {
            internal static void SetControlProperties(Control t, JObject obj, JObject lastObj, Dictionary<string, object> namedObjectMap)
            {
                FrameworkElementDiff.SetFrameworkElementProperties(t, obj, lastObj, namedObjectMap);
                TrySet(obj, lastObj, "background", t, (target, x, lastX) => target.Background = XamlStringParse<Brush>(x));
                TrySet(obj, lastObj, "foreground", t, (target, x, lastX) => target.Foreground = XamlStringParse<Brush>(x));
                TrySet(obj, lastObj, "fontFamily", t, (target, x, lastX) => target.FontFamily = new FontFamily(x.ToString()));
                TrySet(obj, lastObj, "fontSize", t, (target, x, lastX) => target.FontSize = x.Value<double>());
                TrySet(obj, lastObj, "fontWeight", t, (target, x, lastX) => target.FontWeight = ParseEnum<FontWeight>(x));
            }
        }
    }
}
