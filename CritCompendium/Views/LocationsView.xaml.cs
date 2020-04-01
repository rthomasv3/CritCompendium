using System.Linq;
using System.Windows.Controls;
using CritCompendium.ViewModels;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for LocationsView.xaml
   /// </summary>
   public partial class LocationsView : UserControl
   {
      LocationsViewModel _viewModel = DependencyResolver.Resolve<LocationsViewModel>();

      public LocationsView()
      {
         InitializeComponent();

         _viewModel.Search();

         DataContext = _viewModel;

         _viewModel.PropertyChanged += ViewModel_PropertyChanged;
      }

      private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (e.PropertyName == nameof(LocationsViewModel.SelectedLocation))
         {
            ListItemViewModel<LocationModel> selected = _viewModel.Locations.FirstOrDefault(x => x.IsSelected);
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
