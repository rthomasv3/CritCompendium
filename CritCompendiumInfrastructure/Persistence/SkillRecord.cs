using CriticalCompendiumInfrastructure.Enums;

namespace CriticalCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to save skill information.
   /// </summary>
   public sealed class SkillRecord
   {
      /// <summary>
      /// Gets skill.
      /// </summary>
      public Skill Skill { get; set; }

      /// <summary>
      /// Gets skill string.
      /// </summary>
      public string SkillString { get; set; }

      /// <summary>
      /// Gets skill string with ability.
      /// </summary>
      public string SkillAbilityString { get; set; }

      /// <summary>
      /// Gets or sets value.
      /// </summary>
      public int Bonus { get; set; }

      /// <summary>
      /// Gets bonus string.
      /// </summary>
      public string BonusString { get; set; }

      /// <summary>
      /// Gets or set proficient.
      /// </summary>
      public bool Proficient { get; set; }

      /// <summary>
      /// Gets or set expertise.
      /// </summary>
      public bool Expertise { get; set; }
   }
}
