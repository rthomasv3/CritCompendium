using CriticalCompendiumInfrastructure.Enums;

namespace CriticalCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to save ability information.
   /// </summary>
   public sealed class AbilityRecord
   {
      /// <summary>
      /// Gets ability.
      /// </summary>
      public Ability Ability { get; set; }

      /// <summary>
      /// Gets ability string.
      /// </summary>
      public string AbilityString { get; set; }

      /// <summary>
      /// Gets or sets save bonus value.
      /// </summary>
      public int SaveBonus { get; set; }

      /// <summary>
      /// Gets or sets check bonus value.
      /// </summary>
      public int CheckBonus { get; set; }

      /// <summary>
      /// Gets save bonus string.
      /// </summary>
      public string SaveBonusString { get; set; }

      /// <summary>
      /// Gets check bonus string.
      /// </summary>
      public string CheckBonusString { get; set; }

      /// <summary>
      /// Gets or set proficient.
      /// </summary>
      public bool Proficient { get; set; }
   }
}
