using System;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store application launch information.
   /// </summary>
   public sealed class LaunchRecord
   {
      /// <summary>
      /// Gets or sets the total run count.
      /// </summary>
      public int RunCount { get; set; }

      /// <summary>
      /// Gets or sets the last launch date time.
      /// </summary>
      public DateTime LastLaunchDate { get; set; }

      /// <summary>
      /// Gets or sets the last version launched.
      /// </summary>
      public string LastVersionLaunched { get; set; }
   }
}
