using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CritCompendium
{
   /// <summary>
   /// Interaction logic for CustomTextBox.xaml
   /// </summary>
   public partial class CustomTextBox : UserControl
   {
      public CustomTextBox()
      {
         InitializeComponent();

         DataContext = this;

         _editTextBox.PreviewKeyDown += _editTextBox_PreviewKeyDown;
      }

      private void _editTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Tab)
         {
            _editTextBox.SelectedText = string.Empty;

            IDataObject data = Clipboard.GetDataObject();

            string tab = new String(' ', 4);
            Clipboard.SetData(DataFormats.Text, tab);
            _editTextBox.Paste();

            if (data != null)
            {
               Clipboard.SetDataObject(data);
            }

            e.Handled = true;
         }
      }

      public static readonly DependencyProperty TextProperty =
          DependencyProperty.RegisterAttached(
          "Text",
          typeof(string),
          typeof(CustomTextBox),
          new UIPropertyMetadata("", new PropertyChangedCallback(OnTextChanged)));

      /// <summary>
      /// Gets or sets text
      /// </summary>
      public string Text
      {
         get { return (string)GetValue(TextProperty); }
         set { SetValue(TextProperty, value); }
      }

      private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
      {
         ((CustomTextBox)o).Text = (string)e.NewValue;
      }

      private void _textScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
      {
         _lineNumberScrollViewer.ScrollToVerticalOffset(_textScrollViewer.VerticalOffset);

         if (_textScrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
         {
            _lineNumberScrollViewer.Margin = new Thickness(0, 0, 0, 18);
         }
         else
         {
            _lineNumberScrollViewer.Margin = new Thickness(0);
         }
      }
   }
}
