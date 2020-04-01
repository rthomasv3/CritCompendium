using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Models
{
    public sealed class BuildingModel
    {
        #region Fields

        private Guid _id;
        private string _name;
        private string _description;
        private string _map;
        private BuildingType _buildingType;
        private string _customBuildingType;
        private List<RoomModel> _rooms = new List<RoomModel>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="BuildingModel"/>
        /// </summary>
        public BuildingModel()
        {
            _id = Guid.NewGuid();
        }

        /// <summary>
		/// Creates a copy of <see cref="BuildingModel"/>
		/// </summary>
        public BuildingModel(BuildingModel buildingModel)
        {
            _id = buildingModel.ID;
            _name = buildingModel.Name;
            _description = buildingModel.Description;
            _map = buildingModel.Map;
            _buildingType = buildingModel.BuildingType;
            _customBuildingType = buildingModel.CustomBuildingType;

            _rooms = new List<RoomModel>();
            foreach (RoomModel roomModel in buildingModel.Rooms)
            {
                _rooms.Add(new RoomModel(roomModel));
            }
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
        /// Gets or sets description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
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
        /// Gets or sets building type
        /// </summary>
        public BuildingType BuildingType
        {
            get { return _buildingType; }
            set { _buildingType = value; }
        }

        /// <summary>
        /// Gets or sets custom building type
        /// </summary>
        public string CustomBuildingType
        {
            get { return _customBuildingType; }
            set { _customBuildingType = value; }
        }

        /// <summary>
        /// Gets or sets rooms
        /// </summary>
        public List<RoomModel> Rooms
        {
            get { return _rooms; }
            set { _rooms = value; }
        }

        #endregion
    }
}
