using System;
using System.Collections.Generic;

namespace CriticalCompendiumInfrastructure.Models
{
    public sealed class AdventureModel
    {
        #region Fields

        private Guid _id;
        private string _name;
        private List<string> _tags = new List<string>();
        private string _introduction;
        private List<string> _goals = new List<string>();
        private List<HookModel> _hooks = new List<HookModel>();
        private List<EventModel> _events = new List<EventModel>();
        private List<Guid> _sideQuests = new List<Guid>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="AdventureModel"/>
        /// </summary>
        public AdventureModel()
        {
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Creates a copy of <see cref="AdventureModel"/>
        /// </summary>
        public AdventureModel(AdventureModel adventureModel)
        {
            _id = adventureModel.ID;
            _name = adventureModel.Name;
            _tags = new List<string>(adventureModel.Tags);
            _introduction = adventureModel.Introduction;
            _goals = new List<string>(adventureModel.Goals);

            _hooks = new List<HookModel>();
            foreach (HookModel hookModel in adventureModel.Hooks)
            {
                _hooks.Add(new HookModel(hookModel));
            }

            _events = new List<EventModel>();
            foreach (EventModel eventModel in adventureModel.Events)
            {
                _events.Add(new EventModel(eventModel));
            }

            _sideQuests = new List<Guid>(adventureModel.SideQuests);
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
        /// Gets or sets introduction
        /// </summary>
        public string Introduction
        {
            get { return _introduction; }
            set { _introduction = value; }
        }

        /// <summary>
        /// Gets or sets goals
        /// </summary>
        public List<string> Goals
        {
            get { return _goals; }
            set { _goals = value; }
        }

        /// <summary>
        /// Gets or sets hooks
        /// </summary>
        public List<HookModel> Hooks
        {
            get { return _hooks; }
            set { _hooks = value; }
        }

        /// <summary>
        /// Gets or sets events
        /// </summary>
        public List<EventModel> Events
        {
            get { return _events; }
            set { _events = value; }
        }

        /// <summary>
        /// Gets or sets side quests
        /// </summary>
        public List<Guid> SideQuests
        {
            get { return _sideQuests; }
            set { _sideQuests = value; }
        }

        #endregion
    }
}
