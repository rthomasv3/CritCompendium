using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business.Search.Input;

namespace CritCompendiumInfrastructure.Business.Search
{
   public sealed class RaceSearchService
   {
      #region Fields

      private readonly Compendium _compendium;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a new instance of <see cref="RaceSearchService"/>
      /// </summary>
      public RaceSearchService(Compendium compendium)
      {
         _compendium = compendium;
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Searches the compendium for races matching the search input
      /// </summary>
      public List<RaceModel> Search(RaceSearchInput searchInput)
      {
         return Sort(_compendium.Races.Where(x => SearchInputApplies(searchInput, x)), searchInput.SortOption.Key);
      }

      /// <summary>
      /// True if the search input applies to the model
      /// </summary>
      public bool SearchInputApplies(RaceSearchInput searchInput, RaceModel raceModel)
      {
         return HasSearchText(raceModel, searchInput.SearchText) &&
                IsSize(raceModel, searchInput.Size.Key) &&
                HasAbility(raceModel, searchInput.Ability.Key) &&
                HasLanguage(raceModel, searchInput.Language.Key);
      }

      /// <summary>
      /// Sorts using the selection option
      /// </summary>
      public List<RaceModel> Sort(IEnumerable<RaceModel> races, RaceSortOption sortOption)
      {
         if (sortOption == RaceSortOption.Name_Ascending)
         {
            races = races.OrderBy(x => x.Name);
         }
         else if (sortOption == RaceSortOption.Name_Descending)
         {
            races = races.OrderByDescending(x => x.Name);
         }

         return races.ToList();
      }

      #endregion

      #region Non-Public Methods

      private bool HasSearchText(RaceModel raceModel, string searchText)
      {
         return String.IsNullOrWhiteSpace(searchText) ||
                raceModel.Name.ToLower().Contains(searchText.ToLower());
      }

      private bool IsSize(RaceModel raceModel, CreatureSize size)
      {
         return size == CreatureSize.None || raceModel.Size == size;
      }

      private bool HasAbility(RaceModel raceModel, Ability ability)
      {
         return ability == Ability.None || raceModel.Abilities.Keys.Contains(ability);
      }

      private bool HasLanguage(RaceModel raceModel, LanguageModel languageModel)
      {
         bool hasLanguage = languageModel == null;

         if (languageModel != null)
         {
            foreach (string language in raceModel.Languages)
            {
               if (language.ToLower() == languageModel.Name.ToLower())
               {
                  hasLanguage = true;
                  break;
               }
            }
         }

         return hasLanguage;
      }

      #endregion
   }
}
