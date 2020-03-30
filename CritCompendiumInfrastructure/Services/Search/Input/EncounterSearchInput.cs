using System;
using CriticalCompendiumInfrastructure.Enums;

namespace CriticalCompendiumInfrastructure.Services.Search.Input
{
    public sealed class EncounterSearchInput
    {
        #region Fields

        private string _searchText;
        private EncounterSortOption _sortOption;

        #endregion

        #region Constructor

        public EncounterSearchInput()
        {
            Reset();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets search text
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; }
        }

        /// <summary>
        /// Gets or sets sort option
        /// </summary>
        public EncounterSortOption SortOption
        {
            get { return _sortOption; }
            set { _sortOption = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Resets search, sort, and filter options
        /// </summary>
        public void Reset()
        {
            _searchText = String.Empty;
            _sortOption = EncounterSortOption.Name_Ascending;
        }

        #endregion
    }
}
