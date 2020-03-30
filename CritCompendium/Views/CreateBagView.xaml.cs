using System.Windows.Controls;
using CritCompendium.ViewModels.ObjectViewModels;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for CreateBagView.xaml
    /// </summary>
    public partial class CreateBagView : UserControl
    {
        private BagViewModel _viewModel;

        public CreateBagView(BagViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            DataContext = _viewModel;
        }

        /// <summary>
        /// Gets view model
        /// </summary>
        public BagViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
