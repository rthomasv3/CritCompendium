using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Models
{
    public sealed class SettlementModel : LocationModel
    {
        #region Fields

        private string _rulerNotes;
        private string _traits;
        private string _knownFor;
        private string _conflicts;
        private List<BuildingModel> _buildings = new List<BuildingModel>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="SettlementModel"/>
        /// </summary>
        public SettlementModel() : base()
        {

        }

        /// <summary>
		/// Creates a copy of <see cref="SettlementModel"/>
		/// </summary>
        public SettlementModel(SettlementModel settlementModel) : base(settlementModel)
        {
            _rulerNotes = settlementModel.RulerNotes;
            _traits = settlementModel.Traits;
            _knownFor = settlementModel.KnownFor;
            _conflicts = settlementModel.Conflicts;

            _buildings = new List<BuildingModel>();
            foreach (BuildingModel buildingModel in settlementModel.Buildings)
            {
                _buildings.Add(new BuildingModel(buildingModel));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets ruler notes
        /// </summary>
        public string RulerNotes
        {
            get { return _rulerNotes; }
            set { _rulerNotes = value; }
        }

        /// <summary>
        /// Gets or sets traits
        /// </summary>
        public string Traits
        {
            get { return _traits; }
            set { _traits = value; }
        }

        /// <summary>
        /// Gets or sets known for
        /// </summary>
        public string KnownFor
        {
            get { return _knownFor; }
            set { _knownFor = value; }
        }

        /// <summary>
        /// Gets or sets conflicts
        /// </summary>
        public string Conflicts
        {
            get { return _conflicts; }
            set { _conflicts = value; }
        }

        /// <summary>
        /// Gets or sets buildings
        /// </summary>
        public List<BuildingModel> Buildings
        {
            get { return _buildings; }
            set { _buildings = value; }
        }

        #endregion
    }
}
