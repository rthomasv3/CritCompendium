using System.Windows.Input;

namespace CritCompendium.ViewModels.DialogViewModels
{
   public interface ICopyInformation
   {
      ICommand CopyInformationCommand { get; }
   }
}
