using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business.Search.Input;

namespace CritCompendiumInfrastructure.Business.Search
{
   public sealed class ConditionSearchService
   {
      #region Fields

      private readonly Compendium _compendium;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a new instance of <see cref="ConditionSearchService"/>
      /// </summary>
      public ConditionSearchService(Compendium compendium)
      {
         _compendium = compendium;
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Searches the compendium for conditions matching the search input
      /// </summary>
      public List<ConditionModel> Search(ConditionSearchInput searchInput)
      {
         return Sort(_compendium.Conditions.Where(x => SearchInputApplies(searchInput, x)), searchInput.SortOption);
      }

      /// <summary>
      /// True if the search input applies to the model
      /// </summary>
      public bool SearchInputApplies(ConditionSearchInput searchInput, ConditionModel conditionModel)
      {
         return HasSearchText(conditionModel, searchInput.SearchText);
      }

      /// <summary>
      /// Sorts using the selected option
      /// </summary>
      public List<ConditionModel> Sort(IEnumerable<ConditionModel> conditions, ConditionSortOption sortOption)
      {
         if (sortOption == ConditionSortOption.Name_Ascending)
         {
            conditions = conditions.OrderBy(x => x.Name);
         }
         else if (sortOption == ConditionSortOption.Name_Descending)
         {
            conditions = conditions.OrderByDescending(x => x.Name);
         }

         return conditions.ToList();
      }

      #endregion

      #region Non-Public Methods

      private bool HasSearchText(ConditionModel conditionModel, string searchText)
      {
         return String.IsNullOrWhiteSpace(searchText) ||
                conditionModel.Name.ToLower().Contains(searchText.ToLower());
      }

      #endregion
   }
}
