using System.Windows.Controls;
using CritCompendium.ViewModels.ObjectViewModels;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for CreateSpellbookView.xaml
   /// </summary>
   public partial class CreateSpellbookView : UserControl
   {
      public CreateSpellbookView(SpellbookViewModel viewModel)
      {
         InitializeComponent();
         DataContext = viewModel;
      }

      /// <summary>
      /// Gets view model
      /// </summary>
      public SpellbookViewModel ViewModel
      {
         get { return DataContext as SpellbookViewModel; }
      }
   }
}
