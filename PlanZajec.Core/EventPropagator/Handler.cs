using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.Core
{
    /// <summary>
    /// Holds a reference to registered object and handler
    /// </summary>
    public class Handler
    {
        /// <summary>
        /// Event that the handler handles
        /// </summary>
        public Type Event;

        /// <summary>
        /// Owner of the handler
        /// </summary>
        public WeakReference HandlerOwner;

        /// <summary>
        /// Handler of event
        /// </summary>
        public Action<EventPropagatorArgs> Method;
    }
}
