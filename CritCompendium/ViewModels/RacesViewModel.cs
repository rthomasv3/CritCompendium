using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Persistence;
using CritCompendiumInfrastructure.Business;
using CritCompendiumInfrastructure.Business.Search;
using CritCompendiumInfrastructure.Business.Search.Input;
using CritCompendium.Business;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendium.ViewModels.ObjectViewModels;

namespace CritCompendium.ViewModels
{
   public sealed class RacesViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly Compendium _compendium;
      private readonly RaceSearchService _raceSearchService;
      private readonly StringService _stringService;
      private readonly DialogService _dialogService;
      private readonly XMLImporter _xmlImporter;
      private readonly XMLExporter _xmlExporter;
      private readonly ObservableCollection<RaceListItemViewModel> _races = new ObservableCollection<RaceListItemViewModel>();
      private readonly ICommand _selectRaceCommand;
      private readonly ICommand _editRaceCommand;
      private readonly ICommand _exportRaceCommand;
      private readonly ICommand _cancelEditRaceCommand;
      private readonly ICommand _saveEditRaceCommand;
      private readonly ICommand _resetFiltersCommand;
      private readonly ICommand _addCommand;
      private readonly ICommand _copyCommand;
      private readonly ICommand _deleteCommand;
      private readonly ICommand _importCommand;
      private readonly ICommand _selectNextCommand;
      private readonly ICommand _selectPreviousCommand;
      private RaceSearchInput _raceSearchInput;
      private RaceViewModel _selectedRace;
      private string _editRaceXML;
      private bool _editHasUnsavedChanges;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates an instance of <see cref="RacesViewModel"/>
      /// </summary>
      public RacesViewModel(Compendium compendium, RaceSearchService raceSearchService, RaceSearchInput raceSearchInput,
         StringService stringService, DialogService dialogService, XMLImporter xmlImporter, XMLExporter xmlExporter)
      {
         _compendium = compendium;
         _raceSearchService = raceSearchService;
         _raceSearchInput = raceSearchInput;
         _stringService = stringService;
         _dialogService = dialogService;
         _xmlImporter = xmlImporter;
         _xmlExporter = xmlExporter;

         _selectRaceCommand = new RelayCommand(obj => true, obj => SelectRace(obj as RaceListItemViewModel));
         _editRaceCommand = new RelayCommand(obj => true, obj => EditRace(obj as RaceViewModel));
         _exportRaceCommand = new RelayCommand(obj => true, obj => ExportRace(obj as RaceViewModel));
         _cancelEditRaceCommand = new RelayCommand(obj => true, obj => CancelEditRace());
         _saveEditRaceCommand = new RelayCommand(obj => HasUnsavedChanges, obj => SaveEditRace());
         _resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
         _addCommand = new RelayCommand(obj => true, obj => Add());
         _copyCommand = new RelayCommand(obj => _selectedRace != null, obj => Copy());
         _deleteCommand = new RelayCommand(obj => _selectedRace != null, obj => Delete());
         _importCommand = new RelayCommand(obj => true, obj => Import());
         _selectNextCommand = new RelayCommand(obj => true, obj => SelectNext());
         _selectPreviousCommand = new RelayCommand(obj => true, obj => SelectPrevious());

         Search();
      }

      #endregion

      #region Properties

      /// <summary>
      /// List of races
      /// </summary>
      public ObservableCollection<RaceListItemViewModel> Races
      {
         get { return _races; }
      }

      /// <summary>
      /// Gets or sets the search text
      /// </summary>
      public string SearchText
      {
         get { return _raceSearchInput.SearchText; }
         set
         {
            _raceSearchInput.SearchText = value;
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
            return _raceSearchInput.AppliedFilterCount > 0 ? $"Sort and Filter ({_raceSearchInput.AppliedFilterCount})" : "Sort and Filter";
         }
      }

      /// <summary>
      /// Gets or sets sort and filters expanded
      /// </summary>
      public bool SortAndFilterExpanded
      {
         get { return _raceSearchInput.SortAndFiltersExpanded; }
         set { _raceSearchInput.SortAndFiltersExpanded = value; }
      }

      /// <summary>
      /// Gets sort options
      /// </summary>
      public List<KeyValuePair<RaceSortOption, string>> SortOptions
      {
         get { return _raceSearchInput.SortOptions; }
      }

      /// <summary>
      /// Gets selected sort option
      /// </summary>
      public KeyValuePair<RaceSortOption, string> SelectedSortOption
      {
         get { return _raceSearchInput.SortOption; }
         set
         {
            _raceSearchInput.SortOption = value;
            Search();
         }
      }

