
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;

namespace XSRT2 {
    public static class Handler
    {
        internal static class FrameworkElementHandler
        {
            internal static void SetProperties(FrameworkElement t, JObject obj, JObject lastObj, DiffContext context)
            {
                TrySet(obj, lastObj, "grid$row", false, t, (target, x, lastX) => { target.SetValue(Grid.RowProperty, Convert.ToInt32(x.Value<double>())); });
                TrySet(obj, lastObj, "grid$rowSpan", false, t, (target, x, lastX) => { target.SetValue(Grid.RowSpanProperty, Convert.ToInt32(x.Value<double>())); });
                TrySet(obj, lastObj, "grid$column", false, t, (target, x, lastX) => { target.SetValue(Grid.ColumnProperty, Convert.ToInt32(x.Value<double>())); });
                TrySet(obj, lastObj, "grid$columnSpan", false, t, (target, x, lastX) => { target.SetValue(Grid.ColumnSpanProperty, Convert.ToInt32(x.Value<double>())); });
                TrySet(obj, lastObj, "automationId", false, t, (target, x, lastX) => { AutomationProperties.SetAutomationId(target, x.ToString()); });
                TrySet(obj, lastObj, "acc$helpText", false, t, (target, x, lastX) => { AutomationProperties.SetHelpText(target, x.ToString()); });
                TrySet(obj, lastObj, "acc$labeledBy", false, t, (target, x, lastX) => target.SetValue(AutomationProperties.LabeledByProperty, context.ReferenceObject(x.ToString())), context.defer);
                TrySet(obj, lastObj, "acc$liveSetting", false, t, (target, x, lastX) => target.SetValue(AutomationProperties.LiveSettingProperty, ParseEnum<AutomationLiveSetting>(x)));
                TrySet(obj, lastObj, "acc$name", false, t, (target, x, lastX) => { AutomationProperties.SetName(target, x.ToString()); });
                TrySet(obj, lastObj, "relative$above", false, t, (target, x, lastX) => target.SetValue(RelativePanel.AboveProperty, context.ReferenceObject(x.ToString())), context.defer);
                TrySet(obj, lastObj, "relative$alignBottomWith", false, t, (target, x, lastX) => target.SetValue(RelativePanel.AlignBottomWithProperty, context.ReferenceObject(x.ToString())), context.defer);
                TrySet(obj, lastObj, "relative$alignBottomWithPanel", false, t, (target, x, lastX) => target.SetValue(RelativePanel.AlignBottomWithPanelProperty, Convert.ToBoolean(Convert.ToBoolean(((JValue)x).Value))));
                TrySet(obj, lastObj, "relative$alignHorizontalCenterWith", false, t, (target, x, lastX) => target.SetValue(RelativePanel.AlignHorizontalCenterWithProperty, context.ReferenceObject(x.ToString())), context.defer);
                TrySet(obj, lastObj, "relative$alignLeftWith", false, t, (target, x, lastX) => target.SetValue(RelativePanel.AlignLeftWithProperty, context.ReferenceObject(x.ToString())), context.defer);
                TrySet(obj, lastObj, "relative$alignLeftWithPanel", false, t, (target, x, lastX) => target.SetValue(RelativePanel.AlignLeftWithPanelProperty, Convert.ToBoolean(Convert.ToBoolean(((JValue)x).Value))));
                TrySet(obj, lastObj, "relative$alignRightWith", false, t, (target, x, lastX) => target.SetValue(RelativePanel.AlignRightWithProperty, context.ReferenceObject(x.ToString())), context.defer);
                TrySet(obj, lastObj, "relative$alignRightWithPanel", false, t, (target, x, lastX) => target.SetValue(RelativePanel.AlignRightWithPanelProperty, Convert.ToBoolean(Convert.ToBoolean(((JValue)x).Value))));
                TrySet(obj, lastObj, "relative$alignTopWith", false, t, (target, x, lastX) => target.SetValue(RelativePanel.AlignTopWithProperty, context.ReferenceObject(x.ToString())), context.defer);
                TrySet(obj, lastObj, "relative$alignTopWithPanel", false, t, (target, x, lastX) => target.SetValue(RelativePanel.AlignTopWithPanelProperty, Convert.ToBoolean(Convert.ToBoolean(((JValue)x).Value))));
                TrySet(obj, lastObj, "relative$alignVerticalCenterWith", false, t, (target, x, lastX) => target.SetValue(RelativePanel.AlignVerticalCenterWithProperty, context.ReferenceObject(x.ToString())), context.defer);
                TrySet(obj, lastObj, "relative$alignVerticalCenterWithPanel", false, t, (target, x, lastX) => target.SetValue(RelativePanel.AlignVerticalCenterWithPanelProperty, Convert.ToBoolean(Convert.ToBoolean(((JValue)x).Value))));
                TrySet(obj, lastObj, "relative$below", false, t, (target, x, lastX) => target.SetValue(RelativePanel.BelowProperty, context.ReferenceObject(x.ToString())), context.defer);
                TrySet(obj, lastObj, "relative$leftOf", false, t, (target, x, lastX) => target.SetValue(RelativePanel.LeftOfProperty, context.ReferenceObject(x.ToString())), context.defer);
                TrySet(obj, lastObj, "relative$rightOf", false, t, (target, x, lastX) => target.SetValue(RelativePanel.RightOfProperty, context.ReferenceObject(x.ToString())), context.defer);
                TrySet(obj, lastObj, "horizontalAlignment", false, t, (target, x, lastX) => target.HorizontalAlignment = ParseEnum<HorizontalAlignment>(x));
                TrySet(obj, lastObj, "verticalAlignment", false, t, (target, x, lastX) => target.VerticalAlignment = ParseEnum<VerticalAlignment>(x));
                TrySet(obj, lastObj, "margin", false, t, (target, x, lastX) => target.Margin = XamlStringParse<Thickness>(x));
                TrySet(obj, lastObj, "name", false, t, (target, x, lastX) => { target.Name = x.ToString(); });
            }
        }

