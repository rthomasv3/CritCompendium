using System.Windows.Controls;
using CritCompendium.ViewModels;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportView : UserControl
	{
        /// <summary>
        /// Creates an instance of <see cref="ImportView"/>
        /// </summary>
		public ImportView(ImportViewModel viewModel)
		{
			InitializeComponent();

            DataContext = viewModel;
		}

        /// <summary>
        /// Gets view model
        /// </summary>
        public ImportViewModel ViewModel
        {
            get { return DataContext as ImportViewModel; }
        }
	}
}
