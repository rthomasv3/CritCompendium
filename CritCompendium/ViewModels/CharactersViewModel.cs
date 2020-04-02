using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
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
   public sealed class CharactersViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly Compendium _compendium;
      private readonly CharacterSearchService _characterSearchService;
      private readonly CharacterSearchInput _characterSearchInput;
      private readonly StringService _stringService;
      private readonly StatService _statService;
      private readonly DialogService _dialogService;
      private readonly XMLImporter _xmlImporter;
      private readonly XMLExporter _xmlExporter;
      private readonly DocumentService _documentService;
      private readonly DataManager _dataManager;
      private readonly ObservableCollection<CharacterListItemViewModel> _characters = new ObservableCollection<CharacterListItemViewModel>();
      private readonly ICommand _selectCharacterCommand;
      private readonly ICommand _editCharacterCommand;
      private readonly ICommand _exportCharacterCommand;
      private readonly ICommand _cancelEditCharacterCommand;
      private readonly ICommand _saveEditCharacterCommand;
      private readonly ICommand _resetFiltersCommand;
      private readonly ICommand _addCommand;
      private readonly ICommand _copyCommand;
      private readonly ICommand _deleteCommand;
      private readonly ICommand _importCommand;
      private readonly ICommand _selectNextCommand;
      private readonly ICommand _selectPreviousCommand;
      private CharacterViewModel _selectedCharacter;
      private CharacterEditViewModel _characterEditViewModel;
      private bool _editHasUnsavedChanges;

      private readonly ICommand _selectInfoEditCommand;
      private readonly ICommand _selectLevelsEditCommand;
      private readonly ICommand _selectAbilitiesEditCommand;
      private readonly ICommand _selectProficiencyEditCommand;

      private bool _infoEditTabSelected;
      private bool _levelsEditTabSelected;
      private bool _statsEditTabSelected;
      private bool _proficiencyEditTabSelected;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates an instance of <see cref="CharactersViewModel"/>
      /// </summary>
      public CharactersViewModel(Compendium compendium, CharacterSearchService characterSearchService, CharacterSearchInput characterSearchInput, StringService stringService,
            StatService statService, DialogService dialogService, XMLImporter xmlImporter, XMLExporter xmlExporter, DocumentService documentService, DataManager dataManager)
      {
         _compendium = compendium;
         _characterSearchService = characterSearchService;
         _characterSearchInput = characterSearchInput;
         _stringService = stringService;
         _statService = statService;
         _dialogService = dialogService;
         _xmlImporter = xmlImporter;
         _xmlExporter = xmlExporter;
         _documentService = documentService;
         _dataManager = dataManager;

         _selectCharacterCommand = new RelayCommand(obj => true, obj => SelectCharacter(obj as CharacterListItemViewModel));
         _editCharacterCommand = new RelayCommand(obj => true, obj => EditCharacter(obj as CharacterViewModel));
         _exportCharacterCommand = new RelayCommand(obj => true, obj => ExportCharacter(obj as CharacterViewModel));
         _cancelEditCharacterCommand = new RelayCommand(obj => true, obj => CancelEditCharacter());
         _saveEditCharacterCommand = new RelayCommand(obj => HasUnsavedChanges, obj => SaveEditCharacter());
         _resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
         _addCommand = new RelayCommand(obj => true, obj => Add());
         _copyCommand = new RelayCommand(obj => _selectedCharacter != null, obj => Copy());
         _deleteCommand = new RelayCommand(obj => _selectedCharacter != null, obj => Delete());
         _importCommand = new RelayCommand(obj => true, obj => Import());
         _selectNextCommand = new RelayCommand(obj => true, obj => SelectNext());
         _selectPreviousCommand = new RelayCommand(obj => true, obj => SelectPrevious());

         _selectInfoEditCommand = new RelayCommand(obj => true, obj => SelectInfoEdit());
         _selectLevelsEditCommand = new RelayCommand(obj => true, obj => SelectLevelsEdit());
         _selectAbilitiesEditCommand = new RelayCommand(obj => true, obj => SelectAbilitiesEdit());
         _selectProficiencyEditCommand = new RelayCommand(obj => true, obj => SelectProficiencyEdit());

         _compendium.CharacterChanged += _compendium_CharacterChanged;

         Search();
      }

      #endregion

      #region Properties

      /// <summary>
      /// List of characters
      /// </summary>
      public ObservableCollection<CharacterListItemViewModel> Characters
      {
         get { return _characters; }
      }

      /// <summary>
      /// Gets or sets the search text
      /// </summary>
      public string SearchText
      {
         get { return _characterSearchInput.SearchText; }
         set
         {
            _characterSearchInput.SearchText = value;
            Search();
         }
      }

      /// <summary>
      /// Command to select a character
      /// </summary>
      public ICommand SelectCharacterCommand
      {
         get { return _selectCharacterCommand; }
      }

      /// <summary>
      /// Selected character
      /// </summary>
      public CharacterViewModel SelectedCharacter
      {
         get { return _selectedCharacter; }
      }

      /// <summary>
      /// Editing character
      /// </summary>
      public CharacterEditViewModel EditingCharacter
      {
         get { return _characterEditViewModel; }
      }

      /// <summary>
      /// Command to reset filters
      /// </summary>
      public ICommand ResetFiltersCommand
      {
         get { return _resetFiltersCommand; }
      }

      /// <summary>
      /// Command to edit character
      /// </summary>
      public ICommand EditCharacterCommand
      {
         get { return _editCharacterCommand; }
      }

      /// <summary>
      /// Command to export character
      /// </summary>
      public ICommand ExportCharacterCommand
      {
         get { return _exportCharacterCommand; }
      }

      /// <summary>
      /// Command to cancel edit character
      /// </summary>
      public ICommand CancelEditCharacterCommand
      {
         get { return _cancelEditCharacterCommand; }
      }

      /// <summary>
      /// Command to save edit character
      /// </summary>
      public ICommand SaveEditCharacterCommand
      {
         get { return _saveEditCharacterCommand; }
      }

      /// <summary>
      /// Command to add character
      /// </summary>
      public ICommand AddCharacterCommand
      {
         get { return _addCommand; }
      }

      /// <summary>
      /// Command to copy character
      /// </summary>
      public ICommand CopyCharacterCommand
      {
         get { return _copyCommand; }
      }

      /// <summary>
      /// Command to delete selected character
      /// </summary>
      public ICommand DeleteCharacterCommand
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
      /// True if currently editing a character
      /// </summary>
      public bool IsEditingCharacter
      {
         get { return _characterEditViewModel != null; }
      }

      /// <summary>
      /// True if edit has unsaved changes
      /// </summary>
      public bool HasUnsavedChanges
      {
         get { return _editHasUnsavedChanges; }
      }

      /// <summary>
      /// Gets select info edit command
      /// </summary>
      public ICommand SelectInfoEditCommand
      {
         get { return _selectInfoEditCommand; }
      }

      /// <summary>
      /// Gets select levels edit command
      /// </summary>
      public ICommand SelectLevelsEditCommand
      {
         get { return _selectLevelsEditCommand; }
      }

      /// <summary>
      /// Gets select abilities edit command
      /// </summary>
      public ICommand SelectAbilitiesEditCommand
      {
         get { return _selectAbilitiesEditCommand; }
      }

      /// <summary>
      /// Gets select proficiency edit command
      /// </summary>
      public ICommand SelectProficiencyEditCommand
      {
         get { return _selectProficiencyEditCommand; }
      }

      /// <summary>
      /// Gets info edit tab selected
      /// </summary>
      public bool InfoEditTabSelected
      {
         get { return _infoEditTabSelected; }
      }

      /// <summary>
      /// Gets levels edit tab selected
      /// </summary>
      public bool LevelsEditTabSelected
      {
         get { return _levelsEditTabSelected; }
      }

      /// <summary>
      /// Gets abilities edit tab selected
      /// </summary>
      public bool AbilitiesEditTabSelected
      {
         get { return _statsEditTabSelected; }
      }

      /// <summary>
      /// Gets proficiency edit tab selected
      /// </summary>
      public bool ProficiencyEditTabSelected
      {
         get { return _proficiencyEditTabSelected; }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Searches, applying current sorting and filtering
      /// </summary>
      public void Search()
      {
         _characters.Clear();
         foreach (CharacterModel characterModel in _characterSearchService.Search(_characterSearchInput))
         {
            _characters.Add(new CharacterListItemViewModel(characterModel));
         }

         if (_selectedCharacter != null)
         {
            CharacterListItemViewModel character = _characters.FirstOrDefault(x => x.CharacterModel.Id == _selectedCharacter.CharacterModel.Id);
            if (character != null)
            {
               character.IsSelected = true;
            }
         }
      }

      #endregion

      #region Private Methods

      private void InitializeSearch()
      {
         _characterSearchInput.Reset();

         OnPropertyChanged(nameof(SearchText));

         Search();
      }

      private void SelectCharacter(CharacterListItemViewModel characterItem)
      {
         bool selectCharacter = true;

         if (_characterEditViewModel != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                           _selectedCharacter.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditCharacter())
                  {
                     selectCharacter = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditCharacter();
               }
               else
               {
                  selectCharacter = false;
               }
            }
            else
            {
               CancelEditCharacter();
            }
         }

         if (selectCharacter)
         {
            foreach (CharacterListItemViewModel item in _characters)
            {
               item.IsSelected = false;
            }
            characterItem.IsSelected = true;

            _selectedCharacter = new CharacterViewModel(characterItem.CharacterModel);
            _selectedCharacter.LevelUp += _selectedCharacter_LevelUp;

            OnPropertyChanged(nameof(SelectedCharacter));
         }
      }

      private void EditCharacter(CharacterViewModel characterModel)
      {
         _characterEditViewModel = new CharacterEditViewModel(characterModel.CharacterModel, false);
         _characterEditViewModel.PropertyChanged += _characterEditViewModel_PropertyChanged;

         SelectInfoEdit();

         OnPropertyChanged(nameof(EditingCharacter));
         OnPropertyChanged(nameof(IsEditingCharacter));
      }

      private void CancelEditCharacter()
      {
         _editHasUnsavedChanges = false;
         _characterEditViewModel = null;

         OnPropertyChanged(nameof(EditingCharacter));
         OnPropertyChanged(nameof(IsEditingCharacter));
         OnPropertyChanged(nameof(HasUnsavedChanges));
      }

      private bool SaveEditCharacter()
      {
         bool saved = false;

         if (_characterEditViewModel.CharacterModel != null)
         {
            if (String.IsNullOrWhiteSpace(_characterEditViewModel.Name))
            {
               _dialogService.ShowConfirmationDialog("Required Field", "Name is required.", "OK", null, null);
            }
            else
            {
               _characterEditViewModel.CharacterModel.Id = _selectedCharacter.CharacterModel.Id;
               _compendium.UpdateCharacter(_characterEditViewModel.CharacterModel);

               _selectedCharacter = new CharacterViewModel(_characterEditViewModel.CharacterModel);

               if (_characterEditViewModel.IsNew)
               {
                  _selectedCharacter.SetupNewlyCreatedCharacter();
               }

               CharacterListItemViewModel oldListItem = _characters.FirstOrDefault(x => x.CharacterModel.Id == _characterEditViewModel.CharacterModel.Id);
               if (oldListItem != null)
               {
                  if (_characterSearchService.SearchInputApplies(_characterSearchInput, _selectedCharacter.CharacterModel))
                  {
                     oldListItem.UpdateModel(_characterEditViewModel.CharacterModel);
                  }
                  else
                  {
                     _characters.Remove(oldListItem);
                  }
               }

               _characterEditViewModel = null;
               _editHasUnsavedChanges = false;

               SortCharacters();

               _compendium.SaveCharacters();

               OnPropertyChanged(nameof(SelectedCharacter));
               OnPropertyChanged(nameof(EditingCharacter));
               OnPropertyChanged(nameof(IsEditingCharacter));
               OnPropertyChanged(nameof(HasUnsavedChanges));

               saved = true;
            }
         }

         return saved;
      }

      private void Add()
      {
         bool addCharacter = true;

         if (_characterEditViewModel != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                                    _selectedCharacter.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditCharacter())
                  {
                     addCharacter = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditCharacter();
               }
               else
               {
                  addCharacter = false;
               }
            }
            else
            {
               CancelEditCharacter();
            }
         }

         if (addCharacter)
         {
            _characterEditViewModel = new CharacterEditViewModel(new CharacterModel(), true);

            _compendium.AddCharacter(_characterEditViewModel.CharacterModel);

            if (_characterSearchService.SearchInputApplies(_characterSearchInput, _characterEditViewModel.CharacterModel))
            {
               CharacterListItemViewModel listItem = new CharacterListItemViewModel(_characterEditViewModel.CharacterModel);
               _characters.Add(listItem);

               foreach (CharacterListItemViewModel item in _characters)
               {
                  item.IsSelected = false;
               }
               listItem.IsSelected = true;
            }

            _selectedCharacter = new CharacterViewModel(_characterEditViewModel.CharacterModel);

            _characterEditViewModel.PropertyChanged += _characterEditViewModel_PropertyChanged;

            SelectInfoEdit();

            SortCharacters();

            _compendium.SaveCharacters();

            OnPropertyChanged(nameof(EditingCharacter));
            OnPropertyChanged(nameof(IsEditingCharacter));
            OnPropertyChanged(nameof(SelectedCharacter));
         }
      }

      private void Copy()
      {
         if (_selectedCharacter != null)
         {
            bool copyCharacter = true;

            if (_characterEditViewModel != null)
            {
               if (_editHasUnsavedChanges)
               {
                  string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                                       _selectedCharacter.Name, Environment.NewLine + Environment.NewLine);
                  string accept = "Save and Continue";
                  string reject = "Discard Changes";
                  string cancel = "Cancel Navigation";
                  bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                  if (result == true)
                  {
                     if (!SaveEditCharacter())
                     {
                        copyCharacter = false;
                     }
                  }
                  else if (result == false)
                  {
                     CancelEditCharacter();
                  }
                  else
                  {
                     copyCharacter = false;
                  }
               }
               else
               {
                  CancelEditCharacter();
               }
            }

            if (copyCharacter)
            {
               CharacterModel characterModel = new CharacterModel(_selectedCharacter.CharacterModel);
               characterModel.Name += " (copy)";
               characterModel.Id = Guid.NewGuid();

               _compendium.AddCharacter(characterModel);

               if (_characterSearchService.SearchInputApplies(_characterSearchInput, characterModel))
               {
                  CharacterListItemViewModel listItem = new CharacterListItemViewModel(characterModel);
                  _characters.Add(listItem);
                  foreach (CharacterListItemViewModel item in _characters)
                  {
                     item.IsSelected = false;
                  }
                  listItem.IsSelected = true;
               }

               _selectedCharacter = new CharacterViewModel(characterModel);

               SortCharacters();

               _compendium.SaveCharacters();

               OnPropertyChanged(nameof(SelectedCharacter));
            }
         }
      }

      private void Delete()
      {
         if (_selectedCharacter != null)
         {
            bool canDelete = true;

            foreach (EncounterModel encounter in _compendium.Encounters)
            {
               foreach (EncounterCharacterModel encounterCharacter in encounter.Creatures.Where(x => x is EncounterCharacterModel))
               {
                  if (encounterCharacter.CharacterModel != null && encounterCharacter.CharacterModel.Id == _selectedCharacter.CharacterModel.Id)
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

            if (canDelete)
            {
               string message = String.Format("Are you sure you want to delete {0}?",
                                                        _selectedCharacter.Name);

               bool? result = _dialogService.ShowConfirmationDialog("Delete Character", message, "Yes", "No", null);

               if (result == true)
               {
                  _compendium.DeleteCharacter(_selectedCharacter.CharacterModel.Id);

                  CharacterListItemViewModel listItem = _characters.FirstOrDefault(x => x.CharacterModel.Id == _selectedCharacter.CharacterModel.Id);
                  if (listItem != null)
                  {
                     _characters.Remove(listItem);
                  }

                  _selectedCharacter = null;

                  _compendium.SaveCharacters();

                  OnPropertyChanged(nameof(SelectedCharacter));

                  if (_characterEditViewModel != null)
                  {
                     CancelEditCharacter();
                  }
               }
            }
            else
            {
               _dialogService.ShowConfirmationDialog("Unable Delete Character", "Character is in use by an encounter.", "OK", null, null);
            }
         }
      }

      private void SortCharacters()
      {
         if (_characters != null && _characters.Count > 0)
         {
            List<CharacterModel> characters = _characterSearchService.Sort(_characters.Select(x => x.CharacterModel), _characterSearchInput.SortOption.Key);
            for (int i = 0; i < characters.Count; ++i)
            {
               if (characters[i].Id != _characters[i].CharacterModel.Id)
               {
                  CharacterListItemViewModel character = _characters.FirstOrDefault(x => x.CharacterModel.Id == characters[i].Id);
                  if (character != null)
                  {
                     _characters.Move(_characters.IndexOf(character), i);
                  }
               }
            }
         }
      }

      private void _characterEditViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (!_editHasUnsavedChanges)
         {
            _editHasUnsavedChanges = true;
         }
      }

      private void SelectInfoEdit()
      {
         _infoEditTabSelected = true;
         _levelsEditTabSelected = false;
         _statsEditTabSelected = false;
         _proficiencyEditTabSelected = false;

         OnPropertyChanged(nameof(InfoEditTabSelected));
         OnPropertyChanged(nameof(LevelsEditTabSelected));
         OnPropertyChanged(nameof(AbilitiesEditTabSelected));
         OnPropertyChanged(nameof(ProficiencyEditTabSelected));
      }

      private void SelectLevelsEdit()
      {
         _infoEditTabSelected = false;
         _levelsEditTabSelected = true;
         _statsEditTabSelected = false;
         _proficiencyEditTabSelected = false;

         OnPropertyChanged(nameof(InfoEditTabSelected));
         OnPropertyChanged(nameof(LevelsEditTabSelected));
         OnPropertyChanged(nameof(AbilitiesEditTabSelected));
         OnPropertyChanged(nameof(ProficiencyEditTabSelected));
      }

      private void SelectAbilitiesEdit()
      {
         _infoEditTabSelected = false;
         _levelsEditTabSelected = false;
         _statsEditTabSelected = true;
         _proficiencyEditTabSelected = false;

         OnPropertyChanged(nameof(InfoEditTabSelected));
         OnPropertyChanged(nameof(LevelsEditTabSelected));
         OnPropertyChanged(nameof(AbilitiesEditTabSelected));
         OnPropertyChanged(nameof(ProficiencyEditTabSelected));
      }

      private void SelectProficiencyEdit()
      {
         _infoEditTabSelected = false;
         _levelsEditTabSelected = false;
         _statsEditTabSelected = false;
         _proficiencyEditTabSelected = true;

         OnPropertyChanged(nameof(InfoEditTabSelected));
         OnPropertyChanged(nameof(LevelsEditTabSelected));
         OnPropertyChanged(nameof(AbilitiesEditTabSelected));
         OnPropertyChanged(nameof(ProficiencyEditTabSelected));
      }

      private void ExportCharacter(CharacterViewModel characterViewModel)
      {
         Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
         saveFileDialog.Filter = "PDF Document|*.pdf|Character Archive|*.ccca";
         saveFileDialog.Title = "Save Character";
         saveFileDialog.FileName = characterViewModel.Name;

         if (saveFileDialog.ShowDialog() == true)
         {
            try
            {
               Mouse.OverrideCursor = Cursors.Wait;

               string ext = Path.GetExtension(saveFileDialog.FileName);

               if (ext == ".pdf")
               {
                  _documentService.CreateCharacterPDF(saveFileDialog.FileName, characterViewModel.CharacterModel);
               }
               else if (ext == ".ccca")
               {
                  byte[] bytes = _dataManager.CreateCharacterArchive(characterViewModel.CharacterModel);
                  File.WriteAllBytes(saveFileDialog.FileName, bytes);
               }
               else
               {
                  _dialogService.ShowConfirmationDialog("Unable To Export", "Invalid file extension.", "OK", null, null);
               }

               Mouse.OverrideCursor = null;
            }
            catch (Exception)
            {
               Mouse.OverrideCursor = null;

               _dialogService.ShowConfirmationDialog("Unable To Export", "An error occurred when attempting to export the character. The file may be open in another program.", "OK", null, null);
            }
         }
      }

      private void _compendium_CharacterChanged(object sender, CompendiumChangeEventArgs e)
      {
         if (_selectedCharacter != null && e.IDs.Contains(_selectedCharacter.CharacterModel.Id))
         {
            _selectedCharacter = new CharacterViewModel(_selectedCharacter.CharacterModel);
         }
         foreach (CharacterListItemViewModel listItem in _characters)
         {
            if (e.IDs.Contains(listItem.CharacterModel.Id))
            {
               listItem.UpdateModel(listItem.CharacterModel);
            }
         }
      }

      private void _selectedCharacter_LevelUp(object sender, EventArgs e)
      {
         foreach (CharacterListItemViewModel listItem in _characters)
         {
            if (listItem.CharacterModel.Id == _selectedCharacter.CharacterModel.Id)
            {
               listItem.UpdateModel(_selectedCharacter.CharacterModel);
               break;
            }
         }
      }

      private void Import()
      {
         _dialogService.ShowImportView();
      }

      private void SelectNext()
      {
         if (_characters.Any())
         {
            CharacterListItemViewModel selected = _characters.FirstOrDefault(x => x.IsSelected);

            foreach (CharacterListItemViewModel character in _characters)
            {
               character.IsSelected = false;
            }

            if (selected == null)
            {
               _characters[0].IsSelected = true;
               _selectedCharacter = new CharacterViewModel(_characters[0].CharacterModel);
            }
            else
            {
               int index = Math.Min(_characters.IndexOf(selected) + 1, _characters.Count - 1);
               _characters[index].IsSelected = true;
               _selectedCharacter = new CharacterViewModel(_characters[index].CharacterModel);
            }

            OnPropertyChanged(nameof(SelectedCharacter));
         }
      }

      private void SelectPrevious()
      {
         if (_characters.Any())
         {
            CharacterListItemViewModel selected = _characters.FirstOrDefault(x => x.IsSelected);

            foreach (CharacterListItemViewModel character in _characters)
            {
               character.IsSelected = false;
            }

            if (selected == null)
            {
               _characters[_characters.Count - 1].IsSelected = true;
               _selectedCharacter = new CharacterViewModel(_characters[_characters.Count - 1].CharacterModel);
            }
            else
            {
               int index = Math.Max(_characters.IndexOf(selected) - 1, 0);
               _characters[index].IsSelected = true;
               _selectedCharacter = new CharacterViewModel(_characters[index].CharacterModel);
            }

            OnPropertyChanged(nameof(SelectedCharacter));
         }
      }

      #endregion
   }
}
