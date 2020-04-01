using System.Windows.Controls;
using CritCompendium.ViewModels.ObjectViewModels;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for CreateEquipmentView.xaml
   /// </summary>
   public partial class CreateEquipmentView : UserControl
   {
      private EquipmentViewModel _viewModel;

      public CreateEquipmentView(EquipmentViewModel viewModel)
      {
         InitializeComponent();

         _viewModel = viewModel;

         DataContext = _viewModel;
      }

      /// <summary>
      /// Gets view model
      /// </summary>
      public EquipmentViewModel ViewModel
      {
         get { return _viewModel; }
      }
   }
}
