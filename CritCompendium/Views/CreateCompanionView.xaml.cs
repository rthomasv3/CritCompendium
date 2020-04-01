using System.Windows.Controls;
using CritCompendium.ViewModels.ObjectViewModels;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for CreateCompanionView.xaml
   /// </summary>
   public partial class CreateCompanionView : UserControl
   {
      private CompanionViewModel _viewModel;

      public CreateCompanionView(CompanionViewModel viewModel)
      {
         InitializeComponent();

         _viewModel = viewModel;

         DataContext = _viewModel;
      }

      /// <summary>
      /// Gets view model
      /// </summary>
      public CompanionViewModel ViewModel
      {
         get { return _viewModel; }
      }
   }
}
