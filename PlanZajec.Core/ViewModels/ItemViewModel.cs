using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlanZajec.Core
{
    public class ItemViewModel : BaseViewModel
    {
        #region Private Fields

        private IDataProvider _dataProvider;

        private Item _Item { get; set; }

        private ObservableCollection<ItemViewModel> _Children { get; set; }

        private bool _IsSelected { get; set; }

        #endregion

        #region Public Properties

        public ObservableCollection<ItemViewModel> Children
        {
            get => _Children;
            set
            {
                if (this._Children == value) return;

                _Children = value;
                OnPropertyChanged("Children");
            }
        }

        public string Name
        {
            get => _Item?.Name;
            set
            {
                if (_Item?.Name == value) return;

                _Item.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public bool IsExpanded
        {
            get
            {
                return Children?.Count(item => item != null) > 0;
            }
            set
            {
                if (value == true)
                {
                    Task.Run(async () => await ExpandAsync())
                        .GetAwaiter()
                        .OnCompleted(() => OnPropertyChanged("IsExpanded"));
                }
                else
                {
                    this.ClearChildren();
                }
            }
        }

        public bool HasSchedule => _Item.HasSchedule;

        public bool IsSelected
        {
            get => _IsSelected && _Item.HasSchedule;
            set
            {
                if (_IsSelected == value || !_Item.HasSchedule) return;
                _IsSelected = value;
                IoC.ViewModelApplication.CurrentId = _Item.Id;

                IoC.EventPropagator.Publish<LoadNewScheduleEvent>(EventPropagatorArgs.None);

                OnPropertyChanged("IsSelected");
            }
        }

        #endregion

        #region Constructor

        public ItemViewModel(Item item)
        {
            try
            {
                _dataProvider = IoC.Get<IDataProvider>();
            }
            catch (ActivationException)
            {
                throw new Exception("No IDataProvider provided for MainWindowViewModel");
            }

            _Item = item;
            Children = new ObservableCollection<ItemViewModel>();

            if (!item.HasSchedule)
            {
                Children.Add(null);
            }
        }

        #endregion

        #region Methods

        private async Task ExpandAsync()
        {
            List<Item> children_items = await _dataProvider.GetItemContentsAsync(_Item.Id, _Item.Category);

            if (children_items == null)
            {
                this.Children = new ObservableCollection<ItemViewModel>();
            }
            else
            {
                this.Children = new ObservableCollection<ItemViewModel>(children_items.Select(item => new ItemViewModel(item)));
            }
        }

        /// <summary>
        /// Deletes all children and adds a dummy child
        /// </summary>
        public void ClearChildren()
        {
            this.Children = new ObservableCollection<ItemViewModel>();
            this.Children.Add(null);
        }

        #endregion Methods
    }
}
