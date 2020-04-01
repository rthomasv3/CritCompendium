using System.Linq;
using System.Windows.Controls;
using CritCompendium.ViewModels;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendiumInfrastructure;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for ConditionsView.xaml
    /// </summary>
    public partial class ConditionsView : UserControl
	{
		private readonly ConditionsViewModel _viewModel = DependencyResolver.Resolve<ConditionsViewModel>();

		public ConditionsView()
        {
			InitializeComponent();

            _viewModel.Search();

            DataContext = _viewModel;

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ConditionsViewModel.SelectedCondition))
            {
                ConditionListItemViewModel selected = _viewModel.Conditions.FirstOrDefault(x => x.IsSelected);
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
