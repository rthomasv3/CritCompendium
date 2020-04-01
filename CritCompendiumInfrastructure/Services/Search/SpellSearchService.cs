using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services.Search.Input;

namespace CritCompendiumInfrastructure.Services.Search
{
	public sealed class SpellSearchService
	{
		#region Fields

		private readonly Compendium _compendium;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new instance of <see cref="SpellSearchService"/>
		/// </summary>
		public SpellSearchService(Compendium compendium)
		{
			_compendium = compendium;
		}

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches the compendium for spells matching the search input
        /// </summary>
        public List<SpellModel> Search(SpellSearchInput searchInput)
		{
            return Sort(_compendium.Spells.Where(x => SearchInputApplies(searchInput, x)), searchInput.SortOption.Key);
        }

        /// <summary>
        /// True if the search applies to the model
        /// </summary>
        public bool SearchInputApplies(SpellSearchInput searchInput, SpellModel spellModel)
        {
            return HasSearchText(spellModel, searchInput.SearchText) &&
                   IsLevel(spellModel, searchInput.Level.Key) &&
                   IsSchool(spellModel, searchInput.School.Key) &&
                   HasClass(spellModel, searchInput.Class.Key) &&
                   MatchesConcentration(spellModel, searchInput.Concentration.Key) &&
                   MatchesRitual(spellModel, searchInput.Ritual.Key);
        }

        /// <summary>
        /// Sorts using the selected option
        /// </summary>
        public List<SpellModel> Sort(IEnumerable<SpellModel> spells, SpellSortOption sortOption)
        {
            if (sortOption == SpellSortOption.Name_Ascending)
            {
                spells = spells.OrderBy(x => x.Name);
            }
            else if (sortOption == SpellSortOption.Name_Descending)
            {
                spells = spells.OrderByDescending(x => x.Name);
            }
            else if (sortOption == SpellSortOption.Level_Ascending)
            {
                spells = spells.OrderBy(x => x.Level);
            }
            else if (sortOption == SpellSortOption.Level_Descending)
            {
                spells = spells.OrderByDescending(x => x.Level);
            }

            return spells.ToList();
        }

        #endregion

        #region Non-Public Methods

        private bool HasSearchText(SpellModel spellModel, string searchText)
		{
			return String.IsNullOrWhiteSpace(searchText) ||
					 spellModel.Name.ToLower().Contains(searchText.ToLower());
		}

		private bool IsLevel(SpellModel spellModel, int? level)
		{
			return level == null || spellModel.Level == level.Value;
		}

		private bool IsSchool(SpellModel spellModel, SpellSchool school)
		{
			return school == SpellSchool.None || spellModel.SpellSchool == school;
		}

		private bool HasClass(SpellModel spellModel, string classString)
		{
			return classString == null || spellModel.Classes.ToLower().Contains(classString.ToLower());
		}

		private bool MatchesConcentration(SpellModel spellModel, bool? concentration)
		{
			return concentration == null || 
				   (concentration == true && spellModel.Duration != null && spellModel.Duration.ToLower().Contains("concentration")) ||
				   (concentration == false && spellModel.Duration != null && !spellModel.Duration.ToLower().Contains("concentration"));
		}

        private bool MatchesRitual(SpellModel spellModel, bool? ritual)
        {
            return ritual == null || spellModel.IsRitual == ritual.Value;
        }

        #endregion
    }
}
