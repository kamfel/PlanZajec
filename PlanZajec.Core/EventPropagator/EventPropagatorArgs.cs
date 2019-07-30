using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.Core
{
    public class EventPropagatorArgs
    {
        public static EventPropagatorArgs None = new EventPropagatorArgs(null);

        public Dictionary<string, object> Data;

        public EventPropagatorArgs(Dictionary<string, object> args)
        {
            Data = args;
        }
    }
}
