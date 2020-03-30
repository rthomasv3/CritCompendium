using System.Windows.Controls;
using CritCompendium.ViewModels.ObjectViewModels;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for CreateAttackView.xaml
    /// </summary>
    public partial class CreateAttackView : UserControl
    {
        public CreateAttackView(AttackViewModel attackViewModel)
        {
            InitializeComponent();
            
            DataContext = attackViewModel;
        }

        /// <summary>
        /// Gets view model
        /// </summary>
        public AttackViewModel ViewModel
        {
            get { return DataContext as AttackViewModel; }
        }
    }
}
