using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using System.Reflection;
using System.IO;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace XSRT2
{
    public static class RuntimeHelpers
    {
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


        // UNDONE: temporary location, should move to ClassHandlers once implementation
        // is baked.
        //
        internal static void SetItemsSource(ItemsControl control, JToken source, JToken lastSource, Handler.DiffContext context)
        {
            // UNDONE: need to do delta on previous version of the list
            //
            List<object> collection = new List<object>();
            if (source.Type == JTokenType.Array)
            {
                foreach (var child in source.AsJEnumerable())
                {
                    switch (child.Type)
                    {
                        case JTokenType.Float:
                            collection.Add(child.Value<double>());
                            break;
                        case JTokenType.Integer:
                            collection.Add(child.Value<int>());
                            break;
                        case JTokenType.String:
                            collection.Add(child.Value<string>());
                            break;
                        case JTokenType.Object:
                            var instance = Handler.CreateFromState((JObject)child, lastSource as JObject, context);
                            collection.Add(instance);
                            break;
                        default:
                            collection.Add("Unhandled:" + Enum.GetName(typeof(JTokenType), child.Type));
                            break;
                    }
                }
            }

            control.ItemsSource = collection;
        }

        static async Task<string> GetResourceImpl(TypeInfo containingType, string resource)
        {
            string text;
            using (var stream = containingType.Assembly.GetManifestResourceStream(resource))
            {
                using (var reader = new StreamReader(stream))
                {
                    text = await reader.ReadToEndAsync();
                }
            }
            return text;
        }
        public static IAsyncOperation<string> GetResource(Type containingType, string resource)
        {
            var t = GetResourceImpl(containingType.GetTypeInfo(), resource);
            return t.AsAsyncOperation<string>();
        }
        public static IAsyncOperation<string> GetRuntimeJavaScript()
        {
            return GetResource(typeof(RuntimeHelpers), "XSRT2.xsrt.js");
        }
    }
}
