using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Models
{
	public class MonsterModel
	{
		#region Fields

		private Guid _id;
		private string _name;
		private CreatureSize _size;
		private string _type;
		private string _alignment;
		private string _ac;
		private string _hp;
		private string _speed;
		private int _str;
		private int _dex;
		private int _con;
		private int _int;
		private int _wis;
		private int _cha;
		private string _description;
		private Dictionary<Ability, int> _saves = new Dictionary<Ability, int>();
		private Dictionary<Skill, int> _skills = new Dictionary<Skill, int>();
		private string _resistances;
		private string _vulnerabilities;
		private string _immunities;
		private string _conditionImmunities;
		private string _senses;
		private int _passivePerception;
		private List<string> _languages = new List<string>();
		private string _cr;
		private List<TraitModel> _traits = new List<TraitModel>();
		private List<MonsterActionModel> _actions = new List<MonsterActionModel>();
        private List<MonsterActionModel> _reactions = new List<MonsterActionModel>();
        private List<MonsterActionModel> _legendaryActions = new List<MonsterActionModel>();
		private List<string> _spells = new List<string>();
		private List<string> _spellSlots = new List<string>();
		private string _environment;
		private string _xml;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a default instance of <see cref="MonsterModel"/>
		/// </summary>
		public MonsterModel()
		{
			_id = Guid.NewGuid();
		}

		/// <summary>
		/// Creates a copy of <see cref="MonsterModel"/>
		/// </summary>
		public MonsterModel(MonsterModel monsterModel)
		{
			_id = monsterModel.ID;
			_name = monsterModel.Name;
			_size = monsterModel.Size;
			_type = monsterModel.Type;
			_alignment = monsterModel.Alignment;
			_ac = monsterModel.AC;
			_hp = monsterModel.HP;
			_speed = monsterModel.Speed;
			_str = monsterModel.Strength;
			_dex = monsterModel.Dexterity;
			_con = monsterModel.Constitution;
			_int = monsterModel.Intelligence;
			_wis = monsterModel.Wisdom;
			_cha = monsterModel.Charisma;
			_description = monsterModel.Description;
			_saves = new Dictionary<Ability, int>(monsterModel.Saves);
			_skills = new Dictionary<Skill, int>(monsterModel.Skills);
			_resistances = monsterModel.Resistances;
			_vulnerabilities = monsterModel.Vulnerabilities;
			_immunities = monsterModel.Immunities;
			_conditionImmunities = monsterModel.ConditionImmunities;
			_senses = monsterModel.Senses;
			_passivePerception = monsterModel.PassivePerception;
			_languages = monsterModel.Languages;
			_cr = monsterModel.CR;
			_spells = new List<string>(monsterModel.Spells);
			_spellSlots = new List<string>(monsterModel.SpellSlots);

			_traits = new List<TraitModel>();
			foreach (TraitModel traitModel in monsterModel.Traits)
			{
				_traits.Add(new TraitModel(traitModel));
			}

			_actions = new List<MonsterActionModel>();
			foreach (MonsterActionModel monsterActionModel in monsterModel.Actions)
			{
				_actions.Add(new MonsterActionModel(monsterActionModel));
			}

            _reactions = new List<MonsterActionModel>();
            foreach (MonsterActionModel monsterActionModel in monsterModel.Reactions)
            {
                _reactions.Add(new MonsterActionModel(monsterActionModel));
            }

			_legendaryActions = new List<MonsterActionModel>();
			foreach (MonsterActionModel monsterActionModel in monsterModel.LegendaryActions)
			{
				_legendaryActions.Add(new MonsterActionModel(monsterActionModel));
			}

			_environment = monsterModel.Environment;

			_xml = monsterModel.XML;
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
		/// Gets or sets size
		/// </summary>
		public CreatureSize Size
		{
			get { return _size; }
			set { _size = value; }
		}

		/// <summary>
		/// Gets or sets type
		/// </summary>
		public string Type
		{
			get { return _type; }
			set { _type = value; }
		}

		/// <summary>
		/// Gets or sets alignment
		/// </summary>
		public string Alignment
		{
			get { return _alignment; }
			set { _alignment = value; }
		}

		/// <summary>
		/// Gets or sets ac
		/// </summary>
		public string AC
		{
			get { return _ac; }
			set { _ac = value; }
		}

		/// <summary>
		/// Gets or sets hp
		/// </summary>
		public string HP
		{
			get { return _hp; }
			set { _hp = value; }
		}

		/// <summary>
		/// Gets or sets speed
		/// </summary>
		public string Speed
		{
			get { return _speed; }
			set { _speed = value; }
		}

		/// <summary>
		/// Gets or sets strength
		/// </summary>
		public int Strength
		{
			get { return _str; }
			set { _str = value; }
		}

		/// <summary>
		/// Gets or sets dexterity
		/// </summary>
		public int Dexterity
		{
			get { return _dex; }
			set { _dex = value; }
		}

		/// <summary>
		/// Gets or sets constitution
		/// </summary>
		public int Constitution
		{
			get { return _con; }
			set { _con = value; }
		}

		/// <summary>
		/// Gets or sets intelligence
		/// </summary>
		public int Intelligence
		{
			get { return _int; }
			set { _int = value; }
		}

		/// <summary>
		/// Gets or sets wisdom
		/// </summary>
		public int Wisdom
		{
			get { return _wis; }
			set { _wis = value; }
		}

		/// <summary>
		/// Gets or sets charisma
		/// </summary>
		public int Charisma
		{
			get { return _cha; }
			set { _cha = value; }
		}

		/// <summary>
		/// Gets or sets description
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		/// <summary>
		/// Gets or sets saves
		/// </summary>
		public Dictionary<Ability, int> Saves
		{
			get { return _saves; }
			set { _saves = value; }
		}

		/// <summary>
		/// Gets or sets skills
		/// </summary>
		public Dictionary<Skill, int> Skills
		{
			get { return _skills; }
			set { _skills = value; }
		}

		/// <summary>
		/// Gets or sets resistances
		/// </summary>
		public string Resistances
		{
			get { return _resistances; }
			set { _resistances = value; }
		}

		/// <summary>
		/// Gets or sets vulnerablities
		/// </summary>
		public string Vulnerabilities
		{
			get { return _vulnerabilities; }
			set { _vulnerabilities = value; }
		}

		/// <summary>
		/// Gets or sets immunities
		/// </summary>
		public string Immunities
		{
			get { return _immunities; }
			set { _immunities = value; }
		}

		/// <summary>
		/// Gets or sets cond immunities
		/// </summary>
		public string ConditionImmunities
		{
			get { return _conditionImmunities; }
			set { _conditionImmunities = value; }
		}

		/// <summary>
		/// Gets or sets senses
		/// </summary>
		public string Senses
		{
			get { return _senses; }
			set { _senses = value; }
		}

		/// <summary>
		/// Gets or sets passive perception
		/// </summary>
		public int PassivePerception
		{
			get { return _passivePerception; }
			set { _passivePerception = value; }
		}

		/// <summary>
		/// Gets or sets languages
		/// </summary>
		public List<string> Languages
		{
			get { return _languages; }
			set { _languages = value; }
		}

		/// <summary>
		/// Gets or sets cr
		/// </summary>
		public string CR
		{
			get { return _cr; }
			set { _cr = value; }
		}

		/// <summary>
		/// Gets or sets traits
		/// </summary>
		public List<TraitModel> Traits
		{
			get { return _traits; }
			set { _traits = value; }
		}

		/// <summary>
		/// Gets or sets actions
		/// </summary>
		public List<MonsterActionModel> Actions
		{
			get { return _actions; }
			set { _actions = value; }
		}

        /// <summary>
		/// Gets or sets reactions
		/// </summary>
		public List<MonsterActionModel> Reactions
        {
            get { return _reactions; }
            set { _reactions = value; }
        }

        /// <summary>
        /// Gets or sets legendary actions
        /// </summary>
        public List<MonsterActionModel> LegendaryActions
		{
			get { return _legendaryActions; }
			set { _legendaryActions = value; }
		}

		/// <summary>
		/// Gets or sets spells
		/// </summary>
		public List<string> Spells
		{
			get { return _spells; }
			set { _spells = value; }
		}

		/// <summary>
		/// Gets or sets spell slots
		/// </summary>
		public List<string> SpellSlots
		{
			get { return _spellSlots; }
			set { _spellSlots = value; }
		}

		/// <summary>
		/// Gets or sets environment
		/// </summary>
		public string Environment
		{
			get { return _environment; }
			set { _environment = value; }
		}

		/// <summary>
		/// Gets or sets xml
		/// </summary>
		public string XML
		{
			get { return _xml; }
			set { _xml = value; }
		}

		#endregion
	}
}
