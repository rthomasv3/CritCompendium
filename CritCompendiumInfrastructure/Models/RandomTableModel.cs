using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class RandomTableModel
   {
      #region Fields

      private Guid _id;
      private string _name;
      private List<string> _tags = new List<string>();
      private string _die;
      private string _header;
      private List<RandomTableRowModel> _rows = new List<RandomTableRowModel>();

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of <see cref="RandomTableModel"/>
      /// </summary>
      public RandomTableModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates a copy of <see cref="RandomTableModel"/>
      /// </summary>
      public RandomTableModel(RandomTableModel randomTableModel)
      {
         _id = randomTableModel.ID;
         _name = randomTableModel.Name;
         _tags = new List<string>(randomTableModel.Tags);
         _die = randomTableModel.Die;
         _header = randomTableModel.Header;

         _rows = new List<RandomTableRowModel>();
         foreach (RandomTableRowModel randomTableRowModel in randomTableModel.Rows)
         {
            _rows.Add(new RandomTableRowModel(randomTableRowModel));
         }
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
      /// Gets or sets tags
      /// </summary>
      public List<string> Tags
      {
         get { return _tags; }
         set { _tags = value; }
      }

      /// <summary>
      /// Gets or sets die
      /// </summary>
      public string Die
      {
         get { return _die; }
         set { _die = value; }
      }

      /// <summary>
      /// Gets or sets header
      /// </summary>
      public string Header
      {
         get { return _header; }
         set { _header = value; }
      }

      /// <summary>
      /// Gets or sets rows
      /// </summary>
      public List<RandomTableRowModel> Rows
      {
         get { return _rows; }
         set { _rows = value; }
      }

      #endregion
   }
}
