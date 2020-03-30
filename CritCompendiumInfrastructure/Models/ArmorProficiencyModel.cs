namespace CriticalCompendiumInfrastructure.Models
{
    public sealed class ArmorProficiencyModel
    {
        #region Fields

        private bool _lightArmorProficiency;
        private bool _mediumArmorProficiency;
        private bool _heavyArmorProficiency;
        private bool _shieldsProficiency;

        private bool _paddedProficiency;
        private bool _leatherProficiency;
        private bool _studdedLeatherProficiency;

        private bool _hideProficiency;
        private bool _chainShirtProficiency;
        private bool _scaleMailProficiency;
        private bool _breastplateProficiency;
        private bool _halfPlateProficiency;

        private bool _ringMailProficiency;
        private bool _chainMailProficiency;
        private bool _splintProficiency;
        private bool _plateProficiency;

        private bool _shieldProficiency;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="ArmorProficiencyModel"/>
        /// </summary>
        public ArmorProficiencyModel()
        {

        }

        /// <summary>
        /// Creates an instance of <see cref="ArmorProficiencyModel"/>
        /// </summary>
        public ArmorProficiencyModel(ArmorProficiencyModel armorProficiencyModel)
        {
            _lightArmorProficiency = armorProficiencyModel.LightArmorProficiency;
            _mediumArmorProficiency = armorProficiencyModel.MediumArmorProficiency;
            _heavyArmorProficiency = armorProficiencyModel.HeavyArmorProficiency;
            _shieldsProficiency = armorProficiencyModel.ShieldsProficiency;

            _paddedProficiency = armorProficiencyModel.PaddedProficiency;
            _leatherProficiency = armorProficiencyModel.LeatherProficiency;
            _studdedLeatherProficiency = armorProficiencyModel.StuddedLeatherProficiency;

            _hideProficiency = armorProficiencyModel.HideProficiency;
            _chainShirtProficiency = armorProficiencyModel.ChainShirtProficiency;
            _scaleMailProficiency = armorProficiencyModel.ScaleMailProficiency;
            _breastplateProficiency = armorProficiencyModel.BreastplateProficiency;
            _halfPlateProficiency = armorProficiencyModel.HalfPlateProficiency;

            _ringMailProficiency = armorProficiencyModel.RingMailProficiency;
            _chainMailProficiency = armorProficiencyModel.ChainMailProficiency;
            _splintProficiency = armorProficiencyModel.SplintProficiency;
            _plateProficiency = armorProficiencyModel.PlateProficiency;

            _shieldProficiency = armorProficiencyModel.ShieldProficiency;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets light armor proficiency
        /// </summary>
        public bool LightArmorProficiency
        {
            get { return _lightArmorProficiency; }
            set { _lightArmorProficiency = value; }
        }

        /// <summary>
        /// Gets or sets medium armor proficiency
        /// </summary>
        public bool MediumArmorProficiency
        {
            get { return _mediumArmorProficiency; }
            set { _mediumArmorProficiency = value; }
        }

        /// <summary>
        /// Gets or sets heavy armor proficiency
        /// </summary>
        public bool HeavyArmorProficiency
        {
            get { return _heavyArmorProficiency; }
            set { _heavyArmorProficiency = value; }
        }

        /// <summary>
        /// Gets or sets shields proficiency
        /// </summary>
        public bool ShieldsProficiency
        {
            get { return _shieldsProficiency; }
            set { _shieldsProficiency = value; }
        }


        /// <summary>
        /// Gets or sets padded proficiency
        /// </summary>
        public bool PaddedProficiency
        {
            get { return _paddedProficiency; }
            set { _paddedProficiency = value; }
        }

        /// <summary>
        /// Gets or sets leather proficiency
        /// </summary>
        public bool LeatherProficiency
        {
            get { return _leatherProficiency; }
            set { _leatherProficiency = value; }
        }

        /// <summary>
        /// Gets or sets studded leather proficiency
        /// </summary>
        public bool StuddedLeatherProficiency
        {
            get { return _studdedLeatherProficiency; }
            set { _studdedLeatherProficiency = value; }
        }


        /// <summary>
        /// Gets or sets hide proficiency
        /// </summary>
        public bool HideProficiency
        {
            get { return _hideProficiency; }
            set { _hideProficiency = value; }
        }

        /// <summary>
        /// Gets or sets chain shirt proficiency
        /// </summary>
        public bool ChainShirtProficiency
        {
            get { return _chainShirtProficiency; }
            set { _chainShirtProficiency = value; }
        }

        /// <summary>
        /// Gets or sets scale mail proficiency
        /// </summary>
        public bool ScaleMailProficiency
        {
            get { return _scaleMailProficiency; }
            set { _scaleMailProficiency = value; }
        }

        /// <summary>
        /// Gets or sets breastplate proficiency
        /// </summary>
        public bool BreastplateProficiency
        {
            get { return _breastplateProficiency; }
            set { _breastplateProficiency = value; }
        }

        /// <summary>
        /// Gets or sets half plate proficiency
        /// </summary>
        public bool HalfPlateProficiency
        {
            get { return _halfPlateProficiency; }
            set { _halfPlateProficiency = value; }
        }


        /// <summary>
        /// Gets or sets ring mail proficiency
        /// </summary>
        public bool RingMailProficiency
        {
            get { return _ringMailProficiency; }
            set { _ringMailProficiency = value; }
        }

        /// <summary>
        /// Gets or sets chain mail proficiency
        /// </summary>
        public bool ChainMailProficiency
        {
            get { return _chainMailProficiency; }
            set { _chainMailProficiency = value; }
        }

        /// <summary>
        /// Gets or sets splint proficiency
        /// </summary>
        public bool SplintProficiency
        {
            get { return _splintProficiency; }
            set { _splintProficiency = value; }
        }

        /// <summary>
        /// Gets or sets plate proficiency
        /// </summary>
        public bool PlateProficiency
        {
            get { return _plateProficiency; }
            set { _plateProficiency = value; }
        }


        /// <summary>
        /// Gets or sets shield proficiency
        /// </summary>
        public bool ShieldProficiency
        {
            get { return _shieldProficiency; }
            set { _shieldProficiency = value; }
        }

        #endregion
    }
}
