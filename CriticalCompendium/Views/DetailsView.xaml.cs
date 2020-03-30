using System.Windows.Controls;

namespace CritCompendium.Views
{
	/// <summary>
	/// Interaction logic for DetailsView.xaml
	/// </summary>
	public partial class DetailsView : UserControl
	{
		public DetailsView(object detailsViewModel)
        {
            InitializeComponent();

            DataContext = this;

			DetailsViewModel = detailsViewModel;
		}

		public object DetailsViewModel
		{
			get;
			set;
		}
	}
}
