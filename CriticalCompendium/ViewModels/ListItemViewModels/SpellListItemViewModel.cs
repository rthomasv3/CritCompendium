using System;
using CriticalCompendiumInfrastructure.Enums;
using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ListItemViewModels
{
	public sealed class SpellListItemViewModel : NotifyPropertyChanged
	{
		#region Fields

		private readonly StringService _stringService;
		private SpellModel _spellModel;
		private string _details;
		private bool _isSelected = false;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of <see cref="SpellListItemViewModel"/>
		/// </summary>
		public SpellListItemViewModel(SpellModel spellModel, StringService stringService)
		{
			_spellModel = spellModel;
			_stringService = stringService;

			Initialize();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets spell model
		/// </summary>
		public SpellModel SpellModel
		{
			get { return _spellModel; }
		}

		/// <summary>
		/// Gets name
		/// </summary>
		public string Name
		{
			get { return _spellModel.Name; }
		}

		/// <summary>
		/// Gets details
		/// </summary>
		public string Details
		{
			get { return _details; }
		}

		/// <summary>
		/// True if selected
		/// </summary>
		public bool IsSelected
		{
			get { return _isSelected; }
			set { Set(ref _isSelected, value); }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Updates the model
		/// </summary>
		public void UpdateModel(SpellModel spellModel)
		{
			_spellModel = spellModel;

			Initialize();

			OnPropertyChanged("");
		}

		#endregion

		#region Non-Public Methods

		private void Initialize()
		{
			string level = String.Empty;

            if (_spellModel.Level == -1)
            {
                level = "Unknown Level";
            }
            else if (_spellModel.Level == 0)
            {
                level = "Cantrip";
            }
            else
			{
				level = "Level " + _spellModel.Level.ToString();
			}

            _details = level + ", " + (_spellModel.SpellSchool != SpellSchool.None ? _stringService.GetString(_spellModel.SpellSchool) : "Unknown School");
		}

		#endregion
	}
}
