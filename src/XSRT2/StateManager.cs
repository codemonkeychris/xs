using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSRT2
{
    public sealed class StateManager
    {
        bool isDirty = true;

        public void NotifyChanged()
        {
            isDirty = true;
        }

        public event EventHandler<object> Render;

        public void RenderIfNeeded()
        {
            if (isDirty)
            {
                if (Render != null) { Render(null, null); }
                isDirty = false;
            }
        }
    }
}
