using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Models
{
   public class LocationModel
   {
      #region Fields

      private Guid _id;
      private string _name;
      private List<string> _tags = new List<string>();
      private string _description;
      private string _location;
      private string _map;

      private LocationType _locationType = LocationType.Dungeon;

      private string _creator;
      private List<RoomModel> _rooms = new List<RoomModel>();

      private string _rulerNotes;
      private string _traits;
      private string _knownFor;
      private string _conflicts;
      private List<BuildingModel> _buildings = new List<BuildingModel>();

      private string _landmarks;
      private string _environment;
      private string _weather;
      private string _foodAndWater;
      private string _hazards;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of <see cref="LocationModel"/>
      /// </summary>
      public LocationModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates a copy of <see cref="LocationModel"/>
      /// </summary>
      public LocationModel(LocationModel locationModel)
      {
         _id = locationModel.ID;
         _name = locationModel.Name;
         _tags = new List<string>(locationModel.Tags);
         _description = locationModel.Description;
         _location = locationModel.Location;
         _map = locationModel.Map;

         _locationType = locationModel.LocationType;

         _creator = locationModel.Creator;

         _rooms = new List<RoomModel>();
         foreach (RoomModel roomModel in locationModel.Rooms)
         {
            _rooms.Add(new RoomModel(roomModel));
         }

         _rulerNotes = locationModel.RulerNotes;
         _traits = locationModel.Traits;
         _knownFor = locationModel.KnownFor;
         _conflicts = locationModel.Conflicts;

         _buildings = new List<BuildingModel>();
         foreach (BuildingModel buildingModel in locationModel.Buildings)
         {
            _buildings.Add(new BuildingModel(buildingModel));
         }

         _landmarks = locationModel.Landmarks;
         _environment = locationModel.Environment;
         _weather = locationModel.Weather;
         _foodAndWater = locationModel.FoodAndWater;
         _hazards = locationModel.Hazards;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets id
      /// </summary>
      public Guid ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Gets or sets name
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Gets or sets tags
      /// </summary>
      public List<string> Tags
      {
         get { return _tags; }
         set { _tags = value; }
      }

      /// <summary>
      /// Gets or sets description
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      /// <summary>
      /// Gets or sets location
      /// </summary>
      public string Location
      {
         get { return _location; }
         set { _location = value; }
      }

      /// <summary>
      /// Gets or sets map
      /// </summary>
      public string Map
      {
         get { return _map; }
         set { _map = value; }
      }

      /// <summary>
      /// Gets or sets the location type
      /// </summary>
      public LocationType LocationType
      {
         get { return _locationType; }
         set { _locationType = value; }
      }

      /// <summary>
      /// Gets or sets creator
      /// </summary>
      public string Creator
      {
         get { return _creator; }
         set { _creator = value; }
      }

      /// <summary>
      /// Gets or set rooms
      /// </summary>
      public List<RoomModel> Rooms
      {
         get { return _rooms; }
         set { _rooms = value; }
      }

      /// <summary>
      /// Gets or sets ruler notes
      /// </summary>
      public string RulerNotes
      {
         get { return _rulerNotes; }
         set { _rulerNotes = value; }
      }

      /// <summary>
      /// Gets or sets traits
      /// </summary>
      public string Traits
      {
         get { return _traits; }
         set { _traits = value; }
      }

      /// <summary>
      /// Gets or sets known for
      /// </summary>
      public string KnownFor
      {
         get { return _knownFor; }
         set { _knownFor = value; }
      }

      /// <summary>
      /// Gets or sets conflicts
      /// </summary>
      public string Conflicts
      {
         get { return _conflicts; }
         set { _conflicts = value; }
      }

      /// <summary>
      /// Gets or sets buildings
      /// </summary>
      public List<BuildingModel> Buildings
      {
         get { return _buildings; }
         set { _buildings = value; }
      }

      /// <summary>
      /// Gets or sets landmarks
      /// </summary>
      public string Landmarks
      {
         get { return _landmarks; }
         set { _landmarks = value; }
      }

      /// <summary>
      /// Gets or sets environment
      /// </summary>
      public string Environment
      {
         get { return _environment; }
         set { _environment = value; }
      }

      /// <summary>
      /// Gets or sets weather
      /// </summary>
      public string Weather
      {
         get { return _weather; }
         set { _weather = value; }
      }

      /// <summary>
      /// Gets or sets food and water
      /// </summary>
      public string FoodAndWater
      {
         get { return _foodAndWater; }
         set { _foodAndWater = value; }
      }

      /// <summary>
      /// Gets or sets hazards
      /// </summary>
      public string Hazards
      {
         get { return _hazards; }
         set { _hazards = value; }
      }

      #endregion
   }
}