        internal static class TextBlockHandler
        {
            internal static TextBlock Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<TextBlock>(obj, context);
                SetProperties(createResult.Item2, obj, createResult.Item1 ? lastObj : null, context);
                return createResult.Item2;
            }
            internal static void SetProperties(TextBlock t, JObject obj, JObject lastObj, DiffContext context)
            {
                FrameworkElementHandler.SetProperties(t, obj, lastObj, context);
                TrySet(obj, lastObj, "text", true, t, (target, x, lastX) => target.Text = x.ToString());
                TrySet(obj, lastObj, "fontFamily", false, t, (target, x, lastX) => target.FontFamily = new FontFamily(x.ToString()));
                TrySet(obj, lastObj, "fontSize", false, t, (target, x, lastX) => target.FontSize = x.Value<double>());
                TrySet(obj, lastObj, "fontWeight", false, t, (target, x, lastX) => target.FontWeight = XamlStringParse<FontWeight>(x));
            }
        }

        internal static class TextBoxHandler
        {
            internal static TextBox Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<TextBox>(obj, context);
                SetProperties(createResult.Item2, obj, createResult.Item1 ? lastObj : null, context);
                return createResult.Item2;
            }
            internal static void SetProperties(TextBox t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(obj, lastObj, "text", true, t, (target, x, lastX) => target.Text = x.ToString());
                TrySetEvent(obj, lastObj, "TextChanged", t, (target, x, lastX) => SetTextChangedEventHandler(x.ToString(), target));
            }
            static void TextChangedRouter(object sender, RoutedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["TextChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetTextChangedEventHandler(string handlerName, TextBox element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["TextChanged"] = handlerName;
                element.TextChanged -= TextChangedRouter;
                element.TextChanged += TextChangedRouter;
            }
        }

