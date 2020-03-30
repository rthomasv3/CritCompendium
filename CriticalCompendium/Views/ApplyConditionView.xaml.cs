using System.Windows.Controls;
using CritCompendium.ViewModels.ObjectViewModels;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for ApplyConditionView.xaml
    /// </summary>
    public partial class ApplyConditionView : UserControl
    {
        private AppliedConditionViewModel _viewModel;

        public ApplyConditionView(AppliedConditionViewModel viewModel)
        {
            InitializeComponent();
            
            _viewModel = viewModel;

            DataContext = _viewModel;
        }

        /// <summary>
        /// Gets view model
        /// </summary>
        public AppliedConditionViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
