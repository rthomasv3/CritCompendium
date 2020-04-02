using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store random table information.
   /// </summary>
   public sealed class RandomTableRecord : CompendiumEntryRecord
   {
      /// <summary>
      /// Gets or sets tags.
      /// </summary>
      public List<string> Tags { get; set; }

      /// <summary>
      /// Gets or sets die.
      /// </summary>
      public string Die { get; set; }

      /// <summary>
      /// Gets or sets header.
      /// </summary>
      public string Header { get; set; }

      /// <summary>
      /// Gets or sets rows.
      /// </summary>
      public List<RandomTableRowRecord> Rows { get; set; }
   }
}
