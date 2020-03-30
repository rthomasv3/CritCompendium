using System;
using System.Collections.Generic;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendium.ViewModels.ObjectViewModels;
using CriticalCompendiumInfrastructure;
using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.DialogViewModels
{
    public class EncounterCharacterDialogViewModel : NotifyPropertyChanged, IConfirmation
    {
        #region Events

        public event EventHandler AcceptSelected;
        public event EventHandler RejectSelected;
        public event EventHandler CancelSelected;

        #endregion

        #region Fields
        
        private readonly Compendium _compendium = DependencyResolver.Resolve<Compendium>();
        private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();

        private readonly EncounterCharacterModel _encounterCreatureModel;
        private readonly List<KeyValuePair<CharacterModel, string>> _characterOptions = new List<KeyValuePair<CharacterModel, string>>();
        private KeyValuePair<CharacterModel, string> _selectedCharacterOption;
        private readonly ICommand _acceptCommand;
        private readonly ICommand _rejectCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="EncounterCharacterDialogViewModel"/>
        /// </summary>
        public EncounterCharacterDialogViewModel(EncounterCharacterModel encounterCharacter)
        {
            _encounterCreatureModel = encounterCharacter;

            _characterOptions.Clear();
            _characterOptions.Add(new KeyValuePair<CharacterModel, string>(null, "None"));
            foreach (CharacterModel characterModel in _compendium.Characters)
            {
                _characterOptions.Add(new KeyValuePair<CharacterModel, string>(characterModel, characterModel.Name));
            }
            _selectedCharacterOption = _characterOptions[0];

            _acceptCommand = new RelayCommand(obj => true, obj => OnAccept());
            _rejectCommand = new RelayCommand(obj => true, obj => OnReject());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets character options
        /// </summary>
        public IEnumerable<KeyValuePair<CharacterModel, string>> CharacterOptions
        {
            get { return _characterOptions; }
        }

        /// <summary>
        /// Gets selected character option
        /// </summary>
        public KeyValuePair<CharacterModel, string> SelectedCharacterOption
        {
            get { return _selectedCharacterOption; }
            set
            {
                _selectedCharacterOption = value;
                CharacterModel = value.Key;
            }
        }

        /// <summary>
        /// Gets encounter character model
        /// </summary>
        public EncounterCharacterModel EncounterCharacterModel
        {
            get { return _encounterCreatureModel; }
        }

        /// <summary>
        /// Gets or sets character model
        /// </summary>
        public CharacterModel CharacterModel
        {
            get { return _encounterCreatureModel.CharacterModel; }
            set
            {
                _encounterCreatureModel.CharacterModel = value;
                InitializeFromCharacter();
            }
        }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name
        {
            get { return _encounterCreatureModel.Name; }
            set { _encounterCreatureModel.Name = value; }
        }

        /// <summary>
        /// Gets or sets level
        /// </summary>
        public int Level
        {
            get { return _encounterCreatureModel.Level; }
            set { _encounterCreatureModel.Level = value; }
        }

        /// <summary>
        /// Gets or sets current hp
        /// </summary>
        public int CurrentHP
        {
            get { return _encounterCreatureModel.CurrentHP; }
            set { _encounterCreatureModel.CurrentHP = value; }
        }

        /// <summary>
        /// Gets or sets max hp
        /// </summary>
        public int MaxHP
        {
            get { return _encounterCreatureModel.MaxHP; }
            set { _encounterCreatureModel.MaxHP = value; }
        }

        /// <summary>
        /// Gets or sets ac
        /// </summary>
        public int AC
        {
            get { return _encounterCreatureModel.AC; }
            set { _encounterCreatureModel.AC = value; }
        }

        /// <summary>
        /// Gets or sets spell save dc
        /// </summary>
        public int SpellSaveDC
        {
            get { return _encounterCreatureModel.SpellSaveDC; }
            set { _encounterCreatureModel.SpellSaveDC = value; }
        }

        /// <summary>
        /// Gets or sets initiative bonus
        /// </summary>
        public int InitiativeBonus
        {
            get { return _encounterCreatureModel.InitiativeBonus; }
            set { _encounterCreatureModel.InitiativeBonus = value; }
        }

        /// <summary>
        /// Gets or sets initiative
        /// </summary>
        public int? Initiative
        {
            get { return _encounterCreatureModel.Initiative; }
            set { _encounterCreatureModel.Initiative = value; }
        }

        /// <summary>
        /// Gets or sets passive perception
        /// </summary>
        public int PassivePerception
        {
            get { return _encounterCreatureModel.PassivePerception; }
            set { _encounterCreatureModel.PassivePerception = value; }
        }

        /// <summary>
        /// Gets or sets passive investigation
        /// </summary>
        public int PassiveInvestigation
        {
            get { return _encounterCreatureModel.PassiveInvestigation; }
            set { _encounterCreatureModel.PassiveInvestigation = value; }
        }

        /// <summary>
        /// Gets accept command
        /// </summary>
        public ICommand AcceptCommand
        {
            get { return _acceptCommand; }
        }

        /// <summary>
        /// Gets reject command
        /// </summary>
        public ICommand RejectCommand
        {
            get { return _rejectCommand; }
        }

        #endregion

        #region Non-Public Methods

        private void InitializeFromCharacter()
        {
            if (_encounterCreatureModel.CharacterModel != null)
            {
                CharacterViewModel characterViewModel = new CharacterViewModel(_encounterCreatureModel.CharacterModel);
                _encounterCreatureModel.Name = characterViewModel.Name;
                _encounterCreatureModel.Level = characterViewModel.Level;
                _encounterCreatureModel.MaxHP = characterViewModel.MaxHP;
                _encounterCreatureModel.AC = characterViewModel.AC;

                foreach (SpellbookViewModel spellbookView in characterViewModel.Spellbooks)
                {
                    if (spellbookView.BasedOnClass)
                    {
                        _encounterCreatureModel.SpellSaveDC = spellbookView.SaveDC;
                    }
                }

                _encounterCreatureModel.InitiativeBonus = characterViewModel.InitiativeBonus;
                _encounterCreatureModel.PassivePerception = characterViewModel.PassivePerception;
                _encounterCreatureModel.PassiveInvestigation = characterViewModel.PassiveInvestigation;
            }
            else
            {
                _encounterCreatureModel.Name = String.Empty;
                _encounterCreatureModel.Level = 0;
                _encounterCreatureModel.MaxHP = 0;
                _encounterCreatureModel.AC = 0;
                _encounterCreatureModel.SpellSaveDC = 0;
                _encounterCreatureModel.InitiativeBonus = 0;
                _encounterCreatureModel.PassivePerception = 0;
                _encounterCreatureModel.PassiveInvestigation = 0;
            }

            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Level));
            OnPropertyChanged(nameof(MaxHP));
            OnPropertyChanged(nameof(AC));
            OnPropertyChanged(nameof(SpellSaveDC));
            OnPropertyChanged(nameof(InitiativeBonus));
            OnPropertyChanged(nameof(PassivePerception));
            OnPropertyChanged(nameof(PassiveInvestigation));
        }

        private void OnAccept()
        {
            if (String.IsNullOrWhiteSpace(_encounterCreatureModel.Name))
            {
                _dialogService.ShowConfirmationDialog("Required Field", "Name is required.", "OK", null, null);
            }
            else
            {
                AcceptSelected?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnReject()
        {
            RejectSelected?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
