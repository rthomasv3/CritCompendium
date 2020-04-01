using System.Windows.Controls;
using CritCompendium.ViewModels.DialogViewModels;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for AddSubtractView.xaml
   /// </summary>
   public partial class AddSubtractView : UserControl
   {
      public AddSubtractView(AddSubtractViewModel viewModel)
      {
         InitializeComponent();

         DataContext = viewModel;

         Loaded += AddSubtractView_Loaded;
      }

      public AddSubtractViewModel ViewModel
      {
         get { return DataContext as AddSubtractViewModel; }
      }

      private void AddSubtractView_Loaded(object sender, System.Windows.RoutedEventArgs e)
      {
         _amountTextBox.Focus();
         _amountTextBox.SelectAll();
      }
   }
}
