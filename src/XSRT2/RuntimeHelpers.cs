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

namespace XSRT2
{
    public static class RuntimeHelpers
    {
        // UNDONE: temporary location, should move to ClassHandlers once implementation
        // is baked.
        //
        internal static void SetItemsSource(ItemsControl control, JToken source)
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
