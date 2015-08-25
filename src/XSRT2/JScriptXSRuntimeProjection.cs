using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

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
    public sealed class TestReadyEventArgs
    {
        public string Name { get; set; }
        public FrameworkElement Root { get; set; }
    }
    public sealed class TestSetupEventArgs
    {
        public string Name { get; set; }
    }
    public sealed class JScriptXSRuntimeProjection
    {
        private EventRegistrationTokenTable<EventHandler<RenderEventArgs>> render = new EventRegistrationTokenTable<EventHandler<RenderEventArgs>>();
        private EventRegistrationTokenTable<EventHandler<CommandEventArgs>> command = new EventRegistrationTokenTable<EventHandler<CommandEventArgs>>();
        private EventRegistrationTokenTable<EventHandler<TestReadyEventArgs>> testReady = new EventRegistrationTokenTable<EventHandler<TestReadyEventArgs>>();
        private EventRegistrationTokenTable<EventHandler<TestSetupEventArgs>> testSetup = new EventRegistrationTokenTable<EventHandler<TestSetupEventArgs>>();
        XSRuntime realHost;

        internal JScriptXSRuntimeProjection(XSRuntime realHost)
        {
            this.realHost = realHost;
        }

        public bool IsInitialized { get; set; }

        public void ReleaseEventHandlers()
        {
            render = new EventRegistrationTokenTable<EventHandler<RenderEventArgs>>();
            command = new EventRegistrationTokenTable<EventHandler<CommandEventArgs>>();
            testReady = new EventRegistrationTokenTable<EventHandler<TestReadyEventArgs>>();
            testSetup = new EventRegistrationTokenTable<EventHandler<TestSetupEventArgs>>();
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
        public event EventHandler<TestReadyEventArgs> TestReady
        {
            add { return testReady.AddEventHandler(value); }
            remove { testReady.RemoveEventHandler(value); }
        }
        public event EventHandler<TestSetupEventArgs> TestSetup
        {
            add { return testSetup.AddEventHandler(value); }
            remove { testSetup.RemoveEventHandler(value); }
        }

        // hacky helpers to work around JSProjection issues... 
        //
        public double GetRangeValue(object v) { return (((RangeBase)v).Value); }
        public bool? GetIsChecked(object v) { return ((ToggleButton)v).IsChecked; }
        public string GetText(object v)
        {
            string s;
            ((RichEditBox)v).Document.GetText(Windows.UI.Text.TextGetOptions.UseCrlf, out s);
            return s;
        }
        public object ParseJSONStringAsXS(string s)
        {
            Diff d = new Diff(null);
            var stats = d.Process(s);
            return d.LastGeneratedView;
        }

        public void RegisterTests([ReadOnlyArray] string[] names)
        {
            realHost.RegisterTests(names);
        }

        public void ForceCleanReload()
        {
            this.IsInitialized = false;
            realHost.ForceCleanReload();
        }
        public void Assert(bool condition, string message)
        {
            realHost.LogAssert(condition, message);
        }
        public LogEntry[] GetLogs()
        {
            var logs = realHost.GetLogs();
            return logs;
        }
        internal void RaiseTestSetup(string name)
        {
            if (testSetup.InvocationList != null)
            {
                testSetup.InvocationList(null, new TestSetupEventArgs() { Name = name });
            }
        }
        internal void RaiseTestReady(string name)
        { 
            if (testReady.InvocationList != null)
            {
                testReady.InvocationList(null, 
                    new TestReadyEventArgs() { Name = name, Root = realHost.LastGeneratedView as FrameworkElement });
            }
        }

        internal RenderEventArgs CallRender()
        {
            RenderEventArgs e = null;
            if (render.InvocationList != null)
            {
                e = new RenderEventArgs();
                render.InvocationList(null, e);
            }
            return e;
        }

        internal void CallCommand(CommandEventArgs e)
        {
            if (command.InvocationList != null)
            {
                command.InvocationList(this, e);
            }
        }
    }
}
