using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store race information.
   /// </summary>
   public sealed class RaceRecord
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
      /// Gets or sets size.
      /// </summary>
      public CreatureSize Size { get; set; }

      /// <summary>
      /// Gets or sets walk speed.
      /// </summary>
      public int WalkSpeed { get; set; }

      /// <summary>
      /// Gets or sets fly speed.
      /// </summary>
      public int FlySpeed { get; set; }

      /// <summary>
      /// Gets or sets abilities.
      /// </summary>
      public Dictionary<Ability, int> Abilities { get; set; }

      /// <summary>
      /// Gets or sets skill proficiencies.
      /// </summary>
      public List<Skill> SkillProficiencies { get; set; }

      /// <summary>
      /// Gets or sets languages.
      /// </summary>
      public List<string> Languages { get; set; }

      /// <summary>
      /// Gets or sets weapon proficiencies.
      /// </summary>
      public List<string> WeaponProficiencies { get; set; }

      /// <summary>
      /// Gets or sets armor proficiencies.
      /// </summary>
      public List<string> ArmorProficiencies { get; set; }

      /// <summary>
      /// Gets or sets tool proficiencies.
      /// </summary>
      public List<string> ToolProficiencies { get; set; }
      /// <summary>
      /// Gets or sets traits.
      /// </summary>
      public List<TraitRecord> Traits { get; set; }

      /// <summary>
      /// Gets or sets language trait.
      /// </summary>
      public int LanguageTrait { get; set; }
   }
}
