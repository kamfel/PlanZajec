using System;
using System.Collections.Generic;

namespace PlanZajec.Core
{
    /// <summary>
    /// Information about group, teacher or room
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Name of the group, teacher or room
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Id of the group on the webpage
        /// </summary>
        public int Id { get; set; }


        public bool HasSchedule { get; set; }

        /// <summary>
        /// The category of the item (groups, teachers, rooms)
        /// </summary>
        public ScheduleCategory Category { get; set; }
    }
}
