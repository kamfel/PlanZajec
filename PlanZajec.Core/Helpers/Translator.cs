using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanZajec.Core
{
    public static class Translator
    {
        /// <summary>
        /// Translates itemtype to polish equivalent
        /// </summary>
        /// <param name="itemtype"></param>
        /// <returns></returns>
        public static string TranslateToPolish(ScheduleItemType itemtype)
        {
            switch (itemtype)
            {
                case ScheduleItemType.Lecture:
                    return "Wykład";
                case ScheduleItemType.Laboratory:
                    return "Laboratorium";
                case ScheduleItemType.Class:
                    return "Ćwiczenia";
                case ScheduleItemType.Seminar:
                    return "Seminarium";
                case ScheduleItemType.Project:
                    return "Projekt";
                case ScheduleItemType.Other:
                    return "Inne";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Translates abbreviation of an itemtype (like wyk, lab) to a ScheduleItemType enum
        /// </summary>
        /// <param name="abbrev"></param>
        /// <returns></returns>
        public static ScheduleItemType TranslateAbbrevToItemType(string abbrev)
        {
            switch (abbrev)
            {
                case "ćw":
                    return ScheduleItemType.Class;
                case "lab":
                    return ScheduleItemType.Laboratory;
                case "proj":
                    return ScheduleItemType.Project;
                case "wyk":
                    return ScheduleItemType.Lecture;
                case "sem":
                    return ScheduleItemType.Seminar;
                default:
                    return ScheduleItemType.Other;
            }
        }

        /// <summary>
        /// Translates day to polish equivalent
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string TranslateToPolish(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday:
                    return "Niedziela";
                case DayOfWeek.Monday:
                    return "Poniedziałek";
                case DayOfWeek.Tuesday:
                    return "Wtorek";
                case DayOfWeek.Wednesday:
                    return "Środa";
                case DayOfWeek.Thursday:
                    return "Czwartek";
                case DayOfWeek.Friday:
                    return "Piątek";
                case DayOfWeek.Saturday:
                    return "Sobota";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Translates parity to polish equivalent
        /// </summary>
        /// <param name="parity"></param>
        /// <returns></returns>
        public static string TranslateToPolish(Parity parity)
        {
            switch (parity)
            {
                case Parity.Odd:
                    return "Nieparzyste";
                case Parity.Even:
                    return "Parzyste";
                case Parity.Both:
                    return "";
                default:
                    return "";
            }
        }
    }
}
