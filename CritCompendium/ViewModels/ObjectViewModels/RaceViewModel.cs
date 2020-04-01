using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
	public sealed class RaceViewModel
	{
		#region Fields

		private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
		private readonly StatService _statService = DependencyResolver.Resolve<StatService>();
		private readonly RaceModel _raceModel;
		private readonly string _size;
		private readonly string _speed;
		private readonly string _abilities;
		private readonly List<TraitViewModel> _traits = new List<TraitViewModel>();

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance of <see cref="RaceViewModel"/>
		/// </summary>
		public RaceViewModel(RaceModel raceModel)
		{
			_raceModel = raceModel;

			_size = _raceModel.Size != CreatureSize.None ? _stringService.GetString(_raceModel.Size) : "Unknown";
			_speed = raceModel.WalkSpeed.ToString() + " ft.";
			if (_raceModel.FlySpeed > 0)
			{
				_speed += ", Fly" + _raceModel.FlySpeed.ToString() + " ft.";
			}

			if (_raceModel.Abilities.Count > 0)
			{
				List<string> abilities = new List<string>();
				foreach (KeyValuePair<Ability, int> pair in _raceModel.Abilities)
				{
					abilities.Add(_stringService.GetString(pair.Key) + " " + _statService.AddPlusOrMinus(pair.Value));
				}
				_abilities = String.Join(", ", abilities);
			}
			else
			{
				_abilities = "None";
			}

			foreach (TraitModel trait in _raceModel.Traits)
			{
				_traits.Add(new TraitViewModel(trait));
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets race model
		/// </summary>
		public RaceModel RaceModel
		{
			get { return _raceModel; }
		}

		/// <summary>
		/// Gets name
		/// </summary>
		public string Name
		{
			get { return _raceModel.Name; }
		}

		/// <summary>
		/// Gets size
		/// </summary>
		public string Size
		{
			get { return _size; }
		}

		/// <summary>
		/// Gets speed
		/// </summary>
		public string Speed
		{
			get { return _speed; }
		}

		/// <summary>
		/// Gets abilities
		/// </summary>
		public string Abilities
		{
			get { return _abilities; }
		}

		/// <summary>
		/// Gets traits
		/// </summary>
		public List<TraitViewModel> Traits
		{
			get { return _traits; }
		}

		/// <summary>
		/// Gets xml
		/// </summary>
		public string XML
		{
			get { return _raceModel.XML; }
		}

		#endregion
	}
}
