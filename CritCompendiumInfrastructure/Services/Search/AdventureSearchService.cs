using System;
using System.Collections.Generic;
using System.Linq;
using CriticalCompendiumInfrastructure.Enums;
using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Services.Search.Input;

namespace CriticalCompendiumInfrastructure.Services.Search
{
    public sealed class AdventureSearchService
    {
        #region Fields

        private readonly Compendium _compendium;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="AdventureSearchService"/>
        /// </summary>
        public AdventureSearchService(Compendium compendium)
        {
            _compendium = compendium;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches the compendium for adventures matching the search input
        /// </summary>
        public List<AdventureModel> Search(AdventureSearchInput searchInput)
        {
            return Sort(_compendium.Adventures.Where(x => SearchInputApplies(searchInput, x)), searchInput.SortOption.Key);
        }

        /// <summary>
        /// True if the search input applies to the model
        /// </summary>
        public bool SearchInputApplies(AdventureSearchInput searchInput, AdventureModel adventureModel)
        {
            return HasSearchText(adventureModel, searchInput.SearchText) &&
                   HasTag(adventureModel, searchInput.Tag.Key);
        }

        /// <summary>
        /// Sorts using selected option
        /// </summary>
        public List<AdventureModel> Sort(IEnumerable<AdventureModel> adventures, AdventureSortOption sortOption)
        {
            if (sortOption == AdventureSortOption.Name_Ascending)
            {
                adventures = adventures.OrderBy(x => x.Name);
            }
            else if (sortOption == AdventureSortOption.Name_Descending)
            {
                adventures = adventures.OrderByDescending(x => x.Name);
            }

            return adventures.ToList();
        }

        #endregion

        #region Non-Public Methods

        private bool HasSearchText(AdventureModel adventureModel, string searchText)
        {
            return String.IsNullOrWhiteSpace(searchText) ||
                   adventureModel.Name.ToLower().Contains(searchText.ToLower());
        }

        private bool HasTag(AdventureModel adventureModel, string tag)
        {
            return tag == null || adventureModel.Tags.Any(x => x.Equals(tag, StringComparison.CurrentCultureIgnoreCase));
        }

        #endregion
    }
}
