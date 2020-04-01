using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;

namespace CritCompendiumInfrastructure.Services.Search.Input
{
    public sealed class BackgroundSearchInput
	{
        #region Fields

        private readonly Compendium _compendium;
        private readonly StringService _stringService;

        private readonly List<KeyValuePair<BackgroundSortOption, string>> _sortOptions = new List<KeyValuePair<BackgroundSortOption, string>>();
        private readonly List<KeyValuePair<Skill, string>> _skills = new List<KeyValuePair<Skill, string>>();
        private readonly List<KeyValuePair<Enum, string>> _tools = new List<KeyValuePair<Enum, string>>();
        private readonly List<KeyValuePair<LanguageModel, string>> _languages = new List<KeyValuePair<LanguageModel, string>>();

        private string _searchText;
        private bool _sortAndFiltersExpanded;
        private KeyValuePair<BackgroundSortOption, string> _sortOption;
        private KeyValuePair<Skill, string> _skill;
		private KeyValuePair<Enum, string> _tool;
		private KeyValuePair<LanguageModel, string> _language;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="BackgroundSearchInput"/>
        /// </summary>
        public BackgroundSearchInput(Compendium compendium, StringService stringService)
        {
            _compendium = compendium;
            _stringService = stringService;

            foreach (BackgroundSortOption sort in Enum.GetValues(typeof(BackgroundSortOption)))
            {
                _sortOptions.Add(new KeyValuePair<BackgroundSortOption, string>(sort, sort.ToString().Replace("_", " ")));
            }

            _skills.Add(new KeyValuePair<Skill, string>(Enums.Skill.None, "Any Skill"));
            foreach (Skill skill in Enum.GetValues(typeof(Skill)))
            {
                if (skill != Enums.Skill.None)
                {
                    _skills.Add(new KeyValuePair<Skill, string>(skill, _stringService.GetString(skill)));
                }
            }

            foreach (ArtisanTool tool in Enum.GetValues(typeof(ArtisanTool)))
            {
                if (tool != ArtisanTool.None)
                {
                    _tools.Add(new KeyValuePair<Enum, string>(tool, _stringService.GetString(tool)));
                }
            }
            foreach (GamingSet game in Enum.GetValues(typeof(GamingSet)))
            {
                if (game != GamingSet.None)
                {
                    _tools.Add(new KeyValuePair<Enum, string>(game, _stringService.GetString(game)));
                }
            }
            foreach (Kit kit in Enum.GetValues(typeof(Kit)))
            {
                if (kit != Kit.None)
                {
                    _tools.Add(new KeyValuePair<Enum, string>(kit, _stringService.GetString(kit)));
                }
            }
            foreach (MusicalInstrument instrument in Enum.GetValues(typeof(MusicalInstrument)))
            {
                if (instrument != MusicalInstrument.None)
                {
                    _tools.Add(new KeyValuePair<Enum, string>(instrument, _stringService.GetString(instrument)));
                }
            }
            foreach (Tool tool in Enum.GetValues(typeof(Tool)))
            {
                if (tool != Enums.Tool.None)
                {
                    _tools.Add(new KeyValuePair<Enum, string>(tool, _stringService.GetString(tool)));
                }
            }
            foreach (Vehicle vehicle in Enum.GetValues(typeof(Vehicle)))
            {
                if (vehicle != Vehicle.None)
                {
                    _tools.Add(new KeyValuePair<Enum, string>(vehicle, _stringService.GetString(vehicle)));
                }
            }
            _tools = _tools.OrderBy(x => x.Value).ToList();
            _tools.Insert(0, new KeyValuePair<Enum, string>(null, "Any Tool"));

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
                if (_skill.Key != _skills[0].Key)
                {
                    count++;
                }
                if (_tool.Key != _tools[0].Key)
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
        public KeyValuePair<BackgroundSortOption, string> SortOption
        {
            get { return _sortOption; }
            set { _sortOption = value; }
        }

        /// <summary>
        /// Gets or sets skill
        /// </summary>
        public KeyValuePair<Skill, string> Skill
		{
			get { return _skill; }
			set { _skill = value; }
		}

        /// <summary>
        /// Gets or sets tool
        /// </summary>
        public KeyValuePair<Enum, string> Tool
		{
			get { return _tool; }
			set { _tool = value; }
		}

        /// <summary>
        /// Gets or sets language model
        /// </summary>
        public KeyValuePair<LanguageModel, string> Language
		{
			get { return _language; }
			set { _language = value; }
		}

        /// <summary>
        /// Gets sort sptions
        /// </summary>
        public List<KeyValuePair<BackgroundSortOption, string>> SortOptions
        {
            get { return _sortOptions; }
        }

        /// <summary>
        /// Gets skills
        /// </summary>
        public List<KeyValuePair<Skill, string>> Skills
        {
            get { return _skills; }
        }

        /// <summary>
        /// Gets tools
        /// </summary>
        public List<KeyValuePair<Enum, string>> Tools
        {
            get { return _tools; }
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
            _skill = _skills[0];
            _tool = _tools[0];
            _language = _languages[0];
        }

        #endregion
    }
}
