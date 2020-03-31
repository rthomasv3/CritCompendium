using System;

namespace CriticalCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store applied condition information.
   /// </summary>
   public sealed class AppliedConditionRecord
   {
      /// <summary>
      /// Gets or sets id.
      /// </summary>
      public Guid Id { get; set; }

      /// <summary>
      /// Gets or sets condition model id.
      /// </summary>
      public Guid ConditionId { get; set; }

      /// <summary>
      /// Gets or sets name.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or sets level.
      /// </summary>
      public int Level { get; set; }

      /// <summary>
      /// Gets or sets notes.
      /// </summary>
      public string Notes { get; set; }
   }
}
