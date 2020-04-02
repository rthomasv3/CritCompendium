using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store basic compendium entry information.
   /// </summary>
   public class CompendiumEntryRecord
   {
      /// <summary>
      /// Gets or sets the entry id.
      /// </summary>
      public Guid Id { get; set; }

      /// <summary>
      /// Gets or sets the entry name.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or sets the list of tags.
      /// </summary>
      public List<string> Tags { get; set; }
   }
}
