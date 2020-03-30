using System;
using System.Collections.Generic;
using System.Text;

namespace CriticalCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to save movement information.
   /// </summary>
   public sealed class MovementRecord
   {
      /// <summary>
      /// Gets or sets id.
      /// </summary>
      public Guid ID { get; set; }

      /// <summary>
      /// Gets or sets walk speed.
      /// </summary>
      public int WalkSpeed { get; set; }

      /// <summary>
      /// Gets or sets swim speed.
      /// </summary>
      public int SwimSpeed { get; set; }

      /// <summary>
      /// Gets or sets climb speed.
      /// </summary>
      public int ClimbSpeed { get; set; }

      /// <summary>
      /// Gets or sets crawl speed.
      /// </summary>
      public int CrawlSpeed { get; set; }

      /// <summary>
      /// Gets or sets fly speed.
      /// </summary>
      public int FlySpeed { get; set; }

      /// <summary>
      /// Gets or sets apply encumbrance.
      /// </summary>
      public bool ApplyEncumbrance { get; set; }
   }
}
