using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class CharacterModel
   {
      #region Fields

      private Guid _id;
      private string _name;
      private int _currentHP;
      private int _tempHP;
      private int _deathSaveSuccesses;
      private int _deathSaveFailures;
      private RaceModel _race;
      private BackgroundModel _background;
      private Alignment _alignment;
      private MovementModel _movementModel = new MovementModel();
      private bool _inspiration;
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
      private int _baseStrength = 8;
      private int _baseDexterity = 8;
      private int _baseConstitution = 8;
      private int _baseIntelligence = 8;
      private int _baseWisdom = 8;
      private int _baseCharisma = 8;
      private int _bonusStrength = 0;
      private int _bonusDexterity = 0;
      private int _bonusConstitution = 0;
      private int _bonusIntelligence = 0;
      private int _bonusWisdom = 0;
      private int _bonusCharisma = 0;
      private string _personalityTraits;
      private string _ideals;
      private string _bonds;
      private string _flaws;
      private string _backstory;
      private List<LevelModel> _levels = new List<LevelModel>();
      private AbilityRollMethod _abilityRollMethod;
      private int _timesAbilityScoresRolled;
      private List<AbilityModel> _abilitySaveProficiencies = new List<AbilityModel>();
      private List<SkillModel> _skillProficiencies = new List<SkillModel>();
      private List<LanguageModel> _languages = new List<LanguageModel>();
      private ArmorProficiencyModel _armorProficiencyModel = new ArmorProficiencyModel();
      private WeaponProficiencyModel _weaponProficiencyModel = new WeaponProficiencyModel();
      private ToolProficiencyModel _toolProficiencyModel = new ToolProficiencyModel();
      private List<AppliedConditionModel> _conditions = new List<AppliedConditionModel>();
      private string _savingThrowNotes;
      private List<AttackModel> _attacks = new List<AttackModel>();
      private List<CounterModel> _counters = new List<CounterModel>();
      private List<CompanionModel> _companions = new List<CompanionModel>();
      private List<BagModel> _bags = new List<BagModel>();
      private List<SpellbookModel> _spellbooks = new List<SpellbookModel>();
      private List<StatModificationModel> _statModifications = new List<StatModificationModel>();
      private ArmorClassModel _armorClassModel = new ArmorClassModel();

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a new instance of <see cref="CharacterModel"/>
      /// </summary>
      public CharacterModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates a copy of <see cref="CharacterModel"/>
      /// </summary>
      public CharacterModel(CharacterModel characterModel)
      {
         _id = characterModel.ID;
         _name = characterModel.Name;
         _currentHP = characterModel.CurrentHP;
         _tempHP = characterModel.TempHP;
         _deathSaveSuccesses = characterModel.DeathSaveSuccesses;
         _deathSaveFailures = characterModel.DeathSaveFailures;
         _race = characterModel.Race != null ? new RaceModel(characterModel.Race) : null;
         _background = characterModel.Background != null ? new BackgroundModel(characterModel.Background) : null;
         _alignment = characterModel.Alignment;
         _movementModel = new MovementModel(characterModel.MovementModel);
         _inspiration = characterModel.Inspiration;
         _deity = characterModel.Deity;
         _gender = characterModel.Gender;
         _age = characterModel.Age;
         _heightFeet = characterModel.HeightFeet;
         _heightInches = characterModel.HeightInches;
         _weight = characterModel.Weight;
         _hair = characterModel.Hair;
         _eyes = characterModel.Eyes;
         _skin = characterModel.Skin;
         _experience = characterModel.Experience;

         _baseStrength = characterModel.BaseStrength;
         _baseDexterity = characterModel.BaseDexterity;
         _baseConstitution = characterModel.BaseConstitution;
         _baseIntelligence = characterModel.BaseIntelligence;
         _baseWisdom = characterModel.BaseWisdom;
         _baseCharisma = characterModel.BaseCharisma;

         _bonusStrength = characterModel.BonusStrength;
         _bonusDexterity = characterModel.BonusDexterity;
         _bonusConstitution = characterModel.BonusConstitution;
         _bonusIntelligence = characterModel.BonusIntelligence;
         _bonusWisdom = characterModel.BonusWisdom;
         _bonusCharisma = characterModel.BonusCharisma;

         _personalityTraits = characterModel.PersonalityTraits;
         _ideals = characterModel.Ideals;
         _bonds = characterModel.Bonds;
         _flaws = characterModel.Flaws;
         _backstory = characterModel.Backstory;

         _levels = new List<LevelModel>();
         foreach (LevelModel levelModel in characterModel.Levels)
         {
            _levels.Add(new LevelModel(levelModel));
         }
         _abilityRollMethod = characterModel.AbilityRollMethod;
         _timesAbilityScoresRolled = characterModel.TimesAbilityScoresRolled;

         _abilitySaveProficiencies = new List<AbilityModel>();
         foreach (AbilityModel abilityModel in characterModel.AbilitySaveProficiencies)
         {
            _abilitySaveProficiencies.Add(new AbilityModel(abilityModel));
         }

         _skillProficiencies = new List<SkillModel>();
         foreach (SkillModel skillModel in characterModel.SkillProficiencies)
         {
            _skillProficiencies.Add(new SkillModel(skillModel));
         }

         _languages = new List<LanguageModel>();
         foreach (LanguageModel languageModel in characterModel.Languages)
         {
            _languages.Add(new LanguageModel(languageModel));
         }

         _armorProficiencyModel = new ArmorProficiencyModel(characterModel.ArmorProficiency);
         _weaponProficiencyModel = new WeaponProficiencyModel(characterModel.WeaponProficiency);
         _toolProficiencyModel = new ToolProficiencyModel(characterModel.ToolProficiency);

         _conditions = new List<AppliedConditionModel>();
         foreach (AppliedConditionModel appliedCondition in characterModel.Conditions)
         {
            _conditions.Add(new AppliedConditionModel(appliedCondition));
         }

         _savingThrowNotes = characterModel.SavingThrowNotes;

         _attacks = new List<AttackModel>();
         foreach (AttackModel attackModel in characterModel.Attacks)
         {
            _attacks.Add(new AttackModel(attackModel));
         }

         _counters = new List<CounterModel>();
         foreach (CounterModel counter in characterModel.Counters)
         {
            _counters.Add(new CounterModel(counter));
         }

         _companions = new List<CompanionModel>();
         foreach (CompanionModel companion in characterModel.Companions)
         {
            _companions.Add(new CompanionModel(companion));
         }

         _bags = new List<BagModel>();
         foreach (BagModel bag in characterModel.Bags)
         {
            _bags.Add(new BagModel(bag));
         }

         _spellbooks = new List<SpellbookModel>();
         foreach (SpellbookModel spellbook in characterModel.Spellbooks)
         {
            _spellbooks.Add(new SpellbookModel(spellbook));
         }

         _statModifications = new List<StatModificationModel>();
         foreach (StatModificationModel statModification in characterModel.StatModifications)
         {
            _statModifications.Add(new StatModificationModel(statModification));
         }

         _armorClassModel = new ArmorClassModel(characterModel.ArmorClassModel);
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
      /// Gets or sets current hp
      /// </summary>
      public int CurrentHP
      {
         get { return _currentHP; }
         set { _currentHP = value; }
      }

      /// <summary>
      /// Gets or sets temp hp
      /// </summary>
      public int TempHP
      {
         get { return _tempHP; }
         set { _tempHP = value; }
      }

      /// <summary>
      /// Gets or sets death save successes
      /// </summary>
      public int DeathSaveSuccesses
      {
         get { return _deathSaveSuccesses; }
         set { _deathSaveSuccesses = value; }
      }

      /// <summary>
      /// Gets or sets death save failures
      /// </summary>
      public int DeathSaveFailures
      {
         get { return _deathSaveFailures; }
         set { _deathSaveFailures = value; }
      }

      /// <summary>
      /// Gets or sets race
      /// </summary>
      public RaceModel Race
      {
         get { return _race; }
         set { _race = value; }
      }

      /// <summary>
      /// Gets or sets background
      /// </summary>
      public BackgroundModel Background
      {
         get { return _background; }
         set { _background = value; }
      }

      /// <summary>
      /// Gets or sets alignment
      /// </summary>
      public Alignment Alignment
      {
         get { return _alignment; }
         set { _alignment = value; }
      }

      /// <summary>
      /// Gets or sets movement model
      /// </summary>
      public MovementModel MovementModel
      {
         get { return _movementModel; }
         set { _movementModel = value; }
      }

      /// <summary>
      /// Gets or sets inspiration
      /// </summary>
      public bool Inspiration
      {
         get { return _inspiration; }
         set { _inspiration = value; }
      }

      /// <summary>
      /// Gets or sets deity
      /// </summary>
      public string Deity
      {
         get { return _deity; }
         set { _deity = value; }
      }

      /// <summary>
      /// Gets or sets gender
      /// </summary>
      public string Gender
      {
         get { return _gender; }
         set { _gender = value; }
      }

      /// <summary>
      /// Gets or sets age
      /// </summary>
      public int Age
      {
         get { return _age; }
         set { _age = value; }
      }

      /// <summary>
      /// Gets or sets height feet
      /// </summary>
      public int HeightFeet
      {
         get { return _heightFeet; }
         set { _heightFeet = value; }
      }

      /// <summary>
      /// Gets or sets height inches
      /// </summary>
      public int HeightInches
      {
         get { return _heightInches; }
         set { _heightInches = value; }
      }

      /// <summary>
      /// Gets or sets weight
      /// </summary>
      public int Weight
      {
         get { return _weight; }
         set { _weight = value; }
      }

      /// <summary>
      /// Gets or sets hair
      /// </summary>
      public string Hair
      {
         get { return _hair; }
         set { _hair = value; }
      }

      /// <summary>
      /// Gets or sets eyes
      /// </summary>
      public string Eyes
      {
         get { return _eyes; }
         set { _eyes = value; }
      }

      /// <summary>
      /// Gets or sets skin
      /// </summary>
      public string Skin
      {
         get { return _skin; }
         set { _skin = value; }
      }

      /// <summary>
      /// Gets or sets experience
      /// </summary>
      public int Experience
      {
         get { return _experience; }
         set { _experience = value; }
      }

      /// <summary>
      /// Gets or sets base strength
      /// </summary>
      public int BaseStrength
      {
         get { return _baseStrength; }
         set { _baseStrength = value; }
      }

      /// <summary>
      /// Gets or sets base dexterity
      /// </summary>
      public int BaseDexterity
      {
         get { return _baseDexterity; }
         set { _baseDexterity = value; }
      }

      /// <summary>
      /// Gets or sets base constitution
      /// </summary>
      public int BaseConstitution
      {
         get { return _baseConstitution; }
         set { _baseConstitution = value; }
      }

      /// <summary>
      /// Gets or sets base intelligence
      /// </summary>
      public int BaseIntelligence
      {
         get { return _baseIntelligence; }
         set { _baseIntelligence = value; }
      }

      /// <summary>
      /// Gets or sets base wisdom
      /// </summary>
      public int BaseWisdom
      {
         get { return _baseWisdom; }
         set { _baseWisdom = value; }
      }

      /// <summary>
      /// Gets or sets base charisma
      /// </summary>
      public int BaseCharisma
      {
         get { return _baseCharisma; }
         set { _baseCharisma = value; }
      }

      /// <summary>
      /// Gets or sets bonus strength
      /// </summary>
      public int BonusStrength
      {
         get { return _bonusStrength; }
         set { _bonusStrength = value; }
      }

      /// <summary>
      /// Gets or sets bonus dexterity
      /// </summary>
      public int BonusDexterity
      {
         get { return _bonusDexterity; }
         set { _bonusDexterity = value; }
      }

      /// <summary>
      /// Gets or sets bonus constitution
      /// </summary>
      public int BonusConstitution
      {
         get { return _bonusConstitution; }
         set { _bonusConstitution = value; }
      }

      /// <summary>
      /// Gets or sets bonus intelligence
      /// </summary>
      public int BonusIntelligence
      {
         get { return _bonusIntelligence; }
         set { _bonusIntelligence = value; }
      }

      /// <summary>
      /// Gets or sets bonus wisdom
      /// </summary>
      public int BonusWisdom
      {
         get { return _bonusWisdom; }
         set { _bonusWisdom = value; }
      }

      /// <summary>
      /// Gets or sets bonus charisma
      /// </summary>
      public int BonusCharisma
      {
         get { return _bonusCharisma; }
         set { _bonusCharisma = value; }
      }

      /// <summary>
      /// Gets or sets personality traits
      /// </summary>
      public string PersonalityTraits
      {
         get { return _personalityTraits; }
         set { _personalityTraits = value; }
      }

      /// <summary>
      /// Gets or sets ideals
      /// </summary>
      public string Ideals
      {
         get { return _ideals; }
         set { _ideals = value; }
      }

      /// <summary>
      /// Gets or sets bonds
      /// </summary>
      public string Bonds
      {
         get { return _bonds; }
         set { _bonds = value; }
      }

      /// <summary>
      /// Gets or sets flaws
      /// </summary>
      public string Flaws
      {
         get { return _flaws; }
         set { _flaws = value; }
      }

      /// <summary>
      /// Gets or sets backstory
      /// </summary>
      public string Backstory
      {
         get { return _backstory; }
         set { _backstory = value; }
      }

      /// <summary>
      /// Gets or sets levels
      /// </summary>
      public List<LevelModel> Levels
      {
         get { return _levels; }
         set { _levels = value; }
      }

      /// <summary>
      /// Gets or sets score roll method
      /// </summary>
      public AbilityRollMethod AbilityRollMethod
      {
         get { return _abilityRollMethod; }
         set { _abilityRollMethod = value; }
      }

      /// <summary>
      /// Gets or sets times ability scores rolled
      /// </summary>
      public int TimesAbilityScoresRolled
      {
         get { return _timesAbilityScoresRolled; }
         set { _timesAbilityScoresRolled = value; }
      }

      /// <summary>
      /// Gets or sets ability save proficiencies
      /// </summary>
      public List<AbilityModel> AbilitySaveProficiencies
      {
         get { return _abilitySaveProficiencies; }
         set { _abilitySaveProficiencies = value; }
      }

      /// <summary>
      /// Gets or sets skill proficiencies
      /// </summary>
      public List<SkillModel> SkillProficiencies
      {
         get { return _skillProficiencies; }
         set { _skillProficiencies = value; }
      }

      /// <summary>
      /// Gets or sets languages
      /// </summary>
      public List<LanguageModel> Languages
      {
         get { return _languages; }
         set { _languages = value; }
      }

      /// <summary>
      /// Gets or sets armor proficiency
      /// </summary>
      public ArmorProficiencyModel ArmorProficiency
      {
         get { return _armorProficiencyModel; }
         set { _armorProficiencyModel = value; }
      }

      /// <summary>
      /// Gets or sets weapon proficiency
      /// </summary>
      public WeaponProficiencyModel WeaponProficiency
      {
         get { return _weaponProficiencyModel; }
         set { _weaponProficiencyModel = value; }
      }

      /// <summary>
      /// Gets or sets tool proficiency
      /// </summary>
      public ToolProficiencyModel ToolProficiency
      {
         get { return _toolProficiencyModel; }
         set { _toolProficiencyModel = value; }
      }

      /// <summary>
      /// Gets or sets conditions
      /// </summary>
      public List<AppliedConditionModel> Conditions
      {
         get { return _conditions; }
         set { _conditions = value; }
      }

      /// <summary>
      /// Gets or sets saving throw notes
      /// </summary>
      public string SavingThrowNotes
      {
         get { return _savingThrowNotes; }
         set { _savingThrowNotes = value; }
      }

      /// <summary>
      /// Gets or sets attack models
      /// </summary>
      public List<AttackModel> Attacks
      {
         get { return _attacks; }
         set { _attacks = value; }
      }

      /// <summary>
      /// Gets or sets counters
      /// </summary>
      public List<CounterModel> Counters
      {
         get { return _counters; }
         set { _counters = value; }
      }

      /// <summary>
      /// Gets or sets companions
      /// </summary>
      public List<CompanionModel> Companions
      {
         get { return _companions; }
         set { _companions = value; }
      }

      /// <summary>
      /// Gets or sets bags
      /// </summary>
      public List<BagModel> Bags
      {
         get { return _bags; }
         set { _bags = value; }
      }

      /// <summary>
      /// Gets or sets spellbooks
      /// </summary>
      public List<SpellbookModel> Spellbooks
      {
         get { return _spellbooks; }
         set { _spellbooks = value; }
      }

      /// <summary>
      /// Gets or sets stat modifications
      /// </summary>
      public List<StatModificationModel> StatModifications
      {
         get { return _statModifications; }
         set { _statModifications = value; }
      }

      /// <summary>
      /// Gets or sets armor class model
      /// </summary>
      public ArmorClassModel ArmorClassModel
      {
         get { return _armorClassModel; }
         set { _armorClassModel = value; }
      }

      #endregion
   }
}
