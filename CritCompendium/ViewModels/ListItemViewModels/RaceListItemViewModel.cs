using System.Collections.Generic;
using System.Linq;
using System.Text;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ListItemViewModels
{
   public sealed class RaceListItemViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly StringService _stringService;
      private RaceModel _raceModel;
      private string _abilities;
      private bool _isSelected = false;

      #endregion

      #region Constructors

      /// <summary>
      /// Creats an instance of <see cref="RaceListItemViewModel"/>
      /// </summary>
      public RaceListItemViewModel(RaceModel raceModel, StringService stringService)
      {
         _raceModel = raceModel;
         _stringService = stringService;

         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets race model
      /// </summary>
      public RaceModel RaceModel
      {
         get { return _raceModel; }
      }

      /// <summary>
      /// Gets name
      /// </summary>
      public string Name
      {
         get { return _raceModel.Name; }
      }

      /// <summary>
      /// Gets abilities
      /// </summary>
      public string Abilities
      {
         get { return _abilities; }
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
      public void UpdateModel(RaceModel raceModel)
      {
         _raceModel = raceModel;

         Initialize();

         OnPropertyChanged("");
      }

      #endregion

      #region Non-Public Methods

      private void Initialize()
      {
         if (_raceModel.Abilities.Count > 0)
         {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < _raceModel.Abilities.Count; ++i)
            {
               KeyValuePair<Ability, int> pair = _raceModel.Abilities.ElementAt(i);

               string abilityString = _stringService.GetString(pair.Key);
               stringBuilder.Append(abilityString);
               if (pair.Value > 0)
               {
                  stringBuilder.Append(" +");
               }
               else
               {
                  stringBuilder.Append(" ");
               }
               stringBuilder.Append(pair.Value);

               if (i + 1 < _raceModel.Abilities.Count)
               {
                  stringBuilder.Append(", ");
               }
            }

            _abilities = stringBuilder.ToString();
         }
         else
         {
            _abilities = "None";
         }
      }

      #endregion
   }
}
