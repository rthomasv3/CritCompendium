using System;
using System.Collections.Generic;
using System.Linq;
using CriticalCompendiumInfrastructure;
using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
	public sealed class BackgroundViewModel : NotifyPropertyChanged
	{
		#region Fields

		private readonly BackgroundModel _backgroundModel;
		private string _skills;
		private readonly List<TraitViewModel> _traits = new List<TraitViewModel>();
		private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of <see cref="BackgroundViewModel"/>
		/// </summary>
		public BackgroundViewModel(BackgroundModel backgroundModel)
		{
			_backgroundModel = backgroundModel;

			_skills = "None";

			if (_backgroundModel.Skills.Count > 0)
			{
				_skills = String.Join(", ", _backgroundModel.Skills.Select(x => _stringService.GetString(x)));
			}
			else if (_backgroundModel.SkillsTraitIndex > -1 &&
						_backgroundModel.SkillsTraitIndex < _backgroundModel.Traits.Count)
			{
				TraitModel trait = _backgroundModel.Traits[_backgroundModel.SkillsTraitIndex];
				_skills = trait.TextCollection[0].Trim();
			}
			else if (_backgroundModel.StartingTraitIndex > -1 &&
						_backgroundModel.StartingTraitIndex < _backgroundModel.Traits.Count)
			{
				foreach (string text in _backgroundModel.Traits[_backgroundModel.StartingTraitIndex].TextCollection)
				{
					if (text.Contains("Skills: "))
					{
						_skills = text.Replace("Skills: ", "").Trim();
						break;
					}
				}
			}

			foreach (TraitModel trait in _backgroundModel.Traits)
			{
				_traits.Add(new TraitViewModel(trait));
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the background model
		/// </summary>
		public BackgroundModel BackgroundModel
		{
			get { return _backgroundModel; }
		}

		/// <summary>
		/// Background name
		/// </summary>
		public string Name
		{
			get { return _backgroundModel.Name; }
		}

		/// <summary>
		/// Skills
		/// </summary>
		public string Skills
		{
			get { return _skills; }
		}

		/// <summary>
		/// Traits
		/// </summary>
		public List<TraitViewModel> Traits
		{
			get { return _traits; }
		}

		/// <summary>
		/// Gets XML
		/// </summary>
		public string XML
		{
			get { return _backgroundModel.XML; }
		}

		#endregion
	}
}
