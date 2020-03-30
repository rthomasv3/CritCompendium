using System.Windows.Controls;
using CritCompendium.ViewModels;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for LevelUpView.xaml
    /// </summary>
    public partial class LevelUpView : UserControl
    {
        /// <summary>
        /// Creates an instance of <see cref="LevelUpView"/>
        /// </summary>
        public LevelUpView(LevelUpViewModel levelUpViewModel)
        {
            InitializeComponent();

            DataContext = levelUpViewModel;
        }

        /// <summary>
        /// Gets view model
        /// </summary>
        public LevelUpViewModel ViewModel
        {
            get { return DataContext as LevelUpViewModel; }
        }
    }
}
