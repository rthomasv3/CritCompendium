using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendium.ViewModels.DialogViewModels;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;
using CritCompendiumInfrastructure.Services.Search;
using CritCompendiumInfrastructure.Services.Search.Input;

namespace CritCompendium.ViewModels.SearchViewModels
{
   public sealed class ItemSearchViewModel : NotifyPropertyChanged, IConfirmation
   {
      #region Events

      public event EventHandler AcceptSelected;
      public event EventHandler RejectSelected;
      public event EventHandler CancelSelected;

      #endregion

      #region Fields

      private readonly Compendium _compendium;
      private readonly ItemSearchService _itemSearchService;
      private readonly ItemSearchInput _itemSearchInput;
      private readonly StringService _stringService;
      private readonly DialogService _dialogService;
      private readonly ObservableCollection<ItemListItemViewModel> _items = new ObservableCollection<ItemListItemViewModel>();
      private readonly ICommand _selectItemCommand;
      private readonly ICommand _resetFiltersCommand;
      private readonly ICommand _acceptCommand;
      private readonly ICommand _rejectCommand;
      private List<ItemModel> _selectedItems = new List<ItemModel>();

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="ItemSearchViewModel"/>
      /// </summary>
      public ItemSearchViewModel(Compendium compendium, ItemSearchService itemSearchService, ItemSearchInput itemSearchInput,
          StringService stringService, DialogService dialogService)
      {
         _compendium = compendium;
         _itemSearchService = itemSearchService;
         _itemSearchInput = itemSearchInput;
         _stringService = stringService;
         _dialogService = dialogService;

         _selectItemCommand = new RelayCommand(obj => true, obj => SelectItem(obj as ItemListItemViewModel));
         _resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
         _acceptCommand = new RelayCommand(obj => true, obj => OnAccept());
         _rejectCommand = new RelayCommand(obj => true, obj => OnReject());

         Search();
      }

      #endregion

      #region Properties

      /// <summary>
      /// List of items
      /// </summary>
      public ObservableCollection<ItemListItemViewModel> Items
      {
         get { return _items; }
      }

      /// <summary>
      /// Gets or sets the search text
      /// </summary>
      public string SearchText
      {
         get { return _itemSearchInput.SearchText; }
         set
         {
            _itemSearchInput.SearchText = value;
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
            return _itemSearchInput.AppliedFilterCount > 0 ? $"Sort and Filter ({_itemSearchInput.AppliedFilterCount})" : "Sort and Filter";
         }
      }

      /// <summary>
      /// Gets or sets sort and filters expanded
      /// </summary>
      public bool SortAndFiltersExpanded
      {
         get { return _itemSearchInput.SortAndFiltersExpanded; }
         set { _itemSearchInput.SortAndFiltersExpanded = value; }
      }

      /// <summary>
      /// Gets sort options
      /// </summary>
      public List<KeyValuePair<ItemSortOption, string>> SortOptions
      {
         get { return _itemSearchInput.SortOptions; }
      }

      /// <summary>
      /// Gets selected sort option
      /// </summary>
      public KeyValuePair<ItemSortOption, string> SelectedSortOption
      {
         get { return _itemSearchInput.SortOption; }
         set
         {
            _itemSearchInput.SortOption = value;
            Search();
         }
      }

      /// <summary>
      /// List of item types
      /// </summary>
      public List<KeyValuePair<ItemType, string>> ItemTypes
      {
         get { return _itemSearchInput.Types; }
      }

      /// <summary>
      /// Selected item type
      /// </summary>
      public KeyValuePair<ItemType, string> SelectedItemType
      {
         get { return _itemSearchInput.ItemType; }
         set
         {
            _itemSearchInput.ItemType = value;
            Search();
         }
      }

      /// <summary>
      /// List of magic options
      /// </summary>
      public List<KeyValuePair<bool?, string>> MagicOptions
      {
         get { return _itemSearchInput.MagicOptions; }
      }

      /// <summary>
      /// Selected magic option
      /// </summary>
      public KeyValuePair<bool?, string> SelectedMagicOption
      {
         get { return _itemSearchInput.Magic; }
         set
         {
            _itemSearchInput.Magic = value;
            Search();
         }
      }

      /// <summary>
      /// List of rarities
      /// </summary>
      public List<KeyValuePair<Rarity, string>> Rarities
      {
         get { return _itemSearchInput.Rarities; }
      }

      /// <summary>
      /// Selected rarity
      /// </summary>
      public KeyValuePair<Rarity, string> SelectedRarity
      {
         get { return _itemSearchInput.Rarity; }
         set
         {
            _itemSearchInput.Rarity = value;
            Search();
         }
      }

      /// <summary>
      /// List of attunement options
      /// </summary>
      public List<KeyValuePair<bool?, string>> AttunementOptions
      {
         get { return _itemSearchInput.AttunementOptions; }
      }

      /// <summary>
      /// Selected attunement options
      /// </summary>
      public KeyValuePair<bool?, string> SelectedAttunementOption
      {
         get { return _itemSearchInput.RequresAttunement; }
         set
         {
            _itemSearchInput.RequresAttunement = value;
            Search();
         }
      }

      /// <summary>
      /// Gets selected items
      /// </summary>
      public IEnumerable<ItemModel> SelectedItems
      {
         get { return _selectedItems; }
      }

      /// <summary>
      /// Command to select an item
      /// </summary>
      public ICommand SelectItemCommand
      {
         get { return _selectItemCommand; }
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
         _itemSearchInput.Reset();

         OnPropertyChanged(nameof(SearchText));
         OnPropertyChanged(nameof(SelectedSortOption));
         OnPropertyChanged(nameof(SelectedItemType));
         OnPropertyChanged(nameof(SelectedMagicOption));
         OnPropertyChanged(nameof(SelectedRarity));
         OnPropertyChanged(nameof(SelectedAttunementOption));

         Search();
      }

      private void Search()
      {
         _items.Clear();
         foreach (ItemModel itemModel in _itemSearchService.Search(_itemSearchInput))
         {
            ItemListItemViewModel itemListItemViewModel = new ItemListItemViewModel(itemModel, _stringService);
            itemListItemViewModel.PropertyChanged += ItemListItemViewModel_PropertyChanged;
            _items.Add(itemListItemViewModel);
         }

         foreach (ItemModel itemModel in _selectedItems)
         {
            ItemListItemViewModel item = _items.FirstOrDefault(x => x.ItemModel.ID == itemModel.ID);
            if (item != null)
            {
               item.IsSelected = true;
            }
         }

         OnPropertyChanged(nameof(SortAndFilterHeader));
      }

      private void SelectItem(ItemListItemViewModel item)
      {
         _dialogService.ShowDetailsDialog(new ItemViewModel(item.ItemModel));
      }

      private void ItemListItemViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (e.PropertyName == nameof(ItemListItemViewModel.IsSelected))
         {
            ItemListItemViewModel itemListItemViewModel = sender as ItemListItemViewModel;
            if (itemListItemViewModel.IsSelected)
            {
               if (!_selectedItems.Any(x => x.ID == itemListItemViewModel.ItemModel.ID))
               {
                  _selectedItems.Add(itemListItemViewModel.ItemModel);
               }
            }
            else
            {
               _selectedItems.RemoveAll(x => x.ID == itemListItemViewModel.ItemModel.ID);
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
