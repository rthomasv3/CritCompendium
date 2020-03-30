using System.Windows.Controls;
using CritCompendium.ViewModels.ObjectViewModels;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for CreateCounterView.xaml
    /// </summary>
    public partial class CreateCounterView : UserControl
    {
        private CounterViewModel _viewModel;

        public CreateCounterView(CounterViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            DataContext = _viewModel;
        }

        /// <summary>
        /// Gets view model
        /// </summary>
        public CounterViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
