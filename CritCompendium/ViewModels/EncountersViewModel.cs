using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendium.ViewModels.ObjectViewModels;
using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Persistence;
using CriticalCompendiumInfrastructure.Services;
using CriticalCompendiumInfrastructure.Services.Search;
using CriticalCompendiumInfrastructure.Services.Search.Input;

namespace CritCompendium.ViewModels
{
    public sealed class EncountersViewModel : NotifyPropertyChanged
    {
        #region Fields

        private readonly Compendium _compendium;
        private readonly EncounterSearchService _encounterSearchService;
        private readonly EncounterSearchInput _encounterSearchInput;
        private readonly StringService _stringService;
        private readonly DialogService _dialogService;
        private readonly XMLImporter _xmlImporter;
        private readonly DocumentService _documentService;
        private readonly DataManager _dataManager;
        private readonly ObservableCollection<EncounterListItemViewModel> _encounters = new ObservableCollection<EncounterListItemViewModel>();
        private readonly ICommand _selectEncounterCommand;
        private readonly ICommand _editEncounterCommand;
        private readonly ICommand _exportEncounterCommand;
        private readonly ICommand _cancelEditEncounterCommand;
        private readonly ICommand _saveEditEncounterCommand;
        private readonly ICommand _resetFiltersCommand;
        private readonly ICommand _addCommand;
        private readonly ICommand _copyCommand;
        private readonly ICommand _deleteCommand;
        private readonly ICommand _importCommand;
        private readonly ICommand _selectNextCommand;
        private readonly ICommand _selectPreviousCommand;
        private EncounterViewModel _selectedEncounter;
        private EncounterViewModel _encounterEditViewModel;
        private bool _editHasUnsavedChanges;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="EncountersViewModel"/>
        /// </summary>
        public EncountersViewModel(Compendium compendium, EncounterSearchService encounterSearchService, EncounterSearchInput encounterSearchInput,
            StringService stringService, DialogService dialogService, XMLImporter xmlImporter, DocumentService documentService, DataManager dataManager)
        {
            _compendium = compendium;
            _encounterSearchService = encounterSearchService;
            _encounterSearchInput = encounterSearchInput;
            _stringService = stringService;
            _dialogService = dialogService;
            _xmlImporter = xmlImporter;
            _documentService = documentService;
            _dataManager = dataManager;

            _selectEncounterCommand = new RelayCommand(obj => true, obj => SelectEncounter(obj as EncounterListItemViewModel));
            _editEncounterCommand = new RelayCommand(obj => true, obj => EditEncounter(obj as EncounterViewModel));
            _exportEncounterCommand = new RelayCommand(obj => true, obj => ExportEncounter(obj as EncounterViewModel));
            _cancelEditEncounterCommand = new RelayCommand(obj => true, obj => CancelEditEncounter());
            _saveEditEncounterCommand = new RelayCommand(obj => HasUnsavedChanges, obj => SaveEditEncounter());
            _resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
            _addCommand = new RelayCommand(obj => true, obj => Add());
            _copyCommand = new RelayCommand(obj => _selectedEncounter != null, obj => Copy());
            _deleteCommand = new RelayCommand(obj => _selectedEncounter != null, obj => Delete());
            _importCommand = new RelayCommand(obj => true, obj => Import());
            _selectNextCommand = new RelayCommand(obj => true, obj => SelectNext());
            _selectPreviousCommand = new RelayCommand(obj => true, obj => SelectPrevious());

            _compendium.CharacterChanged += _compendium_CharacterChanged;
            _compendium.EncounterChanged += _compendium_EncounterChanged;

           Search();
        }

        #endregion

        #region Properties

        /// <summary>
        /// List of encounters
        /// </summary>
        public ObservableCollection<EncounterListItemViewModel> Encounters
        {
            get { return _encounters; }
        }

        /// <summary>
        /// Gets or sets the search text
        /// </summary>
        public string SearchText
        {
            get { return _encounterSearchInput.SearchText; }
            set
            {
                _encounterSearchInput.SearchText = value;
                Search();
            }
        }

        /// <summary>
        /// Command to select a encounter
        /// </summary>
        public ICommand SelectEncounterCommand
        {
            get { return _selectEncounterCommand; }
        }

        /// <summary>
        /// Selected encounter
        /// </summary>
        public EncounterViewModel SelectedEncounter
        {
            get { return _selectedEncounter; }
        }

        /// <summary>
        /// Editing encounter
        /// </summary>
        public EncounterViewModel EditingEncounter
        {
            get { return _encounterEditViewModel; }
        }

        /// <summary>
        /// Command to reset filters
        /// </summary>
        public ICommand ResetFiltersCommand
        {
            get { return _resetFiltersCommand; }
        }

