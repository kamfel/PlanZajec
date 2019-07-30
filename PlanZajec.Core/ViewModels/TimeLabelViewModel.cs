using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.Core
{
    public class TimeLabelViewModel : BaseViewModel
    {
        private int _RowIndex;

        public int RowIndex
        {
            get => _RowIndex;
            set
            {
                if (_RowIndex == value) return;
                _RowIndex = value;
                OnPropertyChanged("RowIndex");
            }
        }

        private DateTime _Start;

        public DateTime Start
        {
            get => _Start;
            set
            {
                if (_Start == value) return;
                _Start = value;
            }
        }

        private DateTime _End;

        public DateTime End
        {
            get => _End;
            set
            {
                if (_End == value) return;
                _End = value;
            }
        }

        public override string ToString()
        {
            return string.Format("{0:hh}:{0:mm}-{1:hh}:{1:mm}", Start.TimeOfDay, End.TimeOfDay);
        }

    }
}
