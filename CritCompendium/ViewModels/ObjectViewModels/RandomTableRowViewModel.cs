using System;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class RandomTableRowViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly RandomTableRowModel _rowModel;

      private int _min;
      private int _max;
      private string _value;
      private int _dieMax;
      private bool _selected;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of <see cref="RandomTableRowViewModel"/>
      /// </summary>
      public RandomTableRowViewModel(RandomTableRowModel rowModel)
      {
         _rowModel = rowModel;

         _min = _rowModel.Min;
         _max = _rowModel.Max;
         _value = _rowModel.Value;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets the random table row model
      /// </summary>
      public RandomTableRowModel RowModel
      {
         get { return _rowModel; }
      }

      /// <summary>
      /// Gets or sets min
      /// </summary>
      public int Min
      {
         get { return _min; }
         set
         {
            if (Set(ref _min, Math.Min(_dieMax, Math.Max(1, value))))
            {
               _rowModel.Min = _min;
            }
         }
      }

      /// <summary>
      /// Gets or sets max
      /// </summary>
      public int Max
      {
         get { return _max; }
         set
         {
            if (Set(ref _max, Math.Max(_min, Math.Min(_dieMax, value))))
            {
               _rowModel.Max = _max;
            }
         }
      }

      /// <summary>
      /// Gets or sets value
      /// </summary>
      public string Value
      {
         get { return _value; }
         set
         {
            if (Set(ref _value, value))
            {
               _rowModel.Value = _value;
            }
         }
      }

      /// <summary>
      /// Gets or sets die max
      /// </summary>
      public int DieMax
      {
         get { return _dieMax; }
         set
         {
            if (Set(ref _dieMax, value) && _max > _dieMax)
            {
               Max = _dieMax;
            }
         }
      }

      /// <summary>
      /// Gets min max display
      /// </summary>
      public string MinMaxDisplay
      {
         get
         {
            return _min == _max ? _min.ToString() : $"{_min} - {_max}";
         }
      }

      /// <summary>
      /// Gets selected
      /// </summary>
      public bool Selected
      {
         get { return _selected; }
         set { Set(ref _selected, value); }
      }

      #endregion
   }
}
