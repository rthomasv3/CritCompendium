using System;
using CriticalCompendiumInfrastructure.Enums;

namespace CriticalCompendiumInfrastructure.Models
{
   public sealed class AttackModel
   {
      #region Fields

      private Guid _id;
      private string _name;
      private int _numberOfDamageDice;
      private string _damageDie;
      private Ability _ability = Ability.None;
      private bool _proficient;
      private DamageType _damageType = DamageType.None;
      private int _additionalToHitBonus;
      private int _additionalDamageBonus;
      private string _range = "5";
      private string _notes;
      private bool _showToHit = true;
      private bool _showDamage = true;

      private string _toHit;
      private string _damage;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates an instance of <see cref="AttackModel"/>
      /// </summary>
      public AttackModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates an instance of <see cref="AttackModel"/>
      /// </summary>
      public AttackModel(AttackModel attackModel)
      {
         _id = attackModel.ID;
         _name = attackModel.Name;
         _numberOfDamageDice = attackModel.NumberOfDamageDice;
         _damageDie = attackModel.DamageDie;
         _ability = attackModel.Ability;
         _proficient = attackModel.Proficient;
         _damageType = attackModel.DamageType;
         _additionalToHitBonus = attackModel.AdditionalToHitBonus;
         _additionalDamageBonus = attackModel.AdditionalDamageBonus;
         _range = attackModel.Range;
         _notes = attackModel.Notes;
         _showToHit = attackModel.ShowToHit;
         _showDamage = attackModel.ShowDamage;
         _toHit = attackModel.ToHit;
         _damage = attackModel.Damage;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets id
      /// </summary>
      public Guid ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Gets or sets name
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Gets or sets number of damage dice.
      /// </summary>
      public int NumberOfDamageDice
      {
         get { return _numberOfDamageDice; }
         set { _numberOfDamageDice = value; }
      }

      /// <summary>
      /// Gets or sets damage die.
      /// </summary>
      public string DamageDie
      {
         get { return _damageDie; }
         set { _damageDie = value; }
      }

      /// <summary>
      /// Gets or sets ability
      /// </summary>
      public Ability Ability
      {
         get { return _ability; }
         set { _ability = value; }
      }

      /// <summary>
      /// Gets or sets proficient
      /// </summary>
      public bool Proficient
      {
         get { return _proficient; }
         set { _proficient = value; }
      }

      /// <summary>
      /// Gets or sets damage type
      /// </summary>
      public DamageType DamageType
      {
         get { return _damageType; }
         set { _damageType = value; }
      }

      /// <summary>
      /// Gets or sets additional to hit bonus
      /// </summary>
      public int AdditionalToHitBonus
      {
         get { return _additionalToHitBonus; }
         set { _additionalToHitBonus = value; }
      }

      /// <summary>
      /// Gets or sets additional damage bonus
      /// </summary>
      public int AdditionalDamageBonus
      {
         get { return _additionalDamageBonus; }
         set { _additionalDamageBonus = value; }
      }

      /// <summary>
      /// Gets or sets range
      /// </summary>
      public string Range
      {
         get { return _range; }
         set { _range = value; }
      }

      /// <summary>
      /// Gets or sets notes
      /// </summary>
      public string Notes
      {
         get { return _notes; }
         set { _notes = value; }
      }

      /// <summary>
      /// Gets or sets show to hit
      /// </summary>
      public bool ShowToHit
      {
         get { return _showToHit; }
         set { _showToHit = value; }
      }

      /// <summary>
      /// Gets or sets show damage
      /// </summary>
      public bool ShowDamage
      {
         get { return _showDamage; }
         set { _showDamage = value; }
      }

      /// <summary>
      /// Gets or sets to hit
      /// </summary>
      public string ToHit
      {
         get { return _toHit; }
         set { _toHit = value; }
      }

      /// <summary>
      /// Gets or sets damage
      /// </summary>
      public string Damage
      {
         get { return _damage; }
         set { _damage = value; }
      }

      #endregion
   }
}
