using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Models
{
    public sealed class PackModel
    {
        #region Fields

        private Guid _id;
        private string _name;
        private List<string> _items = new List<string>();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="PackModel"/>
        /// </summary>
        public PackModel()
        {
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Creates an instance of <see cref="PackModel"/>
        /// </summary>
        public PackModel(PackModel packModel)
        {
            _id = packModel.ID;
            _name = packModel.Name;

            _items = new List<string>();
            foreach (string item in packModel.Items)
            {
                _items.Add(item);
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
        /// Gets or sets items
        /// </summary>
        public List<string> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        #endregion
    }
}
