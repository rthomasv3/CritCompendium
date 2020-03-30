using System;

namespace CriticalCompendiumInfrastructure.Models
{
    public sealed class SpellbookEntryModel
    {
        #region Fields

        private Guid _id;
        private bool _prepared;
        private bool _used;
        private SpellModel _spell;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="SpellbookEntryModel"/>
        /// </summary>
        public SpellbookEntryModel()
        {
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Creates an instance of <see cref="SpellbookEntryModel"/>
        /// </summary>
        public SpellbookEntryModel(SpellbookEntryModel spellbookEntryModel)
        {
            _id = spellbookEntryModel.ID;
            _prepared = spellbookEntryModel.Prepared;
            _used = spellbookEntryModel.Used;
            if (spellbookEntryModel.Spell != null)
            {
                _spell = new SpellModel(spellbookEntryModel.Spell);
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
        /// Gets or sets prepared
        /// </summary>
        public bool Prepared
        {
            get { return _prepared; }
            set { _prepared = value; }
        }

        /// <summary>
        /// Gets or sets used
        /// </summary>
        public bool Used
        {
            get { return _used; }
            set { _used = value; }
        }

        /// <summary>
        /// Gets or sets spell
        /// </summary>
        public SpellModel Spell
        {
            get { return _spell; }
            set { _spell = value; }
        }
        
        #endregion
    }
}
