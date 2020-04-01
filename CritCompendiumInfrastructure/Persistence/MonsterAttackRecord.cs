using System;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store monster attack information.
   /// </summary>
   public sealed class MonsterAttackRecord
   {
      /// <summary>
      /// Gets or sets id
      /// </summary>
      public Guid Id { get; set; }

      /// <summary>
      /// Gets or sets name.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or sets to hit.
      /// </summary>
      public string ToHit { get; set; }

      /// <summary>
      /// Gets or sets roll.
      /// </summary>
      public string Roll { get; set; }
   }
}
