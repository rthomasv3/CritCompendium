using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Business;
using CritCompendium.ViewModels.DialogViewModels;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business;
using CritCompendiumInfrastructure.Business.Search;
using CritCompendiumInfrastructure.Business.Search.Input;

namespace CritCompendium.ViewModels.SearchViewModels
{
   public sealed class MonsterSearchViewModel : NotifyPropertyChanged, IConfirmation
   {
      #region Events

      public event EventHandler AcceptSelected;
      public event EventHandler RejectSelected;
      public event EventHandler CancelSelected;

      #endregion

      #region Fields

      private readonly Compendium _compendium;
      private readonly MonsterSearchService _monsterSearchService;
      private readonly MonsterSearchInput _monsterSearchInput;
      private readonly StringService _stringService;
      private readonly DialogService _dialogService;
      private readonly ObservableCollection<MonsterListItemViewModel> _monsters = new ObservableCollection<MonsterListItemViewModel>();
      private readonly ICommand _selectMonsterCommand;
      private readonly ICommand _resetFiltersCommand;
      private readonly ICommand _acceptCommand;
      private readonly ICommand _rejectCommand;
      private List<MonsterModel> _selectedMonsters = new List<MonsterModel>();

      #endregion

      #region Constructors

      /// <summary>
      /// Creates an instance of <see cref="MonsterSearchViewModel"/>
      /// </summary>
      public MonsterSearchViewModel(Compendium compendium, MonsterSearchService monsterSearchService, MonsterSearchInput monsterSearchInput,
          StringService stringService, DialogService dialogService)
      {
         _compendium = compendium;
         _monsterSearchService = monsterSearchService;
         _monsterSearchInput = monsterSearchInput;
         _stringService = stringService;
         _dialogService = dialogService;

         _selectMonsterCommand = new RelayCommand(obj => true, obj => SelectMonster(obj as MonsterListItemViewModel));
         _resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
         _acceptCommand = new RelayCommand(obj => true, obj => OnAccept());
         _rejectCommand = new RelayCommand(obj => true, obj => OnReject());

         Search();
      }

      #endregion

      #region Properties

      /// <summary>
      /// List of monsters
      /// </summary>
      public ObservableCollection<MonsterListItemViewModel> Monsters
      {
         get { return _monsters; }
      }

      /// <summary>
      /// Gets or sets the search text
      /// </summary>
      public string SearchText
      {
         get { return _monsterSearchInput.SearchText; }
         set
         {
            _monsterSearchInput.SearchText = value;
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
            return _monsterSearchInput.AppliedFilterCount > 0 ? $"Sort and Filter ({_monsterSearchInput.AppliedFilterCount})" : "Sort and Filter";
         }
      }

      /// <summary>
      /// Gets or sets sort and filters expanded
      /// </summary>
      public bool SortAndFiltersExpanded
      {
         get { return _monsterSearchInput.SortAndFiltersExpanded; }
         set { _monsterSearchInput.SortAndFiltersExpanded = value; }
      }

      /// <summary>
      /// Gets sort options
      /// </summary>
      public List<KeyValuePair<MonsterSortOption, string>> SortOptions
      {
         get { return _monsterSearchInput.SortOptions; }
      }

      /// <summary>
      /// Gets selected sort option
      /// </summary>
      public KeyValuePair<MonsterSortOption, string> SelectedSortOption
      {
         get { return _monsterSearchInput.SortOption; }
         set
         {
            _monsterSearchInput.SortOption = value;
            Search();
         }
      }

      /// <summary>
      /// List of sizes
      /// </summary>
      public List<KeyValuePair<CreatureSize, string>> Sizes
      {
         get { return _monsterSearchInput.Sizes; }
      }

      /// <summary>
      /// Selected size
      /// </summary>
      public KeyValuePair<CreatureSize, string> SelectedSize
      {
         get { return _monsterSearchInput.Size; }
         set
         {
            _monsterSearchInput.Size = value;
            Search();
         }
      }

      /// <summary>
      /// List of alignments
      /// </summary>
      public List<KeyValuePair<Alignment, string>> Alignments
      {
         get { return _monsterSearchInput.Alignments; }
      }

