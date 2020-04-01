using System.Linq;
using System.Windows.Controls;
using CritCompendium.ViewModels;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendiumInfrastructure;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for CharactersView.xaml
   /// </summary>
   public partial class CharactersView : UserControl
   {
      private readonly CharactersViewModel _viewModel = DependencyResolver.Resolve<CharactersViewModel>();

      public CharactersView()
      {
         InitializeComponent();

         _viewModel.Search();

         DataContext = _viewModel;

         _viewModel.PropertyChanged += ViewModel_PropertyChanged;
      }

      private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (e.PropertyName == nameof(CharactersViewModel.SelectedCharacter))
         {
            CharacterListItemViewModel selected = _viewModel.Characters.FirstOrDefault(x => x.IsSelected);
            if (selected != null)
            {
               if (_tree.ItemContainerGenerator.ContainerFromItem(selected) is TreeViewItem item)
               {
                  item.BringIntoView();
               }
            }
         }
      }
   }
}
