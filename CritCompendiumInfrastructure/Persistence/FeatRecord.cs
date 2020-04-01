using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store feat information.
   /// </summary>
   public sealed class FeatRecord
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
      /// Gets or sets prerequisite.
      /// </summary>
      public string Prerequisite { get; set; }

      /// <summary>
      /// Gets or sets text.
      /// </summary>
      public List<string> TextCollection { get; set; }

      /// <summary>
      /// Gets or sets modifiers.
      /// </summary>
      public List<ModifierRecord> Modifiers { get; set; }
   }
}