      /// <summary>
      /// Selected alignment
      /// </summary>
      public KeyValuePair<Alignment, string> SelectedAlignment
      {
         get { return _monsterSearchInput.Alignment; }
         set
         {
            _monsterSearchInput.Alignment = value;
            Search();
         }
      }

      /// <summary>
      /// List of types
      /// </summary>
      public List<KeyValuePair<string, string>> Types
      {
         get { return _monsterSearchInput.Types; }
      }

      /// <summary>
      /// Selected type
      /// </summary>
      public KeyValuePair<string, string> SelectedType
      {
         get { return _monsterSearchInput.Type; }
         set
         {
            _monsterSearchInput.Type = value;
            Search();
         }
      }

      /// <summary>
      /// List of crs
      /// </summary>
      public List<KeyValuePair<string, string>> CRs
      {
         get { return _monsterSearchInput.CRs; }
      }

      /// <summary>
      /// Selected cr
      /// </summary>
      public KeyValuePair<string, string> SelectedCR
      {
         get { return _monsterSearchInput.CR; }
         set
         {
            _monsterSearchInput.CR = value;
            Search();
         }
      }

      /// <summary>
      /// List of environments
      /// </summary>
      public List<KeyValuePair<string, string>> Environments
      {
         get { return _monsterSearchInput.Environments; }
      }

      /// <summary>
      /// Selected environment
      /// </summary>
      public KeyValuePair<string, string> SelectedEnvironment
      {
         get { return _monsterSearchInput.Environment; }
         set
         {
            _monsterSearchInput.Environment = value;
            Search();
         }
      }

      /// <summary>
      /// Gets selected monsters
      /// </summary>
      public IEnumerable<MonsterModel> SelectedMonsters
      {
         get { return _selectedMonsters; }
      }

      /// <summary>
      /// Command to select a monster
      /// </summary>
      public ICommand SelectMonsterCommand
      {
         get { return _selectMonsterCommand; }
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
         _monsterSearchInput.Reset();

         OnPropertyChanged(nameof(SearchText));
         OnPropertyChanged(nameof(SelectedSortOption));
         OnPropertyChanged(nameof(SelectedSize));
         OnPropertyChanged(nameof(SelectedAlignment));
         OnPropertyChanged(nameof(SelectedType));
         OnPropertyChanged(nameof(SelectedCR));
         OnPropertyChanged(nameof(SelectedEnvironment));

         Search();
      }

      private void Search()
      {
         _monsters.Clear();
         foreach (MonsterModel monsterModel in _monsterSearchService.Search(_monsterSearchInput))
         {
            MonsterListItemViewModel monsterListItemViewModel = new MonsterListItemViewModel(monsterModel, _stringService);
            monsterListItemViewModel.PropertyChanged += MonsterListItemViewModel_PropertyChanged;
            _monsters.Add(monsterListItemViewModel);
         }

         foreach (MonsterModel monsterModel in _selectedMonsters)
         {
            MonsterListItemViewModel monster = _monsters.FirstOrDefault(x => x.MonsterModel.ID == monsterModel.ID);
            if (monster != null)
            {
               monster.IsSelected = true;
            }
         }

         OnPropertyChanged(nameof(SortAndFilterHeader));
      }

      private void SelectMonster(MonsterListItemViewModel monster)
      {
         _dialogService.ShowDetailsDialog(new MonsterViewModel(monster.MonsterModel));
      }

      private void MonsterListItemViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (e.PropertyName == nameof(MonsterListItemViewModel.IsSelected))
         {
            MonsterListItemViewModel monsterListItemViewModel = sender as MonsterListItemViewModel;
            if (monsterListItemViewModel.IsSelected)
            {
               if (!_selectedMonsters.Any(x => x.ID == monsterListItemViewModel.MonsterModel.ID))
               {
                  _selectedMonsters.Add(monsterListItemViewModel.MonsterModel);
               }
            }
            else
            {
               _selectedMonsters.RemoveAll(x => x.ID == monsterListItemViewModel.MonsterModel.ID);
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
