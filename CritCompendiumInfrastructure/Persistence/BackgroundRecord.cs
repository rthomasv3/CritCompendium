using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store background information.
   /// </summary>
   public sealed class BackgroundRecord
   {
      /// <summary>
      /// Gets or sets the id.
      /// </summary>
      public Guid Id { get; set; }

      /// <summary>
      /// Gets or sets the name.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or sets the skills.
      /// </summary>
      public List<Skill> Skills { get; set; }

      /// <summary>
      /// Gets or sets the traits.
      /// </summary>
      public List<TraitRecord> Traits { get; set; }

      /// <summary>
      /// Gets or sets the index of the languages trait.
      /// </summary>
      public int LanguagesTraitIndex { get; set; }

      /// <summary>
      /// Gets or sets the index of the tools trait.
      /// </summary>
      public int ToolsTraitIndex { get; set; }

      /// <summary>
      /// Gets or sets the index of the skills trait.
      /// </summary>
      public int SkillsTraitIndex { get; set; }

      /// <summary>
      /// Gets or sets the index of the starting trait.
      /// </summary>
      public int StartingTraitIndex { get; set; }
   }
}
