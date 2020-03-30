using System;
using System.Collections.Generic;

namespace CriticalCompendiumInfrastructure.Models
{
    public sealed class EventModel
    {
        #region Fields

        private Guid _id;
        private string _name;
        private string _description;
        private List<NPCModel> _npcs = new List<NPCModel>();
        private LocationModel _location;
        private List<EncounterModel> _encounters = new List<EncounterModel>();
        private string _outcomes;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="EventModel"/>
        /// </summary>
        public EventModel()
        {
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Creates a copy of <see cref="EventModel"/>
        /// </summary>
        public EventModel(EventModel eventModel)
        {
            _id = eventModel.ID;
            _name = eventModel.Name;
            _description = eventModel.Description;

            _npcs = new List<NPCModel>();
            foreach (NPCModel npcModel in eventModel.NPCs)
            {
                _npcs.Add(new NPCModel(npcModel));
            }

            _location = new LocationModel(eventModel.Location);

            _encounters = new List<EncounterModel>();
            foreach (EncounterModel encounterModel in eventModel.Encounters)
            {
                _encounters.Add(new EncounterModel(encounterModel));
            }

            _outcomes = eventModel.Outcomes;
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
        /// Gets or sets npcs
        /// </summary>
        public List<NPCModel> NPCs
        {
            get { return _npcs; }
            set { _npcs = value; }
        }

        /// <summary>
        /// Gets or sets location
        /// </summary>
        public LocationModel Location
        {
            get { return _location; }
            set { _location = value; }
        }

        /// <summary>
        /// Gets or sets encounters
        /// </summary>
        public List<EncounterModel> Encounters
        {
            get { return _encounters; }
            set { _encounters = value; }
        }

        /// <summary>
        /// Gets or sets outcomes
        /// </summary>
        public string Outcomes
        {
            get { return _outcomes; }
            set { _outcomes = value; }
        }

        #endregion
    }
}
