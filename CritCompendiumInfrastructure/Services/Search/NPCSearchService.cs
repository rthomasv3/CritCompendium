using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services.Search.Input;

namespace CritCompendiumInfrastructure.Services.Search
{
    public sealed class NPCSearchService
    {
        #region Fields

        private readonly Compendium _compendium;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="NPCSearchService"/>
        /// </summary>
        public NPCSearchService(Compendium compendium)
        {
            _compendium = compendium;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches the compendium for npcs matching the search input
        /// </summary>
        public List<NPCModel> Search(NPCSearchInput searchInput)
        {
            return Sort(_compendium.NPCs.Where(x => SearchInputApplies(searchInput, x)), searchInput.SortOption.Key);
        }

        /// <summary>
        /// True if the search input applies to the model
        /// </summary>
        public bool SearchInputApplies(NPCSearchInput searchInput, NPCModel npcModel)
        {
            return HasSearchText(npcModel, searchInput.SearchText) &&
                   HasTag(npcModel, searchInput.Tag.Key);
        }

        /// <summary>
        /// Sorts using selected option
        /// </summary>
        public List<NPCModel> Sort(IEnumerable<NPCModel> npcs, NPCSortOption sortOption)
        {
            if (sortOption == NPCSortOption.Name_Ascending)
            {
                npcs = npcs.OrderBy(x => x.Name);
            }
            else if (sortOption == NPCSortOption.Name_Descending)
            {
                npcs = npcs.OrderByDescending(x => x.Name);
            }

            return npcs.ToList();
        }

        #endregion

        #region Non-Public Methods

        private bool HasSearchText(NPCModel npcModel, string searchText)
        {
            return String.IsNullOrWhiteSpace(searchText) ||
                   npcModel.Name.ToLower().Contains(searchText.ToLower());
        }

        private bool HasTag(NPCModel npcModel, string tag)
        {
            return tag == null || npcModel.Tags.Any(x => x.Equals(tag, StringComparison.CurrentCultureIgnoreCase));
        }

        #endregion
    }
}
