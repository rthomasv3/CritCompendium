using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Business.Search.Input
{
   public sealed class ConditionSearchInput
   {
      #region Fields

      private string _searchText;
      private ConditionSortOption _sortOption;

      #endregion

      #region Constructor

      public ConditionSearchInput()
      {
         Reset();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets search text
      /// </summary>
      public string SearchText
      {
         get { return _searchText; }
         set { _searchText = value; }
      }

      /// <summary>
      /// Gets or sets sort option
      /// </summary>
      public ConditionSortOption SortOption
      {
         get { return _sortOption; }
         set { _sortOption = value; }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Resets search, sort, and filter options
      /// </summary>
      public void Reset()
      {
         _searchText = String.Empty;
         _sortOption = ConditionSortOption.Name_Ascending;
      }

      #endregion
   }
}
