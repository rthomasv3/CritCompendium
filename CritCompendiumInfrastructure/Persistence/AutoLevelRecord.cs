using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store auto level information.
   /// </summary>
   public sealed class AutoLevelRecord
   {
      /// <summary>
      /// Gets or sets the id.
      /// </summary>
      public Guid Id { get; set; }

      /// <summary>
      /// Gets or sets the level.
      /// </summary>
      public int Level { get; set; }

      /// <summary>
      /// Gets or sets score improvement.
      /// </summary>
      public bool ScoreImprovement { get; set; }

      /// <summary>
      /// Gets or sets the features.
      /// </summary>
      public List<FeatureRecord> Features { get; set; }
   }
}
