using System;

namespace CriticalCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store spellbook entry information.
   /// </summary>
   public sealed class SpellbookEntryRecord
   {
      /// <summary>
      /// Gets or sets id.
      /// </summary>
      public Guid Id { get; set;  }

      /// <summary>
      /// Gets or sets prepared.
      /// </summary>
      public bool Prepared { get; set; }

      /// <summary>
      /// Gets or sets used.
      /// </summary>
      public bool Used { get; set; }

      /// <summary>
      /// Gets or sets spell id.
      /// </summary>
      public Guid SpellId { get; set; }
   }
}
