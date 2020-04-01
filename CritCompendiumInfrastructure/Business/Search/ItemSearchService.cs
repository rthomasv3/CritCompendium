using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business.Search.Input;

namespace CritCompendiumInfrastructure.Business.Search
{
   public sealed class ItemSearchService
   {
      #region Fields

      private readonly Compendium _compendium;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a new instance of <see cref="ItemSearchService"/>
      /// </summary>
      public ItemSearchService(Compendium compendium)
      {
         _compendium = compendium;
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Searches the compendium for items matching the search input
      /// </summary>
      public List<ItemModel> Search(ItemSearchInput searchInput)
      {
         return Sort(_compendium.Items.Where(x => SearchInputApplies(searchInput, x)), searchInput.SortOption.Key);
      }

      /// <summary>
      /// True if the search input applies to the model
      /// </summary>
      public bool SearchInputApplies(ItemSearchInput searchInput, ItemModel itemModel)
      {
         return HasSearchText(itemModel, searchInput.SearchText) &&
                IsItemType(itemModel, searchInput.ItemType.Key) &&
                IsMagicSelection(itemModel, searchInput.Magic.Key) &&
                IsRarity(itemModel, searchInput.Rarity.Key) &&
                IsAttunementSelection(itemModel, searchInput.RequresAttunement.Key);
      }

      /// <summary>
      /// Sorts using the selected option
      /// </summary>
      public List<ItemModel> Sort(IEnumerable<ItemModel> items, ItemSortOption sortOption)
      {
         if (sortOption == ItemSortOption.Name_Ascending)
         {
            items = items.OrderBy(x => x.Name);
         }
         else if (sortOption == ItemSortOption.Name_Descending)
         {
            items = items.OrderByDescending(x => x.Name);
         }
         else if (sortOption == ItemSortOption.Type_Ascending)
         {
            items = items.OrderBy(x => x.Type);
         }
         else if (sortOption == ItemSortOption.Type_Descending)
         {
            items = items.OrderByDescending(x => x.Type);
         }
         else if (sortOption == ItemSortOption.Value_Ascending)
         {
            items = items.OrderBy(x => { float.TryParse(x.Value, out float v); return v; });
         }
         else if (sortOption == ItemSortOption.Value_Descending)
         {
            items = items.OrderByDescending(x => { float.TryParse(x.Value, out float v); return v; });
         }
         else if (sortOption == ItemSortOption.Weight_Ascending)
         {
            items = items.OrderBy(x => { int.TryParse(x.Weight, out int w); return w; });
         }
         else if (sortOption == ItemSortOption.Weight_Descending)
         {
            items = items.OrderByDescending(x => { int.TryParse(x.Weight, out int w); return w; });
         }

         return items.ToList();
      }

      #endregion

      #region Non-Public Methods

      private bool HasSearchText(ItemModel itemModel, string searchText)
      {
         return String.IsNullOrWhiteSpace(searchText) ||
                itemModel.Name.ToLower().Contains(searchText.ToLower());
      }

      private bool IsItemType(ItemModel itemModel, ItemType itemType)
      {
         return itemType == ItemType.None || itemModel.Type == itemType;
      }

      private bool IsMagicSelection(ItemModel itemModel, bool? magic)
      {
         return magic == null || itemModel.Magic == magic.Value;
      }

      private bool IsRarity(ItemModel itemModel, Rarity rarity)
      {
         return rarity == Rarity.None || itemModel.Rarity == rarity;
      }

      private bool IsAttunementSelection(ItemModel itemModel, bool? requresAttunement)
      {
         return requresAttunement == null || itemModel.RequiresAttunement == requresAttunement.Value;
      }

      #endregion
   }
}
