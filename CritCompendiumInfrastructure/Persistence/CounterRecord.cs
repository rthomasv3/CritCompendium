using System;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store counter information.
   /// </summary>
   public sealed class CounterRecord
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
      /// Gets or sets current value.
      /// </summary>
      public int CurrentValue { get; set; }

      /// <summary>
      /// Gets or sets max value.
      /// </summary>
      public int MaxValue { get; set; }

      /// <summary>
      /// Gets or sets reset on short rest.
      /// </summary>
      public bool ResetOnShortRest { get; set; }

      /// <summary>
      /// Gets or sets reset on long rest.
      /// </summary>
      public bool ResetOnLongRest { get; set; }

      /// <summary>
      /// Gets or sets notes.
      /// </summary>
      public string Notes { get; set; }
   }
}
