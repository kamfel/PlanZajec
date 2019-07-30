using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanZajec.Core
{
    public class TeachersScheduleItem : ScheduleItem
    {
        public TeachersScheduleItem()
        {
            Rooms = new List<string>();
            Groups = new List<string>();
        }

        public List<string> Rooms { get; set; }

        public List<string> Groups { get; set; }

        public override string ToString()
        {
            return 
                $"Nazwa: {Abbrev} - {Name}\n" +
                $"Typ: {Translator.TranslateToPolish(Type)}\n" +
                $"Dzień: {Translator.TranslateToPolish(Day)} {Translator.TranslateToPolish(Parity)}\n" +
                $"Czas trwania: {StartingTime.TimeOfDay} - {EndingTime.TimeOfDay}\n" +
                $"Sale: {string.Join(", ", Rooms)}\n" +
                $"Grupa: {string.Join(", ", Groups)}\n" +
                $"Dodatkowe infromacje: {AdditionalInfo}";
        }

        public override string ShortDesc()
        {
            return
                $"{Abbrev}\n" +
                $"{string.Join(", ", Groups)}\n" +
                $"{string.Join(", ", Rooms)}";
        }
    }
}
