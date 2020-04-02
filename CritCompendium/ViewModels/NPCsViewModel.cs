using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Business;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Persistence;
using CritCompendiumInfrastructure.Business;
using CritCompendiumInfrastructure.Business.Search;
using CritCompendiumInfrastructure.Business.Search.Input;

namespace CritCompendium.ViewModels
{
   public sealed class NPCsViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly Compendium _compendium;
      private readonly NPCSearchService _npcSearchService;
      private readonly NPCSearchInput _npcSearchInput;
      private readonly StringService _stringService;
      private readonly DialogService _dialogService;
      private readonly XMLImporter _xmlImporter;
      private readonly DocumentService _documentService;
      private readonly DataManager _dataManager;
      private readonly ObservableCollection<ListItemViewModel<NPCModel>> _npcs = new ObservableCollection<ListItemViewModel<NPCModel>>();
      private readonly ICommand _selectNPCCommand;
      private readonly ICommand _editNPCCommand;
      private readonly ICommand _exportNPCCommand;
      private readonly ICommand _cancelEditNPCCommand;
      private readonly ICommand _saveEditNPCCommand;
      private readonly ICommand _resetFiltersCommand;
      private readonly ICommand _addCommand;
      private readonly ICommand _copyCommand;
      private readonly ICommand _deleteCommand;
      private readonly ICommand _importCommand;
      private readonly ICommand _selectNextCommand;
      private readonly ICommand _selectPreviousCommand;
      private NPCViewModel _selectedNPC;
      private NPCEditViewModel _npcEditViewModel;
      private bool _editHasUnsavedChanges;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates an instance of <see cref="NPCsViewModel"/>
      /// </summary>
      public NPCsViewModel(Compendium compendium, NPCSearchService npcSearchService, NPCSearchInput npcSearchInput,
          StringService stringService, DialogService dialogService, XMLImporter xmlImporter, DocumentService documentService, DataManager dataManager)
      {
         _compendium = compendium;
         _npcSearchService = npcSearchService;
         _npcSearchInput = npcSearchInput;
         _stringService = stringService;
         _dialogService = dialogService;
         _xmlImporter = xmlImporter;
         _documentService = documentService;
         _dataManager = dataManager;

         _selectNPCCommand = new RelayCommand(obj => true, obj => SelectNPC(obj as ListItemViewModel<NPCModel>));
         _editNPCCommand = new RelayCommand(obj => true, obj => EditNPC(obj as NPCViewModel));
         _exportNPCCommand = new RelayCommand(obj => true, obj => ExportNPC(obj as NPCViewModel));
         _cancelEditNPCCommand = new RelayCommand(obj => true, obj => CancelEditNPC());
         _saveEditNPCCommand = new RelayCommand(obj => HasUnsavedChanges, obj => SaveEditNPC());
         _resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
         _addCommand = new RelayCommand(obj => true, obj => Add());
         _copyCommand = new RelayCommand(obj => _selectedNPC != null, obj => Copy());
         _deleteCommand = new RelayCommand(obj => _selectedNPC != null, obj => Delete());
         _importCommand = new RelayCommand(obj => true, obj => Import());
         _selectNextCommand = new RelayCommand(obj => true, obj => SelectNext());
         _selectPreviousCommand = new RelayCommand(obj => true, obj => SelectPrevious());

         Search();
      }

      #endregion

      #region Properties

      /// <summary>
      /// List of npcs
      /// </summary>
      public ObservableCollection<ListItemViewModel<NPCModel>> NPCs
      {
         get { return _npcs; }
      }

      /// <summary>
      /// Gets or sets the search text
      /// </summary>
      public string SearchText
      {
         get { return _npcSearchInput.SearchText; }
         set
         {
            _npcSearchInput.SearchText = value;
            Search();
         }
      }

      /// <summary>
      /// Command to select a npc
      /// </summary>
      public ICommand SelectNPCCommand
      {
         get { return _selectNPCCommand; }
      }

      /// <summary>
      /// Selected npc
      /// </summary>
      public NPCViewModel SelectedNPC
      {
         get { return _selectedNPC; }
      }

      /// <summary>
      /// Editing npc
      /// </summary>
      public NPCEditViewModel EditingNPC
      {
         get { return _npcEditViewModel; }
      }

      /// <summary>
      /// Command to reset filters
      /// </summary>
      public ICommand ResetFiltersCommand
      {
         get { return _resetFiltersCommand; }
      }

