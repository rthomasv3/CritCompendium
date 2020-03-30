using System;
using CriticalCompendiumInfrastructure.Enums;

namespace CriticalCompendiumInfrastructure.Models
{
    public sealed class ArmorClassModel
    {
        #region Fields

        private Guid _id;
        private int _armorBonus = 10;
        private ArmorType _armorType = ArmorType.None;
        private Ability _firstAbility = Ability.Dexterity;
        private Ability _secondAbility = Ability.None;
        private int _itemBonus;
        private int _additionalBonus;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="ArmorClassModel"/>
        /// </summary>
        public ArmorClassModel()
        {
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Creates an instance of <see cref="ArmorClassModel"/>
        /// </summary>
        public ArmorClassModel(ArmorClassModel armorClassModel)
        {
            _id = armorClassModel.ID;
            _armorBonus = armorClassModel.ArmorBonus;
            _armorType = armorClassModel.ArmorType;
            _firstAbility = armorClassModel.FirstAbility;
            _secondAbility = armorClassModel.SecondAbility;
            _itemBonus = armorClassModel.ItemBonus;
            _additionalBonus = armorClassModel.AdditionalBonus;
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
        /// Gets or sets armor bonus
        /// </summary>
        public int ArmorBonus
        {
            get { return _armorBonus; }
            set { _armorBonus = value; }
        }

        /// <summary>
        /// Gets or sets armor type
        /// </summary>
        public ArmorType ArmorType
        {
            get { return _armorType; }
            set { _armorType = value; }
        }
        
        /// <summary>
        /// Gets or sets first ability
        /// </summary>
        public Ability FirstAbility
        {
            get { return _firstAbility; }
            set { _firstAbility = value; }
        }

        /// <summary>
        /// Gets or sets second ability
        /// </summary>
        public Ability SecondAbility
        {
            get { return _secondAbility; }
            set { _secondAbility = value; }
        }

        /// <summary>
        /// Gets or sets item bonus
        /// </summary>
        public int ItemBonus
        {
            get { return _itemBonus; }
            set { _itemBonus = value; }
        }

        /// <summary>
        /// Gets or sets additional bonus
        /// </summary>
        public int AdditionalBonus
        {
            get { return _additionalBonus; }
            set { _additionalBonus = value; }
        }
        
        #endregion
    }
}
