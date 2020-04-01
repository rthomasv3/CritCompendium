using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Models
{
    public sealed class DungeonModel : LocationModel
    {
        #region Fields
        
        private string _creator;
        private List<RoomModel> _rooms = new List<RoomModel>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="DungeonModel"/>
        /// </summary>
        public DungeonModel() : base()
        {
        }

        /// <summary>
        /// Creates a copy of <see cref="DungeonModel"/>
        /// </summary>
        public DungeonModel(DungeonModel dungeonModel) : base(dungeonModel)
        {
            _creator = dungeonModel.Creator;

            _rooms = new List<RoomModel>();
            foreach (RoomModel roomModel in dungeonModel.Rooms)
            {
                _rooms.Add(new RoomModel(roomModel));
            }
        }

        #endregion

        #region Properties

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

        #endregion
    }
}
