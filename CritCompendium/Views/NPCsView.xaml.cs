using System.Linq;
using System.Windows.Controls;
using CritCompendium.ViewModels;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for NPCsView.xaml
    /// </summary>
    public partial class NPCsView : UserControl
    {
        NPCsViewModel _viewModel = DependencyResolver.Resolve<NPCsViewModel>();

        public NPCsView()
        {
            InitializeComponent();

            _viewModel.Search();

            DataContext = _viewModel;

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NPCsViewModel.SelectedNPC))
            {
                ListItemViewModel<NPCModel> selected = _viewModel.NPCs.FirstOrDefault(x => x.IsSelected);
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
