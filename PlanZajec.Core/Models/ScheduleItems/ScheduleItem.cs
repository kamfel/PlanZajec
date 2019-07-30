using System;

namespace PlanZajec.Core
{
    /// <summary>
    /// Class containing data about a single item (lecture, class) in the schedule
    /// </summary>
    public abstract class ScheduleItem
    {
        /// <summary>
        /// The type of the item
        /// </summary>
        public ScheduleItemType Type { get; set; }

        /// <summary>
        /// Name of the item (lecture, class etc.)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Abbreviation of the name
        /// </summary>
        public string Abbrev { get; set; }

        /// <summary>
        /// Day which the item takes place on
        /// </summary>
        public DayOfWeek Day { get; set; }

        /// <summary>
        /// Which weeks does the item occur
        /// </summary>
        public Parity Parity { get; set; }

        /// <summary>
        /// Starting time of the item
        /// </summary>
        public DateTime StartingTime { get; set; }

        /// <summary>
        /// Time when the item ends
        /// </summary>
        public DateTime EndingTime { get; set; }

        /// <summary>
        /// Additional info about the item
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Returns short description of the scheduleitem
        /// </summary>
        /// <returns></returns>
        public abstract string ShortDesc();
    }
}
