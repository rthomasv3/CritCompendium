using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ListItemViewModels
{
	public sealed class ClassListItemViewModel : NotifyPropertyChanged
	{
		#region Fields

		private ClassModel _classModel;
		private readonly StringService _stringService;
		private string _abilities;
		private bool _isSelected = false;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of <see cref="ClassListItemViewModel"/>
		/// </summary>
		public ClassListItemViewModel(ClassModel classModel, StringService stringService)
		{
			_classModel = classModel;
			_stringService = stringService;

			Initialize();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Class model
		/// </summary>
		public ClassModel ClassModel
		{
			get { return _classModel; }
		}

		/// <summary>
		/// Class name
		/// </summary>
		public string Name
		{
			get { return _classModel.Name; }
		}

		/// <summary>
		/// Class skills
		/// </summary>
		public string Skills
		{
			get { return String.Join(", ", _classModel.AbilityProficiencies); }
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
		public void UpdateModel(ClassModel classModel)
		{
			_classModel = classModel;
			OnPropertyChanged("");
		}

		#endregion

		#region Non-Public Methods

		private void Initialize()
		{
			List<string> abilities = new List<string>();
			foreach (Ability ability in _classModel.AbilityProficiencies)
			{
				abilities.Add(_stringService.GetString(ability));
			}

			_abilities = String.Join(", ", abilities);
		}

		#endregion
	}
}
