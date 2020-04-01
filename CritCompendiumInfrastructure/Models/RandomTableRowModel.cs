namespace CritCompendiumInfrastructure.Models
{
   public sealed class RandomTableRowModel
   {
      #region Fields

      private int _min;
      private int _max;
      private string _value;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of <see cref="RandomTableRowModel"/>
      /// </summary>
      public RandomTableRowModel()
      {

      }

      /// <summary>
      /// Creates a copy of <see cref="RandomTableRowModel"/>
      /// </summary>
      public RandomTableRowModel(RandomTableRowModel randomTableRowModel)
      {
         _min = randomTableRowModel.Min;
         _max = randomTableRowModel.Max;
         _value = randomTableRowModel.Value;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets min
      /// </summary>
      public int Min
      {
         get { return _min; }
         set { _min = value; }
      }

      /// <summary>
      /// Gets or sets max
      /// </summary>
      public int Max
      {
         get { return _max; }
         set { _max = value; }
      }

      /// <summary>
      /// Gets or sets value
      /// </summary>
      public string Value
      {
         get { return _value; }
         set { _value = value; }
      }

      #endregion
   }
}
