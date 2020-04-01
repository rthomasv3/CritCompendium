using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels
{
   public sealed class LevelUpViewModel : ObjectViewModel
   {
      #region Fields

      private readonly Compendium _compendium = DependencyResolver.Resolve<Compendium>();
      private readonly LevelEditViewModel _levelEditViewModel;
      private readonly Dictionary<KeyValuePair<Guid, string>, int> _classesMap;

      #endregion

      #region Constructor

      public LevelUpViewModel(Dictionary<KeyValuePair<Guid, string>, int> classesMap, int level)
      {
         _classesMap = classesMap;

         LevelModel levelModel = new LevelModel();
         levelModel.Level = level;
         if (_classesMap.Any())
         {
            levelModel.Class = _compendium.Classes.FirstOrDefault(x => _classesMap.Keys.Any(y => y.Key == x.ID));
         }
         if (levelModel.Class == null)
         {
            levelModel.Class = _compendium.Classes.FirstOrDefault();
         }
         levelModel.HitDieResult = levelModel.Class != null ? levelModel.Class.HitDie : 0;

         _levelEditViewModel = new LevelEditViewModel(levelModel);
         _levelEditViewModel.ClassChanged += _levelEditViewModel_ClassChanged;

         SetLevelOfClass();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets level
      /// </summary>
      public LevelEditViewModel Level
      {
         get { return _levelEditViewModel; }
      }

      #endregion

      #region Private Methods

      private void _levelEditViewModel_ClassChanged(object sender, EventArgs e)
      {
         SetLevelOfClass();
      }

      private void SetLevelOfClass()
      {
         KeyValuePair<KeyValuePair<Guid, string>, int> classPair = _classesMap.FirstOrDefault(x => x.Key.Key == _levelEditViewModel.Class.Item1);
         if (!classPair.Equals(default(KeyValuePair<KeyValuePair<Guid, string>, int>)))
         {
            _levelEditViewModel.LevelOfClass = classPair.Value + 1;
         }
         else
         {
            _levelEditViewModel.LevelOfClass = 1;
         }

         _levelEditViewModel.HitDieResult = _levelEditViewModel.LevelModel.Class.HitDie / 2 + 1;
      }

      #endregion
   }
}
