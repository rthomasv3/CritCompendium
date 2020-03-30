using System;
using System.Collections.Generic;
using System.Text;

namespace CriticalCompendiumInfrastructure.Models
{
    public sealed class CompanionModel
    {
        #region Fields

        private Guid _id;
        private MonsterModel _monsterModel;
        private string _name;
        private int _currentHP;
        private int _maxHP;
        private string _notes;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="CompanionModel"/>
        /// </summary>
        public CompanionModel()
        {
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Creates an instance of <see cref="CompanionModel"/>
        /// </summary>
        public CompanionModel(CompanionModel companionModel)
        {
            _id = companionModel.ID;
            if (companionModel.MonsterModel != null)
            {
                _monsterModel = new MonsterModel(companionModel.MonsterModel);
            }
            _name = companionModel.Name;
            _currentHP = companionModel.CurrentHP;
            _maxHP = companionModel.MaxHP;
            _notes = companionModel.Notes;
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
        /// Gets or sets monster model
        /// </summary>
        public MonsterModel MonsterModel
        {
            get { return _monsterModel; }
            set { _monsterModel = value; }
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
        /// Gets or sets current hp
        /// </summary>
        public int CurrentHP
        {
            get { return _currentHP; }
            set { _currentHP = value; }
        }

        /// <summary>
        /// Gets or sets max hp
        /// </summary>
        public int MaxHP
        {
            get { return _maxHP; }
            set { _maxHP = value; }
        }

        /// <summary>
        /// Gets or sets notes
        /// </summary>
        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }

        #endregion
    }
}
