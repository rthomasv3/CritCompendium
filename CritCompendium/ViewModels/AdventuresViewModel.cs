using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Persistence;
using CritCompendiumInfrastructure.Services;
using CritCompendiumInfrastructure.Services.Search;
using CritCompendiumInfrastructure.Services.Search.Input;

namespace CritCompendium.ViewModels
{
   public sealed class AdventuresViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly Compendium _compendium;
      private readonly AdventureSearchService _adventureSearchService;
      private readonly AdventureSearchInput _adventureSearchInput;
      private readonly StringService _stringService;
      private readonly DialogService _dialogService;
      private readonly XMLImporter _xmlImporter;
      private readonly DocumentService _documentService;
      private readonly DataManager _dataManager;
      private readonly ObservableCollection<ListItemViewModel<AdventureModel>> _adventures = new ObservableCollection<ListItemViewModel<AdventureModel>>();
      private readonly ICommand _selectAdventureCommand;
      private readonly ICommand _editAdventureCommand;
      private readonly ICommand _exportAdventureCommand;
      private readonly ICommand _cancelEditAdventureCommand;
      private readonly ICommand _saveEditAdventureCommand;
      private readonly ICommand _resetFiltersCommand;
      private readonly ICommand _addCommand;
      private readonly ICommand _copyCommand;
      private readonly ICommand _deleteCommand;
      private readonly ICommand _importCommand;
      private readonly ICommand _selectNextCommand;
      private readonly ICommand _selectPreviousCommand;
      private AdventureViewModel _selectedAdventure;
      private AdventureEditViewModel _adventureEditViewModel;
      private bool _editHasUnsavedChanges;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates an instance of <see cref="AdventuresViewModel"/>
      /// </summary>
      public AdventuresViewModel(Compendium compendium, AdventureSearchService adventureSearchService, AdventureSearchInput adventureSearchInput,
          StringService stringService, DialogService dialogService, XMLImporter xmlImporter, DocumentService documentService, DataManager dataManager)
      {
         _compendium = compendium;
         _adventureSearchService = adventureSearchService;
         _adventureSearchInput = adventureSearchInput;
         _stringService = stringService;
         _dialogService = dialogService;
         _xmlImporter = xmlImporter;
         _documentService = documentService;
         _dataManager = dataManager;

         _selectAdventureCommand = new RelayCommand(obj => true, obj => SelectAdventure(obj as ListItemViewModel<AdventureModel>));
         _editAdventureCommand = new RelayCommand(obj => true, obj => EditAdventure(obj as AdventureViewModel));
         _exportAdventureCommand = new RelayCommand(obj => true, obj => ExportAdventure(obj as AdventureViewModel));
         _cancelEditAdventureCommand = new RelayCommand(obj => true, obj => CancelEditAdventure());
         _saveEditAdventureCommand = new RelayCommand(obj => HasUnsavedChanges, obj => SaveEditAdventure());
         _resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
         _addCommand = new RelayCommand(obj => true, obj => Add());
         _copyCommand = new RelayCommand(obj => _selectedAdventure != null, obj => Copy());
         _deleteCommand = new RelayCommand(obj => _selectedAdventure != null, obj => Delete());
         _importCommand = new RelayCommand(obj => true, obj => Import());
         _selectNextCommand = new RelayCommand(obj => true, obj => SelectNext());
         _selectPreviousCommand = new RelayCommand(obj => true, obj => SelectPrevious());

         Search();
      }

      #endregion

      #region Properties

      /// <summary>
      /// List of adventures
      /// </summary>
      public ObservableCollection<ListItemViewModel<AdventureModel>> Adventures
      {
         get { return _adventures; }
      }

      /// <summary>
      /// Gets or sets the search text
      /// </summary>
      public string SearchText
      {
         get { return _adventureSearchInput.SearchText; }
         set
         {
            _adventureSearchInput.SearchText = value;
            Search();
         }
      }

      /// <summary>
      /// Command to select a adventure
      /// </summary>
      public ICommand SelectAdventureCommand
      {
         get { return _selectAdventureCommand; }
      }

      /// <summary>
      /// Selected adventure
      /// </summary>
      public AdventureViewModel SelectedAdventure
      {
         get { return _selectedAdventure; }
      }

      /// <summary>
      /// Editing adventure
      /// </summary>
      public AdventureEditViewModel EditingAdventure
      {
         get { return _adventureEditViewModel; }
      }

      /// <summary>
      /// Command to reset filters
      /// </summary>
      public ICommand ResetFiltersCommand
      {
         get { return _resetFiltersCommand; }
      }

