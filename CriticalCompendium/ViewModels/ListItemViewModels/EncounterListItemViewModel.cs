using System;
using System.Linq;
using CriticalCompendiumInfrastructure;
using CriticalCompendiumInfrastructure.Enums;
using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ListItemViewModels
{
    public sealed class EncounterListItemViewModel : NotifyPropertyChanged
    {
        #region Fields

        private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
        private EncounterModel _encounterModel;
        private string _description;
        private bool _isSelected = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="EncounterListItemViewModel"/>
        /// </summary>
        /// <param name="characterModel"></param>
        public EncounterListItemViewModel(EncounterModel encounterModel)
        {
            _encounterModel = encounterModel;

            Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Encounter model
        /// </summary>
        public EncounterModel EncounterModel
        {
            get { return _encounterModel; }
        }

        /// <summary>
        /// Character name
        /// </summary>
        public string Name
        {
            get { return _encounterModel.Name; }
        }

        /// <summary>
        /// Character description
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// True if selected
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { Set(ref _isSelected, value); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the model
        /// </summary>
        public void UpdateModel(EncounterModel encounterModel)
        {
            _encounterModel = encounterModel;
            Initialize();
            OnPropertyChanged(String.Empty);
        }

        #endregion

        #region Non-Public Methods

        private void Initialize()
        {
            int numCharacters = _encounterModel.Creatures.Where(x => x is EncounterCharacterModel).Count();
            int numMonsters = _encounterModel.Creatures.Where(x => x is EncounterMonsterModel).Select(x => ((EncounterMonsterModel)x).Quantity).Sum();

            _description = $"{numCharacters} Characters, {numMonsters} Monsters, Challenge ";

            _description += _stringService.GetString(_encounterModel.EncounterChallenge);
        }

        #endregion
    }
}
