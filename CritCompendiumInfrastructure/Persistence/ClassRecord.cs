using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store character class information.
   /// </summary>
   public sealed class ClassRecord
   {
      /// <summary>
      /// Gets or sets id.
      /// </summary>
      public Guid Id { get; set; }

      /// <summary>
      /// Gets or sets name.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or sets hit die.
      /// </summary>
      public int HitDie { get; set; }

      /// <summary>
      /// Gets or sets ability proficiencies.
      /// </summary>
      public List<Ability> AbilityProficiencies { get; set; }

      /// <summary>
      /// Gets or sets spell ability.
      /// </summary>
      public Ability SpellAbility { get; set; }

      /// <summary>
      /// Gets or sets spell slots.
      /// </summary>
      public List<string> SpellSlots { get; set; }

      /// <summary>
      /// Gets or sets spell start level.
      /// </summary>
      public int SpellStartLevel { get; set; }

      /// <summary>
      /// Gets or sets armor proficiencies.
      /// </summary>
      public string ArmorProficiencies { get; set; }

      /// <summary>
      /// Gets or sets weapon proficiencies.
      /// </summary>
      public string WeaponProficiencies { get; set; }

      /// <summary>
      /// Gets or sets tool proficiencies.
      /// </summary>
      public string ToolProficiencies { get; set; }

      /// <summary>
      /// Gets or sets skill proficiencies.
      /// </summary>
      public string SkillProficiencies { get; set; }

      /// <summary>
      /// Gets or sets auto levels.
      /// </summary>
      public List<AutoLevelRecord> AutoLevels { get; set; }

      /// <summary>
      /// Gets or sets features.
      /// </summary>
      public List<FeatureRecord> Features { get; set; }
   }
}