      /// <summary>
      /// List of sizes
      /// </summary>
      public List<KeyValuePair<CreatureSize, string>> Sizes
      {
         get { return _raceSearchInput.Sizes; }
      }

      /// <summary>
      /// Selected size
      /// </summary>
      public KeyValuePair<CreatureSize, string> SelectedSize
      {
         get { return _raceSearchInput.Size; }
         set
         {
            _raceSearchInput.Size = value;
            Search();
         }
      }

      /// <summary>
      /// List of abilities
      /// </summary>
      public List<KeyValuePair<Ability, string>> Abilities
      {
         get { return _raceSearchInput.Abilities; }
      }

      /// <summary>
      /// Selected ability
      /// </summary>
      public KeyValuePair<Ability, string> SelectedAbility
      {
         get { return _raceSearchInput.Ability; }
         set
         {
            _raceSearchInput.Ability = value;
            Search();
         }
      }

      /// <summary>
      /// List of languages
      /// </summary>
      public List<KeyValuePair<LanguageModel, string>> Languages
      {
         get { return _raceSearchInput.Languages; }
      }

      /// <summary>
      /// Selected language
      /// </summary>
      public KeyValuePair<LanguageModel, string> SelectedLanguage
      {
         get { return _raceSearchInput.Language; }
         set
         {
            _raceSearchInput.Language = value;
            Search();
         }
      }

      /// <summary>
      /// Command to select a race
      /// </summary>
      public ICommand SelectRaceCommand
      {
         get { return _selectRaceCommand; }
      }

      /// <summary>
      /// Selected race
      /// </summary>
      public RaceViewModel SelectedRace
      {
         get { return _selectedRace; }
      }

      /// <summary>
      /// Editing race xml
      /// </summary>
      public string EditingRaceXML
      {
         get { return _editRaceXML; }
         set
         {
            if (Set(ref _editRaceXML, value) && !_editHasUnsavedChanges)
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
      /// Command to edit race
      /// </summary>
      public ICommand EditRaceCommand
      {
         get { return _editRaceCommand; }
      }

      /// <summary>
      /// Command to export race
      /// </summary>
      public ICommand ExportRaceCommand
      {
         get { return _exportRaceCommand; }
      }

      /// <summary>
      /// Command to cancel edit race
      /// </summary>
      public ICommand CancelEditRaceCommand
      {
         get { return _cancelEditRaceCommand; }
      }

      /// <summary>
      /// Command to save edit race
      /// </summary>
      public ICommand SaveEditRaceCommand
      {
         get { return _saveEditRaceCommand; }
      }

      /// <summary>
      /// Command to add race
      /// </summary>
      public ICommand AddRaceCommand
      {
         get { return _addCommand; }
      }

      /// <summary>
      /// Command to copy race
      /// </summary>
      public ICommand CopyRaceCommand
      {
         get { return _copyCommand; }
      }

      /// <summary>
      /// Command to delete selected race
      /// </summary>
      public ICommand DeleteRaceCommand
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
      /// True if currently editing a race
      /// </summary>
      public bool IsEditingRace
      {
         get { return _editRaceXML != null; }
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
         _races.Clear();
         foreach (RaceModel raceModel in _raceSearchService.Search(_raceSearchInput))
         {
            _races.Add(new RaceListItemViewModel(raceModel, _stringService));
         }

         if (_selectedRace != null)
         {
            RaceListItemViewModel race = _races.FirstOrDefault(x => x.RaceModel.Id == _selectedRace.RaceModel.Id);
            if (race != null)
            {
               race.IsSelected = true;
            }
         }

         OnPropertyChanged(nameof(SortAndFilterHeader));
      }

      #endregion

      #region Private Methods

      private void InitializeSearch()
      {
         _raceSearchInput.Reset();

         OnPropertyChanged(nameof(SearchText));
         OnPropertyChanged(nameof(SelectedSortOption));
         OnPropertyChanged(nameof(SelectedSize));
         OnPropertyChanged(nameof(SelectedAbility));
         OnPropertyChanged(nameof(SelectedLanguage));

         Search();
      }

      private void SelectRace(RaceListItemViewModel race)
      {
         bool selectRace = true;

         if (_editRaceXML != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                           _selectedRace.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditRace())
                  {
                     selectRace = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditRace();
               }
               else
               {
                  selectRace = false;
               }
            }
            else
            {
               CancelEditRace();
            }
         }

         if (selectRace)
         {
            foreach (RaceListItemViewModel item in _races)
            {
               item.IsSelected = false;
            }
            race.IsSelected = true;

            _selectedRace = new RaceViewModel(race.RaceModel);
            OnPropertyChanged(nameof(SelectedRace));
         }
      }

