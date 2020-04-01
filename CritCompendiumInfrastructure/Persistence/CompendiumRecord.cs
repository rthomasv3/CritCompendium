using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to save a compendium information.
   /// </summary>
   public sealed class CompendiumRecord
   {
      /// <summary>
		/// Gets or sets characters.
		/// </summary>
		public List<CharacterRecord> Characters { get; set; }

      /// <summary>
      /// Gets or sets adventures.
      /// </summary>
      public List<AdventureRecord> Adventures { get; set; }

      /// <summary>
      /// Gets or sets encounters.
      /// </summary>
      public List<EncounterRecord> Encounters { get; set; }

      /// <summary>
      /// Gets or sets locations.
      /// </summary>
      public List<LocationRecord> Locations { get; set; }

      /// <summary>
      /// Gets or sets npcs.
      /// </summary>
      public List<NPCRecord> NPCs { get; set; }

      /// <summary>
      /// Gets or sets tables.
      /// </summary>
      public List<RandomTableRecord> Tables { get; set; }

      /// <summary>
      /// Gets or sets backgrounds.
      /// </summary>
      public List<BackgroundRecord> Backgrounds { get; set; }

      /// <summary>
      /// Gets or sets classes.
      /// </summary>
      public List<ClassRecord> Classes { get; set; }

      /// <summary>
      /// Gets or sets conditions.
      /// </summary>
      public List<ConditionRecord> Conditions { get; set; }

      /// <summary>
      /// Gets or sets feats.
      /// </summary>
      public List<FeatRecord> Feats { get; set; }

      /// <summary>
      /// Gets or sets items.
      /// </summary>
      public List<ItemRecord> Items { get; set; }

      /// <summary>
      /// Gets or sets languages.
      /// </summary>
      public List<LanguageRecord> Languages { get; set; }

      /// <summary>
      /// Gets or sets monsters.
      /// </summary>
      public List<MonsterRecord> Monsters { get; set; }

      /// <summary>
      /// Gets or sets races.
      /// </summary>
      public List<RaceRecord> Races { get; set; }

      /// <summary>
      /// Gets or sets spells.
      /// </summary>
      public List<SpellRecord> Spells { get; set; }

      /// <summary>
      /// Gets or sets tags.
      /// </summary>
      public List<string> Tags { get; set; }
   }
}
