using PlanZajec.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace PlanZajec
{
    public class ScheduleItemTypeToColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts the type of the schedule item to a corresponding color
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ScheduleItemType)value)
            {
                case ScheduleItemType.Lecture:
                    return Brushes.LightGreen;
                case ScheduleItemType.Laboratory:
                    return Brushes.LightBlue;
                case ScheduleItemType.Class:
                    return Brushes.Turquoise;
                case ScheduleItemType.Seminar:
                    return Brushes.Orange;
                case ScheduleItemType.Project:
                    return Brushes.LightPink;
                case ScheduleItemType.Other:
                    return Brushes.Yellow;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