      private void EditRace(RaceViewModel race)
      {
         _editRaceXML = race.XML;

         OnPropertyChanged(nameof(EditingRaceXML));
         OnPropertyChanged(nameof(IsEditingRace));
      }

      private void CancelEditRace()
      {
         _editHasUnsavedChanges = false;
         _editRaceXML = null;

         OnPropertyChanged(nameof(EditingRaceXML));
         OnPropertyChanged(nameof(IsEditingRace));
         OnPropertyChanged(nameof(HasUnsavedChanges));
      }

      private bool SaveEditRace()
      {
         bool saved = false;

         try
         {
            RaceModel model = _xmlImporter.GetRace(_editRaceXML);

            if (model != null)
            {
               model.Id = _selectedRace.RaceModel.Id;
               _compendium.UpdateRace(model);
               _selectedRace = new RaceViewModel(model);

               RaceListItemViewModel oldListItem = _races.FirstOrDefault(x => x.RaceModel.Id == model.Id);
               if (oldListItem != null)
               {
                  if (_raceSearchService.SearchInputApplies(_raceSearchInput, model))
                  {
                     oldListItem.UpdateModel(model);
                  }
                  else
                  {
                     _races.Remove(oldListItem);
                  }
               }

               _editRaceXML = null;
               _editHasUnsavedChanges = false;

               SortRaces();

               _compendium.SaveRaces();

               OnPropertyChanged(nameof(SelectedRace));
               OnPropertyChanged(nameof(EditingRaceXML));
               OnPropertyChanged(nameof(IsEditingRace));
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
         bool addRace = true;

         if (_editRaceXML != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                           _selectedRace.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditRace())
                  {
                     addRace = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditRace();
               }
               else
               {
                  addRace = false;
               }
            }
            else
            {
               CancelEditRace();
            }
         }

         if (addRace)
         {
            string xml = @"<name>New Race</name>
									<size></size>
									<speed></speed>
									<ability></ability>
									<spellAbility></spellAbility>
									<proficiency></proficiency>
									<trait>
										 <name></name>
										 <text></text>
									</trait>";

            RaceModel raceModel = _xmlImporter.GetRace(xml);

            _compendium.AddRace(raceModel);

            if (_raceSearchService.SearchInputApplies(_raceSearchInput, raceModel))
            {
               RaceListItemViewModel listItem = new RaceListItemViewModel(raceModel, _stringService);
               _races.Add(listItem);
               foreach (RaceListItemViewModel item in _races)
               {
                  item.IsSelected = false;
               }
               listItem.IsSelected = true;
            }


            _selectedRace = new RaceViewModel(raceModel);

            _editRaceXML = raceModel.XML;

            SortRaces();

            _compendium.SaveRaces();

            OnPropertyChanged(nameof(EditingRaceXML));
            OnPropertyChanged(nameof(IsEditingRace));
            OnPropertyChanged(nameof(SelectedRace));
         }
      }

