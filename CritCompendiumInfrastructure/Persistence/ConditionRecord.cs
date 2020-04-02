using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store condition information.
   /// </summary>
   public sealed class ConditionRecord : CompendiumEntryRecord
   {
      /// <summary>
      /// Gets or sets description.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Gets or sets levels.
      /// </summary>
      public int Levels { get; set; }

      /// <summary>
      /// Gets or sets headers.
      /// </summary>
      public List<string> Headers { get; set; }

      /// <summary>
      /// Gets or sets elements.
      /// </summary>
      public List<string> Elements { get; set; }

      /// <summary>
      /// Gets or sets text.
      /// </summary>
      public List<string> TextCollection { get; set; }
   }
}
