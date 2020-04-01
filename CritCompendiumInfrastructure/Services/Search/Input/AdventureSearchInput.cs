using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Services.Search.Input
{
    public sealed class AdventureSearchInput
    {
        #region Fields

        private readonly Compendium _compendium;

        private readonly List<KeyValuePair<AdventureSortOption, string>> _sortOptions = new List<KeyValuePair<AdventureSortOption, string>>();
        private List<KeyValuePair<string, string>> _tagOptions = new List<KeyValuePair<string, string>>();

        private string _searchText;
        private bool _sortAndFiltersExpanded;
        private KeyValuePair<AdventureSortOption, string> _sortOption;
        private KeyValuePair<string, string> _tag;

        #endregion

        #region Constructor

        public AdventureSearchInput(Compendium compendium)
        {
            _compendium = compendium;

            foreach (AdventureSortOption sort in Enum.GetValues(typeof(AdventureSortOption)))
            {
                _sortOptions.Add(new KeyValuePair<AdventureSortOption, string>(sort, sort.ToString().Replace("_", " ")));
            }

            UpdateTags();

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
                if (_tag.Key != _tagOptions[0].Key)
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
        public KeyValuePair<AdventureSortOption, string> SortOption
        {
            get { return _sortOption; }
            set { _sortOption = value; }
        }

        /// <summary>
        /// Gets or sets tag
        /// </summary>
        public KeyValuePair<string, string> Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        /// <summary>
        /// Gets sort options
        /// </summary>
        public List<KeyValuePair<AdventureSortOption, string>> SortOptions
        {
            get { return _sortOptions; }
        }

        /// <summary>
        /// Gets tag options
        /// </summary>
        public List<KeyValuePair<string, string>> TagOptions
        {
            get { return _tagOptions; }
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
            _tag = _tagOptions[0];
        }

        /// <summary>
        /// Updates the list of available tags
        /// </summary>
        public void UpdateTags()
        {
            _tagOptions = new List<KeyValuePair<string, string>>();
            foreach (string tag in _compendium.Adventures.SelectMany(x => x.Tags).Distinct())
            {
                _tagOptions.Add(new KeyValuePair<string, string>(tag, tag));
            }
            _tagOptions.Insert(0, new KeyValuePair<string, string>(null, "Any Tag"));
        }

        #endregion
    }
}
