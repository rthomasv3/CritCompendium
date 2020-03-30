using System.Windows.Controls;
using CritCompendium.ViewModels.ObjectViewModels;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for MovementView.xaml
    /// </summary>
    public partial class MovementView : UserControl
    {
        /// <summary>
        /// Creates an instance of <see cref="MovementView"/>
        /// </summary>
        public MovementView(MovementViewModel movementViewModel)
        {
            InitializeComponent();

            DataContext = movementViewModel;
        }

        /// <summary>
        /// Gets view model
        /// </summary>
        public MovementViewModel ViewModel
        {
            get { return DataContext as MovementViewModel; }
        }
    }
}
