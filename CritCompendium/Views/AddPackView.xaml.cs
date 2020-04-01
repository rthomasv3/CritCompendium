using System.Windows.Controls;
using CritCompendium.ViewModels.ObjectViewModels;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for AddPackView.xaml
   /// </summary>
   public partial class AddPackView : UserControl
   {
      private PackViewModel _viewModel;

      public AddPackView(PackViewModel packViewModel)
      {
         InitializeComponent();

         _viewModel = packViewModel;

         DataContext = _viewModel;
      }

      /// <summary>
      /// Gets view model
      /// </summary>
      public PackViewModel ViewModel
      {
         get { return _viewModel; }
      }
   }
}
