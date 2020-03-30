using System;
using CriticalCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ListItemViewModels
{
	public sealed class FeatListItemViewModel : NotifyPropertyChanged
	{
		#region Fields

		private FeatModel _featModel;
		private string _prerequisite;
		private bool _isSelected = false;

		#endregion

		#region Constructors

		public FeatListItemViewModel(FeatModel featModel)
		{
			_featModel = featModel;

			Initialize();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Feat model
		/// </summary>
		public FeatModel FeatModel
		{
			get { return _featModel; }
		}

		/// <summary>
		/// Name
		/// </summary>
		public string Name
		{
			get { return _featModel.Name; }
		}

		/// <summary>
		/// Gets prerequisite
		/// </summary>
		public string Prerequisite
		{
			get { return _prerequisite; }
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
		public void UpdateModel(FeatModel featModel)
		{
			_featModel = featModel;

			Initialize();

			OnPropertyChanged("");
		}

		#endregion

		#region Non-Pubic Methods

		private void Initialize()
		{
			_prerequisite = "Prerequisite: " + (String.IsNullOrWhiteSpace(_featModel.Prerequisite) ? "None" : _featModel.Prerequisite);
		}

		#endregion
	}
}
