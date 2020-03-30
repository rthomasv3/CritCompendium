using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CritCompendium.Services;
using CriticalCompendiumInfrastructure;
using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class RandomTableEditViewModel : NotifyPropertyChanged
    {
        #region Fields

        private readonly StatService _statService = DependencyResolver.Resolve<StatService>();
        private readonly DialogService _dialogService  = DependencyResolver.Resolve<DialogService>();

        private readonly RandomTableModel _randomTableModel;
        private readonly ObservableCollection<RandomTableRowViewModel> _rows = new ObservableCollection<RandomTableRowViewModel>();
        private readonly List<string> _dieOptions = new List<string>();

        private string _name;
        private string _tags;
        private string _die;
        private string _header;

        private ICommand _addRowCommand;
        private ICommand _deleteRowCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="RandomTableEditViewModel"/>
        /// </summary>
        public RandomTableEditViewModel(RandomTableModel randomTableModel)
        {
            _randomTableModel = new RandomTableModel(randomTableModel);

            foreach (RandomTableRowModel rowModel in _randomTableModel.Rows)
            {
                RandomTableRowViewModel randomTableRowViewModel = new RandomTableRowViewModel(rowModel);
                randomTableRowViewModel.PropertyChanged += Row_PropertyChanged;
                _rows.Add(randomTableRowViewModel);
            }

            _dieOptions.AddRange(_statService.Dice);

            _name = _randomTableModel.Name;

            if (_randomTableModel.Tags.Any())
            {
                _tags = String.Join(", ", _randomTableModel.Tags);
            }

            _die = !String.IsNullOrWhiteSpace(_randomTableModel.Die) ? _randomTableModel.Die : _dieOptions[0];
            _header = _randomTableModel.Header;

            _addRowCommand = new RelayCommand(obj => true, obj => AddRow());
            _deleteRowCommand = new RelayCommand(obj => true, obj => DeleteRow(obj as RandomTableRowViewModel));

            SetRowDieMaximums();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets random table model
        /// </summary>
        public RandomTableModel RandomTableModel
        {
            get { return _randomTableModel; }
        }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name
        {
            get { return _randomTableModel.Name; }
            set
            {
                if (Set(ref _name, value))
                {
                    _randomTableModel.Name = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets tags
        /// </summary>
        public string Tags
        {
            get { return _tags; }
            set
            {
                if (Set(ref _tags, value))
                {
                    if (!String.IsNullOrWhiteSpace(_tags))
                    {
                        _randomTableModel.Tags.Clear();
                        foreach (string tag in _tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            _randomTableModel.Tags.Add(tag.Trim());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets die options
        /// </summary>
        public IEnumerable<string> DieOptions
        {
            get { return _dieOptions; }
        }

        /// <summary>
        /// Gets or sets die
        /// </summary>
        public string Die
        {
            get { return _die; }
            set
            {
                if (Set(ref _die, value))
                {
                    _randomTableModel.Die = value;

                    SetRowDieMaximums();
                }
            }
        }

        /// <summary>
        /// Gets or sets header
        /// </summary>
        public string Header
        {
            get { return _header; }
            set
            {
                if (Set(ref _header, value))
                {
                    _randomTableModel.Header = value;
                }
            }
        }

        /// <summary>
        /// Gets rows
        /// </summary>
        public IEnumerable<RandomTableRowViewModel> Rows
        {
            get { return _rows; }
        }

        /// <summary>
        /// Gets add row command
        /// </summary>
        public ICommand AddRowCommand
        {
            get { return _addRowCommand; }
        }

        /// <summary>
        /// Gets delete row command
        /// </summary>
        public ICommand DeleteRowCommand
        {
            get { return _deleteRowCommand; }
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods

        private void SetRowDieMaximums()
        {
            if (Int32.TryParse(_die.Replace("d", String.Empty), out int die))
            {
                foreach (RandomTableRowViewModel rowViewModel in _rows)
                {
                    rowViewModel.DieMax = die;
                }
            }
        }

        private void AddRow()
        {
            RandomTableRowModel rowModel = new RandomTableRowModel();
            _randomTableModel.Rows.Add(rowModel);

            RandomTableRowViewModel randomTableRowViewModel = new RandomTableRowViewModel(rowModel);

            if (Int32.TryParse(_die.Replace("d", String.Empty), out int die))
            {
                randomTableRowViewModel.DieMax = die;
            }

            if (_rows.Any())
            {
                randomTableRowViewModel.Min = _rows.Last().Max + 1;
                randomTableRowViewModel.Max = randomTableRowViewModel.Min;
            }
            else
            {
                randomTableRowViewModel.Min = 1;
                randomTableRowViewModel.Max = 1;
            }

            randomTableRowViewModel.PropertyChanged += Row_PropertyChanged;

            _rows.Add(randomTableRowViewModel);

            OnPropertyChanged(nameof(Rows));
        }

        private void DeleteRow(RandomTableRowViewModel randomTableRowViewModel)
        {
            if (_dialogService.ShowConfirmationDialog("Delete Row", "Are you sure you want to delete the row?", "Yes", "No", null) == true)
            {
                randomTableRowViewModel.PropertyChanged -= Row_PropertyChanged;

                _randomTableModel.Rows.Remove(randomTableRowViewModel.RowModel);
                _rows.Remove(randomTableRowViewModel);

                OnPropertyChanged(nameof(Rows));
            }
        }

        private void Row_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RandomTableModel));
        }

        #endregion
    }
}
