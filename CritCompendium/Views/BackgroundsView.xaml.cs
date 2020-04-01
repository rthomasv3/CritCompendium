using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CritCompendium.ViewModels;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendiumInfrastructure;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for BackgroundsView.xaml
   /// </summary>
   public partial class BackgroundsView : UserControl
   {
      BackgroundsViewModel _viewModel = DependencyResolver.Resolve<BackgroundsViewModel>();

      public BackgroundsView()
      {
         InitializeComponent();

         _viewModel.Search();

         DataContext = _viewModel;

         _viewModel.PropertyChanged += ViewModel_PropertyChanged;
      }

      private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (e.PropertyName == nameof(BackgroundsViewModel.SelectedBackground))
         {
            BackgroundListItemViewModel selected = _viewModel.Backgrounds.FirstOrDefault(x => x.IsSelected);
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
