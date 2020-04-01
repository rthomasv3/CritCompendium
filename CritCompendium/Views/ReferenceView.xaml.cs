using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for ReferenceView.xaml
   /// </summary>
   public partial class ReferenceView : UserControl
   {
      public ReferenceView()
      {
         InitializeComponent();
      }

      private void Child_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
      {
         if (sender is UIElement element)
         {
            if (!element.IsFocused)
            {
               e.Handled = true;
               MouseWheelEventArgs mouseEventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta) { RoutedEvent = UIElement.MouseWheelEvent };
               _parentScroll.RaiseEvent(mouseEventArgs);
            }
         }
      }
   }
}
