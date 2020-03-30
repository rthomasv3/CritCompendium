using System;
using CriticalCompendiumInfrastructure.Enums;

namespace CriticalCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store attack information.
   /// </summary>
   public sealed class AttackRecord
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
      /// Gets or sets number of damage dice.
      /// </summary>
      public int NumberOfDamageDice { get; set; }

      /// <summary>
      /// Gets or sets damage die.
      /// </summary>
      public string DamageDie { get; set; }

      /// <summary>
      /// Gets or sets ability.
      /// </summary>
      public Ability Ability { get; set; }

      /// <summary>
      /// Gets or sets proficient.
      /// </summary>
      public bool Proficient { get; set; }

      /// <summary>
      /// Gets or sets damage type.
      /// </summary>
      public DamageType DamageType { get; set; }

      /// <summary>
      /// Gets or sets additional to hit bonus.
      /// </summary>
      public int AdditionalToHitBonus { get; set; }

      /// <summary>
      /// Gets or sets additional damage bonus.
      /// </summary>
      public int AdditionalDamageBonus { get; set; }

      /// <summary>
      /// Gets or sets range.
      /// </summary>
      public string Range { get; set; }

      /// <summary>
      /// Gets or sets notes.
      /// </summary>
      public string Notes { get; set; }

      /// <summary>
      /// Gets or sets show to hit.
      /// </summary>
      public bool ShowToHit { get; set; }

      /// <summary>
      /// Gets or sets show damage.
      /// </summary>
      public bool ShowDamage { get; set; }

      /// <summary>
      /// Gets or sets to hit.
      /// </summary>
      public string ToHit { get; set; }

      /// <summary>
      /// Gets or sets damage.
      /// </summary>
      public string Damage { get; set; }
   }
}
