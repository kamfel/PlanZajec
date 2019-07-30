using PlanZajec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PlanZajec
{
    public class ScheduleItemViewModel : BaseViewModel
    {
        /// <summary>
        /// Item 
        /// </summary>
        private ScheduleItem _ScheduleItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ScheduleItem ScheduleItem
        {
            get
            {
                return _ScheduleItem;
            }
            set
            {
                if (_ScheduleItem == value) return;
                _ScheduleItem = value;
                OnPropertyChanged("ScheduleItem");
            }
        }

        public string Short
        {
            get
            {
                return ScheduleItem.ShortDesc();
            }
        }

        public ScheduleItemType Type
        {
            get
            {
                return ScheduleItem.Type;
            }
        }

        public int Column
        {
            get
            {
                if (ScheduleItem.Parity == Parity.Odd)
                    return (((int)ScheduleItem.Day - 1) % 7) * 2 + 1;
                else
                    return (((int)ScheduleItem.Day - 1) % 7) * 2;
            }
        }

        public int ColumnSpan
        {
            get
            {
                if (ScheduleItem.Parity == Parity.Both)
                    return 2;
                else
                    return 1;
            }
        }

        public int Row
        {
            get
            {
                var time = ScheduleItem.StartingTime;
                var min_time = IoC.ViewModelApplication.CurrentMinTime;
                return Convert.ToInt32((time - min_time).TotalMinutes / 15);
            }
        }

        public int RowSpan
        {
            get
            {
                var time = ScheduleItem.EndingTime - ScheduleItem.StartingTime;
                return Convert.ToInt32(time.TotalMinutes / 15);

            }
        }

        public ICommand OnClickCommand { get; set; }


        public ScheduleItemViewModel(ScheduleItem item)
        {
            _ScheduleItem = item;

            OnClickCommand = new RelayCommand(OnClick);
        }

        private void OnClick()
        {
            IoC.ViewModelApplication.GetDescription = ScheduleItem.ToString;
        }
    }
}
