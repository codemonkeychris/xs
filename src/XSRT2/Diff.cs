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

        static T Creator<T>(JObject obj, IEnumerable<PropSetter<T>> props) where T:new()
        {
            T value = new T();
            foreach (var s in props) { s.Set(value, obj); }
            return value;
        }
        delegate FrameworkElement CreateCallback(JObject obj);

        static Dictionary<string, CreateCallback> handlers;
        static Dictionary<string, CreateCallback> GetHandlers()
        {
            if (handlers == null)
            {
                handlers = new Dictionary<string, CreateCallback>();
                handlers["TextBlock"] = (JObject obj) =>
                {
                    return Creator<TextBlock>(obj, new PropSetter<TextBlock>[] {
                        new TextBlockText()
                    });
                };
            }
            return handlers;
        }


        FrameworkElement CreateFromState(JObject item)
        {
            var type = item["type"].ToString();
            CreateCallback create;
            if (GetHandlers().TryGetValue(type, out create))
            {
                return create(item);
            }
            return new TextBlock() { FontSize = 48, Text = "Not found" };
        }
    }
    abstract class PropSetter<ObjectType>
    {
        public PropSetter() { }
        public abstract void Set(ObjectType target, JObject parentValue);
    }
    class TextBlockText : PropSetter<TextBlock>
    {
        public override void Set(TextBlock target, JObject parentValue)
        {
            target.Text = parentValue["Text"].ToString();
        }
    }
}
