using CriticalCompendiumInfrastructure.Enums;
using CriticalCompendiumInfrastructure.Models;
using System;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class SkillViewModel : NotifyPropertyChanged
    {
        #region Events

        public event EventHandler ProficiencyChanged;

        #endregion

        #region Fields

        private readonly SkillModel _skillModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="SkillViewModel"/>
        /// </summary>
        public SkillViewModel(SkillModel skillModel)
        {
            _skillModel = skillModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets skill model
        /// </summary>
        public SkillModel SkillModel
        {
            get { return _skillModel; }
        }

        /// <summary>
        /// Gets skill
        /// </summary>
        public Skill Skill
        {
            get { return _skillModel.Skill; }
        }

        /// <summary>
        /// Gets skill string
        /// </summary>
        public string SkillString
        {
            get { return _skillModel.SkillString; }
        }

        /// <summary>
        /// Gets skill ability string
        /// </summary>
        public string SkillAbilityString
        {
            get { return _skillModel.SkillAbilityString; }
        }

        /// <summary>
        /// Gets or sets value
        /// </summary>
        public int Bonus
        {
            get { return _skillModel.Bonus; }
            set
            {
                if (_skillModel.Bonus != value)
                {
                    _skillModel.Bonus = value;
                    OnPropertyChanged(nameof(Bonus));
                    OnPropertyChanged(nameof(BonusString));
                }
            }
        }

        /// <summary>
        /// Gets bonus string
        /// </summary>
        public string BonusString
        {
            get { return _skillModel.BonusString; }
        }

        /// <summary>
        /// Gets or set proficient
        /// </summary>
        public bool Proficient
        {
            get { return _skillModel.Proficient; }
            set
            {
                if (_skillModel.Proficient != value)
                {
                    _skillModel.Proficient = value;
                    OnPropertyChanged(nameof(Proficient));
                    ProficiencyChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or set expertise
        /// </summary>
        public bool Expertise
        {
            get { return _skillModel.Expertise; }
            set
            {
                if (_skillModel.Expertise != value)
                {
                    _skillModel.Expertise = value;
                    OnPropertyChanged(nameof(Expertise));
                    ProficiencyChanged?.Invoke(this, EventArgs.Empty);

                    if (value && !_skillModel.Proficient)
                    {
                        Proficient = true;
                    }
                }
            }
        }

        #endregion
    }
}
