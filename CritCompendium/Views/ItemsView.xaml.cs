using System.Windows.Controls;
using CritCompendium.ViewModels;
using CritCompendiumInfrastructure;
using System.Linq;
using System.Windows;
using CritCompendium.ViewModels.ListItemViewModels;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for ItemsView.xaml
   /// </summary>
   public partial class ItemsView : UserControl
   {
      private readonly ItemsViewModel _viewModel = DependencyResolver.Resolve<ItemsViewModel>();

      public ItemsView()
      {
         InitializeComponent();

         _viewModel.Search();

         DataContext = _viewModel;

         _viewModel.PropertyChanged += ViewModel_PropertyChanged;
      }

      private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (e.PropertyName == nameof(ItemsViewModel.SelectedItem))
         {
            ItemListItemViewModel selected = _viewModel.Items.FirstOrDefault(x => x.IsSelected);
            if (selected != null)
            {
               if (_tree.ItemContainerGenerator.ContainerFromItem(selected) is TreeViewItem item)
               {
                  item.BringIntoView();
               }
            }
         }
      }
   }
}
