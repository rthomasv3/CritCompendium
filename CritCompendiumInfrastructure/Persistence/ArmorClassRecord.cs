using System;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store armor class information.
   /// </summary>
   public sealed class ArmorClassRecord
   {
      /// <summary>
      /// Gets or sets id.
      /// </summary>
      public Guid Id { get; set; }

      /// <summary>
      /// Gets or sets armor bonus.
      /// </summary>
      public int ArmorBonus { get; set; }

      /// <summary>
      /// Gets or sets armor type.
      /// </summary>
      public ArmorType ArmorType { get; set; }

      /// <summary>
      /// Gets or sets first ability.
      /// </summary>
      public Ability FirstAbility { get; set; }

      /// <summary>
      /// Gets or sets second ability.
      /// </summary>
      public Ability SecondAbility { get; set; }

      /// <summary>
      /// Gets or sets item bonus.
      /// </summary>
      public int ItemBonus { get; set; }

      /// <summary>
      /// Gets or sets additional bonus.
      /// </summary>
      public int AdditionalBonus { get; set; }
   }
}
