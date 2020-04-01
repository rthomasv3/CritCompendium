using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendium.Business;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public class AttackViewModel : ObjectViewModel
   {
      #region Fields

      private readonly StatService _statService = DependencyResolver.Resolve<StatService>();
      private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
      private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();
      private readonly AttackModel _attackModel;
      private readonly List<KeyValuePair<Ability, string>> _abilities = new List<KeyValuePair<Ability, string>>();
      private readonly List<KeyValuePair<DamageType, string>> _damageTypes = new List<KeyValuePair<DamageType, string>>();
      private readonly List<string> _dice = new List<string>();

      private KeyValuePair<Ability, string> _selectedAbility;
      private KeyValuePair<DamageType, string> _selectedDamageType;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="AttackModel"/>
      /// </summary>
      public AttackViewModel(AttackModel attackModel)
      {
         _attackModel = attackModel;

         _abilities.Add(new KeyValuePair<Ability, string>(Ability.None, "None"));
         foreach (Tuple<string, Ability> ability in _statService.Abilities)
         {
            _abilities.Add(new KeyValuePair<Ability, string>(ability.Item2, ability.Item1));
         }
         _selectedAbility = _abilities.FirstOrDefault(x => x.Key == attackModel.Ability);

         _damageTypes.Add(new KeyValuePair<DamageType, string>(DamageType.None, "None"));
         foreach (KeyValuePair<DamageType, string> damageType in _statService.DamageTypes)
         {
            _damageTypes.Add(new KeyValuePair<DamageType, string>(damageType.Key, damageType.Value));
         }
         _selectedDamageType = _damageTypes.FirstOrDefault(x => x.Key == attackModel.DamageType);

         _dice.AddRange(_statService.Dice);

         if (String.IsNullOrWhiteSpace(_attackModel.DamageDie))
         {
            _attackModel.DamageDie = _dice[0];
         }
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets id
      /// </summary>
      public Guid ID
      {
         get { return _attackModel.ID; }
      }

      /// <summary>
      /// Gets or sets attack model
      /// </summary>
      public AttackModel AttackModel
      {
         get { return _attackModel; }
      }

      /// <summary>
      /// Gets or sets name
      /// </summary>
      public string Name
      {
         get { return _attackModel.Name; }
         set { _attackModel.Name = value; }
      }

      /// <summary>
      /// Gets or sets number Of damage dice
      /// </summary>
      public int NumberOfDamageDice
      {
         get { return _attackModel.NumberOfDamageDice; }
         set { _attackModel.NumberOfDamageDice = value; }
      }

      /// <summary>
      /// Gets dice
      /// </summary>
      public List<string> Dice
      {
         get { return _dice; }
      }

      /// <summary>
      /// Gets or sets damage die
      /// </summary>
      public string DamageDie
      {
         get { return _attackModel.DamageDie; }
         set { _attackModel.DamageDie = value; }
      }

      /// <summary>
      /// Gets or sets abilities
      /// </summary>
      public List<KeyValuePair<Ability, string>> Abilities
      {
         get { return _abilities; }
      }

      /// <summary>
      /// Gets or sets ability
      /// </summary>
      public Ability Ability
      {
         get { return _attackModel.Ability; }
         set { _attackModel.Ability = value; }
      }

      /// <summary>
      /// Gets or sets selected ability
      /// </summary>
      public KeyValuePair<Ability, string> SelectedAbility
      {
         get { return _selectedAbility; }
         set { _attackModel.Ability = value.Key; }
      }

      /// <summary>
      /// Gets or sets proficient
      /// </summary>
      public bool Proficient
      {
         get { return _attackModel.Proficient; }
         set { _attackModel.Proficient = value; }
      }

      /// <summary>
      /// Gets or sets damageTypes
      /// </summary>
      public List<KeyValuePair<DamageType, string>> DamageTypes
      {
         get { return _damageTypes; }
      }

      /// <summary>
      /// Gets or sets damage type
      /// </summary>
      public DamageType DamageType
      {
         get { return _attackModel.DamageType; }
         set { _attackModel.DamageType = value; }
      }

      /// <summary>
      /// Gets or sets selected damage type
      /// </summary>
      public KeyValuePair<DamageType, string> SelectedDamageType
      {
         get { return _selectedDamageType; }
         set { _attackModel.DamageType = value.Key; }
      }

      /// <summary>
      /// Gets or sets additional to hit bonus
      /// </summary>
      public int AdditionalToHitBonus
      {
         get { return _attackModel.AdditionalToHitBonus; }
         set { _attackModel.AdditionalToHitBonus = value; }
      }

      /// <summary>
      /// Gets or sets additional damage bonus
      /// </summary>
      public int AdditionalDamageBonus
      {
         get { return _attackModel.AdditionalDamageBonus; }
         set { _attackModel.AdditionalDamageBonus = value; }
      }

      /// <summary>
      /// Gets or sets range
      /// </summary>
      public string Range
      {
         get { return _attackModel.Range; }
         set { _attackModel.Range = value; }
      }

      /// <summary>
      /// Gets range display
      /// </summary>
      public string RangeDisplay
      {
         get { return !String.IsNullOrWhiteSpace(_attackModel.Range) ? $"{_attackModel.Range} ft" : String.Empty; }
      }

      /// <summary>
      /// Gets or sets notes
      /// </summary>
      public string Notes
      {
         get { return _attackModel.Notes; }
         set { _attackModel.Notes = value; }
      }

      /// <summary>
      /// Gets or sets show to hit
      /// </summary>
      public bool ShowToHit
      {
         get { return _attackModel.ShowToHit; }
         set { _attackModel.ShowToHit = value; }
      }

      /// <summary>
      /// Gets or sets show damage
      /// </summary>
      public bool ShowDamage
      {
         get { return _attackModel.ShowDamage; }
         set { _attackModel.ShowDamage = value; }
      }


      /// <summary>
      /// Gets or sets to hit
      /// </summary>
      public string ToHit
      {
         get { return _attackModel.ToHit; }
         set { _attackModel.ToHit = value; }
      }

      /// <summary>
      /// Gets or sets damage
      /// </summary>
      public string Damage
      {
         get { return _attackModel.Damage; }
         set { _attackModel.Damage = value; }
      }

      #endregion

      #region Private Methods

      protected override void OnAccept()
      {
         if (String.IsNullOrWhiteSpace(_attackModel.Name))
         {
            _dialogService.ShowConfirmationDialog("Required Field", "Name is required.", "OK", null, null);
         }
         else
         {
            Accept();
         }
      }

      #endregion
   }
}
