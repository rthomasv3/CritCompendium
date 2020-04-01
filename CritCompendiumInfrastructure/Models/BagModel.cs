using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Models
{
    public sealed class BagModel
    {
        #region Fields

        private Guid _id;
        private string _name;
        private bool _fixedWeight;
        private int _fixedWeightValue;
        private int _copper;
        private int _silver;
        private int _electrum;
        private int _gold;
        private int _platinum;
        private List<EquipmentModel> _equipment = new List<EquipmentModel>();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="BagModel"/>
        /// </summary>
        public BagModel()
        {
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Creates an instance of <see cref="BagModel"/>
        /// </summary>
        public BagModel(BagModel bagModel)
        {
            _id = bagModel.ID;
            _name = bagModel.Name;
            _fixedWeight = bagModel.FixedWeight;
            _fixedWeightValue = bagModel.FixedWeightValue;
            _copper = bagModel.Copper;
            _silver = bagModel.Silver;
            _electrum = bagModel.Electrum;
            _gold = bagModel.Gold;
            _platinum = bagModel.Platinum;
            _equipment = new List<EquipmentModel>();
            foreach (EquipmentModel equipmentModel in bagModel.Equipment)
            {
                _equipment.Add(new EquipmentModel(equipmentModel));
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
        /// Gets or sets fixed weight
        /// </summary>
        public bool FixedWeight
        {
            get { return _fixedWeight; }
            set { _fixedWeight = value; }
        }

        /// <summary>
        /// Gets or sets fixed weight value
        /// </summary>
        public int FixedWeightValue
        {
            get { return _fixedWeightValue; }
            set { _fixedWeightValue = value; }
        }

        /// <summary>
        /// Gets or sets copper
        /// </summary>
        public int Copper
        {
            get { return _copper; }
            set { _copper = value; }
        }

        /// <summary>
        /// Gets or sets silver
        /// </summary>
        public int Silver
        {
            get { return _silver; }
            set { _silver = value; }
        }

        /// <summary>
        /// Gets or sets electrum
        /// </summary>
        public int Electrum
        {
            get { return _electrum; }
            set { _electrum = value; }
        }

        /// <summary>
        /// Gets or sets gold
        /// </summary>
        public int Gold
        {
            get { return _gold; }
            set { _gold = value; }
        }

        /// <summary>
        /// Gets or sets platinum
        /// </summary>
        public int Platinum
        {
            get { return _platinum; }
            set { _platinum = value; }
        }

        /// <summary>
        /// Gets or sets equipment
        /// </summary>
        public List<EquipmentModel> Equipment
        {
            get { return _equipment; }
            set { _equipment = value; }
        }

        #endregion
    }
}
