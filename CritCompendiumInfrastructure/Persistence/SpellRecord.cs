﻿using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store spell information.
   /// </summary>
   public sealed class SpellRecord : CompendiumEntryRecord
   {
      /// <summary>
      /// Gets or sets level.
      /// </summary>
      public int Level { get; set; }

      /// <summary>
      /// Gets or sets spell school.
      /// </summary>
      public SpellSchool SpellSchool { get; set; }

      /// <summary>
      /// Gets or sets is ritual.
      /// </summary>
      public bool IsRitual { get; set; }

      /// <summary>
      /// Gets or sets casting time.
      /// </summary>
      public string CastingTime { get; set; }

      /// <summary>
      /// Gets or sets range.
      /// </summary>
      public string Range { get; set; }

      /// <summary>
      /// Gets or sets components.
      /// </summary>
      public string Components { get; set; }

      /// <summary>
      /// Gets or sets duration.
      /// </summary>
      public string Duration { get; set; }

      /// <summary>
      /// Gets or sets classes.
      /// </summary>
      public string Classes { get; set; }

      /// <summary>
      /// Gets or sets text.
      /// </summary>
      public List<string> TextCollection { get; set; }

      /// <summary>
      /// Gets or sets rolls.
      /// </summary>
      public List<string> Rolls { get; set; }
   }
}
