using System;
using System.Collections.Generic;
using CriticalCompendiumInfrastructure.Enums;
using CriticalCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class LevelViewModel : ObjectViewModel
    {
        #region Fields

        private readonly LevelModel _levelModel;
        private readonly ClassViewModel _classViewModel;
        private readonly List<FeatureViewModel> _features = new List<FeatureViewModel>();
        private readonly List<FeatViewModel> _feats = new List<FeatViewModel>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="LevelViewModel"/>
        /// </summary>
        public LevelViewModel(LevelModel levelModel)
        {
            _levelModel = levelModel;

            if (levelModel.Class != null)
            {
                _classViewModel = new ClassViewModel(levelModel.Class);
            }

            foreach (FeatureModel feature in levelModel.Features)
            {
                _features.Add(new FeatureViewModel(feature));
            }

            foreach (FeatModel feat in levelModel.Feats)
            {
                _feats.Add(new FeatViewModel(feat));
            }
        }

        #endregion

        #region Properties

        /// <summary>
		/// Gets id
		/// </summary>
		public Guid ID
        {
            get { return _levelModel.ID; }
        }

        /// <summary>
        /// Gets level model
        /// </summary>
        public LevelModel LevelModel
        {
            get { return _levelModel; }
        }

        /// <summary>
        /// Gets or sets level
        /// </summary>
        public int Level
        {
            get { return _levelModel.Level; }
            set { _levelModel.Level = value; }
        }

        /// <summary>
        /// Gets or sets level of class
        /// </summary>
        public int LevelOfClass
        {
            get { return _levelModel.LevelOfClass; }
            set { _levelModel.LevelOfClass = value; }
        }

        /// <summary>
        /// Gets class
        /// </summary>
        public ClassViewModel Class
        {
            get { return _classViewModel; }
        }

        /// <summary>
        /// Gets hit die
        /// </summary>
        public string HitDie
        {
            get { return _classViewModel != null ? _classViewModel.HitDie : "0"; }
        }

        /// <summary>
        /// Gets features
        /// </summary>
        public List<FeatureViewModel> Features
        {
            get { return _features; }
        }

        /// <summary>
        /// Gets feats
        /// </summary>
        public List<FeatViewModel> Feats
        {
            get { return _feats; }
        }

        /// <summary>
        /// Gets or sets hit die result
        /// </summary>
        public int HitDieResult
        {
            get { return _levelModel.HitDieResult; }
            set { _levelModel.HitDieResult = value; }
        }

        /// <summary>
        /// Gets or sets hit die used
        /// </summary>
        public bool HitDieUsed
        {
            get { return _levelModel.HitDieUsed; }
            set
            {
                _levelModel.HitDieUsed = value;
                OnPropertyChanged(nameof(HitDieUsed));
            }
        }

        /// <summary>
		/// Gets or sets hit die rest result
		/// </summary>
		public int HitDieRestRoll
        {
            get { return _levelModel.HitDieRestRoll; }
            set
            {
                _levelModel.HitDieRestRoll = value;
                OnPropertyChanged(nameof(HitDieRestRoll));
            }
        }

        /// <summary>
        /// Gets or sets additional HP
        /// </summary>
        public int AdditionalHP
        {
            get { return _levelModel.AdditionalHP; }
            set { _levelModel.AdditionalHP = value; }
        }

        /// <summary>
        /// Gets or sets ability score improvements
        /// </summary>
        public List<KeyValuePair<Ability, int>> AbilityScoreImprovements
        {
            get { return _levelModel.AbilityScoreImprovements; }
            set { _levelModel.AbilityScoreImprovements = value; }
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion
    }
}
