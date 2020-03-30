using CriticalCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class EncounterCharacterViewModel : EncounterCreatureViewModel
    {
        #region Fields

        private EncounterCharacterModel _encounterCharacterModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="EncounterCharacterViewModel"/>
        /// </summary>
        public EncounterCharacterViewModel(EncounterCharacterModel encounterCharacterModel)
            : base(encounterCharacterModel)
        {
            _encounterCharacterModel = encounterCharacterModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets encounter character model
        /// </summary>
        public EncounterCharacterModel EncounterCharacterModel
        {
            get { return _encounterCharacterModel; }
        }

        /// <summary>
        /// Gets level
        /// </summary>
        public int Level
        {
            get { return _encounterCharacterModel.Level; }
        }

        /// <summary>
        /// Gets passive investigation
        /// </summary>
        public int PassiveInvestigation
        {
            get { return _encounterCharacterModel.PassiveInvestigation; }
        }

        #endregion
    }
}
