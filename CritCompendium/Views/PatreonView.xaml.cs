using System.Windows.Controls;
using CritCompendium.ViewModels;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for PatreonView.xaml
    /// </summary>
    public partial class PatreonView : UserControl
    {
        /// <summary>
        /// Creates an instance of <see cref="PatreonView"/>
        /// </summary>
        public PatreonView(PatreonDialogViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        /// <summary>
        /// Gets view model
        /// </summary>
        public PatreonDialogViewModel ViewModel
        {
            get { return DataContext as PatreonDialogViewModel; }
        }
    }
}
