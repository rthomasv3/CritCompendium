using System.Linq;
using System.Windows.Controls;
using CritCompendium.ViewModels;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for TablesView.xaml
    /// </summary>
    public partial class TablesView : UserControl
    {
        TablesViewModel _viewModel = DependencyResolver.Resolve<TablesViewModel>();

        public TablesView()
        {
            InitializeComponent();

            _viewModel.Search();

            DataContext = _viewModel;

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TablesViewModel.SelectedTable))
            {
                ListItemViewModel<RandomTableModel> selected = _viewModel.Tables.FirstOrDefault(x => x.IsSelected);
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
