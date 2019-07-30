using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.Core
{
    public class ApplicationViewModel : BaseViewModel
    {
        public static ApplicationViewModel Instance => IoC.ViewModelApplication;

        private int _CurrentId { get; set; }

        private Func<string> _GetDescription { get; set; } = () => "";

        public int CurrentId
        {
            get => _CurrentId;
            set
            {
                if (_CurrentId == value) return;
                _CurrentId = value;

                //Prepare data for handler
                var args = new EventPropagatorArgs(new Dictionary<string, object>(){
                    {"Category", CurrentCategory },
                    {"Id", CurrentId },
                    {"Semester", CurrentSemester }
                });

                //When id is changed raise event to change the schedule
                IoC.EventPropagator.Publish<LoadNewScheduleEvent>(args);
            }
        }

        private ScheduleCategory _CurrentCategory { get; set; } = ScheduleCategory.Groups;

        public ScheduleCategory CurrentCategory
        {
            get => _CurrentCategory;
            set
            {
                if ((int)_CurrentCategory - 1 == (int)value) return;
                _CurrentCategory = (ScheduleCategory)(value + 1);
            }
        } 


        public Semester CurrentSemester { get; set; } = Semester.Winter;

        public string CurrentTitle { get; set; } = "";

        public DateTime CurrentMinTime { get; set; }

        public Func<string> GetDescription
        {
            get => _GetDescription;
            set
            {
                if (_GetDescription == value) return;
                _GetDescription = value;
                OnPropertyChanged(nameof(CurrentDesc));
            }
        }

        public string CurrentDesc
        {
            get => _GetDescription();
        }
    }
}
