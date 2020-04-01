using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Models
{
	public sealed class ClassModel
	{
		#region Fields

		private Guid _id;
		private string _name;
		private int _hitDie;
		private List<Ability> _abilityProficiencies = new List<Ability>();
		private Ability _spellAbility;
		private List<string> _spellSlots = new List<string>();
		private int _spellStartLevel;
		private string _armorProficiencies;
		private string _weaponProficiencies;
		private string _toolProficiencies;
		private string _skillProficiencies;
		private List<AutoLevelModel> _autoLevels = new List<AutoLevelModel>();
		private List<FeatureModel> _tableFeatures = new List<FeatureModel>();
		private string _xml;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a default instance of <see cref="ClassModel"/>
		/// </summary>
		public ClassModel()
		{
			_id = Guid.NewGuid();
		}

		/// <summary>
		/// Creates a copy of <see cref="ClassModel"/>
		/// </summary>
		public ClassModel(ClassModel classModel)
		{
            _id = classModel.ID;
			_name = classModel.Name;
			_hitDie = classModel.HitDie;
			_abilityProficiencies = new List<Ability>(classModel.AbilityProficiencies);
			_spellAbility = classModel.SpellAbility;
			_spellSlots = new List<string>(classModel.SpellSlots);
			_spellStartLevel = classModel.SpellStartLevel;
			_armorProficiencies = classModel.ArmorProficiencies;
			_weaponProficiencies = classModel.WeaponProficiencies;
			_toolProficiencies = classModel.ToolProficiencies;
			_skillProficiencies = classModel.SkillProficiencies;

			_autoLevels = new List<AutoLevelModel>();
			foreach (AutoLevelModel autoLevelModel in classModel.AutoLevels)
			{
				_autoLevels.Add(new AutoLevelModel(autoLevelModel));
			}

			_tableFeatures = new List<FeatureModel>();
			foreach (FeatureModel featureModel in classModel.TableFeatures)
			{
				_tableFeatures.Add(new FeatureModel(featureModel));
			}

			_xml = classModel.XML;
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
		/// Gets or sets hit die
		/// </summary>
		public int HitDie
		{
			get { return _hitDie; }
			set { _hitDie = value; }
		}

		/// <summary>
		/// Gets or sets ability proficiencies
		/// </summary>
		public List<Ability> AbilityProficiencies
		{
			get { return _abilityProficiencies; }
			set { _abilityProficiencies = value; }
		}

		/// <summary>
		/// Gets or sets spell ability
		/// </summary>
		public Ability SpellAbility
		{
			get { return _spellAbility; }
			set { _spellAbility = value; }
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
		/// Gets or sets spell start level
		/// </summary>
		public int SpellStartLevel
		{
			get { return _spellStartLevel; }
			set { _spellStartLevel = value; }
		}

		/// <summary>
		/// Gets or sets armor proficiencies
		/// </summary>
		public string ArmorProficiencies
		{
			get { return _armorProficiencies; }
			set { _armorProficiencies = value; }
		}

		/// <summary>
		/// Gets or sets weapon proficiencies
		/// </summary>
		public string WeaponProficiencies
		{
			get { return _weaponProficiencies; }
			set { _weaponProficiencies = value; }
		}

		/// <summary>
		/// Gets or sets tool proficiencies
		/// </summary>
		public string ToolProficiencies
		{
			get { return _toolProficiencies; }
			set { _toolProficiencies = value; }
		}

		/// <summary>
		/// Gets or sets skill proficiencies
		/// </summary>
		public string SkillProficiencies
		{
			get { return _skillProficiencies; }
			set { _skillProficiencies = value; }
		}

		/// <summary>
		/// Gets or sets auto levels
		/// </summary>
		public List<AutoLevelModel> AutoLevels
		{
			get { return _autoLevels; }
			set { _autoLevels = value; }
		}

		/// <summary>
		/// Gets or sets table features
		/// </summary>
		public List<FeatureModel> TableFeatures
		{
			get { return _tableFeatures; }
			set { _tableFeatures = value; }
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
