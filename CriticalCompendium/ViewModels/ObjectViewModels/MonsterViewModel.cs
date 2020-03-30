using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CriticalCompendiumInfrastructure;
using CriticalCompendiumInfrastructure.Enums;
using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Services;
using CritCompendium.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
	public sealed class MonsterViewModel
	{
		#region Fields

		private readonly MonsterModel _monsterModel;
		private readonly string _size;
		private readonly string _alignment;
		private readonly string _ac;
		private readonly string _hp;
		private readonly string _type;
		private readonly string _cr;
		private readonly string _passivePerception;
		private readonly string _speed;
		private readonly string _strength;
		private readonly string _dexterity;
		private readonly string _constitution;
		private readonly string _intelligence;
		private readonly string _wisdom;
		private readonly string _charisma;
		private readonly string _strengthBonus;
		private readonly string _dexterityBonus;
		private readonly string _constitutionBonus;
		private readonly string _intelligenceBonus;
		private readonly string _wisdomBonus;
		private readonly string _charismaBonus;
		private readonly string _strengthSave;
		private readonly string _dexteritySave;
		private readonly string _constitutionSave;
		private readonly string _intelligenceSave;
		private readonly string _wisdomSave;
		private readonly string _charismaSave;
		private readonly Dictionary<string, string> _skills = new Dictionary<string, string>();
		private readonly string _vulnerabilities;
		private readonly string _resistances;
		private readonly string _immunities;
		private readonly string _conditionImmunities;
		private readonly string _senses;
		private readonly string _languages;
		private readonly string _environment;
		private readonly string _description;
		private readonly List<TraitViewModel> _traits = new List<TraitViewModel>();
		private readonly List<MonsterActionViewModel> _actions = new List<MonsterActionViewModel>();
		private readonly List<MonsterActionViewModel> _reactions = new List<MonsterActionViewModel>();
        private readonly List<MonsterActionViewModel> _legendaryActions = new List<MonsterActionViewModel>();
		private readonly List<string> _spellSlots = new List<string>();
		private readonly List<string> _spells = new List<string>();
		private readonly string _xml;

		private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
		private readonly StatService _statService = DependencyResolver.Resolve<StatService>();
		private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();
		private readonly Compendium _compendium = DependencyResolver.Resolve<Compendium>();
		private readonly RelayCommand _rollAbilityBonusCommand;
		private readonly RelayCommand _rollAbilitySaveCommand;
		private readonly RelayCommand _rollSkillCommand;
		private readonly RelayCommand _rollAttackToHitCommand;
		private readonly RelayCommand _rollAttackDamageCommand;
		private readonly RelayCommand _showSpellDialogCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="MonsterViewModel"/>
        /// </summary>
        public MonsterViewModel(MonsterModel monsterModel)
		{
			_monsterModel = monsterModel;

			_size = _monsterModel.Size != CreatureSize.None ? _stringService.GetString(_monsterModel.Size) : "Unknown";
			_alignment = !String.IsNullOrWhiteSpace(_monsterModel.Alignment) ? _stringService.CapitalizeWords(_monsterModel.Alignment) : "Unknown";
			_ac = !String.IsNullOrWhiteSpace(_monsterModel.AC) ? _stringService.CapitalizeWords(_monsterModel.AC) : "Unknown";
			_hp = !String.IsNullOrWhiteSpace(_monsterModel.HP) ? _monsterModel.HP : "Unknown";
			_type = !String.IsNullOrWhiteSpace(_monsterModel.Type) ? _stringService.CapitalizeWords(_monsterModel.Type) : "Unknown";

			if (!String.IsNullOrWhiteSpace(_monsterModel.CR))
			{
				string xp = _stringService.CRXPString(_monsterModel.CR);
				if (!String.IsNullOrWhiteSpace(xp))
				{
					_cr = String.Format("{0} ({1} XP)", _monsterModel.CR, xp);
				}
				else
				{
					_cr = _monsterModel.CR;
				}
			}
			else
			{
				_cr = "Unknown";
			}

			_passivePerception = _monsterModel.PassivePerception.ToString();
			_speed = !String.IsNullOrWhiteSpace(_monsterModel.Speed) ? _stringService.CapitalizeWords(_monsterModel.Speed) : "Unknown";

			_strength = _monsterModel.Strength.ToString();
			_dexterity = _monsterModel.Dexterity.ToString();
			_constitution = _monsterModel.Constitution.ToString();
			_intelligence = _monsterModel.Intelligence.ToString();
			_wisdom = _monsterModel.Wisdom.ToString();
			_charisma = _monsterModel.Charisma.ToString();

			_strengthBonus = _statService.GetStatBonusString(_monsterModel.Strength);
			_dexterityBonus = _statService.GetStatBonusString(_monsterModel.Dexterity);
			_constitutionBonus = _statService.GetStatBonusString(_monsterModel.Constitution);
			_intelligenceBonus = _statService.GetStatBonusString(_monsterModel.Intelligence);
			_wisdomBonus = _statService.GetStatBonusString(_monsterModel.Wisdom);
			_charismaBonus = _statService.GetStatBonusString(_monsterModel.Charisma);

			_strengthSave = _monsterModel.Saves.ContainsKey(Ability.Strength) ? _statService.AddPlusOrMinus(_monsterModel.Saves[Ability.Strength]): _strengthBonus;
			_dexteritySave = _monsterModel.Saves.ContainsKey(Ability.Dexterity) ? _statService.AddPlusOrMinus(_monsterModel.Saves[Ability.Dexterity]) : _dexterityBonus;
			_constitutionSave = _monsterModel.Saves.ContainsKey(Ability.Constitution) ? _statService.AddPlusOrMinus(_monsterModel.Saves[Ability.Constitution]) : _constitutionBonus;
			_intelligenceSave = _monsterModel.Saves.ContainsKey(Ability.Intelligence) ? _statService.AddPlusOrMinus(_monsterModel.Saves[Ability.Intelligence]) : _intelligenceBonus;
			_wisdomSave = _monsterModel.Saves.ContainsKey(Ability.Wisdom) ? _statService.AddPlusOrMinus(_monsterModel.Saves[Ability.Wisdom]) : _wisdomBonus;
			_charismaSave = _monsterModel.Saves.ContainsKey(Ability.Charisma) ? _statService.AddPlusOrMinus(_monsterModel.Saves[Ability.Charisma]) : _charismaBonus;

			foreach (KeyValuePair<Skill, int> skillPair in _monsterModel.Skills)
			{
				string skillString = _stringService.GetString(skillPair.Key);
				string skillRoll = _statService.AddPlusOrMinus(skillPair.Value);
				_skills[skillString] = skillRoll;
			}
			
			_vulnerabilities = !String.IsNullOrWhiteSpace(_monsterModel.Vulnerabilities) ? _stringService.CapitalizeWords(_monsterModel.Vulnerabilities) : "None";
			_resistances = !String.IsNullOrWhiteSpace(_monsterModel.Resistances) ? _stringService.CapitalizeWords(_monsterModel.Resistances) : "None";
			_immunities = !String.IsNullOrWhiteSpace(_monsterModel.Immunities) ? _stringService.CapitalizeWords(_monsterModel.Immunities) : "None";
			_conditionImmunities = !String.IsNullOrWhiteSpace(_monsterModel.ConditionImmunities) ? _stringService.CapitalizeWords(_monsterModel.ConditionImmunities) : "None";
			_senses = !String.IsNullOrWhiteSpace(_monsterModel.Senses) ? _stringService.CapitalizeWords(_monsterModel.Senses) : "None";
			_languages = _monsterModel.Languages.Count > 0 ? _stringService.CapitalizeWords(String.Join(", ", _monsterModel.Languages)) : "None";
			_environment = !String.IsNullOrWhiteSpace(_monsterModel.Environment) ? _stringService.CapitalizeWords(_monsterModel.Environment) : "Unknown";

			_description = _monsterModel.Description;

			foreach (TraitModel traitModel in _monsterModel.Traits)
			{
				_traits.Add(new TraitViewModel(traitModel));
			}

			foreach (MonsterActionModel monsterAction in _monsterModel.Actions)
			{
				_actions.Add(new MonsterActionViewModel(monsterAction));
			}

            foreach (MonsterActionModel monsterAction in _monsterModel.Reactions)
            {
                _reactions.Add(new MonsterActionViewModel(monsterAction));
            }

            foreach (MonsterActionModel monsterAction in _monsterModel.LegendaryActions)
			{
				_legendaryActions.Add(new MonsterActionViewModel(monsterAction));
			}

			_spellSlots = new List<string>(_monsterModel.SpellSlots);
			while (_spellSlots.Count < 9)
			{
				_spellSlots.Add("0");
			}

			_spells = new List<string>(_monsterModel.Spells);

			_rollAbilityBonusCommand = new RelayCommand(obj => true, obj => RollAbilityBonus((Ability)obj));
			_rollAbilitySaveCommand = new RelayCommand(obj => true, obj => RollAbilitySave((Ability)obj));
			_rollSkillCommand = new RelayCommand(obj => true, obj => RollSkill((string)obj));
			_rollAttackToHitCommand = new RelayCommand(obj => true, obj => RollAttackToHit((MonsterAttackViewModel)obj));
			_rollAttackDamageCommand = new RelayCommand(obj => true, obj => RollAttackDamage((MonsterAttackViewModel)obj));
			_showSpellDialogCommand = new RelayCommand(obj => true, obj => ShowSpellDialog((string)obj));
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets monster model
		/// </summary>
		public MonsterModel MonsterModel
		{
			get { return _monsterModel; }
		}

		/// <summary>
		/// Gets name
		/// </summary>
		public string Name
		{
			get { return _monsterModel.Name; }
		}

		/// <summary>
		/// Gets size
		/// </summary>
		public string Size
		{
			get { return _size; }
		}

		/// <summary>
		/// Gets alignment
		/// </summary>
		public string Alignment
		{
			get { return _alignment; }
		}

		/// <summary>
		/// Gets ac
		/// </summary>
		public string AC
		{
			get { return _ac; }
		}

		/// <summary>
		/// Gets hp
		/// </summary>
		public string HP
		{
			get { return _hp; }
		}

		/// <summary>
		/// Gets type
		/// </summary>
		public string Type
		{
			get { return _type; }
		}

		/// <summary>
		/// Gets cr
		/// </summary>
		public string CR
		{
			get { return _cr; }
		}

		/// <summary>
		/// Gets passive perception
		/// </summary>
		public string PassivePerception
		{
			get { return _passivePerception; }
		}

		/// <summary>
		/// Gets speed
		/// </summary>
		public string Speed
		{
			get { return _speed; }
		}

		/// <summary>
		/// Gets strength
		/// </summary>
		public string Strength
		{
			get { return _strength; }
		}

		/// <summary>
		/// Gets dexterity
		/// </summary>
		public string Dexterity
		{
			get { return _dexterity; }
		}

		/// <summary>
		/// Gets constitution
		/// </summary>
		public string Constitution
		{
			get { return _constitution; }
		}

		/// <summary>
		/// Gets intelligence
		/// </summary>
		public string Intelligence
		{
			get { return _intelligence; }
		}

		/// <summary>
		/// Gets wisdom
		/// </summary>
		public string Wisdom
		{
			get { return _wisdom; }
		}

		/// <summary>
		/// Gets charisma
		/// </summary>
		public string Charisma
		{
			get { return _charisma; }
		}

		/// <summary>
		/// Gets strength bonus
		/// </summary>
		public string StrengthBonus
		{
			get { return _strengthBonus; }
		}

		/// <summary>
		/// Gets dexterity bonus
		/// </summary>
		public string DexterityBonus
		{
			get { return _dexterityBonus; }
		}

		/// <summary>
		/// Gets constitution bonus
		/// </summary>
		public string ConstitutionBonus
		{
			get { return _constitutionBonus; }
		}

		/// <summary>
		/// Gets intelligence bonus
		/// </summary>
		public string IntelligenceBonus
		{
			get { return _intelligenceBonus; }
		}

		/// <summary>
		/// Gets wisdom bonus
		/// </summary>
		public string WisdomBonus
		{
			get { return _wisdomBonus; }
		}

		/// <summary>
		/// Gets charisma bonus
		/// </summary>
		public string CharismaBonus
		{
			get { return _charismaBonus; }
		}

		/// <summary>
		/// Gets strength save
		/// </summary>
		public string StrengthSave
		{
			get { return _strengthSave; }
		}

		/// <summary>
		/// Gets dexterity save
		/// </summary>
		public string DexteritySave
		{
			get { return _dexteritySave; }
		}

		/// <summary>
		/// Gets constitution save
		/// </summary>
		public string ConstitutionSave
		{
			get { return _constitutionSave; }
		}

		/// <summary>
		/// Gets intelligence save
		/// </summary>
		public string IntelligenceSave
		{
			get { return _intelligenceSave; }
		}

		/// <summary>
		/// Gets wisdom save
		/// </summary>
		public string WisdomSave
		{
			get { return _wisdomSave; }
		}

		/// <summary>
		/// Gets charisma save
		/// </summary>
		public string CharismaSave
		{
			get { return _charismaSave; }
		}

		/// <summary>
		/// Gets skills
		/// </summary>
		public Dictionary<string, string> Skills
		{
			get { return _skills; }
		}

		/// <summary>
		/// Gets vulnerabilities
		/// </summary>
		public string Vulnerabilities
		{
			get { return _vulnerabilities; }
		}

		/// <summary>
		/// Gets resistances
		/// </summary>
		public string Resistances
		{
			get { return _resistances; }
		}

		/// <summary>
		/// Gets immunities
		/// </summary>
		public string Immunities
		{
			get { return _immunities; }
		}

		/// <summary>
		/// Gets condition immunities
		/// </summary>
		public string ConditionImmunities
		{
			get { return _conditionImmunities; }
		}

		/// <summary>
		/// Gets senses
		/// </summary>
		public string Senses
		{
			get { return _senses; }
		}

		/// <summary>
		/// Gets languages
		/// </summary>
		public string Languages
		{
			get { return _languages; }
		}

		/// <summary>
		/// Gets environment
		/// </summary>
		public string Environment
		{
			get { return _environment; }
		}

		/// <summary>
		/// Gets description
		/// </summary>
		public string Description
		{
			get { return _description; }
		}

		/// <summary>
		/// Gets traits
		/// </summary>
		public List<TraitViewModel> Traits
		{
			get { return _traits; }
		}

		/// <summary>
		/// Gets actions
		/// </summary>
		public List<MonsterActionViewModel> Actions
		{
			get { return _actions; }
		}

        /// <summary>
		/// Gets reactions
		/// </summary>
		public List<MonsterActionViewModel> Reactions
        {
            get { return _reactions; }
        }

        /// <summary>
        /// Gets legendary actions
        /// </summary>
        public List<MonsterActionViewModel> LegendaryActions
		{
			get { return _legendaryActions; }
		}

		/// <summary>
		/// Gets spell slots
		/// </summary>
		public List<string> SpellSlots
		{
			get { return _spellSlots; }
		}

		/// <summary>
		/// Gets spells
		/// </summary>
		public List<string> Spells
		{
			get { return _spells; }
		}

		/// <summary>
		/// Gets xml
		/// </summary>
		public string XML
		{
			get { return _monsterModel.XML; }
		}

		/// <summary>
		/// Gets roll ability bonus command
		/// </summary>
		public ICommand RollAbilityBonusCommand
		{
			get { return _rollAbilityBonusCommand; }
		}

		/// <summary>
		/// Gets roll ability save command
		/// </summary>
		public ICommand RollAbilitySaveCommand
		{
			get { return _rollAbilitySaveCommand; }
		}

		/// <summary>
		/// Gets roll skill command
		/// </summary>
		public ICommand RollSkillCommand
		{
			get { return _rollSkillCommand; }
		}

		/// <summary>
		/// Gets roll attack to hit command
		/// </summary>
		public ICommand RollAttackToHitCommand
		{
			get { return _rollAttackToHitCommand; }
		}

		/// <summary>
		/// Gets roll attack damage command
		/// </summary>
		public ICommand RollAttackDamageCommand
		{
			get { return _rollAttackDamageCommand; }
		}

		/// <summary>
		/// Gets show spell dialog command
		/// </summary>
		public ICommand ShowSpellDialogCommand
		{
			get { return _showSpellDialogCommand; }
		}

		#endregion

		#region Private Methods

		private void RollAbilityBonus(Ability ability)
		{
			string bonus = "+0";

			switch (ability)
			{
				case Ability.Strength:
					bonus = _strengthBonus;
					break;
				case Ability.Dexterity:
					bonus = _dexterityBonus;
					break;
				case Ability.Constitution:
					bonus = _constitutionBonus;
					break;
				case Ability.Intelligence:
					bonus = _intelligenceBonus;
					break;
				case Ability.Wisdom:
					bonus = _wisdomBonus;
					break;
				case Ability.Charisma:
					bonus = _charismaBonus;
					break;
			}

			_dialogService.ShowDiceRollDialog(_stringService.GetString(ability) + " Bonus", "1d20" + bonus);
		}

		private void RollAbilitySave(Ability ability)
		{
			string save = "+0";

			switch (ability)
			{
				case Ability.Strength:
					save = _strengthSave;
					break;
				case Ability.Dexterity:
					save = _dexteritySave;
					break;
				case Ability.Constitution:
					save = _constitutionSave;
					break;
				case Ability.Intelligence:
					save = _intelligenceSave;
					break;
				case Ability.Wisdom:
					save = _wisdomSave;
					break;
				case Ability.Charisma:
					save = _charismaSave;
					break;
			}

			_dialogService.ShowDiceRollDialog(_stringService.GetString(ability) + " Save", "1d20" + save);
		}

		private void RollSkill(string skillString)
		{
			string bonus = "+0";

			Skill skill = _stringService.GetEnum<Skill>(skillString);

			if (_monsterModel.Skills.ContainsKey(skill))
			{
				bonus = _statService.AddPlusOrMinus(_monsterModel.Skills[skill]);
			}

			_dialogService.ShowDiceRollDialog(skillString + " Check", "1d20" + bonus);
		}

		private void RollAttackToHit(MonsterAttackViewModel attackViewModel)
		{
			_dialogService.ShowDiceRollDialog(attackViewModel.Name + " (To Hit)", "1d20" + attackViewModel.ToHit);
		}

		private void RollAttackDamage(MonsterAttackViewModel attackViewModel)
		{
			_dialogService.ShowDiceRollDialog(attackViewModel.Name + " (Damage)", attackViewModel.Roll);
		}

		private void ShowSpellDialog(string spellName)
		{
			SpellViewModel spellViewModel = null;

			foreach (SpellModel spellModel in _compendium.Spells)
			{
				if (String.Equals(spellModel.Name, spellName, StringComparison.CurrentCultureIgnoreCase))
				{
					spellViewModel = new SpellViewModel(spellModel);
					break;
				}
			}

			if (spellViewModel != null)
			{
				_dialogService.ShowDetailsDialog(spellViewModel);
			}
			else
			{
				_dialogService.ShowConfirmationDialog("Compendium", "Unable to find the spell '" + spellName + "' in the compendium.", "OK", null, null);
			}
		}

		#endregion
	}
}
