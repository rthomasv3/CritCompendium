using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using Microsoft.Win32;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class LocationEditViewModel : NotifyPropertyChanged
    {
        #region Fields

        private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();

        private readonly LocationModel _locationModel;

        private string _name;
        private string _tags;
        private string _description;
        private string _location;
        private string _map;

        private readonly Dictionary<LocationType, string> _locationTypeOptions = new Dictionary<LocationType, string>();
        private KeyValuePair<LocationType, string> _selectedLocationType;

        private string _creator;
        private ObservableCollection<RoomViewModel> _rooms = new ObservableCollection<RoomViewModel>();

        private string _rulerNotes;
        private string _traits;
        private string _knownFor;
        private string _conflicts;
        private ObservableCollection<BuildingViewModel> _buildings = new ObservableCollection<BuildingViewModel>();

        private string _landmarks;
        private string _environment;
        private string _weather;
        private string _foodAndWater;
        private string _hazards;

        public readonly ICommand _browseMapLocationCommand;
        public readonly ICommand _addRoomCommand;
        public readonly ICommand _deleteRoomCommand;
        public readonly ICommand _addBuildingCommand;
        public readonly ICommand _deleteBuildingCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="LocationEditViewModel"/>
        /// </summary>
        public LocationEditViewModel(LocationModel locationModel)
        {
            _locationModel = new LocationModel(locationModel);

            _name = _locationModel.Name;
            if (_locationModel.Tags.Any())
            {
                _tags = String.Join(", ", _locationModel.Tags);
            }
            _description = _locationModel.Description;
            _location = _locationModel.Location;
            _map = _locationModel.Map;

            foreach (LocationType locationType in Enum.GetValues(typeof(LocationType)))
            {
                _locationTypeOptions.Add(locationType, locationType.ToString());
            }
            
            _selectedLocationType = _locationTypeOptions.FirstOrDefault(x => x.Key == _locationModel.LocationType);

            _creator = _locationModel.Creator;
            foreach (RoomModel roomModel in _locationModel.Rooms)
            {
                RoomViewModel roomViewModel = new RoomViewModel(roomModel);
                roomViewModel.PropertyChanged += RoomViewModel_PropertyChanged;
                _rooms.Add(roomViewModel);
            }

            _rulerNotes = _locationModel.RulerNotes;
            _traits = _locationModel.Traits;
            _knownFor = _locationModel.KnownFor;
            _conflicts = _locationModel.Conflicts;
            foreach (BuildingModel buildingModel in _locationModel.Buildings)
            {
                BuildingViewModel buildingViewModel = new BuildingViewModel(buildingModel);
                buildingViewModel.PropertyChanged += BuildingViewModel_PropertyChanged;
                _buildings.Add(buildingViewModel);
            }

            _landmarks = _locationModel.Landmarks;
            _environment = _locationModel.Environment;
            _weather = _locationModel.Weather;
            _foodAndWater = _locationModel.FoodAndWater;
            _hazards = _locationModel.Hazards;

            _browseMapLocationCommand = new RelayCommand(obj => true, obj => BrowseMapLocation());
            _addRoomCommand = new RelayCommand(obj => true, obj => AddRoom());
            _deleteRoomCommand = new RelayCommand(obj => true, obj => DeleteRoom(obj as RoomViewModel));
            _addBuildingCommand = new RelayCommand(obj => true, obj => AddBuilding());
            _deleteBuildingCommand = new RelayCommand(obj => true, obj => DeleteBuilding(obj as BuildingViewModel));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets location model
        /// </summary>
        public LocationModel LocationModel
        {
            get { return _locationModel; }
        }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (Set(ref _name, value))
                {
                    _locationModel.Name = _name;
                }
            }
        }

        /// <summary>
        /// Gets or sets tags
        /// </summary>
        public string Tags
        {
            get { return _tags; }
            set
            {
                if (Set(ref _tags, value))
                {
                    if (!String.IsNullOrWhiteSpace(_tags))
                    {
                        _locationModel.Tags.Clear();
                        foreach (string tag in _tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            _locationModel.Tags.Add(tag.Trim());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets decsription
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                if (Set(ref _description, value))
                {
                    _locationModel.Description = _description;
                }
            }
        }

        /// <summary>
        /// Gets or sets location
        /// </summary>
        public string Location
        {
            get { return _location; }
            set
            {
                if (Set(ref _location, value))
                {
                    _locationModel.Location = _location;
                }
            }
        }

        /// <summary>
        /// Gets or sets map
        /// </summary>
        public string Map
        {
            get { return _map; }
            set
            {
                if (Set(ref _map, value))
                {
                    _locationModel.Map = _map;
                }
            }
        }

        /// <summary>
        /// Gets location type options
        /// </summary>
        public Dictionary<LocationType, string> LocationTypeOptions
        {
            get { return _locationTypeOptions; }
        }

        /// <summary>
        /// Gets or sets the selected location type
        /// </summary>
        public KeyValuePair<LocationType, string> SelectedLocationType
        {
            get { return _selectedLocationType; }
            set
            {
                if (Set(ref _selectedLocationType, value))
                {
                    _locationModel.LocationType = _selectedLocationType.Key;

                    OnPropertyChanged(nameof(LocationTypeIsDungeon));
                    OnPropertyChanged(nameof(LocationTypeIsSettlement));
                    OnPropertyChanged(nameof(LocationTypeIsWilderness));
                }
            }
        }

        /// <summary>
        /// Returns true if the selected location type is a dungeon.
        /// </summary>
        public bool LocationTypeIsDungeon
        {
            get { return _selectedLocationType.Key == LocationType.Dungeon; }
        }

        /// <summary>
        /// Returns true if the selected location type is a settlement.
        /// </summary>
        public bool LocationTypeIsSettlement
        {
            get { return _selectedLocationType.Key == LocationType.Settlement; }
        }

        /// <summary>
        /// Returns true if the selected location type is a wilderness.
        /// </summary>
        public bool LocationTypeIsWilderness
        {
            get { return _selectedLocationType.Key == LocationType.Wilderness; }
        }

        /// <summary>
        /// Gets or sets creator
        /// </summary>
        public string Creator
        {
            get { return _creator; }
            set
            {
                if (Set(ref _creator, value))
                {
                    _locationModel.Creator = _creator;
                }
            }
        }

        /// <summary>
        /// Gets rooms
        /// </summary>
        public IEnumerable<RoomViewModel> Rooms
        {
            get { return _rooms; }
        }

        /// <summary>
        /// Gets or sets ruler notes
        /// </summary>
        public string RulerNotes
        {
            get { return _rulerNotes; }
            set
            {
                if (Set(ref _rulerNotes, value))
                {
                    _locationModel.RulerNotes = _rulerNotes;
                }
            }
        }

        /// <summary>
        /// Gets or sets traits
        /// </summary>
        public string Traits
        {
            get { return _traits; }
            set
            {
                if (Set(ref _traits, value))
                {
                    _locationModel.Traits = _traits;
                }
            }
        }

        /// <summary>
        /// Gets or sets know for
        /// </summary>
        public string KnownFor
        {
            get { return _knownFor; }
            set
            {
                if (Set(ref _knownFor, value))
                {
                    _locationModel.KnownFor = _knownFor;
                }
            }
        }

        /// <summary>
        /// Gets or sets conflicts
        /// </summary>
        public string Conflicts
        {
            get { return _conflicts; }
            set
            {
                if (Set(ref _conflicts, value))
                {
                    _locationModel.Conflicts = _conflicts;
                }
            }
        }

        /// <summary>
        /// Gets buildings
        /// </summary>
        public IEnumerable<BuildingViewModel> Buildings
        {
            get { return _buildings; }
        }

        /// <summary>
        /// Gets or sets landmarks
        /// </summary>
        public string Landmarks
        {
            get { return _landmarks; }
            set
            {
                if (Set(ref _landmarks, value))
                {
                    _locationModel.Landmarks = _landmarks;
                }
            }
        }

        /// <summary>
        /// Gets or sets environment
        /// </summary>
        public string Environment
        {
            get { return _environment; }
            set
            {
                if (Set(ref _environment, value))
                {
                    _locationModel.Environment = _environment;
                }
            }
        }

        /// <summary>
        /// Gets or sets weather
        /// </summary>
        public string Weather
        {
            get { return _weather; }
            set
            {
                if (Set(ref _weather, value))
                {
                    _locationModel.Weather = _weather;
                }
            }
        }

        /// <summary>
        /// Gets or sets food and water
        /// </summary>
        public string FoodAndWater
        {
            get { return _foodAndWater; }
            set
            {
                if (Set(ref _foodAndWater, value))
                {
                    _locationModel.FoodAndWater = _foodAndWater;
                }
            }
        }

        /// <summary>
        /// Gets or sets hazards
        /// </summary>
        public string Hazards
        {
            get { return _hazards; }
            set
            {
                if (Set(ref _hazards, value))
                {
                    _locationModel.Hazards = _hazards;
                }
            }
        }

        /// <summary>
        /// Gets browse map location
        /// </summary>
        public ICommand BrowseMapLocationCommand
        {
            get { return _browseMapLocationCommand; }
        }

        /// <summary>
        /// Gets add room command
        /// </summary>
        public ICommand AddRoomCommand
        {
            get { return _addRoomCommand; }
        }

        /// <summary>
        /// Gets delete room command
        /// </summary>
        public ICommand DeleteRoomCommand
        {
            get { return _deleteRoomCommand; }
        }

        /// <summary>
        /// Gets add building command
        /// </summary>
        public ICommand AddBuildingCommand
        {
            get { return _addBuildingCommand; }
        }

        /// <summary>
        /// Gets delete building command
        /// </summary>
        public ICommand DeleteBuildingCommand
        {
            get { return _deleteBuildingCommand; }
        }

        #endregion

        #region Private Methods

        private void BrowseMapLocation()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                _map = Path.GetFileName(openFileDialog.FileName);
                _locationModel.Map = _map;

                OnPropertyChanged(nameof(Map));
            }
        }

        private void AddRoom()
        {
            RoomModel roomModel = new RoomModel();
            RoomViewModel roomViewModel = new RoomViewModel(roomModel);

            roomViewModel.PropertyChanged += RoomViewModel_PropertyChanged;

            _locationModel.Rooms.Add(roomModel);
            _rooms.Add(roomViewModel);

            OnPropertyChanged(nameof(Rooms));
        }

        private void DeleteRoom(RoomViewModel roomViewModel)
        {
            if (roomViewModel != null)
            {
                string name = !String.IsNullOrWhiteSpace(roomViewModel.Name) ? roomViewModel.Name : "Unknown Name";
                if (_dialogService.ShowConfirmationDialog("Delete Room", $"Are you sure you want to delete {name}?", "Yes", "No", null) == true)
                {
                    roomViewModel.PropertyChanged -= RoomViewModel_PropertyChanged;

                    _locationModel.Rooms.Remove(roomViewModel.RoomModel);
                    _rooms.Remove(roomViewModel);

                    OnPropertyChanged(nameof(Rooms));
                }
            }
        }

        private void AddBuilding()
        {
            BuildingModel buildingModel = new BuildingModel();
            BuildingViewModel buildingViewModel = new BuildingViewModel(buildingModel);

            buildingViewModel.PropertyChanged += BuildingViewModel_PropertyChanged;

            _locationModel.Buildings.Add(buildingModel);
            _buildings.Add(buildingViewModel);

            OnPropertyChanged(nameof(Buildings));
        }

        private void DeleteBuilding(BuildingViewModel buildingViewModel)
        {
            if (buildingViewModel != null)
            {
                string name = !String.IsNullOrWhiteSpace(buildingViewModel.Name) ? buildingViewModel.Name : "Unknown Name";
                if (_dialogService.ShowConfirmationDialog("Delete Building", $"Are you sure you want to delete {name}?", "Yes", "No", null) == true)
                {
                    buildingViewModel.PropertyChanged -= BuildingViewModel_PropertyChanged;

                    _locationModel.Buildings.Remove(buildingViewModel.BuildingModel);
                    _buildings.Remove(buildingViewModel);

                    OnPropertyChanged(nameof(Buildings));
                }
            }
        }

        private void RoomViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Rooms));
        }

        private void BuildingViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Buildings));
        }

        #endregion
    }
}