        /// <summary>
        /// Command to edit encounter
        /// </summary>
        public ICommand EditEncounterCommand
        {
            get { return _editEncounterCommand; }
        }

        /// <summary>
        /// Command to export encounter
        /// </summary>
        public ICommand ExportEncounterCommand
        {
            get { return _exportEncounterCommand; }
        }

        /// <summary>
        /// Command to cancel edit encounter
        /// </summary>
        public ICommand CancelEditEncounterCommand
        {
            get { return _cancelEditEncounterCommand; }
        }

        /// <summary>
        /// Command to save edit encounter
        /// </summary>
        public ICommand SaveEditEncounterCommand
        {
            get { return _saveEditEncounterCommand; }
        }

        /// <summary>
        /// Command to add encounter
        /// </summary>
        public ICommand AddEncounterCommand
        {
            get { return _addCommand; }
        }

        /// <summary>
        /// Command to copy encounter
        /// </summary>
        public ICommand CopyEncounterCommand
        {
            get { return _copyCommand; }
        }

        /// <summary>
        /// Command to delete selected encounter
        /// </summary>
        public ICommand DeleteEncounterCommand
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
        /// True if currently editing a encounter
        /// </summary>
        public bool IsEditingEncounter
        {
            get { return _encounterEditViewModel != null; }
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
            _encounters.Clear();
            foreach (EncounterModel encounterModel in _encounterSearchService.Search(_encounterSearchInput))
            {
                _encounters.Add(new EncounterListItemViewModel(encounterModel));
            }

            if (_selectedEncounter != null)
            {
                EncounterListItemViewModel encounter = _encounters.FirstOrDefault(x => x.EncounterModel.ID == _selectedEncounter.EncounterModel.ID);
                if (encounter != null)
                {
                    encounter.IsSelected = true;
                }
            }
        }

        #endregion

        #region Private Methods

        private void InitializeSearch()
        {
            _encounterSearchInput.Reset();

            OnPropertyChanged(nameof(SearchText));

            Search();
        }

        private void SelectEncounter(EncounterListItemViewModel encounterItem)
        {
            bool selectEncounter = true;

            if (_encounterEditViewModel != null)
            {
                if (_editHasUnsavedChanges)
                {
                    string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                                         _selectedEncounter.Name, Environment.NewLine + Environment.NewLine);
                    string accept = "Save and Continue";
                    string reject = "Discard Changes";
                    string cancel = "Cancel Navigation";
                    bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                    if (result == true)
                    {
                        if (!SaveEditEncounter())
                        {
                            selectEncounter = false;
                        }
                    }
                    else if (result == false)
                    {
                        CancelEditEncounter();
                    }
                    else
                    {
                        selectEncounter = false;
                    }
                }
                else
                {
                    CancelEditEncounter();
                }
            }

            if (selectEncounter)
            {
                foreach (EncounterListItemViewModel item in _encounters)
                {
                    item.IsSelected = false;
                }
                encounterItem.IsSelected = true;

                _selectedEncounter = new EncounterViewModel(encounterItem.EncounterModel);
                OnPropertyChanged(nameof(SelectedEncounter));
            }
        }

        private void EditEncounter(EncounterViewModel encounterModel)
        {
            if (encounterModel != null)
            {
                _encounterEditViewModel = new EncounterViewModel(encounterModel.EncounterModel);
                _encounterEditViewModel.PropertyChanged += _encounterEditViewModel_PropertyChanged;

                OnPropertyChanged(nameof(EditingEncounter));
                OnPropertyChanged(nameof(IsEditingEncounter));
            }
        }

        private void CancelEditEncounter()
        {
            _editHasUnsavedChanges = false;
            _encounterEditViewModel = null;

            OnPropertyChanged(nameof(EditingEncounter));
            OnPropertyChanged(nameof(IsEditingEncounter));
            OnPropertyChanged(nameof(HasUnsavedChanges));
        }

        private bool SaveEditEncounter()
        {
            bool saved = false;

            if (_encounterEditViewModel.EncounterModel != null)
            {
                _encounterEditViewModel.EncounterModel.ID = _selectedEncounter.EncounterModel.ID;
                _compendium.UpdateEncounter(_encounterEditViewModel.EncounterModel);

                _selectedEncounter = new EncounterViewModel(_encounterEditViewModel.EncounterModel);

                EncounterListItemViewModel oldListItem = _encounters.FirstOrDefault(x => x.EncounterModel.ID == _encounterEditViewModel.EncounterModel.ID);
                if (oldListItem != null)
                {
                    if (_encounterSearchService.SearchInputApplies(_encounterSearchInput, _encounterEditViewModel.EncounterModel))
                    {
                        oldListItem.UpdateModel(_encounterEditViewModel.EncounterModel);
                    }
                    else
                    {
                        _encounters.Remove(oldListItem);
                    }
                }

                _encounterEditViewModel = null;
                _editHasUnsavedChanges = false;

                SortEncounters();

                _compendium.SaveEncounters();

                OnPropertyChanged(nameof(SelectedEncounter));
                OnPropertyChanged(nameof(EditingEncounter));
                OnPropertyChanged(nameof(IsEditingEncounter));
                OnPropertyChanged(nameof(HasUnsavedChanges));

                saved = true;
            }

            return saved;
        }