      /// <summary>
      /// Command to edit npc
      /// </summary>
      public ICommand EditNPCCommand
      {
         get { return _editNPCCommand; }
      }

      /// <summary>
      /// Command to export npc
      /// </summary>
      public ICommand ExportNPCCommand
      {
         get { return _exportNPCCommand; }
      }

      /// <summary>
      /// Command to cancel edit npc
      /// </summary>
      public ICommand CancelEditNPCCommand
      {
         get { return _cancelEditNPCCommand; }
      }

      /// <summary>
      /// Command to save edit npc
      /// </summary>
      public ICommand SaveEditNPCCommand
      {
         get { return _saveEditNPCCommand; }
      }

      /// <summary>
      /// Command to add npc
      /// </summary>
      public ICommand AddNPCCommand
      {
         get { return _addCommand; }
      }

      /// <summary>
      /// Command to copy npc
      /// </summary>
      public ICommand CopyNPCCommand
      {
         get { return _copyCommand; }
      }

      /// <summary>
      /// Command to delete selected npc
      /// </summary>
      public ICommand DeleteNPCCommand
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
      /// True if currently editing a npc
      /// </summary>
      public bool IsEditingNPC
      {
         get { return _npcEditViewModel != null; }
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
         _npcs.Clear();
         foreach (NPCModel npcModel in _npcSearchService.Search(_npcSearchInput))
         {
            ListItemViewModel<NPCModel> listItem = new ListItemViewModel<NPCModel>(npcModel);
            InitializeListItemDetails(listItem, npcModel);
            _npcs.Add(listItem);
         }

         if (_selectedNPC != null)
         {
            ListItemViewModel<NPCModel> npc = _npcs.FirstOrDefault(x => x.Model.Id == _selectedNPC.NPCModel.Id);
            if (npc != null)
            {
               npc.IsSelected = true;
            }
         }
      }

      #endregion

      #region Private Methods

      private void InitializeSearch()
      {
         _npcSearchInput.Reset();

         OnPropertyChanged(nameof(SearchText));

         Search();
      }

