using CritCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class ToolProficiencyViewModel : NotifyPropertyChanged
    {
        #region Fields

        private readonly ToolProficiencyModel _toolProficiencyModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="ToolProficiencyViewModel"/>
        /// </summary>
        public ToolProficiencyViewModel(ToolProficiencyModel toolProficiencyModel)
        {
            _toolProficiencyModel = toolProficiencyModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets alchemists supplies proficiency
        /// </summary>
        public bool AlchemistsSuppliesProficiency
        {
            get { return _toolProficiencyModel.AlchemistsSuppliesProficiency; }
            set
            {
                _toolProficiencyModel.AlchemistsSuppliesProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets brewers supplies proficiency
        /// </summary>
        public bool BrewersSuppliesProficiency
        {
            get { return _toolProficiencyModel.BrewersSuppliesProficiency; }
            set
            {
                _toolProficiencyModel.BrewersSuppliesProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets calligraphers supplies proficiency
        /// </summary>
        public bool CalligraphersSuppliesProficiency
        {
            get { return _toolProficiencyModel.CalligraphersSuppliesProficiency; }
            set
            {
                _toolProficiencyModel.CalligraphersSuppliesProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets carpenters tools proficiency
        /// </summary>
        public bool CarpentersToolsProficiency
        {
            get { return _toolProficiencyModel.CarpentersToolsProficiency; }
            set
            {
                _toolProficiencyModel.CarpentersToolsProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets cartographers tools proficiency
        /// </summary>
        public bool CartographersToolsProficiency
        {
            get { return _toolProficiencyModel.CartographersToolsProficiency; }
            set
            {
                _toolProficiencyModel.CartographersToolsProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets cobblers tools proficiency
        /// </summary>
        public bool CobblersToolsProficiency
        {
            get { return _toolProficiencyModel.CobblersToolsProficiency; }
            set
            {
                _toolProficiencyModel.CobblersToolsProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets cooks utensils proficiency
        /// </summary>
        public bool CooksUtensilsProficiency
        {
            get { return _toolProficiencyModel.CooksUtensilsProficiency; }
            set
            {
                _toolProficiencyModel.CooksUtensilsProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets glassblowers tools proficiency
        /// </summary>
        public bool GlassblowersToolsProficiency
        {
            get { return _toolProficiencyModel.GlassblowersToolsProficiency; }
            set
            {
                _toolProficiencyModel.GlassblowersToolsProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets jewelerss tools proficiency
        /// </summary>
        public bool JewelerssToolsProficiency
        {
            get { return _toolProficiencyModel.JewelerssToolsProficiency; }
            set
            {
                _toolProficiencyModel.JewelerssToolsProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets leatherworkers tools proficiency
        /// </summary>
        public bool LeatherworkersToolsProficiency
        {
            get { return _toolProficiencyModel.LeatherworkersToolsProficiency; }
            set
            {
                _toolProficiencyModel.LeatherworkersToolsProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets masons tools proficiency
        /// </summary>
        public bool MasonsToolsProficiency
        {
            get { return _toolProficiencyModel.MasonsToolsProficiency; }
            set
            {
                _toolProficiencyModel.MasonsToolsProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets painters supplies proficiency
        /// </summary>
        public bool PaintersSuppliesProficiency
        {
            get { return _toolProficiencyModel.PaintersSuppliesProficiency; }
            set
            {
                _toolProficiencyModel.PaintersSuppliesProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets potters tools proficiency
        /// </summary>
        public bool PottersToolsProficiency
        {
            get { return _toolProficiencyModel.PottersToolsProficiency; }
            set
            {
                _toolProficiencyModel.PottersToolsProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets smiths tools proficiency
        /// </summary>
        public bool SmithsToolsProficiency
        {
            get { return _toolProficiencyModel.SmithsToolsProficiency; }
            set
            {
                _toolProficiencyModel.SmithsToolsProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets tinkers tools proficiency
        /// </summary>
        public bool TinkersToolsProficiency
        {
            get { return _toolProficiencyModel.TinkersToolsProficiency; }
            set
            {
                _toolProficiencyModel.TinkersToolsProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets weavers tools proficiency
        /// </summary>
        public bool WeaversToolsProficiency
        {
            get { return _toolProficiencyModel.WeaversToolsProficiency; }
            set
            {
                _toolProficiencyModel.WeaversToolsProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets woodcarvers tools proficiency
        /// </summary>
        public bool WoodcarversToolsProficiency
        {
            get { return _toolProficiencyModel.WoodcarversToolsProficiency; }
            set
            {
                _toolProficiencyModel.WoodcarversToolsProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets dice set proficiency
        /// </summary>
        public bool DiceSetProficiency
        {
            get { return _toolProficiencyModel.DiceSetProficiency; }
            set
            {
                _toolProficiencyModel.DiceSetProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets playing card set proficiency
        /// </summary>
        public bool PlayingCardSetProficiency
        {
            get { return _toolProficiencyModel.PlayingCardSetProficiency; }
            set
            {
                _toolProficiencyModel.PlayingCardSetProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets disguise kit proficiency
        /// </summary>
        public bool DisguiseKitProficiency
        {
            get { return _toolProficiencyModel.DisguiseKitProficiency; }
            set
            {
                _toolProficiencyModel.DisguiseKitProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets forgery kit proficiency
        /// </summary>
        public bool ForgeryKitProficiency
        {
            get { return _toolProficiencyModel.ForgeryKitProficiency; }
            set
            {
                _toolProficiencyModel.ForgeryKitProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets herbalism kit proficiency
        /// </summary>
        public bool HerbalismKitProficiency
        {
            get { return _toolProficiencyModel.HerbalismKitProficiency; }
            set
            {
                _toolProficiencyModel.HerbalismKitProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets poisoners kit proficiency
        /// </summary>
        public bool PoisonersKitProficiency
        {
            get { return _toolProficiencyModel.PoisonersKitProficiency; }
            set
            {
                _toolProficiencyModel.PoisonersKitProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets bagpipes proficiency
        /// </summary>
        public bool BagpipesProficiency
        {
            get { return _toolProficiencyModel.BagpipesProficiency; }
            set
            {
                _toolProficiencyModel.BagpipesProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets drum proficiency
        /// </summary>
        public bool DrumProficiency
        {
            get { return _toolProficiencyModel.DrumProficiency; }
            set
            {
                _toolProficiencyModel.DrumProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets dulcimer proficiency
        /// </summary>
        public bool DulcimerProficiency
        {
            get { return _toolProficiencyModel.DulcimerProficiency; }
            set
            {
                _toolProficiencyModel.DulcimerProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets flute proficiency
        /// </summary>
        public bool FluteProficiency
        {
            get { return _toolProficiencyModel.FluteProficiency; }
            set
            {
                _toolProficiencyModel.FluteProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets lute proficiency
        /// </summary>
        public bool LuteProficiency
        {
            get { return _toolProficiencyModel.LuteProficiency; }
            set
            {
                _toolProficiencyModel.LuteProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets lyre proficiency
        /// </summary>
        public bool LyreProficiency
        {
            get { return _toolProficiencyModel.LyreProficiency; }
            set
            {
                _toolProficiencyModel.LyreProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets horn proficiency
        /// </summary>
        public bool HornProficiency
        {
            get { return _toolProficiencyModel.HornProficiency; }
            set
            {
                _toolProficiencyModel.HornProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets pan flute proficiency
        /// </summary>
        public bool PanFluteProficiency
        {
            get { return _toolProficiencyModel.PanFluteProficiency; }
            set
            {
                _toolProficiencyModel.PanFluteProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets shawm proficiency
        /// </summary>
        public bool ShawmProficiency
        {
            get { return _toolProficiencyModel.ShawmProficiency; }
            set
            {
                _toolProficiencyModel.ShawmProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets viol proficiency
        /// </summary>
        public bool ViolProficiency
        {
            get { return _toolProficiencyModel.ViolProficiency; }
            set
            {
                _toolProficiencyModel.ViolProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets navigators tools proficiency
        /// </summary>
        public bool NavigatorsToolsProficiency
        {
            get { return _toolProficiencyModel.NavigatorsToolsProficiency; }
            set
            {
                _toolProficiencyModel.NavigatorsToolsProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets thieves tools proficiency
        /// </summary>
        public bool ThievesToolsProficiency
        {
            get { return _toolProficiencyModel.ThievesToolsProficiency; }
            set
            {
                _toolProficiencyModel.ThievesToolsProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets land vehicles proficiency
        /// </summary>
        public bool LandVehiclesProficiency
        {
            get { return _toolProficiencyModel.LandVehiclesProficiency; }
            set
            {
                _toolProficiencyModel.LandVehiclesProficiency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets water vehicles proficiency
        /// </summary>
        public bool WaterVehiclesProficiency
        {
            get { return _toolProficiencyModel.WaterVehiclesProficiency; }
            set
            {
                _toolProficiencyModel.WaterVehiclesProficiency = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
