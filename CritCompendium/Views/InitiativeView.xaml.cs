using System.Windows.Controls;
using CritCompendium.ViewModels.DialogViewModels;
using CritCompendiumInfrastructure;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for InitiativeView.xaml
    /// </summary>
    public partial class InitiativeView : UserControl
    {
        private readonly InitiativeViewModel _viewModel = DependencyResolver.Resolve<InitiativeViewModel>();

        public InitiativeView()
        {
            InitializeComponent();

            DataContext = _viewModel;
        }

        public InitiativeViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
