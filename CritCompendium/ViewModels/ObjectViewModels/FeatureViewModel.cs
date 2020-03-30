using System;
using CriticalCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
	public sealed class FeatureViewModel
	{
		#region Events

		public event EventHandler IsSelectedChanged;

		#endregion

		#region Fields

		private readonly FeatureModel _featureModel;
		private bool _selected;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance of <see cref="FeatureViewModel"/>
		/// </summary>
		public FeatureViewModel(FeatureModel featureModel)
		{
			_featureModel = featureModel;
			_selected = !_featureModel.Optional;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets feature model
		/// </summary>
		public FeatureModel FeatureModel
		{
			get { return _featureModel; }
		}

		/// <summary>
		/// Gets name
		/// </summary>
		public string Name
		{
			get { return _featureModel.Name; }
		}

		/// <summary>
		/// Gets text
		/// </summary>
		public string Text
		{
			get { return _featureModel.Text; }
		}

		/// <summary>
		/// Gets or sets is selected
		/// </summary>
		public bool IsSelected
		{
			get { return _selected; }
			set
			{
				_selected = value;
				IsSelectedChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Gets or is optional
		/// </summary>
		public bool IsOptional
		{
			get { return _featureModel.Optional; }
		}

		#endregion
	}
}
