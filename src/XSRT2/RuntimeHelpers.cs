using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using System.Reflection;
using System.IO;

namespace XSRT2
{
    public static class RuntimeHelpers
    {
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
