namespace CriticalCompendiumInfrastructure.Models
{
    public sealed class ToolProficiencyModel
    {
        #region Fields

        private bool _alchemistsSuppliesProficiency;
        private bool _brewersSuppliesProficiency;
        private bool _calligraphersSuppliesProficiency;
        private bool _carpentersToolsProficiency;
        private bool _cartographersToolsProficiency;
        private bool _cobblersToolsProficiency;
        private bool _cooksUtensilsProficiency;
        private bool _glassblowersToolsProficiency;
        private bool _jewelerssToolsProficiency;
        private bool _leatherworkersToolsProficiency;
        private bool _masonsToolsProficiency;
        private bool _paintersSuppliesProficiency;
        private bool _pottersToolsProficiency;
        private bool _smithsToolsProficiency;
        private bool _tinkersToolsProficiency;
        private bool _weaversToolsProficiency;
        private bool _woodcarversToolsProficiency;

        private bool _diceSetProficiency;
        private bool _playingCardSetProficiency;

        private bool _disguiseKitProficiency;
        private bool _forgeryKitProficiency;
        private bool _herbalismKitProficiency;
        private bool _poisonersKitProficiency;

        private bool _bagpipesProficiency;
        private bool _drumProficiency;
        private bool _dulcimerProficiency;
        private bool _fluteProficiency;
        private bool _luteProficiency;
        private bool _lyreProficiency;
        private bool _hornProficiency;
        private bool _panFluteProficiency;
        private bool _shawmProficiency;
        private bool _violProficiency;

        private bool _navigatorsToolsProficiency;
        private bool _thievesToolsProficiency;

