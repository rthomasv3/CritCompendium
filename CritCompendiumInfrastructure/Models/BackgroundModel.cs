using System;
using System.Collections.Generic;
using System.Linq;
using CriticalCompendiumInfrastructure.Enums;

namespace CriticalCompendiumInfrastructure.Models
{
	public sealed class BackgroundModel
	{
		#region Fields

		private Guid _id;
		private string _name;
		private int _languagesIndex;
		private int _toolsIndex;
		private int _skillsIndex;
		private int _startingIndex;
		private List<Skill> _skills = new List<Skill>();
		private List<TraitModel> _traits = new List<TraitModel>();
		private string _xml;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a default  instance of <see cref="BackgroundModel"/>
		/// </summary>
		public BackgroundModel()
		{
			_id = Guid.NewGuid();
		}

		/// <summary>
		/// Creates a copy of <see cref="BackgroundModel"/>
		/// </summary>
		public BackgroundModel(BackgroundModel backgroundModel)
		{
			_id = backgroundModel.ID;
			_name = backgroundModel.Name;
			_skills = new List<Skill>(backgroundModel.Skills);

			_traits = new List<TraitModel>();
			foreach (TraitModel traitModel in backgroundModel.Traits)
			{
				_traits.Add(new TraitModel(traitModel));
			}

			_languagesIndex = _traits.FindIndex(x => x.TraitType == TraitType.Language);
			_toolsIndex = _traits.FindIndex(x => x.TraitType == TraitType.Tool_Proficiency);
			_skillsIndex = _traits.FindIndex(x => x.TraitType == TraitType.Skill_Proficiency);
			_startingIndex = _traits.FindIndex(x => x.TraitType == TraitType.Starting_Proficiency);

			_xml = backgroundModel.XML;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the id
		/// </summary>
		public Guid ID
		{
			get { return _id; }
			set { _id = value; }
		}

		/// <summary>
		/// Gets or sets the name
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// Gets or sets the skills
		/// </summary>
		public List<Skill> Skills
		{
			get { return _skills; }
			set { _skills = value; }
		}

		/// <summary>
		/// Gets or sets the traits
		/// </summary>
		public List<TraitModel> Traits
		{
			get { return _traits; }
			set { _traits = value; }
		}

		/// <summary>
		/// Gets or sets the index of the languages trait
		/// </summary>
		public int LanguagesTraitIndex
		{
			get { return _languagesIndex; }
			set { _languagesIndex = value; }
		}

		/// <summary>
		/// Gets or sets the index of the tools trait
		/// </summary>
		public int ToolsTraitIndex
		{
			get { return _toolsIndex; }
			set { _toolsIndex = value; }
		}

		/// <summary>
		/// Gets or sets the index of the skills trait
		/// </summary>
		public int SkillsTraitIndex
		{
			get { return _skillsIndex; }
			set { _skillsIndex = value; }
		}

		/// <summary>
		/// Gets or sets the index of the starting trait
		/// </summary>
		public int StartingTraitIndex
		{
			get { return _startingIndex; }
			set { _startingIndex = value; }
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
