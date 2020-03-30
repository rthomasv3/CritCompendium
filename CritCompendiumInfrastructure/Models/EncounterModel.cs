using CriticalCompendiumInfrastructure.Enums;
using System;
using System.Collections.Generic;

namespace CriticalCompendiumInfrastructure.Models
{
    public sealed class EncounterModel
    {
        #region Fields

        private Guid _id;
        private string _name;
        private List<EncounterCreatureModel> _encounterCreatures = new List<EncounterCreatureModel>();
        private int _round;
        private EncounterChallenge _encounterChallenge;
        private int _totalCharacterHP;
        private int _totalMonsterHP;
        private int _timeElapsed;
        private int _currentTurn;
        private EncounterState _encounterState = EncounterState.Stopped;
        private string _notes;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="EncounterModel"/>
        /// </summary>
        public EncounterModel()
        {
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Creates an instance of <see cref="EncounterModel"/>
        /// </summary>
        public EncounterModel(EncounterModel encounterModel)
        {
            _id = encounterModel.ID;
            _name = encounterModel.Name;
            _encounterCreatures = new List<EncounterCreatureModel>();
            foreach (EncounterCreatureModel encounterCreature in encounterModel.Creatures)
            {
                if (encounterCreature is EncounterCharacterModel encounterCharacter)
                {
                    _encounterCreatures.Add(new EncounterCharacterModel(encounterCharacter));
                }
                else if (encounterCreature is EncounterMonsterModel encounterMonster)
                {
                    _encounterCreatures.Add(new EncounterMonsterModel(encounterMonster));
                }
            }
            _round = encounterModel.Round;
            _encounterChallenge = encounterModel.EncounterChallenge;
            _totalCharacterHP = encounterModel.TotalCharacterHP;
            _totalMonsterHP = encounterModel.TotalMonsterHP;
            _timeElapsed = encounterModel.TimeElapsed;
            _currentTurn = encounterModel.CurrentTurn;
            _encounterState = encounterModel.EncounterState;
            _notes = encounterModel.Notes;
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
        /// Gets or sets creatures
        /// </summary>
        public List<EncounterCreatureModel> Creatures
        {
            get { return _encounterCreatures; }
            set { _encounterCreatures = value; }
        }

        /// <summary>
        /// Gets or sets round
        /// </summary>
        public int Round
        {
            get { return _round; }
            set { _round = value; }
        }

        /// <summary>
        /// Gets or sets encounter challenge
        /// </summary>
        public EncounterChallenge EncounterChallenge
        {
            get { return _encounterChallenge; }
            set { _encounterChallenge = value; }
        }

        /// <summary>
        /// Gets or sets total character hp
        /// </summary>
        public int TotalCharacterHP
        {
            get { return _totalCharacterHP; }
            set { _totalCharacterHP = value; }
        }

        /// <summary>
        /// Gets or sets total monster hp
        /// </summary>
        public int TotalMonsterHP
        {
            get { return _totalMonsterHP; }
            set { _totalMonsterHP = value; }
        }

        /// <summary>
        /// Gets or sets time elapsed
        /// </summary>
        public int TimeElapsed
        {
            get { return _timeElapsed; }
            set { _timeElapsed = value; }
        }

        /// <summary>
        /// Gets or sets current turn
        /// </summary>
        public int CurrentTurn
        {
            get { return _currentTurn; }
            set { _currentTurn = value; }
        }

        /// <summary>
        /// Gets or sets encounter state
        /// </summary>
        public EncounterState EncounterState
        {
            get { return _encounterState; }
            set { _encounterState = value; }
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
