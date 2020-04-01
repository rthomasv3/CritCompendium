using System.Windows.Controls;
using CritCompendium.ViewModels;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for AboutView.xaml
   /// </summary>
   public partial class AboutView : UserControl
   {
      /// <summary>
      /// Creates an instance of <see cref="AboutView"/>
      /// </summary>
      public AboutView(AboutViewModel viewModel)
      {
         InitializeComponent();

         DataContext = viewModel;
      }

      /// <summary>
      /// Gets view model
      /// </summary>
      public AboutViewModel ViewModel
      {
         get { return DataContext as AboutViewModel; }
      }
   }
}
