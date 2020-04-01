using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store monster information.
   /// </summary>
   public sealed class MonsterRecord
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
      /// Gets or sets size.
      /// </summary>
      public CreatureSize Size { get; set; }

      /// <summary>
      /// Gets or sets type.
      /// </summary>
      public string Type { get; set; }

      /// <summary>
      /// Gets or sets alignment.
      /// </summary>
      public string Alignment { get; set; }

      /// <summary>
      /// Gets or sets ac.
      /// </summary>
      public string AC { get; set; }

      /// <summary>
      /// Gets or sets hp.
      /// </summary>
      public string HP { get; set; }

      /// <summary>
      /// Gets or sets speed.
      /// </summary>
      public string Speed { get; set; }

      /// <summary>
      /// Gets or sets strength.
      /// </summary>
      public int Strength { get; set; }

      /// <summary>
      /// Gets or sets dexterity.
      /// </summary>
      public int Dexterity { get; set; }

      /// <summary>
      /// Gets or sets constitution.
      /// </summary>
      public int Constitution { get; set; }

      /// <summary>
      /// Gets or sets intelligence.
      /// </summary>
      public int Intelligence { get; set; }

      /// <summary>
      /// Gets or sets wisdom.
      /// </summary>
      public int Wisdom { get; set; }

      /// <summary>
      /// Gets or sets charisma.
      /// </summary>
      public int Charisma { get; set; }

      /// <summary>
      /// Gets or sets description.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Gets or sets saves.
      /// </summary>
      public Dictionary<Ability, int> Saves { get; set; }

      /// <summary>
      /// Gets or sets skills.
      /// </summary>
      public Dictionary<Skill, int> Skills { get; set; }

      /// <summary>
      /// Gets or sets resistances.
      /// </summary>
      public string Resistances { get; set; }

      /// <summary>
      /// Gets or sets vulnerablities.
      /// </summary>
      public string Vulnerabilities { get; set; }

      /// <summary>
      /// Gets or sets immunities.
      /// </summary>
      public string Immunities { get; set; }

      /// <summary>
      /// Gets or sets cond immunities.
      /// </summary>
      public string ConditionImmunities { get; set; }

      /// <summary>
      /// Gets or sets senses.
      /// </summary>
      public string Senses { get; set; }

      /// <summary>
      /// Gets or sets passive perception.
      /// </summary>
      public int PassivePerception { get; set; }

      /// <summary>
      /// Gets or sets languages.
      /// </summary>
      public List<string> Languages { get; set; }

      /// <summary>
      /// Gets or sets cr.
      /// </summary>
      public string CR { get; set; }

      /// <summary>
      /// Gets or sets traits.
      /// </summary>
      public List<TraitRecord> Traits { get; set; }

      /// <summary>
      /// Gets or sets actions.
      /// </summary>
      public List<MonsterActionRecord> Actions { get; set; }

      /// <summary>
      /// Gets or sets reactions.
      /// </summary>
      public List<MonsterActionRecord> Reactions { get; set; }

      /// <summary>
      /// Gets or sets legendary actions.
      /// </summary>
      public List<MonsterActionRecord> LegendaryActions { get; set; }

      /// <summary>
      /// Gets or sets spells.
      /// </summary>
      public List<string> Spells { get; set; }

      /// <summary>
      /// Gets or sets spell slots.
      /// </summary>
      public List<string> SpellSlots { get; set; }

      /// <summary>
      /// Gets or sets environment.
      /// </summary>
      public string Environment { get; set; }
   }
}
