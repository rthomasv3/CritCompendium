using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class RandomTableModel : CompendiumEntryModel
   {
      #region Fields

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
      }

      /// <summary>
      /// Creates a copy of <see cref="RandomTableModel"/>
      /// </summary>
      public RandomTableModel(RandomTableModel randomTableModel) : base(randomTableModel)
      {
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
