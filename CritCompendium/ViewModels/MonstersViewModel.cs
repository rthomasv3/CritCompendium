using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Persistence;
using CritCompendiumInfrastructure.Services;
using CritCompendiumInfrastructure.Services.Search;
using CritCompendiumInfrastructure.Services.Search.Input;
using CritCompendium.Services;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendium.ViewModels.ObjectViewModels;

namespace CritCompendium.ViewModels
{
   public sealed class MonstersViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly Compendium _compendium;
      private readonly MonsterSearchService _monsterSearchService;
      private readonly MonsterSearchInput _monsterSearchInput;
      private readonly StringService _stringService;
      private readonly DialogService _dialogService;
      private readonly XMLImporter _xmlImporter;
      private readonly XMLExporter _xmlExporter;
      private readonly DocumentService _documentService;
      private readonly ObservableCollection<MonsterListItemViewModel> _monsters = new ObservableCollection<MonsterListItemViewModel>();
      private readonly ICommand _selectMonsterCommand;
      private readonly ICommand _editMonsterCommand;
      private readonly ICommand _exportMonsterCommand;
      private readonly ICommand _cancelEditMonsterCommand;
      private readonly ICommand _saveEditMonsterCommand;
      private readonly ICommand _resetFiltersCommand;
      private readonly ICommand _addCommand;
      private readonly ICommand _copyCommand;
      private readonly ICommand _deleteCommand;
      private readonly ICommand _importCommand;
      private readonly ICommand _selectNextCommand;
      private readonly ICommand _selectPreviousCommand;
      private MonsterViewModel _selectedMonster;
      private string _editMonsterXML;
      private bool _editHasUnsavedChanges;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates an instance of <see cref="MonstersViewModel"/>
      /// </summary>
      public MonstersViewModel(Compendium compendium, MonsterSearchService monsterSearchService, MonsterSearchInput monsterSearchInput,
          StringService stringService, DialogService dialogService, XMLImporter xmlImporter, XMLExporter xmlExporter, DocumentService documentService)
      {
         _compendium = compendium;
         _monsterSearchService = monsterSearchService;
         _monsterSearchInput = monsterSearchInput;
         _stringService = stringService;
         _dialogService = dialogService;
         _xmlImporter = xmlImporter;
         _xmlExporter = xmlExporter;
         _documentService = documentService;

         _selectMonsterCommand = new RelayCommand(obj => true, obj => SelectMonster(obj as MonsterListItemViewModel));
         _editMonsterCommand = new RelayCommand(obj => true, obj => EditMonster(obj as MonsterViewModel));
         _exportMonsterCommand = new RelayCommand(obj => true, obj => ExportMonster(obj as MonsterViewModel));
         _cancelEditMonsterCommand = new RelayCommand(obj => true, obj => CancelEditMonster());
         _saveEditMonsterCommand = new RelayCommand(obj => HasUnsavedChanges, obj => SaveEditMonster());
         _resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
         _addCommand = new RelayCommand(obj => true, obj => Add());
         _copyCommand = new RelayCommand(obj => _selectedMonster != null, obj => Copy());
         _deleteCommand = new RelayCommand(obj => _selectedMonster != null, obj => Delete());
         _importCommand = new RelayCommand(obj => true, obj => Import());
         _selectNextCommand = new RelayCommand(obj => true, obj => SelectNext());
         _selectPreviousCommand = new RelayCommand(obj => true, obj => SelectPrevious());

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
      /// Command to select a monster
      /// </summary>
      public ICommand SelectMonsterCommand
      {
         get { return _selectMonsterCommand; }
      }

      /// <summary>
      /// Selected monster
      /// </summary>
      public MonsterViewModel SelectedMonster
      {
         get { return _selectedMonster; }
      }

      /// <summary>
      /// Editing monster xml
      /// </summary>
      public string EditingMonsterXML
      {
         get { return _editMonsterXML; }
         set
         {
            if (Set(ref _editMonsterXML, value) && !_editHasUnsavedChanges)
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
      /// Command to edit monster
      /// </summary>
      public ICommand EditMonsterCommand
      {
         get { return _editMonsterCommand; }
      }

      /// <summary>
      /// Command to export monster
      /// </summary>
      public ICommand ExportMonsterCommand
      {
         get { return _exportMonsterCommand; }
      }

      /// <summary>
      /// Command to cancel edit monster
      /// </summary>
      public ICommand CancelEditMonsterCommand
      {
         get { return _cancelEditMonsterCommand; }
      }

      /// <summary>
      /// Command to save edit monster
      /// </summary>
      public ICommand SaveEditMonsterCommand
      {
         get { return _saveEditMonsterCommand; }
      }

      /// <summary>
      /// Command to add monster
      /// </summary>
      public ICommand AddMonsterCommand
      {
         get { return _addCommand; }
      }

      /// <summary>
      /// Command to copy monster
      /// </summary>
      public ICommand CopyMonsterCommand
      {
         get { return _copyCommand; }
      }

      /// <summary>
      /// Command to delete selected monster
      /// </summary>
      public ICommand DeleteMonsterCommand
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
      /// True if currently editing a monster
      /// </summary>
      public bool IsEditingMonster
      {
         get { return _editMonsterXML != null; }
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
         _monsters.Clear();
         foreach (MonsterModel monsterModel in _monsterSearchService.Search(_monsterSearchInput))
         {
            _monsters.Add(new MonsterListItemViewModel(monsterModel, _stringService));
         }

         if (_selectedMonster != null)
         {
            MonsterListItemViewModel monster = _monsters.FirstOrDefault(x => x.MonsterModel.ID == _selectedMonster.MonsterModel.ID);
            if (monster != null)
            {
               monster.IsSelected = true;
            }
         }

         OnPropertyChanged(nameof(SortAndFilterHeader));
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

      private void SelectMonster(MonsterListItemViewModel monster)
      {
         bool selectMonster = true;

         if (_editMonsterXML != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                                    _selectedMonster.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditMonster())
                  {
                     selectMonster = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditMonster();
               }
               else
               {
                  selectMonster = false;
               }
            }
            else
            {
               CancelEditMonster();
            }
         }

         if (selectMonster)
         {
            foreach (MonsterListItemViewModel item in _monsters)
            {
               item.IsSelected = false;
            }
            monster.IsSelected = true;

            _selectedMonster = new MonsterViewModel(monster.MonsterModel);
            OnPropertyChanged(nameof(SelectedMonster));
         }
      }

      private void EditMonster(MonsterViewModel monster)
      {
         _editMonsterXML = monster.XML;

         OnPropertyChanged(nameof(EditingMonsterXML));
         OnPropertyChanged(nameof(IsEditingMonster));
      }

      private void CancelEditMonster()
      {
         _editHasUnsavedChanges = false;
         _editMonsterXML = null;

         OnPropertyChanged(nameof(EditingMonsterXML));
         OnPropertyChanged(nameof(IsEditingMonster));
         OnPropertyChanged(nameof(HasUnsavedChanges));
      }

      private bool SaveEditMonster()
      {
         bool saved = false;

         try
         {
            MonsterModel model = _xmlImporter.GetMonster(_editMonsterXML);

            if (model != null)
            {
               model.ID = _selectedMonster.MonsterModel.ID;
               _compendium.UpdateMonster(model);
               _selectedMonster = new MonsterViewModel(model);

               MonsterListItemViewModel oldListItem = _monsters.FirstOrDefault(x => x.MonsterModel.ID == model.ID);
               if (oldListItem != null)
               {
                  if (_monsterSearchService.SearchInputApplies(_monsterSearchInput, model))
                  {
                     oldListItem.UpdateModel(model);
                  }
                  else
                  {
                     _monsters.Remove(oldListItem);
                  }
               }

               _editMonsterXML = null;
               _editHasUnsavedChanges = false;

               SortMonsters();

               _compendium.SaveMonsters();

               OnPropertyChanged(nameof(SelectedMonster));
               OnPropertyChanged(nameof(EditingMonsterXML));
               OnPropertyChanged(nameof(IsEditingMonster));
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
         bool addMonster = true;

         if (_editMonsterXML != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                                    _selectedMonster.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditMonster())
                  {
                     addMonster = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditMonster();
               }
               else
               {
                  addMonster = false;
               }
            }
            else
            {
               CancelEditMonster();
            }
         }

         if (addMonster)
         {
            string xml = @"<name>New Monster</name>
									<size></size>
									<type></type>
									<alignment></alignment>
									<ac></ac>
									<hp></hp>
									<speed></speed>
									<str></str>
									<dex></dex>
									<con></con>
									<int></int>
									<wis></wis>
									<cha></cha>
									<save></save>
									<skill></skill>
									<resist></resist>
									<vulnerable></vulnerable>
									<immune></immune>
									<conditionImmune></conditionImmune>
									<senses></senses>
									<passive></passive>
									<languages></languages>
									<cr></cr>
									<trait>
										 <name></name>
										 <text></text>
									</trait>
									<action>
										 <name></name>
										 <text></text>
										 <attack></attack>
									</action>
									<legendary>
										 <name></name>
										 <text></text>
									</legendary>
									<spells></spells>
									<slots></slots>
									<environment></environment>";

            MonsterModel monsterModel = _xmlImporter.GetMonster(xml);

            _compendium.AddMonster(monsterModel);

            if (_monsterSearchService.SearchInputApplies(_monsterSearchInput, monsterModel))
            {
               MonsterListItemViewModel listItem = new MonsterListItemViewModel(monsterModel, _stringService);
               _monsters.Add(listItem);
               foreach (MonsterListItemViewModel item in _monsters)
               {
                  item.IsSelected = false;
               }
               listItem.IsSelected = true;
            }

            _selectedMonster = new MonsterViewModel(monsterModel);

            _editMonsterXML = monsterModel.XML;

            SortMonsters();

            _compendium.SaveMonsters();

            OnPropertyChanged(nameof(EditingMonsterXML));
            OnPropertyChanged(nameof(IsEditingMonster));
            OnPropertyChanged(nameof(SelectedMonster));
         }
      }

      private void Copy()
      {
         if (_selectedMonster != null)
         {
            bool copyMonster = true;

            if (_editMonsterXML != null)
            {
               if (_editHasUnsavedChanges)
               {
                  string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                                       _selectedMonster.Name, Environment.NewLine + Environment.NewLine);
                  string accept = "Save and Continue";
                  string reject = "Discard Changes";
                  string cancel = "Cancel Navigation";
                  bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                  if (result == true)
                  {
                     if (!SaveEditMonster())
                     {
                        copyMonster = false;
                     }
                  }
                  else if (result == false)
                  {
                     CancelEditMonster();
                  }
                  else
                  {
                     copyMonster = false;
                  }
               }
               else
               {
                  CancelEditMonster();
               }
            }

            if (copyMonster)
            {
               MonsterModel newMonster = new MonsterModel(_selectedMonster.MonsterModel);
               newMonster.Name += " (copy)";
               newMonster.ID = Guid.NewGuid();
               newMonster.XML = newMonster.XML.Replace("<name>" + _selectedMonster.MonsterModel.Name + "</name>",
                                                                    "<name>" + newMonster.Name + "</name>");

               _compendium.AddMonster(newMonster);

               if (_monsterSearchService.SearchInputApplies(_monsterSearchInput, newMonster))
               {
                  MonsterListItemViewModel listItem = new MonsterListItemViewModel(newMonster, _stringService);
                  _monsters.Add(listItem);
                  foreach (MonsterListItemViewModel item in _monsters)
                  {
                     item.IsSelected = false;
                  }
                  listItem.IsSelected = true;
               }

               _selectedMonster = new MonsterViewModel(newMonster);

               SortMonsters();

               _compendium.SaveMonsters();

               OnPropertyChanged(nameof(SelectedMonster));
            }
         }
      }

      private void Delete()
      {
         if (_selectedMonster != null)
         {
            bool canDelete = true;

            foreach (CharacterModel character in _compendium.Characters)
            {
               foreach (CompanionModel companion in character.Companions)
               {
                  if (companion.MonsterModel != null && companion.MonsterModel.ID == _selectedMonster.MonsterModel.ID)
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
               foreach (EncounterModel encounter in _compendium.Encounters)
               {
                  foreach (EncounterMonsterModel encounterMonster in encounter.Creatures.Where(x => x is EncounterMonsterModel))
                  {
                     if (encounterMonster.MonsterModel != null && encounterMonster.MonsterModel.ID == _selectedMonster.MonsterModel.ID)
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
            }

            if (canDelete)
            {
               string message = String.Format("Are you sure you want to delete {0}?",
                                                            _selectedMonster.Name);

               bool? result = _dialogService.ShowConfirmationDialog("Delete Monster", message, "Yes", "No", null);

               if (result == true)
               {
                  _compendium.DeleteMonster(_selectedMonster.MonsterModel.ID);

                  MonsterListItemViewModel listItem = _monsters.FirstOrDefault(x => x.MonsterModel.ID == _selectedMonster.MonsterModel.ID);
                  if (listItem != null)
                  {
                     _monsters.Remove(listItem);
                  }

                  _selectedMonster = null;

                  _compendium.SaveMonsters();

                  OnPropertyChanged(nameof(SelectedMonster));

                  if (_editMonsterXML != null)
                  {
                     CancelEditMonster();
                  }
               }
            }
            else
            {
               _dialogService.ShowConfirmationDialog("Unable Delete Monster", "Monster is in use by a character or encounter.", "OK", null, null);
            }
         }
      }

      private void SortMonsters()
      {
         if (_monsters != null && _monsters.Count > 0)
         {
            List<MonsterModel> monsters = _monsterSearchService.Sort(_monsters.Select(x => x.MonsterModel), _monsterSearchInput.SortOption.Key);
            for (int i = 0; i < monsters.Count; ++i)
            {
               if (monsters[i].ID != _monsters[i].MonsterModel.ID)
               {
                  MonsterListItemViewModel monster = _monsters.FirstOrDefault(x => x.MonsterModel.ID == monsters[i].ID);
                  if (monster != null)
                  {
                     _monsters.Move(_monsters.IndexOf(monster), i);
                  }
               }
            }
         }
      }

      private void ExportMonster(MonsterViewModel monsterViewModel)
      {
         Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
         saveFileDialog.Filter = "Word Document|*.docx|XML Document|*.xml";
         saveFileDialog.Title = "Save Monster";
         saveFileDialog.FileName = monsterViewModel.Name;

         if (saveFileDialog.ShowDialog() == true)
         {
            try
            {
               string ext = System.IO.Path.GetExtension(saveFileDialog.FileName);

               if (ext == ".docx")
               {
                  _documentService.CreateWordDoc(saveFileDialog.FileName, monsterViewModel.MonsterModel);
               }
               else if (ext == ".xml")
               {
                  string xml = _xmlExporter.FormatXMLWithHeader(_xmlExporter.GetXML(monsterViewModel.MonsterModel));
                  System.IO.File.WriteAllText(saveFileDialog.FileName, xml, System.Text.Encoding.UTF8);
               }
               else
               {
                  _dialogService.ShowConfirmationDialog("Unable To Export", "Invalid file extension.", "OK", null, null);
               }
            }
            catch (Exception)
            {
               _dialogService.ShowConfirmationDialog("Unable To Export", "An error occurred when attempting to export the monster.", "OK", null, null);
            }
         }
      }

      private void Import()
      {
         _dialogService.ShowImportView();
      }

      private void SelectNext()
      {
         if (_monsters.Any())
         {
            MonsterListItemViewModel selected = _monsters.FirstOrDefault(x => x.IsSelected);

            foreach (MonsterListItemViewModel monster in _monsters)
            {
               monster.IsSelected = false;
            }

            if (selected == null)
            {
               _monsters[0].IsSelected = true;
               _selectedMonster = new MonsterViewModel(_monsters[0].MonsterModel);
            }
            else
            {
               int index = Math.Min(_monsters.IndexOf(selected) + 1, _monsters.Count - 1);
               _monsters[index].IsSelected = true;
               _selectedMonster = new MonsterViewModel(_monsters[index].MonsterModel);
            }

            OnPropertyChanged(nameof(SelectedMonster));
         }
      }

      private void SelectPrevious()
      {
         if (_monsters.Any())
         {
            MonsterListItemViewModel selected = _monsters.FirstOrDefault(x => x.IsSelected);

            foreach (MonsterListItemViewModel monster in _monsters)
            {
               monster.IsSelected = false;
            }

            if (selected == null)
            {
               _monsters[_monsters.Count - 1].IsSelected = true;
               _selectedMonster = new MonsterViewModel(_monsters[_monsters.Count - 1].MonsterModel);
            }
            else
            {
               int index = Math.Max(_monsters.IndexOf(selected) - 1, 0);
               _monsters[index].IsSelected = true;
               _selectedMonster = new MonsterViewModel(_monsters[index].MonsterModel);
            }

            OnPropertyChanged(nameof(SelectedMonster));
         }
      }

      #endregion
   }
}
