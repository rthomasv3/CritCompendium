using CritCompendiumInfrastructure.Services;
using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Models
{
    public class EncounterCreatureModel
    {
        #region Fields

        private readonly StatService _statService = DependencyResolver.Resolve<StatService>();
        
        private Guid _id;
        private string _name;
        private int _currentHP;
        private int _maxHP;
        private int _ac;
        private int _spellSaveDC;
        private int _passivePerception;
        private int _initiativeBonus;
        private int? _initiative;
        private bool _selected;
        private List<AppliedConditionModel> _conditions = new List<AppliedConditionModel>();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="EncounterCreatureModel"/>
        /// </summary>
        public EncounterCreatureModel()
        {
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Creates an instance of <see cref="EncounterCreatureModel"/>
        /// </summary>
        public EncounterCreatureModel(EncounterCreatureModel encounterCreatureModel)
        {
            _id = encounterCreatureModel.ID;
            _name = encounterCreatureModel.Name;
            _currentHP = encounterCreatureModel.CurrentHP;
            _maxHP = encounterCreatureModel.MaxHP;
            _ac = encounterCreatureModel.AC;
            _spellSaveDC = encounterCreatureModel.SpellSaveDC;
            _passivePerception = encounterCreatureModel.PassivePerception;
            _initiativeBonus = encounterCreatureModel.InitiativeBonus;
            _initiative = encounterCreatureModel.Initiative;
            _selected = Selected;
            _conditions.Clear();
            foreach (AppliedConditionModel appliedConditionModel in encounterCreatureModel.Conditions)
            {
                _conditions.Add(new AppliedConditionModel(appliedConditionModel));
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
        /// Gets less hp
        /// </summary>
        public int LessHP
        {
            get { return _maxHP - _currentHP; }
        }

        /// <summary>
        /// Gets or sets ac
        /// </summary>
        public int AC
        {
            get { return _ac; }
            set { _ac = value; }
        }

        /// <summary>
        /// Gets or sets spell save dc
        /// </summary>
        public int SpellSaveDC
        {
            get { return _spellSaveDC; }
            set { _spellSaveDC = value; }
        }

        /// <summary>
        /// Gets or sets passive perception
        /// </summary>
        public int PassivePerception
        {
            get { return _passivePerception; }
            set { _passivePerception = value; }
        }

        /// <summary>
        /// Gets or sets initiative bonus
        /// </summary>
        public int InitiativeBonus
        {
            get { return _initiativeBonus; }
            set { _initiativeBonus = value; }
        }

        /// <summary>
        /// Gets or sets initiative
        /// </summary>
        public int? Initiative
        {
            get { return _initiative; }
            set { _initiative = value; }
        }

        /// <summary>
        /// Gets or sets selected
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        /// <summary>
        /// Gets or sets conditions
        /// </summary>
        public List<AppliedConditionModel> Conditions
        {
            get { return _conditions; }
            set { _conditions = value; }
        }

        #endregion
    }
}
