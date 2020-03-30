using System;
using CriticalCompendiumInfrastructure.Enums;

namespace CriticalCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store stat modification information.
   /// </summary>
   public sealed class StatModificationRecord
   {
      /// <summary>
      /// Gets or sets id.
      /// </summary>
      public Guid Id { get; set; }

      /// <summary>
      /// Gets or sets modification option.
      /// </summary>
      public StatModificationOption ModificationOption { get; set; }

      /// <summary>
      /// Gets or sets fixed value.
      /// </summary>
      public bool FixedValue { get; set; }

      /// <summary>
      /// Gets or sets value
      /// </summary>
      public int Value { get; set; }

      /// <summary>
      /// Gets or sets notes.
      /// </summary>
      public string Notes { get; set; }
   }
}
