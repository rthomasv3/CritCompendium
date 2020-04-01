using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Services.Search.Input
{
	public sealed class ItemSearchInput
	{
        #region Fields

        private readonly StringService _stringService;

        private readonly List<KeyValuePair<ItemSortOption, string>> _sortOptions = new List<KeyValuePair<ItemSortOption, string>>();
        private readonly List<KeyValuePair<ItemType, string>> _types = new List<KeyValuePair<ItemType, string>>();
        private readonly List<KeyValuePair<bool?, string>> _magicOptions = new List<KeyValuePair<bool?, string>>();
        private readonly List<KeyValuePair<Rarity, string>> _rarities = new List<KeyValuePair<Rarity, string>>();
        private readonly List<KeyValuePair<bool?, string>> _attunementOptions = new List<KeyValuePair<bool?, string>>();

        private string _searchText;
        private bool _sortAndFiltersExpanded;
        private KeyValuePair<ItemSortOption, string> _selectedSortOption;
        private KeyValuePair<ItemType, string> _itemType;
		private KeyValuePair<bool?, string> _magic;
		private KeyValuePair<Rarity, string> _rarity;
		private KeyValuePair<bool?, string> _requresAttunement;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="ItemSearchInput"/>
        /// </summary>
        public ItemSearchInput(StringService stringService)
        {
            _stringService = stringService;

            foreach (ItemSortOption sort in Enum.GetValues(typeof(ItemSortOption)))
            {
                _sortOptions.Add(new KeyValuePair<ItemSortOption, string>(sort, sort.ToString().Replace("_", " ")));
            }

            _types.Add(new KeyValuePair<ItemType, string>(Enums.ItemType.None, "Any Item Type"));
            foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
            {
                if (itemType != Enums.ItemType.None)
                {
                    _types.Add(new KeyValuePair<ItemType, string>(itemType, _stringService.GetString(itemType)));
                }
            }

            _magicOptions.Add(new KeyValuePair<bool?, string>(null, "Any Magic"));
            _magicOptions.Add(new KeyValuePair<bool?, string>(true, "Magic"));
            _magicOptions.Add(new KeyValuePair<bool?, string>(false, "Non-Magic"));

            _rarities.Add(new KeyValuePair<Rarity, string>(Enums.Rarity.None, "Any Rarity"));
            foreach (Rarity rarity in Enum.GetValues(typeof(Rarity)))
            {
                if (rarity != Enums.Rarity.None)
                {
                    _rarities.Add(new KeyValuePair<Rarity, string>(rarity, _stringService.GetString(rarity)));
                }
            }

            _attunementOptions.Add(new KeyValuePair<bool?, string>(null, "Any Attunement"));
            _attunementOptions.Add(new KeyValuePair<bool?, string>(true, "Yes"));
            _attunementOptions.Add(new KeyValuePair<bool?, string>(false, "No"));

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

                if (_selectedSortOption.Key != _sortOptions[0].Key)
                {
                    count++;
                }
                if (_itemType.Key != _types[0].Key)
                {
                    count++;
                }
                if (_magic.Key != _magicOptions[0].Key)
                {
                    count++;
                }
                if (_rarity.Key != _rarities[0].Key)
                {
                    count++;
                }
                if (_requresAttunement.Key != _attunementOptions[0].Key)
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
        public KeyValuePair<ItemSortOption, string> SortOption
        {
            get { return _selectedSortOption; }
            set { _selectedSortOption = value; }
        }

        /// <summary>
        /// Gets or sets itemType
        /// </summary>
        public KeyValuePair<ItemType, string> ItemType
		{
			get { return _itemType; }
			set { _itemType = value; }
		}

		/// <summary>
		/// Gets or sets magic
		/// </summary>
		public KeyValuePair<bool?, string> Magic
		{
			get { return _magic; }
			set { _magic = value; }
		}

		/// <summary>
		/// Gets or sets rarity
		/// </summary>
		public KeyValuePair<Rarity, string> Rarity
		{
			get { return _rarity; }
			set { _rarity = value; }
		}

		/// <summary>
		/// Gets or sets requresAttunement
		/// </summary>
		public KeyValuePair<bool?, string> RequresAttunement
		{
			get { return _requresAttunement; }
			set { _requresAttunement = value; }
		}

        /// <summary>
        /// Gets sort options
        /// </summary>
        public List<KeyValuePair<ItemSortOption, string>> SortOptions
        {
            get { return _sortOptions; }
        }

        /// <summary>
        /// Gets types
        /// </summary>
        public List<KeyValuePair<ItemType, string>> Types
        {
            get { return _types; }
        }

        /// <summary>
        /// Gets magic options
        /// </summary>
        public List<KeyValuePair<bool?, string>> MagicOptions
        {
            get { return _magicOptions; }
        }

        /// <summary>
        /// Gets rarities
        /// </summary>
        public List<KeyValuePair<Rarity, string>> Rarities
        {
            get { return _rarities; }
        }

        /// <summary>
        /// Gets attunement options
        /// </summary>
        public List<KeyValuePair<bool?, string>> AttunementOptions
        {
            get { return _attunementOptions; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Resets search input
        /// </summary>
        public void Reset()
        {
            _searchText = String.Empty;
            _selectedSortOption = _sortOptions[0];
            _itemType = _types[0];
            _magic = _magicOptions[0];
            _rarity = _rarities[0];
            _requresAttunement = _attunementOptions[0];
        }

        #endregion
    }
}
