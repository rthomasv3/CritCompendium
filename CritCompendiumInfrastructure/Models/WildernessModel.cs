using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Models
{
    public sealed class WildernessModel : LocationModel
    {
        #region Fields

        private string _landmarks;
        private string _environment;
        private string _weather;
        private string _foodAndWater;
        private string _hazards;
        private List<SettlementModel> _settlements = new List<SettlementModel>();
        private List<DungeonModel> _dungeons = new List<DungeonModel>();
        private List<BuildingModel> _buildings = new List<BuildingModel>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="WildernessModel"/>
        /// </summary>
        public WildernessModel() : base()
        {
        }

        /// <summary>
        /// Creates a copy of <see cref="WildernessModel"/>
        /// </summary>
        public WildernessModel(WildernessModel wildernessModel) : base(wildernessModel)
        {
            _landmarks = wildernessModel.Landmarks;
            _environment = wildernessModel._environment;
            _weather = wildernessModel.Weather;
            _foodAndWater = wildernessModel._foodAndWater;
            _hazards = wildernessModel._hazards;

            _settlements = new List<SettlementModel>();
            foreach (SettlementModel settlementModel in wildernessModel.Settlements)
            {
                _settlements.Add(new SettlementModel(settlementModel));
            }

            _dungeons = new List<DungeonModel>();
            foreach (DungeonModel dungeonModel in wildernessModel.Dungeons)
            {
                _dungeons.Add(new DungeonModel(dungeonModel));
            }

            _buildings = new List<BuildingModel>();
            foreach (BuildingModel buildingModel in wildernessModel.Buildings)
            {
                _buildings.Add(new BuildingModel(buildingModel));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets landmarks
        /// </summary>
        public string Landmarks
        {
            get { return _landmarks; }
            set { _landmarks = value; }
        }

        /// <summary>
        /// Gets or sets environment
        /// </summary>
        public string Environment
        {
            get { return _environment; }
            set { _environment = value; }
        }

        /// <summary>
        /// Gets or sets weather
        /// </summary>
        public string Weather
        {
            get { return _weather; }
            set { _weather = value; }
        }

        /// <summary>
        /// Gets or sets food and water
        /// </summary>
        public string FoodAndWater
        {
            get { return _foodAndWater; }
            set { _foodAndWater = value; }
        }

        /// <summary>
        /// Gets or sets hazards
        /// </summary>
        public string Hazards
        {
            get { return _hazards; }
            set { _hazards = value; }
        }

        /// <summary>
        /// Gets or sets settlements
        /// </summary>
        public List<SettlementModel> Settlements
        {
            get { return _settlements; }
            set { _settlements = value; }
        }

        /// <summary>
        /// Gets or sets dungeons
        /// </summary>
        public List<DungeonModel> Dungeons
        {
            get { return _dungeons; }
            set { _dungeons = value; }
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
