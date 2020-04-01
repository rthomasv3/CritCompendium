using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;

namespace CritCompendiumInfrastructure.Services.Search.Input
{
	public sealed class RaceSearchInput
	{
        #region Fields

        private readonly Compendium _compendium;
        private readonly StringService _stringService;

        private readonly List<KeyValuePair<RaceSortOption, string>> _sortOptions = new List<KeyValuePair<RaceSortOption, string>>();
        private readonly List<KeyValuePair<CreatureSize, string>> _sizes = new List<KeyValuePair<CreatureSize, string>>();
        private readonly List<KeyValuePair<Ability, string>> _abilities = new List<KeyValuePair<Ability, string>>();
        private readonly List<KeyValuePair<LanguageModel, string>> _languages = new List<KeyValuePair<LanguageModel, string>>();

        private string _searchText;
        private bool _sortAndFiltersExpanded;
        private KeyValuePair<RaceSortOption, string> _sortOption;
        private KeyValuePair<CreatureSize, string> _size;
		private KeyValuePair<Ability, string> _ability;
		private KeyValuePair<LanguageModel, string> _language;

        #endregion

        #region Constructor

        public RaceSearchInput(Compendium compendium, StringService stringService)
        {
            _compendium = compendium;
            _stringService = stringService;

            foreach (RaceSortOption sort in Enum.GetValues(typeof(RaceSortOption)))
            {
                _sortOptions.Add(new KeyValuePair<RaceSortOption, string>(sort, sort.ToString().Replace("_", " ")));
            }

            _sizes.Add(new KeyValuePair<CreatureSize, string>(CreatureSize.None, "Any Size"));
            foreach (CreatureSize size in Enum.GetValues(typeof(CreatureSize)))
            {
                if (size != CreatureSize.None)
                {
                    _sizes.Add(new KeyValuePair<CreatureSize, string>(size, _stringService.GetString(size)));
                }
            }

            _abilities.Add(new KeyValuePair<Ability, string>(Enums.Ability.None, "Any Ability"));
            foreach (Ability ability in Enum.GetValues(typeof(Ability)))
            {
                if (ability != Enums.Ability.None)
                {
                    _abilities.Add(new KeyValuePair<Ability, string>(ability, _stringService.GetString(ability)));
                }
            }

            _languages.Add(new KeyValuePair<LanguageModel, string>(null, "Any Language"));
            foreach (LanguageModel languageModel in _compendium.Languages)
            {
                _languages.Add(new KeyValuePair<LanguageModel, string>(languageModel, languageModel.Name));
            }

            Reset();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets searchText
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
                if (_size.Key != _sizes[0].Key)
                {
                    count++;
                }
                if (_ability.Key != _abilities[0].Key)
                {
                    count++;
                }
                if (_language.Key != _languages[0].Key)
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
        public KeyValuePair<RaceSortOption, string> SortOption
        {
            get { return _sortOption; }
            set { _sortOption = value; }
        }

        /// <summary>
        /// Gets or sets size
        /// </summary>
        public KeyValuePair<CreatureSize, string> Size
		{
			get { return _size; }
			set { _size = value; }
		}

        /// <summary>
        /// Gets or sets ability
        /// </summary>
        public KeyValuePair<Ability, string> Ability
		{
			get { return _ability; }
			set { _ability = value; }
		}

		/// <summary>
		/// Gets or sets language
		/// </summary>
		public KeyValuePair<LanguageModel, string> Language
		{
			get { return _language; }
			set { _language = value; }
		}
        /// <summary>
        /// Gets sort options
        /// </summary>
        public List<KeyValuePair<RaceSortOption, string>> SortOptions
        {
            get { return _sortOptions; }
        }

        /// <summary>
        /// Gets sizes
        /// </summary>
        public List<KeyValuePair<CreatureSize, string>> Sizes
        {
            get { return _sizes; }
        }

        /// <summary>
        /// Gets abilities
        /// </summary>
        public List<KeyValuePair<Ability, string>> Abilities
        {
            get { return _abilities; }
        }

        /// <summary>
        /// Gets languages
        /// </summary>
        public List<KeyValuePair<LanguageModel, string>> Languages
        {
            get { return _languages; }
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
            _size = _sizes[0];
            _ability = _abilities[0];
            _language = _languages[0];
        }

        #endregion
    }
}
