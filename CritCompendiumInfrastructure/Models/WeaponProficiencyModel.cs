namespace CritCompendiumInfrastructure.Models
{
    public sealed class WeaponProficiencyModel
    {
        #region Fields

        private bool _simpleWeaponsProficiency;
        private bool _martialWeaponsProficiency;

        private bool _clubProficiency;
        private bool _daggerProficiency;
        private bool _greatclubProficiency;
        private bool _handaxeProficiency;
        private bool _javelinProficiency;
        private bool _lightHammerProficiency;
        private bool _maceProficiency;
        private bool _quarterstaffProficiency;
        private bool _sickleProficiency;
        private bool _spearProficiency;
        private bool _crossbowLightProficiency;
        private bool _dartProficiency;
        private bool _shortbowProficiency;
        private bool _slingProficiency;

        private bool _battleaxeProficiency;
        private bool _flailProficiency;
        private bool _glaiveProficiency;
        private bool _greataxeProficiency;
        private bool _greatswordProficiency;
        private bool _halberdProficiency;
        private bool _lanceProficiency;
        private bool _longswordProficiency;
        private bool _maulProficiency;
        private bool _morningstarProficiency;
        private bool _pikeProficiency;
        private bool _rapierProficiency;
        private bool _scimitarProficiency;
        private bool _shortswordProficiency;
        private bool _tridentProficiency;
        private bool _warPickProficiency;
        private bool _warhammerProficiency;
        private bool _whipProficiency;
        private bool _blowgunProficiency;
        private bool _crossbowHandProficiency;
        private bool _crossbowHeavyProficiency;
        private bool _longbowProficiency;
        private bool _netProficiency;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="WeaponProficiencyModel"/>
        /// </summary>
        public WeaponProficiencyModel()
        {

        }

        /// <summary>
        /// Creates an instance of <see cref="WeaponProficiencyModel"/>
        /// </summary>
        public WeaponProficiencyModel(WeaponProficiencyModel armorProficiencyModel)
        {
            _simpleWeaponsProficiency = armorProficiencyModel.SimpleWeaponsProficiency;
            _martialWeaponsProficiency = armorProficiencyModel.MartialWeaponsProficiency;

            _clubProficiency = armorProficiencyModel.ClubProficiency;
            _daggerProficiency = armorProficiencyModel.DaggerProficiency;
            _greatclubProficiency = armorProficiencyModel.GreatclubProficiency;
            _handaxeProficiency = armorProficiencyModel.HandaxeProficiency;
            _javelinProficiency = armorProficiencyModel.JavelinProficiency;
            _lightHammerProficiency = armorProficiencyModel.LightHammerProficiency;
            _maceProficiency = armorProficiencyModel.MaceProficiency;
            _quarterstaffProficiency = armorProficiencyModel.QuarterstaffProficiency;
            _sickleProficiency = armorProficiencyModel.SickleProficiency;
            _spearProficiency = armorProficiencyModel.SpearProficiency;
            _crossbowLightProficiency = armorProficiencyModel.CrossbowLightProficiency;
            _dartProficiency = armorProficiencyModel.DartProficiency;
            _shortbowProficiency = armorProficiencyModel.ShortbowProficiency;
            _slingProficiency = armorProficiencyModel.SlingProficiency;

            _battleaxeProficiency = armorProficiencyModel.BattleaxeProficiency;
            _flailProficiency = armorProficiencyModel.FlailProficiency;
            _glaiveProficiency = armorProficiencyModel.GlaiveProficiency;
            _greataxeProficiency = armorProficiencyModel.GreataxeProficiency;
            _greatswordProficiency = armorProficiencyModel.GreatswordProficiency;
            _halberdProficiency = armorProficiencyModel.HalberdProficiency;
            _lanceProficiency = armorProficiencyModel.LanceProficiency;
            _longswordProficiency = armorProficiencyModel.LongswordProficiency;
            _maulProficiency = armorProficiencyModel.MaulProficiency;
            _morningstarProficiency = armorProficiencyModel.MorningstarProficiency;
            _pikeProficiency = armorProficiencyModel.PikeProficiency;
            _rapierProficiency = armorProficiencyModel.RapierProficiency;
            _scimitarProficiency = armorProficiencyModel.ScimitarProficiency;
            _shortswordProficiency = armorProficiencyModel.ShortswordProficiency;
            _tridentProficiency = armorProficiencyModel.TridentProficiency;
            _warPickProficiency = armorProficiencyModel.WarPickProficiency;
            _warhammerProficiency = armorProficiencyModel.WarhammerProficiency;
            _whipProficiency = armorProficiencyModel.WhipProficiency;
            _blowgunProficiency = armorProficiencyModel.BlowgunProficiency;
            _crossbowHandProficiency = armorProficiencyModel.CrossbowHandProficiency;
            _crossbowHeavyProficiency = armorProficiencyModel.CrossbowHeavyProficiency;
            _longbowProficiency = armorProficiencyModel.LongbowProficiency;
            _netProficiency = armorProficiencyModel.NetProficiency;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets simple weapons proficiency
        /// </summary>
        public bool SimpleWeaponsProficiency
        {
            get { return _simpleWeaponsProficiency; }
            set { _simpleWeaponsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets martial weapons proficiency
        /// </summary>
        public bool MartialWeaponsProficiency
        {
            get { return _martialWeaponsProficiency; }
            set { _martialWeaponsProficiency = value; }
        }


        /// <summary>
        /// Gets or sets club proficiency
        /// </summary>
        public bool ClubProficiency
        {
            get { return _clubProficiency; }
            set { _clubProficiency = value; }
        }

        /// <summary>
        /// Gets or sets dagger proficiency
        /// </summary>
        public bool DaggerProficiency
        {
            get { return _daggerProficiency; }
            set { _daggerProficiency = value; }
        }

        /// <summary>
        /// Gets or sets greatclub proficiency
        /// </summary>
        public bool GreatclubProficiency
        {
            get { return _greatclubProficiency; }
            set { _greatclubProficiency = value; }
        }

        /// <summary>
        /// Gets or sets handaxe proficiency
        /// </summary>
        public bool HandaxeProficiency
        {
            get { return _handaxeProficiency; }
            set { _handaxeProficiency = value; }
        }

        /// <summary>
        /// Gets or sets javelin proficiency
        /// </summary>
        public bool JavelinProficiency
        {
            get { return _javelinProficiency; }
            set { _javelinProficiency = value; }
        }

        /// <summary>
        /// Gets or sets light hammer proficiency
        /// </summary>
        public bool LightHammerProficiency
        {
            get { return _lightHammerProficiency; }
            set { _lightHammerProficiency = value; }
        }

        /// <summary>
        /// Gets or sets mace proficiency
        /// </summary>
        public bool MaceProficiency
        {
            get { return _maceProficiency; }
            set { _maceProficiency = value; }
        }

        /// <summary>
        /// Gets or sets quarterstaff proficiency
        /// </summary>
        public bool QuarterstaffProficiency
        {
            get { return _quarterstaffProficiency; }
            set { _quarterstaffProficiency = value; }
        }

        /// <summary>
        /// Gets or sets sickle proficiency
        /// </summary>
        public bool SickleProficiency
        {
            get { return _sickleProficiency; }
            set { _sickleProficiency = value; }
        }

        /// <summary>
        /// Gets or sets spear proficiency
        /// </summary>
        public bool SpearProficiency
        {
            get { return _spearProficiency; }
            set { _spearProficiency = value; }
        }

        /// <summary>
        /// Gets or sets crossbow light proficiency
        /// </summary>
        public bool CrossbowLightProficiency
        {
            get { return _crossbowLightProficiency; }
            set { _crossbowLightProficiency = value; }
        }

        /// <summary>
        /// Gets or sets dart proficiency
        /// </summary>
        public bool DartProficiency
        {
            get { return _dartProficiency; }
            set { _dartProficiency = value; }
        }

        /// <summary>
        /// Gets or sets shortbow proficiency
        /// </summary>
        public bool ShortbowProficiency
        {
            get { return _shortbowProficiency; }
            set { _shortbowProficiency = value; }
        }

        /// <summary>
        /// Gets or sets sling proficiency
        /// </summary>
        public bool SlingProficiency
        {
            get { return _slingProficiency; }
            set { _slingProficiency = value; }
        }


        /// <summary>
        /// Gets or sets battleaxe  proficiency
        /// </summary>
        public bool BattleaxeProficiency
        {
            get { return _battleaxeProficiency; }
            set { _battleaxeProficiency = value; }
        }

        /// <summary>
        /// Gets or sets flail proficiency
        /// </summary>
        public bool FlailProficiency
        {
            get { return _flailProficiency; }
            set { _flailProficiency = value; }
        }

        /// <summary>
        /// Gets or sets glaive proficiency
        /// </summary>
        public bool GlaiveProficiency
        {
            get { return _glaiveProficiency; }
            set { _glaiveProficiency = value; }
        }

        /// <summary>
        /// Gets or sets greataxe proficiency
        /// </summary>
        public bool GreataxeProficiency
        {
            get { return _greataxeProficiency; }
            set { _greataxeProficiency = value; }
        }

        /// <summary>
        /// Gets or sets greatsword proficiency
        /// </summary>
        public bool GreatswordProficiency
        {
            get { return _greatswordProficiency; }
            set { _greatswordProficiency = value; }
        }

        /// <summary>
        /// Gets or sets halberd proficiency
        /// </summary>
        public bool HalberdProficiency
        {
            get { return _halberdProficiency; }
            set { _halberdProficiency = value; }
        }

        /// <summary>
        /// Gets or sets lance proficiency
        /// </summary>
        public bool LanceProficiency
        {
            get { return _lanceProficiency; }
            set { _lanceProficiency = value; }
        }

        /// <summary>
        /// Gets or sets longsword proficiency
        /// </summary>
        public bool LongswordProficiency
        {
            get { return _longswordProficiency; }
            set { _longswordProficiency = value; }
        }

        /// <summary>
        /// Gets or sets maul proficiency
        /// </summary>
        public bool MaulProficiency
        {
            get { return _maulProficiency; }
            set { _maulProficiency = value; }
        }

        /// <summary>
        /// Gets or sets morningstar proficiency
        /// </summary>
        public bool MorningstarProficiency
        {
            get { return _morningstarProficiency; }
            set { _morningstarProficiency = value; }
        }

        /// <summary>
        /// Gets or sets pike proficiency
        /// </summary>
        public bool PikeProficiency
        {
            get { return _pikeProficiency; }
            set { _pikeProficiency = value; }
        }

        /// <summary>
        /// Gets or sets rapier proficiency
        /// </summary>
        public bool RapierProficiency
        {
            get { return _rapierProficiency; }
            set { _rapierProficiency = value; }
        }

        /// <summary>
        /// Gets or sets scimitar proficiency
        /// </summary>
        public bool ScimitarProficiency
        {
            get { return _scimitarProficiency; }
            set { _scimitarProficiency = value; }
        }

        /// <summary>
        /// Gets or sets shortsword proficiency
        /// </summary>
        public bool ShortswordProficiency
        {
            get { return _shortswordProficiency; }
            set { _shortswordProficiency = value; }
        }

        /// <summary>
        /// Gets or sets trident proficiency
        /// </summary>
        public bool TridentProficiency
        {
            get { return _tridentProficiency; }
            set { _tridentProficiency = value; }
        }

        /// <summary>
        /// Gets or sets war pick proficiency
        /// </summary>
        public bool WarPickProficiency
        {
            get { return _warPickProficiency; }
            set { _warPickProficiency = value; }
        }

        /// <summary>
        /// Gets or sets warhammer proficiency
        /// </summary>
        public bool WarhammerProficiency
        {
            get { return _warhammerProficiency; }
            set { _warhammerProficiency = value; }
        }

        /// <summary>
        /// Gets or sets whip proficiency
        /// </summary>
        public bool WhipProficiency
        {
            get { return _whipProficiency; }
            set { _whipProficiency = value; }
        }

        /// <summary>
        /// Gets or sets blowgun proficiency
        /// </summary>
        public bool BlowgunProficiency
        {
            get { return _blowgunProficiency; }
            set { _blowgunProficiency = value; }
        }

        /// <summary>
        /// Gets or sets crossbow hand proficiency
        /// </summary>
        public bool CrossbowHandProficiency
        {
            get { return _crossbowHandProficiency; }
            set { _crossbowHandProficiency = value; }
        }

        /// <summary>
        /// Gets or sets crossbow heavy proficiency
        /// </summary>
        public bool CrossbowHeavyProficiency
        {
            get { return _crossbowHeavyProficiency; }
            set { _crossbowHeavyProficiency = value; }
        }

        /// <summary>
        /// Gets or sets longbow proficiency
        /// </summary>
        public bool LongbowProficiency
        {
            get { return _longbowProficiency; }
            set { _longbowProficiency = value; }
        }

        /// <summary>
        /// Gets or sets net proficiency
        /// </summary>
        public bool NetProficiency
        {
            get { return _netProficiency; }
            set { _netProficiency = value; }
        }

        #endregion
    }
}
