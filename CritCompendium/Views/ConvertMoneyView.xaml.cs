using System.Windows.Controls;
using CritCompendium.ViewModels;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for ConvertMoneyView.xaml
   /// </summary>
   public partial class ConvertMoneyView : UserControl
   {
      /// <summary>
      /// Creates an instance of <see cref="ConvertMoneyView"/>
      /// </summary>
      public ConvertMoneyView(ConvertMoneyViewModel convertMoneyViewModel)
      {
         InitializeComponent();

         DataContext = convertMoneyViewModel;
      }

      /// <summary>
      /// Gets view model
      /// </summary>
      public ConvertMoneyViewModel ViewModel
      {
         get { return DataContext as ConvertMoneyViewModel; }
      }
   }
}
