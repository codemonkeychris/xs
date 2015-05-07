using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSRT2
{
    public sealed class RenderEventArgs
    {
        public object View { get; set; }
    }
    public sealed class CommandEventArgs
    {
        public string CommandHandlerToken { get; set; }
        public object Sender { get; set; }
        public object EventArgs { get; set; }
    }
    public sealed class StateManager
    {
        bool isDirty = true;
        Dictionary<string, string> state = new Dictionary<string, string>();

        public void NotifyChanged()
        {
            isDirty = true;
        }

        public void SetState(string key, string value)
        {
            state[key] = value;
            NotifyChanged();
        }
        public string GetState(string key, string defaultValue)
        {
            string value = defaultValue;
            state.TryGetValue(key, out value);
            return value;
        }

        public event EventHandler<RenderEventArgs> Render;
        public event EventHandler<CommandEventArgs> Command;

        public RenderEventArgs RenderIfNeeded()
        {
            RenderEventArgs e = null;
            if (isDirty)
            {
                if (Render != null)
                {
                    e = new RenderEventArgs();
                    Render(null, e);
                }
                isDirty = false;
            }
            return e;
        }

        public void NotifyCommand(CommandEventArgs e)
        {
            if (Command != null)
            {
                Command(this, e);
            }
        }
    }
}
