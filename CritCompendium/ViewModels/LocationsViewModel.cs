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
   public sealed class LocationsViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly Compendium _compendium;
      private readonly LocationSearchService _locationSearchService;
      private readonly LocationSearchInput _locationSearchInput;
      private readonly StringService _stringService;
      private readonly DialogService _dialogService;
      private readonly XMLImporter _xmlImporter;
      private readonly DocumentService _documentService;
      private readonly DataManager _dataManager;
      private readonly ObservableCollection<ListItemViewModel<LocationModel>> _locations = new ObservableCollection<ListItemViewModel<LocationModel>>();
      private readonly ICommand _selectLocationCommand;
      private readonly ICommand _editLocationCommand;
      private readonly ICommand _exportLocationCommand;
      private readonly ICommand _cancelEditLocationCommand;
      private readonly ICommand _saveEditLocationCommand;
      private readonly ICommand _resetFiltersCommand;
      private readonly ICommand _addCommand;
      private readonly ICommand _copyCommand;
      private readonly ICommand _deleteCommand;
      private readonly ICommand _importCommand;
      private readonly ICommand _selectNextCommand;
      private readonly ICommand _selectPreviousCommand;
      private LocationViewModel _selectedLocation;
      private LocationEditViewModel _locationEditViewModel;
      private bool _editHasUnsavedChanges;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates an instance of <see cref="LocationsViewModel"/>
      /// </summary>
      public LocationsViewModel(Compendium compendium, LocationSearchService locationSearchService, LocationSearchInput locationSearchInput,
          StringService stringService, DialogService dialogService, XMLImporter xmlImporter, DocumentService documentService, DataManager dataManager)
      {
         _compendium = compendium;
         _locationSearchService = locationSearchService;
         _locationSearchInput = locationSearchInput;
         _stringService = stringService;
         _dialogService = dialogService;
         _xmlImporter = xmlImporter;
         _documentService = documentService;
         _dataManager = dataManager;

         _selectLocationCommand = new RelayCommand(obj => true, obj => SelectLocation(obj as ListItemViewModel<LocationModel>));
         _editLocationCommand = new RelayCommand(obj => true, obj => EditLocation(obj as LocationViewModel));
         _exportLocationCommand = new RelayCommand(obj => true, obj => ExportLocation(obj as LocationViewModel));
         _cancelEditLocationCommand = new RelayCommand(obj => true, obj => CancelEditLocation());
         _saveEditLocationCommand = new RelayCommand(obj => HasUnsavedChanges, obj => SaveEditLocation());
         _resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
         _addCommand = new RelayCommand(obj => true, obj => Add());
         _copyCommand = new RelayCommand(obj => _selectedLocation != null, obj => Copy());
         _deleteCommand = new RelayCommand(obj => _selectedLocation != null, obj => Delete());
         _importCommand = new RelayCommand(obj => true, obj => Import());
         _selectNextCommand = new RelayCommand(obj => true, obj => SelectNext());
         _selectPreviousCommand = new RelayCommand(obj => true, obj => SelectPrevious());

         Search();
      }

      #endregion

      #region Properties

      /// <summary>
      /// List of locations
      /// </summary>
      public ObservableCollection<ListItemViewModel<LocationModel>> Locations
      {
         get { return _locations; }
      }

      /// <summary>
      /// Gets or sets the search text
      /// </summary>
      public string SearchText
      {
         get { return _locationSearchInput.SearchText; }
         set
         {
            _locationSearchInput.SearchText = value;
            Search();
         }
      }

      /// <summary>
      /// Command to select a location
      /// </summary>
      public ICommand SelectLocationCommand
      {
         get { return _selectLocationCommand; }
      }

      /// <summary>
      /// Selected location
      /// </summary>
      public LocationViewModel SelectedLocation
      {
         get { return _selectedLocation; }
      }

      /// <summary>
      /// Editing location
      /// </summary>
      public LocationEditViewModel EditingLocation
      {
         get { return _locationEditViewModel; }
      }

      /// <summary>
      /// Command to reset filters
      /// </summary>
      public ICommand ResetFiltersCommand
      {
         get { return _resetFiltersCommand; }
      }

      /// <summary>
      /// Command to edit location
      /// </summary>
      public ICommand EditLocationCommand
      {
         get { return _editLocationCommand; }
      }

      /// <summary>
      /// Command to export location
      /// </summary>
      public ICommand ExportLocationCommand
      {
         get { return _exportLocationCommand; }
      }

      /// <summary>
      /// Command to cancel edit location
      /// </summary>
      public ICommand CancelEditLocationCommand
      {
         get { return _cancelEditLocationCommand; }
      }

      /// <summary>
      /// Command to save edit location
      /// </summary>
      public ICommand SaveEditLocationCommand
      {
         get { return _saveEditLocationCommand; }
      }

      /// <summary>
      /// Command to add location
      /// </summary>
      public ICommand AddLocationCommand
      {
         get { return _addCommand; }
      }

      /// <summary>
      /// Command to copy location
      /// </summary>
      public ICommand CopyLocationCommand
      {
         get { return _copyCommand; }
      }

      /// <summary>
      /// Command to delete selected location
      /// </summary>
      public ICommand DeleteLocationCommand
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
      /// True if currently editing a location
      /// </summary>
      public bool IsEditingLocation
      {
         get { return _locationEditViewModel != null; }
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
         _locations.Clear();
         foreach (LocationModel locationModel in _locationSearchService.Search(_locationSearchInput))
         {
            ListItemViewModel<LocationModel> listItem = new ListItemViewModel<LocationModel>(locationModel);
            InitializeListItemDetails(listItem, locationModel);
            _locations.Add(listItem);
         }

         if (_selectedLocation != null)
         {
            ListItemViewModel<LocationModel> location = _locations.FirstOrDefault(x => x.Model.Id == _selectedLocation.LocationModel.Id);
            if (location != null)
            {
               location.IsSelected = true;
            }
         }
      }

      #endregion

      #region Private Methods

      private void InitializeSearch()
      {
         _locationSearchInput.Reset();

         OnPropertyChanged(nameof(SearchText));

         Search();
      }

      private void SelectLocation(ListItemViewModel<LocationModel> locationItem)
      {
         bool selectLocation = true;

         if (_locationEditViewModel != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                            _selectedLocation.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditLocation())
                  {
                     selectLocation = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditLocation();
               }
               else
               {
                  selectLocation = false;
               }
            }
            else
            {
               CancelEditLocation();
            }
         }

         if (selectLocation)
         {
            foreach (ListItemViewModel<LocationModel> item in _locations)
            {
               item.IsSelected = false;
            }
            locationItem.IsSelected = true;

            _selectedLocation = new LocationViewModel(locationItem.Model);
            OnPropertyChanged(nameof(SelectedLocation));
         }
      }

      private void EditLocation(LocationViewModel locationModel)
      {
         if (locationModel != null)
         {
            _locationEditViewModel = new LocationEditViewModel(locationModel.LocationModel);
            _locationEditViewModel.PropertyChanged += _locationEditViewModel_PropertyChanged;

            OnPropertyChanged(nameof(EditingLocation));
            OnPropertyChanged(nameof(IsEditingLocation));
         }
      }

      private void CancelEditLocation()
      {
         _editHasUnsavedChanges = false;
         _locationEditViewModel = null;

         OnPropertyChanged(nameof(EditingLocation));
         OnPropertyChanged(nameof(IsEditingLocation));
         OnPropertyChanged(nameof(HasUnsavedChanges));
      }

      private bool SaveEditLocation()
      {
         bool saved = false;

         if (_locationEditViewModel.LocationModel != null)
         {
            _locationEditViewModel.LocationModel.Id = _selectedLocation.LocationModel.Id;
            _compendium.UpdateLocation(_locationEditViewModel.LocationModel);

            _selectedLocation = new LocationViewModel(_locationEditViewModel.LocationModel);

            ListItemViewModel<LocationModel> oldListItem = _locations.FirstOrDefault(x => x.Model.Id == _locationEditViewModel.LocationModel.Id);
            if (oldListItem != null)
            {
               if (_locationSearchService.SearchInputApplies(_locationSearchInput, _locationEditViewModel.LocationModel))
               {
                  InitializeListItemDetails(oldListItem, _locationEditViewModel.LocationModel);
               }
               else
               {
                  _locations.Remove(oldListItem);
               }
            }

            _locationEditViewModel = null;
            _editHasUnsavedChanges = false;

            SortLocations();

            _compendium.SaveLocations();

            OnPropertyChanged(nameof(SelectedLocation));
            OnPropertyChanged(nameof(EditingLocation));
            OnPropertyChanged(nameof(IsEditingLocation));
            OnPropertyChanged(nameof(HasUnsavedChanges));

            saved = true;
         }

         return saved;
      }

      private void Add()
      {
         bool addLocation = true;

         if (_locationEditViewModel != null)
         {
            if (_editHasUnsavedChanges)
            {
               string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                                    _selectedLocation.Name, Environment.NewLine + Environment.NewLine);
               string accept = "Save and Continue";
               string reject = "Discard Changes";
               string cancel = "Cancel Navigation";
               bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

               if (result == true)
               {
                  if (!SaveEditLocation())
                  {
                     addLocation = false;
                  }
               }
               else if (result == false)
               {
                  CancelEditLocation();
               }
               else
               {
                  addLocation = false;
               }
            }
            else
            {
               CancelEditLocation();
            }
         }

         if (addLocation)
         {
            LocationModel locationModel = new LocationModel();

            _compendium.AddLocation(locationModel);

            if (_locationSearchService.SearchInputApplies(_locationSearchInput, locationModel))
            {
               ListItemViewModel<LocationModel> listItem = new ListItemViewModel<LocationModel>(locationModel);
               InitializeListItemDetails(listItem, locationModel);
               _locations.Add(listItem);
               foreach (ListItemViewModel<LocationModel> item in _locations)
               {
                  item.IsSelected = false;
               }
               listItem.IsSelected = true;
            }

            _selectedLocation = new LocationViewModel(locationModel);

            _locationEditViewModel = new LocationEditViewModel(locationModel);
            _locationEditViewModel.PropertyChanged += _locationEditViewModel_PropertyChanged;

            SortLocations();

            _compendium.SaveLocations();

            OnPropertyChanged(nameof(EditingLocation));
            OnPropertyChanged(nameof(IsEditingLocation));
            OnPropertyChanged(nameof(SelectedLocation));
         }
      }

      private void Copy()
      {
         if (_selectedLocation != null)
         {
            bool copyLocation = true;

            if (_locationEditViewModel != null)
            {
               if (_editHasUnsavedChanges)
               {
                  string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
                                               _selectedLocation.Name, Environment.NewLine + Environment.NewLine);
                  string accept = "Save and Continue";
                  string reject = "Discard Changes";
                  string cancel = "Cancel Navigation";
                  bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                  if (result == true)
                  {
                     if (!SaveEditLocation())
                     {
                        copyLocation = false;
                     }
                  }
                  else if (result == false)
                  {
                     CancelEditLocation();
                  }
                  else
                  {
                     copyLocation = false;
                  }
               }
               else
               {
                  CancelEditLocation();
               }
            }

            if (copyLocation)
            {
               LocationModel locationModel = new LocationModel(_selectedLocation.LocationModel);
               locationModel.Name += " (copy)";
               locationModel.Id = Guid.NewGuid();

               _compendium.AddLocation(locationModel);

               if (_locationSearchService.SearchInputApplies(_locationSearchInput, locationModel))
               {
                  ListItemViewModel<LocationModel> listItem = new ListItemViewModel<LocationModel>(locationModel);
                  InitializeListItemDetails(listItem, locationModel);
                  _locations.Add(listItem);
                  foreach (ListItemViewModel<LocationModel> item in _locations)
                  {
                     item.IsSelected = false;
                  }
                  listItem.IsSelected = true;
               }

               _selectedLocation = new LocationViewModel(locationModel);

               SortLocations();

               _compendium.SaveLocations();

               OnPropertyChanged(nameof(SelectedLocation));
            }
         }
      }

      private void Delete()
      {
         if (_selectedLocation != null)
         {
            string message = String.Format("Are you sure you want to delete {0}?", _selectedLocation.Name);

            bool? result = _dialogService.ShowConfirmationDialog("Delete Location", message, "Yes", "No", null);

            if (result == true)
            {
               _compendium.DeleteLocation(_selectedLocation.LocationModel.Id);

               ListItemViewModel<LocationModel> listItem = _locations.FirstOrDefault(x => x.Model.Id == _selectedLocation.LocationModel.Id);
               if (listItem != null)
               {
                  _locations.Remove(listItem);
               }

               _selectedLocation = null;

               _compendium.SaveLocations();

               OnPropertyChanged(nameof(SelectedLocation));

               if (_locationEditViewModel != null)
               {
                  CancelEditLocation();
               }
            }
         }
      }

      private void SortLocations()
      {
         if (_locations != null && _locations.Count > 0)
         {
            List<LocationModel> locations = _locationSearchService.Sort(_locations.Select(x => x.Model), _locationSearchInput.SortOption.Key);
            for (int i = 0; i < locations.Count; ++i)
            {
               if (locations[i].Id != _locations[i].Model.Id)
               {
                  ListItemViewModel<LocationModel> location = _locations.FirstOrDefault(x => x.Model.Id == locations[i].Id);
                  if (location != null)
                  {
                     _locations.Move(_locations.IndexOf(location), i);
                  }
               }
            }

            List<ListItemViewModel<LocationModel>> sorted = _locations.OrderBy(x => x.Name).ToList();

            for (int i = 0; i < sorted.Count; ++i)
            {
               if (!sorted[i].Equals(_locations[i]))
               {
                  _locations.Move(_locations.IndexOf(sorted[i]), i);
               }
            }
         }
      }

      private void _locationEditViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (!_editHasUnsavedChanges)
         {
            _editHasUnsavedChanges = true;
         }
      }

      private void ExportLocation(LocationViewModel locationViewModel)
      {
         Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
         saveFileDialog.Filter = "Location Archive|*.ccea|Word Document|*.docx";
         saveFileDialog.Title = "Save Location";
         saveFileDialog.FileName = locationViewModel.Name;

         if (saveFileDialog.ShowDialog() == true)
         {
            try
            {
               string ext = Path.GetExtension(saveFileDialog.FileName);

               if (ext == ".ccaa")
               {
                  byte[] bytes = _dataManager.CreateLocationArchive(locationViewModel.LocationModel);
                  File.WriteAllBytes(saveFileDialog.FileName, bytes);
               }
               else if (ext == "*.docx")
               {
                  //_documentService.CreateWordDoc(saveFileDialog.FileName, locationViewModel);
               }
               else
               {
                  _dialogService.ShowConfirmationDialog("Unable To Export", "Invalid file extension.", "OK", null, null);
               }
            }
            catch (Exception)
            {
               _dialogService.ShowConfirmationDialog("Unable To Export", "An error occurred when attempting to export the location.", "OK", null, null);
            }
         }
      }

      private void InitializeListItemDetails(ListItemViewModel<LocationModel> listItem, LocationModel locationModel)
      {
         listItem.Model = locationModel;

         if (!String.IsNullOrWhiteSpace(locationModel.Name))
         {
            listItem.Name = locationModel.Name;
         }
         else
         {
            listItem.Name = "Unknown Name";
         }

         if (!String.IsNullOrWhiteSpace(locationModel.Description))
         {
            if (locationModel.Description.Length > 50)
            {
               listItem.Description = new String(locationModel.Description.Take(50).ToArray()) + "...";
            }
            else
            {
               listItem.Description = locationModel.Description;
            }
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
         if (_locations.Any())
         {
            ListItemViewModel<LocationModel> selected = _locations.FirstOrDefault(x => x.IsSelected);

            foreach (ListItemViewModel<LocationModel> location in _locations)
            {
               location.IsSelected = false;
            }

            if (selected == null)
            {
               _locations[0].IsSelected = true;
               _selectedLocation = new LocationViewModel(_locations[0].Model);
            }
            else
            {
               int index = Math.Min(_locations.IndexOf(selected) + 1, _locations.Count - 1);
               _locations[index].IsSelected = true;
               _selectedLocation = new LocationViewModel(_locations[index].Model);
            }

            OnPropertyChanged(nameof(SelectedLocation));
         }
      }

      private void SelectPrevious()
      {
         if (_locations.Any())
         {
            ListItemViewModel<LocationModel> selected = _locations.FirstOrDefault(x => x.IsSelected);

            foreach (ListItemViewModel<LocationModel> location in _locations)
            {
               location.IsSelected = false;
            }

            if (selected == null)
            {
               _locations[_locations.Count - 1].IsSelected = true;
               _selectedLocation = new LocationViewModel(_locations[_locations.Count - 1].Model);
            }
            else
            {
               int index = Math.Max(_locations.IndexOf(selected) - 1, 0);
               _locations[index].IsSelected = true;
               _selectedLocation = new LocationViewModel(_locations[index].Model);
            }

            OnPropertyChanged(nameof(SelectedLocation));
         }
      }

      #endregion
   }
}
