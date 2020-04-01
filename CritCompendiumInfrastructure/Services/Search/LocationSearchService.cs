using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services.Search.Input;

namespace CritCompendiumInfrastructure.Services.Search
{
   public sealed class LocationSearchService
   {
      #region Fields

      private readonly Compendium _compendium;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a new instance of <see cref="LocationSearchService"/>
      /// </summary>
      public LocationSearchService(Compendium compendium)
      {
         _compendium = compendium;
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Searches the compendium for locations matching the search input
      /// </summary>
      public List<LocationModel> Search(LocationSearchInput searchInput)
      {
         return Sort(_compendium.Locations.Where(x => SearchInputApplies(searchInput, x)), searchInput.SortOption.Key);
      }

      /// <summary>
      /// True if the search input applies to the model
      /// </summary>
      public bool SearchInputApplies(LocationSearchInput searchInput, LocationModel locationModel)
      {
         return HasSearchText(locationModel, searchInput.SearchText) &&
                HasTag(locationModel, searchInput.Tag.Key);
      }

      /// <summary>
      /// Sorts using selected option
      /// </summary>
      public List<LocationModel> Sort(IEnumerable<LocationModel> locations, LocationSortOption sortOption)
      {
         if (sortOption == LocationSortOption.Name_Ascending)
         {
            locations = locations.OrderBy(x => x.Name);
         }
         else if (sortOption == LocationSortOption.Name_Descending)
         {
            locations = locations.OrderByDescending(x => x.Name);
         }

         return locations.ToList();
      }

      #endregion

      #region Non-Public Methods

      private bool HasSearchText(LocationModel locationModel, string searchText)
      {
         return String.IsNullOrWhiteSpace(searchText) ||
                locationModel.Name.ToLower().Contains(searchText.ToLower());
      }

      private bool HasTag(LocationModel locationModel, string tag)
      {
         return tag == null || locationModel.Tags.Any(x => x.Equals(tag, StringComparison.CurrentCultureIgnoreCase));
      }

      #endregion
   }
}
