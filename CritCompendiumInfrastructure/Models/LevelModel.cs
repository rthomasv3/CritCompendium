using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Models
{
	public sealed class LevelModel
	{
		#region Fields

		private Guid _id;
		private int _level;
		private int _levelOfClass;
		private ClassModel _class;
		private List<FeatureModel> _features = new List<FeatureModel>();
		private List<FeatModel> _feats = new List<FeatModel>();
		private int _hitDieResult;
		private bool _hitDieUsed;
        private int _hitDieRestRoll;
		private int _additionalHP;
		private List<KeyValuePair<Ability, int>> _abilityScoreImprovements = new List<KeyValuePair<Ability, int>>();

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a default instance of <see cref="LevelModel"/>
		/// </summary>
		public LevelModel()
		{
			_id = Guid.NewGuid();
		}

		/// <summary>
		/// Creates a copy of <see cref="LevelModel"/>
		/// </summary>
		public LevelModel(LevelModel levelModel)
		{
			_id = levelModel.ID;
			_level = levelModel.Level;
			_levelOfClass = levelModel.LevelOfClass;
			_class = new ClassModel(levelModel.Class);
			_features = new List<FeatureModel>();
			foreach (FeatureModel featureModel in levelModel.Features)
			{
				_features.Add(new FeatureModel(featureModel));
			}
			_feats = new List<FeatModel>();
			foreach (FeatModel featModel in levelModel.Feats)
			{
				_feats.Add(new FeatModel(featModel));
			}
			_hitDieResult = levelModel.HitDieResult;
			_hitDieUsed = levelModel.HitDieUsed;
            _hitDieRestRoll = levelModel.HitDieRestRoll;
			_additionalHP = levelModel.AdditionalHP;
			_abilityScoreImprovements = new List<KeyValuePair<Ability, int>>(levelModel.AbilityScoreImprovements);
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
		/// Gets or sets level
		/// </summary>
		public int Level
		{
			get { return _level; }
			set { _level = value; }
		}

		/// <summary>
		/// Gets or sets level of class
		/// </summary>
		public int LevelOfClass
		{
			get { return _levelOfClass; }
			set { _levelOfClass = value; }
		}

		/// <summary>
		/// Gets or sets class
		/// </summary>
		public ClassModel Class
		{
			get { return _class; }
			set { _class = value; }
		}

		/// <summary>
		/// Gets or sets features
		/// </summary>
		public List<FeatureModel> Features
		{
			get { return _features; }
			set { _features = value; }
		}

		/// <summary>
		/// Gets or sets feats
		/// </summary>
		public List<FeatModel> Feats
		{
			get { return _feats; }
			set { _feats = value; }
		}

		/// <summary>
		/// Gets or sets hit die result
		/// </summary>
		public int HitDieResult
		{
			get { return _hitDieResult; }
			set { _hitDieResult = value; }
		}

		/// <summary>
		/// Gets or sets hit die used
		/// </summary>
		public bool HitDieUsed
		{
			get { return _hitDieUsed; }
			set { _hitDieUsed = value; }
		}

        /// <summary>
		/// Gets or sets hit die rest result
		/// </summary>
		public int HitDieRestRoll
        {
            get { return _hitDieRestRoll; }
            set { _hitDieRestRoll = value; }
        }

        /// <summary>
        /// Gets or sets additional HP
        /// </summary>
        public int AdditionalHP
		{
			get { return _additionalHP; }
			set { _additionalHP = value; }
		}

		/// <summary>
		/// Gets or sets ability score improvements
		/// </summary>
		public List<KeyValuePair<Ability, int>> AbilityScoreImprovements
		{
			get { return _abilityScoreImprovements; }
			set { _abilityScoreImprovements = value; }
		}
		
		#endregion
	}
}
