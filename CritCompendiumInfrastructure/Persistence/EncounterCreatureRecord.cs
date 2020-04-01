using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store encounter creature information.
   /// </summary>
   public sealed class EncounterCreatureRecord
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
      /// Gets or sets current hp.
      /// </summary>
      public int CurrentHP { get; set; }

      /// <summary>
      /// Gets or sets max hp.
      /// </summary>
      public int MaxHP { get; set; }

      /// <summary>
      /// Gets less hp.
      /// </summary>
      public int LessHP { get; set; }

      /// <summary>
      /// Gets or sets ac.
      /// </summary>
      public int AC { get; set; }

      /// <summary>
      /// Gets or sets spell save dc.
      /// </summary>
      public int SpellSaveDC { get; set; }

      /// <summary>
      /// Gets or sets passive perception.
      /// </summary>
      public int PassivePerception { get; set; }

      /// <summary>
      /// Gets or sets initiative bonus.
      /// </summary>
      public int InitiativeBonus { get; set; }

      /// <summary>
      /// Gets or sets initiative.
      /// </summary>
      public int Initiative { get; set; }

      /// <summary>
      /// Gets or sets selected.
      /// </summary>
      public bool Selected { get; set; }

      /// <summary>
      /// Gets or sets conditions.
      /// </summary>
      public List<AppliedConditionRecord> Conditions { get; set; }
   }
}
