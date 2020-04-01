using System.Collections.Generic;
using System.Windows.Controls;
using CritCompendium.ViewModels;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for ShortRestView.xaml
   /// </summary>
   public partial class ShortRestView : UserControl
   {
      /// <summary>
      /// Creates an instance of <see cref="ShortRestView"/>
      /// </summary>
      public ShortRestView(ShortRestViewModel shortRestViewModel)
      {
         InitializeComponent();

         DataContext = shortRestViewModel;
      }

      /// <summary>
      /// Gets view model
      /// </summary>
      public ShortRestViewModel ViewModel
      {
         get { return DataContext as ShortRestViewModel; }
      }
   }
}
