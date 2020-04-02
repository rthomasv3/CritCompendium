using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to save a character information.
   /// </summary>
   public sealed class CharacterRecord : CompendiumEntryRecord
   {
      /// <summary>
      /// Gets or sets current hp.
      /// </summary>
      public int CurrentHP { get; set; }

      /// <summary>
      /// Gets or sets temp hp.
      /// </summary>
      public int TempHP { get; set; }

      /// <summary>
      /// Gets or sets death save successes.
      /// </summary>
      public int DeathSaveSuccesses { get; set; }

      /// <summary>
      /// Gets or sets death save failures.
      /// </summary>
      public int DeathSaveFailures { get; set; }

      /// <summary>
      /// Gets or sets race id.
      /// </summary>
      public Guid Race { get; set; }

      /// <summary>
      /// Gets or sets background id.
      /// </summary>
      public Guid Background { get; set; }

      /// <summary>
      /// Gets or sets alignment.
      /// </summary>
      public Alignment Alignment { get; set; }

      /// <summary>
      /// Gets or sets movement information.
      /// </summary>
      public MovementRecord Movement { get; set; }

      /// <summary>
      /// Gets or sets inspiration.
      /// </summary>
      public bool Inspiration { get; set; }

      /// <summary>
      /// Gets or sets deity.
      /// </summary>
      public string Deity { get; set; }

      /// <summary>
      /// Gets or sets gender.
      /// </summary>
      public string Gender { get; set; }

      /// <summary>
      /// Gets or sets age.
      /// </summary>
      public int Age { get; set; }

      /// <summary>
      /// Gets or sets height feet.
      /// </summary>
      public int HeightFeet { get; set; }

      /// <summary>
      /// Gets or sets height inches.
      /// </summary>
      public int HeightInches { get; set; }

      /// <summary>
      /// Gets or sets weight.
      /// </summary>
      public int Weight { get; set; }

      /// <summary>
      /// Gets or sets hair.
      /// </summary>
      public string Hair { get; set; }

      /// <summary>
      /// Gets or sets eyes.
      /// </summary>
      public string Eyes { get; set; }

      /// <summary>
      /// Gets or sets skin.
      /// </summary>
      public string Skin { get; set; }

      /// <summary>
      /// Gets or sets experience.
      /// </summary>
      public int Experience { get; set; }

      /// <summary>
      /// Gets or sets base strength.
      /// </summary>
      public int BaseStrength { get; set; }

      /// <summary>
      /// Gets or sets base dexterity.
      /// </summary>
      public int BaseDexterity { get; set; }

      /// <summary>
      /// Gets or sets base constitution.
      /// </summary>
      public int BaseConstitution { get; set; }

      /// <summary>
      /// Gets or sets base intelligence.
      /// </summary>
      public int BaseIntelligence { get; set; }

      /// <summary>
      /// Gets or sets base wisdom.
      /// </summary>
      public int BaseWisdom { get; set; }

      /// <summary>
      /// Gets or sets base charisma.
      /// </summary>
      public int BaseCharisma { get; set; }

      /// <summary>
      /// Gets or sets bonus strength.
      /// </summary>
      public int BonusStrength { get; set; }

      /// <summary>
      /// Gets or sets bonus dexterity.
      /// </summary>
      public int BonusDexterity { get; set; }

      /// <summary>
      /// Gets or sets bonus constitution.
      /// </summary>
      public int BonusConstitution { get; set; }

      /// <summary>
      /// Gets or sets bonus intelligence.
      /// </summary>
      public int BonusIntelligence { get; set; }

      /// <summary>
      /// Gets or sets bonus wisdom.
      /// </summary>
      public int BonusWisdom { get; set; }

      /// <summary>
      /// Gets or sets bonus charisma.
      /// </summary>
      public int BonusCharisma { get; set; }

      /// <summary>
      /// Gets or sets personality traits.
      /// </summary>
      public string PersonalityTraits { get; set; }

      /// <summary>
      /// Gets or sets ideals.
      /// </summary>
      public string Ideals { get; set; }

      /// <summary>
      /// Gets or sets bonds.
      /// </summary>
      public string Bonds { get; set; }

      /// <summary>
      /// Gets or sets flaws.
      /// </summary>
      public string Flaws { get; set; }

      /// <summary>
      /// Gets or sets backstory.
      /// </summary>
      public string Backstory { get; set; }

      /// <summary>
      /// Gets or sets levels.
      /// </summary>
      public List<LevelRecord> Levels { get; set; }

      /// <summary>
      /// Gets or sets ability score roll method.
      /// </summary>
      public AbilityRollMethod AbilityRollMethod { get; set; }

      /// <summary>
      /// Gets or sets times ability scores rolled.
      /// </summary>
      public int TimesAbilityScoresRolled { get; set; }

      /// <summary>
      /// Gets or sets ability save proficiencies.
      /// </summary>
      public List<AbilityRecord> AbilitySaveProficiencies { get; set; }

      /// <summary>
      /// Gets or sets skill proficiencies.
      /// </summary>
      public List<SkillRecord> SkillProficiencies { get; set; }

      /// <summary>
      /// Gets or sets languages.
      /// </summary>
      public List<Guid> Languages { get; set; }

      /// <summary>
      /// Gets or sets armor proficiency.
      /// </summary>
      public ArmorProficiencyRecord ArmorProficiency { get; set; }

      /// <summary>
      /// Gets or sets weapon proficiency.
      /// </summary>
      public WeaponProficiencyRecord WeaponProficiency { get; set; }

      /// <summary>
      /// Gets or sets tool proficiency.
      /// </summary>
      public ToolProficiencyRecord ToolProficiency { get; set; }

      /// <summary>
      /// Gets or sets conditions.
      /// </summary>
      public List<AppliedConditionRecord> Conditions { get; set; }

      /// <summary>
      /// Gets or sets saving throw notes.
      /// </summary>
      public string SavingThrowNotes { get; set; }

      /// <summary>
      /// Gets or sets attacks.
      /// </summary>
      public List<AttackRecord> Attacks { get; set; }

      /// <summary>
      /// Gets or sets counters.
      /// </summary>
      public List<CounterRecord> Counters { get; set; }

      /// <summary>
      /// Gets or sets companions.
      /// </summary>
      public List<CompanionRecord> Companions { get; set; }

      /// <summary>
      /// Gets or sets bags.
      /// </summary>
      public List<BagRecord> Bags { get; set; }

      /// <summary>
      /// Gets or sets spellbooks.
      /// </summary>
      public List<SpellbookRecord> Spellbooks { get; set; }

      /// <summary>
      /// Gets or sets stat modifications.
      /// </summary>
      public List<StatModificationRecord> StatModifications { get; set; }

      /// <summary>
      /// Gets or sets armor class information.
      /// </summary>
      public ArmorClassRecord ArmorClassModel { get; set; }
   }
}
