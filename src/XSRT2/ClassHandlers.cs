
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
using Windows.UI.Xaml.Media.Imaging;

namespace XSRT2 {
    public static class Handler
    {
        internal static class FrameworkElementHandler
        {
            internal static void SetProperties(FrameworkElement t, JObject obj, JObject lastObj, DiffContext context)
            {
                TrySet(context, obj, lastObj,
                    "scrollViewer$verticalScrollBarVisibility", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, XamlStringParse<ScrollBarVisibility>(valueToken)); });
                TrySet(context, obj, lastObj,
                    "scrollViewer$horizontalScrollBarVisibility", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, XamlStringParse<ScrollBarVisibility>(valueToken)); });
                TrySet(context, obj, lastObj,
                    "grid$row", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.SetValue(Grid.RowProperty, Convert.ToInt32(valueToken.Value<double>())); });
                TrySet(context, obj, lastObj,
                    "grid$rowSpan", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.SetValue(Grid.RowSpanProperty, Convert.ToInt32(valueToken.Value<double>())); });
                TrySet(context, obj, lastObj,
                    "grid$column", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.SetValue(Grid.ColumnProperty, Convert.ToInt32(valueToken.Value<double>())); });
                TrySet(context, obj, lastObj,
                    "grid$columnSpan", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.SetValue(Grid.ColumnSpanProperty, Convert.ToInt32(valueToken.Value<double>())); });
                TrySet(context, obj, lastObj,
                    "automationId", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { AutomationProperties.SetAutomationId(target, valueToken.ToString()); });
                TrySet(context, obj, lastObj,
                    "acc$helpText", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { AutomationProperties.SetHelpText(target, valueToken.ToString()); });
                TrySet(context, obj, lastObj,
                    "acc$labeledBy", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(AutomationProperties.LabeledByProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "acc$liveSetting", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(AutomationProperties.LiveSettingProperty, ParseEnum<AutomationLiveSetting>(valueToken)));
                TrySet(context, obj, lastObj,
                    "acc$name", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { AutomationProperties.SetName(target, valueToken.ToString()); });
                TrySet(context, obj, lastObj,
                    "relative$above", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AboveProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$alignBottomWith", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignBottomWithProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$alignBottomWithPanel", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignBottomWithPanelProperty, Convert.ToBoolean(Convert.ToBoolean(((JValue)valueToken).Value))));
                TrySet(context, obj, lastObj,
                    "relative$alignHorizontalCenterWith", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignHorizontalCenterWithProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$alignLeftWith", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignLeftWithProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$alignLeftWithPanel", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignLeftWithPanelProperty, Convert.ToBoolean(Convert.ToBoolean(((JValue)valueToken).Value))));
                TrySet(context, obj, lastObj,
                    "relative$alignRightWith", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignRightWithProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$alignRightWithPanel", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignRightWithPanelProperty, Convert.ToBoolean(Convert.ToBoolean(((JValue)valueToken).Value))));
                TrySet(context, obj, lastObj,
                    "relative$alignTopWith", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignTopWithProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$alignTopWithPanel", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignTopWithPanelProperty, Convert.ToBoolean(Convert.ToBoolean(((JValue)valueToken).Value))));
                TrySet(context, obj, lastObj,
                    "relative$alignVerticalCenterWith", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignVerticalCenterWithProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$alignVerticalCenterWithPanel", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.AlignVerticalCenterWithPanelProperty, Convert.ToBoolean(Convert.ToBoolean(((JValue)valueToken).Value))));
                TrySet(context, obj, lastObj,
                    "relative$below", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.BelowProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$leftOf", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.LeftOfProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "relative$rightOf", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SetValue(RelativePanel.RightOfProperty, context.ReferenceObject(valueToken.ToString())), context.Defer);
                TrySet(context, obj, lastObj,
                    "horizontalAlignment", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.HorizontalAlignment = ParseEnum<HorizontalAlignment>(valueToken));
                TrySet(context, obj, lastObj,
                    "verticalAlignment", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.VerticalAlignment = ParseEnum<VerticalAlignment>(valueToken));
                TrySet(context, obj, lastObj,
                    "margin", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Margin = XamlStringParse<Thickness>(valueToken));
                TrySet(context, obj, lastObj,
                    "name", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.Name = valueToken.ToString(); });
            }
        }

        internal static class TextBlockHandler
        {
            internal static TextBlock Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<TextBlock>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(TextBlock t, JObject obj, JObject lastObj, DiffContext context)
            {
                FrameworkElementHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "text", 
                    true,
                    t,
                    (target, valueToken, lastValueToken) => target.Text = valueToken.ToString());
                TrySet(context, obj, lastObj,
                    "fontFamily", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.FontFamily = new FontFamily(valueToken.ToString()));
                TrySet(context, obj, lastObj,
                    "fontSize", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.FontSize = valueToken.Value<double>());
                TrySet(context, obj, lastObj,
                    "fontWeight", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.FontWeight = XamlStringParse<FontWeight>(valueToken));
            }
        }

        internal static class ImageHandler
        {
            internal static Image Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<Image>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(Image t, JObject obj, JObject lastObj, DiffContext context)
            {
                FrameworkElementHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "source", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Source = new BitmapImage(FromRelativeUri(target, valueToken.ToString(), context)), context.Defer);
            }
        }
        
        internal static class RichEditBoxHandler
        {
            internal static RichEditBox Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<RichEditBox>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(RichEditBox t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "acceptsReturn", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.AcceptsReturn = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "text", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Document.SetText(TextSetOptions.None, valueToken.ToString()));
                TrySet(context, obj, lastObj,
                    "isColorFontEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsColorFontEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isReadOnly", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsReadOnly = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isSpellCheckEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsSpellCheckEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isTextPredictionEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsTextPredictionEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "placeholderText", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.PlaceholderText = valueToken.ToString());
                TrySet(context, obj, lastObj,
                    "preventKeyboardDisplayOnProgrammaticFocus", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.PreventKeyboardDisplayOnProgrammaticFocus = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "textAlignment", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TextAlignment = ParseEnum<TextAlignment>(valueToken));
                TrySet(context, obj, lastObj,
                    "textReadingOrder", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TextReadingOrder = ParseEnum<TextReadingOrder>(valueToken));
                TrySet(context, obj, lastObj,
                    "textWrapping", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TextWrapping = ParseEnum<TextWrapping>(valueToken));
                TrySetEvent(context, obj, lastObj, "TextChanged", t, (target, valueToken, lastValueToken) => SetTextChangedEventHandler(valueToken.ToString(), target));
            }
            static void TextChangedRouter(object sender, RoutedEventArgs e)
            {
                if (Command != null)
                {
                    var map = (Dictionary<string, string>)((FrameworkElement)sender).GetValue(eventMap);
                    Command(null, new CommandEventArgs() { CommandHandlerToken = map["TextChanged"], Sender = sender, EventArgs = e });
                }
            }
            static void SetTextChangedEventHandler(string handlerName, RichEditBox element)
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

        internal static class TextBoxHandler
        {
            internal static TextBox Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<TextBox>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(TextBox t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "acceptsReturn", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.AcceptsReturn = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "text", 
                    true,
                    t,
                    (target, valueToken, lastValueToken) => target.Text = valueToken.ToString());
                TrySet(context, obj, lastObj,
                    "isColorFontEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsColorFontEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isReadOnly", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsReadOnly = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isSpellCheckEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsSpellCheckEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isTextPredictionEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsTextPredictionEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "maxLength", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.MaxLength = valueToken.Value<int>());
                TrySet(context, obj, lastObj,
                    "placeholderText", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.PlaceholderText = valueToken.ToString());
                TrySet(context, obj, lastObj,
                    "preventKeyboardDisplayOnProgrammaticFocus", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.PreventKeyboardDisplayOnProgrammaticFocus = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "selectionLength", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SelectionLength = valueToken.Value<int>());
                TrySet(context, obj, lastObj,
                    "selectionStart", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.SelectionStart = valueToken.Value<int>());
                TrySet(context, obj, lastObj,
                    "textAlignment", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TextAlignment = ParseEnum<TextAlignment>(valueToken));
                TrySet(context, obj, lastObj,
                    "textReadingOrder", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TextReadingOrder = ParseEnum<TextReadingOrder>(valueToken));
                TrySet(context, obj, lastObj,
                    "textWrapping", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TextWrapping = ParseEnum<TextWrapping>(valueToken));
                TrySetEvent(context, obj, lastObj, "TextChanged", t, (target, valueToken, lastValueToken) => SetTextChangedEventHandler(valueToken.ToString(), target));
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
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
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
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
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
        
        internal static class ComboBoxHandler
        {
            internal static ComboBox Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<ComboBox>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(ComboBox t, JObject obj, JObject lastObj, DiffContext context)
            {
                SelectorHandler.SetProperties(t, obj, lastObj, context);
            }
        }
        
        internal static class ListBoxHandler
        {
            internal static ListBox Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<ListBox>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
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
                TrySet(context, obj, lastObj,
                    "selectedItem", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { target.SelectedItem = CreateForObjectType(valueToken, lastValueToken, context); });
                TrySetEvent(context, obj, lastObj, "SelectionChanged", t, (target, valueToken, lastValueToken) => SetSelectionChangedEventHandler(valueToken.ToString(), target));
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
                TrySet(context, obj, lastObj,
                    "itemsSource", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { SetItemsSource(target, valueToken, lastValueToken, context); });
                TrySet(context, obj, lastObj,
                    "itemContainerTransitions", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { SetItemContainerTransitions(target, valueToken, lastValueToken, context); });
            }
        }

        internal static class RangeBaseHandler
        {
            internal static void SetProperties(RangeBase t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "minimum", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Minimum = valueToken.Value<double>());
                TrySet(context, obj, lastObj,
                    "maximum", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Maximum = valueToken.Value<double>());
                TrySet(context, obj, lastObj,
                    "value", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Value = valueToken.Value<double>());
                TrySetEvent(context, obj, lastObj, "ValueChanged", t, (target, valueToken, lastValueToken) => SetValueChangedEventHandler(valueToken.ToString(), target));
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
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
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
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
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
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
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
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
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
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
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
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
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
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
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
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(CheckBox t, JObject obj, JObject lastObj, DiffContext context)
            {
                ButtonBaseHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "isChecked", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsChecked = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySetEvent(context, obj, lastObj, "Checked", t, (target, valueToken, lastValueToken) => SetCheckedEventHandler(valueToken.ToString(), target));
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

        internal static class ScrollViewerHandler
        {
            internal static ScrollViewer Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<ScrollViewer>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(ScrollViewer t, JObject obj, JObject lastObj, DiffContext context)
            {
                ContentControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "horizontalScrollBarVisibility", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.HorizontalScrollBarVisibility = XamlStringParse<ScrollBarVisibility>(valueToken));
                TrySet(context, obj, lastObj,
                    "horizontalScrollMode", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.HorizontalScrollMode = XamlStringParse<ScrollMode>(valueToken));
                TrySet(context, obj, lastObj,
                    "horizontalSnapPointsAlignment", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.HorizontalSnapPointsAlignment = XamlStringParse<SnapPointsAlignment>(valueToken));
                TrySet(context, obj, lastObj,
                    "horizontalSnapPointsType", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.HorizontalSnapPointsType = XamlStringParse<SnapPointsType>(valueToken));
                TrySet(context, obj, lastObj,
                    "isDeferredScrollingEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsDeferredScrollingEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isHorizontalRailEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsHorizontalRailEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isHorizontalScrollChainingEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsHorizontalScrollChainingEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isScrollInertiaEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsScrollInertiaEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isVerticalRailEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsVerticalRailEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isVerticalScrollChainingEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsVerticalScrollChainingEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isZoomChainingEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsZoomChainingEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "isZoomInertiaEnabled", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.IsZoomInertiaEnabled = Convert.ToBoolean(((JValue)valueToken).Value));
                TrySet(context, obj, lastObj,
                    "maxZoomFactor", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.MaxZoomFactor = valueToken.Value<float>());
                TrySet(context, obj, lastObj,
                    "minZoomFactor", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.MinZoomFactor = valueToken.Value<float>());
                TrySet(context, obj, lastObj,
                    "leftHeader", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.LeftHeader = CreateFromState(valueToken, lastValueToken, context) as UIElement);
                TrySet(context, obj, lastObj,
                    "topHeader", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TopHeader = CreateFromState(valueToken, lastValueToken, context) as UIElement);
                TrySet(context, obj, lastObj,
                    "topLeftHeader", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.TopLeftHeader = CreateFromState(valueToken, lastValueToken, context) as UIElement);
                TrySet(context, obj, lastObj,
                    "verticalScrollBarVisibility", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.VerticalScrollBarVisibility = XamlStringParse<ScrollBarVisibility>(valueToken));
                TrySet(context, obj, lastObj,
                    "verticalScrollMode", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.VerticalScrollMode = XamlStringParse<ScrollMode>(valueToken));
                TrySet(context, obj, lastObj,
                    "verticalSnapPointsAlignment", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.VerticalSnapPointsAlignment = XamlStringParse<SnapPointsAlignment>(valueToken));
                TrySet(context, obj, lastObj,
                    "verticalSnapPointsType", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.VerticalSnapPointsType = XamlStringParse<SnapPointsType>(valueToken));
                TrySet(context, obj, lastObj,
                    "zoomMode", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.ZoomMode = XamlStringParse<ZoomMode>(valueToken));
                TrySet(context, obj, lastObj,
                    "zoomSnapPointsType", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.ZoomSnapPointsType = XamlStringParse<SnapPointsType>(valueToken));
            }
        }

        internal static class ButtonBaseHandler
        {
            internal static void SetProperties(ButtonBase t, JObject obj, JObject lastObj, DiffContext context)
            {
                ContentControlHandler.SetProperties(t, obj, lastObj, context);
                TrySetEvent(context, obj, lastObj, "Click", t, (target, valueToken, lastValueToken) => SetClickEventHandler(valueToken.ToString(), target));
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
        
        internal static class ContentControlHandler
        {
            internal static void SetProperties(ContentControl t, JObject obj, JObject lastObj, DiffContext context)
            {
                ControlHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "content", 
                    true,
                    t,
                    (target, valueToken, lastValueToken) => target.Content = CreateFromState(valueToken, lastValueToken, context));
            }
        }

        internal static class ControlHandler
        {
            internal static void SetProperties(Control t, JObject obj, JObject lastObj, DiffContext context)
            {
                FrameworkElementHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "background", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Background = XamlStringParse<Brush>(valueToken));
                TrySet(context, obj, lastObj,
                    "foreground", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Foreground = XamlStringParse<Brush>(valueToken));
                TrySet(context, obj, lastObj,
                    "fontFamily", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.FontFamily = new FontFamily(valueToken.ToString()));
                TrySet(context, obj, lastObj,
                    "fontSize", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.FontSize = valueToken.Value<double>());
                TrySet(context, obj, lastObj,
                    "fontWeight", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.FontWeight = ParseEnum<FontWeight>(valueToken));
            }
        }

        internal static class StackPanelHandler
        {
            internal static StackPanel Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<StackPanel>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(StackPanel t, JObject obj, JObject lastObj, DiffContext context)
            {
                PanelHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "orientation", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => target.Orientation = ParseEnum<Orientation>(valueToken));
            }
        }

        internal static class GridHandler
        {
            internal static Grid Create(JObject obj, JObject lastObj, DiffContext context)
            {
                var createResult = CreateOrGetLast<Grid>(obj, context);
                context.PushName(createResult.Name);
                SetProperties(createResult.Value, obj, createResult.Recycled ? lastObj : null, context);
                context.PopName(createResult.Name);
                return createResult.Value;
            }
            internal static void SetProperties(Grid t, JObject obj, JObject lastObj, DiffContext context)
            {
                PanelHandler.SetProperties(t, obj, lastObj, context);
                TrySet(context, obj, lastObj,
                    "rows", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { PanelHandler.SetGridRowDefinitions(target, (JArray)valueToken); });
                TrySet(context, obj, lastObj,
                    "columns", 
                    false,
                    t,
                    (target, valueToken, lastValueToken) => { PanelHandler.SetGridColumnDefinitions(target, (JArray)valueToken); });
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

            static void Unparent(UIElement child)
            {
                DependencyObject visualParent = null;

                var fe = child as FrameworkElement;
                if (fe != null && fe.Parent != null)
                {
                    visualParent = fe.Parent;
                }
                if (visualParent == null)
                {
                    visualParent = VisualTreeHelper.GetParent(child);
                }
                var parentContent = visualParent as ContentControl;
                var parentPresenter = visualParent as ContentPresenter;
                var parentBorder = visualParent as Border;
                var parentPanel = visualParent as Panel;
                if (parentPanel != null)
                {
                    parentPanel.Children.Remove(child);
                }
                else if (parentContent != null)
                {
                    parentContent.Content = null;
                }
                else if (parentBorder != null)
                {
                    parentBorder.Child = null;
                }
                else if (parentPresenter != null)
                {
                    parentPresenter.Content = null;
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
                        Unparent(child);
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
                if(items != null)
                {
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
            }
            internal static void SetProperties(Panel t, JObject obj, JObject lastObj, DiffContext context)
            {
                FrameworkElementHandler.SetProperties(t, obj, lastObj, context);
                SetPanelChildren(t, obj, lastObj, context);
                TrySet(context, obj, lastObj, "background", false, t, (target, valueToken, lastValueToken) => target.Background = XamlStringParse<Brush>(valueToken));
                TrySet(context, obj, lastObj, "childrenTransitions", false, t, (target, valueToken, lastValueToken) => { SetChildrenTransitions(target, valueToken, lastValueToken, context); });
            }

        }


        static DependencyProperty eventMap = DependencyProperty.RegisterAttached("XSEventMap", typeof(Dictionary<string, string>), typeof(FrameworkElement), PropertyMetadata.Create((object)null));
        static Dictionary<string, CreateCallback> handlers;

        public static event EventHandler<CommandEventArgs> Command;


        delegate void SetCollectionPropertyCallback<TObject, TValue>(TObject target, List<TValue> items);
        internal static void SetItemContainerTransitions(ItemsControl control, JToken obj, JToken last, Handler.DiffContext context)
        {
            SetCollectionProperty<ItemsControl, Transition>(
                control,
                "itemContainerTransitions",
                obj,
                last,
                context,
                (target, list) =>
                {
                    target.ItemContainerTransitions.Clear();
                    foreach (var child in list)
                    {
                        target.ItemContainerTransitions.Add(child);
                    }
                });

        }
        internal static void SetChildrenTransitions(Panel control, JToken obj, JToken last, Handler.DiffContext context)
        {
            SetCollectionProperty<Panel, Transition>(
                control,
                "itemContainerTransitions",
                obj,
                last,
                context,
                (target, list) =>
                {
                    TransitionCollection col = target.ChildrenTransitions;
                    if (col == null)
                    {
                        target.ChildrenTransitions = col = new TransitionCollection();
                    }
                    else
                    {
                        col.Clear();
                    }
                    foreach (var child in list)
                    {
                        col.Add(child);
                    }
                });

        }

        static void SetCollectionProperty<TObject, TValue>(
            TObject t, 
            string propertyName,
            JToken obj,
            JToken lastObj,
            Handler.DiffContext context, 
            SetCollectionPropertyCallback<TObject, TValue> setter) where TValue : DependencyObject
        {
            List<TValue> children = new List<TValue>();
            IJEnumerable<JToken> lastChildren = null;
            if (lastObj != null)
            {
                lastChildren = lastObj.AsJEnumerable();
            }
            CollectItemsWorker(t, obj.AsJEnumerable(), lastChildren, children, context);
            // UNDONE: better diff
            //
            var setChildrenNeeded = true;

            if (setChildrenNeeded)
            {
                setter(t, children);
            }
        }
        static void CollectItemsWorker<TObject, TValue>(
            TObject t, 
            IJEnumerable<JToken> items, 
            IEnumerable<JToken> lastItems, 
            List<TValue> children,
            Handler.DiffContext context) where TValue : DependencyObject
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
                    CollectItemsWorker(t, child.AsJEnumerable(), lastChild != null ? lastChild.AsJEnumerable() : null, children, context);
                }
                else
                {
                    var instance = Handler.CreateFromState((JObject)child, lastChild as JObject, context);
                    children.Add((TValue)instance);
                }
            }
        }

        internal static object CreateForObjectType(JToken obj, JToken objLast, Handler.DiffContext context)
        {
            switch (obj.Type)
            {
                case JTokenType.Float:
                    return obj.Value<double>();
                case JTokenType.Integer:
                    return obj.Value<int>();
                case JTokenType.String:
                    return obj.Value<string>();
                case JTokenType.Object:
                    var instance = Handler.CreateFromState((JObject)obj, objLast as JObject, context);
                    return instance;
                default:
                    return "Unhandled:" + Enum.GetName(typeof(JTokenType), obj.Type);
            }
        }

        internal static void SetItemsSource(ItemsControl control, JToken source, JToken lastSource, Handler.DiffContext context)
        {
            // UNDONE: need to do delta on previous version of the list
            //
            List<object> collection = new List<object>();
            if (source.Type == JTokenType.Array)
            {
                foreach (var child in source.AsJEnumerable())
                {
                    collection.Add(CreateForObjectType(child, lastSource, context));
                }
            }

            control.ItemsSource = collection;
        }

        static CreateResult<T> CreateOrGetLast<T>(JObject obj, DiffContext context) where T:new()
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
                        return new CreateResult<T>() { Value = (T)value, Recycled = true, Name = resolvedName };
                    }
                }
            }
            context.ObjectCreateCount++;
            var instance = new T();
            context.AddObject(resolvedName, instance);
            return new CreateResult<T>() { Value = instance, Recycled = false, Name = resolvedName };
        }

        static void TrySet<T>(DiffContext context, JObject obj, JObject last, string name, T target, Setter<T> setter)
        {
            TrySet<T>(context, obj, last, name, false, target, setter);
        }
        static void TrySet<T>(DiffContext context, JObject obj, JObject last, string name, bool aliasFirstChild, T target, Setter<T> setter)
        {
            TrySet<T>(context, obj, last, name, aliasFirstChild, target, setter, null);
        }
        static void TrySet<T>(DiffContext context, JObject obj, JObject last, string name, bool aliasFirstChild, T target, Setter<T> setter, List<DeferSetter> defer)
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
                if (last != null)
                {
                    if (!last.TryGetValue(name, out tokLast))
                    {
                        if (aliasFirstChild && last.TryGetValue("children", out tokLast))
                        {
                            tokLast = ((JArray)tokLast).First;
                        }
                    }

                    if (tokLast != null && tokLast.ToString() == tok.ToString())
                    {
                        return; // bail early if old & new are the same
                    }
                }
                context.PropertySetCount++;
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
        static void TrySetEvent<T>(DiffContext context, JObject obj, JObject last, string name, T target, Setter<T> setter)
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
                context.EventSetCount++;
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
                handlers["Image"] = ImageHandler.Create;
                handlers["RichEditBox"] = RichEditBoxHandler.Create;
                handlers["TextBox"] = TextBoxHandler.Create;
                handlers["GridView"] = GridViewHandler.Create;
                handlers["ListView"] = ListViewHandler.Create;
                handlers["ComboBox"] = ComboBoxHandler.Create;
                handlers["ListBox"] = ListBoxHandler.Create;
                handlers["Button"] = ButtonHandler.Create;
                handlers["CalendarDatePicker"] = CalendarDatePickerHandler.Create;
                handlers["CalendarView"] = CalendarViewHandler.Create;
                handlers["RelativePanel"] = RelativePanelHandler.Create;
                handlers["RepositionThemeTransition"] = RepositionThemeTransitionHandler.Create;
                handlers["ProgressBar"] = ProgressBarHandler.Create;
                handlers["Slider"] = SliderHandler.Create;
                handlers["CheckBox"] = CheckBoxHandler.Create;
                handlers["ScrollViewer"] = ScrollViewerHandler.Create;
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
        internal static Uri FromRelativeUri(FrameworkElement relativeElement, string path, DiffContext context)
        {
            var r = relativeElement;
            var b = r.BaseUri;
            while (b == null)
            {
                r = VisualTreeHelper.GetParent(r) as FrameworkElement;
                if (r == null) { b = context.HostElement.BaseUri; break; }
                b = r.BaseUri;
            }
            if (b == null) { throw new InvalidOperationException("Can't determine base uri"); }
            var u = new Uri(b, path);
            return u;
        }

        internal delegate void Setter<T>(T target, JToken value, JToken lastValue);
        delegate DependencyObject CreateCallback(JObject obj, JObject lastObj, DiffContext context);

        struct CreateResult<T> 
        {
            public T Value;
            public bool Recycled;
            public string Name;
        }
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
            Dictionary<string, int> surrogateKeys = new Dictionary<string, int>();
            List<string> nameStack = new List<string>();

            internal DiffContext(Dictionary<string, object> lastNamedObjectMap)
            {
                this.lastNamedObjectMap = lastNamedObjectMap;
            }

            public List<DeferSetter> Defer;
            public int EventSetCount;
            public int PropertySetCount;
            public int ObjectCreateCount;
            public DateTime Start;
            public DateTime End;
            public FrameworkElement HostElement;
                
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

                var concat = nameStack.Count > 0 ? nameStack.Aggregate((a, b) => a + "-" + b) : "root";
                 
                return concat + "-" + key;
            }
            public void PushName(string name)
            {
                nameStack.Add(name);
            }
            public void PopName(string name)
            {
                var n = nameStack[nameStack.Count - 1];
                if (name != n) { throw new InvalidOperationException("unbalanced name"); }
                nameStack.RemoveAt(nameStack.Count - 1);
            }
        }
    }
}

