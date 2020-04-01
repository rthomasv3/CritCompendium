using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class ArmorClassViewModel : ObjectViewModel
   {
      #region Fields

      private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
      private readonly ArmorClassModel _armorClassModel;
      private readonly List<KeyValuePair<ArmorType, string>> _armorTypeOptions = new List<KeyValuePair<ArmorType, string>>();
      private readonly List<KeyValuePair<Ability, string>> _abilityOptions = new List<KeyValuePair<Ability, string>>();
      private KeyValuePair<ArmorType, string> _selectedArmorTypeOption;
      private KeyValuePair<Ability, string> _selectedFirstAbilityOption;
      private KeyValuePair<Ability, string> _selectedSecondAbilityOption;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="ArmorClassViewModel"/>
      /// </summary>
      public ArmorClassViewModel(ArmorClassModel armorClassModel)
      {
         _armorClassModel = armorClassModel;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets id
      /// </summary>
      public Guid ID
      {
         get { return _armorClassModel.ID; }
         set { _armorClassModel.ID = value; }
      }

      /// <summary>
      /// Gets armor class model
      /// </summary>
      public ArmorClassModel ArmorClassModel
      {
         get { return _armorClassModel; }
      }

      /// <summary>
      /// Gets or sets armor bonus
      /// </summary>
      public int ArmorBonus
      {
         get { return _armorClassModel.ArmorBonus; }
         set { _armorClassModel.ArmorBonus = value; }
      }

      /// <summary>
      /// Gets armor type options
      /// </summary>
      public List<KeyValuePair<ArmorType, string>> ArmorTypeOptions
      {
         get { return _armorTypeOptions; }
      }

      /// <summary>
      /// Gets or sets selected armor type option
      /// </summary>
      public KeyValuePair<ArmorType, string> SelectedArmorTypeOption
      {
         get { return _selectedArmorTypeOption; }
         set
         {
            _selectedArmorTypeOption = value;
            _armorClassModel.ArmorType = value.Key;
            OnPropertyChanged(nameof(ArmorType));
         }
      }

      /// <summary>
      /// Gets or sets armor type
      /// </summary>
      public ArmorType ArmorType
      {
         get { return _armorClassModel.ArmorType; }
         set { _armorClassModel.ArmorType = value; }
      }

      /// <summary>
      /// Gets ability options
      /// </summary>
      public List<KeyValuePair<Ability, string>> AbilityOptions
      {
         get { return _abilityOptions; }
      }

      /// <summary>
      /// Gets or sets selected first ability option
      /// </summary>
      public KeyValuePair<Ability, string> SelectedFirstAbilityOption
      {
         get { return _selectedFirstAbilityOption; }
         set
         {
            _selectedFirstAbilityOption = value;
            _armorClassModel.FirstAbility = value.Key;
            OnPropertyChanged(nameof(FirstAbility));
         }
      }

      /// <summary>
      /// Gets or sets first ability
      /// </summary>
      public Ability FirstAbility
      {
         get { return _armorClassModel.FirstAbility; }
         set { _armorClassModel.FirstAbility = value; }
      }

      /// <summary>
      /// Gets or sets selected second ability option
      /// </summary>
      public KeyValuePair<Ability, string> SelectedSecondAbilityOption
      {
         get { return _selectedSecondAbilityOption; }
         set
         {
            _selectedSecondAbilityOption = value;
            _armorClassModel.SecondAbility = value.Key;
            OnPropertyChanged(nameof(SecondAbility));
         }
      }

      /// <summary>
      /// Gets or sets second ability
      /// </summary>
      public Ability SecondAbility
      {
         get { return _armorClassModel.SecondAbility; }
         set { _armorClassModel.SecondAbility = value; }
      }

      /// <summary>
      /// Gets or sets item bonus
      /// </summary>
      public int ItemBonus
      {
         get { return _armorClassModel.ItemBonus; }
         set { _armorClassModel.ItemBonus = value; }
      }

      /// <summary>
      /// Gets or sets additional bonus
      /// </summary>
      public int AdditionalBonus
      {
         get { return _armorClassModel.AdditionalBonus; }
         set { _armorClassModel.AdditionalBonus = value; }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Initializes options
      /// </summary>
      public void InitializeOptions()
      {
         _armorTypeOptions.Add(new KeyValuePair<ArmorType, string>(ArmorType.None, "None"));
         _armorTypeOptions.Add(new KeyValuePair<ArmorType, string>(ArmorType.Light_Armor, _stringService.GetString(ArmorType.Light_Armor)));
         _armorTypeOptions.Add(new KeyValuePair<ArmorType, string>(ArmorType.Medium_Armor, _stringService.GetString(ArmorType.Medium_Armor)));
         _armorTypeOptions.Add(new KeyValuePair<ArmorType, string>(ArmorType.Heavy_Armor, _stringService.GetString(ArmorType.Heavy_Armor)));

         KeyValuePair<ArmorType, string> armorType = _armorTypeOptions.FirstOrDefault(x => x.Key == _armorClassModel.ArmorType);
         if (!armorType.Equals(default(KeyValuePair<ArmorType, string>)))
         {
            _selectedArmorTypeOption = armorType;
         }
         else
         {
            _selectedArmorTypeOption = _armorTypeOptions[0];
         }

         _abilityOptions.Add(new KeyValuePair<Ability, string>(Ability.None, "None"));
         foreach (Ability ability in Enum.GetValues(typeof(Ability)))
         {
            if (ability != Ability.None)
            {
               _abilityOptions.Add(new KeyValuePair<Ability, string>(ability, _stringService.GetString(ability)));
            }
         }

         KeyValuePair<Ability, string> firstAbility = _abilityOptions.FirstOrDefault(x => x.Key == _armorClassModel.FirstAbility);
         if (!firstAbility.Equals(default(KeyValuePair<Ability, string>)))
         {
            _selectedFirstAbilityOption = firstAbility;
         }
         else
         {
            _selectedFirstAbilityOption = _abilityOptions[0];
         }

         KeyValuePair<Ability, string> secondAbility = _abilityOptions.FirstOrDefault(x => x.Key == _armorClassModel.SecondAbility);
         if (!secondAbility.Equals(default(KeyValuePair<Ability, string>)))
         {
            _selectedSecondAbilityOption = secondAbility;
         }
         else
         {
            _selectedSecondAbilityOption = _abilityOptions[0];
         }
      }

      #endregion
   }
}
