using System.Linq;
using System.Windows.Controls;
using CritCompendium.ViewModels;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendiumInfrastructure;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for EncountersView.xaml
    /// </summary>
    public partial class EncountersView : UserControl
	{
        EncountersViewModel _viewModel = DependencyResolver.Resolve<EncountersViewModel>();

        public EncountersView()
        {
            InitializeComponent();

            _viewModel.Search();

            DataContext = _viewModel;

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EncountersViewModel.SelectedEncounter))
            {
                EncounterListItemViewModel selected = _viewModel.Encounters.FirstOrDefault(x => x.IsSelected);
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
