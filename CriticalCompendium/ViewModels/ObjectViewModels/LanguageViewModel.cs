using CriticalCompendiumInfrastructure.Models;
using System;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class LanguageViewModel : NotifyPropertyChanged
    {
        #region Events

        public event EventHandler ProficiencyChanged;

        #endregion

        #region Fields

        private readonly LanguageModel _languageModel;
        private readonly string _name;
        private bool _proficient;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="LanguageViewModel"/>
        /// </summary>
        public LanguageViewModel(LanguageModel languageModel)
        {
            _languageModel = languageModel;
            _name = languageModel.Name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets language model
        /// </summary>
        public LanguageModel Model
        {
            get { return _languageModel; }
        }

        /// <summary>
        /// Gets ID
        /// </summary>
        public Guid ID
        {
            get { return _languageModel.ID; }
        }

        /// <summary>
        /// Gets name
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets or set proficient
        /// </summary>
        public bool Proficient
        {
            get { return _proficient; }
            set
            {
                if (Set(ref _proficient, value))
                {
                    ProficiencyChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        #endregion
    }
}
