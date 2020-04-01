using System.Windows.Controls;
using CritCompendium.ViewModels.SearchViewModels;
using CritCompendiumInfrastructure;

namespace CritCompendium.Views.SearchViews
{
   /// <summary>
   /// Interaction logic for ItemSearchView.xaml
   /// </summary>
   public partial class ItemSearchView : UserControl
   {
      ItemSearchViewModel _viewModel = DependencyResolver.Resolve<ItemSearchViewModel>();

      public ItemSearchView()
      {
         DataContext = _viewModel;

         InitializeComponent();
      }

      /// <summary>
      /// Gets view model
      /// </summary>
      public ItemSearchViewModel ViewModel
      {
         get { return _viewModel; }
      }
   }
}
