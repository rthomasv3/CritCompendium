using System;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
	public sealed class AbilityScoreEditViewModel
	{
		#region Events

		public event EventHandler SelectionChanged;

		#endregion

		#region Fields

		private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
		private Tuple<Ability, string> _selectedAbility;
		private int _value;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance of <see cref="AbilityScoreEditViewModel"/>
		/// </summary>
		public AbilityScoreEditViewModel(Ability ability, int value)
		{
			_selectedAbility = new Tuple<Ability, string>(ability, _stringService.GetString(ability));
			_value = Math.Min(Math.Max(1, value), 2);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets selected ability
		/// </summary>
		public Tuple<Ability, string> SelectedAbility
		{
			get { return _selectedAbility; }
			set
			{
				_selectedAbility = value;
				SelectionChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Gets or sets value
		/// </summary>
		public int Value
		{
			get { return _value; }
			set
			{
				_value = value;
				SelectionChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		#endregion
	}
}
