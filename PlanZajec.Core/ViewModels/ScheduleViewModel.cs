using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlanZajec.Core
{
    public class ScheduleViewModel : BaseViewModel
    {
        #region Private Fields

        private ObservableCollection<ScheduleItemViewModel> _ScheduleItems { get; set; }

        private ObservableCollection<TimeLabelViewModel> _Labels { get; set; }

        private uint _ScheduleRowAmount { get; set; }

        private readonly CancellationTokenSource _TokenSource;

        #endregion

        /// <summary>
        /// Items on the schedule like lectures etc.
        /// </summary>
        public ObservableCollection<ScheduleItemViewModel> ScheduleItems
        {
            get => _ScheduleItems;
            set
            {
                if (_ScheduleItems == value) return;
                _ScheduleItems = value;
                OnPropertyChanged("ScheduleItems");
            }
        }

        /// <summary>
        /// The labels which describe the time on the schedule
        /// </summary>
        public ObservableCollection<TimeLabelViewModel> Labels
        {
            get => _Labels;
            set
            {
                if (_Labels == value) return;
                _Labels = value;
                OnPropertyChanged("Labels");
            }
        }

        /// <summary>
        /// Amount of rows of the schedule's modifiable part (part where schedule items are)
        /// </summary>
        public uint ScheduleRowAmount
        {
            get => _ScheduleRowAmount;
            set
            {
                if (_ScheduleRowAmount == value) return;
                _ScheduleRowAmount = value;
                OnPropertyChanged("ScheduleRowAmount");
            }
        }



        public ScheduleViewModel()
        {
            //Check if a data provider is provided
            if (IoC.DataProvider == null)
                throw new Exception("No IDataProvider provided for MainWindowViewModel");

            //Check if an event propagator is provided
            if (IoC.EventPropagator == null)
                throw new Exception("No IEventPropagator provided for MainWindowViewModel");

            IoC.EventPropagator.Register<LoadNewScheduleEvent>(this, LoadScheduleItems);

            _TokenSource = new CancellationTokenSource();
        }

        public void LoadScheduleItems(EventPropagatorArgs args)
        {
            //Load current values from app view model
            var appvm = IoC.ViewModelApplication;
            var id = appvm.CurrentId;
            var category = appvm.CurrentCategory;
            var semester = appvm.CurrentSemester;

            //Load schedule items and update schedule title
            Task.Run(async () => appvm.CurrentTitle = await LoadScheduleItemsAsync(id, category, semester, _TokenSource.Token), _TokenSource.Token);
        }

        public async Task<string> LoadScheduleItemsAsync(int id, ScheduleCategory category, Semester semester, CancellationToken ct)
        {
            ScheduleItems = null;

            //Load schedule items
            var scheduleInfo = await IoC.DataProvider.GetScheduleInfoAsync(id, category, semester, ct);

            //Check if task is canceled and exit task if true
            ct.ThrowIfCancellationRequested();

            //Convert schedule items to schedule items view model
            var schedule_items = scheduleInfo.Item2?.Select(item => new ScheduleItemViewModel(item));

            if (schedule_items == null)
                //Set null because there are no schedule items
                ScheduleItems = null;
            else
            {
                //Get min and max values of time for the labels
                var min_time = schedule_items.Select(item => item.ScheduleItem.StartingTime).Min();
                var max_time = schedule_items.Select(item => item.ScheduleItem.EndingTime).Max();

                IoC.ViewModelApplication.CurrentMinTime = min_time;

                //Adjust row amount so the labels will fit
                ScheduleRowAmount = Convert.ToUInt32((max_time - min_time).TotalMinutes / 15 - 1);

                //Set new time labels for the schedule
                Labels = new ObservableCollection<TimeLabelViewModel>(GetLabelsFromRange(min_time, max_time));

                //Translate schedule items to scheduleitemviewmodels
                ScheduleItems = new ObservableCollection<ScheduleItemViewModel>(schedule_items);
            }

            //Raise event to load new legend
            var args = new Dictionary<string, object>() { { "Legend", GetLegend() } };
            IoC.EventPropagator.Publish<NewScheduleLoadedEvent>(new EventPropagatorArgs(args));

            //Return title
            return scheduleInfo.Item1;
        }

        public string GetLegend()
        {
            var legend_builder = new StringBuilder(50);
            var already_used = new List<string>();

            foreach (var item in ScheduleItems)
            {
                var abbrev = item.ScheduleItem.Abbrev;

                if (already_used.Contains(abbrev))
                    continue;
                else
                    already_used.Add(abbrev);

                var name = item.ScheduleItem.Name;

                legend_builder.AppendLine($"{abbrev} - {name}");
            }

            return legend_builder.ToString();
        }

        /// <summary>
        /// Gets a list of labels in format eg. 00:00-00:15 between specified range separated by 15 minutes
        /// </summary>
        /// <param name="min_time"></param>
        /// <param name="max_time"></param>
        /// <returns></returns>
        private List<TimeLabelViewModel> GetLabelsFromRange(DateTime min_time, DateTime max_time)
        {
            //Max time cannot be bigger than min time
            if (max_time < min_time) return null;

            //Get difference between max and min time
            var diff_in_minutes = (max_time - min_time).TotalMinutes - 15;

            var timelabels = new List<TimeLabelViewModel>();

            //Create labels and add to the list
            for (int i = 0; i < diff_in_minutes; i += 15)
            {
                var label = new TimeLabelViewModel();
                label.RowIndex = i / 15;
                label.Start = new DateTime(min_time.Ticks) + TimeSpan.FromMinutes(i);
                label.End = new DateTime(min_time.Ticks) + TimeSpan.FromMinutes(i + 15);

                timelabels.Add(label);
            }

            return timelabels;
        }
    }
}
