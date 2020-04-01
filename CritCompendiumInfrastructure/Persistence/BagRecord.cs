using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store bag information.
   /// </summary>
   public sealed class BagRecord
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
      /// Gets or sets fixed weight.
      /// </summary>
      public bool FixedWeight { get; set; }

      /// <summary>
      /// Gets or sets fixed weight value.
      /// </summary>
      public int FixedWeightValue { get; set; }

      /// <summary>
      /// Gets or sets copper.
      /// </summary>
      public int Copper { get; set; }

      /// <summary>
      /// Gets or sets silver.
      /// </summary>
      public int Silver { get; set; }

      /// <summary>
      /// Gets or sets electrum.
      /// </summary>
      public int Electrum { get; set; }

      /// <summary>
      /// Gets or sets gold.
      /// </summary>
      public int Gold { get; set; }

      /// <summary>
      /// Gets or sets platinum.
      /// </summary>
      public int Platinum { get; set; }

      /// <summary>
      /// Gets or sets equipment.
      /// </summary>
      public List<EquipmentRecord> Equipment { get; set; }
   }
}
