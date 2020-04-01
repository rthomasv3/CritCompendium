using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Services;

namespace CritCompendiumInfrastructure.Models
{
    public class AbilityModel
    {
        #region Fields

        private readonly StatService _statService = DependencyResolver.Resolve<StatService>();
        private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
        private readonly Ability _ability;
        private readonly string _abilityString;
        private int _saveBonus;
        private int _checkBonus;
        private bool _proficient;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="AbilityModel"/>
        /// </summary>
        public AbilityModel(Ability ability, int saveBonus, int checkBonus, bool proficient)
        {
            _ability = ability;
            _saveBonus = saveBonus;
            _checkBonus = checkBonus;
            _proficient = proficient;
            
            _abilityString = _stringService.GetString(ability);
        }

        /// <summary>
        /// Creates a copy of <see cref="AbilityModel"/>
        /// </summary>
        public AbilityModel(AbilityModel abilityModel)
        {
            _ability = abilityModel.Ability;
            _abilityString = abilityModel.AbilityString;
            _saveBonus = abilityModel.SaveBonus;
            _checkBonus = abilityModel.CheckBonus;
            _proficient = abilityModel.Proficient;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets ability
        /// </summary>
        public Ability Ability
        {
            get { return _ability; }
        }

        /// <summary>
        /// Gets ability string
        /// </summary>
        public string AbilityString
        {
            get { return _abilityString; }
        }

        /// <summary>
        /// Gets or sets save value
        /// </summary>
        public int SaveBonus
        {
            get { return _saveBonus; }
            set { _saveBonus = value; }
        }

        /// <summary>
        /// Gets or sets check value
        /// </summary>
        public int CheckBonus
        {
            get { return _checkBonus; }
            set { _checkBonus = value; }
        }

        /// <summary>
        /// Gets save bonus string
        /// </summary>
        public string SaveBonusString
        {
            get { return _statService.AddPlusOrMinus(_saveBonus); }
        }

        /// <summary>
        /// Gets check bonus string
        /// </summary>
        public string CheckBonusString
        {
            get { return _statService.AddPlusOrMinus(_checkBonus); }
        }

        /// <summary>
        /// Gets or set proficient
        /// </summary>
        public bool Proficient
        {
            get { return _proficient; }
            set { _proficient = value; }
        }

        #endregion
    }
}
