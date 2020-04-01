using System;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store companion information.
   /// </summary>
   public sealed class CompanionRecord
   {
      /// <summary>
      /// Gets or sets id.
      /// </summary>
      public Guid Id { get; set; }

      /// <summary>
      /// Gets or sets monster id.
      /// </summary>
      public Guid MonsterId { get; set; }

      /// <summary>
      /// Gets or sets name.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or sets current hp.
      /// </summary>
      public int CurrentHP { get; set; }

      /// <summary>
      /// Gets or sets max hp.
      /// </summary>
      public int MaxHP { get; set; }

      /// <summary>
      /// Gets or sets notes.
      /// </summary>
      public string Notes { get; set; }
   }
}
