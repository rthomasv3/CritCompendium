using System.Collections.Generic;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class AutoLevelViewModel
   {
      #region Fields

      private readonly AutoLevelModel _autoLevelModel;
      private readonly List<FeatureViewModel> _features = new List<FeatureViewModel>();

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="AutoLevelViewModel"/>
      /// </summary>
      public AutoLevelViewModel(AutoLevelModel autoLevelModel)
      {
         _autoLevelModel = autoLevelModel;

         foreach (FeatureModel featureModel in _autoLevelModel.Features)
         {
            _features.Add(new FeatureViewModel(featureModel));
         }
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets level
      /// </summary>
      public int Level
      {
         get { return _autoLevelModel.Level; }
      }

      /// <summary>
      /// Gets features
      /// </summary>
      public List<FeatureViewModel> Features
      {
         get { return _features; }
      }

      #endregion
   }
}
