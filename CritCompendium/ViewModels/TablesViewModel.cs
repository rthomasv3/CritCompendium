using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
   public sealed class TablesViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly Compendium _compendium;
      private readonly TableSearchService _tableSearchService;
      private readonly TableSearchInput _tableSearchInput;
      private readonly StringService _stringService;
      private readonly DialogService _dialogService;
      private readonly XMLImporter _xmlImporter;
      private readonly DocumentService _documentService;
      private readonly DataManager _dataManager;
      private readonly ObservableCollection<ListItemViewModel<RandomTableModel>> _tables = new ObservableCollection<ListItemViewModel<RandomTableModel>>();
      private readonly ICommand _selectTableCommand;
      private readonly ICommand _editTableCommand;
      private readonly ICommand _exportTableCommand;
      private readonly ICommand _cancelEditTableCommand;
      private readonly ICommand _saveEditTableCommand;
      private readonly ICommand _resetFiltersCommand;
      private readonly ICommand _addCommand;
      private readonly ICommand _copyCommand;
      private readonly ICommand _deleteCommand;
      private readonly ICommand _importCommand;
      private readonly ICommand _selectNextCommand;
      private readonly ICommand _selectPreviousCommand;
      private RandomTableViewModel _selectedTable;
      private RandomTableEditViewModel _tableEditViewModel;
      private bool _editHasUnsavedChanges;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates an instance of <see cref="TablesViewModel"/>
      /// </summary>
      public TablesViewModel(Compendium compendium, TableSearchService tableSearchService, TableSearchInput tableSearchInput,
          StringService stringService, DialogService dialogService, XMLImporter xmlImporter, DocumentService documentService, DataManager dataManager)
      {
         _compendium = compendium;
         _tableSearchService = tableSearchService;
         _tableSearchInput = tableSearchInput;
         _stringService = stringService;
         _dialogService = dialogService;
         _xmlImporter = xmlImporter;
         _documentService = documentService;
         _dataManager = dataManager;

         _selectTableCommand = new RelayCommand(obj => true, obj => SelectTable(obj as ListItemViewModel<RandomTableModel>));
         _editTableCommand = new RelayCommand(obj => true, obj => EditTable(obj as RandomTableViewModel));
         _exportTableCommand = new RelayCommand(obj => true, obj => ExportTable(obj as RandomTableViewModel));
         _cancelEditTableCommand = new RelayCommand(obj => true, obj => CancelEditTable());
         _saveEditTableCommand = new RelayCommand(obj => HasUnsavedChanges, obj => SaveEditTable());
         _resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
         _addCommand = new RelayCommand(obj => true, obj => Add());
         _copyCommand = new RelayCommand(obj => _selectedTable != null, obj => Copy());
         _deleteCommand = new RelayCommand(obj => _selectedTable != null, obj => Delete());
         _importCommand = new RelayCommand(obj => true, obj => Import());
         _selectNextCommand = new RelayCommand(obj => true, obj => SelectNext());
         _selectPreviousCommand = new RelayCommand(obj => true, obj => SelectPrevious());

         Search();
      }

      #endregion

      #region Properties

      /// <summary>
      /// List of tables
      /// </summary>
      public ObservableCollection<ListItemViewModel<RandomTableModel>> Tables
      {
         get { return _tables; }
      }

      /// <summary>
      /// Gets or sets the search text
      /// </summary>
      public string SearchText
      {
         get { return _tableSearchInput.SearchText; }
         set
         {
            _tableSearchInput.SearchText = value;
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
            return _tableSearchInput.AppliedFilterCount > 0 ? $"Sort and Filter ({_tableSearchInput.AppliedFilterCount})" : "Sort and Filter";
         }
      }

      /// <summary>
      /// Gets or sets sort and filters expanded
      /// </summary>
      public bool SortAndFiltersExpanded
      {
         get { return _tableSearchInput.SortAndFiltersExpanded; }
         set { _tableSearchInput.SortAndFiltersExpanded = value; }
      }

      /// <summary>
      /// Gets sort options
      /// </summary>
      public IEnumerable<KeyValuePair<RandomTableSortOption, string>> SortOptions
      {
         get { return _tableSearchInput.SortOptions; }
      }

      /// <summary>
      /// Gets selected sort option
      /// </summary>
      public KeyValuePair<RandomTableSortOption, string> SelectedSortOption
      {
         get { return _tableSearchInput.SortOption; }
         set
         {
            _tableSearchInput.SortOption = value;
            Search();
         }
      }

      /// <summary>
      /// Gets tag options
      /// </summary>
      public IEnumerable<KeyValuePair<string, string>> TagOptions
      {
         get { return _tableSearchInput.TagOptions; }
      }

      /// <summary>
      /// Gets selected tag option
      /// </summary>
      public KeyValuePair<string, string> SelectedTagOption
      {
         get { return _tableSearchInput.Tag; }
         set
         {
            _tableSearchInput.Tag = value;
            Search();
         }
      }

      /// <summary>
      /// Command to select a table
      /// </summary>
      public ICommand SelectTableCommand
      {
         get { return _selectTableCommand; }
      }

      /// <summary>
      /// Selected table
      /// </summary>
      public RandomTableViewModel SelectedTable
      {
         get { return _selectedTable; }
      }

      /// <summary>
      /// Editing table
      /// </summary>
      public RandomTableEditViewModel EditingTable
      {
         get { return _tableEditViewModel; }
      }

      /// <summary>
      /// Command to reset filters
      /// </summary>
      public ICommand ResetFiltersCommand
      {
         get { return _resetFiltersCommand; }
      }

      /// <summary>
      /// Command to edit table
      /// </summary>
      public ICommand EditTableCommand
      {
         get { return _editTableCommand; }
      }

      /// <summary>
      /// Command to export table
      /// </summary>
      public ICommand ExportTableCommand
      {
         get { return _exportTableCommand; }
      }

      /// <summary>
      /// Command to cancel edit table
      /// </summary>
      public ICommand CancelEditTableCommand
      {
         get { return _cancelEditTableCommand; }
      }

      /// <summary>
      /// Command to save edit table
      /// </summary>
      public ICommand SaveEditTableCommand
      {
         get { return _saveEditTableCommand; }
      }

      /// <summary>
      /// Command to add table
      /// </summary>
      public ICommand AddTableCommand
      {
         get { return _addCommand; }
      }

      /// <summary>
      /// Command to copy table
      /// </summary>
      public ICommand CopyTableCommand
      {
         get { return _copyCommand; }
      }

      /// <summary>
      /// Command to delete selected table
      /// </summary>
      public ICommand DeleteTableCommand
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
      /// True if currently editing a table
      /// </summary>
      public bool IsEditingTable
      {
         get { return _tableEditViewModel != null; }
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
         _tables.Clear();
         foreach (RandomTableModel tableModel in _tableSearchService.Search(_tableSearchInput))
         {
            ListItemViewModel<RandomTableModel> listItem = new ListItemViewModel<RandomTableModel>(tableModel);
            InitializeListItemDetails(listItem, tableModel);
            _tables.Add(listItem);
         }

         if (_selectedTable != null)
         {
            ListItemViewModel<RandomTableModel> table = _tables.FirstOrDefault(x => x.Model.ID == _selectedTable.RandomTableModel.ID);
            if (table != null)
            {
               table.IsSelected = true;
            }
         }
      }

      #endregion

      #region Private Methods

      private void InitializeSearch()
      {
         _tableSearchInput.Reset();

         OnPropertyChanged(nameof(SearchText));

         Search();
      }

      private void SelectTable(ListItemViewModel<RandomTableModel> tableItem)
      {
         bool selectTable = true;

         if (_tableEditViewModel != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                            _selectedTable.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditTable())
                  {
                     selectTable = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditTable();
               }
               else
               {
                  selectTable = false;
               }
            }
            else
            {
               CancelEditTable();
            }
         }

         if (selectTable)
         {
            foreach (ListItemViewModel<RandomTableModel> item in _tables)
            {
               item.IsSelected = false;
            }
            tableItem.IsSelected = true;

            _selectedTable = new RandomTableViewModel(tableItem.Model);
            OnPropertyChanged(nameof(SelectedTable));
         }
      }

      private void EditTable(RandomTableViewModel tableModel)
      {
         if (tableModel != null)
         {
            _tableEditViewModel = new RandomTableEditViewModel(tableModel.RandomTableModel);
            _tableEditViewModel.PropertyChanged += _tableEditViewModel_PropertyChanged;

            OnPropertyChanged(nameof(EditingTable));
            OnPropertyChanged(nameof(IsEditingTable));
         }
      }

      private void CancelEditTable()
      {
         _editHasUnsavedChanges = false;
         _tableEditViewModel = null;

         OnPropertyChanged(nameof(EditingTable));
         OnPropertyChanged(nameof(IsEditingTable));
         OnPropertyChanged(nameof(HasUnsavedChanges));
      }

      private bool SaveEditTable()
      {
         bool saved = false;

         if (_tableEditViewModel.RandomTableModel != null)
         {
            _tableEditViewModel.RandomTableModel.ID = _selectedTable.RandomTableModel.ID;
            _compendium.UpdateTable(_tableEditViewModel.RandomTableModel);

            _selectedTable = new RandomTableViewModel(_tableEditViewModel.RandomTableModel);

            ListItemViewModel<RandomTableModel> oldListItem = _tables.FirstOrDefault(x => x.Model.ID == _tableEditViewModel.RandomTableModel.ID);
            if (oldListItem != null)
            {
               if (_tableSearchService.SearchInputApplies(_tableSearchInput, _tableEditViewModel.RandomTableModel))
               {
                  InitializeListItemDetails(oldListItem, _tableEditViewModel.RandomTableModel);
               }
               else
               {
                  _tables.Remove(oldListItem);
               }
            }

            _tableEditViewModel = null;
            _editHasUnsavedChanges = false;

            SortTables();

            _compendium.SaveTables();

            OnPropertyChanged(nameof(SelectedTagOption));
            OnPropertyChanged(nameof(SelectedTable));
            OnPropertyChanged(nameof(EditingTable));
            OnPropertyChanged(nameof(IsEditingTable));
            OnPropertyChanged(nameof(HasUnsavedChanges));

            saved = true;
         }

         return saved;
      }

      private void Add()
      {
         bool addTable = true;

         if (_tableEditViewModel != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                                    _selectedTable.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditTable())
                  {
                     addTable = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditTable();
               }
               else
               {
                  addTable = false;
               }
            }
            else
            {
               CancelEditTable();
            }
         }

         if (addTable)
         {
            RandomTableModel tableModel = new RandomTableModel();

            _compendium.AddTable(tableModel);

            if (_tableSearchService.SearchInputApplies(_tableSearchInput, tableModel))
            {
               ListItemViewModel<RandomTableModel> listItem = new ListItemViewModel<RandomTableModel>(tableModel);
               InitializeListItemDetails(listItem, tableModel);
               _tables.Add(listItem);
               foreach (ListItemViewModel<RandomTableModel> item in _tables)
               {
                  item.IsSelected = false;
               }
               listItem.IsSelected = true;
            }

            _selectedTable = new RandomTableViewModel(tableModel);

            _tableEditViewModel = new RandomTableEditViewModel(tableModel);
            _tableEditViewModel.PropertyChanged += _tableEditViewModel_PropertyChanged;

            SortTables();

            _compendium.SaveTables();

            OnPropertyChanged(nameof(SelectedTagOption));
            OnPropertyChanged(nameof(EditingTable));
            OnPropertyChanged(nameof(IsEditingTable));
            OnPropertyChanged(nameof(SelectedTable));
         }
      }

      private void Copy()
      {
         if (_selectedTable != null)
         {
            bool copyTable = true;

            if (_tableEditViewModel != null)
            {
               if (_editHasUnsavedChanges)
               {
                  string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                               _selectedTable.Name, Environment.NewLine + Environment.NewLine);
                  string accept = "Save and Continue";
                  string reject = "Discard Changes";
                  string cancel = "Cancel Navigation";
                  bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                  if (result == true)
                  {
                     if (!SaveEditTable())
                     {
                        copyTable = false;
                     }
                  }
                  else if (result == false)
                  {
                     CancelEditTable();
                  }
                  else
                  {
                     copyTable = false;
                  }
               }
               else
               {
                  CancelEditTable();
               }
            }

            if (copyTable)
            {
               RandomTableModel tableModel = new RandomTableModel(_selectedTable.RandomTableModel);
               tableModel.Name += " (copy)";
               tableModel.ID = Guid.NewGuid();

               _compendium.AddTable(tableModel);

               if (_tableSearchService.SearchInputApplies(_tableSearchInput, tableModel))
               {
                  ListItemViewModel<RandomTableModel> listItem = new ListItemViewModel<RandomTableModel>(tableModel);
                  InitializeListItemDetails(listItem, tableModel);
                  _tables.Add(listItem);
                  foreach (ListItemViewModel<RandomTableModel> item in _tables)
                  {
                     item.IsSelected = false;
                  }
                  listItem.IsSelected = true;
               }

               _selectedTable = new RandomTableViewModel(tableModel);

               SortTables();

               _compendium.SaveTables();

               OnPropertyChanged(nameof(SelectedTable));
            }
         }
      }

      private void Delete()
      {
         if (_selectedTable != null)
         {
            string message = String.Format("Are you sure you want to delete {0}?",
                                                     _selectedTable.Name);

            bool? result = _dialogService.ShowConfirmationDialog("Delete Table", message, "Yes", "No", null);

            if (result == true)
            {
               _compendium.DeleteTable(_selectedTable.RandomTableModel.ID);

               ListItemViewModel<RandomTableModel> listItem = _tables.FirstOrDefault(x => x.Model.ID == _selectedTable.RandomTableModel.ID);
               if (listItem != null)
               {
                  _tables.Remove(listItem);
               }

               _selectedTable = null;

               _compendium.SaveTables();

               OnPropertyChanged(nameof(SelectedTagOption));
               OnPropertyChanged(nameof(SelectedTable));

               if (_tableEditViewModel != null)
               {
                  CancelEditTable();
               }
            }
         }
      }

      private void SortTables()
      {
         if (_tables != null && _tables.Count > 0)
         {
            List<RandomTableModel> tables = _tableSearchService.Sort(_tables.Select(x => x.Model), _tableSearchInput.SortOption.Key);
            for (int i = 0; i < tables.Count; ++i)
            {
               if (tables[i].ID != _tables[i].Model.ID)
               {
                  ListItemViewModel<RandomTableModel> table = _tables.FirstOrDefault(x => x.Model.ID == tables[i].ID);
                  if (table != null)
                  {
                     _tables.Move(_tables.IndexOf(table), i);
                  }
               }
            }

            List<ListItemViewModel<RandomTableModel>> sorted = _tables.OrderBy(x => x.Name).ToList();

            for (int i = 0; i < sorted.Count; ++i)
            {
               if (!sorted[i].Equals(_tables[i]))
               {
                  _tables.Move(_tables.IndexOf(sorted[i]), i);
               }
            }
         }
      }

      private void _tableEditViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (!_editHasUnsavedChanges)
         {
            _editHasUnsavedChanges = true;
         }
      }

      private void ExportTable(RandomTableViewModel tableViewModel)
      {
         Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
         saveFileDialog.Filter = "Table Archive|*.ccea|Word Document|*.docx";
         saveFileDialog.Title = "Save Table";
         saveFileDialog.FileName = tableViewModel.Name;

         if (saveFileDialog.ShowDialog() == true)
         {
            try
            {
               string ext = Path.GetExtension(saveFileDialog.FileName);

               if (ext == ".ccaa")
               {
                  byte[] bytes = _dataManager.CreateTableArchive(tableViewModel.RandomTableModel);
                  File.WriteAllBytes(saveFileDialog.FileName, bytes);
               }
               else if (ext == "*.docx")
               {
                  //_documentService.CreateWordDoc(saveFileDialog.FileName, tableViewModel);
               }
               else
               {
                  _dialogService.ShowConfirmationDialog("Unable To Export", "Invalid file extension.", "OK", null, null);
               }
            }
            catch (Exception)
            {
               _dialogService.ShowConfirmationDialog("Unable To Export", "An error occurred when attempting to export the table.", "OK", null, null);
            }
         }
      }

      private void InitializeListItemDetails(ListItemViewModel<RandomTableModel> listItem, RandomTableModel tableModel)
      {
         listItem.Model = tableModel;

         if (!String.IsNullOrWhiteSpace(tableModel.Name))
         {
            listItem.Name = tableModel.Name;
         }
         else
         {
            listItem.Name = "Unknown Name";
         }

         if (!String.IsNullOrWhiteSpace(tableModel.Die) && !String.IsNullOrWhiteSpace(tableModel.Header))
         {
            listItem.Description = $"{tableModel.Die}, {tableModel.Header}";
         }
         else if (String.IsNullOrWhiteSpace(tableModel.Die) && !String.IsNullOrWhiteSpace(tableModel.Header))
         {
            listItem.Description = $"Unknown Die, {tableModel.Header}";
         }
         else if (!String.IsNullOrWhiteSpace(tableModel.Die) && String.IsNullOrWhiteSpace(tableModel.Header))
         {
            listItem.Description = $"{tableModel.Die}, Unknown Header";
         }
         else
         {
            listItem.Description = "Unknown description";
         }
      }

      private void Import()
      {
         _dialogService.ShowImportView();
      }

      private void SelectNext()
      {
         if (_tables.Any())
         {
            ListItemViewModel<RandomTableModel> selected = _tables.FirstOrDefault(x => x.IsSelected);

            foreach (ListItemViewModel<RandomTableModel> table in _tables)
            {
               table.IsSelected = false;
            }

            if (selected == null)
            {
               _tables[0].IsSelected = true;
               _selectedTable = new RandomTableViewModel(_tables[0].Model);
            }
            else
            {
               int index = Math.Min(_tables.IndexOf(selected) + 1, _tables.Count - 1);
               _tables[index].IsSelected = true;
               _selectedTable = new RandomTableViewModel(_tables[index].Model);
            }

            OnPropertyChanged(nameof(SelectedTable));
         }
      }

      private void SelectPrevious()
      {
         if (_tables.Any())
         {
            ListItemViewModel<RandomTableModel> selected = _tables.FirstOrDefault(x => x.IsSelected);

            foreach (ListItemViewModel<RandomTableModel> table in _tables)
            {
               table.IsSelected = false;
            }

            if (selected == null)
            {
               _tables[_tables.Count - 1].IsSelected = true;
               _selectedTable = new RandomTableViewModel(_tables[_tables.Count - 1].Model);
            }
            else
            {
               int index = Math.Max(_tables.IndexOf(selected) - 1, 0);
               _tables[index].IsSelected = true;
               _selectedTable = new RandomTableViewModel(_tables[index].Model);
            }

            OnPropertyChanged(nameof(SelectedTable));
         }
      }

      #endregion
   }
}
