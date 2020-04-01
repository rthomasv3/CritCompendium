using System.Windows.Controls;
using CritCompendiumInfrastructure;
using CritCompendium.ViewModels;
using CritCompendium.ViewModels.ListItemViewModels;
using System.Linq;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for SpellsView.xaml
   /// </summary>
   public partial class SpellsView : UserControl
   {
      private SpellsViewModel _viewModel = DependencyResolver.Resolve<SpellsViewModel>();

      public SpellsView()
      {
         InitializeComponent();

         _viewModel.Search();

         DataContext = _viewModel;

         _viewModel.PropertyChanged += ViewModel_PropertyChanged;
      }

      private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (e.PropertyName == nameof(SpellsViewModel.SelectedSpell))
         {
            SpellListItemViewModel selected = _viewModel.Spells.FirstOrDefault(x => x.IsSelected);
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
