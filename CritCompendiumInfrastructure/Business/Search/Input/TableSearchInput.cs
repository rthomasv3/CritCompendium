using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Business.Search.Input
{
   public sealed class TableSearchInput
   {
      #region Fields

      private readonly Compendium _compendium;

      private readonly List<KeyValuePair<RandomTableSortOption, string>> _sortOptions = new List<KeyValuePair<RandomTableSortOption, string>>();
      private ObservableCollection<KeyValuePair<string, string>> _tagOptions = new ObservableCollection<KeyValuePair<string, string>>();

      private string _searchText;
      private bool _sortAndFiltersExpanded;
      private KeyValuePair<RandomTableSortOption, string> _sortOption;
      private KeyValuePair<string, string> _tag;

      #endregion

      #region Constructor

      public TableSearchInput(Compendium compendium)
      {
         _compendium = compendium;

         foreach (RandomTableSortOption sort in Enum.GetValues(typeof(RandomTableSortOption)))
         {
            _sortOptions.Add(new KeyValuePair<RandomTableSortOption, string>(sort, sort.ToString().Replace("_", " ")));
         }

         UpdateTags();
         _tag = _tagOptions[0];

         Reset();

         _compendium.TagsChanged += _compendium_TagsChanged;
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
      /// Gets applied filter count
      /// </summary>
      public int AppliedFilterCount
      {
         get
         {
            int count = 0;

            if (_sortOption.Key != _sortOptions[0].Key)
            {
               count++;
            }
            if (_tag.Key != _tagOptions[0].Key)
            {
               count++;
            }

            return count;
         }
      }

      /// <summary>
      /// Gets or sets sort and filters expanded
      /// </summary>
      public bool SortAndFiltersExpanded
      {
         get { return _sortAndFiltersExpanded; }
         set { _sortAndFiltersExpanded = value; }
      }

      /// <summary>
      /// Gets sort options
      /// </summary>
      public IEnumerable<KeyValuePair<RandomTableSortOption, string>> SortOptions
      {
         get { return _sortOptions; }
      }

      /// <summary>
      /// Gets or sets sort option
      /// </summary>
      public KeyValuePair<RandomTableSortOption, string> SortOption
      {
         get { return _sortOption; }
         set { _sortOption = value; }
      }

      /// <summary>
      /// Gets tag options
      /// </summary>
      public IEnumerable<KeyValuePair<string, string>> TagOptions
      {
         get { return _tagOptions; }
      }

      /// <summary>
      /// Gets or sets tag
      /// </summary>
      public KeyValuePair<string, string> Tag
      {
         get { return _tag; }
         set { _tag = value; }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Resets search, sort, and filter options
      /// </summary>
      public void Reset()
      {
         _searchText = String.Empty;
         _sortOption = _sortOptions[0];
      }

      #endregion

      #region Private Methods

      private void UpdateTags()
      {
         _tag = default(KeyValuePair<string, string>);
         _tagOptions.Clear();
         _tagOptions.Add(new KeyValuePair<string, string>(null, "Any Tag"));
         foreach (string tag in _compendium.Tags)
         {
            KeyValuePair<string, string> tagOption = new KeyValuePair<string, string>(tag, tag);
            _tagOptions.Add(tagOption);
            if (tagOption.Key == _tag.Key)
            {
               _tag = tagOption;
            }
         }
         if (_tag.Equals(default(KeyValuePair<string, string>)))
         {
            _tag = _tagOptions[0];
         }
      }

      private void _compendium_TagsChanged(object sender, EventArgs e)
      {
         UpdateTags();
      }

      #endregion
   }
}
