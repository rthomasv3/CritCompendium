using System.Windows.Controls;
using CritCompendium.ViewModels.SearchViewModels;
using CritCompendiumInfrastructure;

namespace CritCompendium.Views.SearchViews
{
    /// <summary>
    /// Interaction logic for MonsterSelectionView.xaml
    /// </summary>
    public partial class MonsterSearchView : UserControl
    {
        MonsterSearchViewModel _viewModel = DependencyResolver.Resolve<MonsterSearchViewModel>();

        public MonsterSearchView()
        {
            DataContext = _viewModel;

            InitializeComponent();
        }

        /// <summary>
        /// Gets view model
        /// </summary>
        public MonsterSearchViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
