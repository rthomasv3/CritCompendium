using CriticalCompendiumInfrastructure.Enums;
using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Services.Search.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CriticalCompendiumInfrastructure.Services.Search
{
    public sealed class EncounterSearchService
    {
        #region Fields

        private readonly Compendium _compendium;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="EncounterSearchService"/>
        /// </summary>
        public EncounterSearchService(Compendium compendium)
        {
            _compendium = compendium;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches for encounters matching the search input
        /// </summary>
        public List<EncounterModel> Search(EncounterSearchInput searchInput)
        {
            return Sort(_compendium.Encounters.Where(x => SearchInputApplies(searchInput, x)), searchInput.SortOption);
        }

        /// <summary>
        /// True if the search input applies to the model
        /// </summary>
        public bool SearchInputApplies(EncounterSearchInput searchInput, EncounterModel encounterModel)
        {
            return HasSearchText(encounterModel, searchInput.SearchText);
        }

        /// <summary>
        /// Sorts using the selected option
        /// </summary>
        public List<EncounterModel> Sort(IEnumerable<EncounterModel> encounters, EncounterSortOption sortOption)
        {
            if (sortOption == EncounterSortOption.Name_Ascending)
            {
                encounters = encounters.OrderBy(x => x.Name);
            }
            else if (sortOption == EncounterSortOption.Name_Descending)
            {
                encounters = encounters.OrderByDescending(x => x.Name);
            }

            return encounters.ToList();
        }

        #endregion

        #region Non-Public Methods

        private bool HasSearchText(EncounterModel encounterModel, string searchText)
        {
            return String.IsNullOrWhiteSpace(searchText) ||
                   encounterModel.Name.ToLower().Contains(searchText.ToLower());
        }

        #endregion
    }
}
