using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class LocationViewModel : NotifyPropertyChanged
    {
        #region Fields

        private readonly LocationModel _locationModel;
        private ObservableCollection<RoomViewModel> _rooms = new ObservableCollection<RoomViewModel>();
        private ObservableCollection<BuildingViewModel> _buildings = new ObservableCollection<BuildingViewModel>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="LocationViewModel"/>
        /// </summary>
        public LocationViewModel(LocationModel locationModel)
        {
            _locationModel = locationModel;

            foreach (RoomModel roomModel in _locationModel.Rooms)
            {
                _rooms.Add(new RoomViewModel(roomModel));
            }

            foreach (BuildingModel buildingModel in _locationModel.Buildings)
            {
                _buildings.Add(new BuildingViewModel(buildingModel));
            }
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
        /// Gets name
        /// </summary>
        public string Name
        {
            get { return !String.IsNullOrWhiteSpace(_locationModel.Name) ? _locationModel.Name : "Unknown Name"; }
        }

        /// <summary>
        /// Gets location type
        /// </summary>
        public string LocationType
        {
            get { return _locationModel.LocationType.ToString(); }
        }

        /// <summary>
        /// Gets map
        /// </summary>
        public string Map
        {
            get { return _locationModel.Map; }
        }

        /// <summary>
        /// Gets description
        /// </summary>
        public string Description
        {
            get { return _locationModel.Description; }
        }

        /// <summary>
        /// Returns true if the selected location type is a dungeon.
        /// </summary>
        public bool LocationTypeIsDungeon
        {
            get { return _locationModel.LocationType == CritCompendiumInfrastructure.Enums.LocationType.Dungeon; }
        }

        /// <summary>
        /// Returns true if the selected location type is a settlement.
        /// </summary>
        public bool LocationTypeIsSettlement
        {
            get { return _locationModel.LocationType == CritCompendiumInfrastructure.Enums.LocationType.Settlement; }
        }

        /// <summary>
        /// Returns true if the selected location type is a wilderness.
        /// </summary>
        public bool LocationTypeIsWilderness
        {
            get { return _locationModel.LocationType == CritCompendiumInfrastructure.Enums.LocationType.Wilderness; }
        }

        /// <summary>
        /// Gets creator
        /// </summary>
        public string Creator
        {
            get { return !String.IsNullOrWhiteSpace(_locationModel.Creator) ? _locationModel.Creator : "Unknown Creator"; }
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
            get { return !String.IsNullOrWhiteSpace(_locationModel.RulerNotes) ? _locationModel.RulerNotes : "Unknown Ruler"; }
        }

        /// <summary>
        /// Gets or sets traits
        /// </summary>
        public string Traits
        {
            get { return !String.IsNullOrWhiteSpace(_locationModel.Traits) ? _locationModel.Traits : "Unknown Traits"; }
        }

        /// <summary>
        /// Gets or sets know for
        /// </summary>
        public string KnownFor
        {
            get { return !String.IsNullOrWhiteSpace(_locationModel.KnownFor) ? _locationModel.KnownFor : "Unknown"; }
        }

        /// <summary>
        /// Gets or sets conflicts
        /// </summary>
        public string Conflicts
        {
            get { return !String.IsNullOrWhiteSpace(_locationModel.Conflicts) ? _locationModel.Conflicts : "Unknown Conflicts"; }
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
            get { return !String.IsNullOrWhiteSpace(_locationModel.Landmarks) ? _locationModel.Landmarks : "Unknown Landmarks"; }
        }

        /// <summary>
        /// Gets or sets environment
        /// </summary>
        public string Environment
        {
            get { return !String.IsNullOrWhiteSpace(_locationModel.Environment) ? _locationModel.Environment : "Unknown Environment"; }
        }

        /// <summary>
        /// Gets or sets weather
        /// </summary>
        public string Weather
        {
            get { return !String.IsNullOrWhiteSpace(_locationModel.Weather) ? _locationModel.Weather : "Unknown Weather"; }
        }

        /// <summary>
        /// Gets or sets food and water
        /// </summary>
        public string FoodAndWater
        {
            get { return !String.IsNullOrWhiteSpace(_locationModel.FoodAndWater) ? _locationModel.FoodAndWater : "Unknown"; }
        }

        /// <summary>
        /// Gets hazards
        /// </summary>
        public string Hazards
        {
            get { return !String.IsNullOrWhiteSpace(_locationModel.Hazards) ? _locationModel.Hazards : "Unknown"; }
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion
    }
}
