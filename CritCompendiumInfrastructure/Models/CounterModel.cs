using System;

namespace CriticalCompendiumInfrastructure.Models
{
    public sealed class CounterModel
    {
        #region Fields

        private Guid _id;
        private string _name;
        private int _currentValue;
        private int _maxValue;
        private bool _resetOnShortRest;
        private bool _resetOnLongRest;
        private string _notes;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="CounterModel"/>
        /// </summary>
        public CounterModel()
        {
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Creates an instance of <see cref="CounterModel"/>
        /// </summary>
        public CounterModel(CounterModel counterModel)
        {
            _id = counterModel.ID;
            _name = counterModel.Name;
            _currentValue = counterModel.CurrentValue;
            _maxValue = counterModel.MaxValue;
            _resetOnShortRest = counterModel.ResetOnShortRest;
            _resetOnLongRest = counterModel.ResetOnLongRest;
            _notes = counterModel.Notes;
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
        /// Gets or sets current value
        /// </summary>
        public int CurrentValue
        {
            get { return _currentValue; }
            set { _currentValue = value; }
        }

        /// <summary>
        /// Gets or sets max value
        /// </summary>
        public int MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        /// <summary>
        /// Gets or sets reset on short rest
        /// </summary>
        public bool ResetOnShortRest
        {
            get { return _resetOnShortRest; }
            set { _resetOnShortRest = value; }
        }

        /// <summary>
        /// Gets or sets reset on long rest
        /// </summary>
        public bool ResetOnLongRest
        {
            get { return _resetOnLongRest; }
            set { _resetOnLongRest = value; }
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
