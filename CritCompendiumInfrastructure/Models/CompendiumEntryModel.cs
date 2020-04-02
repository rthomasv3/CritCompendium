using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Models
{
   /// <summary>
   /// Class used to define basic properties of any compendium entry.
   /// </summary>
   public class CompendiumEntryModel
   {
      #region Fields

      private Guid _id;
      private string _name;
      private List<string> _tags;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of the <see cref="CompendiumEntryModel"/> class.
      /// </summary>
      public CompendiumEntryModel()
      {
         _id = Guid.NewGuid();
         _name = String.Empty;
         _tags = new List<string>();
      }

      /// <summary>
      /// Creates a copy of the <see cref="CompendiumEntryModel"/> instance.
      /// </summary>
      public CompendiumEntryModel(CompendiumEntryModel compendiumEntryModel)
      {
         _id = Guid.NewGuid();
         _name = compendiumEntryModel.Name;
         _tags = new List<string>(compendiumEntryModel.Tags);
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the entry id.
      /// </summary>
      public Guid Id
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Gets or sets the entry name.
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Gets or sets the list of tags.
      /// </summary>
      public List<string> Tags
      {
         get { return _tags; }
         set { _tags = value; }
      }

      #endregion
   }
}
