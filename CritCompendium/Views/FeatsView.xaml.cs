﻿using System.Linq;
using System.Windows.Controls;
using CritCompendium.ViewModels;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendiumInfrastructure;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for FeatsView.xaml
   /// </summary>
   public partial class FeatsView : UserControl
   {
      private FeatsViewModel _viewModel = DependencyResolver.Resolve<FeatsViewModel>();

      public FeatsView()
      {
         InitializeComponent();

         _viewModel.Search();

         DataContext = _viewModel;

         _viewModel.PropertyChanged += ViewModel_PropertyChanged;
      }

      private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (e.PropertyName == nameof(FeatsViewModel.SelectedFeat))
         {
            FeatListItemViewModel selected = _viewModel.Feats.FirstOrDefault(x => x.IsSelected);
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
