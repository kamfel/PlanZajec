using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PlanZajec.Core
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Private fields
        /// <summary>
        /// Injected class that provides data to display
        /// </summary>
        private IDataProvider _dataProvider { get; set; }

        private bool _IsScheduleLoaded { get; set; } = false;

        private ObservableCollection<ItemViewModel> _Groups { get; set; }

        private ObservableCollection<ItemViewModel> _Teachers { get; set; }

        private ObservableCollection<ItemViewModel> _Rooms { get; set; }

        private string _Legend { get; set; }

        #endregion

        #region Public properties

        public ObservableCollection<ItemViewModel> Groups
        {
            get
            {
                return _Groups;
            }
            set
            {
                if (_Groups == value) return;
                _Groups = value;
                OnPropertyChanged("Groups");
            }
        }

        public ObservableCollection<ItemViewModel> Teachers
        {
            get
            {
                return _Teachers;
            }
            set
            {
                if (_Teachers == value) return;
                _Teachers = value;
                OnPropertyChanged("Groups");
            }
        }

        public ObservableCollection<ItemViewModel> Rooms
        {
            get
            {
                return _Rooms;
            }
            set
            {
                if (_Rooms == value) return;
                _Rooms = value;
                OnPropertyChanged("Groups");
            }
        }

        public ScheduleViewModel Schedule { get; set; }

        public string Legend
        {
            get => _Legend;
            set
            {
                _Legend = value;
                OnPropertyChanged("Legend");
            }
        }

        public bool IsScheduleLoaded
        {
            get => _IsScheduleLoaded;
            set
            {
                if (_IsScheduleLoaded == value) return;
                _IsScheduleLoaded = value;
                OnPropertyChanged("IsScheduleLoaded");
                OnPropertyChanged(nameof(IsScheduleLoading));
            }
        }

        public bool IsScheduleLoading
        {
            get => !_IsScheduleLoaded;
        }

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            //Try to get the necessary data provider
            try
            {
                _dataProvider = IoC.Get<IDataProvider>();
            }
            catch (ActivationException)
            {
                throw new Exception("No IDataProvider provided for MainWindowViewModel");
            }

            //Load items
            LoadGroupItems();
            LoadTeacherItems();
            LoadRoomItems();

            Schedule = new ScheduleViewModel();

            //Task.Run(async () => await Schedule.LoadScheduleItemsAsync(512, ScheduleCategory.Groups, Semester.Winter));

            IoC.EventPropagator.Register<LoadNewScheduleEvent>(this, (args) => IsScheduleLoaded = false);

            IoC.EventPropagator.Register<NewScheduleLoadedEvent>(this, LoadNewLegend);

            IoC.EventPropagator.Register<NewScheduleLoadedEvent>(this, (args) =>
            {
                IsScheduleLoaded = true;
            });
        }

        #endregion

        #region Private methods

        private void LoadNewLegend(EventPropagatorArgs args)
        {
            object legend;
            if (!args.Data.TryGetValue("Legend", out legend)) return;

            Legend = (string)legend;
        }

        private void LoadGroupItems()
        {
            Task.Run(async () =>
            {
                var items = await _dataProvider.GetMainItemsOfCategoryAsync(ScheduleCategory.Groups);
                Groups = new ObservableCollection<ItemViewModel>(items.Select(item => new ItemViewModel(item)));
            });
        }

        private void LoadTeacherItems()
        {
            Task.Run(async () =>
            {
                var items = await _dataProvider.GetMainItemsOfCategoryAsync(ScheduleCategory.Teachers);
                Teachers = new ObservableCollection<ItemViewModel>(items.Select(item => new ItemViewModel(item)));
            });
        }

        private void LoadRoomItems()
        {
            Task.Run(async () =>
            {
                var items = await _dataProvider.GetMainItemsOfCategoryAsync(ScheduleCategory.Rooms);
                Rooms = new ObservableCollection<ItemViewModel>(items.Select(item => new ItemViewModel(item)));
            });
        }

        #endregion
    }
}
