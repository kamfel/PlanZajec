using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanZajec.Core
{
    public class RoomsScheduleItem : ScheduleItem
    {
        public RoomsScheduleItem()
        {
            Groups = new List<string>();
            Teachers = new List<string>();
        }
        public List<string> Groups { get; set; }

        public List<string> Teachers { get; set; }

        public override string ToString()
        {
            return
                $"Nazwa: {Abbrev} - {Name}\n" +
                $"Typ: {Translator.TranslateToPolish(Type)}\n" +
                $"Dzień: {Translator.TranslateToPolish(Day)} {Translator.TranslateToPolish(Parity)}\n" +
                $"Czas trwania: {StartingTime.TimeOfDay} - {EndingTime.TimeOfDay}\n" +
                $"Grupa: {string.Join(", ", Groups)}\n" +
                $"Nauczyciele: {string.Join(", ", Teachers)}\n" +
                $"Dodatkowe infromacje: {AdditionalInfo}";
        }

        public override string ShortDesc()
        {
            return
                $"{Abbrev}\n" +
                $"{string.Join(", ", Teachers)}\n" +
                $"{string.Join(", ", Groups)}";
        }
    }
}
