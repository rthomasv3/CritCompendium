using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class CharacterEditViewModel : NotifyPropertyChanged
    {
		#region Fields

		private readonly CharacterModel _characterModel;
		private readonly Compendium _compendium = DependencyResolver.Resolve<Compendium>();
		private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
		private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();
		private readonly DiceService _diceService = DependencyResolver.Resolve<DiceService>();
		private readonly StatService _statService = DependencyResolver.Resolve<StatService>();

        bool _isNew;

        private string _name;
		private Guid _race;
		private List<Tuple<Guid, string>> _races = new List<Tuple<Guid, string>>();
		private Guid _background;
		private List<Tuple<Guid, string>> _backgrounds = new List<Tuple<Guid, string>>();
		private Tuple<Alignment, string> _alignment;
		private readonly List<Tuple<Alignment, string>> _alignments = new List<Tuple<Alignment, string>>();
		private string _deity;
		private string _gender;
		private int _age;
		private int _heightFeet;
		private int _heightInches;
		private int _weight;
		private string _hair;
		private string _eyes;
		private string _skin;
		private int _experience;
		private int _baseStrength;
		private int _baseDexterity;
		private int _baseConstitution;
		private int _baseIntelligence;
		private int _baseWisdom;
		private int _baseCharisma;
        private int _bonusStrength;
        private int _bonusDexterity;
        private int _bonusConstitution;
        private int _bonusIntelligence;
        private int _bonusWisdom;
        private int _bonusCharisma;
        private string _personalityTraits;
        private string _ideals;
        private string _bonds;
        private string _flaws;
        private string _backstory;
		private ObservableCollection<LevelEditViewModel> _levels = new ObservableCollection<LevelEditViewModel>();
		private List<Tuple<AbilityRollMethod, string>> _abilityRollMethods = new List<Tuple<AbilityRollMethod, string>>();
		private Tuple<AbilityRollMethod, string> _selectedAbilityRollMethod;
		private int _timesAbilityScoresRolled;
        private List<AbilityViewModel> _abilityProficiencies = new List<AbilityViewModel>();
        private string _savingThrowNotes;
        private List<SkillViewModel> _skillProficiencies = new List<SkillViewModel>();
        private ObservableCollection<LanguageViewModel> _allLanguages = new ObservableCollection<LanguageViewModel>();
        private List<LanguageViewModel> _languageProficiencies = new List<LanguageViewModel>();
        private ArmorProficiencyViewModel _armorProficiency;
        private WeaponProficiencyViewModel _weaponProficiency;
        private ToolProficiencyViewModel _toolProficiency;

        private readonly RelayCommand _viewRaceCommand;
		private readonly RelayCommand _viewBackgroundCommand;
		private readonly RelayCommand _addLevelCommand;
		private readonly RelayCommand _deleteLevelCommand;
		private readonly RelayCommand _rollAbilityScoresCommand;
		private readonly RelayCommand _moveAbilityUpCommand;
		private readonly RelayCommand _moveAbilityDownCommand;
		private readonly RelayCommand _increaseAbilityCommand;
		private readonly RelayCommand _decreaseAbilityCommand;
		private readonly RelayCommand _rollAbilitySaveCommand;
		private readonly RelayCommand _rollSkillCheckCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates and instance of <see cref="CharacterEditViewModel"/>
        /// </summary>
        public CharacterEditViewModel(CharacterModel characterModel, bool isNew)
		{
			_characterModel = new CharacterModel(characterModel);
            _isNew = isNew;

            if (_isNew)
            {
                InitializeNewCharacter();
            }

            _name = _characterModel.Name;

			foreach (RaceModel race in _compendium.Races)
			{
				_races.Add(new Tuple<Guid, string>(race.ID, race.Name));
			}

			if (_characterModel.Race != null)
			{
				_race = _characterModel.Race.ID;
			}

			foreach (BackgroundModel background in _compendium.Backgrounds)
			{
				_backgrounds.Add(new Tuple<Guid, string>(background.ID, background.Name));
			}

			if (_characterModel.Background != null)
			{
				_background = _characterModel.Background.ID;
			}

			foreach (Alignment alignment in Enum.GetValues(typeof(Alignment)))
			{
				Tuple<Alignment, string> alignmentTuple = new Tuple<Alignment, string>(alignment, _stringService.GetString(alignment));
				_alignments.Add(alignmentTuple);
				if (_characterModel.Alignment == alignment)
				{
					_alignment = alignmentTuple;
				}
			}

			_deity = _characterModel.Deity;
			_gender = _characterModel.Gender;
			_age = _characterModel.Age;
			_heightFeet = _characterModel.HeightFeet;
			_heightInches = _characterModel.HeightInches;
			_weight = _characterModel.Weight;
			_hair = _characterModel.Hair;
			_eyes = _characterModel.Eyes;
			_skin = _characterModel.Skin;
			_experience = _characterModel.Experience;

			_baseStrength = _characterModel.BaseStrength;
			_baseDexterity = _characterModel.BaseDexterity;
			_baseConstitution = _characterModel.BaseConstitution;
			_baseIntelligence = _characterModel.BaseIntelligence;
			_baseWisdom = _characterModel.BaseWisdom;
			_baseCharisma = _characterModel.BaseCharisma;

            _bonusStrength = _characterModel.BonusStrength;
            _bonusDexterity = _characterModel.BonusDexterity;
            _bonusConstitution = _characterModel.BonusConstitution;
            _bonusIntelligence = _characterModel.BonusIntelligence;
            _bonusWisdom = _characterModel.BonusWisdom;
            _bonusCharisma = _characterModel.BonusCharisma;

            _personalityTraits = _characterModel.PersonalityTraits;
			_ideals = _characterModel.Ideals;
			_bonds = _characterModel.Bonds;
			_flaws = _characterModel.Flaws;
			_backstory = _characterModel.Backstory;

            foreach (LevelModel level in _characterModel.Levels)
			{
				LevelEditViewModel levelView = new LevelEditViewModel(level);
				levelView.ClassChanged += LevelView_ClassChanged;
				levelView.PropertyChanged += LevelEditView_PropertyChanged;
				_levels.Add(levelView);
			}
            if (_levels.Count > 1)
            {
                _levels.Last().CanDelete = true;
            }

			SetLevelOfClasses();

			foreach (AbilityRollMethod rollMethod in Enum.GetValues(typeof(AbilityRollMethod)))
			{
				Tuple<AbilityRollMethod, string> method = new Tuple<AbilityRollMethod, string>(rollMethod, rollMethod.ToString().Replace("_", " "));
				_abilityRollMethods.Add(method);
				if (_selectedAbilityRollMethod == null)
				{
					_selectedAbilityRollMethod = method;
				}
			}

            Tuple<AbilityRollMethod, string> selectedRollMethod = _abilityRollMethods.FirstOrDefault(x => x.Item1 == _characterModel.AbilityRollMethod);
            if (!selectedRollMethod.Equals(default(Tuple<AbilityRollMethod, string>)))
            {
                _selectedAbilityRollMethod = selectedRollMethod;
            }

            _savingThrowNotes = _characterModel.SavingThrowNotes;

            foreach (AbilityModel abilityModel in _characterModel.AbilitySaveProficiencies)
            {
                AbilityViewModel abilityView = new AbilityViewModel(abilityModel);
                abilityView.ProficiencyChanged += AbilityView_ProficiencyChanged;
                _abilityProficiencies.Add(abilityView);
            }

            foreach (SkillModel skillModel in _characterModel.SkillProficiencies)
            {
                SkillViewModel skillView = new SkillViewModel(skillModel);
                skillView.ProficiencyChanged += SkillView_ProficiencyChanged;
                _skillProficiencies.Add(skillView);
            }

            foreach (LanguageModel language in _compendium.Languages.OrderBy(x => x.Name))
            {
                LanguageViewModel languageView = new LanguageViewModel(language);
                languageView.Proficient = _characterModel.Languages.Any(x => x.Name.Equals(language.Name, StringComparison.CurrentCultureIgnoreCase));
                languageView.ProficiencyChanged += LanguageView_ProficiencyChanged;
                _allLanguages.Add(languageView);
            }

            foreach (LanguageModel language in _characterModel.Languages)
            {
                _languageProficiencies.Add(new LanguageViewModel(language));
            }
            
            _armorProficiency = new ArmorProficiencyViewModel(_characterModel.ArmorProficiency);
            _armorProficiency.PropertyChanged += _armors_PropertyChanged;

            _weaponProficiency = new WeaponProficiencyViewModel(_characterModel.WeaponProficiency);
            _weaponProficiency.PropertyChanged += _weaponProficiency_PropertyChanged;

            _toolProficiency = new ToolProficiencyViewModel(_characterModel.ToolProficiency);
            _toolProficiency.PropertyChanged += _toolProficiency_PropertyChanged;

            _timesAbilityScoresRolled = _characterModel.TimesAbilityScoresRolled;
            
            if (_isNew)
            {
                UpdateLanguageProficiencies();
                UpdateArmorProficiencies();
                UpdateWeaponProficiencies();
            }

            _viewRaceCommand = new RelayCommand(obj => true, obj => ViewRace());
			_viewBackgroundCommand = new RelayCommand(obj => true, obj => ViewBackground());
			_addLevelCommand = new RelayCommand(obj => _levels.Count < 20, obj => AddLevel());
			_deleteLevelCommand = new RelayCommand(obj => true, obj => DeleteLevel((LevelEditViewModel)obj));
			_rollAbilityScoresCommand = new RelayCommand(obj => true, obj => RollAbilityScores());
			_moveAbilityUpCommand = new RelayCommand(obj => true, obj => MoveAbilityUp((Ability)obj));
			_moveAbilityDownCommand = new RelayCommand(obj => true, obj => MoveAbilityDown((Ability)obj));
			_increaseAbilityCommand = new RelayCommand(obj => true, obj => IncreaseAbility((Ability)obj));
			_decreaseAbilityCommand = new RelayCommand(obj => true, obj => DecreaseAbility((Ability)obj));
			_rollAbilitySaveCommand = new RelayCommand(obj => true, obj => RollAbilitySave((Ability)obj));
			_rollSkillCheckCommand = new RelayCommand(obj => true, obj => RollSkillCheck((Skill)obj));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets is new
        /// </summary>
        public bool IsNew
        {
            get { return _isNew; }
        }

        /// <summary>
        /// Gets model
        /// </summary>
        public CharacterModel CharacterModel
        {
            get { return _characterModel; }
        }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name
		{
			get { return _name; }
			set
			{
				if (Set(ref _name, value))
				{
					_characterModel.Name = _name;
				}
			}
		}

		/// <summary>
		/// Gets or sets race
		/// </summary>
		public Tuple<Guid, string> Race
		{
			get { return _races.FirstOrDefault(x => x.Item1 == _race); }
			set
			{
				if (Set(ref _race, value.Item1))
				{
					_characterModel.Race = _compendium.Races.FirstOrDefault(x => x.ID == _race);

                    _characterModel.MovementModel.WalkSpeed = _characterModel.Race != null ? _characterModel.Race.WalkSpeed : 0;
                    int halfWalk = Math.Max(_characterModel.MovementModel.WalkSpeed / 2, 0);
                    _characterModel.MovementModel.SwimSpeed = halfWalk;
                    _characterModel.MovementModel.ClimbSpeed = halfWalk;
                    _characterModel.MovementModel.CrawlSpeed = halfWalk;
                    _characterModel.MovementModel.FlySpeed = _characterModel.Race != null ? _characterModel.Race.FlySpeed : 0;

                    if (_isNew)
                    {
                        UpdateSkillProficiencies();
                        UpdateLanguageProficiencies();
                    }

                    UpdateAbilityBonusValues();
                    UpdateSkillBonusValues();

                    OnPropertyChanged(nameof(RaceStrengthBonus));
					OnPropertyChanged(nameof(TotalStrength));
					OnPropertyChanged(nameof(StrengthSave));
					OnPropertyChanged(nameof(RaceDexterityBonus));
					OnPropertyChanged(nameof(TotalDexterity));
					OnPropertyChanged(nameof(DexteritySave));
					OnPropertyChanged(nameof(RaceConstitutionBonus));
					OnPropertyChanged(nameof(TotalConstitution));
					OnPropertyChanged(nameof(ConstitutionSave));
					OnPropertyChanged(nameof(RaceIntelligenceBonus));
					OnPropertyChanged(nameof(TotalIntelligence));
					OnPropertyChanged(nameof(IntelligenceSave));
					OnPropertyChanged(nameof(RaceWisdomBonus));
					OnPropertyChanged(nameof(TotalWisdom));
					OnPropertyChanged(nameof(WisdomSave));
					OnPropertyChanged(nameof(RaceCharismaBonus));
					OnPropertyChanged(nameof(TotalCharisma));
					OnPropertyChanged(nameof(CharismaSave));
					OnPropertyChanged(nameof(RaceTraitSkillBonus));
					OnPropertyChanged(nameof(RaceTraitLanguageBonus));
                }
			}
		}

		/// <summary>
		/// Gets races
		/// </summary>
		public IEnumerable<Tuple<Guid, string>> Races
		{
			get { return _races; }
		}

		/// <summary>
		/// Gets or sets background
		/// </summary>
		public Tuple<Guid, string> Background
		{
			get { return _backgrounds.FirstOrDefault(x => x.Item1 == _background); }
			set
			{
				if (Set(ref _background, value.Item1))
				{
					_characterModel.Background = _compendium.Backgrounds.FirstOrDefault(x => x.ID == _background);

                    if (_isNew)
                    {
                        UpdateSkillProficiencies();
                        UpdateLanguageProficiencies();
                    }

                    OnPropertyChanged(nameof(BackgroundTraitSkillBonus));
                    OnPropertyChanged(nameof(BackgroundTraitLanguageBonus));
                    OnPropertyChanged(nameof(BackgroundTraitToolBonus));
                }
			}
		}

		/// <summary>
		/// Gets backgrounds
		/// </summary>
		public IEnumerable<Tuple<Guid, string>> Backgrounds
		{
			get { return _backgrounds; }
		}

		/// <summary>
		/// Gets or sets alignment
		/// </summary>
		public Tuple<Alignment, string> Alignment
		{
			get { return _alignment; }
			set
			{
				if (Set(ref _alignment, value))
				{
					_characterModel.Alignment = _alignment.Item1;
				}
			}
		}

		/// <summary>
		/// Gets alignments
		/// </summary>
		public IEnumerable<Tuple<Alignment, string>> Alignments
		{
			get { return _alignments; }
		}

		/// <summary>
		/// Gets or sets deity
		/// </summary>
		public string Deity
		{
			get { return _deity; }
			set
			{
				if (Set(ref _deity, value))
				{
					_characterModel.Deity = _deity;
				}
			}
		}

		/// <summary>
		/// Gets or sets gender
		/// </summary>
		public string Gender
		{
			get { return _gender; }
			set
			{
				if (Set(ref _gender, value))
				{
					_characterModel.Gender = _gender;
				}
			}
		}

		/// <summary>
		/// Gets or sets age
		/// </summary>
		public int Age
		{
			get { return _age; }
			set
			{
				if (Set(ref _age, value))
				{
					_characterModel.Age = _age;
				}
			}
		}

		/// <summary>
		/// Gets or sets height feet
		/// </summary>
		public int HeightFeet
		{
			get { return _heightFeet; }
			set
			{
				if (Set(ref _heightFeet, value))
				{
					_characterModel.HeightFeet = _heightFeet;
				}
			}
		}

		/// <summary>
		/// Gets or sets height inches
		/// </summary>
		public int HeightInches
		{
			get { return _heightInches; }
			set
			{
				if (Set(ref _heightInches, value))
				{
					_characterModel.HeightInches = _heightInches;
				}
			}
		}

		/// <summary>
		/// Gets or sets weight
		/// </summary>
		public int Weight
		{
			get { return _weight; }
			set
			{
				if (Set(ref _weight, value))
				{
					_characterModel.Weight = _weight;
				}
			}
		}

		/// <summary>
		/// Gets or sets hair
		/// </summary>
		public string Hair
		{
			get { return _hair; }
			set
			{
				if (Set(ref _hair, value))
				{
					_characterModel.Hair = _hair;
				}
			}
		}

		/// <summary>
		/// Gets or sets eyes
		/// </summary>
		public string Eyes
		{
			get { return _eyes; }
			set
			{
				if (Set(ref _eyes, value))
				{
					_characterModel.Eyes = _eyes;
				}
			}
		}

		/// <summary>
		/// Gets or sets skin
		/// </summary>
		public string Skin
		{
			get { return _skin; }
			set
			{
				if (Set(ref _skin, value))
				{
					_characterModel.Skin = _skin;
				}
			}
		}

		/// <summary>
		/// Gets or sets experience
		/// </summary>
		public int Experience
		{
			get { return _experience; }
			set
			{
				if (Set(ref _experience, value))
				{
					_characterModel.Experience = _experience;
				}
			}
		}

		/// <summary>
		/// Gets or sets base strength
		/// </summary>
		public int BaseStrength
		{
			get { return _baseStrength; }
			set
			{
				if (Set(ref _baseStrength, value))
				{
					_characterModel.BaseStrength = _baseStrength;
					OnPropertyChanged(nameof(TotalStrength));
					OnPropertyChanged(nameof(StrengthSave));
				}
			}
		}

		/// <summary>
		/// Gets or sets base dexterity
		/// </summary>
		public int BaseDexterity
		{
			get { return _baseDexterity; }
			set
			{
				if (Set(ref _baseDexterity, value))
				{
					_characterModel.BaseDexterity = _baseDexterity;
					OnPropertyChanged(nameof(TotalDexterity));
					OnPropertyChanged(nameof(DexteritySave));
				}
			}
		}

		/// <summary>
		/// Gets or sets base constitution
		/// </summary>
		public int BaseConstitution
		{
			get { return _baseConstitution; }
			set
			{
				if (Set(ref _baseConstitution, value))
				{
					_characterModel.BaseConstitution = _baseConstitution;
					OnPropertyChanged(nameof(TotalConstitution));
					OnPropertyChanged(nameof(ConstitutionSave));
				}
			}
		}

		/// <summary>
		/// Gets or sets base intelligence
		/// </summary>
		public int BaseIntelligence
		{
			get { return _baseIntelligence; }
			set
			{
				if (Set(ref _baseIntelligence, value))
				{
					_characterModel.BaseIntelligence = _baseIntelligence;
					OnPropertyChanged(nameof(TotalIntelligence));
					OnPropertyChanged(nameof(IntelligenceSave));
				}
			}
		}

		/// <summary>
		/// Gets or sets base wisdom
		/// </summary>
		public int BaseWisdom
		{
			get { return _baseWisdom; }
			set
			{
				if (Set(ref _baseWisdom, value))
				{
					_characterModel.BaseWisdom = _baseWisdom;
					OnPropertyChanged(nameof(TotalWisdom));
					OnPropertyChanged(nameof(WisdomSave));
				}
			}
		}

		/// <summary>
		/// Gets or sets base charisma
		/// </summary>
		public int BaseCharisma
		{
			get { return _baseCharisma; }
			set
			{
				if (Set(ref _baseCharisma, value))
				{
					_characterModel.BaseCharisma = _baseCharisma;
					OnPropertyChanged(nameof(TotalCharisma));
					OnPropertyChanged(nameof(CharismaSave));
				}
			}
		}

        /// <summary>
        /// Gets or sets bonus strength
        /// </summary>
        public int BonusStrength
        {
            get { return _bonusStrength; }
            set
            {
                if (Set(ref _bonusStrength, value))
                {
                    _characterModel.BonusStrength = _bonusStrength;
                    OnPropertyChanged(nameof(TotalStrength));
                    OnPropertyChanged(nameof(StrengthSave));
                }
            }
        }

        /// <summary>
        /// Gets or sets bonus dexterity
        /// </summary>
        public int BonusDexterity
        {
            get { return _bonusDexterity; }
            set
            {
                if (Set(ref _bonusDexterity, value))
                {
                    _characterModel.BonusDexterity = _bonusDexterity;
                    OnPropertyChanged(nameof(TotalDexterity));
                    OnPropertyChanged(nameof(DexteritySave));
                }
            }
        }

        /// <summary>
        /// Gets or sets bonus constitution
        /// </summary>
        public int BonusConstitution
        {
            get { return _bonusConstitution; }
            set
            {
                if (Set(ref _bonusConstitution, value))
                {
                    _characterModel.BonusConstitution = _bonusConstitution;
                    OnPropertyChanged(nameof(TotalConstitution));
                    OnPropertyChanged(nameof(ConstitutionSave));
                }
            }
        }

        /// <summary>
        /// Gets or sets bonus intelligence
        /// </summary>
        public int BonusIntelligence
        {
            get { return _bonusIntelligence; }
            set
            {
                if (Set(ref _bonusIntelligence, value))
                {
                    _characterModel.BonusIntelligence = _bonusIntelligence;
                    OnPropertyChanged(nameof(TotalIntelligence));
                    OnPropertyChanged(nameof(IntelligenceSave));
                }
            }
        }

        /// <summary>
        /// Gets or sets bonus wisdom
        /// </summary>
        public int BonusWisdom
        {
            get { return _bonusWisdom; }
            set
            {
                if (Set(ref _bonusWisdom, value))
                {
                    _characterModel.BonusWisdom = _bonusWisdom;
                    OnPropertyChanged(nameof(TotalWisdom));
                    OnPropertyChanged(nameof(WisdomSave));
                }
            }
        }

        /// <summary>
        /// Gets or sets bonus charisma
        /// </summary>
        public int BonusCharisma
        {
            get { return _bonusCharisma; }
            set
            {
                if (Set(ref _bonusCharisma, value))
                {
                    _characterModel.BonusCharisma = _bonusCharisma;
                    OnPropertyChanged(nameof(TotalCharisma));
                    OnPropertyChanged(nameof(CharismaSave));
                }
            }
        }

        /// <summary>
        /// Gets race strength bonus
        /// </summary>
        public int RaceStrengthBonus
		{
			get { return GetRaceAbilityBonus(Ability.Strength); }
		}

		/// <summary>
		/// Gets race dexterity bonus
		/// </summary>
		public int RaceDexterityBonus
		{
			get { return GetRaceAbilityBonus(Ability.Dexterity); }
		}

		/// <summary>
		/// Gets race constitution bonus
		/// </summary>
		public int RaceConstitutionBonus
		{
			get { return GetRaceAbilityBonus(Ability.Constitution); }
		}

		/// <summary>
		/// Gets race intelligence bonus
		/// </summary>
		public int RaceIntelligenceBonus
		{
			get { return GetRaceAbilityBonus(Ability.Intelligence); }
		}

		/// <summary>
		/// Gets race wisdom bonus
		/// </summary>
		public int RaceWisdomBonus
		{
			get { return GetRaceAbilityBonus(Ability.Wisdom); }
		}

		/// <summary>
		/// Gets race charisma bonus
		/// </summary>
		public int RaceCharismaBonus
		{
			get { return GetRaceAbilityBonus(Ability.Charisma); }
		}

		/// <summary>
		/// Gets level strength bonus
		/// </summary>
		public int LevelStrengthBonus
		{
			get { return GetLevelAbilityBonus(Ability.Strength); }
		}

		/// <summary>
		/// Gets level dexterity bonus
		/// </summary>
		public int LevelDexterityBonus
		{
			get { return GetLevelAbilityBonus(Ability.Dexterity); }
		}

		/// <summary>
		/// Gets level constitution bonus
		/// </summary>
		public int LevelConstitutionBonus
		{
			get { return GetLevelAbilityBonus(Ability.Constitution); }
		}

		/// <summary>
		/// Gets level intelligence bonus
		/// </summary>
		public int LevelIntelligenceBonus
		{
			get { return GetLevelAbilityBonus(Ability.Intelligence); }
		}

		/// <summary>
		/// Gets level wisdom bonus
		/// </summary>
		public int LevelWisdomBonus
		{
			get { return GetLevelAbilityBonus(Ability.Wisdom); }
		}

		/// <summary>
		/// Gets level charisma bonus
		/// </summary>
		public int LevelCharismaBonus
		{
			get { return GetLevelAbilityBonus(Ability.Charisma); }
		}

		/// <summary>
		/// Gets total strength
		/// </summary>
		public int TotalStrength
		{
			get { return _baseStrength + _bonusStrength + RaceStrengthBonus + LevelStrengthBonus; }
		}

		/// <summary>
		/// Gets total dexterity
		/// </summary>
		public int TotalDexterity
		{
			get { return _baseDexterity + _bonusDexterity + RaceDexterityBonus + LevelDexterityBonus; }
		}

		/// <summary>
		/// Gets total constitution
		/// </summary>
		public int TotalConstitution
		{
			get { return _baseConstitution + _bonusConstitution + RaceConstitutionBonus + LevelConstitutionBonus; }
		}

		/// <summary>
		/// Gets total intelligence
		/// </summary>
		public int TotalIntelligence
		{
			get { return _baseIntelligence + _bonusIntelligence + RaceIntelligenceBonus + LevelIntelligenceBonus; }
		}

		/// <summary>
		/// Gets total wisdom
		/// </summary>
		public int TotalWisdom
		{
			get { return _baseWisdom + _bonusWisdom + RaceWisdomBonus + LevelWisdomBonus; }
		}

		/// <summary>
		/// Gets total charisma
		/// </summary>
		public int TotalCharisma
		{
			get { return _baseCharisma + _bonusCharisma + RaceCharismaBonus + LevelCharismaBonus; }
		}

        /// <summary>
		/// Gets or sets personality traits
		/// </summary>
		public string PersonalityTraits
        {
            get { return _personalityTraits; }
            set
            {
                if (Set(ref _personalityTraits, value))
                {
                    _characterModel.PersonalityTraits = _personalityTraits;
                }
            }
        }

        /// <summary>
		/// Gets or sets ideals
		/// </summary>
		public string Ideals
        {
            get { return _ideals; }
            set
            {
                if (Set(ref _ideals, value))
                {
                    _characterModel.Ideals = _ideals;
                }
            }
        }

        /// <summary>
		/// Gets or sets bonds
		/// </summary>
		public string Bonds
        {
            get { return _bonds; }
            set
            {
                if (Set(ref _bonds, value))
                {
                    _characterModel.Bonds = _bonds;
                }
            }
        }

        /// <summary>
		/// Gets or sets Flaws
		/// </summary>
		public string Flaws
        {
            get { return _flaws; }
            set
            {
                if (Set(ref _flaws, value))
                {
                    _characterModel.Flaws = _flaws;
                }
            }
        }

        /// <summary>
        /// Gets or sets backstory
        /// </summary>
        public string Backstory
		{
			get { return _backstory; }
			set
			{
				if (Set(ref _backstory, value))
				{
					_characterModel.Backstory = _backstory;
				}
			}
		}

		/// <summary>
		/// Gets or sets levels
		/// </summary>
		public IEnumerable<LevelEditViewModel> Levels
		{
			get { return _levels; }
		}

		/// <summary>
		/// Gets ability roll methods
		/// </summary>
		public IEnumerable<Tuple<AbilityRollMethod, string>> AbilityRollMethods
		{
			get { return _abilityRollMethods; }
		}

		/// <summary>
		/// Gets or sets selected ability roll method
		/// </summary>
		public Tuple<AbilityRollMethod, string> SelectedAbilityRollMethod
		{
			get { return _selectedAbilityRollMethod; }
			set
			{
				if (Set(ref _selectedAbilityRollMethod, value))
				{
					_characterModel.AbilityRollMethod = value.Item1;
					OnPropertyChanged(nameof(AbilityPointsUsed));
					OnPropertyChanged(nameof(RollMethodHasRoll));
					OnPropertyChanged(nameof(RollMethodHasPoints));
                    OnPropertyChanged(nameof(RollMethodManual));
                }
			}
		}

		/// <summary>
		/// Gets times ability scores rolled
		/// </summary>
		public int TimesAbilityScoresRolled
		{
			get { return _timesAbilityScoresRolled; }
			set
			{
				if (Set(ref _timesAbilityScoresRolled, value))
				{
					_characterModel.TimesAbilityScoresRolled = value;
				}
			}
		}

        /// <summary>
		/// Gets roll method has roll
		/// </summary>
		public bool RollMethodHasRoll
        {
            get { return _selectedAbilityRollMethod.Item1 == AbilityRollMethod.Roll_3d6 ||
                         _selectedAbilityRollMethod.Item1 == AbilityRollMethod.Roll_4d6_Drop_Lowest; }
        }

        /// <summary>
        /// Gets roll method has points
        /// </summary>
        public bool RollMethodHasPoints
		{
			get { return _selectedAbilityRollMethod.Item1 == AbilityRollMethod.Point_Buy; }
		}

        /// <summary>
		/// Gets roll method manual
		/// </summary>
		public bool RollMethodManual
        {
            get { return _selectedAbilityRollMethod.Item1 == AbilityRollMethod.Manual; }
        }

        /// <summary>
        /// Gets ability points used
        /// </summary>
        public int AbilityPointsUsed
        {
            get
            {
                return AbilityPointCost(_baseStrength) +
                       AbilityPointCost(_baseDexterity) +
                       AbilityPointCost(_baseConstitution) +
                       AbilityPointCost(_baseIntelligence) +
                       AbilityPointCost(_baseWisdom) +
                       AbilityPointCost(_baseCharisma);
            }
        }

        /// <summary>
        /// Gets strength save
        /// </summary>
        public string StrengthSave
		{
			get { return GetAbilitySaveString(Ability.Strength); }
		}

		/// <summary>
		/// Gets dexterity save
		/// </summary>
		public string DexteritySave
		{
			get { return GetAbilitySaveString(Ability.Dexterity); }
		}

		/// <summary>
		/// Gets constitution save
		/// </summary>
		public string ConstitutionSave
		{
			get { return GetAbilitySaveString(Ability.Constitution); }
		}

		/// <summary>
		/// Gets intelligence save
		/// </summary>
		public string IntelligenceSave
		{
			get { return GetAbilitySaveString(Ability.Intelligence); }
		}

		/// <summary>
		/// Gets wisdom save
		/// </summary>
		public string WisdomSave
		{
			get { return GetAbilitySaveString(Ability.Wisdom); }
		}

		/// <summary>
		/// Gets charisma save
		/// </summary>
		public string CharismaSave
		{
			get { return GetAbilitySaveString(Ability.Charisma); }
		}

        /// <summary>
        /// Gets ability save proficiencies
        /// </summary>
        public IEnumerable<AbilityViewModel> AbilitySaveProficiencies
        {
            get { return _abilityProficiencies; }
        }

        /// <summary>
        /// Gets or sets saving throw notes
        /// </summary>
        public string SavingThrowNotes
        {
            get { return _savingThrowNotes; }
            set
            {
                if (Set(ref _savingThrowNotes, value))
                {
                    _characterModel.SavingThrowNotes = value;
                }
            }
        }

        /// <summary>
        /// Gets skill proficiencies
        /// </summary>
        public IEnumerable<SkillViewModel> SkillProficiencies
        {
            get { return _skillProficiencies; }
        }

        /// <summary>
        /// Gets class trait skill bonus
        /// </summary>
        public string ClassTraitSavingThrows
        {
            get
            {
                string bonus = "None";
                if (_characterModel.Levels.Any())
                {
                    bonus = String.Join(", ", _characterModel.Levels[0].Class.AbilityProficiencies.Select(x=> _stringService.GetString(x)));
                }
                return bonus;
            }
        }

        /// <summary>
        /// Gets background trait skill bonus
        /// </summary>
        public string BackgroundTraitSkillBonus
        {
            get
            {
                string bonus = "None";
                if (_characterModel.Background != null && 
                    _characterModel.Background.Skills.Any())
                {
                    bonus = String.Join(", ", _characterModel.Background.Skills.Select(x => _stringService.GetString(x)));
                }
                return bonus;
            }
        }

        /// <summary>
        /// Gets class trait skill bonus
        /// </summary>
        public string ClassTraitSkillBonus
        {
            get
            {
                string bonus = "None";
                if (_characterModel.Levels.Any())
                {
                    bonus = _characterModel.Levels[0].Class.SkillProficiencies;
                }
                return bonus;
            }
        }

        /// <summary>
        /// Gets race trait skill bonus
        /// </summary>
        public string RaceTraitSkillBonus
        {
            get
            {
                string bonus = "None";
                if (_characterModel.Race != null && 
                    _characterModel.Race.SkillProficiencies.Any())
                {
                    bonus = String.Join(", ", _characterModel.Race.SkillProficiencies.Select(x => _stringService.GetString(x)));
                }
                return bonus;
            }
        }

        /// <summary>
        /// Gets background trait language bonus
        /// </summary>
        public string BackgroundTraitLanguageBonus
        {
            get
            {
                string bonus = "None";
                if (_characterModel.Background != null)
                {
                    if (_characterModel.Background.LanguagesTraitIndex != -1)
                    {
                        TraitModel languageTrait = _characterModel.Background.Traits[_characterModel.Background.LanguagesTraitIndex];
                        bonus = languageTrait.Text;
                    }
                    else if (_characterModel.Background.StartingTraitIndex != -1)
                    {
                        TraitModel startingTrait = _characterModel.Background.Traits[_characterModel.Background.StartingTraitIndex];
                        foreach (string s in startingTrait.TextCollection)
                        {
                            if (s.StartsWith("languages:", StringComparison.OrdinalIgnoreCase))
                            {
                                bonus = new String(s.Skip(10).ToArray()).Trim();
                                break;
                            }
                            else if (s.StartsWith("language:", StringComparison.OrdinalIgnoreCase))
                            {
                                bonus = new String(s.Skip(9).ToArray()).Trim();
                                break;
                            }
                        }
                    }
                }
                return bonus;
            }
        }

        /// <summary>
        /// Gets background trait language bonus
        /// </summary>
        public string RaceTraitLanguageBonus
        {
            get
            {
                string bonus = "None";
                if (_characterModel.Race != null)
                {
                    if (_characterModel.Race.Languages.Any())
                    {
                        bonus = String.Join(", ", _characterModel.Race.Languages);
                    }
                    else
                    {
                        foreach (TraitModel trait in _characterModel.Race.Traits)
                        {
                            if (trait.Name.ToLower() == "language" || trait.Name.ToLower() == "languages")
                            {
                                bonus = trait.Text;
                                break;
                            }
                        }
                    }
                }
                return bonus;
            }
        }

        public IEnumerable<LanguageViewModel> Languages
        {
            get { return _allLanguages; }
        }

        /// <summary>
        /// Gets armors view model
        /// </summary>
        public ArmorProficiencyViewModel ArmorProficiency
        {
            get { return _armorProficiency; }
        }

        /// <summary>
        /// Gets class trait armor bonus
        /// </summary>
        public string ClassTraitArmorBonus
        {
            get
            {
                string bonus = "None";
                if (_characterModel.Levels.Any())
                {
                    bonus = _characterModel.Levels[0].Class.ArmorProficiencies;
                }
                return bonus;
            }
        }

        /// <summary>
        /// Gets weapons view model
        /// </summary>
        public WeaponProficiencyViewModel WeaponProficiency
        {
            get { return _weaponProficiency; }
        }

        /// <summary>
        /// Gets class trait weapon bonus
        /// </summary>
        public string ClassTraitWeaponBonus
        {
            get
            {
                string bonus = "None";
                if (_characterModel.Levels.Any())
                {
                    bonus = _characterModel.Levels[0].Class.WeaponProficiencies;
                }
                return bonus;
            }
        }

        /// <summary>
        /// Gets tools view model
        /// </summary>
        public ToolProficiencyViewModel ToolProficiency
        {
            get { return _toolProficiency; }
        }

        /// <summary>
        /// Gets background trait tool bonus
        /// </summary>
        public string BackgroundTraitToolBonus
        {
            get
            {
                string bonus = "None";
                if (_characterModel.Background != null)
                {
                    if (_characterModel.Background.ToolsTraitIndex != -1)
                    {
                        TraitModel toolTrait = _characterModel.Background.Traits[_characterModel.Background.ToolsTraitIndex];
                        bonus = toolTrait.Text;
                    }
                    else if (_characterModel.Background.StartingTraitIndex != -1)
                    {
                        TraitModel startingTrait = _characterModel.Background.Traits[_characterModel.Background.StartingTraitIndex];
                        foreach (string s in startingTrait.TextCollection)
                        {
                            if (s.StartsWith("tools:", StringComparison.OrdinalIgnoreCase))
                            {
                                bonus = new String(s.Skip(6).ToArray()).Trim();
                                break;
                            }
                            else if (s.StartsWith("tool:", StringComparison.OrdinalIgnoreCase))
                            {
                                bonus = new String(s.Skip(5).ToArray()).Trim();
                                break;
                            }
                        }
                    }
                }
                return bonus;
            }
        }

        /// <summary>
        /// Gets class trait tool bonus
        /// </summary>
        public string ClassTraitToolBonus
        {
            get
            {
                string bonus = "None";
                if (_characterModel.Levels.Any())
                {
                    bonus = _characterModel.Levels[0].Class.ToolProficiencies;
                }
                return bonus;
            }
        }

        /// <summary>
        /// Gets view race command
        /// </summary>
        public ICommand ViewRaceCommand
		{
			get { return _viewRaceCommand; }
		}

		/// <summary>
		/// Gets view background command
		/// </summary>
		public ICommand ViewBackgroundCommand
		{
			get { return _viewBackgroundCommand; }
		}

		/// <summary>
		/// Gets add level command
		/// </summary>
		public ICommand AddLevelCommand
		{
			get { return _addLevelCommand; }
		}

		/// <summary>
		/// Gets delete level command
		/// </summary>
		public ICommand DeleteLevelCommand
		{
			get { return _deleteLevelCommand; }
		}

		/// <summary>
		/// Gets roll ability scores command
		/// </summary>
		public RelayCommand RollAbilityScoresCommand
		{
			get { return _rollAbilityScoresCommand; }
		}

		/// <summary>
		/// Gets move ability up command
		/// </summary>
		public RelayCommand MoveAbilityUpCommand
		{
			get { return _moveAbilityUpCommand; }
		}

		/// <summary>
		/// Gets move ability down command
		/// </summary>
		public RelayCommand MoveAbilityDownCommand
		{
			get { return _moveAbilityDownCommand; }
		}

		/// <summary>
		/// Gets increase ability command
		/// </summary>
		public RelayCommand IncreaseAbilityCommand
		{
			get { return _increaseAbilityCommand; }
		}

		/// <summary>
		/// Gets decrease ability command
		/// </summary>
		public RelayCommand DecreaseAbilityCommand
		{
			get { return _decreaseAbilityCommand; }
		}

		/// <summary>
		/// Gets roll ability save command
		/// </summary>
		public RelayCommand RollAbilitySaveCommand
		{
			get { return _rollAbilitySaveCommand; }
		}

        /// <summary>
		/// Gets roll skill check command
		/// </summary>
		public RelayCommand RollSkillCheckCommand
        {
            get { return _rollSkillCheckCommand; }
        }

        #endregion

        #region Private Methods

        private void InitializeNewCharacter()
        {
            _characterModel.Background = _compendium.Backgrounds.FirstOrDefault();
            _characterModel.Race = _compendium.Races.FirstOrDefault();

            LevelModel levelModel = new LevelModel();
            levelModel.Level = 1;
            levelModel.Class = _compendium.Classes.FirstOrDefault();
            levelModel.HitDieResult = levelModel.Class != null ? levelModel.Class.HitDie : 0;
            _characterModel.Levels.Add(levelModel);

            _baseStrength = _characterModel.BaseStrength;
            _baseDexterity = _characterModel.BaseDexterity;
            _baseConstitution = _characterModel.BaseConstitution;
            _baseIntelligence = _characterModel.BaseIntelligence;
            _baseWisdom = _characterModel.BaseWisdom;
            _baseCharisma = _characterModel.BaseCharisma;

            foreach (Tuple<string, Ability> ability in _statService.Abilities)
            {
                bool proficient = false;

                if (_characterModel.Levels.Any() && _characterModel.Levels[0].Class != null)
                {
                    proficient = _characterModel.Levels[0].Class.AbilityProficiencies.Contains(ability.Item2);
                }

                _characterModel.AbilitySaveProficiencies.Add(new AbilityModel(ability.Item2, GetAbilitySaveBonus(ability.Item2, proficient), GetAbilitySaveBonus(ability.Item2, false), proficient));
            }

            foreach (Tuple<string, Skill> skill in _statService.Skills)
            {
                bool proficient = _characterModel.Background != null && _characterModel.Background.Skills.Contains(skill.Item2);
                _characterModel.SkillProficiencies.Add(new SkillModel(skill.Item2, GetSkillBonus(skill.Item2, proficient, false), proficient, false));
            }
        }

        private void ViewRace()
		{
			RaceModel raceModel = _characterModel.Race;
			if (raceModel != null)
			{
				_dialogService.ShowDetailsDialog(new RaceViewModel(raceModel));
			}
		}

		private void ViewBackground()
		{
			BackgroundModel backgroundModel = _characterModel.Background;
			if (backgroundModel != null)
			{
				_dialogService.ShowDetailsDialog(new BackgroundViewModel(backgroundModel));
			}
		}

		private void LevelView_ClassChanged(object sender, EventArgs e)
		{
			SetLevelOfClasses();

            LevelEditViewModel levelEditView = sender as LevelEditViewModel;
            if (levelEditView.Level == 1)
            {
                if (_isNew)
                {
                    foreach (AbilityViewModel abilityView in _abilityProficiencies)
                    {
                        abilityView.Proficient = levelEditView.LevelModel.Class.AbilityProficiencies.Contains(abilityView.Ability);
                    }

                    UpdateArmorProficiencies();
                    UpdateWeaponProficiencies();
                }
                OnPropertyChanged(nameof(ClassTraitSkillBonus));
                OnPropertyChanged(nameof(ClassTraitArmorBonus));
                OnPropertyChanged(nameof(ClassTraitWeaponBonus));
                OnPropertyChanged(nameof(ClassTraitToolBonus));
            }
        }

		public void SetLevelOfClasses()
		{
			Dictionary<Guid, int> _classLevels = new Dictionary<Guid, int>();

			foreach (LevelEditViewModel levelView in _levels)
			{
				if (_classLevels.ContainsKey(levelView.Class.Item1))
				{
					_classLevels[levelView.Class.Item1] = _classLevels[levelView.Class.Item1] + 1;
				}
				else
				{
					_classLevels[levelView.Class.Item1] = 1;
				}

				levelView.LevelOfClass = _classLevels[levelView.Class.Item1];
			}
		}

		private void AddLevel()
		{
			LevelModel levelModel = new LevelModel();
			levelModel.Level = _levels.Count + 1;
            if (_characterModel.Levels.Any())
            {
                levelModel.Class = _characterModel.Levels.Last().Class;
            }
            else
            {
                levelModel.Class = _compendium.Classes.FirstOrDefault();
            }
			levelModel.HitDieResult = levelModel.Class != null ? levelModel.Class.HitDie / 2 + 1 : 0;

			LevelEditViewModel levelEditView = new LevelEditViewModel(levelModel);
			levelEditView.PropertyChanged += LevelEditView_PropertyChanged;
			levelEditView.ClassChanged += LevelView_ClassChanged;
			_levels.Add(levelEditView);

			_characterModel.Levels.Add(levelModel);

			SetLevelOfClasses();

			for (int i = 0; i < _levels.Count; ++i)
			{
				_levels[i].CanDelete = i > 0 && i + 1 == _levels.Count;
            }

            UpdateSkillBonusValues();

            OnPropertyChanged(nameof(StrengthSave));
			OnPropertyChanged(nameof(DexteritySave));
			OnPropertyChanged(nameof(ConstitutionSave));
			OnPropertyChanged(nameof(IntelligenceSave));
			OnPropertyChanged(nameof(WisdomSave));
			OnPropertyChanged(nameof(CharismaSave));
		}

		private void DeleteLevel(LevelEditViewModel levelEditView)
		{
			_levels.Remove(levelEditView);
			_characterModel.Levels.Remove(levelEditView.LevelModel);

			for (int i = 0; i < _levels.Count; ++i)
			{
				_levels[i].CanDelete = i > 0 && i + 1 == _levels.Count;
            }

            UpdateSkillBonusValues();

            OnPropertyChanged(nameof(StrengthSave));
			OnPropertyChanged(nameof(DexteritySave));
			OnPropertyChanged(nameof(ConstitutionSave));
			OnPropertyChanged(nameof(IntelligenceSave));
			OnPropertyChanged(nameof(WisdomSave));
			OnPropertyChanged(nameof(CharismaSave));
		}

		private void LevelEditView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateAbilityBonusValues();
            UpdateSkillBonusValues();

            OnPropertyChanged(String.Empty);
        }

        private void UpdateAbilityBonusValues()
        {
            foreach (AbilityViewModel abilityView in _abilityProficiencies)
            {
                abilityView.CheckBonus = GetAbilitySaveBonus(abilityView.Ability, false);
                abilityView.SaveBonus = GetAbilitySaveBonus(abilityView.Ability, abilityView.Proficient);
            }
        }

        private void UpdateSkillBonusValues()
        {
            foreach (SkillViewModel skillView in _skillProficiencies)
            {
                skillView.Bonus = GetSkillBonus(skillView.Skill, skillView.Proficient, skillView.Expertise);
            }
        }

        private void UpdateSkillProficiencies()
        {
            foreach (SkillViewModel skillView in _skillProficiencies)
            {
                bool proficient = false;

                if (_characterModel.Background != null && _characterModel.Background.Skills.Contains(skillView.Skill))
                {
                    proficient = true;
                }

                if (_characterModel.Race != null && _characterModel.Race.SkillProficiencies.Contains(skillView.Skill))
                {
                    proficient = true;
                }

                skillView.Proficient = proficient;
            }
        }

        private void UpdateLanguageProficiencies()
        {
            string raceLanguages = RaceTraitLanguageBonus.ToLower();
            string backgroundLanguages = BackgroundTraitLanguageBonus.ToLower();

            foreach (LanguageViewModel languageView in _allLanguages)
            {
                bool proficient = false;

                if (!raceLanguages.Contains("choose") && raceLanguages.Contains(languageView.Name.ToLower()))
                {
                    proficient = true;
                }

                if (!backgroundLanguages.Contains("choose") && backgroundLanguages.Contains(languageView.Name.ToLower()))
                {
                    proficient = true;
                }

                languageView.Proficient = proficient;
            }
        }

        private void UpdateArmorProficiencies()
        {
            string classArmorBonus = ClassTraitArmorBonus.ToLower();
            _armorProficiency.LightChecked = classArmorBonus.Contains("light");
            _armorProficiency.MediumChecked = classArmorBonus.Contains("medium");
            _armorProficiency.HeavyChecked = classArmorBonus.Contains("heavy");
            _armorProficiency.ShieldsChecked = classArmorBonus.Contains("shield");
        }

        private void UpdateWeaponProficiencies()
        {
            string classWeaponBonus = ClassTraitWeaponBonus.ToLower();
            _weaponProficiency.SimpleWeaponsChecked = classWeaponBonus.Contains("simple");
            _weaponProficiency.MartialWeaponsChecked = classWeaponBonus.Contains("martial");
        }

        private int GetRaceAbilityBonus(Ability ability)
		{
			int bonus = 0;

			if (_characterModel.Race != null)
			{
				if (_characterModel.Race.Abilities.ContainsKey(ability))
				{
					bonus = _characterModel.Race.Abilities[ability];
				}
			}

			return bonus;
		}

		private int GetLevelAbilityBonus(Ability ability)
		{
			int bonus = 0;

			foreach (LevelModel levelModel in _characterModel.Levels)
			{
				foreach (KeyValuePair<Ability, int> abilityBonus in levelModel.AbilityScoreImprovements.Where(x => x.Key == ability))
				{
					bonus += abilityBonus.Value;
				}
			}

			return bonus;
		}

		private void RollAbilityScores()
		{
			if (_selectedAbilityRollMethod.Item1 == AbilityRollMethod.Roll_3d6)
			{
				BaseStrength = (int)_diceService.EvaluateExpression("3d6").Item1;
				BaseDexterity = (int)_diceService.EvaluateExpression("3d6").Item1;
				BaseConstitution = (int)_diceService.EvaluateExpression("3d6").Item1;
				BaseIntelligence = (int)_diceService.EvaluateExpression("3d6").Item1;
				BaseWisdom = (int)_diceService.EvaluateExpression("3d6").Item1;
				BaseCharisma = (int)_diceService.EvaluateExpression("3d6").Item1;
			}
			else if (_selectedAbilityRollMethod.Item1 == AbilityRollMethod.Roll_4d6_Drop_Lowest)
			{
				List<int> rolls = new List<int>();

				for (int i = 0; i < 4; ++i)
				{
					rolls.Add(_diceService.RandomNumber(1, 6));
				}
				rolls.Sort();
				BaseStrength = rolls.Skip(1).Sum();

				rolls.Clear();
				for (int i = 0; i < 4; ++i)
				{
					rolls.Add(_diceService.RandomNumber(1, 6));
				}
				rolls.Sort();
				BaseDexterity = rolls.Skip(1).Sum();

				rolls.Clear();
				for (int i = 0; i < 4; ++i)
				{
					rolls.Add(_diceService.RandomNumber(1, 6));
				}
				rolls.Sort();
				BaseConstitution = rolls.Skip(1).Sum();

				rolls.Clear();
				for (int i = 0; i < 4; ++i)
				{
					rolls.Add(_diceService.RandomNumber(1, 6));
				}
				rolls.Sort();
				BaseIntelligence = rolls.Skip(1).Sum();

				rolls.Clear();
				for (int i = 0; i < 4; ++i)
				{
					rolls.Add(_diceService.RandomNumber(1, 6));
				}
				rolls.Sort();
				BaseWisdom = rolls.Skip(1).Sum();

				rolls.Clear();
				for (int i = 0; i < 4; ++i)
				{
					rolls.Add(_diceService.RandomNumber(1, 6));
				}
				rolls.Sort();
				BaseCharisma = rolls.Skip(1).Sum();
			}

			TimesAbilityScoresRolled++;

            UpdateAbilityBonusValues();
            UpdateSkillBonusValues();
		}

		private void MoveAbilityUp(Ability ability)
		{
			int temp;

			switch (ability)
			{
				case Ability.Strength:
					temp = BaseCharisma;
					BaseCharisma = BaseStrength;
					BaseStrength = temp;
					break;

				case Ability.Dexterity:
					temp = BaseStrength;
					BaseStrength = BaseDexterity;
					BaseDexterity = temp;
					break;

				case Ability.Constitution:
					temp = BaseDexterity;
					BaseDexterity = BaseConstitution;
					BaseConstitution = temp;
					break;

				case Ability.Intelligence:
					temp = BaseConstitution;
					BaseConstitution = BaseIntelligence;
					BaseIntelligence = temp;
					break;

				case Ability.Wisdom:
					temp = BaseIntelligence;
					BaseIntelligence = BaseWisdom;
					BaseWisdom = temp;
					break;

				case Ability.Charisma:
					temp = BaseWisdom;
					BaseWisdom = BaseCharisma;
					BaseCharisma = temp;
					break;
			}

            UpdateAbilityBonusValues();
            UpdateSkillBonusValues();
		}

		private void MoveAbilityDown(Ability ability)
		{
			int temp;

			switch (ability)
			{
				case Ability.Strength:
					temp = BaseDexterity;
					BaseDexterity = BaseStrength;
					BaseStrength = temp;
					break;

				case Ability.Dexterity:
					temp = BaseConstitution;
					BaseConstitution = BaseDexterity;
					BaseDexterity = temp;
					break;

				case Ability.Constitution:
					temp = BaseIntelligence;
					BaseIntelligence = BaseConstitution;
					BaseConstitution = temp;
					break;

				case Ability.Intelligence:
					temp = BaseWisdom;
					BaseWisdom = BaseIntelligence;
					BaseIntelligence = temp;
					break;

				case Ability.Wisdom:
					temp = BaseCharisma;
					BaseCharisma = BaseWisdom;
					BaseWisdom = temp;
					break;

				case Ability.Charisma:
					temp = BaseStrength;
					BaseStrength = BaseCharisma;
					BaseCharisma = temp;
					break;
			}

            UpdateAbilityBonusValues();
            UpdateSkillBonusValues();
        }

		private void IncreaseAbility(Ability ability)
		{
			switch (ability)
			{
				case Ability.Strength:
					BaseStrength = Math.Min(_baseStrength + 1, 20);
					break;

				case Ability.Dexterity:
					BaseDexterity = Math.Min(_baseDexterity + 1, 20);
					break;

				case Ability.Constitution:
					BaseConstitution = Math.Min(_baseConstitution + 1, 20);
					break;

				case Ability.Intelligence:
					BaseIntelligence = Math.Min(_baseIntelligence + 1, 20);
					break;

				case Ability.Wisdom:
					BaseWisdom = Math.Min(_baseWisdom + 1, 20);
					break;

				case Ability.Charisma:
					BaseCharisma = Math.Min(_baseCharisma + 1, 20);
					break;
			}

            UpdateAbilityBonusValues();
            UpdateSkillBonusValues();

            OnPropertyChanged(nameof(AbilityPointsUsed));
		}

		private void DecreaseAbility(Ability ability)
		{
			switch (ability)
			{
				case Ability.Strength:
					BaseStrength = Math.Max(_baseStrength - 1, 0);
					break;

				case Ability.Dexterity:
					BaseDexterity = Math.Max(_baseDexterity - 1, 0);
					break;

				case Ability.Constitution:
					BaseConstitution = Math.Max(_baseConstitution - 1, 0);
					break;

				case Ability.Intelligence:
					BaseIntelligence = Math.Max(_baseIntelligence - 1, 0);
					break;

				case Ability.Wisdom:
					BaseWisdom = Math.Max(_baseWisdom - 1, 0);
					break;

				case Ability.Charisma:
					BaseCharisma = Math.Max(_baseCharisma - 1, 0);
					break;
			}

            UpdateAbilityBonusValues();
            UpdateSkillBonusValues();

            OnPropertyChanged(nameof(AbilityPointsUsed));
		}

		private int AbilityPointCost(int value)
		{
			int cost = 0;

			if (value > 8)
			{
				if (value < 14)
				{
					cost = value - 8;
				}
				else
				{
					cost = (value - 8) + (value - 13);
				}
			}

			return cost;
		}

		private string GetAbilitySaveString(Ability ability)
		{
			string save = "+0";

			int abilityScore = 0;
			int proficiencyBonus = (7 + _characterModel.Levels.Count) / 4;

			switch (ability)
			{
				case Ability.Strength:
					abilityScore = TotalStrength;
					break;

				case Ability.Dexterity:
					abilityScore = TotalDexterity;
					break;

				case Ability.Constitution:
					abilityScore = TotalConstitution;
					break;

				case Ability.Intelligence:
					abilityScore = TotalIntelligence;
					break;

				case Ability.Wisdom:
					abilityScore = TotalWisdom;
					break;

				case Ability.Charisma:
					abilityScore = TotalCharisma;
					break;
			}

			if (_characterModel.AbilitySaveProficiencies.Any(x => x.Ability == ability && x.Proficient))
			{
				save = _statService.GetStatBonusString(abilityScore + proficiencyBonus);
			}
			else
			{
				save = _statService.GetStatBonusString(abilityScore);
			}

			return save;
		}

        private int GetAbilitySave(Ability ability)
        {
            int save = 0;

            int abilityScore = 0;
            int proficiencyBonus = (7 + _characterModel.Levels.Count) / 4;

            switch (ability)
            {
                case Ability.Strength:
                    abilityScore = TotalStrength;
                    break;

                case Ability.Dexterity:
                    abilityScore = TotalDexterity;
                    break;

                case Ability.Constitution:
                    abilityScore = TotalConstitution;
                    break;

                case Ability.Intelligence:
                    abilityScore = TotalIntelligence;
                    break;

                case Ability.Wisdom:
                    abilityScore = TotalWisdom;
                    break;

                case Ability.Charisma:
                    abilityScore = TotalCharisma;
                    break;
            }

            if (_characterModel.AbilitySaveProficiencies.Any(x => x.Ability == ability && x.Proficient))
            {
                save = _statService.GetStatBonus(abilityScore + proficiencyBonus);
            }
            else
            {
                save = _statService.GetStatBonus(abilityScore);
            }

            return save;
        }

        private int GetAbilitySaveBonus(Ability ability, bool proficient)
        {
            int bonus = 0;

            int abilityScore = 0;
            int proficiencyBonus = (7 + _characterModel.Levels.Count) / 4;

            switch (ability)
            {
                case Ability.Strength:
                    abilityScore = TotalStrength;
                    break;

                case Ability.Dexterity:
                    abilityScore = TotalDexterity;
                    break;

                case Ability.Constitution:
                    abilityScore = TotalConstitution;
                    break;

                case Ability.Intelligence:
                    abilityScore = TotalIntelligence;
                    break;

                case Ability.Wisdom:
                    abilityScore = TotalWisdom;
                    break;

                case Ability.Charisma:
                    abilityScore = TotalCharisma;
                    break;
            }

            if (proficient)
            {
                bonus = _statService.GetStatBonus(abilityScore) + proficiencyBonus;
            }
            else
            {
                bonus = _statService.GetStatBonus(abilityScore);
            }

            return bonus;
        }

        private int GetSkillBonus(Skill skill, bool proficient, bool expertise)
        {
            int bonus = 0;

            int abilityScore = 0;
            int proficiencyBonus = (7 + _characterModel.Levels.Count) / 4;

            switch (skill)
            {
                case Skill.Athletics:
                    abilityScore = TotalStrength;
                    break;

                case Skill.Acrobatics:
                case Skill.Sleight_of_Hand:
                case Skill.Stealth:
                    abilityScore = TotalDexterity;
                    break;

                case Skill.Arcana:
                case Skill.History:
                case Skill.Investigation:
                case Skill.Nature:
                case Skill.Religion:
                    abilityScore = TotalIntelligence;
                    break;

                case Skill.Animal_Handling:
                case Skill.Insight:
                case Skill.Medicine:
                case Skill.Perception:
                case Skill.Survival:
                    abilityScore = TotalWisdom;
                    break;

                case Skill.Deception:
                case Skill.Intimidation:
                case Skill.Performance:
                case Skill.Persuasion:
                    abilityScore = TotalCharisma;
                    break;
            }

            bonus = _statService.GetStatBonus(abilityScore);

            if (proficient)
            {
                bonus += proficiencyBonus;
            }

            if (expertise)
            {
                bonus += proficiencyBonus;
            }

            return bonus;
        }

        private void AbilityView_ProficiencyChanged(object sender, EventArgs e)
        {
            AbilityViewModel abilityView = sender as AbilityViewModel;
            abilityView.SaveBonus = GetAbilitySaveBonus(abilityView.Ability, abilityView.Proficient);

            OnPropertyChanged(String.Empty);
        }

        private void SkillView_ProficiencyChanged(object sender, EventArgs e)
        {
            SkillViewModel skillView = sender as SkillViewModel;
            skillView.Bonus = GetSkillBonus(skillView.Skill, skillView.Proficient, skillView.Expertise);

            OnPropertyChanged(String.Empty);
        }

        private void RollAbilitySave(Ability ability)
		{
			_dialogService.ShowDiceRollDialog(_stringService.GetString(ability) + " Save", "1d20" + GetAbilitySaveString(ability));
        }

        private void RollSkillCheck(Skill skill)
        {
            _dialogService.ShowDiceRollDialog(_stringService.GetString(skill) + " Check", "1d20" + GetSkillCheckString(skill));
        }

        private string GetSkillCheckString(Skill skill)
        {
            return "+0";
        }

        private void LanguageView_ProficiencyChanged(object sender, EventArgs e)
        {
            LanguageViewModel languageView = sender as LanguageViewModel;

            if (languageView != null)
            {
                if (languageView.Proficient)
                {
                    _languageProficiencies.Add(new LanguageViewModel(languageView.Model));
                    _characterModel.Languages.Add(languageView.Model);
                }
                else
                {
                    _languageProficiencies.RemoveAll(x => x.ID == languageView.ID);
                    _characterModel.Languages.RemoveAll(x => x.ID == languageView.ID);
                }
                
                OnPropertyChanged(String.Empty);
            }
        }

        private void _armors_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(String.Empty);
        }

        private void _weaponProficiency_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(String.Empty);
        }

        private void _toolProficiency_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(String.Empty);
        }

        #endregion
    }
}