      /// <summary>
      /// Command to edit adventure
      /// </summary>
      public ICommand EditAdventureCommand
      {
         get { return _editAdventureCommand; }
      }

      /// <summary>
      /// Command to export adventure
      /// </summary>
      public ICommand ExportAdventureCommand
      {
         get { return _exportAdventureCommand; }
      }

      /// <summary>
      /// Command to cancel edit adventure
      /// </summary>
      public ICommand CancelEditAdventureCommand
      {
         get { return _cancelEditAdventureCommand; }
      }

      /// <summary>
      /// Command to save edit adventure
      /// </summary>
      public ICommand SaveEditAdventureCommand
      {
         get { return _saveEditAdventureCommand; }
      }

      /// <summary>
      /// Command to add adventure
      /// </summary>
      public ICommand AddAdventureCommand
      {
         get { return _addCommand; }
      }

      /// <summary>
      /// Command to copy adventure
      /// </summary>
      public ICommand CopyAdventureCommand
      {
         get { return _copyCommand; }
      }

      /// <summary>
      /// Command to delete selected adventure
      /// </summary>
      public ICommand DeleteAdventureCommand
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
      /// True if currently editing a adventure
      /// </summary>
      public bool IsEditingAdventure
      {
         get { return _adventureEditViewModel != null; }
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
         _adventures.Clear();
         foreach (AdventureModel adventureModel in _adventureSearchService.Search(_adventureSearchInput))
         {
            _adventures.Add(new ListItemViewModel<AdventureModel>(adventureModel));
         }

         if (_selectedAdventure != null)
         {
            ListItemViewModel<AdventureModel> adventure = _adventures.FirstOrDefault(x => x.Model.ID == _selectedAdventure.AdventureModel.ID);
            if (adventure != null)
            {
               adventure.IsSelected = true;
            }
         }
      }

      #endregion

      #region Private Methods

      private void InitializeSearch()
      {
         _adventureSearchInput.Reset();

         OnPropertyChanged(nameof(SearchText));

         Search();
      }

