using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Persistence;
using CritCompendiumInfrastructure.Services;
using CritCompendiumInfrastructure.Services.Search;
using CritCompendiumInfrastructure.Services.Search.Input;

namespace CritCompendium.ViewModels
{
    public sealed class ItemsViewModel : NotifyPropertyChanged
	{
		#region Fields

		private readonly Compendium _compendium;
		private readonly ItemSearchService _itemSearchService;
		private readonly StringService _stringService;
		private readonly DialogService _dialogService;
		private readonly XMLImporter _xmlImporter;
        private readonly XMLExporter _xmlExporter;
        private readonly DocumentService _documentService;
        private readonly ObservableCollection<ItemListItemViewModel> _items = new ObservableCollection<ItemListItemViewModel>();
		private readonly ICommand _selectItemCommand;
		private readonly ICommand _editItemCommand;
        private readonly ICommand _exportItemCommand;
        private readonly ICommand _cancelEditItemCommand;
		private readonly ICommand _saveEditItemCommand;
		private readonly ICommand _resetFiltersCommand;
		private readonly ICommand _addCommand;
		private readonly ICommand _copyCommand;
		private readonly ICommand _deleteCommand;
        private readonly ICommand _importCommand;
        private readonly ICommand _selectNextCommand;
        private readonly ICommand _selectPreviousCommand;
        private ItemSearchInput _itemSearchInput;
		private ItemViewModel _selectedItem;
        private string _editItemXML;
		private bool _editHasUnsavedChanges;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of <see cref="ItemsViewModel"/>
		/// </summary>
		public ItemsViewModel(Compendium compendium, ItemSearchService itemSearchService, ItemSearchInput itemSearchInput,
			StringService stringService, DialogService dialogService, XMLImporter xmlImporter, XMLExporter xmlExporter, DocumentService documentService)
		{
			_compendium = compendium;
			_itemSearchService = itemSearchService;
            _itemSearchInput = itemSearchInput;
			_stringService = stringService;
			_dialogService = dialogService;
			_xmlImporter = xmlImporter;
            _xmlExporter = xmlExporter;
            _documentService = documentService;

            _selectItemCommand = new RelayCommand(obj => true, obj => SelectItem(obj as ItemListItemViewModel));
			_editItemCommand = new RelayCommand(obj => true, obj => EditItem(obj as ItemViewModel));
            _exportItemCommand = new RelayCommand(obj => true, obj => ExportItem(obj as ItemViewModel));
            _cancelEditItemCommand = new RelayCommand(obj => true, obj => CancelEditItem());
			_saveEditItemCommand = new RelayCommand(obj => HasUnsavedChanges, obj => SaveEditItem());
			_resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
			_addCommand = new RelayCommand(obj => true, obj => Add());
			_copyCommand = new RelayCommand(obj => _selectedItem != null, obj => Copy());
			_deleteCommand = new RelayCommand(obj => _selectedItem != null, obj => Delete());
            _importCommand = new RelayCommand(obj => true, obj => Import());
            _selectNextCommand = new RelayCommand(obj => true, obj => SelectNext());
            _selectPreviousCommand = new RelayCommand(obj => true, obj => SelectPrevious());

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
		/// Command to select a item
		/// </summary>
		public ICommand SelectItemCommand
		{
			get { return _selectItemCommand; }
		}

		/// <summary>
		/// Selected item
		/// </summary>
		public ItemViewModel SelectedItem
		{
			get { return _selectedItem; }
		}

		/// <summary>
		/// Editing item xml
		/// </summary>
		public string EditingItemXML
		{
			get { return _editItemXML; }
			set
			{
				if (Set(ref _editItemXML, value) && !_editHasUnsavedChanges)
				{
					_editHasUnsavedChanges = true;
					OnPropertyChanged(nameof(HasUnsavedChanges));
				}
			}
		}

		/// <summary>
		/// Command to reset filters
		/// </summary>
		public ICommand ResetFiltersCommand
		{
			get { return _resetFiltersCommand; }
		}

		/// <summary>
		/// Command to edit item
		/// </summary>
		public ICommand EditItemCommand
		{
			get { return _editItemCommand; }
		}

        /// <summary>
		/// Command to export item
		/// </summary>
		public ICommand ExportItemCommand
        {
            get { return _exportItemCommand; }
        }

        /// <summary>
        /// Command to cancel edit item
        /// </summary>
        public ICommand CancelEditItemCommand
		{
			get { return _cancelEditItemCommand; }
		}

		/// <summary>
		/// Command to save edit item
		/// </summary>
		public ICommand SaveEditItemCommand
		{
			get { return _saveEditItemCommand; }
		}

		/// <summary>
		/// Command to add item
		/// </summary>
		public ICommand AddItemCommand
		{
			get { return _addCommand; }
		}

		/// <summary>
		/// Command to copy item
		/// </summary>
		public ICommand CopyItemCommand
		{
			get { return _copyCommand; }
		}

		/// <summary>
		/// Command to delete selected item
		/// </summary>
		public ICommand DeleteItemCommand
		{
			get { return _deleteCommand; }
        }

        /// <summary>
        /// Command to import
        /// </summary>
        public ICommand ImportCommand
        {
            get { return _importCommand; }
        }

        /// <summary>
        /// Command to select next
        /// </summary>
        public ICommand SelectNextCommand
        {
            get { return _selectNextCommand; }
        }

        /// <summary>
        /// Command to select previous
        /// </summary>
        public ICommand SelectPreviousCommand
        {
            get { return _selectPreviousCommand; }
        }

        /// <summary>
        /// True if currently editing a item
        /// </summary>
        public bool IsEditingItem
		{
			get { return _editItemXML != null; }
		}

		/// <summary>
		/// True if edit has unsaved changes
		/// </summary>
		public bool HasUnsavedChanges
		{
			get { return _editHasUnsavedChanges; }
		}

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches, applying current sorting and filtering
        /// </summary>
        public void Search()
        {
            _items.Clear();
            foreach (ItemModel itemModel in _itemSearchService.Search(_itemSearchInput))
            {
                _items.Add(new ItemListItemViewModel(itemModel, _stringService));
            }

            if (_selectedItem != null)
            {
                ItemListItemViewModel item = _items.FirstOrDefault(x => x.ItemModel.ID == _selectedItem.ItemModel.ID);
                if (item != null)
                {
                    item.IsSelected = true;
                }
            }

            OnPropertyChanged(nameof(SortAndFilterHeader));
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

        private void SelectItem(ItemListItemViewModel listItem)
		{
			bool selectItem = true;

			if (_editItemXML != null)
			{
				if (_editHasUnsavedChanges)
				{
					string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
														 _selectedItem.Name, Environment.NewLine + Environment.NewLine);
                    string accept = "Save and Continue";
                    string reject = "Discard Changes";
                    string cancel = "Cancel Navigation";
                    bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                    if (result == true)
                    {
                        if (!SaveEditItem())
                        {
                            selectItem = false;
                        }
                    }
                    else if (result == false)
                    {
                        CancelEditItem();
                    }
                    else
                    {
                        selectItem = false;
                    }
                }
				else
				{
					CancelEditItem();
				}
			}

			if (selectItem)
			{
				foreach (ItemListItemViewModel item in _items)
				{
					item.IsSelected = false;
				}
				listItem.IsSelected = true;

				_selectedItem = new ItemViewModel(listItem.ItemModel);
				OnPropertyChanged(nameof(SelectedItem));
			}
		}

		private void EditItem(ItemViewModel itemModel)
		{
			_editItemXML = itemModel.XML;

			OnPropertyChanged(nameof(EditingItemXML));
			OnPropertyChanged(nameof(IsEditingItem));
		}

		private void CancelEditItem()
		{
			_editHasUnsavedChanges = false;
			_editItemXML = null;

			OnPropertyChanged(nameof(EditingItemXML));
			OnPropertyChanged(nameof(IsEditingItem));
			OnPropertyChanged(nameof(HasUnsavedChanges));
		}

		private bool SaveEditItem()
		{
			bool saved = false;

			try
			{
				ItemModel model = _xmlImporter.GetItem(_editItemXML);

				if (model != null)
				{
					model.ID = _selectedItem.ItemModel.ID;
                    _compendium.UpdateItem(model);
					_selectedItem = new ItemViewModel(model);

					ItemListItemViewModel oldListItem = _items.FirstOrDefault(x => x.ItemModel.ID == model.ID);
					if (oldListItem != null)
					{
                        if (_itemSearchService.SearchInputApplies(_itemSearchInput, model))
                        {
                            oldListItem.UpdateModel(model);
                        }
                        else
                        {
                            _items.Remove(oldListItem);
                        }
					}

					_editItemXML = null;
					_editHasUnsavedChanges = false;

					SortItems();

                    _compendium.SaveItems();

					OnPropertyChanged(nameof(SelectedItem));
					OnPropertyChanged(nameof(EditingItemXML));
					OnPropertyChanged(nameof(IsEditingItem));
					OnPropertyChanged(nameof(HasUnsavedChanges));

					saved = true;
				}
				else
				{
					string message = String.Format("Something went wrong...{0}{1}{2}{3}",
														 Environment.NewLine + Environment.NewLine,
														 "The following are required:",
														 Environment.NewLine,
														 "-name");
					_dialogService.ShowConfirmationDialog("Unable To Save", message, "OK", null, null);
				}
			}
			catch (Exception ex)
			{
				string message = String.Format("Something went wrong...{0}{1}",
														 Environment.NewLine + Environment.NewLine,
														 ex.Message);
				_dialogService.ShowConfirmationDialog("Unable To Save", message, "OK", null, null);
			}

			return saved;
		}

		private void Add()
		{
			bool addItem = true;

			if (_editItemXML != null)
			{
				if (_editHasUnsavedChanges)
				{
					string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
														 _selectedItem.Name, Environment.NewLine + Environment.NewLine);
                    string accept = "Save and Continue";
                    string reject = "Discard Changes";
                    string cancel = "Cancel Navigation";
                    bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                    if (result == true)
                    {
                        if (!SaveEditItem())
                        {
                            addItem = false;
                        }
                    }
                    else if (result == false)
                    {
                        CancelEditItem();
                    }
                    else
                    {
                        addItem = false;
                    }
                }
				else
				{
					CancelEditItem();
				}
			}

			if (addItem)
			{
				string xml = @"<name>New Item</name>
									<type></type>
									<magic></magic>
									<value></value>
									<detail></detail>
									<weight></weight>
									<ac></ac>
									<strength></strength>
									<stealth></stealth>
									<dmg1></dmg1>
									<dmg2></dmg2>
									<dmgType></dmgType>
									<property></property>
									<rarity></rarity>
									<range></range>
									<text></text>
									<modifier category=""""></modifier>
									<roll></roll>";

				ItemModel itemModel = _xmlImporter.GetItem(xml);

				_compendium.AddItem(itemModel);

                if (_itemSearchService.SearchInputApplies(_itemSearchInput, itemModel))
                {
                    ItemListItemViewModel listItem = new ItemListItemViewModel(itemModel, _stringService);
                    _items.Add(listItem);
                    foreach (ItemListItemViewModel item in _items)
                    {
                        item.IsSelected = false;
                    }
                    listItem.IsSelected = true;
                }

				_selectedItem = new ItemViewModel(itemModel);

				_editItemXML = itemModel.XML;

				SortItems();

                _compendium.SaveItems();

                OnPropertyChanged(nameof(EditingItemXML));
				OnPropertyChanged(nameof(IsEditingItem));
				OnPropertyChanged(nameof(SelectedItem));
			}
		}

		private void Copy()
		{
			if (_selectedItem != null)
			{
				bool copyItem = true;

				if (_editItemXML != null)
				{
					if (_editHasUnsavedChanges)
					{
						string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
															 _selectedItem.Name, Environment.NewLine + Environment.NewLine);
                        string accept = "Save and Continue";
                        string reject = "Discard Changes";
                        string cancel = "Cancel Navigation";
                        bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                        if (result == true)
                        {
                            if (!SaveEditItem())
                            {
                                copyItem = false;
                            }
                        }
                        else if (result == false)
                        {
                            CancelEditItem();
                        }
                        else
                        {
                            copyItem = false;
                        }
                    }
					else
					{
						CancelEditItem();
					}
				}

				if (copyItem)
				{
					ItemModel newItem = new ItemModel(_selectedItem.ItemModel);
					newItem.Name += " (copy)";
					newItem.ID = Guid.NewGuid();
					newItem.XML = newItem.XML.Replace("<name>" + _selectedItem.ItemModel.Name + "</name>",
																 "<name>" + newItem.Name + "</name>");

					_compendium.AddItem(newItem);

                    if (_itemSearchService.SearchInputApplies(_itemSearchInput, newItem))
                    {
                        ItemListItemViewModel listItem = new ItemListItemViewModel(newItem, _stringService);
                        _items.Add(listItem);
                        foreach (ItemListItemViewModel item in _items)
                        {
                            item.IsSelected = false;
                        }
                        listItem.IsSelected = true;
                    }

					_selectedItem = new ItemViewModel(newItem);

					SortItems();

                    _compendium.SaveItems();

                    OnPropertyChanged(nameof(SelectedItem));
				}
			}
		}

		private void Delete()
		{
			if (_selectedItem != null)
			{
                bool canDelete = true;

                foreach (CharacterModel character in _compendium.Characters)
                {
                    foreach (BagModel bag in character.Bags)
                    {
                        foreach (EquipmentModel equipment in bag.Equipment)
                        {
                            if (equipment.Item != null && equipment.Item.ID == _selectedItem.ItemModel.ID)
                            {
                                canDelete = false;
                                break;
                            }
                        }

                        if (!canDelete)
                        {
                            break;
                        }
                    }

                    if (!canDelete)
                    {
                        break;
                    }
                }

                if (canDelete)
                {
                    string message = String.Format("Are you sure you want to delete {0}?",
                                                                 _selectedItem.Name);

                    bool? result = _dialogService.ShowConfirmationDialog("Delete Item", message, "Yes", "No", null);

                    if (result == true)
                    {
                        _compendium.DeleteItem(_selectedItem.ItemModel.ID);

                        ItemListItemViewModel listItem = _items.FirstOrDefault(x => x.ItemModel.ID == _selectedItem.ItemModel.ID);
                        if (listItem != null)
                        {
                            _items.Remove(listItem);
                        }

                        _selectedItem = null;

                        _compendium.SaveItems();

                        OnPropertyChanged(nameof(SelectedItem));

                        if (_editItemXML != null)
                        {
                            CancelEditItem();
                        }
                    }
                }
                else
                {
                    _dialogService.ShowConfirmationDialog("Unable Delete Item", "Item is in use by a character.", "OK", null, null);
                }
			}
		}

		private void SortItems()
		{
			if (_items != null && _items.Count > 0)
			{
                List<ItemModel> items = _itemSearchService.Sort(_items.Select(x => x.ItemModel), _itemSearchInput.SortOption.Key);
                for (int i = 0; i < items.Count; ++i)
                {
                    if (items[i].ID != _items[i].ItemModel.ID)
                    {
                        ItemListItemViewModel item = _items.FirstOrDefault(x => x.ItemModel.ID == items[i].ID);
                        if (item != null)
                        {
                            _items.Move(_items.IndexOf(item), i);
                        }
                    }
                }
			}
        }

        private void ExportItem(ItemViewModel itemViewModel)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Word Document|*.docx|XML Document|*.xml";
            saveFileDialog.Title = "Save Item";
            saveFileDialog.FileName = itemViewModel.Name;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string ext = System.IO.Path.GetExtension(saveFileDialog.FileName);

                    if (ext == ".docx")
                    {
                        _documentService.CreateWordDoc(saveFileDialog.FileName, itemViewModel.ItemModel);
                    }
                    else if (ext == ".xml")
                    {
                        string xml = _xmlExporter.FormatXMLWithHeader(_xmlExporter.GetXML(itemViewModel.ItemModel));
                        System.IO.File.WriteAllText(saveFileDialog.FileName, xml, System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        _dialogService.ShowConfirmationDialog("Unable To Export", "Invalid file extension.", "OK", null, null);
                    }
                }
                catch (Exception)
                {
                    _dialogService.ShowConfirmationDialog("Unable To Export", "An error occurred when attempting to export the item.", "OK", null, null);
                }
            }
        }

        private void Import()
        {
            _dialogService.ShowImportView();
        }

        private void SelectNext()
        {
            if (_items.Any())
            {
                ItemListItemViewModel selected = _items.FirstOrDefault(x => x.IsSelected);

                foreach (ItemListItemViewModel item in _items)
                {
                    item.IsSelected = false;
                }

                if (selected == null)
                {
                    _items[0].IsSelected = true;
                    _selectedItem = new ItemViewModel(_items[0].ItemModel);
                }
                else
                {
                    int index = Math.Min(_items.IndexOf(selected) + 1, _items.Count - 1);
                    _items[index].IsSelected = true;
                    _selectedItem = new ItemViewModel(_items[index].ItemModel);
                }

                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private void SelectPrevious()
        {
            if (_items.Any())
            {
                ItemListItemViewModel selected = _items.FirstOrDefault(x => x.IsSelected);

                foreach (ItemListItemViewModel item in _items)
                {
                    item.IsSelected = false;
                }

                if (selected == null)
                {
                    _items[_items.Count - 1].IsSelected = true;
                    _selectedItem = new ItemViewModel(_items[_items.Count - 1].ItemModel);
                }
                else
                {
                    int index = Math.Max(_items.IndexOf(selected)  - 1, 0);
                    _items[index].IsSelected = true;
                    _selectedItem = new ItemViewModel(_items[index].ItemModel);
                }

                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        #endregion
    }
}
