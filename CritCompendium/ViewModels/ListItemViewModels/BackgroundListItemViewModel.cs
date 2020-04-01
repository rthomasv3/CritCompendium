using System;
using System.Linq;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ListItemViewModels
{
   public sealed class BackgroundListItemViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly StringService _stringService;
      private BackgroundModel _backgroundModel;
      private string _details;

      private bool _isSelected = false;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates an instance of <see cref="BackgroundListItemViewModel"/>
      /// </summary>
      public BackgroundListItemViewModel(BackgroundModel backgroundModel, StringService stringService)
      {
         _backgroundModel = backgroundModel;
         _stringService = stringService;

         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Background model
      /// </summary>
      public BackgroundModel BackgroundModel
      {
         get { return _backgroundModel; }
      }

      /// <summary>
      /// Background name
      /// </summary>
      public string Name
      {
         get { return _backgroundModel.Name; }
      }

      /// <summary>
      /// Background details
      /// </summary>
      public string Details
      {
         get { return _details; }
      }

      /// <summary>
      /// True if selected
      /// </summary>
      public bool IsSelected
      {
         get { return _isSelected; }
         set { Set(ref _isSelected, value); }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Updates the model
      /// </summary>
      public void UpdateModel(BackgroundModel backgroundModel)
      {
         _backgroundModel = backgroundModel;

         Initialize();

         OnPropertyChanged("");
      }

      #endregion

      #region Non-Public Methods

      private void Initialize()
      {
         _details = "None";

         if (_backgroundModel.Skills.Count > 0)
         {
            _details = String.Join(", ", _backgroundModel.Skills.Select(x => _stringService.GetString(x)));
         }
         else if (_backgroundModel.SkillsTraitIndex > -1 &&
                  _backgroundModel.SkillsTraitIndex < _backgroundModel.Traits.Count)
         {
            TraitModel trait = _backgroundModel.Traits[_backgroundModel.SkillsTraitIndex];
            _details = trait.TextCollection[0].Trim();
         }
         else if (_backgroundModel.StartingTraitIndex > -1 &&
                  _backgroundModel.StartingTraitIndex < _backgroundModel.Traits.Count)
         {
            foreach (string text in _backgroundModel.Traits[_backgroundModel.StartingTraitIndex].TextCollection)
            {
               if (text.Contains("Skills: "))
               {
                  _details = text.Replace("Skills: ", "").Trim();
                  break;
               }
            }
         }
      }

      #endregion
   }
}