        private bool _landVehiclesProficiency;
        private bool _waterVehiclesProficiency;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="ToolProficiencyModel"/>
        /// </summary>
        public ToolProficiencyModel()
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="ToolProficiencyModel"/>
        /// </summary>
        public ToolProficiencyModel(ToolProficiencyModel toolProficiencyModel)
        {
            _alchemistsSuppliesProficiency = toolProficiencyModel.AlchemistsSuppliesProficiency;
            _brewersSuppliesProficiency = toolProficiencyModel.BrewersSuppliesProficiency;
            _calligraphersSuppliesProficiency = toolProficiencyModel.CalligraphersSuppliesProficiency;
            _carpentersToolsProficiency = toolProficiencyModel.CarpentersToolsProficiency;
            _cartographersToolsProficiency = toolProficiencyModel.CartographersToolsProficiency;
            _cobblersToolsProficiency = toolProficiencyModel.CobblersToolsProficiency;
            _cooksUtensilsProficiency = toolProficiencyModel.CooksUtensilsProficiency;
            _glassblowersToolsProficiency = toolProficiencyModel.GlassblowersToolsProficiency;
            _jewelerssToolsProficiency = toolProficiencyModel.JewelerssToolsProficiency;
            _leatherworkersToolsProficiency = toolProficiencyModel.LeatherworkersToolsProficiency;
            _masonsToolsProficiency = toolProficiencyModel.MasonsToolsProficiency;
            _paintersSuppliesProficiency = toolProficiencyModel.PaintersSuppliesProficiency;
            _pottersToolsProficiency = toolProficiencyModel.PottersToolsProficiency;
            _smithsToolsProficiency = toolProficiencyModel.SmithsToolsProficiency;
            _tinkersToolsProficiency = toolProficiencyModel.TinkersToolsProficiency;
            _weaversToolsProficiency = toolProficiencyModel.WeaversToolsProficiency;
            _woodcarversToolsProficiency = toolProficiencyModel.WoodcarversToolsProficiency;

            _diceSetProficiency = toolProficiencyModel.DiceSetProficiency;
            _playingCardSetProficiency = toolProficiencyModel.PlayingCardSetProficiency;

            _disguiseKitProficiency = toolProficiencyModel.DisguiseKitProficiency;
            _forgeryKitProficiency = toolProficiencyModel.ForgeryKitProficiency;
            _herbalismKitProficiency = toolProficiencyModel.HerbalismKitProficiency;
            _poisonersKitProficiency = toolProficiencyModel.PoisonersKitProficiency;

            _bagpipesProficiency = toolProficiencyModel.BagpipesProficiency;
            _drumProficiency = toolProficiencyModel.DrumProficiency;
            _dulcimerProficiency = toolProficiencyModel.DulcimerProficiency;
            _fluteProficiency = toolProficiencyModel.FluteProficiency;
            _luteProficiency = toolProficiencyModel.LuteProficiency;
            _lyreProficiency = toolProficiencyModel.LyreProficiency;
            _hornProficiency = toolProficiencyModel.HornProficiency;
            _panFluteProficiency = toolProficiencyModel.PanFluteProficiency;
            _shawmProficiency = toolProficiencyModel.ShawmProficiency;
            _violProficiency = toolProficiencyModel.ViolProficiency;

            _navigatorsToolsProficiency = toolProficiencyModel.NavigatorsToolsProficiency;
            _thievesToolsProficiency = toolProficiencyModel.ThievesToolsProficiency;

            _landVehiclesProficiency = toolProficiencyModel.LandVehiclesProficiency;
            _waterVehiclesProficiency = toolProficiencyModel.WaterVehiclesProficiency;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets alchemists supplies proficiency
        /// </summary>
        public bool AlchemistsSuppliesProficiency
        {
            get { return _alchemistsSuppliesProficiency; }
            set { _alchemistsSuppliesProficiency = value; }
        }

        /// <summary>
        /// Gets or sets brewers supplies proficiency
        /// </summary>
        public bool BrewersSuppliesProficiency
        {
            get { return _brewersSuppliesProficiency; }
            set { _brewersSuppliesProficiency = value; }
        }

        /// <summary>
        /// Gets or sets calligraphers supplies proficiency
        /// </summary>
        public bool CalligraphersSuppliesProficiency
        {
            get { return _calligraphersSuppliesProficiency; }
            set { _calligraphersSuppliesProficiency = value; }
        }

        /// <summary>
        /// Gets or sets carpenters tools proficiency
        /// </summary>
        public bool CarpentersToolsProficiency
        {
            get { return _carpentersToolsProficiency; }
            set { _carpentersToolsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets cartographers tools proficiency
        /// </summary>
        public bool CartographersToolsProficiency
        {
            get { return _cartographersToolsProficiency; }
            set { _cartographersToolsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets cobblers tools proficiency
        /// </summary>
        public bool CobblersToolsProficiency
        {
            get { return _cobblersToolsProficiency; }
            set { _cobblersToolsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets cooks utensils proficiency
        /// </summary>
        public bool CooksUtensilsProficiency
        {
            get { return _cooksUtensilsProficiency; }
            set { _cooksUtensilsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets glassblowers tools proficiency
        /// </summary>
        public bool GlassblowersToolsProficiency
        {
            get { return _glassblowersToolsProficiency; }
            set { _glassblowersToolsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets jewelerss tools proficiency
        /// </summary>
        public bool JewelerssToolsProficiency
        {
            get { return _jewelerssToolsProficiency; }
            set { _jewelerssToolsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets leatherworkers tools proficiency
        /// </summary>
        public bool LeatherworkersToolsProficiency
        {
            get { return _leatherworkersToolsProficiency; }
            set { _leatherworkersToolsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets masons tools proficiency
        /// </summary>
        public bool MasonsToolsProficiency
        {
            get { return _masonsToolsProficiency; }
            set { _masonsToolsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets painters supplies proficiency
        /// </summary>
        public bool PaintersSuppliesProficiency
        {
            get { return _paintersSuppliesProficiency; }
            set { _paintersSuppliesProficiency = value; }
        }

        /// <summary>
        /// Gets or sets potters tools proficiency
        /// </summary>
        public bool PottersToolsProficiency
        {
            get { return _pottersToolsProficiency; }
            set { _pottersToolsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets smiths tools proficiency
        /// </summary>
        public bool SmithsToolsProficiency
        {
            get { return _smithsToolsProficiency; }
            set { _smithsToolsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets tinkers tools proficiency
        /// </summary>
        public bool TinkersToolsProficiency
        {
            get { return _tinkersToolsProficiency; }
            set { _tinkersToolsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets weavers tools proficiency
        /// </summary>
        public bool WeaversToolsProficiency
        {
            get { return _weaversToolsProficiency; }
            set { _weaversToolsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets woodcarvers tools proficiency
        /// </summary>
        public bool WoodcarversToolsProficiency
        {
            get { return _woodcarversToolsProficiency; }
            set { _woodcarversToolsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets dice set proficiency
        /// </summary>
        public bool DiceSetProficiency
        {
            get { return _diceSetProficiency; }
            set { _diceSetProficiency = value; }
        }

        /// <summary>
        /// Gets or sets playing card set proficiency
        /// </summary>
        public bool PlayingCardSetProficiency
        {
            get { return _playingCardSetProficiency; }
            set { _playingCardSetProficiency = value; }
        }

        /// <summary>
        /// Gets or sets disguise kit proficiency
        /// </summary>
        public bool DisguiseKitProficiency
        {
            get { return _disguiseKitProficiency; }
            set { _disguiseKitProficiency = value; }
        }

        /// <summary>
        /// Gets or sets forgery kit proficiency
        /// </summary>
        public bool ForgeryKitProficiency
        {
            get { return _forgeryKitProficiency; }
            set { _forgeryKitProficiency = value; }
        }

        /// <summary>
        /// Gets or sets herbalism kit proficiency
        /// </summary>
        public bool HerbalismKitProficiency
        {
            get { return _herbalismKitProficiency; }
            set { _herbalismKitProficiency = value; }
        }

        /// <summary>
        /// Gets or sets poisoners kit proficiency
        /// </summary>
        public bool PoisonersKitProficiency
        {
            get { return _poisonersKitProficiency; }
            set { _poisonersKitProficiency = value; }
        }

        /// <summary>
        /// Gets or sets bagpipes proficiency
        /// </summary>
        public bool BagpipesProficiency
        {
            get { return _bagpipesProficiency; }
            set { _bagpipesProficiency = value; }
        }

        /// <summary>
        /// Gets or sets drum proficiency
        /// </summary>
        public bool DrumProficiency
        {
            get { return _drumProficiency; }
            set { _drumProficiency = value; }
        }

        /// <summary>
        /// Gets or sets dulcimer proficiency
        /// </summary>
        public bool DulcimerProficiency
        {
            get { return _dulcimerProficiency; }
            set { _dulcimerProficiency = value; }
        }

        /// <summary>
        /// Gets or sets flute proficiency
        /// </summary>
        public bool FluteProficiency
        {
            get { return _fluteProficiency; }
            set { _fluteProficiency = value; }
        }

        /// <summary>
        /// Gets or sets lute proficiency
        /// </summary>
        public bool LuteProficiency
        {
            get { return _luteProficiency; }
            set { _luteProficiency = value; }
        }

        /// <summary>
        /// Gets or sets lyre proficiency
        /// </summary>
        public bool LyreProficiency
        {
            get { return _lyreProficiency; }
            set { _lyreProficiency = value; }
        }

        /// <summary>
        /// Gets or sets horn proficiency
        /// </summary>
        public bool HornProficiency
        {
            get { return _hornProficiency; }
            set { _hornProficiency = value; }
        }

        /// <summary>
        /// Gets or sets pan flute proficiency
        /// </summary>
        public bool PanFluteProficiency
        {
            get { return _panFluteProficiency; }
            set { _panFluteProficiency = value; }
        }

        /// <summary>
        /// Gets or sets shawm proficiency
        /// </summary>
        public bool ShawmProficiency
        {
            get { return _shawmProficiency; }
            set { _shawmProficiency = value; }
        }

        /// <summary>
        /// Gets or sets viol proficiency
        /// </summary>
        public bool ViolProficiency
        {
            get { return _violProficiency; }
            set { _violProficiency = value; }
        }

        /// <summary>
        /// Gets or sets navigators tools proficiency
        /// </summary>
        public bool NavigatorsToolsProficiency
        {
            get { return _navigatorsToolsProficiency; }
            set { _navigatorsToolsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets thieves tools proficiency
        /// </summary>
        public bool ThievesToolsProficiency
        {
            get { return _thievesToolsProficiency; }
            set { _thievesToolsProficiency = value; }
        }

        /// <summary>
        /// Gets or sets land vehicles proficiency
        /// </summary>
        public bool LandVehiclesProficiency
        {
            get { return _landVehiclesProficiency; }
            set { _landVehiclesProficiency = value; }
        }

        /// <summary>
        /// Gets or sets water vehicles proficiency
        /// </summary>
        public bool WaterVehiclesProficiency
        {
            get { return _waterVehiclesProficiency; }
            set { _waterVehiclesProficiency = value; }
        }

        #endregion
    }
}