      private void Copy()
      {
         if (_selectedRace != null)
         {
            bool copyRace = true;

            if (_editRaceXML != null)
            {
               if (_editHasUnsavedChanges)
               {
                  string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                              _selectedRace.Name, Environment.NewLine + Environment.NewLine);
                  string accept = "Save and Continue";
                  string reject = "Discard Changes";
                  string cancel = "Cancel Navigation";
                  bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                  if (result == true)
                  {
                     if (!SaveEditRace())
                     {
                        copyRace = false;
                     }
                  }
                  else if (result == false)
                  {
                     CancelEditRace();
                  }
                  else
                  {
                     copyRace = false;
                  }
               }
               else
               {
                  CancelEditRace();
               }
            }

            if (copyRace)
            {
               RaceModel newRace = new RaceModel(_selectedRace.RaceModel);
               newRace.Name += " (copy)";
               newRace.Id = Guid.NewGuid();
               newRace.XML = newRace.XML.Replace("<name>" + _selectedRace.RaceModel.Name + "</name>",
                                                 "<name>" + newRace.Name + "</name>");

               _compendium.AddRace(newRace);

               if (_raceSearchService.SearchInputApplies(_raceSearchInput, newRace))
               {
                  RaceListItemViewModel listItem = new RaceListItemViewModel(newRace, _stringService);
                  _races.Add(listItem);
                  foreach (RaceListItemViewModel item in _races)
                  {
                     item.IsSelected = false;
                  }
                  listItem.IsSelected = true;
               }

               _selectedRace = new RaceViewModel(newRace);

               SortRaces();

               _compendium.SaveRaces();

               OnPropertyChanged(nameof(SelectedRace));
            }
         }
      }

      private void Delete()
      {
         if (_selectedRace != null)
         {
            bool canDelete = true;

            foreach (CharacterModel character in _compendium.Characters)
            {
               if (character.Race != null && character.Race.Id == _selectedRace.RaceModel.Id)
               {
                  canDelete = false;
                  break;
               }
            }

            if (canDelete)
            {
               string message = String.Format("Are you sure you want to delete {0}?", _selectedRace.Name);

               bool? result = _dialogService.ShowConfirmationDialog("Delete Race", message, "Yes", "No", null);

               if (result == true)
               {
                  _compendium.DeleteRace(_selectedRace.RaceModel.Id);

                  RaceListItemViewModel listItem = _races.FirstOrDefault(x => x.RaceModel.Id == _selectedRace.RaceModel.Id);
                  if (listItem != null)
                  {
                     _races.Remove(listItem);
                  }

                  _selectedRace = null;

                  _compendium.SaveRaces();

                  OnPropertyChanged(nameof(SelectedRace));

                  if (_editRaceXML != null)
                  {
                     CancelEditRace();
                  }
               }
            }
            else
            {
               _dialogService.ShowConfirmationDialog("Unable Delete Race", "Race is in use by a character.", "OK", null, null);
            }
         }
      }

      private void SortRaces()
      {
         if (_races != null && _races.Count > 0)
         {
            List<RaceModel> races = _raceSearchService.Sort(_races.Select(x => x.RaceModel), _raceSearchInput.SortOption.Key);
            for (int i = 0; i < races.Count; ++i)
            {
               if (races[i].Id != _races[i].RaceModel.Id)
               {
                  RaceListItemViewModel race = _races.FirstOrDefault(x => x.RaceModel.Id == races[i].Id);
                  if (race != null)
                  {
                     _races.Move(_races.IndexOf(race), i);
                  }
               }
            }
         }
      }

      private void ExportRace(RaceViewModel raceViewModel)
      {
         Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
         saveFileDialog.Filter = "XML Document|*.xml";
         saveFileDialog.Title = "Save Race";
         saveFileDialog.FileName = raceViewModel.Name;

         if (saveFileDialog.ShowDialog() == true)
         {
            try
            {
               string ext = System.IO.Path.GetExtension(saveFileDialog.FileName);

               if (ext == ".xml")
               {
                  string xml = _xmlExporter.FormatXMLWithHeader(_xmlExporter.GetXML(raceViewModel.RaceModel));
                  System.IO.File.WriteAllText(saveFileDialog.FileName, xml, System.Text.Encoding.UTF8);
               }
               else
               {
                  _dialogService.ShowConfirmationDialog("Unable To Export", "Invalid file extension.", "OK", null, null);
               }
            }
            catch (Exception)
            {
               _dialogService.ShowConfirmationDialog("Unable To Export", "An error occurred when attempting to export the race.", "OK", null, null);
            }
         }
      }

      private void Import()
      {
         _dialogService.ShowImportView();
      }

      private void SelectNext()
      {
         if (_races.Any())
         {
            RaceListItemViewModel selected = _races.FirstOrDefault(x => x.IsSelected);

            foreach (RaceListItemViewModel race in _races)
            {
               race.IsSelected = false;
            }

            if (selected == null)
            {
               _races[0].IsSelected = true;
               _selectedRace = new RaceViewModel(_races[0].RaceModel);
            }
            else
            {
               int index = Math.Min(_races.IndexOf(selected) + 1, _races.Count - 1);
               _races[index].IsSelected = true;
               _selectedRace = new RaceViewModel(_races[index].RaceModel);
            }

            OnPropertyChanged(nameof(SelectedRace));
         }
      }

      private void SelectPrevious()
      {
         if (_races.Any())
         {
            RaceListItemViewModel selected = _races.FirstOrDefault(x => x.IsSelected);

            foreach (RaceListItemViewModel race in _races)
            {
               race.IsSelected = false;
            }

            if (selected == null)
            {
               _races[_races.Count - 1].IsSelected = true;
               _selectedRace = new RaceViewModel(_races[_races.Count - 1].RaceModel);
            }
            else
            {
               int index = Math.Max(_races.IndexOf(selected) - 1, 0);
               _races[index].IsSelected = true;
               _selectedRace = new RaceViewModel(_races[index].RaceModel);
            }

            OnPropertyChanged(nameof(SelectedRace));
         }
      }

      #endregion
   }
}
