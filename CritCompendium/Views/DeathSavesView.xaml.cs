using System.Windows.Controls;
using CritCompendium.ViewModels;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for DeathSavesView.xaml
   /// </summary>
   public partial class DeathSavesView : UserControl
   {
      /// <summary>
      /// Creates an instance of <see cref="DeathSavesView"/>
      /// </summary>
      public DeathSavesView(DeathSavesViewModel viewModel)
      {
         InitializeComponent();

         DataContext = viewModel;
      }

      /// <summary>
      /// Gets view model
      /// </summary>
      public DeathSavesViewModel ViewModel
      {
         get { return DataContext as DeathSavesViewModel; }
      }
   }
}
