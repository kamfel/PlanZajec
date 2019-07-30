using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.Core
{
    public class EventPropagator : IEventPropagator<EventPropagatorArgs>
    {
        private List<Handler> _handlers = new List<Handler>();

        public void Publish<TEvent>(EventPropagatorArgs args)
        {
            //Propagate events to registered handlers
            foreach (var handler in _handlers)
            {
                //Check if handler handles this type of event
                if (handler.Event != typeof(TEvent)) continue;

                //Check if owner of handler exists
                if (handler.HandlerOwner.IsAlive == false)
                {
                    //If not remove handler
                    _handlers.Remove(handler);
                    continue;
                }

                //Invoke handler
                handler.Method.Invoke(args);
            }
        }

        public void Register<TEvent>(object owner, Action<EventPropagatorArgs> handler)
        {
            Handler h = new Handler()
            {
                Event = typeof(TEvent),
                HandlerOwner = new WeakReference(owner, false),
                Method = handler
            };

            _handlers.Add(h);
        }
    }
}
