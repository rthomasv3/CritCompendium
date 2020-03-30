using System;

namespace CriticalCompendiumInfrastructure.Models
{
    public sealed class EquipmentModel
    {
        #region Fields

        private Guid _id;
        private ItemModel _item;
        private string _name;
        private bool _equipped;
        private int _quantity = 1;
        private string _notes;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="EquipmentModel"/>
        /// </summary>
        public EquipmentModel()
        {
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Creates an instance of <see cref="EquipmentModel"/>
        /// </summary>
        public EquipmentModel(EquipmentModel equipmentModel)
        {
            _id = equipmentModel.ID;
            if (equipmentModel.Item != null)
            {
                _item = new ItemModel(equipmentModel.Item);
            }
            _name = equipmentModel.Name;
            _equipped = equipmentModel.Equipped;
            _quantity = equipmentModel.Quantity;
            _notes = equipmentModel.Notes;
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
        /// Gets or sets item
        /// </summary>
        public ItemModel Item
        {
            get { return _item; }
            set { _item = value; }
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
        /// Gets or sets equipped
        /// </summary>
        public bool Equipped
        {
            get { return _equipped; }
            set { _equipped = value; }
        }

        /// <summary>
        /// Gets or sets quantity
        /// </summary>
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
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
