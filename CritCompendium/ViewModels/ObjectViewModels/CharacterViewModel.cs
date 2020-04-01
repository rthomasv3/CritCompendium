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
   public sealed class CharacterViewModel : NotifyPropertyChanged
   {
      #region Events

      public event EventHandler LevelUp;

      #endregion

      #region Fields

      private readonly CharacterModel _characterModel;
      private readonly Compendium _compendium = DependencyResolver.Resolve<Compendium>();
      private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
      private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();
      private readonly DiceService _diceService = DependencyResolver.Resolve<DiceService>();
      private readonly StatService _statService = DependencyResolver.Resolve<StatService>();

      private string _name;
      private int _maxHP;
      private int _currentHP;
      private int _tempHP;
      private ObservableCollection<KeyValuePair<ClassViewModel, int>> _classes = new ObservableCollection<KeyValuePair<ClassViewModel, int>>();
      private Dictionary<KeyValuePair<Guid, string>, int> _classesMap = new Dictionary<KeyValuePair<Guid, string>, int>();
      private string _classDisplay;
      private RaceViewModel _race;
      private BackgroundViewModel _background;
      private Alignment _alignment;
      private int _ac;
      private int _initiativeBonus;
      private MovementViewModel _movementViewModel;
      private int _proficiencyBonus;
      private bool _inspiration;
      private int _passivePerception;
      private int _passiveInvestigation;
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
      private int _totalStrength;
      private int _totalDexterity;
      private int _totalConstitution;
      private int _totalIntelligence;
      private int _totalWisdom;
      private int _totalCharisma;
      private int _carryingCapacity;
      private string _personalityTraits;
      private string _ideals;
      private string _bonds;
      private string _flaws;
      private string _backstory;
      private string _armorProficiencies;
      private string _weaponProficiencies;
      private string _toolProficiencies;
      private string _languages;
      private List<AbilityViewModel> _abilityProficiencies = new List<AbilityViewModel>();
      private string _savingThrowNotes;
      private List<SkillViewModel> _skillProficiencies = new List<SkillViewModel>();
      private List<LevelViewModel> _levels = new List<LevelViewModel>();
      private ObservableCollection<AppliedConditionViewModel> _conditions = new ObservableCollection<AppliedConditionViewModel>();
      private ObservableCollection<FeatViewModel> _feats = new ObservableCollection<FeatViewModel>();
      private ObservableCollection<FeatureViewModel> _features = new ObservableCollection<FeatureViewModel>();
      private ObservableCollection<AttackViewModel> _attacks = new ObservableCollection<AttackViewModel>();
      private ObservableCollection<CounterViewModel> _counters = new ObservableCollection<CounterViewModel>();
      private ObservableCollection<CompanionViewModel> _companions = new ObservableCollection<CompanionViewModel>();
      private ObservableCollection<BagViewModel> _bags = new ObservableCollection<BagViewModel>();
      private float _totalWeight;
      private int _totalAttunedItems;
      private int _totalCopper;
      private int _totalSilver;
      private int _totalElectrum;
      private int _totalGold;
      private int _totalPlatinum;
      private ObservableCollection<SpellbookViewModel> _spellbooks = new ObservableCollection<SpellbookViewModel>();
      private ObservableCollection<StatModificationViewModel> _statModifications = new ObservableCollection<StatModificationViewModel>();
      private ArmorClassViewModel _armorClassViewModel;

      private readonly ICommand _viewClassCommand;
      private readonly ICommand _viewBackgroundCommand;
      private readonly ICommand _viewRaceCommand;
      private readonly ICommand _showHPDialogCommand;
      private readonly ICommand _showExperienceDialogCommand;
      private readonly ICommand _rollAbilityCheckCommand;
      private readonly ICommand _rollAbilitySaveCommand;
      private readonly ICommand _rollSkillCheckCommand;
      private readonly ICommand _rollPassivePerceptionCommand;
      private readonly ICommand _rollPassiveInvestigationCommand;
      private readonly ICommand _showACDialogCommand;
      private readonly ICommand _rollInitiativeCommand;
      private readonly ICommand _showMovementDialogCommand;
      private readonly ICommand _rollProficiencyCommand;
      private readonly ICommand _showTempHPDialogCommand;
      private readonly ICommand _showLongRestDialogCommand;
      private readonly ICommand _showShortRestDialogCommand;
      private readonly ICommand _showAddAttackDialogCommand;
      private readonly ICommand _rollAttackToHitCommand;
      private readonly ICommand _rollAttackDamageCommand;
      private readonly ICommand _showEditAttackDialogCommand;
      private readonly ICommand _deleteAttackCommand;
      private readonly ICommand _showAddCounterDialogCommand;
      private readonly ICommand _showEditCounterDialogCommand;
      private readonly ICommand _deleteCounterCommand;
      private readonly ICommand _showAddConditionDialogCommand;
      private readonly ICommand _showEditConditionDialogCommand;
      private readonly ICommand _removeConditionCommand;
      private readonly ICommand _showAddCompanionDialogCommand;
      private readonly ICommand _showEditCompanionDialogCommand;
      private readonly ICommand _deleteCompanionCommand;
      private readonly ICommand _showAddBagDialogCommand;
      private readonly ICommand _showEditBagDialogCommand;
      private readonly ICommand _deleteBagCommand;
      private readonly ICommand _showAddSpellbookDialogCommand;
      private readonly ICommand _showEditSpellbookDialogCommand;
      private readonly ICommand _deleteSpellbookCommand;
      private readonly ICommand _showAddStatModificationDialogCommand;
      private readonly ICommand _showEditStatModificationDialogCommand;
      private readonly ICommand _deleteStatModificationCommand;
      private readonly ICommand _viewFeatCommand;
      private readonly ICommand _viewFeatureCommand;
      private readonly ICommand _showConvertMoneyDialogCommand;
      private readonly ICommand _showLevelUpDialogCommand;
      private readonly ICommand _showDeathSavesDialogCommand;
      private readonly ICommand _toggleInspirationCommand;

      private bool _statsTabSelected = true;
      private bool _combatTabSelected;
      private bool _inventoryAndEquipmentTabSelected;
      private bool _spellcastingTabSelected;
      private bool _informationTabSelected;
      private readonly ICommand _selectStatsTabCommand;
      private readonly ICommand _selectCombatTabCommand;
      private readonly ICommand _selectInventoryAndEquipmentTabCommand;
      private readonly ICommand _selectSpellcastingTabCommand;
      private readonly ICommand _selectInformationTabCommand;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of <see cref="CharacterViewModel"/>
      /// </summary>
      public CharacterViewModel(CharacterModel characterModel)
      {
         _characterModel = characterModel;

         _name = String.IsNullOrWhiteSpace(_characterModel.Name) ? "Unknown Name" : _characterModel.Name;

         List<ClassViewModel> classes = new List<ClassViewModel>();
         if (_characterModel.Levels.Any())
         {
            foreach (LevelModel level in _characterModel.Levels)
            {
               _levels.Add(new LevelViewModel(level));

               KeyValuePair<Guid, string> pair = new KeyValuePair<Guid, string>(level.Class.ID, level.Class.Name);

               if (_classesMap.ContainsKey(pair))
               {
                  _classesMap[pair]++;
               }
               else
               {
                  _classesMap[pair] = 1;
               }

               if (!classes.Any(x => x.ClassModel.ID == level.Class.ID))
               {
                  classes.Add(new ClassViewModel(level.Class));
               }
            }

            foreach (ClassViewModel classView in classes)
            {
               KeyValuePair<KeyValuePair<Guid, string>, int> pair = _classesMap.FirstOrDefault(x => x.Key.Key == classView.ClassModel.ID);
               if (!pair.Equals(default(KeyValuePair<KeyValuePair<Guid, string>, int>)))
               {
                  _classes.Add(new KeyValuePair<ClassViewModel, int>(classView, pair.Value));
               }
            }

            _classDisplay = String.Join(", ", _classesMap.Select(x => $"{x.Key.Value} {x.Value}"));
         }
         else
         {
            _classDisplay = "Unknown";
         }

         if (_characterModel.Race != null)
         {
            _race = new RaceViewModel(_characterModel.Race);
         }

         if (_characterModel.Background != null)
         {
            _background = new BackgroundViewModel(_characterModel.Background);
         }

         _alignment = _characterModel.Alignment;
         _armorClassViewModel = new ArmorClassViewModel(_characterModel.ArmorClassModel);
         _deity = String.IsNullOrWhiteSpace(_characterModel.Deity) ? "Unknown" : _characterModel.Deity;
         _gender = String.IsNullOrWhiteSpace(_characterModel.Gender) ? "Unknown" : _characterModel.Gender;
         _age = _characterModel.Age;
         _heightFeet = _characterModel.HeightFeet;
         _heightInches = _characterModel.HeightInches;
         _weight = _characterModel.Weight;
         _hair = String.IsNullOrWhiteSpace(_characterModel.Hair) ? "Unknown" : _characterModel.Hair;
         _eyes = String.IsNullOrWhiteSpace(_characterModel.Eyes) ? "Unknown" : _characterModel.Eyes;
         _skin = String.IsNullOrWhiteSpace(_characterModel.Skin) ? "Unknown" : _characterModel.Skin;
         _experience = _characterModel.Experience;
         _totalStrength = _characterModel.BaseStrength + _characterModel.BonusStrength + GetRaceAbilityBonus(Ability.Strength) + GetLevelAbilityBonus(Ability.Strength);
         _totalDexterity = _characterModel.BaseDexterity + _characterModel.BonusDexterity + GetRaceAbilityBonus(Ability.Dexterity) + GetLevelAbilityBonus(Ability.Dexterity);
         _totalConstitution = _characterModel.BaseConstitution + _characterModel.BonusConstitution + GetRaceAbilityBonus(Ability.Constitution) + GetLevelAbilityBonus(Ability.Constitution);
         _totalIntelligence = _characterModel.BaseIntelligence + _characterModel.BonusIntelligence + GetRaceAbilityBonus(Ability.Intelligence) + GetLevelAbilityBonus(Ability.Intelligence);
         _totalWisdom = _characterModel.BaseWisdom + _characterModel.BonusWisdom + GetRaceAbilityBonus(Ability.Wisdom) + GetLevelAbilityBonus(Ability.Wisdom);
         _totalCharisma = _characterModel.BaseCharisma + _characterModel.BonusCharisma + GetRaceAbilityBonus(Ability.Charisma) + GetLevelAbilityBonus(Ability.Charisma);

         _carryingCapacity = _totalStrength * 15;

         _initiativeBonus = _statService.GetStatBonus(_totalDexterity);
         _movementViewModel = new MovementViewModel(_characterModel.MovementModel);
         _proficiencyBonus = (7 + _characterModel.Levels.Count) / 4;
         _inspiration = _characterModel.Inspiration;

         _maxHP = 0;
         int conBonus = _statService.GetStatBonus(_totalConstitution);
         foreach (LevelModel level in _characterModel.Levels)
         {
            _maxHP += level.HitDieResult + conBonus + level.AdditionalHP;
         }

         _characterModel.CurrentHP = Math.Min(_characterModel.CurrentHP, _maxHP);

         _currentHP = _characterModel.CurrentHP;
         _tempHP = _characterModel.TempHP;

         _personalityTraits = String.IsNullOrWhiteSpace(_characterModel.PersonalityTraits) ? "Unknown" : _characterModel.PersonalityTraits;
         _ideals = String.IsNullOrWhiteSpace(_characterModel.Ideals) ? "Unknown" : _characterModel.Ideals;
         _bonds = String.IsNullOrWhiteSpace(_characterModel.Bonds) ? "Unknown" : _characterModel.Bonds;
         _flaws = String.IsNullOrWhiteSpace(_characterModel.Flaws) ? "Unknown" : _characterModel.Flaws;
         _backstory = String.IsNullOrWhiteSpace(_characterModel.Backstory) ? "Unknown" : _characterModel.Backstory;

         _armorProficiencies = _stringService.GetArmorProficiencyString(_characterModel.ArmorProficiency);
         _weaponProficiencies = _stringService.GetWeaponProficiencyString(_characterModel.WeaponProficiency);
         _toolProficiencies = _stringService.GetToolProficiencyString(_characterModel.ToolProficiency);

         _languages = String.Join(", ", _characterModel.Languages.Select(x => x.Name));

         foreach (AbilityModel ability in _characterModel.AbilitySaveProficiencies)
         {
            _abilityProficiencies.Add(new AbilityViewModel(ability));
         }

         _savingThrowNotes = _characterModel.SavingThrowNotes;

         foreach (SkillModel skill in _characterModel.SkillProficiencies)
         {
            _skillProficiencies.Add(new SkillViewModel(skill));
         }

         SkillViewModel perceptionSkill = _skillProficiencies.FirstOrDefault(x => x.Skill == Skill.Perception);
         if (perceptionSkill != null)
         {
            _passivePerception = 10 + perceptionSkill.Bonus;
         }

         SkillViewModel investigationSkill = _skillProficiencies.FirstOrDefault(x => x.Skill == Skill.Investigation);
         if (investigationSkill != null)
         {
            _passiveInvestigation = 10 + investigationSkill.Bonus;
         }

         foreach (AppliedConditionModel condition in _characterModel.Conditions.OrderBy(x => x.Name))
         {
            _conditions.Add(new AppliedConditionViewModel(condition));
         }

         foreach (LevelModel levelModel in _characterModel.Levels)
         {
            foreach (FeatureModel featureModel in levelModel.Features)
            {
               _features.Add(new FeatureViewModel(featureModel));
            }
            foreach (FeatModel featModel in levelModel.Feats.OrderBy(x => x.Name))
            {
               _feats.Add(new FeatViewModel(featModel));
            }
         }

         foreach (AttackModel attackModel in _characterModel.Attacks.OrderBy(x => x.Name))
         {
            _attacks.Add(CreateAttackViewModel(attackModel));
         }

         foreach (CounterModel counterModel in _characterModel.Counters.OrderBy(x => x.Name))
         {
            _counters.Add(new CounterViewModel(counterModel));
         }

         foreach (CompanionModel companionModel in _characterModel.Companions.OrderBy(x => x.Name))
         {
            _companions.Add(new CompanionViewModel(companionModel));
         }

         foreach (BagModel bagModel in _characterModel.Bags.OrderBy(x => x.Name))
         {
            BagViewModel bagViewModel = new BagViewModel(bagModel);
            bagViewModel.PropertyChanged += BagViewModel_PropertyChanged;
            bagViewModel.EquippedChanged += BagViewModel_EquippedChanged;
            _bags.Add(bagViewModel);
         }

         UpdateBagTotals();

         CalculateArmorClass();

         foreach (SpellbookModel spellbookModel in _characterModel.Spellbooks.OrderBy(x => x.Name))
         {
            _spellbooks.Add(CreateSpellbookViewModel(spellbookModel));
         }

         foreach (StatModificationModel statModification in _characterModel.StatModifications.OrderBy(x => x.ModificationOption))
         {
            _statModifications.Add(new StatModificationViewModel(statModification));
         }

         ApplyStatModifications();

         _viewClassCommand = new RelayCommand(obj => true, obj => ViewClass((ClassViewModel)obj));
         _viewBackgroundCommand = new RelayCommand(obj => true, obj => ViewBackground());
         _viewRaceCommand = new RelayCommand(obj => true, obj => ViewRace());
         _showHPDialogCommand = new RelayCommand(obj => true, obj => ShowHPDialog());
         _showExperienceDialogCommand = new RelayCommand(obj => true, obj => ShowExperienceDialog());
         _rollAbilityCheckCommand = new RelayCommand(obj => true, obj => RollAbilityCheck((Ability)obj));
         _rollAbilitySaveCommand = new RelayCommand(obj => true, obj => RollAbilitySave((AbilityViewModel)obj));
         _rollSkillCheckCommand = new RelayCommand(obj => true, obj => RollSkillCheck((SkillViewModel)obj));
         _rollPassivePerceptionCommand = new RelayCommand(obj => true, obj => RollPassivePerception());
         _rollPassiveInvestigationCommand = new RelayCommand(obj => true, obj => RollPassiveInvestigation());
         _showACDialogCommand = new RelayCommand(obj => true, obj => ShowACDialog());
         _rollInitiativeCommand = new RelayCommand(obj => true, obj => RollInitiative());
         _showMovementDialogCommand = new RelayCommand(obj => true, obj => ShowMovementDialog());
         _rollProficiencyCommand = new RelayCommand(obj => true, obj => RollProficiency());
         _showTempHPDialogCommand = new RelayCommand(obj => true, obj => ShowTempHPDialog());
         _showLongRestDialogCommand = new RelayCommand(obj => true, obj => ShowLongRestDialog());
         _showShortRestDialogCommand = new RelayCommand(obj => true, obj => ShowShortRestDialog());
         _showAddAttackDialogCommand = new RelayCommand(obj => true, obj => ShowAddAttackDialog());
         _rollAttackToHitCommand = new RelayCommand(obj => true, obj => RollAttackToHit((AttackViewModel)obj));
         _rollAttackDamageCommand = new RelayCommand(obj => true, obj => RollAttackDamage((AttackViewModel)obj));
         _showEditAttackDialogCommand = new RelayCommand(obj => true, obj => ShowEditAttackDialog((AttackViewModel)obj));
         _deleteAttackCommand = new RelayCommand(obj => true, obj => DeleteAttack((AttackViewModel)obj));
         _showAddCounterDialogCommand = new RelayCommand(obj => true, obj => ShowAddCounterDialog());
         _showEditCounterDialogCommand = new RelayCommand(obj => true, obj => ShowEditCounterDialog((CounterViewModel)obj));
         _deleteCounterCommand = new RelayCommand(obj => true, obj => DeleteCounter((CounterViewModel)obj));
         _showAddConditionDialogCommand = new RelayCommand(obj => true, obj => ShowAddConditionDialog());
         _showEditConditionDialogCommand = new RelayCommand(obj => true, obj => ShowEditConditionDialog((AppliedConditionViewModel)obj));
         _removeConditionCommand = new RelayCommand(obj => true, obj => RemoveCondition((AppliedConditionViewModel)obj));
         _showAddCompanionDialogCommand = new RelayCommand(obj => true, obj => ShowAddCompanionDialog());
         _showEditCompanionDialogCommand = new RelayCommand(obj => true, obj => ShowEditCompanionDialog((CompanionViewModel)obj));
         _deleteCompanionCommand = new RelayCommand(obj => true, obj => DeleteCompanion((CompanionViewModel)obj));
         _showAddBagDialogCommand = new RelayCommand(obj => true, obj => ShowAddBagDialog());
         _showEditBagDialogCommand = new RelayCommand(obj => true, obj => ShowEditBagDialog((BagViewModel)obj));
         _deleteBagCommand = new RelayCommand(obj => true, obj => DeleteBag((BagViewModel)obj));
         _showAddSpellbookDialogCommand = new RelayCommand(obj => true, obj => ShowAddSpellbookDialog());
         _showEditSpellbookDialogCommand = new RelayCommand(obj => true, obj => ShowEditSpellbookDialog((SpellbookViewModel)obj));
         _deleteSpellbookCommand = new RelayCommand(obj => true, obj => DeleteSpellbook((SpellbookViewModel)obj));
         _showAddStatModificationDialogCommand = new RelayCommand(obj => true, obj => ShowAddStatModificationDialog());
         _showEditStatModificationDialogCommand = new RelayCommand(obj => true, obj => ShowEditStatModificationDialog((StatModificationViewModel)obj));
         _deleteStatModificationCommand = new RelayCommand(obj => true, obj => DeleteStatModification((StatModificationViewModel)obj));
         _viewFeatCommand = new RelayCommand(obj => true, obj => ViewFeat((FeatViewModel)obj));
         _viewFeatureCommand = new RelayCommand(obj => true, obj => ViewFeature((FeatureViewModel)obj));
         _showConvertMoneyDialogCommand = new RelayCommand(obj => true, obj => ShowConvertMoneyDialog((BagViewModel)obj));
         _showLevelUpDialogCommand = new RelayCommand(obj => true, obj => ShowLevelUpDialog());
         _showDeathSavesDialogCommand = new RelayCommand(obj => true, obj => ShowDeathSavesDialog());
         _toggleInspirationCommand = new RelayCommand(obj => true, obj => ToggleInspiration());

         _selectStatsTabCommand = new RelayCommand(obj => true, obj => SelectStatsTab());
         _selectCombatTabCommand = new RelayCommand(obj => true, obj => SelectCombatTab());
         _selectInventoryAndEquipmentTabCommand = new RelayCommand(obj => true, obj => SelectInventoryandEquipmentTab());
         _selectSpellcastingTabCommand = new RelayCommand(obj => true, obj => SelectSpellcastingTab());
         _selectInformationTabCommand = new RelayCommand(obj => true, obj => SelectInformationTab());
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets character model
      /// </summary>
      public CharacterModel CharacterModel
      {
         get { return _characterModel; }
      }

      /// <summary>
      /// Gets name
      /// </summary>
      public string Name
      {
         get { return _name; }
      }

      /// <summary>
      /// Gets hp display
      /// </summary>
      public string HPDisplay
      {
         get
         {
            string hp = String.Empty;

            if (_tempHP == 0)
            {
               hp = $"{_currentHP}/{_maxHP}";
            }
            else
            {
               hp = $"{_currentHP}/{_maxHP} (+{_tempHP})";
            }

            return hp;
         }
      }

      /// <summary>
      /// Gets current hp
      /// </summary>
      public int CurrentHP
      {
         get { return _currentHP; }
      }

      /// <summary>
      /// Gets less hp
      /// </summary>
      public int LessHP
      {
         get { return _maxHP - _currentHP; }
      }

      /// <summary>
      /// Gets max hp
      /// </summary>
      public int MaxHP
      {
         get { return _maxHP; }
      }

      /// <summary>
      /// Gets temp hp
      /// </summary>
      public int TempHP
      {
         get { return _tempHP; }
      }

      /// <summary>
      /// Gets level
      /// </summary>
      public int Level
      {
         get { return _characterModel.Levels.Count; }
      }

      /// <summary>
      /// Gets next level
      /// </summary>
      public int NextLevel
      {
         get { return Math.Min(_characterModel.Levels.Count + 1, 20); }
      }

      /// <summary>
      /// Gets classes
      /// </summary>
      public IEnumerable<KeyValuePair<ClassViewModel, int>> Classes
      {
         get { return _classes; }
      }

      /// <summary>
      /// Gets class display
      /// </summary>
      public string ClassDisplay
      {
         get { return _classDisplay; }
      }

      /// <summary>
      /// Gets race
      /// </summary>
      public RaceViewModel Race
      {
         get { return _race; }
      }

      /// <summary>
      /// Gets background
      /// </summary>
      public BackgroundViewModel Background
      {
         get { return _background; }
      }

      /// <summary>
      /// Gets alignment
      /// </summary>
      public string Alignment
      {
         get { return _stringService.GetString(_alignment); }
      }

      /// <summary>
      /// Gets ac
      /// </summary>
      public int AC
      {
         get { return _ac; }
      }

      /// <summary>
      /// Gets initiative display
      /// </summary>
      public string InitiativeDisplay
      {
         get { return _statService.AddPlusOrMinus(_initiativeBonus); }
      }

      /// <summary>
      /// Gets initiative bonus
      /// </summary>
      public int InitiativeBonus
      {
         get { return _initiativeBonus; }
      }

      /// <summary>
      /// Gets walk speed
      /// </summary>
      public int WalkSpeed
      {
         get { return _movementViewModel.WalkSpeed; }
      }

      /// <summary>
      /// Gets swim speed
      /// </summary>
      public int SwimSpeed
      {
         get { return _movementViewModel.SwimSpeed; }
      }

      /// <summary>
      /// Gets climb speed
      /// </summary>
      public int ClimbSpeed
      {
         get { return _movementViewModel.ClimbSpeed; }
      }

      /// <summary>
      /// Gets crawl speed
      /// </summary>
      public int CrawlSpeed
      {
         get { return _movementViewModel.CrawlSpeed; }
      }

      /// <summary>
      /// Gets fly speed
      /// </summary>
      public int FlySpeed
      {
         get { return _movementViewModel.FlySpeed; }
      }

      /// <summary>
      /// Gets proficiency display
      /// </summary>
      public string ProficiencyDisplay
      {
         get { return _statService.AddPlusOrMinus(_proficiencyBonus); }
      }

      /// <summary>
      /// Gets proficiency bonus
      /// </summary>
      public int ProficiencyBonus
      {
         get { return _proficiencyBonus; }
      }

      /// <summary>
      /// Gets or sets inspiration
      /// </summary>
      public bool Inspiration
      {
         get { return _inspiration; }
         set
         {
            if (Set(ref _inspiration, value))
            {
               _characterModel.Inspiration = value;
            }
         }
      }

      /// <summary>
      /// Gets passive perception
      /// </summary>
      public int PassivePerception
      {
         get { return _passivePerception; }
      }

      /// <summary>
      /// Gets passive investigation
      /// </summary>
      public int PassiveInvestigation
      {
         get { return _passiveInvestigation; }
      }

      /// <summary>
      /// Gets show death saves
      /// </summary>
      public bool ShowDeathSaves
      {
         get { return _currentHP <= 0; }
      }

      /// <summary>
      /// Gets deity
      /// </summary>
      public string Deity
      {
         get { return _deity; }
      }

      /// <summary>
      /// Gets gender
      /// </summary>
      public string Gender
      {
         get { return _gender; }
      }

      /// <summary>
      /// Gets age
      /// </summary>
      public int Age
      {
         get { return _age; }
      }

      /// <summary>
      /// Gets height feet
      /// </summary>
      public int HeightFeet
      {
         get { return _heightFeet; }
      }

      /// <summary>
      /// Gets height inches
      /// </summary>
      public int HeightInches
      {
         get { return _heightInches; }
      }

      /// <summary>
      /// Gets height
      /// </summary>
      public string Height
      {
         get { return $"{_heightFeet}' {HeightInches}''"; }
      }

      /// <summary>
      /// Gets weight
      /// </summary>
      public int Weight
      {
         get { return _weight; }
      }

      /// <summary>
      /// Gets hair
      /// </summary>
      public string Hair
      {
         get { return _hair; }
      }

      /// <summary>
      /// Gets eyes
      /// </summary>
      public string Eyes
      {
         get { return _eyes; }
      }

      /// <summary>
      /// Gets skin
      /// </summary>
      public string Skin
      {
         get { return _skin; }
      }

      /// <summary>
      /// Gets experience
      /// </summary>
      public int Experience
      {
         get { return _experience; }
      }

      /// <summary>
      /// Gets experience at current level
      /// </summary>
      public int ExperienceAtCurrentLevel
      {
         get
         {
            if (_levels.Count < 20)
            {
               return Math.Max(_experience - _statService.GetXPForLevel(Level), 0);
            }
            else
            {
               return _experience;
            }
         }
      }

      /// <summary>
      /// Gets less experience
      /// </summary>
      public int LessExperience
      {
         get
         {
            if (_levels.Count < 20)
            {
               return _statService.GetXPForLevel(NextLevel) - _experience;
            }
            else
            {
               return 0;
            }
         }
      }

      /// <summary>
      /// Gets experience display
      /// </summary>
      public string ExperienceDisplay
      {
         get
         {
            if (_levels.Count < 20)
            {
               return $"{String.Format("{0:n0}", _experience)} / {String.Format("{0:n0}", _statService.GetXPForLevel(NextLevel))}";
            }
            else
            {
               return $"{String.Format("{0:n0}", _experience)} / {String.Format("{0:n0}", _experience)}";
            }
         }
      }

      /// <summary>
      /// Gets level up pending
      /// </summary>
      public bool LevelUpPending
      {
         get
         {
            if (_levels.Count < 20)
            {
               return _experience >= _statService.GetXPForLevel(_levels.Count + 1);
            }
            else
            {
               return false;
            }
         }
      }

      /// <summary>
      /// Gets total strength
      /// </summary>
      public int TotalStrength
      {
         get { return _totalStrength; }
      }

      /// <summary>
      /// Gets strength display
      /// </summary>
      public string StrengthDisplay
      {
         get { return $"{_totalStrength} ({_statService.GetStatBonusString(_totalStrength)})"; }
      }

      /// <summary>
      /// Gets total dexterity
      /// </summary>
      public int TotalDexterity
      {
         get { return _totalDexterity; }
      }

      /// <summary>
      /// Gets dexterity display
      /// </summary>
      public string DexterityDisplay
      {
         get { return $"{_totalDexterity} ({_statService.GetStatBonusString(_totalDexterity)})"; }
      }

      /// <summary>
      /// Gets total constitution
      /// </summary>
      public int TotalConstitution
      {
         get { return _totalConstitution; }
      }

      /// <summary>
      /// Gets constitution display
      /// </summary>
      public string ConstitutionDisplay
      {
         get { return $"{_totalConstitution} ({_statService.GetStatBonusString(_totalConstitution)})"; }
      }

      /// <summary>
      /// Gets total intelligence
      /// </summary>
      public int TotalIntelligence
      {
         get { return _totalIntelligence; }
      }

      /// <summary>
      /// Gets intelligence display
      /// </summary>
      public string IntelligenceDisplay
      {
         get { return $"{_totalIntelligence} ({_statService.GetStatBonusString(_totalIntelligence)})"; }
      }

      /// <summary>
      /// Gets total wisdom
      /// </summary>
      public int TotalWisdom
      {
         get { return _totalWisdom; }
      }

      /// <summary>
      /// Gets strenwisdomgth display
      /// </summary>
      public string WisdomDisplay
      {
         get { return $"{_totalWisdom} ({_statService.GetStatBonusString(_totalWisdom)})"; }
      }

      /// <summary>
      /// Gets total charisma
      /// </summary>
      public int TotalCharisma
      {
         get { return _totalCharisma; }
      }

      /// <summary>
      /// Gets charisma display
      /// </summary>
      public string CharismaDisplay
      {
         get { return $"{_totalCharisma} ({_statService.GetStatBonusString(_totalCharisma)})"; }
      }

      /// <summary>
      /// Gets carrying capacity
      /// </summary>
      public int CarryingCapacity
      {
         get { return _carryingCapacity; }
      }

      /// <summary>
      /// Gets personality traits
      /// </summary>
      public string PersonalityTraits
      {
         get { return _personalityTraits; }
      }

      /// <summary>
      /// Gets ideals
      /// </summary>
      public string Ideals
      {
         get { return _ideals; }
      }

      /// <summary>
      /// Gets bonds
      /// </summary>
      public string Bonds
      {
         get { return _bonds; }
      }

      /// <summary>
      /// Gets flaws
      /// </summary>
      public string Flaws
      {
         get { return _flaws; }
      }

      /// <summary>
      /// Gets backstory
      /// </summary>
      public string Backstory
      {
         get { return _backstory; }
      }

      /// <summary>
      /// Gets armor proficiencies
      /// </summary>
      public string ArmorProficiencies
      {
         get { return _armorProficiencies; }
      }

      /// <summary>
      /// Gets weapon proficiencies
      /// </summary>
      public string WeaponProficiencies
      {
         get { return _weaponProficiencies; }
      }

      /// <summary>
      /// Gets tool proficiencies
      /// </summary>
      public string ToolProficiencies
      {
         get { return _toolProficiencies; }
      }

      /// <summary>
      /// Gets languages
      /// </summary>
      public string Languages
      {
         get { return !String.IsNullOrWhiteSpace(_languages) ? _languages : "None"; }
      }

      /// <summary>
      /// Gets ability proficiencies
      /// </summary>
      public List<AbilityViewModel> AbilityProficiencies
      {
         get { return _abilityProficiencies; }
      }

      /// <summary>
      /// Gets saving throw notes
      /// </summary>
      public string SavingThrowNotes
      {
         get { return _savingThrowNotes; }
      }

      /// <summary>
      /// Gets has saving throw notes
      /// </summary>
      public bool HasSavingThrowNotes
      {
         get { return !String.IsNullOrWhiteSpace(_savingThrowNotes); }
      }

      /// <summary>
      /// Gets skill proficiencies
      /// </summary>
      public List<SkillViewModel> SkillProficiencies
      {
         get { return _skillProficiencies; }
      }

      /// <summary>
      /// Gets conditions
      /// </summary>
      public IEnumerable<AppliedConditionViewModel> Conditions
      {
         get { return _conditions; }
      }

      /// <summary>
      /// Gets show conditions header
      /// </summary>
      public bool ShowConditionsHeader
      {
         get { return _conditions.Any(); }
      }

      /// <summary>
      /// Gets feats
      /// </summary>
      public IEnumerable<FeatViewModel> Feats
      {
         get { return _feats; }
      }

      /// <summary>
      /// Gets features
      /// </summary>
      public IEnumerable<FeatureViewModel> Features
      {
         get { return _features; }
      }

      /// <summary>
      /// Gets attacks
      /// </summary>
      public IEnumerable<AttackViewModel> Attacks
      {
         get { return _attacks; }
      }

      /// <summary>
      /// Gets show attacks header
      /// </summary>
      public bool ShowAttacksHeader
      {
         get { return _attacks.Any(); }
      }

      /// <summary>
      /// Gets counters
      /// </summary>
      public IEnumerable<CounterViewModel> Counters
      {
         get { return _counters; }
      }

      /// <summary>
      /// Gets show counters header
      /// </summary>
      public bool ShowCountersHeader
      {
         get { return _counters.Any(); }
      }

      /// <summary>
      /// Gets companions
      /// </summary>
      public IEnumerable<CompanionViewModel> Companions
      {
         get { return _companions; }
      }

      /// <summary>
      /// Gets show companions header
      /// </summary>
      public bool ShowCompanionsHeader
      {
         get { return _companions.Any(); }
      }

      /// <summary>
      /// Gets bags
      /// </summary>
      public IEnumerable<BagViewModel> Bags
      {
         get { return _bags; }
      }

      /// <summary>
      /// Gets total weight
      /// </summary>
      public float TotalWeight
      {
         get { return _totalWeight; }
      }

      /// <summary>
      /// Gets total attuned items
      /// </summary>
      public int TotalAttunedItems
      {
         get { return _totalAttunedItems; }
      }

      /// <summary>
      /// Gets total copper
      /// </summary>
      public int TotalCopper
      {
         get { return _totalCopper; }
      }

      /// <summary>
      /// Gets total silver
      /// </summary>
      public int TotalSilver
      {
         get { return _totalSilver; }
      }

      /// <summary>
      /// Gets total electrum
      /// </summary>
      public int TotalElectrum
      {
         get { return _totalElectrum; }
      }

      /// <summary>
      /// Gets total gold
      /// </summary>
      public int TotalGold
      {
         get { return _totalGold; }
      }

      /// <summary>
      /// Gets total platinum
      /// </summary>
      public int TotalPlatinum
      {
         get { return _totalPlatinum; }
      }

      /// <summary>
      /// Gets equipped items
      /// </summary>
      public IEnumerable<EquipmentViewModel> EquippedItems
      {
         get { return _bags.SelectMany(x => x.Equipment).Where(x => x.Equipped); }
      }

      /// <summary>
      /// Gets spellbooks
      /// </summary>
      public IEnumerable<SpellbookViewModel> Spellbooks
      {
         get { return _spellbooks; }
      }

      /// <summary>
      /// Gets stat modifications
      /// </summary>
      public IEnumerable<StatModificationViewModel> StatModifications
      {
         get { return _statModifications; }
      }

      /// <summary>
      /// Gets show stat modifications header
      /// </summary>
      public bool ShowStatModificationsHeader
      {
         get { return _statModifications.Any(); }
      }

      /// <summary>
      /// Gets view class command
      /// </summary>
      public ICommand ViewClassCommand
      {
         get { return _viewClassCommand; }
      }

      /// <summary>
      /// Gets view background command
      /// </summary>
      public ICommand ViewBackgroundCommand
      {
         get { return _viewBackgroundCommand; }
      }

      /// <summary>
      /// Gets view race command
      /// </summary>
      public ICommand ViewRaceCommand
      {
         get { return _viewRaceCommand; }
      }

      /// <summary>
      /// Gets show hp dialog command
      /// </summary>
      public ICommand ShowHPDialogCommand
      {
         get { return _showHPDialogCommand; }
      }

      /// <summary>
      /// Gets show experience dialog command
      /// </summary>
      public ICommand ShowExperienceDialogCommand
      {
         get { return _showExperienceDialogCommand; }
      }

      /// <summary>
      /// Gets roll ability check command
      /// </summary>
      public ICommand RollAbilityCheckCommand
      {
         get { return _rollAbilityCheckCommand; }
      }

      /// <summary>
      /// Gets roll ability save command
      /// </summary>
      public ICommand RollAbilitySaveCommand
      {
         get { return _rollAbilitySaveCommand; }
      }

      /// <summary>
      /// Gets roll skill check command
      /// </summary>
      public ICommand RollSkillCheckCommand
      {
         get { return _rollSkillCheckCommand; }
      }

      /// <summary>
      /// Gets roll passive perception command
      /// </summary>
      public ICommand RollPassivePerceptionCommand
      {
         get { return _rollPassivePerceptionCommand; }
      }

      /// <summary>
      /// Gets roll passive investigation command
      /// </summary>
      public ICommand RollPassiveInvestigationCommand
      {
         get { return _rollPassiveInvestigationCommand; }
      }

      /// <summary>
      /// Gets show ac dialog command
      /// </summary>
      public ICommand ShowACDialogCommand
      {
         get { return _showACDialogCommand; }
      }

      /// <summary>
      /// Gets roll initiative command
      /// </summary>
      public ICommand RollInitiativeCommand
      {
         get { return _rollInitiativeCommand; }
      }

      /// <summary>
      /// Gets show movement dialog command
      /// </summary>
      public ICommand ShowMovementDialogCommand
      {
         get { return _showMovementDialogCommand; }
      }

      /// <summary>
      /// Gets roll proficiency command
      /// </summary>
      public ICommand RollProficiencyCommand
      {
         get { return _rollProficiencyCommand; }
      }

      /// <summary>
      /// Gets show temp hp dialog command
      /// </summary>
      public ICommand ShowTempHPDialogCommand
      {
         get { return _showTempHPDialogCommand; }
      }

      /// <summary>
      /// Gets show long rest dialog command
      /// </summary>
      public ICommand ShowLongRestDialogCommand
      {
         get { return _showLongRestDialogCommand; }
      }

      /// <summary>
      /// Gets show short rest dialog command
      /// </summary>
      public ICommand ShowShortRestDialogCommand
      {
         get { return _showShortRestDialogCommand; }
      }

      /// <summary>
      /// Gets show add attack dialog command
      /// </summary>
      public ICommand ShowAddAttackDialogCommand
      {
         get { return _showAddAttackDialogCommand; }
      }

      /// <summary>
      /// Gets roll attack to hit command
      /// </summary>
      public ICommand RollAttackToHitCommand
      {
         get { return _rollAttackToHitCommand; }
      }

      /// <summary>
      /// Gets roll attack damage command
      /// </summary>
      public ICommand RollAttackDamageCommand
      {
         get { return _rollAttackDamageCommand; }
      }

      /// <summary>
      /// Gets show edit attack dialog command
      /// </summary>
      public ICommand ShowEditAttackDialogCommand
      {
         get { return _showEditAttackDialogCommand; }
      }

      /// <summary>
      /// Gets delete attack command
      /// </summary>
      public ICommand DeleteAttackCommand
      {
         get { return _deleteAttackCommand; }
      }

      /// <summary>
      /// Gets show add counter dialog command
      /// </summary>
      public ICommand ShowAddCounterDialogCommand
      {
         get { return _showAddCounterDialogCommand; }
      }

      /// <summary>
      /// Gets show edit counter dialog command
      /// </summary>
      public ICommand ShowEditCounterDialogCommand
      {
         get { return _showEditCounterDialogCommand; }
      }

      /// <summary>
      /// Gets delete counter command
      /// </summary>
      public ICommand DeleteCounterCommand
      {
         get { return _deleteCounterCommand; }
      }


      /// <summary>
      /// Gets show add condition dialog command
      /// </summary>
      public ICommand ShowAddConditionDialogCommand
      {
         get { return _showAddConditionDialogCommand; }
      }

      /// <summary>
      /// Gets show edit condition dialog command
      /// </summary>
      public ICommand ShowEditConditionDialogCommand
      {
         get { return _showEditConditionDialogCommand; }
      }

      /// <summary>
      /// Gets remove condition command
      /// </summary>
      public ICommand RemoveConditionCommand
      {
         get { return _removeConditionCommand; }
      }

      /// <summary>
      /// Gets show add companion dialog command
      /// </summary>
      public ICommand ShowAddCompanionDialogCommand
      {
         get { return _showAddCompanionDialogCommand; }
      }

      /// <summary>
      /// Gets show edit companion dialog command
      /// </summary>
      public ICommand ShowEditCompanionDialogCommand
      {
         get { return _showEditCompanionDialogCommand; }
      }

      /// <summary>
      /// Gets delete companion command
      /// </summary>
      public ICommand DeleteCompanionCommand
      {
         get { return _deleteCompanionCommand; }
      }

      /// <summary>
      /// Gets show add bag dialog command
      /// </summary>
      public ICommand ShowAddBagDialogCommand
      {
         get { return _showAddBagDialogCommand; }
      }

      /// <summary>
      /// Gets show edit bag dialog command
      /// </summary>
      public ICommand ShowEditBagDialogCommand
      {
         get { return _showEditBagDialogCommand; }
      }

      /// <summary>
      /// Gets delete bag command
      /// </summary>
      public ICommand DeleteBagCommand
      {
         get { return _deleteBagCommand; }
      }

      /// <summary>
      /// Gets show add spellbook dialog command
      /// </summary>
      public ICommand ShowAddSpellbookDialogCommand
      {
         get { return _showAddSpellbookDialogCommand; }
      }

      /// <summary>
      /// Gets show edit spellbook dialog command
      /// </summary>
      public ICommand ShowEditSpellbookDialogCommand
      {
         get { return _showEditSpellbookDialogCommand; }
      }

      /// <summary>
      /// Gets delete spellbook command
      /// </summary>
      public ICommand DeleteSpellbookCommand
      {
         get { return _deleteSpellbookCommand; }
      }

      /// <summary>
      /// Gets show add stat modification dialog command
      /// </summary>
      public ICommand ShowAddStatModificationDialogCommand
      {
         get { return _showAddStatModificationDialogCommand; }
      }

      /// <summary>
      /// Gets show edit stat modification command
      /// </summary>
      public ICommand ShowEditStatModificationDialogCommand
      {
         get { return _showEditStatModificationDialogCommand; }
      }

      /// <summary>
      /// Gets delete stat modification command
      /// </summary>
      public ICommand DeleteStatModificationCommand
      {
         get { return _deleteStatModificationCommand; }
      }

      /// <summary>
      /// Gets view feat command
      /// </summary>
      public ICommand ViewFeatCommand
      {
         get { return _viewFeatCommand; }
      }

      /// <summary>
      /// Gets view feature command
      /// </summary>
      public ICommand ViewFeatureCommand
      {
         get { return _viewFeatureCommand; }
      }

      /// <summary>
      /// Gets show convert money dialog command
      /// </summary>
      public ICommand ShowConvertMoneyDialogCommand
      {
         get { return _showConvertMoneyDialogCommand; }
      }

      /// <summary>
      /// Gets show level up dialog command
      /// </summary>
      public ICommand ShowLevelUpDialogCommand
      {
         get { return _showLevelUpDialogCommand; }
      }

      /// <summary>
      /// Gets show death saves dialog command
      /// </summary>
      public ICommand ShowDeathSavesDialogCommand
      {
         get { return _showDeathSavesDialogCommand; }
      }

      /// <summary>
      /// Gets toggle inspiration command
      /// </summary>
      public ICommand ToggleInspirationCommand
      {
         get { return _toggleInspirationCommand; }
      }

      /// <summary>
      /// Gets stats tab selected
      /// </summary>
      public bool StatsTabSelected
      {
         get { return _statsTabSelected; }
      }

      /// <summary>
      /// Gets combat tab selected
      /// </summary>
      public bool CombatTabSelected
      {
         get { return _combatTabSelected; }
      }

      /// <summary>
      /// Gets inventory and equipment tab selected
      /// </summary>
      public bool InventoryAndEquipmentTabSelected
      {
         get { return _inventoryAndEquipmentTabSelected; }
      }

      /// <summary>
      /// Gets spellcasting tab selected
      /// </summary>
      public bool SpellcastingTabSelected
      {
         get { return _spellcastingTabSelected; }
      }

      /// <summary>
      /// Gets information tab selected
      /// </summary>
      public bool InformationTabSelected
      {
         get { return _informationTabSelected; }
      }

      /// <summary>
      /// Gets select stats tab command
      /// </summary>
      public ICommand SelectStatsTabCommand
      {
         get { return _selectStatsTabCommand; }
      }

      /// <summary>
      /// Gets select combat tab command
      /// </summary>
      public ICommand SelectCombatTabCommand
      {
         get { return _selectCombatTabCommand; }
      }

      /// <summary>
      /// Gets select inventory and equipment tab command
      /// </summary>
      public ICommand SelectInventoryAndEquipmentTabCommand
      {
         get { return _selectInventoryAndEquipmentTabCommand; }
      }

      /// <summary>
      /// Gets select spellcasting tab command
      /// </summary>
      public ICommand SelectSpellcastingTabCommand
      {
         get { return _selectSpellcastingTabCommand; }
      }

      /// <summary>
      /// Gets select information tab command
      /// </summary>
      public ICommand SelectInformationTabCommand
      {
         get { return _selectInformationTabCommand; }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Sets up a newly created character
      /// </summary>
      public void SetupNewlyCreatedCharacter()
      {
         _characterModel.CurrentHP = _maxHP;
         _currentHP = _characterModel.CurrentHP;

         _characterModel.Experience = _statService.GetXPForLevel(_characterModel.Levels.Count);
         _experience = _characterModel.Experience;

         _characterModel.MovementModel.WalkSpeed = _characterModel.Race != null ? _characterModel.Race.WalkSpeed : 0;

         int halfWalk = Math.Max(_characterModel.MovementModel.WalkSpeed / 2, 0);
         _characterModel.MovementModel.SwimSpeed = halfWalk;
         _characterModel.MovementModel.ClimbSpeed = halfWalk;
         _characterModel.MovementModel.CrawlSpeed = halfWalk;
         _characterModel.MovementModel.FlySpeed = _characterModel.Race != null ? _characterModel.Race.FlySpeed : 0;

         _movementViewModel = new MovementViewModel(_characterModel.MovementModel);
      }

      #endregion

      #region Private Methods

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

      private int GetAbilityBonus(Ability ability, bool proficient)
      {
         int bonus = 0;

         if (ability != Ability.None)
         {
            int abilityScore = 0;

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
               bonus = _statService.GetStatBonus(abilityScore) + _proficiencyBonus;
            }
            else
            {
               bonus = _statService.GetStatBonus(abilityScore);
            }
         }

         return bonus;
      }

      private AttackViewModel CreateAttackViewModel(AttackModel attackModel)
      {
         AttackViewModel attackViewModel = new AttackViewModel(attackModel);

         attackViewModel.ToHit = "1d20";
         int toHitbonus = GetAbilityBonus(attackModel.Ability, attackModel.Proficient) + attackModel.AdditionalToHitBonus;
         if (toHitbonus != 0)
         {
            attackViewModel.ToHit += _statService.AddPlusOrMinus(toHitbonus);
         }

         attackViewModel.Damage = attackModel.NumberOfDamageDice + attackModel.DamageDie;
         int damageBonus = GetAbilityBonus(attackModel.Ability, false) + attackViewModel.AdditionalDamageBonus;
         if (damageBonus != 0)
         {
            attackViewModel.Damage += _statService.AddPlusOrMinus(damageBonus);
         }

         if (attackModel.DamageType != DamageType.None)
         {
            string damageAbbreviation = _stringService.GetAbbreviationString(attackModel.DamageType);
            if (!String.IsNullOrWhiteSpace(damageAbbreviation))
            {
               attackViewModel.Damage += $" {damageAbbreviation}";
            }
         }

         return attackViewModel;
      }

      private SpellbookViewModel CreateSpellbookViewModel(SpellbookModel spellbookModel)
      {
         SpellbookViewModel spellbookViewModel = new SpellbookViewModel(spellbookModel);

         if (spellbookViewModel.BasedOnClass && spellbookViewModel.Class != null)
         {
            List<int> spellSlots = new List<int>();

            ClassModel classModel = _compendium.Classes.FirstOrDefault(x => x.ID == spellbookViewModel.Class.ID);
            if (classModel != null)
            {
               KeyValuePair<KeyValuePair<Guid, string>, int> classPair = _classesMap.FirstOrDefault(x => x.Key.Key == spellbookViewModel.Class.ID);
               if (!classPair.Equals(default(KeyValuePair<KeyValuePair<Guid, string>, int>)))
               {
                  int slotIndex = classPair.Value - 1;
                  if (slotIndex < classModel.SpellSlots.Count)
                  {
                     foreach (string slot in classModel.SpellSlots[slotIndex].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                     {
                        if (Int32.TryParse(slot, out int value))
                        {
                           spellSlots.Add(value);
                        }
                        else
                        {
                           spellSlots.Add(0);
                        }
                     }
                  }
               }
            }

            while (spellSlots.Count < 10)
            {
               spellSlots.Add(0);
            }

            spellbookViewModel.SpellSlots = spellSlots;
         }

         int spellBookAbilityBonus = GetAbilityBonus(spellbookViewModel.Ability, true);
         spellbookViewModel.BaseSaveDC = 8 + spellBookAbilityBonus;
         spellbookViewModel.BaseToHitBonus = spellBookAbilityBonus;

         spellbookViewModel.InitializeSpellsByLevel();

         return spellbookViewModel;
      }

      private void UpdateSpellbookSlots(SpellbookViewModel spellbookViewModel)
      {
         if (spellbookViewModel.BasedOnClass && spellbookViewModel.Class != null)
         {
            List<int> spellSlots = new List<int>();

            ClassModel classModel = _compendium.Classes.FirstOrDefault(x => x.ID == spellbookViewModel.Class.ID);
            if (classModel != null)
            {
               KeyValuePair<KeyValuePair<Guid, string>, int> classPair = _classesMap.FirstOrDefault(x => x.Key.Key == spellbookViewModel.Class.ID);
               if (!classPair.Equals(default(KeyValuePair<KeyValuePair<Guid, string>, int>)))
               {
                  int slotIndex = classPair.Value - 1;
                  if (slotIndex < classModel.SpellSlots.Count)
                  {
                     foreach (string slot in classModel.SpellSlots[slotIndex].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                     {
                        if (Int32.TryParse(slot, out int value))
                        {
                           spellSlots.Add(value);
                        }
                        else
                        {
                           spellSlots.Add(0);
                        }
                     }
                  }
               }
            }

            while (spellSlots.Count < 10)
            {
               spellSlots.Add(0);
            }

            spellbookViewModel.SpellSlots = spellSlots;
         }
      }

      private int GetAbilitySaveBonus(Ability ability, bool proficient)
      {
         int bonus = 0;

         int abilityScore = 0;

         switch (ability)
         {
            case Ability.Strength:
               abilityScore = _totalStrength;
               break;

            case Ability.Dexterity:
               abilityScore = _totalDexterity;
               break;

            case Ability.Constitution:
               abilityScore = _totalConstitution;
               break;

            case Ability.Intelligence:
               abilityScore = _totalIntelligence;
               break;

            case Ability.Wisdom:
               abilityScore = _totalWisdom;
               break;

            case Ability.Charisma:
               abilityScore = _totalCharisma;
               break;
         }

         if (proficient)
         {
            bonus = _statService.GetStatBonus(abilityScore) + _proficiencyBonus;
         }
         else
         {
            bonus = _statService.GetStatBonus(abilityScore);
         }

         foreach (StatModificationViewModel statModificationViewModel in _statModifications.Where(x => x.ModificationOption == StatModificationOption.SavingThrows))
         {
            bonus += statModificationViewModel.Value;
         }

         return bonus;
      }

      private int GetSkillBonus(Skill skill, bool proficient, bool expertise)
      {
         int bonus = 0;

         int abilityScore = 0;

         switch (skill)
         {
            case Skill.Athletics:
               abilityScore = _totalStrength;
               break;

            case Skill.Acrobatics:
            case Skill.Sleight_of_Hand:
            case Skill.Stealth:
               abilityScore = _totalDexterity;
               break;

            case Skill.Arcana:
            case Skill.History:
            case Skill.Investigation:
            case Skill.Nature:
            case Skill.Religion:
               abilityScore = _totalIntelligence;
               break;

            case Skill.Animal_Handling:
            case Skill.Insight:
            case Skill.Medicine:
            case Skill.Perception:
            case Skill.Survival:
               abilityScore = _totalWisdom;
               break;

            case Skill.Deception:
            case Skill.Intimidation:
            case Skill.Performance:
            case Skill.Persuasion:
               abilityScore = _totalCharisma;
               break;
         }

         bonus = _statService.GetStatBonus(abilityScore);

         if (proficient)
         {
            bonus += _proficiencyBonus;
         }
         else
         {
            foreach (StatModificationViewModel statModificationViewModel in _statModifications.Where(x => x.ModificationOption == StatModificationOption.NonProficientAbilityChecks))
            {
               bonus += statModificationViewModel.Value;
            }
         }

         if (expertise)
         {
            bonus += _proficiencyBonus;
         }

         return bonus;
      }

      private void ApplyStatModifications()
      {
         _totalStrength = _characterModel.BaseStrength + _characterModel.BonusStrength + GetRaceAbilityBonus(Ability.Strength) + GetLevelAbilityBonus(Ability.Strength);
         _totalDexterity = _characterModel.BaseDexterity + _characterModel.BonusDexterity + GetRaceAbilityBonus(Ability.Dexterity) + GetLevelAbilityBonus(Ability.Dexterity);
         _totalConstitution = _characterModel.BaseConstitution + _characterModel.BonusConstitution + GetRaceAbilityBonus(Ability.Constitution) + GetLevelAbilityBonus(Ability.Constitution);
         _totalIntelligence = _characterModel.BaseIntelligence + _characterModel.BonusIntelligence + GetRaceAbilityBonus(Ability.Intelligence) + GetLevelAbilityBonus(Ability.Intelligence);
         _totalWisdom = _characterModel.BaseWisdom + _characterModel.BonusWisdom + GetRaceAbilityBonus(Ability.Wisdom) + GetLevelAbilityBonus(Ability.Wisdom);
         _totalCharisma = _characterModel.BaseCharisma + _characterModel.BonusCharisma + GetRaceAbilityBonus(Ability.Charisma) + GetLevelAbilityBonus(Ability.Charisma);
         _initiativeBonus = _statService.GetStatBonus(_totalDexterity);
         _passivePerception = 10 + _skillProficiencies.First(x => x.Skill == Skill.Perception).Bonus;
         _passiveInvestigation = 10 + _skillProficiencies.First(x => x.Skill == Skill.Investigation).Bonus;
         _carryingCapacity = _totalStrength * 15;

         UpdateAbilityBonus(Ability.Strength);
         UpdateAbilityBonus(Ability.Dexterity);
         UpdateAbilityBonus(Ability.Constitution);
         UpdateAbilityBonus(Ability.Intelligence);
         UpdateAbilityBonus(Ability.Wisdom);
         UpdateAbilityBonus(Ability.Charisma);

         UpdateSkillBonus(Skill.Acrobatics);
         UpdateSkillBonus(Skill.Animal_Handling);
         UpdateSkillBonus(Skill.Arcana);
         UpdateSkillBonus(Skill.Athletics);
         UpdateSkillBonus(Skill.Deception);
         UpdateSkillBonus(Skill.History);
         UpdateSkillBonus(Skill.Insight);
         UpdateSkillBonus(Skill.Intimidation);
         UpdateSkillBonus(Skill.Investigation);
         UpdateSkillBonus(Skill.Medicine);
         UpdateSkillBonus(Skill.Nature);
         UpdateSkillBonus(Skill.None);
         UpdateSkillBonus(Skill.Perception);
         UpdateSkillBonus(Skill.Performance);
         UpdateSkillBonus(Skill.Persuasion);
         UpdateSkillBonus(Skill.Religion);
         UpdateSkillBonus(Skill.Sleight_of_Hand);
         UpdateSkillBonus(Skill.Stealth);
         UpdateSkillBonus(Skill.Survival);

         foreach (StatModificationModel statModification in _characterModel.StatModifications)
         {
            switch (statModification.ModificationOption)
            {
               case StatModificationOption.Strength:
                  if (ApplyStatModification(ref _totalStrength, statModification.Value, statModification.FixedValue))
                  {
                     UpdateAbilityBonus(Ability.Strength);

                     UpdateSkillBonus(Skill.Athletics);

                     _carryingCapacity = _totalStrength * 15;

                     CalculateArmorClass();

                     UpdateEncumbrance();

                     OnPropertyChanged(nameof(WalkSpeed));
                     OnPropertyChanged(nameof(SwimSpeed));
                     OnPropertyChanged(nameof(ClimbSpeed));
                     OnPropertyChanged(nameof(CrawlSpeed));
                     OnPropertyChanged(nameof(FlySpeed));

                     OnPropertyChanged(nameof(AC));
                     OnPropertyChanged(nameof(TotalStrength));
                     OnPropertyChanged(nameof(StrengthDisplay));
                     OnPropertyChanged(nameof(CarryingCapacity));
                  }
                  break;

               case StatModificationOption.Dexterity:
                  if (ApplyStatModification(ref _totalDexterity, statModification.Value, statModification.FixedValue))
                  {
                     UpdateAbilityBonus(Ability.Dexterity);

                     UpdateSkillBonus(Skill.Acrobatics);
                     UpdateSkillBonus(Skill.Sleight_of_Hand);
                     UpdateSkillBonus(Skill.Stealth);

                     _initiativeBonus = _statService.GetStatBonus(_totalDexterity);

                     CalculateArmorClass();

                     OnPropertyChanged(nameof(AC));
                     OnPropertyChanged(nameof(TotalDexterity));
                     OnPropertyChanged(nameof(DexterityDisplay));
                     OnPropertyChanged(nameof(InitiativeDisplay));
                  }
                  break;

               case StatModificationOption.Constitution:
                  if (ApplyStatModification(ref _totalConstitution, statModification.Value, statModification.FixedValue))
                  {
                     UpdateAbilityBonus(Ability.Constitution);

                     CalculateArmorClass();

                     OnPropertyChanged(nameof(AC));
                     OnPropertyChanged(nameof(TotalConstitution));
                     OnPropertyChanged(nameof(ConstitutionDisplay));
                  }
                  break;

               case StatModificationOption.Intelligence:
                  if (ApplyStatModification(ref _totalIntelligence, statModification.Value, statModification.FixedValue))
                  {
                     UpdateAbilityBonus(Ability.Intelligence);

                     UpdateSkillBonus(Skill.Arcana);
                     UpdateSkillBonus(Skill.History);
                     UpdateSkillBonus(Skill.Investigation);
                     UpdateSkillBonus(Skill.Nature);
                     UpdateSkillBonus(Skill.Religion);

                     _passiveInvestigation = 10 + _skillProficiencies.First(x => x.Skill == Skill.Investigation).Bonus;

                     CalculateArmorClass();

                     OnPropertyChanged(nameof(AC));
                     OnPropertyChanged(nameof(TotalIntelligence));
                     OnPropertyChanged(nameof(IntelligenceDisplay));
                     OnPropertyChanged(nameof(PassiveInvestigation));
                  }
                  break;

               case StatModificationOption.Wisdom:
                  if (ApplyStatModification(ref _totalWisdom, statModification.Value, statModification.FixedValue))
                  {
                     UpdateAbilityBonus(Ability.Wisdom);

                     UpdateSkillBonus(Skill.Animal_Handling);
                     UpdateSkillBonus(Skill.Insight);
                     UpdateSkillBonus(Skill.Medicine);
                     UpdateSkillBonus(Skill.Perception);
                     UpdateSkillBonus(Skill.Survival);

                     _passivePerception = 10 + _skillProficiencies.First(x => x.Skill == Skill.Perception).Bonus;

                     CalculateArmorClass();

                     OnPropertyChanged(nameof(AC));
                     OnPropertyChanged(nameof(TotalWisdom));
                     OnPropertyChanged(nameof(WisdomDisplay));
                     OnPropertyChanged(nameof(PassivePerception));
                  }
                  break;

               case StatModificationOption.Charisma:
                  if (ApplyStatModification(ref _totalCharisma, statModification.Value, statModification.FixedValue))
                  {
                     UpdateAbilityBonus(Ability.Charisma);

                     UpdateSkillBonus(Skill.Deception);
                     UpdateSkillBonus(Skill.Insight);
                     UpdateSkillBonus(Skill.Performance);
                     UpdateSkillBonus(Skill.Persuasion);

                     CalculateArmorClass();

                     OnPropertyChanged(nameof(AC));
                     OnPropertyChanged(nameof(TotalCharisma));
                     OnPropertyChanged(nameof(CharismaDisplay));
                  }
                  break;

               case StatModificationOption.Initiative:
                  if (ApplyStatModification(ref _initiativeBonus, statModification.Value, statModification.FixedValue))
                  {
                     OnPropertyChanged(nameof(InitiativeDisplay));
                  }
                  break;

               case StatModificationOption.PassivePerception:
                  if (ApplyStatModification(ref _passivePerception, statModification.Value, statModification.FixedValue))
                  {
                     OnPropertyChanged(nameof(PassivePerception));
                  }
                  break;

               case StatModificationOption.PassiveInvestigation:
                  if (ApplyStatModification(ref _passiveInvestigation, statModification.Value, statModification.FixedValue))
                  {
                     OnPropertyChanged(nameof(PassiveInvestigation));
                  }
                  break;

               case StatModificationOption.SavingThrows:
                  UpdateAbilityBonus(Ability.Strength);
                  UpdateAbilityBonus(Ability.Dexterity);
                  UpdateAbilityBonus(Ability.Constitution);
                  UpdateAbilityBonus(Ability.Intelligence);
                  UpdateAbilityBonus(Ability.Wisdom);
                  UpdateAbilityBonus(Ability.Charisma);

                  OnPropertyChanged(nameof(AbilityProficiencies));
                  break;

               case StatModificationOption.NonProficientAbilityChecks:
                  _initiativeBonus += statModification.Value;
                  UpdateSkillBonus(Skill.Acrobatics);
                  UpdateSkillBonus(Skill.Animal_Handling);
                  UpdateSkillBonus(Skill.Arcana);
                  UpdateSkillBonus(Skill.Athletics);
                  UpdateSkillBonus(Skill.Deception);
                  UpdateSkillBonus(Skill.History);
                  UpdateSkillBonus(Skill.Insight);
                  UpdateSkillBonus(Skill.Intimidation);
                  UpdateSkillBonus(Skill.Investigation);
                  UpdateSkillBonus(Skill.Medicine);
                  UpdateSkillBonus(Skill.Nature);
                  UpdateSkillBonus(Skill.None);
                  UpdateSkillBonus(Skill.Perception);
                  UpdateSkillBonus(Skill.Performance);
                  UpdateSkillBonus(Skill.Persuasion);
                  UpdateSkillBonus(Skill.Religion);
                  UpdateSkillBonus(Skill.Sleight_of_Hand);
                  UpdateSkillBonus(Skill.Stealth);
                  UpdateSkillBonus(Skill.Survival);

                  OnPropertyChanged(nameof(InitiativeDisplay));
                  OnPropertyChanged(nameof(SkillProficiencies));
                  break;
            }
         }
      }

      private bool ApplyStatModification(ref int stat, int value, bool isFixed)
      {
         bool set = false;

         if (isFixed)
         {
            if (stat < value)
            {
               stat = value;
               set = true;
            }
         }
         else
         {
            stat += value;
            set = true;
         }

         return set;
      }

      private void UpdateAbilityBonusValues()
      {
         foreach (AbilityViewModel abilityView in _abilityProficiencies)
         {
            abilityView.CheckBonus = GetAbilitySaveBonus(abilityView.Ability, false);
            abilityView.SaveBonus = GetAbilitySaveBonus(abilityView.Ability, abilityView.Proficient);
         }
      }

      private void UpdateAbilityBonus(Ability ability)
      {
         AbilityViewModel abilityView = _abilityProficiencies.First(x => x.Ability == ability);
         if (abilityView != null)
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

      private void UpdateSkillBonus(Skill skill)
      {
         SkillViewModel skillView = _skillProficiencies.FirstOrDefault(x => x.Skill == skill);
         if (skillView != null)
         {
            skillView.Bonus = GetSkillBonus(skillView.Skill, skillView.Proficient, skillView.Expertise);
         }
      }

      private void ViewClass(ClassViewModel classView)
      {
         if (classView != null)
         {
            _dialogService.ShowDetailsDialog(classView);
         }
      }

      private void ViewBackground()
      {
         if (_background != null)
         {
            _dialogService.ShowDetailsDialog(_background);
         }
      }

      private void ViewRace()
      {
         if (_race != null)
         {
            _dialogService.ShowDetailsDialog(_race);
         }
      }

      private void ShowHPDialog()
      {
         int? result = _dialogService.ShowAddSubtractDialog(_currentHP + _tempHP);

         if (result.HasValue)
         {
            if (_tempHP > 0)
            {
               int temp = result.Value - _currentHP;

               if (temp > _tempHP)
               {
                  int toHeal = temp - _tempHP;
                  _currentHP = Math.Max(Math.Min(_maxHP, _currentHP + toHeal), 0);
                  _characterModel.CurrentHP = _currentHP;
               }
               else
               {
                  _tempHP = temp;
               }

               if (_tempHP < 0)
               {
                  _currentHP = Math.Max(_currentHP + _tempHP, 0);
                  _tempHP = 0;
               }
            }
            else
            {
               _currentHP = Math.Max(Math.Min(_maxHP, result.Value), 0);
               _characterModel.CurrentHP = _currentHP;
            }

            if (_currentHP > 0 && (_characterModel.DeathSaveSuccesses > 0 || _characterModel.DeathSaveFailures > 0))
            {
               _characterModel.DeathSaveSuccesses = 0;
               _characterModel.DeathSaveFailures = 0;
            }

            OnPropertyChanged(nameof(CurrentHP));
            OnPropertyChanged(nameof(LessHP));
            OnPropertyChanged(nameof(HPDisplay));
            OnPropertyChanged(nameof(TempHP));
            OnPropertyChanged(nameof(ShowDeathSaves));
         }
      }

      private void ShowExperienceDialog()
      {
         int? result = _dialogService.ShowAddSubtractDialog(_experience);

         if (result.HasValue)
         {
            _experience = Math.Min(Math.Max(result.Value, _statService.GetXPForLevel(Level)), _statService.GetXPForLevel(20));
            _characterModel.Experience = _experience;
            OnPropertyChanged(nameof(Experience));
            OnPropertyChanged(nameof(ExperienceAtCurrentLevel));
            OnPropertyChanged(nameof(LessExperience));
            OnPropertyChanged(nameof(ExperienceDisplay));
            OnPropertyChanged(nameof(LevelUpPending));
         }
      }

      private void RollAbilityCheck(Ability ability)
      {
         AbilityViewModel abilityViewModel = _abilityProficiencies.FirstOrDefault(x => x.Ability == ability);
         if (abilityViewModel != null)
         {
            int bonus = abilityViewModel.CheckBonus;

            if (!abilityViewModel.Proficient)
            {
               foreach (StatModificationModel statModification in _characterModel.StatModifications.Where(x => x.ModificationOption == StatModificationOption.NonProficientAbilityChecks))
               {
                  bonus += statModification.Value;
               }
            }

            _dialogService.ShowDiceRollDialog(abilityViewModel.AbilityString + " Check", "1d20" + _statService.AddPlusOrMinus(bonus));
         }
      }

      private void RollAbilitySave(AbilityViewModel ability)
      {
         _dialogService.ShowDiceRollDialog(ability.AbilityString + " Save", "1d20" + ability.SaveBonusString);
      }

      private void RollSkillCheck(SkillViewModel skill)
      {
         _dialogService.ShowDiceRollDialog(skill.SkillString + " Check", "1d20" + skill.BonusString);
      }

      private void RollPassivePerception()
      {
         SkillViewModel skillViewModel = _skillProficiencies.FirstOrDefault(x => x.Skill == Skill.Perception);
         if (skillViewModel != null)
         {
            _dialogService.ShowDiceRollDialog(skillViewModel.SkillString + " Check", "1d20" + skillViewModel.BonusString);
         }
      }

      private void RollPassiveInvestigation()
      {
         SkillViewModel skillViewModel = _skillProficiencies.FirstOrDefault(x => x.Skill == Skill.Investigation);
         if (skillViewModel != null)
         {
            _dialogService.ShowDiceRollDialog(skillViewModel.SkillString + " Check", "1d20" + skillViewModel.BonusString);
         }
      }

      private void ShowACDialog()
      {
         ArmorClassModel armorClassModel = _dialogService.ShowArmorClassDialog(_characterModel.ArmorClassModel);
         if (armorClassModel != null)
         {
            _armorClassViewModel = new ArmorClassViewModel(armorClassModel);
            _characterModel.ArmorClassModel = armorClassModel;

            CalculateArmorClass();

            OnPropertyChanged(nameof(AC));
         }
      }

      private void CalculateArmorClass()
      {
         _ac = _armorClassViewModel.ArmorBonus;

         if (_armorClassViewModel.FirstAbility != Ability.None)
         {
            int bonus = GetAbilitySaveBonus(_armorClassViewModel.FirstAbility, false);
            if (_armorClassViewModel.ArmorType == ArmorType.Medium_Armor && _armorClassViewModel.FirstAbility == Ability.Dexterity)
            {
               bonus = Math.Min(bonus, 2);
            }
            _ac += bonus;
         }

         if (_armorClassViewModel.SecondAbility != Ability.None)
         {
            int bonus = GetAbilitySaveBonus(_armorClassViewModel.SecondAbility, false);
            if (_armorClassViewModel.ArmorType == ArmorType.Medium_Armor && _armorClassViewModel.SecondAbility == Ability.Dexterity)
            {
               bonus = Math.Min(bonus, 2);
            }
            _ac += bonus;
         }

         _ac += _armorClassViewModel.ItemBonus;
         _ac += _armorClassViewModel.AdditionalBonus;
      }

      private void RollInitiative()
      {
         _dialogService.ShowDiceRollDialog("Initiative", "1d20" + InitiativeDisplay);
      }

      private void ShowMovementDialog()
      {
         MovementModel movementModel = _dialogService.ShowMovementDialog(_characterModel.MovementModel);
         if (movementModel != null)
         {
            _movementViewModel = new MovementViewModel(movementModel);
            _characterModel.MovementModel = movementModel;

            UpdateEncumbrance();

            OnPropertyChanged(nameof(WalkSpeed));
            OnPropertyChanged(nameof(SwimSpeed));
            OnPropertyChanged(nameof(ClimbSpeed));
            OnPropertyChanged(nameof(CrawlSpeed));
            OnPropertyChanged(nameof(FlySpeed));
         }
      }

      private void UpdateEncumbrance()
      {
         if (_movementViewModel.ApplyEncumbrance)
         {
            if (_totalWeight > _totalStrength * 10)
            {
               _movementViewModel.WalkSpeed = Math.Max((_characterModel.Race != null ? _characterModel.Race.WalkSpeed : 0) - 20, 0);
            }
            else if (_totalWeight > _totalStrength * 5)
            {
               _movementViewModel.WalkSpeed = Math.Max((_characterModel.Race != null ? _characterModel.Race.WalkSpeed : 0) - 10, 0);
            }
            else
            {
               _movementViewModel.WalkSpeed = _characterModel.Race != null ? _characterModel.Race.WalkSpeed : 0;
            }

            int halfWalk = Math.Max(_movementViewModel.WalkSpeed / 2, 0);

            _movementViewModel.SwimSpeed = halfWalk;
            _movementViewModel.ClimbSpeed = halfWalk;
            _movementViewModel.CrawlSpeed = halfWalk;
            _movementViewModel.FlySpeed = _characterModel.Race != null ? _characterModel.Race.FlySpeed : 0;
         }
      }

      private void RollProficiency()
      {
         _dialogService.ShowDiceRollDialog("Proficiency", "1d20+" + _proficiencyBonus);
      }

      private void ShowTempHPDialog()
      {
         int? result = _dialogService.ShowAddSubtractDialog(_tempHP);

         if (result.HasValue)
         {
            _tempHP = Math.Max(result.Value, 0);
            _characterModel.TempHP = _tempHP;
            OnPropertyChanged(nameof(TempHP));
            OnPropertyChanged(nameof(HPDisplay));
         }
      }

      private void ShowLongRestDialog()
      {
         bool? result = _dialogService.ShowConfirmationDialog("Long Rest", _stringService.LongRestDescription, "Apply Long Rest", "Cancel", null);
         if (result == true)
         {
            if (_currentHP <= 0)
            {
               _dialogService.ShowConfirmationDialog("Long Rest", "A character must have at least 1 hit point at the start of the rest to gain its benefits.", "OK", null, null);
            }
            else
            {
               _currentHP = _maxHP;
               _characterModel.CurrentHP = _currentHP;

               _tempHP = 0;
               _characterModel.TempHP = _tempHP;

               int index = 0;
               int hitDiceToRecover = Math.Max(_levels.Count / 2, 1);
               while (hitDiceToRecover > 0 && index < _levels.Count)
               {
                  if (_levels[index].HitDieUsed)
                  {
                     _levels[index].HitDieUsed = false;
                     _levels[index].HitDieRestRoll = 0;
                     hitDiceToRecover--;
                  }
                  index++;
               }

               _characterModel.DeathSaveSuccesses = 0;
               _characterModel.DeathSaveFailures = 0;

               foreach (CounterViewModel counterViewModel in _counters)
               {
                  if (counterViewModel.ResetOnLongRest)
                  {
                     counterViewModel.CurrentValue = counterViewModel.MaxValue;
                  }
               }

               foreach (CompanionViewModel companionViewModel in _companions)
               {
                  companionViewModel.CurrentHP = companionViewModel.MaxHP;
               }

               foreach (SpellbookViewModel spellbookViewModel in _spellbooks)
               {
                  if (spellbookViewModel.ResetOnLongRest)
                  {
                     spellbookViewModel.ResetSpellSlotsAndUses();
                  }
               }

               OnPropertyChanged(nameof(CurrentHP));
               OnPropertyChanged(nameof(LessHP));
               OnPropertyChanged(nameof(TempHP));
               OnPropertyChanged(nameof(HPDisplay));
               OnPropertyChanged(nameof(ShowDeathSaves));
            }
         }
      }

      private void ShowShortRestDialog()
      {
         int conModifier = _statService.GetStatBonus(_totalConstitution);

         List<LevelModel> levelModels = _dialogService.ShowShortRestDialog(_characterModel.Levels, conModifier);
         if (levelModels != null)
         {
            IEnumerable<LevelModel> unUsedLevels = levelModels.Where(x => !x.HitDieUsed);

            if (unUsedLevels.Any())
            {
               int total = 0;
               foreach (LevelModel level in unUsedLevels)
               {
                  if (level.HitDieRestRoll > 0)
                  {
                     total += Math.Max(level.HitDieRestRoll + conModifier, 0);
                     level.HitDieUsed = true;
                  }
               }

               _levels.Clear();
               _characterModel.Levels = levelModels;
               foreach (LevelModel level in _characterModel.Levels)
               {
                  _levels.Add(new LevelViewModel(level));
               }

               _currentHP = Math.Min(_currentHP + total, _maxHP);
               _characterModel.CurrentHP = _currentHP;

               OnPropertyChanged(nameof(CurrentHP));
               OnPropertyChanged(nameof(LessHP));
               OnPropertyChanged(nameof(HPDisplay));
               OnPropertyChanged(nameof(ShowDeathSaves));
            }
         }
      }

      private void ShowAddAttackDialog()
      {
         AttackModel attackModel = _dialogService.ShowCreateAttackDialog("Add New Attack", new AttackModel());
         if (attackModel != null)
         {
            AttackViewModel attackViewModel = CreateAttackViewModel(attackModel);

            _characterModel.Attacks.Add(attackModel);
            _attacks.Add(attackViewModel);

            _attacks = new ObservableCollection<AttackViewModel>(_attacks.OrderBy(x => x.Name));

            OnPropertyChanged(nameof(Attacks));
            OnPropertyChanged(nameof(ShowAttacksHeader));
         }
      }

      private void RollAttackToHit(AttackViewModel attack)
      {
         _dialogService.ShowDiceRollDialog(attack.Name + " (To Hit)", attack.ToHit);
      }

      private void RollAttackDamage(AttackViewModel attack)
      {
         string damage = attack.NumberOfDamageDice + attack.DamageDie;
         int damageBonus = GetAbilityBonus(attack.Ability, false) + attack.AdditionalDamageBonus;
         if (damageBonus != 0)
         {
            damage += _statService.AddPlusOrMinus(damageBonus);
         }

         _dialogService.ShowDiceRollDialog(attack.Name + $" ({_stringService.GetString(attack.DamageType)} Damage)", damage);
      }

      private void ShowEditAttackDialog(AttackViewModel attack)
      {
         AttackModel attackModel = _dialogService.ShowCreateAttackDialog($"Edit {attack.Name}", attack.AttackModel);
         if (attackModel != null)
         {
            _attacks.Remove(attack);
            _characterModel.Attacks.Remove(attack.AttackModel);

            AttackViewModel attackViewModel = CreateAttackViewModel(attackModel);
            _characterModel.Attacks.Add(attackModel);
            _attacks.Add(attackViewModel);

            _attacks = new ObservableCollection<AttackViewModel>(_attacks.OrderBy(x => x.Name));

            OnPropertyChanged(nameof(Attacks));
            OnPropertyChanged(nameof(ShowAttacksHeader));
         }
      }

      private void DeleteAttack(AttackViewModel attack)
      {
         bool? result = _dialogService.ShowConfirmationDialog("Delete Attack", "Are you sure you want to delete " + attack.Name + "?", "Yes", "No", null);
         if (result == true)
         {
            _characterModel.Attacks.RemoveAll(x => x.ID == attack.ID);
            _attacks.Remove(attack);
            OnPropertyChanged(nameof(Attacks));
            OnPropertyChanged(nameof(ShowAttacksHeader));
         }
      }

      private void ShowAddCounterDialog()
      {
         CounterModel counterModel = _dialogService.ShowCreateCounterDialog("Add Counter", new CounterModel());
         if (counterModel != null)
         {
            _counters.Add(new CounterViewModel(counterModel));
            _characterModel.Counters.Add(counterModel);

            _counters = new ObservableCollection<CounterViewModel>(_counters.OrderBy(x => x.Name));

            OnPropertyChanged(nameof(Counters));
            OnPropertyChanged(nameof(ShowCountersHeader));
         }
      }

      private void ShowEditCounterDialog(CounterViewModel counterViewModel)
      {
         CounterModel counterModel = _dialogService.ShowCreateCounterDialog($"Edit {counterViewModel.Name}", counterViewModel.CounterModel);
         if (counterModel != null)
         {
            _counters.Remove(counterViewModel);
            _characterModel.Counters.Remove(counterViewModel.CounterModel);

            _counters.Add(new CounterViewModel(counterModel));
            _characterModel.Counters.Add(counterModel);

            _counters = new ObservableCollection<CounterViewModel>(_counters.OrderBy(x => x.Name));

            OnPropertyChanged(nameof(Counters));
            OnPropertyChanged(nameof(ShowCountersHeader));
         }
      }

      private void DeleteCounter(CounterViewModel counterViewModel)
      {
         bool? result = _dialogService.ShowConfirmationDialog("Delete Counter", "Are you sure you want to delete " + counterViewModel.Name + "?", "Yes", "No", null);
         if (result == true)
         {
            _counters.Remove(counterViewModel);
            _characterModel.Counters.Remove(counterViewModel.CounterModel);
            OnPropertyChanged(nameof(Counters));
            OnPropertyChanged(nameof(ShowCountersHeader));
         }
      }

      private void ShowAddConditionDialog()
      {
         AppliedConditionModel appliedConditionModel = _dialogService.ShowCreateAppliedConditionDialog("Add Condition", new AppliedConditionModel());
         if (appliedConditionModel != null)
         {
            _conditions.Add(new AppliedConditionViewModel(appliedConditionModel));
            _characterModel.Conditions.Add(appliedConditionModel);

            _conditions = new ObservableCollection<AppliedConditionViewModel>(_conditions.OrderBy(x => x.Name));

            OnPropertyChanged(nameof(Conditions));
            OnPropertyChanged(nameof(ShowConditionsHeader));
         }
      }

      private void ShowEditConditionDialog(AppliedConditionViewModel appliedCondition)
      {
         AppliedConditionModel appliedConditionModel = _dialogService.ShowCreateAppliedConditionDialog($"Edit {appliedCondition.Name}", appliedCondition.AppliedConditionModel);
         if (appliedConditionModel != null)
         {
            _conditions.Remove(appliedCondition);
            _characterModel.Conditions.Remove(appliedCondition.AppliedConditionModel);

            _conditions.Add(new AppliedConditionViewModel(appliedConditionModel));
            _characterModel.Conditions.Add(appliedConditionModel);

            _conditions = new ObservableCollection<AppliedConditionViewModel>(_conditions.OrderBy(x => x.Name));

            OnPropertyChanged(nameof(Conditions));
            OnPropertyChanged(nameof(ShowConditionsHeader));
         }
      }

      private void RemoveCondition(AppliedConditionViewModel appliedCondition)
      {
         bool? result = _dialogService.ShowConfirmationDialog("Remove Condition", "Are you sure you want to remove " + appliedCondition.Name + "?", "Yes", "No", null);
         if (result == true)
         {
            _characterModel.Conditions.RemoveAll(x => x.ID == appliedCondition.ID);
            _conditions.Remove(appliedCondition);
            OnPropertyChanged(nameof(Conditions));
            OnPropertyChanged(nameof(ShowConditionsHeader));
         }
      }

      private void ShowAddCompanionDialog()
      {
         CompanionModel companionModel = _dialogService.ShowCreateCompanionDialog("Add Companion", new CompanionModel());
         if (companionModel != null)
         {
            companionModel.CurrentHP = companionModel.MaxHP;

            _companions.Add(new CompanionViewModel(companionModel));
            _characterModel.Companions.Add(companionModel);

            _companions = new ObservableCollection<CompanionViewModel>(_companions.OrderBy(x => x.Name));

            OnPropertyChanged(nameof(Companions));
            OnPropertyChanged(nameof(ShowCompanionsHeader));
         }
      }

      private void ShowEditCompanionDialog(CompanionViewModel companionViewModel)
      {
         CompanionModel companionModel = _dialogService.ShowCreateCompanionDialog($"Edit {companionViewModel.Name}", companionViewModel.CompanionModel);
         if (companionModel != null)
         {
            _companions.Remove(companionViewModel);
            _characterModel.Companions.Remove(companionViewModel.CompanionModel);

            companionModel.CurrentHP = Math.Min(companionModel.MaxHP, companionViewModel.CurrentHP);

            _companions.Add(new CompanionViewModel(companionModel));
            _characterModel.Companions.Add(companionModel);

            _companions = new ObservableCollection<CompanionViewModel>(_companions.OrderBy(x => x.Name));

            OnPropertyChanged(nameof(Companions));
            OnPropertyChanged(nameof(ShowCompanionsHeader));
         }
      }

      private void DeleteCompanion(CompanionViewModel companionViewModel)
      {
         bool? result = _dialogService.ShowConfirmationDialog("Delete Companion", "Are you sure you want to delete " + companionViewModel.Name + "?", "Yes", "No", null);
         if (result == true)
         {
            _characterModel.Companions.Remove(companionViewModel.CompanionModel);
            _companions.Remove(companionViewModel);
            OnPropertyChanged(nameof(Companions));
            OnPropertyChanged(nameof(ShowCompanionsHeader));
         }
      }

      private void ShowAddBagDialog()
      {
         BagModel bagModel = _dialogService.ShowCreateBagDialog("Add Bag", new BagModel());
         if (bagModel != null)
         {
            BagViewModel bagViewModel = new BagViewModel(bagModel);
            bagViewModel.PropertyChanged += BagViewModel_PropertyChanged;
            bagViewModel.EquippedChanged += BagViewModel_EquippedChanged;
            _bags.Add(bagViewModel);
            _characterModel.Bags.Add(bagModel);

            _bags = new ObservableCollection<BagViewModel>(_bags.OrderBy(x => x.Name));

            OnPropertyChanged(nameof(Bags));
            UpdateBagTotals();
         }
      }

      private void ShowEditBagDialog(BagViewModel bagViewModel)
      {
         BagModel bagModel = _dialogService.ShowCreateBagDialog($"Edit {bagViewModel.Name}", bagViewModel.BagModel);
         if (bagModel != null)
         {
            _bags.Remove(bagViewModel);
            _characterModel.Bags.Remove(bagViewModel.BagModel);

            BagViewModel newBagViewModel = new BagViewModel(bagModel);
            newBagViewModel.PropertyChanged += BagViewModel_PropertyChanged;
            newBagViewModel.EquippedChanged += BagViewModel_EquippedChanged;
            _bags.Add(newBagViewModel);
            _characterModel.Bags.Add(bagModel);

            _bags = new ObservableCollection<BagViewModel>(_bags.OrderBy(x => x.Name));

            OnPropertyChanged(nameof(Bags));
            UpdateBagTotals();
         }
      }

      private void DeleteBag(BagViewModel bagViewModel)
      {
         bool? result = _dialogService.ShowConfirmationDialog("Delete Bag", "Are you sure you want to delete " + bagViewModel.Name + "?", "Yes", "No", null);
         if (result == true)
         {
            _characterModel.Bags.Remove(bagViewModel.BagModel);
            _bags.Remove(bagViewModel);
            OnPropertyChanged(nameof(Bags));
            UpdateBagTotals();
         }
      }

      private void BagViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         UpdateBagTotals();
      }

      private void BagViewModel_EquippedChanged(object sender, EventArgs e)
      {
         if (sender is EquipmentViewModel equipmentView && equipmentView.Item != null)
         {
            if (equipmentView.Equipped && (equipmentView.Item.Type == ItemType.Melee_Weapon || equipmentView.Item.Type == ItemType.Ranged_Weapon))
            {
               if (!_attacks.Any(x => x.Name.Equals(equipmentView.Name, StringComparison.CurrentCultureIgnoreCase)))
               {
                  bool? result = _dialogService.ShowConfirmationDialog("Create Attack", $"Would you like to create an attack for {equipmentView.Name}?", "Yes", "No", null);
                  if (result == true)
                  {
                     AttackModel attackModel = new AttackModel();

                     attackModel.Name = equipmentView.Item.Name;

                     if (equipmentView.Item.Type == ItemType.Melee_Weapon && !equipmentView.Item.Properties.ToLower().Contains("f"))
                     {
                        attackModel.Ability = Ability.Strength;
                     }
                     else
                     {
                        attackModel.Ability = Ability.Dexterity;
                     }

                     if ((equipmentView.Item.Properties.ToLower().Contains("m") && _characterModel.WeaponProficiency.MartialWeaponsProficiency) ||
                         (!equipmentView.Item.Properties.ToLower().Contains("m") && _characterModel.WeaponProficiency.SimpleWeaponsProficiency) ||
                         (_weaponProficiencies.ToLower().Contains(equipmentView.Item.Name)))
                     {
                        attackModel.Proficient = true;
                     }

                     if (!String.IsNullOrWhiteSpace(equipmentView.Item.Range))
                     {
                        attackModel.Range = equipmentView.Item.Range;
                     }

                     attackModel.DamageType = _stringService.GetEnum<DamageType>(equipmentView.Item.DmgType);
                     attackModel.ShowDamage = true;
                     attackModel.ShowToHit = true;

                     foreach (ModifierModel modifier in equipmentView.Item.Modifiers)
                     {
                        if (modifier.ModifierCategory == ModifierCategory.Bonus)
                        {
                           if (modifier.Text.ToLower().Contains("attack") && modifier.Value != 0)
                           {
                              attackModel.AdditionalToHitBonus += modifier.Value;
                           }
                           else if (modifier.Text.ToLower().Contains("damage") && modifier.Value != 0)
                           {
                              attackModel.AdditionalDamageBonus += modifier.Value;
                           }
                        }
                     }

                     if (!String.IsNullOrWhiteSpace(equipmentView.Item.Dmg1) && !String.IsNullOrWhiteSpace(equipmentView.Item.Dmg2))
                     {
                        string[] damage1 = equipmentView.Item.Dmg1.Split(new char[] { 'd' }, StringSplitOptions.RemoveEmptyEntries);
                        if (damage1.Length == 2)
                        {
                           if (Int32.TryParse(damage1[0], out int dice))
                           {
                              attackModel.NumberOfDamageDice = dice;
                           }
                           attackModel.DamageDie = $"d{damage1[1]}";
                        }

                        _characterModel.Attacks.Add(attackModel);
                        _attacks.Add(CreateAttackViewModel(attackModel));

                        string[] damage2 = equipmentView.Item.Dmg2.Split(new char[] { 'd' }, StringSplitOptions.RemoveEmptyEntries);
                        if (damage2.Length == 2)
                        {
                           AttackModel modelCopy = new AttackModel(attackModel);
                           if (Int32.TryParse(damage2[0], out int dice))
                           {
                              modelCopy.NumberOfDamageDice = dice;
                           }
                           modelCopy.DamageDie = $"d{damage2[1]}";

                           _characterModel.Attacks.Add(modelCopy);
                           _attacks.Add(CreateAttackViewModel(modelCopy));
                        }

                        _attacks = new ObservableCollection<AttackViewModel>(_attacks.OrderBy(x => x.Name));

                        OnPropertyChanged(nameof(Attacks));
                        OnPropertyChanged(nameof(ShowAttacksHeader));
                     }
                     else if (!String.IsNullOrWhiteSpace(equipmentView.Item.Dmg1))
                     {
                        string[] damage1 = equipmentView.Item.Dmg1.Split(new char[] { 'd' }, StringSplitOptions.RemoveEmptyEntries);
                        if (damage1.Length == 2)
                        {
                           if (Int32.TryParse(damage1[0], out int dice))
                           {
                              attackModel.NumberOfDamageDice = dice;
                           }
                           attackModel.DamageDie = $"d{damage1[1]}";
                        }

                        _characterModel.Attacks.Add(attackModel);
                        _attacks.Add(CreateAttackViewModel(attackModel));

                        _attacks = new ObservableCollection<AttackViewModel>(_attacks.OrderBy(x => x.Name));

                        OnPropertyChanged(nameof(Attacks));
                        OnPropertyChanged(nameof(ShowAttacksHeader));
                     }
                  }
               }
            }
            else if (equipmentView.Item.Type == ItemType.Light_Armor || equipmentView.Item.Type == ItemType.Medium_Armor ||
                     equipmentView.Item.Type == ItemType.Heavy_Armor || equipmentView.Item.Type == ItemType.Shield)
            {
               UpdateAC();
            }
            else
            {
               foreach (ModifierModel modifierModel in equipmentView.Item.Modifiers)
               {
                  if (modifierModel.ModifierCategory == ModifierCategory.Bonus &&
                     (modifierModel.Text.ToLower().Contains("ac ") || modifierModel.Text.ToLower() == "ac"))
                  {
                     UpdateAC();
                     break;
                  }
               }
            }

            _totalAttunedItems = 0;
            foreach (BagViewModel bagViewModel in _bags)
            {
               foreach (EquipmentViewModel equipment in bagViewModel.Equipment.Where(x => x.Equipped))
               {
                  if (equipment.Item != null)
                  {
                     if (equipment.Item.RequiresAttunement)
                     {
                        _totalAttunedItems += equipment.Quantity;
                     }
                  }
               }
            }

            OnPropertyChanged(nameof(TotalAttunedItems));
            OnPropertyChanged(nameof(EquippedItems));
         }
      }

      private void UpdateBagTotals()
      {
         _totalWeight = 0.0f;
         _totalAttunedItems = 0;
         _totalCopper = 0;
         _totalSilver = 0;
         _totalElectrum = 0;
         _totalGold = 0;
         _totalPlatinum = 0;

         foreach (BagViewModel bagViewModel in _bags)
         {
            _totalWeight += bagViewModel.TotalWeight;
            _totalCopper += bagViewModel.Copper;
            _totalSilver += bagViewModel.Silver;
            _totalElectrum += bagViewModel.Electrum;
            _totalGold += bagViewModel.Gold;
            _totalPlatinum += bagViewModel.Platinum;

            foreach (EquipmentViewModel equipmentView in bagViewModel.Equipment.Where(x => x.Equipped))
            {
               if (equipmentView.Item != null)
               {
                  if (equipmentView.Item.RequiresAttunement)
                  {
                     _totalAttunedItems += equipmentView.Quantity;
                  }
               }
            }
         }

         if (_movementViewModel.ApplyEncumbrance)
         {
            UpdateEncumbrance();

            OnPropertyChanged(nameof(WalkSpeed));
            OnPropertyChanged(nameof(SwimSpeed));
            OnPropertyChanged(nameof(ClimbSpeed));
            OnPropertyChanged(nameof(CrawlSpeed));
            OnPropertyChanged(nameof(FlySpeed));
         }

         OnPropertyChanged(nameof(TotalWeight));
         OnPropertyChanged(nameof(TotalAttunedItems));
         OnPropertyChanged(nameof(TotalCopper));
         OnPropertyChanged(nameof(TotalSilver));
         OnPropertyChanged(nameof(TotalElectrum));
         OnPropertyChanged(nameof(TotalGold));
         OnPropertyChanged(nameof(TotalPlatinum));
         OnPropertyChanged(nameof(EquippedItems));
      }

      private void UpdateAC()
      {
         int baseArmor = 10;
         int armorBonus = 0;

         _armorClassViewModel.ArmorBonus = 10;
         _armorClassViewModel.ArmorType = ArmorType.None;
         _armorClassViewModel.FirstAbility = Ability.Dexterity;
         _armorClassViewModel.SecondAbility = Ability.None;
         _armorClassViewModel.ItemBonus = 0;
         _armorClassViewModel.AdditionalBonus = 0;

         foreach (BagViewModel bagViewModel in _bags)
         {
            foreach (EquipmentViewModel equipmentView in bagViewModel.Equipment.Where(x => x.Equipped))
            {
               if (equipmentView.Item != null)
               {
                  if (equipmentView.Item.Type == ItemType.Light_Armor ||
                      equipmentView.Item.Type == ItemType.Medium_Armor ||
                      equipmentView.Item.Type == ItemType.Heavy_Armor)
                  {
                     if (Int32.TryParse(equipmentView.Item.AC, out int ac))
                     {
                        baseArmor = ac;

                        if (equipmentView.Item.Type == ItemType.Light_Armor)
                        {
                           _armorClassViewModel.ArmorType = ArmorType.Light_Armor;
                           _armorClassViewModel.FirstAbility = Ability.Dexterity;
                           _armorClassViewModel.SecondAbility = Ability.None;
                        }
                        else if (equipmentView.Item.Type == ItemType.Medium_Armor)
                        {
                           _armorClassViewModel.ArmorType = ArmorType.Medium_Armor;
                           _armorClassViewModel.FirstAbility = Ability.Dexterity;
                           _armorClassViewModel.SecondAbility = Ability.None;
                        }
                        else if (equipmentView.Item.Type == ItemType.Heavy_Armor)
                        {
                           _armorClassViewModel.ArmorType = ArmorType.Heavy_Armor;
                           _armorClassViewModel.FirstAbility = Ability.None;
                           _armorClassViewModel.SecondAbility = Ability.None;
                        }
                     }

                     foreach (ModifierModel modifierModel in equipmentView.Item.Modifiers)
                     {
                        if (modifierModel.ModifierCategory == ModifierCategory.Bonus &&
                            modifierModel.Text.ToLower().Contains("ac"))
                        {
                           armorBonus += modifierModel.Value;
                        }
                     }
                  }
                  else if (equipmentView.Item.Type == ItemType.Shield)
                  {
                     if (Int32.TryParse(equipmentView.Item.AC, out int ac))
                     {
                        armorBonus += ac;
                     }

                     foreach (ModifierModel modifierModel in equipmentView.Item.Modifiers)
                     {
                        if (modifierModel.ModifierCategory == ModifierCategory.Bonus &&
                            modifierModel.Text.ToLower().Contains("ac"))
                        {
                           armorBonus += modifierModel.Value;
                        }
                     }
                  }
                  else
                  {
                     foreach (ModifierModel modifierModel in equipmentView.Item.Modifiers)
                     {
                        if (modifierModel.ModifierCategory == ModifierCategory.Bonus &&
                           (modifierModel.Text.ToLower().Contains("ac ") || modifierModel.Text.ToLower() == "ac"))
                        {
                           _armorClassViewModel.ItemBonus += modifierModel.Value;
                        }
                     }
                  }
               }
            }
         }

         _armorClassViewModel.ArmorBonus = baseArmor + armorBonus;

         CalculateArmorClass();

         OnPropertyChanged(nameof(AC));
      }

      private void ShowAddSpellbookDialog()
      {
         SpellbookModel model = new SpellbookModel();
         LevelViewModel levelView = _levels.LastOrDefault(x => x.Class != null && x.Class.ClassModel != null && x.Class.ClassModel.SpellAbility != Ability.None);
         if (levelView != null)
         {
            model.Class = levelView.Class.ClassModel;
            model.Ability = levelView.Class.ClassModel.SpellAbility;
         }

         SpellbookModel spellbookModel = _dialogService.ShowCreateSpellbookDialog("Add Spellbook", model);
         if (spellbookModel != null)
         {
            SpellbookViewModel spellbookViewModel = CreateSpellbookViewModel(spellbookModel);
            _spellbooks.Add(spellbookViewModel);
            _characterModel.Spellbooks.Add(spellbookModel);

            _spellbooks = new ObservableCollection<SpellbookViewModel>(_spellbooks.OrderBy(x => x.Name));

            OnPropertyChanged(nameof(Spellbooks));
         }
      }

      private void ShowEditSpellbookDialog(SpellbookViewModel spellbookViewModel)
      {
         SpellbookModel spellbookModel = _dialogService.ShowCreateSpellbookDialog($"Edit {spellbookViewModel.Name}", spellbookViewModel.SpellbookModel);
         if (spellbookModel != null)
         {
            _spellbooks.Remove(spellbookViewModel);
            _characterModel.Spellbooks.Remove(spellbookViewModel.SpellbookModel);
            SpellbookViewModel newSpellbookViewModel = CreateSpellbookViewModel(spellbookModel);
            _spellbooks.Add(newSpellbookViewModel);
            _characterModel.Spellbooks.Add(spellbookModel);

            _spellbooks = new ObservableCollection<SpellbookViewModel>(_spellbooks.OrderBy(x => x.Name));

            OnPropertyChanged(nameof(Spellbooks));
         }
      }

      private void DeleteSpellbook(SpellbookViewModel spellbookViewModel)
      {
         bool? result = _dialogService.ShowConfirmationDialog("Delete Spellbook", "Are you sure you want to delete " + spellbookViewModel.Name + "?", "Yes", "No", null);
         if (result == true)
         {
            _characterModel.Spellbooks.Remove(spellbookViewModel.SpellbookModel);
            _spellbooks.Remove(spellbookViewModel);
            OnPropertyChanged(nameof(Spellbooks));
         }
      }

      private void ShowAddStatModificationDialog()
      {
         StatModificationModel statModification = _dialogService.ShowCreatetatModificationDialog("Add Stat Modification", new StatModificationModel());
         if (statModification != null)
         {
            _statModifications.Add(new StatModificationViewModel(statModification));
            _characterModel.StatModifications.Add(statModification);

            _statModifications = new ObservableCollection<StatModificationViewModel>(_statModifications.OrderBy(x => x.ModificationOptionDisplay));

            ApplyStatModifications();

            OnPropertyChanged(nameof(StatModifications));
            OnPropertyChanged(nameof(ShowStatModificationsHeader));
         }
      }

      private void ShowEditStatModificationDialog(StatModificationViewModel statModificationViewModel)
      {
         StatModificationModel statModification = _dialogService.ShowCreatetatModificationDialog("Edit Stat Modification", statModificationViewModel.StatModificationModel);
         if (statModification != null)
         {
            _statModifications.Remove(statModificationViewModel);
            _characterModel.StatModifications.Remove(statModificationViewModel.StatModificationModel);

            _statModifications.Add(new StatModificationViewModel(statModification));
            _characterModel.StatModifications.Add(statModification);

            _statModifications = new ObservableCollection<StatModificationViewModel>(_statModifications.OrderBy(x => x.ModificationOptionDisplay));

            ApplyStatModifications();

            OnPropertyChanged(nameof(StatModifications));
            OnPropertyChanged(nameof(ShowStatModificationsHeader));
         }
      }

      private void DeleteStatModification(StatModificationViewModel statModificationViewModel)
      {
         bool? result = _dialogService.ShowConfirmationDialog("Delete Stat Modification", "Are you sure you want to delete " + _stringService.GetString(statModificationViewModel.ModificationOption) + "?", "Yes", "No", null);
         if (result == true)
         {
            _characterModel.StatModifications.Remove(statModificationViewModel.StatModificationModel);
            _statModifications.Remove(statModificationViewModel);

            ApplyStatModifications();

            switch (statModificationViewModel.ModificationOption)
            {
               case StatModificationOption.Strength:
                  OnPropertyChanged(nameof(TotalStrength));
                  OnPropertyChanged(nameof(StrengthDisplay));
                  OnPropertyChanged(nameof(CarryingCapacity));
                  break;

               case StatModificationOption.Dexterity:
                  OnPropertyChanged(nameof(TotalDexterity));
                  OnPropertyChanged(nameof(DexterityDisplay));
                  OnPropertyChanged(nameof(InitiativeDisplay));
                  break;

               case StatModificationOption.Constitution:
                  OnPropertyChanged(nameof(TotalConstitution));
                  OnPropertyChanged(nameof(ConstitutionDisplay));
                  break;

               case StatModificationOption.Intelligence:
                  OnPropertyChanged(nameof(TotalIntelligence));
                  OnPropertyChanged(nameof(IntelligenceDisplay));
                  OnPropertyChanged(nameof(PassiveInvestigation));
                  break;

               case StatModificationOption.Wisdom:
                  OnPropertyChanged(nameof(TotalWisdom));
                  OnPropertyChanged(nameof(WisdomDisplay));
                  OnPropertyChanged(nameof(PassivePerception));
                  break;

               case StatModificationOption.Charisma:
                  OnPropertyChanged(nameof(TotalCharisma));
                  OnPropertyChanged(nameof(CharismaDisplay));
                  break;

               case StatModificationOption.Initiative:
                  OnPropertyChanged(nameof(InitiativeDisplay));
                  break;

               case StatModificationOption.PassivePerception:
                  OnPropertyChanged(nameof(PassivePerception));
                  break;

               case StatModificationOption.PassiveInvestigation:
                  OnPropertyChanged(nameof(PassiveInvestigation));
                  break;
            }

            OnPropertyChanged(nameof(StatModifications));
            OnPropertyChanged(nameof(ShowStatModificationsHeader));
         }
      }

      private void ViewFeat(FeatViewModel feat)
      {
         if (feat != null)
         {
            _dialogService.ShowDetailsDialog(feat);
         }
      }

      private void ViewFeature(FeatureViewModel feature)
      {
         if (feature != null)
         {
            _dialogService.ShowDetailsDialog(feature);
         }
      }

      private void ShowConvertMoneyDialog(BagViewModel bagView)
      {
         BagModel bagModel = _dialogService.ShowConvertMoneyDialog(bagView.BagModel);
         if (bagModel != null)
         {
            bagView.Copper = bagModel.Copper;
            bagView.Silver = bagModel.Silver;
            bagView.Electrum = bagModel.Electrum;
            bagView.Gold = bagModel.Gold;
            bagView.Platinum = bagModel.Platinum;
         }
      }

      private void ShowLevelUpDialog()
      {
         LevelModel levelModel = _dialogService.ShowLevelUpDialog(_classesMap, _levels.Count + 1);
         if (levelModel != null)
         {
            _characterModel.Levels.Add(levelModel);
            _levels.Add(new LevelViewModel(levelModel));

            KeyValuePair<Guid, string> pair = new KeyValuePair<Guid, string>(levelModel.Class.ID, levelModel.Class.Name);

            if (_classesMap.ContainsKey(pair))
            {
               _classesMap[pair]++;
            }
            else
            {
               _classesMap[pair] = 1;
            }

            _classDisplay = String.Join(", ", _classesMap.Select(x => $"{x.Key.Value} {x.Value}"));

            _classes.Clear();
            foreach (KeyValuePair<KeyValuePair<Guid, string>, int> mapPair in _classesMap)
            {
               ClassModel classModel = _compendium.Classes.FirstOrDefault(x => x.ID == mapPair.Key.Key);
               if (classModel != null)
               {
                  _classes.Add(new KeyValuePair<ClassViewModel, int>(new ClassViewModel(classModel), mapPair.Value));
               }
            }

            _maxHP = 0;
            int conBonus = _statService.GetStatBonus(_totalConstitution);
            foreach (LevelModel level in _characterModel.Levels)
            {
               _maxHP += level.HitDieResult + conBonus + level.AdditionalHP;
            }

            foreach (FeatureModel featureModel in levelModel.Features)
            {
               _features.Add(new FeatureViewModel(featureModel));
            }

            if (levelModel.Feats.Any())
            {
               foreach (FeatModel featModel in levelModel.Feats)
               {
                  _feats.Add(new FeatViewModel(featModel));
               }
               _feats = new ObservableCollection<FeatViewModel>(_feats.OrderBy(x => x.Name));
            }

            foreach (SpellbookViewModel spellbookViewModel in _spellbooks)
            {
               UpdateSpellbookSlots(spellbookViewModel);
            }

            if (levelModel.AbilityScoreImprovements.Any())
            {
               ApplyStatModifications();

               OnPropertyChanged(nameof(WalkSpeed));
               OnPropertyChanged(nameof(SwimSpeed));
               OnPropertyChanged(nameof(ClimbSpeed));
               OnPropertyChanged(nameof(CrawlSpeed));
               OnPropertyChanged(nameof(FlySpeed));
               OnPropertyChanged(nameof(AC));
               OnPropertyChanged(nameof(TotalStrength));
               OnPropertyChanged(nameof(StrengthDisplay));
               OnPropertyChanged(nameof(CarryingCapacity));
               OnPropertyChanged(nameof(TotalDexterity));
               OnPropertyChanged(nameof(DexterityDisplay));
               OnPropertyChanged(nameof(InitiativeDisplay));
               OnPropertyChanged(nameof(TotalConstitution));
               OnPropertyChanged(nameof(ConstitutionDisplay));
               OnPropertyChanged(nameof(TotalIntelligence));
               OnPropertyChanged(nameof(IntelligenceDisplay));
               OnPropertyChanged(nameof(PassiveInvestigation));
               OnPropertyChanged(nameof(TotalWisdom));
               OnPropertyChanged(nameof(WisdomDisplay));
               OnPropertyChanged(nameof(PassivePerception));
               OnPropertyChanged(nameof(TotalCharisma));
               OnPropertyChanged(nameof(CharismaDisplay));
               OnPropertyChanged(nameof(AbilityProficiencies));
               OnPropertyChanged(nameof(SkillProficiencies));
            }

            OnPropertyChanged(nameof(Level));
            OnPropertyChanged(nameof(NextLevel));
            OnPropertyChanged(nameof(ClassDisplay));
            OnPropertyChanged(nameof(CurrentHP));
            OnPropertyChanged(nameof(LessHP));
            OnPropertyChanged(nameof(HPDisplay));
            OnPropertyChanged(nameof(TempHP));
            OnPropertyChanged(nameof(Experience));
            OnPropertyChanged(nameof(ExperienceAtCurrentLevel));
            OnPropertyChanged(nameof(LessExperience));
            OnPropertyChanged(nameof(ExperienceDisplay));
            OnPropertyChanged(nameof(LevelUpPending));

            LevelUp?.Invoke(this, EventArgs.Empty);
         }
      }

      private void ShowDeathSavesDialog()
      {
         (int?, int?) result = _dialogService.ShowDeathSavesDialog(_characterModel.DeathSaveSuccesses, _characterModel.DeathSaveFailures);
         if (result.Item1.HasValue && result.Item2.HasValue)
         {
            _characterModel.DeathSaveSuccesses = result.Item1.Value;
            _characterModel.DeathSaveFailures = result.Item2.Value;
         }
      }

      private void ToggleInspiration()
      {
         _inspiration = !_inspiration;
         _characterModel.Inspiration = _inspiration;
         OnPropertyChanged(nameof(Inspiration));
      }

      private void SelectStatsTab()
      {
         _statsTabSelected = true;
         _combatTabSelected = false;
         _inventoryAndEquipmentTabSelected = false;
         _spellcastingTabSelected = false;
         _informationTabSelected = false;

         OnPropertyChanged(nameof(StatsTabSelected));
         OnPropertyChanged(nameof(CombatTabSelected));
         OnPropertyChanged(nameof(InventoryAndEquipmentTabSelected));
         OnPropertyChanged(nameof(SpellcastingTabSelected));
         OnPropertyChanged(nameof(InformationTabSelected));
      }

      private void SelectCombatTab()
      {
         _statsTabSelected = false;
         _combatTabSelected = true;
         _inventoryAndEquipmentTabSelected = false;
         _spellcastingTabSelected = false;
         _informationTabSelected = false;

         OnPropertyChanged(nameof(StatsTabSelected));
         OnPropertyChanged(nameof(CombatTabSelected));
         OnPropertyChanged(nameof(InventoryAndEquipmentTabSelected));
         OnPropertyChanged(nameof(SpellcastingTabSelected));
         OnPropertyChanged(nameof(InformationTabSelected));
      }

      private void SelectInventoryandEquipmentTab()
      {
         _statsTabSelected = false;
         _combatTabSelected = false;
         _inventoryAndEquipmentTabSelected = true;
         _spellcastingTabSelected = false;
         _informationTabSelected = false;

         OnPropertyChanged(nameof(StatsTabSelected));
         OnPropertyChanged(nameof(CombatTabSelected));
         OnPropertyChanged(nameof(InventoryAndEquipmentTabSelected));
         OnPropertyChanged(nameof(SpellcastingTabSelected));
         OnPropertyChanged(nameof(InformationTabSelected));
      }

      private void SelectSpellcastingTab()
      {
         _statsTabSelected = false;
         _combatTabSelected = false;
         _inventoryAndEquipmentTabSelected = false;
         _spellcastingTabSelected = true;
         _informationTabSelected = false;

         OnPropertyChanged(nameof(StatsTabSelected));
         OnPropertyChanged(nameof(CombatTabSelected));
         OnPropertyChanged(nameof(InventoryAndEquipmentTabSelected));
         OnPropertyChanged(nameof(SpellcastingTabSelected));
         OnPropertyChanged(nameof(InformationTabSelected));
      }

      private void SelectInformationTab()
      {
         _statsTabSelected = false;
         _combatTabSelected = false;
         _inventoryAndEquipmentTabSelected = false;
         _spellcastingTabSelected = false;
         _informationTabSelected = true;

         OnPropertyChanged(nameof(StatsTabSelected));
         OnPropertyChanged(nameof(CombatTabSelected));
         OnPropertyChanged(nameof(InventoryAndEquipmentTabSelected));
         OnPropertyChanged(nameof(SpellcastingTabSelected));
         OnPropertyChanged(nameof(InformationTabSelected));
      }

      #endregion
   }
}
