using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Services.Search.Input
{
	public sealed class FeatSearchInput
	{
        #region Fields


        private readonly List<KeyValuePair<FeatSortOption, string>> _sortOptions = new List<KeyValuePair<FeatSortOption, string>>();
        private readonly List<KeyValuePair<bool, string>> _prerequisiteOptions = new List<KeyValuePair<bool, string>>();

        private string _searchText;
        private bool _sortAndFiltersExpanded;
        private KeyValuePair<FeatSortOption, string> _sortOption;
        private KeyValuePair<bool, string> _prerequisite;

        #endregion

        #region Constructor

        public FeatSearchInput()
        {
            foreach (FeatSortOption sort in Enum.GetValues(typeof(FeatSortOption)))
            {
                _sortOptions.Add(new KeyValuePair<FeatSortOption, string>(sort, sort.ToString().Replace("_", " ")));
            }

            _prerequisiteOptions.Add(new KeyValuePair<bool, string>(false, "Any Prerequisite"));
            _prerequisiteOptions.Add(new KeyValuePair<bool, string>(true, "None"));

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
        /// Gets applied filter count
        /// </summary>
        public int AppliedFilterCount
        {
            get
            {
                int count = 0;

                if (_sortOption.Key != _sortOptions[0].Key)
                {
                    count++;
                }
                if (_prerequisite.Key != _prerequisiteOptions[0].Key)
                {
                    count++;
                }

                return count;
            }
        }

        /// <summary>
        /// Gets or sets sort and filters expanded
        /// </summary>
        public bool SortAndFiltersExpanded
        {
            get { return _sortAndFiltersExpanded; }
            set { _sortAndFiltersExpanded = value; }
        }

        /// <summary>
        /// Gets or sets sort option
        /// </summary>
        public KeyValuePair<FeatSortOption, string> SortOption
        {
            get { return _sortOption; }
            set { _sortOption = value; }
        }

        /// <summary>
        /// Gets or sets prerequisite
        /// </summary>
        public KeyValuePair<bool, string> Prerequisite
		{
			get { return _prerequisite; }
			set { _prerequisite = value; }
		}
        /// <summary>
        /// Gets sort options
        /// </summary>
        public List<KeyValuePair<FeatSortOption, string>> SortOptions
        {
            get { return _sortOptions; }
        }

        /// <summary>
        /// Gets prerequisite options
        /// </summary>
        public List<KeyValuePair<bool, string>> PrerequisiteOptions
        {
            get { return _prerequisiteOptions; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Resets search, sort, and filter options
        /// </summary>
        public void Reset()
        {
            _searchText = String.Empty;
            _sortOption = _sortOptions[0];
            _prerequisite = _prerequisiteOptions[0];
        }

        #endregion
    }
}