      private void SelectAdventure(ListItemViewModel<AdventureModel> adventureItem)
      {
         bool selectAdventure = true;

         if (_adventureEditViewModel != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                            _selectedAdventure.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditAdventure())
                  {
                     selectAdventure = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditAdventure();
               }
               else
               {
                  selectAdventure = false;
               }
            }
            else
            {
               CancelEditAdventure();
            }
         }

         if (selectAdventure)
         {
            foreach (ListItemViewModel<AdventureModel> item in _adventures)
            {
               item.IsSelected = false;
            }
            adventureItem.IsSelected = true;

            _selectedAdventure = new AdventureViewModel(adventureItem.Model);
            OnPropertyChanged(nameof(SelectedAdventure));
         }
      }

      private void EditAdventure(AdventureViewModel adventureModel)
      {
         if (adventureModel != null)
         {
            _adventureEditViewModel = new AdventureEditViewModel(adventureModel.AdventureModel);
            _adventureEditViewModel.PropertyChanged += _adventureEditViewModel_PropertyChanged;

            OnPropertyChanged(nameof(EditingAdventure));
            OnPropertyChanged(nameof(IsEditingAdventure));
         }
      }

      private void CancelEditAdventure()
      {
         _editHasUnsavedChanges = false;
         _adventureEditViewModel = null;

         OnPropertyChanged(nameof(EditingAdventure));
         OnPropertyChanged(nameof(IsEditingAdventure));
         OnPropertyChanged(nameof(HasUnsavedChanges));
      }

      private bool SaveEditAdventure()
      {
         bool saved = false;

         if (_adventureEditViewModel.AdventureModel != null)
         {
            _adventureEditViewModel.AdventureModel.ID = _selectedAdventure.AdventureModel.ID;
            _compendium.UpdateAdventure(_adventureEditViewModel.AdventureModel);

            _selectedAdventure = new AdventureViewModel(_adventureEditViewModel.AdventureModel);

            ListItemViewModel<AdventureModel> oldListItem = _adventures.FirstOrDefault(x => x.Model.ID == _adventureEditViewModel.AdventureModel.ID);
            if (oldListItem != null)
            {
               if (_adventureSearchService.SearchInputApplies(_adventureSearchInput, _adventureEditViewModel.AdventureModel))
               {
                  InitializeListItemDetails(oldListItem, _adventureEditViewModel.AdventureModel);
               }
               else
               {
                  _adventures.Remove(oldListItem);
               }
            }

            _adventureEditViewModel = null;
            _editHasUnsavedChanges = false;

            SortAdventures();

            _compendium.SaveAdventures();

            OnPropertyChanged(nameof(SelectedAdventure));
            OnPropertyChanged(nameof(EditingAdventure));
            OnPropertyChanged(nameof(IsEditingAdventure));
            OnPropertyChanged(nameof(HasUnsavedChanges));

            saved = true;
         }

         return saved;
      }

      private void Add()
      {
         bool addAdventure = true;

         if (_adventureEditViewModel != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                                    _selectedAdventure.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditAdventure())
                  {
                     addAdventure = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditAdventure();
               }
               else
               {
                  addAdventure = false;
               }
            }
            else
            {
               CancelEditAdventure();
            }
         }

         if (addAdventure)
         {
            AdventureModel adventureModel = new AdventureModel();
            adventureModel.Name = "New Adventure";

            _compendium.AddAdventure(adventureModel);

            if (_adventureSearchService.SearchInputApplies(_adventureSearchInput, adventureModel))
            {
               ListItemViewModel<AdventureModel> listItem = new ListItemViewModel<AdventureModel>(adventureModel);
               InitializeListItemDetails(listItem, adventureModel);
               _adventures.Add(listItem);
               foreach (ListItemViewModel<AdventureModel> item in _adventures)
               {
                  item.IsSelected = false;
               }
               listItem.IsSelected = true;
            }

            _selectedAdventure = new AdventureViewModel(adventureModel);

            _adventureEditViewModel = new AdventureEditViewModel(adventureModel);
            _adventureEditViewModel.PropertyChanged += _adventureEditViewModel_PropertyChanged;

            SortAdventures();

            _compendium.SaveAdventures();

            OnPropertyChanged(nameof(EditingAdventure));
            OnPropertyChanged(nameof(IsEditingAdventure));
            OnPropertyChanged(nameof(SelectedAdventure));
         }
      }

      private void Copy()
      {
         if (_selectedAdventure != null)
         {
            bool copyAdventure = true;

            if (_adventureEditViewModel != null)
            {
               if (_editHasUnsavedChanges)
               {
                  string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                               _selectedAdventure.Name, Environment.NewLine + Environment.NewLine);
                  string accept = "Save and Continue";
                  string reject = "Discard Changes";
                  string cancel = "Cancel Navigation";
                  bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                  if (result == true)
                  {
                     if (!SaveEditAdventure())
                     {
                        copyAdventure = false;
                     }
                  }
                  else if (result == false)
                  {
                     CancelEditAdventure();
                  }
                  else
                  {
                     copyAdventure = false;
                  }
               }
               else
               {
                  CancelEditAdventure();
               }
            }

            if (copyAdventure)
            {
               AdventureModel adventureModel = new AdventureModel(_selectedAdventure.AdventureModel);
               adventureModel.Name += " (copy)";
               adventureModel.ID = Guid.NewGuid();

               _compendium.AddAdventure(adventureModel);

               if (_adventureSearchService.SearchInputApplies(_adventureSearchInput, adventureModel))
               {
                  ListItemViewModel<AdventureModel> listItem = new ListItemViewModel<AdventureModel>(adventureModel);
                  InitializeListItemDetails(listItem, adventureModel);
                  _adventures.Add(listItem);
                  foreach (ListItemViewModel<AdventureModel> item in _adventures)
                  {
                     item.IsSelected = false;
                  }
                  listItem.IsSelected = true;
               }

               _selectedAdventure = new AdventureViewModel(adventureModel);

               SortAdventures();

               _compendium.SaveAdventures();

               OnPropertyChanged(nameof(SelectedAdventure));
            }
         }
      }

      private void Delete()
      {
         if (_selectedAdventure != null)
         {
            string message = String.Format("Are you sure you want to delete {0}?",
                                                     _selectedAdventure.Name);

            bool? result = _dialogService.ShowConfirmationDialog("Delete Adventure", message, "Yes", "No", null);

            if (result == true)
            {
               _compendium.DeleteAdventure(_selectedAdventure.AdventureModel.ID);

               ListItemViewModel<AdventureModel> listItem = _adventures.FirstOrDefault(x => x.Model.ID == _selectedAdventure.AdventureModel.ID);
               if (listItem != null)
               {
                  _adventures.Remove(listItem);
               }

               _selectedAdventure = null;

               _compendium.SaveAdventures();

               OnPropertyChanged(nameof(SelectedAdventure));

               if (_adventureEditViewModel != null)
               {
                  CancelEditAdventure();
               }
            }
         }
      }

      private void SortAdventures()
      {
         if (_adventures != null && _adventures.Count > 0)
         {
            List<AdventureModel> adventures = _adventureSearchService.Sort(_adventures.Select(x => x.Model), _adventureSearchInput.SortOption.Key);
            for (int i = 0; i < adventures.Count; ++i)
            {
               if (adventures[i].ID != _adventures[i].Model.ID)
               {
                  ListItemViewModel<AdventureModel> adventure = _adventures.FirstOrDefault(x => x.Model.ID == adventures[i].ID);
                  if (adventure != null)
                  {
                     _adventures.Move(_adventures.IndexOf(adventure), i);
                  }
               }
            }

            List<ListItemViewModel<AdventureModel>> sorted = _adventures.OrderBy(x => x.Name).ToList();

            for (int i = 0; i < sorted.Count; ++i)
            {
               if (!sorted[i].Equals(_adventures[i]))
               {
                  _adventures.Move(_adventures.IndexOf(sorted[i]), i);
               }
            }
         }
      }

      private void _adventureEditViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (!_editHasUnsavedChanges)
         {
            _editHasUnsavedChanges = true;
         }
      }

      private void ExportAdventure(AdventureViewModel adventureViewModel)
      {
         Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
         saveFileDialog.Filter = "Adventure Archive|*.ccea|Word Document|*.docx";
         saveFileDialog.Title = "Save Adventure";
         saveFileDialog.FileName = adventureViewModel.Name;

         if (saveFileDialog.ShowDialog() == true)
         {
            try
            {
               string ext = Path.GetExtension(saveFileDialog.FileName);

               if (ext == ".ccaa")
               {
                  byte[] bytes = _dataManager.CreateAdventureArchive(adventureViewModel.AdventureModel);
                  File.WriteAllBytes(saveFileDialog.FileName, bytes);
               }
               else if (ext == "*.docx")
               {
                  //_documentService.CreateWordDoc(saveFileDialog.FileName, adventureViewModel);
               }
               else
               {
                  _dialogService.ShowConfirmationDialog("Unable To Export", "Invalid file extension.", "OK", null, null);
               }
            }
            catch (Exception)
            {
               _dialogService.ShowConfirmationDialog("Unable To Export", "An error occurred when attempting to export the adventure.", "OK", null, null);
            }
         }
      }

      private void InitializeListItemDetails(ListItemViewModel<AdventureModel> listItem, AdventureModel adventureModel)
      {
         listItem.Model = adventureModel;

         if (!String.IsNullOrWhiteSpace(adventureModel.Name))
         {
            listItem.Name = adventureModel.Name;
         }
         else
         {
            listItem.Name = "Unknown Name";
         }

         if (!String.IsNullOrWhiteSpace(adventureModel.Introduction))
         {
            listItem.Description = new String(adventureModel.Introduction.Take(50).ToArray());
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
         if (_adventures.Any())
         {
            ListItemViewModel<AdventureModel> selected = _adventures.FirstOrDefault(x => x.IsSelected);

            foreach (ListItemViewModel<AdventureModel> adventure in _adventures)
            {
               adventure.IsSelected = false;
            }

            if (selected == null)
            {
               _adventures[0].IsSelected = true;
               _selectedAdventure = new AdventureViewModel(_adventures[0].Model);
            }
            else
            {
               int index = Math.Min(_adventures.IndexOf(selected) + 1, _adventures.Count - 1);
               _adventures[index].IsSelected = true;
               _selectedAdventure = new AdventureViewModel(_adventures[index].Model);
            }

            OnPropertyChanged(nameof(SelectedAdventure));
         }
      }

      private void SelectPrevious()
      {
         if (_adventures.Any())
         {
            ListItemViewModel<AdventureModel> selected = _adventures.FirstOrDefault(x => x.IsSelected);

            foreach (ListItemViewModel<AdventureModel> adventure in _adventures)
            {
               adventure.IsSelected = false;
            }

            if (selected == null)
            {
               _adventures[_adventures.Count - 1].IsSelected = true;
               _selectedAdventure = new AdventureViewModel(_adventures[_adventures.Count - 1].Model);
            }
            else
            {
               int index = Math.Max(_adventures.IndexOf(selected) - 1, 0);
               _adventures[index].IsSelected = true;
               _selectedAdventure = new AdventureViewModel(_adventures[index].Model);
            }

            OnPropertyChanged(nameof(SelectedAdventure));
         }
      }

      #endregion
   }
}
