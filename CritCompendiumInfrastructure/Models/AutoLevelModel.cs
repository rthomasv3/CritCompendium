using System;
using System.Collections.Generic;

namespace CriticalCompendiumInfrastructure.Models
{
	public sealed class AutoLevelModel
	{
		#region Fields

		private Guid _id;
		private int _level;
		private bool _scoreImprovement;
		private List<FeatureModel> _features = new List<FeatureModel>();

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a default instance of <see cref="AutoLevelModel"/>
		/// </summary>
		public AutoLevelModel()
		{
			_id = Guid.NewGuid();
		}

		/// <summary>
		/// Creates a copy of <see cref="AutoLevelModel"/>
		/// </summary>
		public AutoLevelModel(AutoLevelModel autoLevelModel)
		{
			_level = autoLevelModel.Level;

			_features = new List<FeatureModel>();
			foreach (FeatureModel featureModel in autoLevelModel.Features)
			{
				_features.Add(new FeatureModel(featureModel));
			}
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
		/// Gets or sets the level
		/// </summary>
		public int Level
		{
			get { return _level; }
			set { _level = value; }
		}

		/// <summary>
		/// Gets or sets score improvement
		/// </summary>
		public bool ScoreImprovement
		{
			get { return _scoreImprovement; }
			set { _scoreImprovement = value; }
		}

		/// <summary>
		/// Gets or sets the features
		/// </summary>
		public List<FeatureModel> Features
		{
			get { return _features; }
			set { _features = value; }
		}

		#endregion
	}
}
