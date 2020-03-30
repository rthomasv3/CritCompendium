using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendium.ViewModels.DialogViewModels;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendium.ViewModels.ObjectViewModels;
using CriticalCompendiumInfrastructure.Enums;
using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Services;
using CriticalCompendiumInfrastructure.Services.Search;
using CriticalCompendiumInfrastructure.Services.Search.Input;

namespace CritCompendium.ViewModels.SearchViewModels
{
    public sealed class SpellSearchViewModel : NotifyPropertyChanged, IConfirmation
    {
        #region Events

        public event EventHandler AcceptSelected;
        public event EventHandler RejectSelected;
        public event EventHandler CancelSelected;

        #endregion

        #region Fields

        private readonly Compendium _compendium;
        private readonly SpellSearchService _spellSearchService;
        private readonly SpellSearchInput _spellSearchInput;
        private readonly StringService _stringService;
        private readonly DialogService _dialogService;
        private readonly ObservableCollection<SpellListItemViewModel> _spells = new ObservableCollection<SpellListItemViewModel>();
        private readonly ICommand _selectSpellCommand;
        private readonly ICommand _resetFiltersCommand;
        private readonly ICommand _acceptCommand;
        private readonly ICommand _rejectCommand;
        private List<SpellModel> _selectedSpells = new List<SpellModel>();
        private bool _multiSelect;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="SpellSearchViewModel"/>
        /// </summary>
        public SpellSearchViewModel(Compendium compendium, SpellSearchService spellSearchService, SpellSearchInput spellSearchInput,
            StringService stringService, DialogService dialogService)
        {
            _compendium = compendium;
            _spellSearchService = spellSearchService;
            _spellSearchInput = spellSearchInput;
            _stringService = stringService;
            _dialogService = dialogService;

            _selectSpellCommand = new RelayCommand(obj => true, obj => SelectSpell(obj as SpellListItemViewModel));
            _resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
            _acceptCommand = new RelayCommand(obj => true, obj => OnAccept());
            _rejectCommand = new RelayCommand(obj => true, obj => OnReject());

            Search();
        }

        #endregion

        #region Properties

        /// <summary>
        /// List of spells
        /// </summary>
        public ObservableCollection<SpellListItemViewModel> Spells
        {
            get { return _spells; }
        }

        /// <summary>
        /// Gets or sets multi select
        /// </summary>
        public bool MultiSelect
        {
            get { return _multiSelect; }
            set { _multiSelect = value; }
        }

        /// <summary>
        /// Gets radio group name
        /// </summary>
        public string RadioGroupName
        {
            get { return _multiSelect ? null : "Spells"; }
        }

        /// <summary>
        /// Gets or sets the search text
        /// </summary>
        public string SearchText
        {
            get { return _spellSearchInput.SearchText; }
            set
            {
                _spellSearchInput.SearchText = value;
                Search();
            }
        }

        /// <summary>
        /// Gets sort and filter header
        /// </summary>
        public string SortAndFilterHeader
        {
            get
            {
                return _spellSearchInput.AppliedFilterCount > 0 ? $"Sort and Filter ({_spellSearchInput.AppliedFilterCount})" : "Sort and Filter";
            }
        }

        /// <summary>
        /// Gets or sets sort and filters expanded
        /// </summary>
        public bool SortAndFiltersExpanded
        {
            get { return _spellSearchInput.SortAndFiltersExpanded; }
            set { _spellSearchInput.SortAndFiltersExpanded = value; }
        }

        /// <summary>
        /// Gets sort options
        /// </summary>
        public List<KeyValuePair<SpellSortOption, string>> SortOptions
        {
            get { return _spellSearchInput.SortOptions; }
        }

        /// <summary>
        /// Gets selected sort option
        /// </summary>
        public KeyValuePair<SpellSortOption, string> SelectedSortOption
        {
            get { return _spellSearchInput.SortOption; }
            set
            {
                _spellSearchInput.SortOption = value;
                Search();
            }
        }

        /// <summary>
        /// List of levels
        /// </summary>
        public List<KeyValuePair<int?, string>> Levels
        {
            get { return _spellSearchInput.Levels; }
        }

        /// <summary>
        /// Selected level
        /// </summary>
        public KeyValuePair<int?, string> SelectedLevel
        {
            get { return _spellSearchInput.Level; }
            set
            {
                _spellSearchInput.Level = value;
                Search();
            }
        }

        /// <summary>
        /// List of schools
        /// </summary>
        public List<KeyValuePair<SpellSchool, string>> Schools
        {
            get { return _spellSearchInput.Schools; }
        }

        /// <summary>
        /// Selected school
        /// </summary>
        public KeyValuePair<SpellSchool, string> SelectedSchool
        {
            get { return _spellSearchInput.School; }
            set
            {
                _spellSearchInput.School = value;
                Search();
            }
        }