      private void SelectNPC(ListItemViewModel<NPCModel> npcItem)
      {
         bool selectNPC = true;

         if (_npcEditViewModel != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                            _selectedNPC.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditNPC())
                  {
                     selectNPC = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditNPC();
               }
               else
               {
                  selectNPC = false;
               }
            }
            else
            {
               CancelEditNPC();
            }
         }

         if (selectNPC)
         {
            foreach (ListItemViewModel<NPCModel> item in _npcs)
            {
               item.IsSelected = false;
            }
            npcItem.IsSelected = true;

            _selectedNPC = new NPCViewModel(npcItem.Model);
            OnPropertyChanged(nameof(SelectedNPC));
         }
      }

      private void EditNPC(NPCViewModel npcModel)
      {
         if (npcModel != null)
         {
            _npcEditViewModel = new NPCEditViewModel(npcModel.NPCModel);
            _npcEditViewModel.PropertyChanged += _npcEditViewModel_PropertyChanged;

            OnPropertyChanged(nameof(EditingNPC));
            OnPropertyChanged(nameof(IsEditingNPC));
         }
      }

      private void CancelEditNPC()
      {
         _editHasUnsavedChanges = false;
         _npcEditViewModel = null;

         OnPropertyChanged(nameof(EditingNPC));
         OnPropertyChanged(nameof(IsEditingNPC));
         OnPropertyChanged(nameof(HasUnsavedChanges));
      }

      private bool SaveEditNPC()
      {
         bool saved = false;

         if (_npcEditViewModel.NPCModel != null)
         {
            _npcEditViewModel.NPCModel.Id = _selectedNPC.NPCModel.Id;
            _compendium.UpdateNPC(_npcEditViewModel.NPCModel);

            _selectedNPC = new NPCViewModel(_npcEditViewModel.NPCModel);

            ListItemViewModel<NPCModel> oldListItem = _npcs.FirstOrDefault(x => x.Model.Id == _npcEditViewModel.NPCModel.Id);
            if (oldListItem != null)
            {
               if (_npcSearchService.SearchInputApplies(_npcSearchInput, _npcEditViewModel.NPCModel))
               {
                  InitializeListItemDetails(oldListItem, _npcEditViewModel.NPCModel);
               }
               else
               {
                  _npcs.Remove(oldListItem);
               }
            }

            _npcEditViewModel = null;
            _editHasUnsavedChanges = false;

            SortNPCs();

            _compendium.SaveNPCs();

            OnPropertyChanged(nameof(SelectedNPC));
            OnPropertyChanged(nameof(EditingNPC));
            OnPropertyChanged(nameof(IsEditingNPC));
            OnPropertyChanged(nameof(HasUnsavedChanges));

            saved = true;
         }

         return saved;
      }

      private void Add()
      {
         bool addNPC = true;

         if (_npcEditViewModel != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                                    _selectedNPC.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditNPC())
                  {
                     addNPC = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditNPC();
               }
               else
               {
                  addNPC = false;
               }
            }
            else
            {
               CancelEditNPC();
            }
         }

         if (addNPC)
         {
            NPCModel npcModel = new NPCModel();

            _compendium.AddNPC(npcModel);

            if (_npcSearchService.SearchInputApplies(_npcSearchInput, npcModel))
            {
               ListItemViewModel<NPCModel> listItem = new ListItemViewModel<NPCModel>(npcModel);
               InitializeListItemDetails(listItem, npcModel);
               _npcs.Add(listItem);
               foreach (ListItemViewModel<NPCModel> item in _npcs)
               {
                  item.IsSelected = false;
               }
               listItem.IsSelected = true;
            }

            _selectedNPC = new NPCViewModel(npcModel);

            _npcEditViewModel = new NPCEditViewModel(npcModel);
            _npcEditViewModel.PropertyChanged += _npcEditViewModel_PropertyChanged;

            SortNPCs();

            _compendium.SaveNPCs();

            OnPropertyChanged(nameof(EditingNPC));
            OnPropertyChanged(nameof(IsEditingNPC));
            OnPropertyChanged(nameof(SelectedNPC));
         }
      }

      private void Copy()
      {
         if (_selectedNPC != null)
         {
            bool copyNPC = true;

            if (_npcEditViewModel != null)
            {
               if (_editHasUnsavedChanges)
               {
                  string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                               _selectedNPC.Name, Environment.NewLine + Environment.NewLine);
                  string accept = "Save and Continue";
                  string reject = "Discard Changes";
                  string cancel = "Cancel Navigation";
                  bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                  if (result == true)
                  {
                     if (!SaveEditNPC())
                     {
                        copyNPC = false;
                     }
                  }
                  else if (result == false)
                  {
                     CancelEditNPC();
                  }
                  else
                  {
                     copyNPC = false;
                  }
               }
               else
               {
                  CancelEditNPC();
               }
            }

            if (copyNPC)
            {
               NPCModel npcModel = new NPCModel(_selectedNPC.NPCModel);
               npcModel.Name += " (copy)";
               npcModel.Id = Guid.NewGuid();

               _compendium.AddNPC(npcModel);

               if (_npcSearchService.SearchInputApplies(_npcSearchInput, npcModel))
               {
                  ListItemViewModel<NPCModel> listItem = new ListItemViewModel<NPCModel>(npcModel);
                  InitializeListItemDetails(listItem, npcModel);
                  _npcs.Add(listItem);
                  foreach (ListItemViewModel<NPCModel> item in _npcs)
                  {
                     item.IsSelected = false;
                  }
                  listItem.IsSelected = true;
               }

               _selectedNPC = new NPCViewModel(npcModel);

               SortNPCs();

               _compendium.SaveNPCs();

               OnPropertyChanged(nameof(SelectedNPC));
            }
         }
      }

      private void Delete()
      {
         if (_selectedNPC != null)
         {
            string message = String.Format("Are you sure you want to delete {0}?",
                                                     _selectedNPC.Name);

            bool? result = _dialogService.ShowConfirmationDialog("Delete NPC", message, "Yes", "No", null);

            if (result == true)
            {
               _compendium.DeleteNPC(_selectedNPC.NPCModel.Id);

               ListItemViewModel<NPCModel> listItem = _npcs.FirstOrDefault(x => x.Model.Id == _selectedNPC.NPCModel.Id);
               if (listItem != null)
               {
                  _npcs.Remove(listItem);
               }

               _selectedNPC = null;

               _compendium.SaveNPCs();

               OnPropertyChanged(nameof(SelectedNPC));

               if (_npcEditViewModel != null)
               {
                  CancelEditNPC();
               }
            }
         }
      }

      private void SortNPCs()
      {
         if (_npcs != null && _npcs.Count > 0)
         {
            List<NPCModel> npcs = _npcSearchService.Sort(_npcs.Select(x => x.Model), _npcSearchInput.SortOption.Key);
            for (int i = 0; i < npcs.Count; ++i)
            {
               if (npcs[i].Id != _npcs[i].Model.Id)
               {
                  ListItemViewModel<NPCModel> npc = _npcs.FirstOrDefault(x => x.Model.Id == npcs[i].Id);
                  if (npc != null)
                  {
                     _npcs.Move(_npcs.IndexOf(npc), i);
                  }
               }
            }

            List<ListItemViewModel<NPCModel>> sorted = _npcs.OrderBy(x => x.Name).ToList();

            for (int i = 0; i < sorted.Count; ++i)
            {
               if (!sorted[i].Equals(_npcs[i]))
               {
                  _npcs.Move(_npcs.IndexOf(sorted[i]), i);
               }
            }
         }
      }

      private void _npcEditViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (!_editHasUnsavedChanges)
         {
            _editHasUnsavedChanges = true;
         }
      }

      private void ExportNPC(NPCViewModel npcViewModel)
      {
         Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
         saveFileDialog.Filter = "NPC Archive|*.ccea|Word Document|*.docx";
         saveFileDialog.Title = "Save NPC";
         saveFileDialog.FileName = npcViewModel.Name;

         if (saveFileDialog.ShowDialog() == true)
         {
            try
            {
               string ext = Path.GetExtension(saveFileDialog.FileName);

               if (ext == ".ccaa")
               {
                  byte[] bytes = _dataManager.CreateNPCArchive(npcViewModel.NPCModel);
                  File.WriteAllBytes(saveFileDialog.FileName, bytes);
               }
               else if (ext == "*.docx")
               {
                  //_documentService.CreateWordDoc(saveFileDialog.FileName, npcViewModel);
               }
               else
               {
                  _dialogService.ShowConfirmationDialog("Unable To Export", "Invalid file extension.", "OK", null, null);
               }
            }
            catch (Exception)
            {
               _dialogService.ShowConfirmationDialog("Unable To Export", "An error occurred when attempting to export the npc.", "OK", null, null);
            }
         }
      }

      private void InitializeListItemDetails(ListItemViewModel<NPCModel> listItem, NPCModel npcModel)
      {
         listItem.Model = npcModel;

         if (!String.IsNullOrWhiteSpace(npcModel.Name))
         {
            listItem.Name = npcModel.Name;
         }
         else
         {
            listItem.Name = "Unknown Name";
         }

         if (!String.IsNullOrWhiteSpace(npcModel.Appearance))
         {
            listItem.Description = new String(npcModel.Appearance.Take(50).ToArray());
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
         if (_npcs.Any())
         {
            ListItemViewModel<NPCModel> selected = _npcs.FirstOrDefault(x => x.IsSelected);

            foreach (ListItemViewModel<NPCModel> npc in _npcs)
            {
               npc.IsSelected = false;
            }

            if (selected == null)
            {
               _npcs[0].IsSelected = true;
               _selectedNPC = new NPCViewModel(_npcs[0].Model);
            }
            else
            {
               int index = Math.Min(_npcs.IndexOf(selected) + 1, _npcs.Count - 1);
               _npcs[index].IsSelected = true;
               _selectedNPC = new NPCViewModel(_npcs[index].Model);
            }

            OnPropertyChanged(nameof(SelectedNPC));
         }
      }

      private void SelectPrevious()
      {
         if (_npcs.Any())
         {
            ListItemViewModel<NPCModel> selected = _npcs.FirstOrDefault(x => x.IsSelected);

            foreach (ListItemViewModel<NPCModel> npc in _npcs)
            {
               npc.IsSelected = false;
            }

            if (selected == null)
            {
               _npcs[_npcs.Count - 1].IsSelected = true;
               _selectedNPC = new NPCViewModel(_npcs[_npcs.Count - 1].Model);
            }
            else
            {
               int index = Math.Max(_npcs.IndexOf(selected) - 1, 0);
               _npcs[index].IsSelected = true;
               _selectedNPC = new NPCViewModel(_npcs[index].Model);
            }

            OnPropertyChanged(nameof(SelectedNPC));
         }
      }

      #endregion
   }
}
