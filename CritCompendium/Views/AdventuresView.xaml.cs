using System.Linq;
using System.Windows.Controls;
using CritCompendium.ViewModels;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for AdventuresView.xaml
    /// </summary>
    public partial class AdventuresView : UserControl
    {
        AdventuresViewModel _viewModel = DependencyResolver.Resolve<AdventuresViewModel>();

        public AdventuresView()
        {
            InitializeComponent();

            _viewModel.Search();

            DataContext = _viewModel;

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AdventuresViewModel.SelectedAdventure))
            {
                ListItemViewModel<AdventureModel> selected = _viewModel.Adventures.FirstOrDefault(x => x.IsSelected);
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
