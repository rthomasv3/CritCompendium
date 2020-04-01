using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Persistence;
using CritCompendiumInfrastructure.Business;
using CritCompendiumInfrastructure.Business.Search;
using CritCompendiumInfrastructure.Business.Search.Input;
using CritCompendium.Business;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendium.ViewModels
{
   public sealed class FeatsViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly Compendium _compendium;
      private readonly FeatSearchService _featSearchService;
      private readonly FeatSearchInput _featSearchInput;
      private readonly StringService _stringService;
      private readonly DialogService _dialogService;
      private readonly XMLImporter _xmlImporter;
      private readonly XMLExporter _xmlExporter;
      private readonly ObservableCollection<FeatListItemViewModel> _feats = new ObservableCollection<FeatListItemViewModel>();
      private readonly ICommand _selectFeatCommand;
      private readonly ICommand _editFeatCommand;
      private readonly ICommand _exportFeatCommand;
      private readonly ICommand _cancelEditFeatCommand;
      private readonly ICommand _saveEditFeatCommand;
      private readonly ICommand _resetFiltersCommand;
      private readonly ICommand _addCommand;
      private readonly ICommand _copyCommand;
      private readonly ICommand _deleteCommand;
      private readonly ICommand _importCommand;
      private readonly ICommand _selectNextCommand;
      private readonly ICommand _selectPreviousCommand;
      private FeatViewModel _selectedFeat;
      private string _editFeatXML;
      private bool _editHasUnsavedChanges;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates an instance of <see cref="FeatsViewModel"/>
      /// </summary>
      public FeatsViewModel(Compendium compendium, FeatSearchService classSearchService, FeatSearchInput featSearchInput,
         StringService stringService, DialogService dialogService, XMLImporter xmlImporter, XMLExporter xmlExporter)
      {
         _compendium = compendium;
         _featSearchService = classSearchService;
         _featSearchInput = featSearchInput;
         _stringService = stringService;
         _dialogService = dialogService;
         _xmlImporter = xmlImporter;
         _xmlExporter = xmlExporter;

         _selectFeatCommand = new RelayCommand(obj => true, obj => SelectFeat(obj as FeatListItemViewModel));
         _editFeatCommand = new RelayCommand(obj => true, obj => EditFeat(obj as FeatViewModel));
         _exportFeatCommand = new RelayCommand(obj => true, obj => ExportFeat(obj as FeatViewModel));
         _cancelEditFeatCommand = new RelayCommand(obj => true, obj => CancelEditFeat());
         _saveEditFeatCommand = new RelayCommand(obj => HasUnsavedChanges, obj => SaveEditFeat());
         _resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
         _addCommand = new RelayCommand(obj => true, obj => Add());
         _copyCommand = new RelayCommand(obj => _selectedFeat != null, obj => Copy());
         _deleteCommand = new RelayCommand(obj => _selectedFeat != null, obj => Delete());
         _importCommand = new RelayCommand(obj => true, obj => Import());
         _selectNextCommand = new RelayCommand(obj => true, obj => SelectNext());
         _selectPreviousCommand = new RelayCommand(obj => true, obj => SelectPrevious());

         Search();
      }

      #endregion

      #region Properties

      /// <summary>
      /// List of classes
      /// </summary>
      public ObservableCollection<FeatListItemViewModel> Feats
      {
         get { return _feats; }
      }

      /// <summary>
      /// Gets or sets the search text
      /// </summary>
      public string SearchText
      {
         get { return _featSearchInput.SearchText; }
         set
         {
            _featSearchInput.SearchText = value;
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
            return _featSearchInput.AppliedFilterCount > 0 ? $"Sort and Filter ({_featSearchInput.AppliedFilterCount})" : "Sort and Filter";
         }
      }

      /// <summary>
      /// Gets or sets sort and filters expanded
      /// </summary>
      public bool SortAndFilterExpanded
      {
         get { return _featSearchInput.SortAndFiltersExpanded; }
         set { _featSearchInput.SortAndFiltersExpanded = value; }
      }

      /// <summary>
      /// Gets sort options
      /// </summary>
      public List<KeyValuePair<FeatSortOption, string>> SortOptions
      {
         get { return _featSearchInput.SortOptions; }
      }

      /// <summary>
      /// Gets selected sort option
      /// </summary>
      public KeyValuePair<FeatSortOption, string> SelectedSortOption
      {
         get { return _featSearchInput.SortOption; }
         set
         {
            _featSearchInput.SortOption = value;
            Search();
         }
      }

      /// <summary>
      /// List of prerequisite options
      /// </summary>
      public List<KeyValuePair<bool, string>> PrerequisiteOptions
      {
         get { return _featSearchInput.PrerequisiteOptions; }
      }

      /// <summary>
      /// Selected prerequisite option
      /// </summary>
      public KeyValuePair<bool, string> SelectedPrerequisite
      {
         get { return _featSearchInput.Prerequisite; }
         set
         {
            _featSearchInput.Prerequisite = value;
            Search();
         }
      }

      /// <summary>
      /// Command to select a feat
      /// </summary>
      public ICommand SelectFeatCommand
      {
         get { return _selectFeatCommand; }
      }

      /// <summary>
      /// Selected feat
      /// </summary>
      public FeatViewModel SelectedFeat
      {
         get { return _selectedFeat; }
      }

      /// <summary>
      /// Editing feat xml
      /// </summary>
      public string EditingFeatXML
      {
         get { return _editFeatXML; }
         set
         {
            if (Set(ref _editFeatXML, value) && !_editHasUnsavedChanges)
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
      /// Command to edit feat
      /// </summary>
      public ICommand EditFeatCommand
      {
         get { return _editFeatCommand; }
      }

      /// <summary>
      /// Command to export feat
      /// </summary>
      public ICommand ExportFeatCommand
      {
         get { return _exportFeatCommand; }
      }

      /// <summary>
      /// Command to cancel edit feat
      /// </summary>
      public ICommand CancelEditFeatCommand
      {
         get { return _cancelEditFeatCommand; }
      }

      /// <summary>
      /// Command to save edit feat
      /// </summary>
      public ICommand SaveEditFeatCommand
      {
         get { return _saveEditFeatCommand; }
      }

      /// <summary>
      /// Command to add feat
      /// </summary>
      public ICommand AddFeatCommand
      {
         get { return _addCommand; }
      }

      /// <summary>
      /// Command to copy feat
      /// </summary>
      public ICommand CopyFeatCommand
      {
         get { return _copyCommand; }
      }

      /// <summary>
      /// Command to delete selected feat
      /// </summary>
      public ICommand DeleteFeatCommand
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
      /// True if currently editing a feat
      /// </summary>
      public bool IsEditingFeat
      {
         get { return _editFeatXML != null; }
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
         _feats.Clear();
         foreach (FeatModel featModel in _featSearchService.Search(_featSearchInput))
         {
            _feats.Add(new FeatListItemViewModel(featModel));
         }

         if (_selectedFeat != null)
         {
            FeatListItemViewModel feat = _feats.FirstOrDefault(x => x.FeatModel.ID == _selectedFeat.FeatModel.ID);
            if (feat != null)
            {
               feat.IsSelected = true;
            }
         }

         OnPropertyChanged(nameof(SortAndFilterHeader));
      }

      #endregion

      #region Private Methods

      private void InitializeSearch()
      {
         _featSearchInput.Reset();

         OnPropertyChanged(nameof(SearchText));
         OnPropertyChanged(nameof(SelectedSortOption));
         OnPropertyChanged(nameof(SelectedPrerequisite));

         Search();
      }

      private void SelectFeat(FeatListItemViewModel featItem)
      {
         bool selectFeat = true;

         if (_editFeatXML != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                           _selectedFeat.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditFeat())
                  {
                     selectFeat = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditFeat();
               }
               else
               {
                  selectFeat = false;
               }
            }
            else
            {
               CancelEditFeat();
            }
         }

         if (selectFeat)
         {
            foreach (FeatListItemViewModel item in _feats)
            {
               item.IsSelected = false;
            }
            featItem.IsSelected = true;

            _selectedFeat = new FeatViewModel(featItem.FeatModel);
            OnPropertyChanged(nameof(SelectedFeat));
         }
      }

      private void EditFeat(FeatViewModel featModel)
      {
         _editFeatXML = featModel.XML;

         OnPropertyChanged(nameof(EditingFeatXML));
         OnPropertyChanged(nameof(IsEditingFeat));
      }

      private void CancelEditFeat()
      {
         _editHasUnsavedChanges = false;
         _editFeatXML = null;

         OnPropertyChanged(nameof(EditingFeatXML));
         OnPropertyChanged(nameof(IsEditingFeat));
         OnPropertyChanged(nameof(HasUnsavedChanges));
      }

      private bool SaveEditFeat()
      {
         bool saved = false;

         try
         {
            FeatModel model = _xmlImporter.GetFeat(_editFeatXML);

            if (model != null)
            {
               model.ID = _selectedFeat.FeatModel.ID;
               _compendium.UpdateFeat(model);
               _selectedFeat = new FeatViewModel(model);

               FeatListItemViewModel oldListItem = _feats.FirstOrDefault(x => x.FeatModel.ID == model.ID);
               if (oldListItem != null)
               {
                  if (_featSearchService.SearchInputApplies(_featSearchInput, model))
                  {
                     oldListItem.UpdateModel(model);
                  }
                  else
                  {
                     _feats.Remove(oldListItem);
                  }
               }

               _editFeatXML = null;
               _editHasUnsavedChanges = false;

               SortFeats();

               _compendium.SaveFeats();

               OnPropertyChanged(nameof(SelectedFeat));
               OnPropertyChanged(nameof(EditingFeatXML));
               OnPropertyChanged(nameof(IsEditingFeat));
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
         bool addFeat = true;

         if (_editFeatXML != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                           _selectedFeat.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditFeat())
                  {
                     addFeat = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditFeat();
               }
               else
               {
                  addFeat = false;
               }
            }
            else
            {
               CancelEditFeat();
            }
         }

         if (addFeat)
         {
            string xml = "<name>New Feat</name><prerequisite></prerequisite><text></text>";

            FeatModel featModel = _xmlImporter.GetFeat(xml);

            _compendium.AddFeat(featModel);

            if (_featSearchService.SearchInputApplies(_featSearchInput, featModel))
            {
               FeatListItemViewModel listItem = new FeatListItemViewModel(featModel);
               _feats.Add(listItem);
               foreach (FeatListItemViewModel item in _feats)
               {
                  item.IsSelected = false;
               }
               listItem.IsSelected = true;
            }

            _selectedFeat = new FeatViewModel(featModel);

            _editFeatXML = featModel.XML;

            SortFeats();

            _compendium.SaveFeats();

            OnPropertyChanged(nameof(EditingFeatXML));
            OnPropertyChanged(nameof(IsEditingFeat));
            OnPropertyChanged(nameof(SelectedFeat));
         }
      }

      private void Copy()
      {
         if (_selectedFeat != null)
         {
            bool copyFeat = true;

            if (_editFeatXML != null)
            {
               if (_editHasUnsavedChanges)
               {
                  string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                              _selectedFeat.Name, Environment.NewLine + Environment.NewLine);
                  string accept = "Save and Continue";
                  string reject = "Discard Changes";
                  string cancel = "Cancel Navigation";
                  bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                  if (result == true)
                  {
                     if (!SaveEditFeat())
                     {
                        copyFeat = false;
                     }
                  }
                  else if (result == false)
                  {
                     CancelEditFeat();
                  }
                  else
                  {
                     copyFeat = false;
                  }
               }
               else
               {
                  CancelEditFeat();
               }
            }

            if (copyFeat)
            {
               FeatModel newFeat = new FeatModel(_selectedFeat.FeatModel);
               newFeat.Name += " (copy)";
               newFeat.ID = Guid.NewGuid();
               newFeat.XML = newFeat.XML.Replace("<name>" + _selectedFeat.FeatModel.Name + "</name>",
                                                 "<name>" + newFeat.Name + "</name>");

               _compendium.AddFeat(newFeat);

               if (_featSearchService.SearchInputApplies(_featSearchInput, newFeat))
               {
                  FeatListItemViewModel listItem = new FeatListItemViewModel(newFeat);
                  _feats.Add(listItem);
                  foreach (FeatListItemViewModel item in _feats)
                  {
                     item.IsSelected = false;
                  }
                  listItem.IsSelected = true;
               }

               _selectedFeat = new FeatViewModel(newFeat);

               SortFeats();

               _compendium.SaveFeats();

               OnPropertyChanged(nameof(SelectedFeat));
            }
         }
      }

      private void Delete()
      {
         if (_selectedFeat != null)
         {
            bool canDelete = true;

            foreach (CharacterModel character in _compendium.Characters)
            {
               foreach (LevelModel level in character.Levels)
               {
                  foreach (FeatModel feat in level.Feats)
                  {
                     if (feat.ID == _selectedFeat.FeatModel.ID)
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
                                                            _selectedFeat.Name);

               bool? result = _dialogService.ShowConfirmationDialog("Delete Feat", message, "Yes", "No", null);

               if (result == true)
               {
                  _compendium.DeleteFeat(_selectedFeat.FeatModel.ID);

                  FeatListItemViewModel listItem = _feats.FirstOrDefault(x => x.FeatModel.ID == _selectedFeat.FeatModel.ID);
                  if (listItem != null)
                  {
                     _feats.Remove(listItem);
                  }

                  _selectedFeat = null;

                  _compendium.SaveFeats();

                  OnPropertyChanged(nameof(SelectedFeat));

                  if (_editFeatXML != null)
                  {
                     CancelEditFeat();
                  }
               }
            }
            else
            {
               _dialogService.ShowConfirmationDialog("Unable Delete Feat", "Feat is in use by a character.", "OK", null, null);
            }
         }
      }

      private void SortFeats()
      {
         if (_feats != null && _feats.Count > 0)
         {
            List<FeatModel> feats = _featSearchService.Sort(_feats.Select(x => x.FeatModel), _featSearchInput.SortOption.Key);
            for (int i = 0; i < feats.Count; ++i)
            {
               if (feats[i].ID != _feats[i].FeatModel.ID)
               {
                  FeatListItemViewModel feat = _feats.FirstOrDefault(x => x.FeatModel.ID == feats[i].ID);
                  if (feat != null)
                  {
                     _feats.Move(_feats.IndexOf(feat), i);
                  }
               }
            }
         }
      }

      private void ExportFeat(FeatViewModel featViewModel)
      {
         Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
         saveFileDialog.Filter = "XML Document|*.xml";
         saveFileDialog.Title = "Save Feat";
         saveFileDialog.FileName = featViewModel.Name;

         if (saveFileDialog.ShowDialog() == true)
         {
            try
            {
               string ext = System.IO.Path.GetExtension(saveFileDialog.FileName);

               if (ext == ".xml")
               {
                  string xml = _xmlExporter.FormatXMLWithHeader(_xmlExporter.GetXML(featViewModel.FeatModel));
                  System.IO.File.WriteAllText(saveFileDialog.FileName, xml, System.Text.Encoding.UTF8);
               }
               else
               {
                  _dialogService.ShowConfirmationDialog("Unable To Export", "Invalid file extension.", "OK", null, null);
               }
            }
            catch (Exception)
            {
               _dialogService.ShowConfirmationDialog("Unable To Export", "An error occurred when attempting to export the feat.", "OK", null, null);
            }
         }
      }

      private void Import()
      {
         _dialogService.ShowImportView();
      }

      private void SelectNext()
      {
         if (_feats.Any())
         {
            FeatListItemViewModel selected = _feats.FirstOrDefault(x => x.IsSelected);

            foreach (FeatListItemViewModel feat in _feats)
            {
               feat.IsSelected = false;
            }

            if (selected == null)
            {
               _feats[0].IsSelected = true;
               _selectedFeat = new FeatViewModel(_feats[0].FeatModel);
            }
            else
            {
               int index = Math.Min(_feats.IndexOf(selected) + 1, _feats.Count - 1);
               _feats[index].IsSelected = true;
               _selectedFeat = new FeatViewModel(_feats[index].FeatModel);
            }

            OnPropertyChanged(nameof(SelectedFeat));
         }
      }

      private void SelectPrevious()
      {
         if (_feats.Any())
         {
            FeatListItemViewModel selected = _feats.FirstOrDefault(x => x.IsSelected);

            foreach (FeatListItemViewModel feat in _feats)
            {
               feat.IsSelected = false;
            }

            if (selected == null)
            {
               _feats[_feats.Count - 1].IsSelected = true;
               _selectedFeat = new FeatViewModel(_feats[_feats.Count - 1].FeatModel);
            }
            else
            {
               int index = Math.Max(_feats.IndexOf(selected) - 1, 0);
               _feats[index].IsSelected = true;
               _selectedFeat = new FeatViewModel(_feats[index].FeatModel);
            }

            OnPropertyChanged(nameof(SelectedFeat));
         }
      }

      #endregion
   }
}
