using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanZajec.Core
{
    public class GroupsScheduleItem : ScheduleItem
    {
        public GroupsScheduleItem()
        {
            Teachers = new List<string>();
            Rooms = new List<string>();
        }

        public List<string> Teachers { get; set; }

        public List<string> Rooms { get; set; }

        public override string ToString()
        {
            return          
                $"Nazwa: {Abbrev} - {Name}\n" +
                $"Typ: {Translator.TranslateToPolish(Type)}\n" +
                $"Dzień: {Translator.TranslateToPolish(Day)} {Translator.TranslateToPolish(Parity)}\n" +
                $"Czas trwania: {string.Format("{0:hh}:{0:mm}-{1:hh}:{1:mm}",StartingTime.TimeOfDay, EndingTime.TimeOfDay)}\n" +
                $"Sale: {string.Join(", ", Rooms)}\n" +
                $"Nauczyciel: {string.Join(", ", Teachers)}\n" +
                $"Dodatkowe infromacje: {AdditionalInfo}";
        }

        public override string ShortDesc()
        {
            return
                $"{Abbrev}\n" +
                $"{string.Join(", ", Teachers)}\n" +
                $"{string.Join(", ", Rooms)}";
        }
    }
}
