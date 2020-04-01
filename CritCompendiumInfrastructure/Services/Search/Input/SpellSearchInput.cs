using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;

namespace CritCompendiumInfrastructure.Services.Search.Input
{
   public sealed class SpellSearchInput
   {
      #region Fields

      private readonly Compendium _compendium = DependencyResolver.Resolve<Compendium>();
      private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();

      private readonly List<KeyValuePair<SpellSortOption, string>> _sortOptions = new List<KeyValuePair<SpellSortOption, string>>();
      private readonly List<KeyValuePair<int?, string>> _levels = new List<KeyValuePair<int?, string>>();
      private readonly List<KeyValuePair<SpellSchool, string>> _schools = new List<KeyValuePair<SpellSchool, string>>();
      private readonly List<KeyValuePair<string, string>> _classes = new List<KeyValuePair<string, string>>();
      private readonly List<KeyValuePair<bool?, string>> _concentrationOptions = new List<KeyValuePair<bool?, string>>();
      private readonly List<KeyValuePair<bool?, string>> _ritualOptions = new List<KeyValuePair<bool?, string>>();

      private string _searchText;
      private bool _sortAndFiltersExpanded;
      private KeyValuePair<SpellSortOption, string> _sortOption;
      private KeyValuePair<int?, string> _level;
      private KeyValuePair<SpellSchool, string> _school;
      private KeyValuePair<String, string> _class;
      private KeyValuePair<bool?, string> _concentration;
      private KeyValuePair<bool?, string> _ritual;

      #endregion

      #region Constructor

      public SpellSearchInput()
      {
         foreach (SpellSortOption sort in Enum.GetValues(typeof(SpellSortOption)))
         {
            _sortOptions.Add(new KeyValuePair<SpellSortOption, string>(sort, sort.ToString().Replace("_", " ")));
         }

         _levels.Add(new KeyValuePair<int?, string>(null, "Any Level"));
         _levels.Add(new KeyValuePair<int?, string>(-1, "Unknown"));
         _levels.Add(new KeyValuePair<int?, string>(0, "Cantrip"));
         _levels.Add(new KeyValuePair<int?, string>(1, "Level 1"));
         _levels.Add(new KeyValuePair<int?, string>(2, "Level 2"));
         _levels.Add(new KeyValuePair<int?, string>(3, "Level 3"));
         _levels.Add(new KeyValuePair<int?, string>(4, "Level 4"));
         _levels.Add(new KeyValuePair<int?, string>(5, "Level 5"));
         _levels.Add(new KeyValuePair<int?, string>(6, "Level 6"));
         _levels.Add(new KeyValuePair<int?, string>(7, "Level 7"));
         _levels.Add(new KeyValuePair<int?, string>(8, "Level 8"));
         _levels.Add(new KeyValuePair<int?, string>(9, "Level 9"));

         _schools.Add(new KeyValuePair<SpellSchool, string>(SpellSchool.None, "Any School"));
         foreach (SpellSchool school in Enum.GetValues(typeof(SpellSchool)))
         {
            if (school != SpellSchool.None)
            {
               _schools.Add(new KeyValuePair<SpellSchool, string>(school, _stringService.GetString(school)));
            }
         }

         foreach (ClassModel classModel in _compendium.Classes)
         {
            _classes.Add(new KeyValuePair<string, string>(classModel.Name, classModel.Name));
         }
         _classes.Insert(0, new KeyValuePair<string, string>(null, "Any Class"));

         _concentrationOptions.Add(new KeyValuePair<bool?, string>(null, "Any Concentration"));
         _concentrationOptions.Add(new KeyValuePair<bool?, string>(false, "No"));
         _concentrationOptions.Add(new KeyValuePair<bool?, string>(true, "Yes"));

         _ritualOptions.Add(new KeyValuePair<bool?, string>(null, "Any Ritual"));
         _ritualOptions.Add(new KeyValuePair<bool?, string>(false, "No"));
         _ritualOptions.Add(new KeyValuePair<bool?, string>(true, "Yes"));

         Reset();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets searchText
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
            if (_level.Key != _levels[0].Key)
            {
               count++;
            }
            if (_school.Key != _schools[0].Key)
            {
               count++;
            }
            if (_class.Key != _classes[0].Key)
            {
               count++;
            }
            if (_concentration.Key != _concentrationOptions[0].Key)
            {
               count++;
            }
            if (_ritual.Key != _ritualOptions[0].Key)
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
      /// Gets or sets sort option
      /// </summary>
      public KeyValuePair<SpellSortOption, string> SortOption
      {
         get { return _sortOption; }
         set { _sortOption = value; }
      }

      /// <summary>
      /// Gets or sets level
      /// </summary>
      public KeyValuePair<int?, string> Level
      {
         get { return _level; }
         set { _level = value; }
      }

      /// <summary>
      /// Gets or sets school
      /// </summary>
      public KeyValuePair<SpellSchool, string> School
      {
         get { return _school; }
         set { _school = value; }
      }

      /// <summary>
      /// Gets or sets class
      /// </summary>
      public KeyValuePair<string, string> Class
      {
         get { return _class; }
         set { _class = value; }
      }

      /// <summary>
      /// Gets or sets concentration
      /// </summary>
      public KeyValuePair<bool?, string> Concentration
      {
         get { return _concentration; }
         set { _concentration = value; }
      }

      /// <summary>
      /// Gets or sets ritual
      /// </summary>
      public KeyValuePair<bool?, string> Ritual
      {
         get { return _ritual; }
         set { _ritual = value; }
      }

      /// <summary>
      /// Gets sort options
      /// </summary>
      public List<KeyValuePair<SpellSortOption, string>> SortOptions
      {
         get { return _sortOptions; }
      }

      /// <summary>
      /// Gets levels
      /// </summary>
      public List<KeyValuePair<int?, string>> Levels
      {
         get { return _levels; }
      }

      /// <summary>
      /// Gets schools
      /// </summary>
      public List<KeyValuePair<SpellSchool, string>> Schools
      {
         get { return _schools; }
      }

      /// <summary>
      /// Gets classes
      /// </summary>
      public List<KeyValuePair<string, string>> Classes
      {
         get { return _classes; }
      }

      /// <summary>
      /// Gets concentration options
      /// </summary>
      public List<KeyValuePair<bool?, string>> ConcentrationOptions
      {
         get { return _concentrationOptions; }
      }

      /// <summary>
      /// Gets ritual options
      /// </summary>
      public List<KeyValuePair<bool?, string>> RitualOptions
      {
         get { return _ritualOptions; }
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
         _level = _levels[0];
         _school = _schools[0];
         _class = _classes[0];
         _concentration = _concentrationOptions[0];
         _ritual = _ritualOptions[0];
      }

      #endregion
   }
}
