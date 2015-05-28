using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        private EventRegistrationTokenTable<EventHandler<RenderEventArgs>> render = new EventRegistrationTokenTable<EventHandler<RenderEventArgs>>();
        private EventRegistrationTokenTable<EventHandler<CommandEventArgs>> command = new EventRegistrationTokenTable<EventHandler<CommandEventArgs>>();
        bool isDirty = true;

        public bool IsInitialized { get; set; }

        public void ReleaseEventHandlers()
        {
            render = new EventRegistrationTokenTable<EventHandler<RenderEventArgs>>();
            command = new EventRegistrationTokenTable<EventHandler<CommandEventArgs>>();
        }

        public void NotifyChanged()
        {
            isDirty = true;
        }

        public event EventHandler<RenderEventArgs> Render
        {
            add { return render.AddEventHandler(value); }
            remove { render.RemoveEventHandler(value); }
        }
        public event EventHandler<CommandEventArgs> Command
        {
            add { return command.AddEventHandler(value); }
            remove { command.RemoveEventHandler(value); }
        }


        public RenderEventArgs RenderIfNeeded()
        {
            RenderEventArgs e = null;
            if (isDirty)
            {
                if (render.InvocationList != null)
                {
                    e = new RenderEventArgs();
                    render.InvocationList(null, e);
                }
                isDirty = false;
            }
            return e;
        }

        public void NotifyCommand(CommandEventArgs e)
        {
            if (command.InvocationList != null)
            {
                command.InvocationList(this, e);
            }
        }
    }
}