        /// <summary>
        /// List of classes
        /// </summary>
        public List<KeyValuePair<string, string>> Classes
        {
            get { return _spellSearchInput.Classes; }
        }

        /// <summary>
        /// Selected class
        /// </summary>
        public KeyValuePair<string, string> SelectedClass
        {
            get { return _spellSearchInput.Class; }
            set
            {
                _spellSearchInput.Class = value;
                Search();
            }
        }

        /// <summary>
        /// List of concentration options
        /// </summary>
        public List<KeyValuePair<bool?, string>> ConcentrationOptions
        {
            get { return _spellSearchInput.ConcentrationOptions; }
        }

        /// <summary>
        /// Selected concentration option
        /// </summary>
        public KeyValuePair<bool?, string> SelectedConcentrationOption
        {
            get { return _spellSearchInput.Concentration; }
            set
            {
                _spellSearchInput.Concentration = value;
                Search();
            }
        }

        /// <summary>
        /// List of ritual options
        /// </summary>
        public List<KeyValuePair<bool?, string>> RitualOptions
        {
            get { return _spellSearchInput.RitualOptions; }
        }

        /// <summary>
        /// Selected ritual option
        /// </summary>
        public KeyValuePair<bool?, string> SelectedRitualOption
        {
            get { return _spellSearchInput.Ritual; }
            set
            {
                _spellSearchInput.Ritual = value;
                Search();
            }
        }

        /// <summary>
        /// Gets selected spells
        /// </summary>
        public IEnumerable<SpellModel> SelectedSpells
        {
            get { return _selectedSpells; }
        }

        /// <summary>
        /// Command to select an spell
        /// </summary>
        public ICommand SelectSpellCommand
        {
            get { return _selectSpellCommand; }
        }

        /// <summary>
        /// Command to reset filters
        /// </summary>
        public ICommand ResetFiltersCommand
        {
            get { return _resetFiltersCommand; }
        }

        /// <summary>
        /// Gets acceptCommand
        /// </summary>
        public ICommand AcceptCommand
        {
            get { return _acceptCommand; }
        }

        /// <summary>
        /// Gets rejectCommand
        /// </summary>
        public ICommand RejectCommand
        {
            get { return _rejectCommand; }
        }

        #endregion

        #region Private Methods

        private void InitializeSearch()
        {
            _spellSearchInput.Reset();

            OnPropertyChanged(nameof(SearchText));
            OnPropertyChanged(nameof(SelectedSortOption));
            OnPropertyChanged(nameof(SelectedLevel));
            OnPropertyChanged(nameof(SelectedSchool));
            OnPropertyChanged(nameof(SelectedClass));
            OnPropertyChanged(nameof(SelectedConcentrationOption));
            OnPropertyChanged(nameof(SelectedRitualOption));

            Search();
        }

        private void Search()
        {
            _spells.Clear();
            foreach (SpellModel spellModel in _spellSearchService.Search(_spellSearchInput))
            {
                SpellListItemViewModel spellListSpellViewModel = new SpellListItemViewModel(spellModel, _stringService);
                spellListSpellViewModel.PropertyChanged += SpellListItemViewModel_PropertyChanged;
                _spells.Add(spellListSpellViewModel);
            }

            foreach (SpellModel spellModel in _selectedSpells)
            {
                SpellListItemViewModel spell = _spells.FirstOrDefault(x => x.SpellModel.ID == spellModel.ID);
                if (spell != null)
                {
                    spell.IsSelected = true;
                }
            }

            OnPropertyChanged(nameof(SortAndFilterHeader));
        }

        private void SelectSpell(SpellListItemViewModel spell)
        {
            _dialogService.ShowDetailsDialog(new SpellViewModel(spell.SpellModel));
        }

        private void SpellListItemViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SpellListItemViewModel.IsSelected))
            {
                SpellListItemViewModel spellListSpellViewModel = sender as SpellListItemViewModel;

                if (spellListSpellViewModel.IsSelected)
                {
                    if (!_multiSelect)
                    {
                        _selectedSpells.Clear();
                    }

                    if (!_selectedSpells.Any(x => x.ID == spellListSpellViewModel.SpellModel.ID))
                    {
                        _selectedSpells.Add(spellListSpellViewModel.SpellModel);
                    }
                }
                else
                {
                    _selectedSpells.RemoveAll(x => x.ID == spellListSpellViewModel.SpellModel.ID);
                }
            }
        }

        private void OnAccept()
        {
            AcceptSelected?.Invoke(this, EventArgs.Empty);
        }

        private void OnReject()
        {
            RejectSelected?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
