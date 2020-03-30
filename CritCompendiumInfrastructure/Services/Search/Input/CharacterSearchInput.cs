using System;
using System.Collections.Generic;
using CriticalCompendiumInfrastructure.Enums;

namespace CriticalCompendiumInfrastructure.Services.Search.Input
{
    public sealed class CharacterSearchInput
	{
        #region Fields

        private readonly List<KeyValuePair<CharacterSortOption, string>> _sortOptions = new List<KeyValuePair<CharacterSortOption, string>>();

        private string _searchText;
        private KeyValuePair<CharacterSortOption, string> _sortOption;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="CharacterSearchInput"/>
        /// </summary>
        public CharacterSearchInput()
        {
            foreach (CharacterSortOption sort in Enum.GetValues(typeof(CharacterSortOption)))
            {
                _sortOptions.Add(new KeyValuePair<CharacterSortOption, string>(sort, sort.ToString().Replace("_", " ")));
            }

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
        public KeyValuePair<CharacterSortOption, string> SortOption
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
            _sortOption = _sortOptions[0];
        }

        #endregion
    }
}
