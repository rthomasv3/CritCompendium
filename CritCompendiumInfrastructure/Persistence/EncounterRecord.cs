using System;
using System.Collections.Generic;
using CriticalCompendiumInfrastructure.Enums;

namespace CriticalCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store encounter information.
   /// </summary>
   public sealed class EncounterRecord
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
      /// Gets or sets creatures.
      /// </summary>
      public List<EncounterCreatureRecord> Creatures { get; set; }

      /// <summary>
      /// Gets or sets round.
      /// </summary>
      public int Round { get; set; }

      /// <summary>
      /// Gets or sets encounter challenge.
      /// </summary>
      public EncounterChallenge EncounterChallenge { get; set; }

      /// <summary>
      /// Gets or sets total character hp.
      /// </summary>
      public int TotalCharacterHP { get; set; }

      /// <summary>
      /// Gets or sets total monster hp.
      /// </summary>
      public int TotalMonsterHP { get; set; }

      /// <summary>
      /// Gets or sets time elapsed.
      /// </summary>
      public int TimeElapsed { get; set; }

      /// <summary>
      /// Gets or sets current turn.
      /// </summary>
      public int CurrentTurn { get; set; }

      /// <summary>
      /// Gets or sets encounter state.
      /// </summary>
      public EncounterState EncounterState { get; set; }

      /// <summary>
      /// Gets or sets notes.
      /// </summary>
      public string Notes { get; set; }
   }
}