        internal static class GridViewHandler
        {
            internal static GridView Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<GridView>(obj, context);
                SetProperties(createResult.Item2, obj, createResult.Item1 ? lastObj : null, context);
                return createResult.Item2;
            }
            internal static void SetProperties(GridView t, JObject obj, JObject lastObj, DiffContext context)
            {
                ListViewBaseHandler.SetProperties(t, obj, lastObj, context);
            }
        }

        internal static class ListViewHandler
        {
            internal static ListView Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<ListView>(obj, context);
                SetProperties(createResult.Item2, obj, createResult.Item1 ? lastObj : null, context);
                return createResult.Item2;
            }
            internal static void SetProperties(ListView t, JObject obj, JObject lastObj, DiffContext context)
            {
                ListViewBaseHandler.SetProperties(t, obj, lastObj, context);
            }
        }

        internal static class ListViewBaseHandler
        {
            internal static void SetProperties(ListViewBase t, JObject obj, JObject lastObj, DiffContext context)
            {
                SelectorHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        
        internal static class ListBoxHandler
        {
            internal static ListBox Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<ListBox>(obj, context);
                SetProperties(createResult.Item2, obj, createResult.Item1 ? lastObj : null, context);
                return createResult.Item2;
            }
            internal static void SetProperties(ListBox t, JObject obj, JObject lastObj, DiffContext context)
            {
                SelectorHandler.SetProperties(t, obj, lastObj, context);
            }
        }

        internal static class SelectorHandler
        {
            internal static void SetProperties(Selector t, JObject obj, JObject lastObj, DiffContext context)
            {
                ItemsControlHandler.SetProperties(t, obj, lastObj, context);
                TrySetEvent(obj, lastObj, "SelectionChanged", t, (target, x, lastX) => SetSelectionChangedEventHandler(x.ToString(), target));
            }
            static void SelectionChangedRouter(object sender, SelectionChangedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["SelectionChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetSelectionChangedEventHandler(string handlerName, Selector element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["SelectionChanged"] = handlerName;
                element.SelectionChanged -= SelectionChangedRouter;
                element.SelectionChanged += SelectionChangedRouter;
            }
        }

        internal static class ItemsControlHandler
        {
            internal static void SetProperties(ItemsControl t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(obj, lastObj, "itemsSource", false, t, (target, x, lastX) => { RuntimeHelpers.SetItemsSource(target, x, lastX, context); });
                TrySet(obj, lastObj, "itemContainerTransitions", false, t, (target, x, lastX) => { RuntimeHelpers.SetItemContainerTransitions(target, x, lastX, context); });
            }
        }

        internal static class RangeBaseHandler
        {
            internal static void SetProperties(RangeBase t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(obj, lastObj, "minimum", false, t, (target, x, lastX) => target.Minimum = x.Value<double>());
                TrySet(obj, lastObj, "maximum", false, t, (target, x, lastX) => target.Maximum = x.Value<double>());
                TrySet(obj, lastObj, "value", false, t, (target, x, lastX) => target.Value = x.Value<double>());
                TrySetEvent(obj, lastObj, "ValueChanged", t, (target, x, lastX) => SetValueChangedEventHandler(x.ToString(), target));
            }
            static void ValueChangedRouter(object sender, RangeBaseValueChangedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["ValueChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetValueChangedEventHandler(string handlerName, RangeBase element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["ValueChanged"] = handlerName;
                element.ValueChanged -= ValueChangedRouter;
                element.ValueChanged += ValueChangedRouter;
            }
        }

        // UNDONE: Content property (and others) now will recreate when child props change instead of incremental
        // update now that we drop "lastNamedObjectMap" on the floor and track references... 
        // 
        internal static class ButtonHandler
        {
            internal static Button Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<Button>(obj, context);
                SetProperties(createResult.Item2, obj, createResult.Item1 ? lastObj : null, context);
                return createResult.Item2;
            }
            internal static void SetProperties(Button t, JObject obj, JObject lastObj, DiffContext context)
            {
                ButtonBaseHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        internal static class CalendarDatePickerHandler
        {
            internal static CalendarDatePicker Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<CalendarDatePicker>(obj, context);
                SetProperties(createResult.Item2, obj, createResult.Item1 ? lastObj : null, context);
                return createResult.Item2;
            }
            internal static void SetProperties(CalendarDatePicker t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        internal static class CalendarViewHandler
        {
            internal static CalendarView Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<CalendarView>(obj, context);
                SetProperties(createResult.Item2, obj, createResult.Item1 ? lastObj : null, context);
                return createResult.Item2;
            }
            internal static void SetProperties(CalendarView t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        internal static class RelativePanelHandler
        {
            internal static RelativePanel Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<RelativePanel>(obj, context);
                SetProperties(createResult.Item2, obj, createResult.Item1 ? lastObj : null, context);
                return createResult.Item2;
            }
            internal static void SetProperties(RelativePanel t, JObject obj, JObject lastObj, DiffContext context)
            {
                PanelHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        internal static class RepositionThemeTransitionHandler
        {
            internal static RepositionThemeTransition Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<RepositionThemeTransition>(obj, context);
                SetProperties(createResult.Item2, obj, createResult.Item1 ? lastObj : null, context);
                return createResult.Item2;
            }
            internal static void SetProperties(RepositionThemeTransition t, JObject obj, JObject lastObj, DiffContext context)
            {
            }
        }
        internal static class ProgressBarHandler
        {
            internal static ProgressBar Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<ProgressBar>(obj, context);
                SetProperties(createResult.Item2, obj, createResult.Item1 ? lastObj : null, context);
                return createResult.Item2;
            }
            internal static void SetProperties(ProgressBar t, JObject obj, JObject lastObj, DiffContext context)
            {
                RangeBaseHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        internal static class SliderHandler
        {
            internal static Slider Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<Slider>(obj, context);
                SetProperties(createResult.Item2, obj, createResult.Item1 ? lastObj : null, context);
                return createResult.Item2;
            }
            internal static void SetProperties(Slider t, JObject obj, JObject lastObj, DiffContext context)
            {
                RangeBaseHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        

        internal static class CheckBoxHandler
        {
            internal static CheckBox Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<CheckBox>(obj, context);
                SetProperties(createResult.Item2, obj, createResult.Item1 ? lastObj : null, context);
                return createResult.Item2;
            }
            internal static void SetProperties(CheckBox t, JObject obj, JObject lastObj, DiffContext context)
            {
                ButtonBaseHandler.SetProperties(t, obj, lastObj, context);
                TrySet(obj, lastObj, "isChecked", false, t, (target, x, lastX) => target.IsChecked = Convert.ToBoolean(((JValue)x).Value));
                TrySetEvent(obj, lastObj, "Checked", t, (target, x, lastX) => SetCheckedEventHandler(x.ToString(), target));
            }
            static void CheckedRouter(object sender, RoutedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["Checked"], Sender = sender, EventArgs = e });
                }
            }
            static void SetCheckedEventHandler(string handlerName, CheckBox element)
            {
                var map = (Dictionary<string, string>)element.GetValue(eventMap);
                if (map == null)
                {
                    element.SetValue(eventMap, map = new Dictionary<string, string>());
                }
                map["Checked"] = handlerName;
                element.Checked -= CheckedRouter;
                element.Checked += CheckedRouter;
            }
        }

        internal static class ButtonBaseHandler
        {
            internal static void SetProperties(ButtonBase t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(obj, lastObj, "content", true, t, (target, x, lastX) => target.Content = CreateFromState(x, lastX, context));
                TrySetEvent(obj, lastObj, "Click", t, (target, x, lastX) => SetClickEventHandler(x.ToString(), target));
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
                element.Click -= ClickRouter;
                element.Click += ClickRouter;
            }
        }

        internal static class ControlHandler
        {
            internal static void SetProperties(Control t, JObject obj, JObject lastObj, DiffContext context)
            {
                FrameworkElementHandler.SetProperties(t, obj, lastObj, context);
                TrySet(obj, lastObj, "background", false, t, (target, x, lastX) => target.Background = XamlStringParse<Brush>(x));
                TrySet(obj, lastObj, "foreground", false, t, (target, x, lastX) => target.Foreground = XamlStringParse<Brush>(x));
                TrySet(obj, lastObj, "fontFamily", false, t, (target, x, lastX) => target.FontFamily = new FontFamily(x.ToString()));
                TrySet(obj, lastObj, "fontSize", false, t, (target, x, lastX) => target.FontSize = x.Value<double>());
                TrySet(obj, lastObj, "fontWeight", false, t, (target, x, lastX) => target.FontWeight = ParseEnum<FontWeight>(x));
            }
        }

        internal static class StackPanelHandler
        {
            internal static StackPanel Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<StackPanel>(obj, context);
                SetProperties(createResult.Item2, obj, createResult.Item1 ? lastObj : null, context);
                return createResult.Item2;
            }
            internal static void SetProperties(StackPanel t, JObject obj, JObject lastObj, DiffContext context)
            {
                PanelHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        internal static class GridHandler
        {
            internal static Grid Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<Grid>(obj, context);
                SetProperties(createResult.Item2, obj, createResult.Item1 ? lastObj : null, context);
                return createResult.Item2;
            }
            internal static void SetProperties(Grid t, JObject obj, JObject lastObj, DiffContext context)
            {
                PanelHandler.SetProperties(t, obj, lastObj, context);
                TrySet(obj, lastObj, "rows", false, t, (target, x, lastX) => { PanelHandler.SetGridRowDefinitions(target, (JArray)x); });
                TrySet(obj, lastObj, "columns", false, t, (target, x, lastX) => { PanelHandler.SetGridColumnDefinitions(target, (JArray)x); });
            }
        }

        internal static class PanelHandler
        {
            internal static void SetGridRowDefinitions(Grid t, JArray obj) 
            {
                t.RowDefinitions.Clear();
                foreach (var d in obj.AsJEnumerable())
                {
                    RowDefinition rd = new RowDefinition();
                    rd.Height = XamlStringParse<GridLength>(d);
                    t.RowDefinitions.Add(rd);
                }
            }
            internal static void SetGridColumnDefinitions(Grid t, JArray obj) 
            {
                t.ColumnDefinitions.Clear();
                foreach (var d in obj.AsJEnumerable())
                {
                    ColumnDefinition cd = new ColumnDefinition();
                    cd.Width = XamlStringParse<GridLength>(d);
                    t.ColumnDefinitions.Add(cd);
                }
            }

            static void SetPanelChildren(Panel t, JObject obj, JObject lastObj, DiffContext context)
            {
                Handler.FrameworkElementHandler.SetProperties(t, obj, lastObj, context);
                List<UIElement> children = new List<UIElement>();
                IJEnumerable<JToken> lastChildren = null;
                JToken last;
                if (lastObj != null && lastObj.TryGetValue("children", out last))
                {
                    lastChildren = last.AsJEnumerable();
                }
                CollectPanelChildrenWorker(t, obj["children"].AsJEnumerable(), lastChildren, children, context);
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
                    foreach (var child in children)
                    {
                        var parent = VisualTreeHelper.GetParent(child) as Panel;
                        if (parent != null)
                        {
                            parent.Children.Remove(child);
                        }
                        t.Children.Add(child);
                    }
                }
            }
            static void CollectPanelChildrenWorker(Panel t, IJEnumerable<JToken> items, IEnumerable<JToken> lastItems, List<UIElement> children, DiffContext context)
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
                        CollectPanelChildrenWorker(t, child.AsJEnumerable(), lastChild != null ? lastChild.AsJEnumerable() : null, children, context);
                    }
                    else
                    {
                        var instance = CreateFromState((JObject)child, lastChild as JObject, context);
                        children.Add((FrameworkElement)instance);
                    }
                }
            }
            internal static void SetProperties(Panel t, JObject obj, JObject lastObj, DiffContext context)
            {
                FrameworkElementHandler.SetProperties(t, obj, lastObj, context);
                SetPanelChildren(t, obj, lastObj, context);
                TrySet(obj, lastObj, "childrenTransitions", false, t, (target, x, lastX) => { RuntimeHelpers.SetChildrenTransitions(target, x, lastX, context); });
            }

        }


        static DependencyProperty eventMap = DependencyProperty.RegisterAttached("XSEventMap", typeof(Dictionary<string, string>), typeof(FrameworkElement), PropertyMetadata.Create((object)null));
        static Dictionary<string, CreateCallback> handlers;

        public static event EventHandler<CommandEventArgs> Command;

        static Tuple<bool, T> CreateOrGetLast<T>(JObject obj, DiffContext context) where T:new()
        {
            JToken name;
            string resolvedName = null;
            if (obj.TryGetValue("name", out name))
            {
                resolvedName = name.ToString();
            }
            else
            {
                resolvedName = context.CreateSurrogateName(obj);
            }

            if (resolvedName != null)
            { 
                object value;
                if (context.TryGetObject(resolvedName, out value))
                {
                    if (value != null && value is T)
                    {
                        return new Tuple<bool, T>(true, (T)value);
                    }
                }
            }
            var instance = new T();
            context.AddObject(resolvedName, instance);
            return new Tuple<bool, T>(false, instance);
        }

        static void TrySet<T>(JObject obj, JObject last, string name, T target, Setter<T> setter)
        {
            TrySet<T>(obj, last, name, false, target, setter);
        }
        static void TrySet<T>(JObject obj, JObject last, string name, bool aliasFirstChild, T target, Setter<T> setter)
        {
            TrySet<T>(obj, last, name, aliasFirstChild, target, setter, null);
        }
        static void TrySet<T>(JObject obj, JObject last, string name, bool aliasFirstChild, T target, Setter<T> setter, List<DeferSetter> defer)
        {
            JToken tok;
            JToken tokLast = null;
            bool found = false;
            if (!obj.TryGetValue(name, out tok))
            {
                if (aliasFirstChild && obj.TryGetValue("children", out tok))
                {
                    found = true;
                    tok = ((JArray)tok).First;
                }
            }
            else
            {
                found = true;
            }
            if (found)
            {
                if (last != null && last.TryGetValue(name, out tokLast))
                {
                    if (tokLast.ToString() == tok.ToString())
                    {
                        return; // bail early if old & new are the same
                    }
                }
                if (defer != null) 
                {
                    defer.Add(new DeferSetter<T>() { setter = setter, target = target, tok = tok, tokLast = tokLast });
                }
                else 
                {
                    setter(target, tok, tokLast);
                }
            }
        }
        static void TrySetEvent<T>(JObject obj, JObject last, string name, T target, Setter<T> setter)
        {
            JToken tok;
            JToken tokLast = null;
            if (obj.TryGetValue("on" + name, out tok))
            {
                if (last != null && last.TryGetValue("on" + name, out tokLast))
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
                handlers["TextBlock"] = TextBlockHandler.Create;
                handlers["TextBox"] = TextBoxHandler.Create;
                handlers["GridView"] = GridViewHandler.Create;
                handlers["ListView"] = ListViewHandler.Create;
                handlers["ListBox"] = ListBoxHandler.Create;
                handlers["Button"] = ButtonHandler.Create;
                handlers["CalendarDatePicker"] = CalendarDatePickerHandler.Create;
                handlers["CalendarView"] = CalendarViewHandler.Create;
                handlers["RelativePanel"] = RelativePanelHandler.Create;
                handlers["RepositionThemeTransition"] = RepositionThemeTransitionHandler.Create;
                handlers["ProgressBar"] = ProgressBarHandler.Create;
                handlers["Slider"] = SliderHandler.Create;
                handlers["CheckBox"] = CheckBoxHandler.Create;
                handlers["StackPanel"] = StackPanelHandler.Create;
                handlers["Grid"] = GridHandler.Create;
            }
            return handlers;
        }
        internal static DependencyObject CreateFromState(JToken item, JToken lastItem, DiffContext context)
        {
            if (item.Type == JTokenType.Object)
            {
                var type = item["type"].ToString();
                CreateCallback create;
                if (GetHandlers().TryGetValue(type, out create))
                {
                    return create((JObject)item, (JObject)lastItem, context);
                }
                return new TextBlock() { FontSize = 48, Text = "'" + type + "'Not found" };
            }
            else
            {
                return new TextBlock() { Text = item.ToString() };
            }
        }

        internal delegate void Setter<T>(T target, JToken value, JToken lastValue);
        delegate DependencyObject CreateCallback(JObject obj, JObject lastObj, DiffContext context);

        internal abstract class DeferSetter
        {
            public abstract void Do();
        }
        internal class DeferSetter<T> : DeferSetter
        {
            internal T target;
            internal JToken tok;
            internal JToken tokLast;
            internal Setter<T> setter;

            public override void Do()
            {
                setter(target, tok, tokLast);
            }
        }
        internal class DiffContext 
        {
            Dictionary<string, object> lastNamedObjectMap;
            Dictionary<string, object> currentNamedObjectMap = new Dictionary<string, object>();
            // UNDONE: get smarter about surrogate key generation
            Dictionary<string, int> surrogateKeys = new Dictionary<string, int>();

            internal DiffContext(Dictionary<string, object> lastNamedObjectMap)
            {
                this.lastNamedObjectMap = lastNamedObjectMap;
            }

            public List<DeferSetter> defer;
            
            public Dictionary<String, object> GetNamedObjectMap() { return currentNamedObjectMap; }

            public object ReferenceObject(string name) 
            {
                object value;
                if (lastNamedObjectMap.TryGetValue(name, out value))
                {
                    currentNamedObjectMap[name] = value;
                    return value;
                }
                return null;
            }
            public bool TryGetObject(string name, out object value)
            {
                var ret = lastNamedObjectMap.TryGetValue(name, out value);
                if (ret) { currentNamedObjectMap[name] = value; }
                return ret;
            }
            public void AddObject(string name, object value)
            {
                lastNamedObjectMap[name] = currentNamedObjectMap[name] = value;
            }
            public string CreateSurrogateName(JObject obj) 
            { 
                string baseName = "xsrt";
                JToken t;
                if (obj.TryGetValue("type", out t))
                {
                    baseName = t.ToString();
                }
                int n = 0;
                if (surrogateKeys.TryGetValue(baseName, out n)) 
                {
                    n++;
                }
                surrogateKeys[baseName] = n;
                string key = baseName + n; 
                return key;
            }
        }
    }
}

