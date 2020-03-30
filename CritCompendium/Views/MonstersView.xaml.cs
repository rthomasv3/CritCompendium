using System.Windows.Controls;
using CriticalCompendiumInfrastructure;
using CritCompendium.ViewModels;
using CritCompendium.ViewModels.ListItemViewModels;
using System.Linq;

namespace CritCompendium.Views
{
	/// <summary>
	/// Interaction logic for MonstersView.xaml
	/// </summary>
	public partial class MonstersView : UserControl
	{
		private MonstersViewModel _viewModel = DependencyResolver.Resolve<MonstersViewModel>();

		public MonstersView()
        {
            InitializeComponent();

            _viewModel.Search();

			DataContext = _viewModel;

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MonstersViewModel.SelectedMonster))
            {
                MonsterListItemViewModel selected = _viewModel.Monsters.FirstOrDefault(x => x.IsSelected);
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
