using System;
using System.Collections.Generic;
using System.Linq;
using CriticalCompendiumInfrastructure.Enums;

namespace CriticalCompendiumInfrastructure.Services.Search.Input
{
    public sealed class NPCSearchInput
    {
        #region Fields

        private readonly Compendium _compendium;

        private readonly List<KeyValuePair<NPCSortOption, string>> _sortOptions = new List<KeyValuePair<NPCSortOption, string>>();
        private List<KeyValuePair<string, string>> _tagOptions = new List<KeyValuePair<string, string>>();

        private string _searchText;
        private bool _sortAndFiltersExpanded;
        private KeyValuePair<NPCSortOption, string> _sortOption;
        private KeyValuePair<string, string> _tag;

        #endregion

        #region Constructor

        public NPCSearchInput(Compendium compendium)
        {
            _compendium = compendium;

            foreach (NPCSortOption sort in Enum.GetValues(typeof(NPCSortOption)))
            {
                _sortOptions.Add(new KeyValuePair<NPCSortOption, string>(sort, sort.ToString().Replace("_", " ")));
            }

            UpdateTags();
            _tag = _tagOptions[0];

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
        public KeyValuePair<NPCSortOption, string> SortOption
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
        public List<KeyValuePair<NPCSortOption, string>> SortOptions
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
            foreach (string tag in _compendium.NPCs.SelectMany(x => x.Tags).Distinct())
            {
                _tagOptions.Add(new KeyValuePair<string, string>(tag, tag));
            }
            _tagOptions.Insert(0, new KeyValuePair<string, string>(null, "Any Tag"));
        }

        #endregion
    }
}
