using System.Windows.Controls;
using CritCompendiumInfrastructure;
using CritCompendium.ViewModels;
using System.Linq;
using CritCompendium.ViewModels.ListItemViewModels;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for RacesView.xaml
   /// </summary>
   public partial class RacesView : UserControl
   {
      private RacesViewModel _viewModel = DependencyResolver.Resolve<RacesViewModel>();

      public RacesView()
      {
         InitializeComponent();

         _viewModel.Search();

         DataContext = _viewModel;

         _viewModel.PropertyChanged += ViewModel_PropertyChanged;
      }

      private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (e.PropertyName == nameof(RacesViewModel.SelectedRace))
         {
            RaceListItemViewModel selected = _viewModel.Races.FirstOrDefault(x => x.IsSelected);
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
