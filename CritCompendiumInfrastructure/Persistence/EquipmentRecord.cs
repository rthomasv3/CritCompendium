using System;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store equipment information.
   /// </summary>
   public sealed class EquipmentRecord
   {
      /// <summary>
      /// Gets or sets id.
      /// </summary>
      public Guid Id { get; set; }

      /// <summary>
      /// Gets or sets item id.
      /// </summary>
      public Guid ItemId { get; set; }

      /// <summary>
      /// Gets or sets name.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or sets equipped.
      /// </summary>
      public bool Equipped { get; set; }

      /// <summary>
      /// Gets or sets quantity.
      /// </summary>
      public int Quantity { get; set; }

      /// <summary>
      /// Gets or sets notes.
      /// </summary>
      public string Notes { get; set; }
   }
}
