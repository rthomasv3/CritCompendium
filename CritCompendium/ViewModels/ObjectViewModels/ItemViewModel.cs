using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;
using CritCompendium.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
	public sealed class ItemViewModel
	{
		#region Fields

		private readonly ItemModel _itemModel;
		private readonly string _type;
		private readonly string _magic;
		private readonly string _requiresAttunement;
		private readonly string _value;
		private readonly string _weight;
		private readonly string _damage;
		private readonly string _damageType;
		private readonly string _properties;
		private readonly string _rarity;
		private readonly string _ac;
		private readonly string _strengthRequirement;
		private readonly string _stealthDisadvantage;
		private readonly string _range;
		private readonly string _text;
		private readonly List<ModifierModel> _modifiers = new List<ModifierModel>();
		private readonly List<string> _rolls = new List<string>();
		private readonly RelayCommand _rollCommand;

		private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
		private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();

		#endregion

		#region Constructor

        /// <summary>
        /// Creates an instance of <see cref="ItemViewModel"/>
        /// </summary>
		public ItemViewModel(ItemModel itemModel)
		{
			_itemModel = itemModel;

			_type = _stringService.GetString(_itemModel.Type);
			_magic = _itemModel.Magic ? "Yes" : "No";
			_requiresAttunement = _itemModel.RequiresAttunement ? "Yes" : "No";

			if (String.IsNullOrWhiteSpace(_itemModel.Value))
			{
				_value = "Unknown";
			}
			else if (itemModel.Value.Any(x => Char.IsLetter(x)))
			{
				_value = _itemModel.Value;
			}
			else
			{
				_value = _itemModel.Value + " g";
			}

			_weight = !String.IsNullOrWhiteSpace(_itemModel.Weight) ? _itemModel.Weight + " lbs" : "Unknown";

			if (!String.IsNullOrWhiteSpace(_itemModel.Dmg1) &&
				!String.IsNullOrWhiteSpace(_itemModel.Dmg2))
			{
				_damage = _itemModel.Dmg1 + " / " + _itemModel.Dmg2;
			}
			else if (!String.IsNullOrWhiteSpace(_itemModel.Dmg1))
			{
				_damage = _itemModel.Dmg1;
			}
			else if (!String.IsNullOrWhiteSpace(_itemModel.Dmg2))
			{
				_damage = _itemModel.Dmg2;
			}
			else
			{
				_damage = "None";
			}

			if (!String.IsNullOrWhiteSpace(_itemModel.DmgType))
			{
				_damageType = _itemModel.DmgType;
			}
			else
			{
				_damageType = "None";
			}

			if (!String.IsNullOrWhiteSpace(_itemModel.Properties))
			{
				_properties = _itemModel.Properties;
			}
			else
			{
				_properties = "None";
			}

			if (_itemModel.Rarity != CritCompendiumInfrastructure.Enums.Rarity.None)
			{
				_rarity = _stringService.GetString(_itemModel.Rarity);
			}
			else
			{
				_rarity = "Unknown";
			}

			if (!String.IsNullOrWhiteSpace(_itemModel.AC))
			{
				_ac = _itemModel.AC;
			}
			else
			{
				_ac = "None";
			}
			
			if (!String.IsNullOrWhiteSpace(_itemModel.StrengthRequirement))
			{
				_strengthRequirement = _itemModel.StrengthRequirement;
			}
			else
			{
				_strengthRequirement = "None";
			}

			_stealthDisadvantage = _itemModel.StealthDisadvantage ? "Yes" : "No";

			if (!String.IsNullOrWhiteSpace(_itemModel.Range))
			{
				_range = _itemModel.Range;
			}
			else
			{
				_range = "None";
			}

			_text = String.Join(Environment.NewLine + Environment.NewLine, _itemModel.TextCollection.Where(x => !String.IsNullOrWhiteSpace(x)));

            if (String.IsNullOrWhiteSpace(_text))
            {
                _text = "Unknown";
            }
            else
            {
                _text = _text.Replace("\t", "").Trim();
            }

			_rolls = new List<string>(itemModel.Rolls);

			_rollCommand = new RelayCommand(obj => true, obj => Roll((string)obj));
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets item model
		/// </summary>
		public ItemModel ItemModel
		{
			get { return _itemModel; }
		}

		/// <summary>
		/// Gets name
		/// </summary>
		public string Name
		{
			get { return _itemModel.Name; }
		}

		/// <summary>
		/// Gets type
		/// </summary>
		public string Type
		{
			get { return _type; }
		}

		/// <summary>
		/// Gets magic
		/// </summary>
		public string Magic
		{
			get { return _magic; }
		}

		/// <summary>
		/// Gets requres attunement
		/// </summary>
		public string RequiresAttunement
		{
			get { return _requiresAttunement; }
		}

		/// <summary>
		/// Gets value
		/// </summary>
		public string Value
		{
			get { return _value; }
		}

		/// <summary>
		/// Gets weight
		/// </summary>
		public string Weight
		{
			get { return _weight; }
		}

		/// <summary>
		/// Gets damage
		/// </summary>
		public string Damage
		{
			get { return _damage; }
		}

		/// <summary>
		/// Gets dmgType
		/// </summary>
		public string DamageType
		{
			get { return _damageType; }
		}

		/// <summary>
		/// Gets properties
		/// </summary>
		public string Properties
		{
			get { return _properties; }
		}

		/// <summary>
		/// Gets rarity
		/// </summary>
		public string Rarity
		{
			get { return _rarity; }
		}

		/// <summary>
		/// Gets ac
		/// </summary>
		public string AC
		{
			get { return _ac; }
		}

		/// <summary>
		/// Gets strength requirement
		/// </summary>
		public string StrengthRequirement
		{
			get { return _strengthRequirement; }
		}

		/// <summary>
		/// Gets stealth disadvantage
		/// </summary>
		public string StealthDisadvantage
		{
			get { return _stealthDisadvantage; }
		}

		/// <summary>
		/// Gets range
		/// </summary>
		public string Range
		{
			get { return _range; }
		}

		/// <summary>
		/// Gets text
		/// </summary>
		public string Text
		{
			get { return _text; }
		}

		/// <summary>
		/// Gets rolls
		/// </summary>
		public List<string> Rolls
		{
			get { return _rolls; }
		}

		/// <summary>
		/// Gets xml
		/// </summary>
		public string XML
		{
			get { return _itemModel.XML; }
		}
		
		/// <summary>
		/// Gets roll command
		/// </summary>
		public ICommand RollCommand
		{
			get { return _rollCommand; }
		}

		#endregion

		#region Private Methods

		private void Roll(string roll)
		{
			_dialogService.ShowDiceRollDialog("Roll " + roll, roll);
		}

		#endregion
	}
}
