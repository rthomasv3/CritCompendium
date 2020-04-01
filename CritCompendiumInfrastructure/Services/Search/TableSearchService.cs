using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services.Search.Input;

namespace CritCompendiumInfrastructure.Services.Search
{
   public sealed class TableSearchService
   {
      #region Fields

      private readonly Compendium _compendium;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a new instance of <see cref="TableSearchService"/>
      /// </summary>
      public TableSearchService(Compendium compendium)
      {
         _compendium = compendium;
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Searches for random tables matching the search input
      /// </summary>
      public List<RandomTableModel> Search(TableSearchInput searchInput)
      {
         return Sort(_compendium.Tables.Where(x => SearchInputApplies(searchInput, x)), searchInput.SortOption.Key);
      }

      /// <summary>
      /// True if the search input applies to the model
      /// </summary>
      public bool SearchInputApplies(TableSearchInput searchInput, RandomTableModel randomTableModel)
      {
         return HasSearchText(randomTableModel, searchInput.SearchText) &&
                HasTag(randomTableModel, searchInput.Tag.Key);
      }

      /// <summary>
      /// Sorts using the selected option
      /// </summary>
      public List<RandomTableModel> Sort(IEnumerable<RandomTableModel> randomTables, RandomTableSortOption sortOption)
      {
         if (sortOption == RandomTableSortOption.Name_Ascending)
         {
            randomTables = randomTables.OrderBy(x => x.Name);
         }
         else if (sortOption == RandomTableSortOption.Name_Descending)
         {
            randomTables = randomTables.OrderByDescending(x => x.Name);
         }

         return randomTables.ToList();
      }

      #endregion

      #region Non-Public Methods

      private bool HasSearchText(RandomTableModel randomTableModel, string searchText)
      {
         return String.IsNullOrWhiteSpace(searchText) ||
                randomTableModel.Name.ToLower().Contains(searchText.ToLower());
      }

      private bool HasTag(RandomTableModel randomTableModel, string tag)
      {
         return tag == null || randomTableModel.Tags.Any(x => x.Equals(tag, StringComparison.CurrentCultureIgnoreCase));
      }

      #endregion
   }
}
