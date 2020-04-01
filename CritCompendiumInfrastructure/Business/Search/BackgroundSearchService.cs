using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business.Search.Input;

namespace CritCompendiumInfrastructure.Business.Search
{
   public sealed class BackgroundSearchService
   {
      #region Fields

      private readonly Compendium _compendium;
      private readonly StringService _stringService;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a new instance of <see cref="BackgroundSearchService"/>
      /// </summary>
      public BackgroundSearchService(Compendium compendium, StringService stringService)
      {
         _compendium = compendium;
         _stringService = stringService;
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Searches the compendium for backgrounds matching the search input
      /// </summary>
      public List<BackgroundModel> Search(BackgroundSearchInput searchInput)
      {
         return Sort(_compendium.Backgrounds.Where(x => SearchInputApplies(searchInput, x)), searchInput.SortOption.Key);
      }

      /// <summary>
      /// Returns true if the search input applies to the model
      /// </summary>
      public bool SearchInputApplies(BackgroundSearchInput searchInput, BackgroundModel backgroundModel)
      {
         return HasSearchText(backgroundModel, searchInput.SearchText) &&
                HasSkill(backgroundModel, searchInput.Skill.Key) &&
                HasTool(backgroundModel, searchInput.Tool.Key) &&
                HasLanguage(backgroundModel, searchInput.Language.Key);
      }

      /// <summary>
      /// Sorts using selected option
      /// </summary>
      public List<BackgroundModel> Sort(IEnumerable<BackgroundModel> backgrounds, BackgroundSortOption sortOption)
      {
         if (sortOption == BackgroundSortOption.Name_Ascending)
         {
            backgrounds = backgrounds.OrderBy(x => x.Name);
         }
         else if (sortOption == BackgroundSortOption.Name_Descending)
         {
            backgrounds = backgrounds.OrderByDescending(x => x.Name);
         }

         return backgrounds.ToList();
      }

      #endregion

      #region Non-Public Methods

      private bool HasSearchText(BackgroundModel backgroundModel, string searchText)
      {
         return String.IsNullOrWhiteSpace(searchText) || backgroundModel.Name.ToLower().Contains(searchText.ToLower());
      }

      private bool HasSkill(BackgroundModel backgroundModel, Skill skill)
      {
         return skill == Skill.None || backgroundModel.Skills.Contains(skill);
      }

      private bool HasTool(BackgroundModel backgroundModel, Enum tool)
      {
         bool hasTool = tool == null;

         if (tool != null)
         {
            string toolString = _stringService.GetString(tool).ToLower();

            if (backgroundModel.ToolsTraitIndex > -1 &&
                backgroundModel.ToolsTraitIndex < backgroundModel.Traits.Count)
            {
               TraitModel trait = backgroundModel.Traits[backgroundModel.ToolsTraitIndex];
               hasTool = trait.Text.ToLower().Contains(toolString);
            }
            else if (backgroundModel.StartingTraitIndex > -1 &&
                     backgroundModel.StartingTraitIndex < backgroundModel.Traits.Count)
            {
               TraitModel trait = backgroundModel.Traits[backgroundModel.StartingTraitIndex];
               hasTool = trait.Text.ToLower().Contains(toolString);
            }
         }

         return hasTool;
      }

      private bool HasLanguage(BackgroundModel backgroundModel, LanguageModel languageModel)
      {
         bool hasLanguage = languageModel == null;

         if (languageModel != null)
         {
            if (backgroundModel.LanguagesTraitIndex > -1 &&
                backgroundModel.LanguagesTraitIndex < backgroundModel.Traits.Count)
            {
               TraitModel trait = backgroundModel.Traits[backgroundModel.LanguagesTraitIndex];
               hasLanguage = trait.Text.ToLower().Contains(languageModel.Name.ToLower());
            }
            else if (backgroundModel.StartingTraitIndex > -1 &&
                     backgroundModel.StartingTraitIndex < backgroundModel.Traits.Count)
            {
               TraitModel trait = backgroundModel.Traits[backgroundModel.StartingTraitIndex];
               hasLanguage = trait.Text.ToLower().Contains(languageModel.Name.ToLower());
            }
         }

         return hasLanguage;
      }

      #endregion
   }
}
