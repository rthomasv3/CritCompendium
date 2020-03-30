using System;
using System.Collections.Generic;
using System.Linq;
using CriticalCompendiumInfrastructure.Enums;
using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Services.Search.Input;

namespace CriticalCompendiumInfrastructure.Services.Search
{
    public sealed class FeatSearchService
	{
		#region Fields

		private readonly Compendium _compendium;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new instance of <see cref="FeatSearchService"/>
		/// </summary>
		public FeatSearchService(Compendium compendium)
		{
			_compendium = compendium;
		}

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches the compendium for feats matching the search input
        /// </summary>
        public List<FeatModel> Search(FeatSearchInput searchInput)
		{
			return Sort(_compendium.Feats.Where(x => SearchInputApplies(searchInput, x)), searchInput.SortOption.Key);
		}
        
        /// <summary>
        /// True if the search input applies to the model
        /// </summary>
        public bool SearchInputApplies(FeatSearchInput searchInput, FeatModel featModel)
        {
            return HasSearchText(featModel, searchInput.SearchText) &&
                   PrerequisiteMatches(featModel, searchInput.Prerequisite.Key);
        }

        /// <summary>
        /// Sorts using selected option
        /// </summary>
        public List<FeatModel> Sort(IEnumerable<FeatModel> feats, FeatSortOption sortOption)
        {
            if (sortOption == FeatSortOption.Name_Ascending)
            {
                feats = feats.OrderBy(x => x.Name);
            }
            else if (sortOption == FeatSortOption.Name_Descending)
            {
                feats = feats.OrderByDescending(x => x.Name);
            }

            return feats.ToList();
        }

        #endregion

        #region Non-Public Methods

        private bool HasSearchText(FeatModel featModel, string searchText)
		{
			return String.IsNullOrWhiteSpace(searchText) ||
				   featModel.Name.ToLower().Contains(searchText.ToLower());
		}

		private bool PrerequisiteMatches(FeatModel featModel, bool prerequisite)
		{
			return prerequisite == false || String.IsNullOrWhiteSpace(featModel.Prerequisite);
		}

		#endregion
	}
}
