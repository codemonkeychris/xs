using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        FrameworkElement CreateFromState(JObject item)
        {
            switch (item["type"].ToString()) 
            {
                case "TextBlock":
                    return new TextBlock() { Text = item["Text"].ToString() };
            }
            return new TextBlock() { FontSize = 48, Text = "Not found" };
        }
    }
}
