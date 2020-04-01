using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using System;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class AbilityViewModel : NotifyPropertyChanged
   {
      #region Events

      public event EventHandler ProficiencyChanged;

      #endregion

      #region Fields

      private readonly AbilityModel _abilityModel;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of <see cref="AbilityViewModel"/>
      /// </summary>
      public AbilityViewModel(AbilityModel abilityModel)
      {
         _abilityModel = abilityModel;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets ability model
      /// </summary>
      public AbilityModel AbilityModel
      {
         get { return _abilityModel; }
      }

      /// <summary>
      /// Gets ability
      /// </summary>
      public Ability Ability
      {
         get { return _abilityModel.Ability; }
      }

      /// <summary>
      /// Gets ability string
      /// </summary>
      public string AbilityString
      {
         get { return _abilityModel.AbilityString; }
      }

      /// <summary>
      /// Gets or sets save value
      /// </summary>
      public int SaveBonus
      {
         get { return _abilityModel.SaveBonus; }
         set
         {
            if (_abilityModel.SaveBonus != value)
            {
               _abilityModel.SaveBonus = value;
               OnPropertyChanged(nameof(SaveBonus));
               OnPropertyChanged(nameof(SaveBonusString));
            }
         }
      }

      /// <summary>
      /// Gets or sets check value
      /// </summary>
      public int CheckBonus
      {
         get { return _abilityModel.CheckBonus; }
         set
         {
            if (_abilityModel.CheckBonus != value)
            {
               _abilityModel.CheckBonus = value;
               OnPropertyChanged(nameof(CheckBonus));
               OnPropertyChanged(nameof(CheckBonusString));
            }
         }
      }

      /// <summary>
      /// Gets save bonus string
      /// </summary>
      public string SaveBonusString
      {
         get { return _abilityModel.SaveBonusString; }
      }

      /// <summary>
      /// Gets check bonus string
      /// </summary>
      public string CheckBonusString
      {
         get { return _abilityModel.CheckBonusString; }
      }

      /// <summary>
      /// Gets or set proficient
      /// </summary>
      public bool Proficient
      {
         get { return _abilityModel.Proficient; }
         set
         {
            if (_abilityModel.Proficient != value)
            {
               _abilityModel.Proficient = value;
               OnPropertyChanged(nameof(Proficient));
               ProficiencyChanged?.Invoke(this, EventArgs.Empty);
            }
         }
      }

      #endregion
   }
}
