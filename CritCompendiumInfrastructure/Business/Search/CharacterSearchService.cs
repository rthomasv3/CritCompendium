using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business.Search.Input;

namespace CritCompendiumInfrastructure.Business.Search
{
   public sealed class CharacterSearchService
   {
      #region Fields

      private readonly Compendium _compendium;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a new instance of <see cref="CharacterSearchService"/>
      /// </summary>
      public CharacterSearchService(Compendium compendium)
      {
         _compendium = compendium;
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Searches for characteres matching the search input
      /// </summary>
      public List<CharacterModel> Search(CharacterSearchInput searchInput)
      {
         return Sort(_compendium.Characters.Where(x => SearchInputApplies(searchInput, x)), searchInput.SortOption.Key);
      }

      /// <summary>
      /// True if the search input applies to the model
      /// </summary>
      public bool SearchInputApplies(CharacterSearchInput searchInput, CharacterModel characterModel)
      {
         return HasSearchText(characterModel, searchInput.SearchText);
      }

      /// <summary>
      /// Sorts using selected option
      /// </summary>
      public List<CharacterModel> Sort(IEnumerable<CharacterModel> characters, CharacterSortOption sortOption)
      {
         if (sortOption == CharacterSortOption.Name_Ascending)
         {
            characters = characters.OrderBy(x => x.Name);
         }
         else if (sortOption == CharacterSortOption.Name_Descending)
         {
            characters = characters.OrderByDescending(x => x.Name);
         }

         return characters.ToList();
      }

      #endregion

      #region Non-Public Methods

      private bool HasSearchText(CharacterModel characterModel, string searchText)
      {
         return String.IsNullOrWhiteSpace(searchText) ||
               characterModel.Name.ToLower().Contains(searchText.ToLower());
      }

      #endregion
   }
}