        private void Add()
        {
            bool addEncounter = true;

            if (_encounterEditViewModel != null)
            {
                if (_editHasUnsavedChanges)
                {
                    string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                                         _selectedEncounter.Name, Environment.NewLine + Environment.NewLine);
                    string accept = "Save and Continue";
                    string reject = "Discard Changes";
                    string cancel = "Cancel Navigation";
                    bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                    if (result == true)
                    {
                        if (!SaveEditEncounter())
                        {
                            addEncounter = false;
                        }
                    }
                    else if (result == false)
                    {
                        CancelEditEncounter();
                    }
                    else
                    {
                        addEncounter = false;
                    }
                }
                else
                {
                    CancelEditEncounter();
                }
            }

            if (addEncounter)
            {
                EncounterModel encounterModel = new EncounterModel();
                encounterModel.Name = "New Encounter";

                _compendium.AddEncounter(encounterModel);

                if (_encounterSearchService.SearchInputApplies(_encounterSearchInput, encounterModel))
                {
                    EncounterListItemViewModel listItem = new EncounterListItemViewModel(encounterModel);
                    _encounters.Add(listItem);
                    foreach (EncounterListItemViewModel item in _encounters)
                    {
                        item.IsSelected = false;
                    }
                    listItem.IsSelected = true;
                }

                _selectedEncounter = new EncounterViewModel(encounterModel);

                _encounterEditViewModel = new EncounterViewModel(encounterModel);
                _encounterEditViewModel.PropertyChanged += _encounterEditViewModel_PropertyChanged;

                SortEncounters();

                _compendium.SaveEncounters();

                OnPropertyChanged(nameof(EditingEncounter));
                OnPropertyChanged(nameof(IsEditingEncounter));
                OnPropertyChanged(nameof(SelectedEncounter));
            }
        }

        private void Copy()
        {
            if (_selectedEncounter != null)
            {
                bool copyEncounter = true;

                if (_encounterEditViewModel != null)
                {
                    if (_editHasUnsavedChanges)
                    {
                        string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                                     _selectedEncounter.Name, Environment.NewLine + Environment.NewLine);
                        string accept = "Save and Continue";
                        string reject = "Discard Changes";
                        string cancel = "Cancel Navigation";
                        bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                        if (result == true)
                        {
                            if (!SaveEditEncounter())
                            {
                                copyEncounter = false;
                            }
                        }
                        else if (result == false)
                        {
                            CancelEditEncounter();
                        }
                        else
                        {
                            copyEncounter = false;
                        }
                    }
                    else
                    {
                        CancelEditEncounter();
                    }
                }

                if (copyEncounter)
                {
                    EncounterModel encounterModel = new EncounterModel(_selectedEncounter.EncounterModel);
                    encounterModel.Name += " (copy)";
                    encounterModel.ID = Guid.NewGuid();

                    _compendium.AddEncounter(encounterModel);

                    if (_encounterSearchService.SearchInputApplies(_encounterSearchInput, encounterModel))
                    {
                        EncounterListItemViewModel listItem = new EncounterListItemViewModel(encounterModel);
                        _encounters.Add(listItem);
                        foreach (EncounterListItemViewModel item in _encounters)
                        {
                            item.IsSelected = false;
                        }
                        listItem.IsSelected = true;
                    }

                    _selectedEncounter = new EncounterViewModel(encounterModel);

                    SortEncounters();

                    _compendium.SaveEncounters();

                    OnPropertyChanged(nameof(SelectedEncounter));
                }
            }
        }

        private void Delete()
        {
            if (_selectedEncounter != null)
            {
                string message = String.Format("Are you sure you want to delete {0}?",
                                                         _selectedEncounter.Name);

                bool? result = _dialogService.ShowConfirmationDialog("Delete Encounter", message, "Yes", "No", null);

                if (result == true)
                {
                    _compendium.DeleteEncounter(_selectedEncounter.EncounterModel.ID);

                    EncounterListItemViewModel listItem = _encounters.FirstOrDefault(x => x.EncounterModel.ID == _selectedEncounter.EncounterModel.ID);
                    if (listItem != null)
                    {
                        _encounters.Remove(listItem);
                    }

                    _selectedEncounter = null;

                    _compendium.SaveEncounters();

                    OnPropertyChanged(nameof(SelectedEncounter));

                    if (_encounterEditViewModel != null)
                    {
                        CancelEditEncounter();
                    }
                }
            }
        }

        private void SortEncounters()
        {
            if (_encounters != null && _encounters.Count > 0)
            {
                List<EncounterModel> encounters = _encounterSearchService.Sort(_encounters.Select(x => x.EncounterModel), _encounterSearchInput.SortOption);
                for (int i = 0; i < encounters.Count; ++i)
                {
                    if (encounters[i].ID != _encounters[i].EncounterModel.ID)
                    {
                        EncounterListItemViewModel encounter = _encounters.FirstOrDefault(x => x.EncounterModel.ID == encounters[i].ID);
                        if (encounter != null)
                        {
                            _encounters.Move(_encounters.IndexOf(encounter), i);
                        }
                    }
                }

                List<EncounterListItemViewModel> sorted = _encounters.OrderBy(x => x.Name).ToList();

                for (int i = 0; i < sorted.Count; ++i)
                {
                    if (!sorted[i].Equals(_encounters[i]))
                    {
                        _encounters.Move(_encounters.IndexOf(sorted[i]), i);
                    }
                }
            }
        }

        private void _encounterEditViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!_editHasUnsavedChanges)
            {
                _editHasUnsavedChanges = true;
            }
        }

        private void ExportEncounter(EncounterViewModel encounterViewModel)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Encounter Archive|*.ccea";
            saveFileDialog.Title = "Save Encounter";
            saveFileDialog.FileName = encounterViewModel.Name;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string ext = System.IO.Path.GetExtension(saveFileDialog.FileName);

                    if (ext == ".ccea")
                    {
                        byte[] bytes = _dataManager.CreateEncounterArchive(encounterViewModel.EncounterModel);
                        File.WriteAllBytes(saveFileDialog.FileName, bytes);
                    }
                    else
                    {
                        _dialogService.ShowConfirmationDialog("Unable To Export", "Invalid file extension.", "OK", null, null);
                    }
                }
                catch (Exception)
                {
                    _dialogService.ShowConfirmationDialog("Unable To Export", "An error occurred when attempting to export the encounter.", "OK", null, null);
                }
            }
        }

        private void _compendium_CharacterChanged(object sender, CompendiumChangeEventArgs e)
        {
            if (_selectedEncounter != null)
            {
                foreach (EncounterCharacterViewModel encounterCharacter in _selectedEncounter.Characters)
                {
                    if (encounterCharacter.EncounterCharacterModel.CharacterModel != null && e.IDs.Contains(encounterCharacter.EncounterCharacterModel.CharacterModel.ID))
                    {
                        _selectedEncounter = new EncounterViewModel(_selectedEncounter.EncounterModel);
                        break;
                    }
                }
            }
        }

        private void _compendium_EncounterChanged(object sender, CompendiumChangeEventArgs e)
        {
            if (_selectedEncounter != null && e.IDs.Contains(_selectedEncounter.EncounterModel.ID))
            {
                _selectedEncounter = new EncounterViewModel(_selectedEncounter.EncounterModel);
            }
        }

        private void Import()
        {
            _dialogService.ShowImportView();
        }

        private void SelectNext()
        {
            if (_encounters.Any())
            {
                EncounterListItemViewModel selected = _encounters.FirstOrDefault(x => x.IsSelected);

                foreach (EncounterListItemViewModel encounter in _encounters)
                {
                    encounter.IsSelected = false;
                }

                if (selected == null)
                {
                    _encounters[0].IsSelected = true;
                    _selectedEncounter = new EncounterViewModel(_encounters[0].EncounterModel);
                }
                else
                {
                    int index = Math.Min(_encounters.IndexOf(selected) + 1, _encounters.Count - 1);
                    _encounters[index].IsSelected = true;
                    _selectedEncounter = new EncounterViewModel(_encounters[index].EncounterModel);
                }

                OnPropertyChanged(nameof(SelectedEncounter));
            }
        }

        private void SelectPrevious()
        {
            if (_encounters.Any())
            {
                EncounterListItemViewModel selected = _encounters.FirstOrDefault(x => x.IsSelected);

                foreach (EncounterListItemViewModel encounter in _encounters)
                {
                    encounter.IsSelected = false;
                }

                if (selected == null)
                {
                    _encounters[_encounters.Count - 1].IsSelected = true;
                    _selectedEncounter = new EncounterViewModel(_encounters[_encounters.Count - 1].EncounterModel);
                }
                else
                {
                    int index = Math.Max(_encounters.IndexOf(selected) - 1, 0);
                    _encounters[index].IsSelected = true;
                    _selectedEncounter = new EncounterViewModel(_encounters[index].EncounterModel);
                }

                OnPropertyChanged(nameof(SelectedEncounter));
            }
        }

        #endregion
    }
}
