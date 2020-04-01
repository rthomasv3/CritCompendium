using System;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class RoomModel
   {
      #region Fields

      private Guid _id;
      private string _name;
      private string _description;
      private string _entry;
      private string _map;
      private string _floor;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of <see cref="RoomModel"/>
      /// </summary>
      public RoomModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates a copy of <see cref="RoomModel"/>
      /// </summary>
      public RoomModel(RoomModel roomModel)
      {
         _id = roomModel.ID;
         _name = roomModel.Name;
         _description = roomModel.Description;
         _entry = roomModel.Entry;
         _map = roomModel.Map;
         _floor = roomModel.Floor;
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

      /// <summary>
      /// Gets or sets entry
      /// </summary>
      public string Entry
      {
         get { return _entry; }
         set { _entry = value; }
      }

      /// <summary>
      /// Gets or sets map
      /// </summary>
      public string Map
      {
         get { return _map; }
         set { _map = value; }
      }

      /// <summary>
      /// Gets or sets floor
      /// </summary>
      public string Floor
      {
         get { return _floor; }
         set { _floor = value; }
      }

      #endregion
   }
}
