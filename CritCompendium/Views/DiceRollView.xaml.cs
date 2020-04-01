using System.Windows;
using System.Windows.Controls;
using CritCompendium.ViewModels.DialogViewModels;
using CritCompendiumInfrastructure;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for DiceRollView.xaml
   /// </summary>
   public partial class DiceRollView : UserControl
   {
      private readonly DiceRollViewModel _viewModel = DependencyResolver.Resolve<DiceRollViewModel>();

      public DiceRollView()
      {
         InitializeComponent();

         DataContext = _viewModel;

         Loaded += DiceRollView_Loaded;
      }

      public DiceRollViewModel ViewModel
      {
         get { return _viewModel; }
      }

      private void DiceRollView_Loaded(object sender, RoutedEventArgs e)
      {
         _viewModel.RollCommand.Execute(null);

         RoutedEventArgs routedEventArgs = new RoutedEventArgs(Button.ClickEvent);
         _rollButton.RaiseEvent(routedEventArgs);
      }
   }
}
