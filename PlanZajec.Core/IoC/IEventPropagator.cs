using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.Core
{
    /// <summary>
    /// Simple interface that provides functionality for sending events to diffrent objects
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    public interface IEventPropagator<TArgs>
    {
        /// <summary>
        /// Registers a handler for event of type <typeparamref name="TEvent"/>
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="owner"></param>
        /// <param name="handler"></param>
        void Register<TEvent>(object owner, Action<TArgs> handler);

        /// <summary>
        /// Notifies that an event occured
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        void Publish<TEvent>(TArgs args);

    }
}
