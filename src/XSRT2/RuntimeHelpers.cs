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
        static async Task<string> GetResource(string resource)
        {
            string text;
            using (var stream = typeof(RuntimeHelpers).GetTypeInfo().Assembly.GetManifestResourceStream(resource))
            {
                using (var reader = new StreamReader(stream))
                {
                    text = await reader.ReadToEndAsync();
                }
            }
            return text;
        }

        public static IAsyncOperation<string> GetRuntimeJavaScript()
        {
            var t = GetResource("XSRT2.xsrt.js");
            return t.AsAsyncOperation<string>();
        }
    }
}
