using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store spellbook information.
   /// </summary>
   public sealed class SpellbookRecord
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
      /// Gets or sets based on class.
      /// </summary>
      public bool BasedOnClass { get; set; }

      /// <summary>
      /// Gets or sets based on race.
      /// </summary>
      public bool BasedOnRace { get; set; }

      /// <summary>
      /// Gets or sets class id.
      /// </summary>
      public Guid ClassId { get; set; }

      /// <summary>
      /// Gets or sets race id.
      /// </summary>
      public Guid RaceId { get; set; }

      /// <summary>
      /// Gets or sets ability.
      /// </summary>
      public Ability Ability { get; set; }

      /// <summary>
      /// Gets or sets additional dc bonus.
      /// </summary>
      public int AdditionalDCBonus { get; set; }

      /// <summary>
      /// Gets or sets additional to hit bonus.
      /// </summary>
      public int AdditionalToHitBonus { get; set; }

      /// <summary>
      /// Gets or sets reset on short rest.
      /// </summary>
      public bool ResetOnShortRest { get; set; }

      /// <summary>
      /// Gets or sets reset on long rest.
      /// </summary>
      public bool ResetOnLongRest { get; set; }

      /// <summary>
      /// Gets or sets spells.
      /// </summary>
      public List<SpellbookEntryRecord> Spells { get; set; }

      /// <summary>
      /// Gets or sets current first level spell slots.
      /// </summary>
      public int CurrentFirstLevelSpellSlots { get; set; }

      /// <summary>
      /// Gets or sets current second level spell slots.
      /// </summary>
      public int CurrentSecondLevelSpellSlots { get; set; }

      /// <summary>
      /// Gets or sets current thrid level spell slots.
      /// </summary>
      public int CurrentThirdLevelSpellSlots { get; set; }

      /// <summary>
      /// Gets or sets current fourth level spell slots.
      /// </summary>
      public int CurrentFourthLevelSpellSlots { get; set; }

      /// <summary>
      /// Gets or sets current fifth level spell slots.
      /// </summary>
      public int CurrentFifthLevelSpellSlots { get; set; }

      /// <summary>
      /// Gets or sets current sixth level spell slots.
      /// </summary>
      public int CurrentSixthLevelSpellSlots { get; set; }

      /// <summary>
      /// Gets or sets current seventh level spell slots.
      /// </summary>
      public int CurrentSeventhLevelSpellSlots { get; set; }

      /// <summary>
      /// Gets or sets current eighth level spell slots.
      /// </summary>
      public int CurrentEighthLevelSpellSlots { get; set; }

      /// <summary>
      /// Gets or sets current ninth level spell slots.
      /// </summary>
      public int CurrentNinthLevelSpellSlots { get; set; }
   }
}
