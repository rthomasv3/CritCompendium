using System;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class HookModel
   {
      #region Fields

      private Guid _id;
      private string _name;
      private string _description;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="HookModel"/>
      /// </summary>
      public HookModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates a copy of <see cref="HookModel"/>
      /// </summary>
      public HookModel(HookModel hookModel)
      {
         _id = hookModel.ID;
         _name = hookModel.Name;
         _description = hookModel.Description;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets id
      /// </summary>
      public Guid ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Gets or sets name
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Gets or sets description
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      #endregion
   }
}
