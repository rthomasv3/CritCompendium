using System.Windows.Controls;
using CritCompendium.ViewModels.SearchViewModels;
using CritCompendiumInfrastructure;

namespace CritCompendium.Views.SearchViews
{
    /// <summary>
    /// Interaction logic for SpellSearchView.xaml
    /// </summary>
    public partial class SpellSearchView : UserControl
    {
        SpellSearchViewModel _viewModel = DependencyResolver.Resolve<SpellSearchViewModel>();

        public SpellSearchView()
        {
            DataContext = _viewModel;

            InitializeComponent();
        }

        /// <summary>
        /// Gets view model
        /// </summary>
        public SpellSearchViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
