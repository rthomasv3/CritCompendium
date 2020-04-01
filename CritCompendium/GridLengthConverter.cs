using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CritCompendium
{
   public sealed class GridLengthConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         double doubleValue = double.Parse(value.ToString());
         bool star = parameter != null ? bool.Parse(parameter.ToString()) : false;
         GridLength gridLength = new GridLength(doubleValue, star ? GridUnitType.Star : GridUnitType.Pixel);
         return gridLength;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         GridLength gridLength = (GridLength)value;
         return gridLength.Value;
      }
   }
}
