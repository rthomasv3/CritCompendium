using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;

namespace CritCompendiumInfrastructure.Services.Search.Input
{
   public sealed class MonsterSearchInput
   {
      #region Fields

      private readonly Compendium _compendium;
      private readonly StringService _stringService;
      private readonly StatService _statService;

      private readonly List<KeyValuePair<MonsterSortOption, string>> _sortOptions = new List<KeyValuePair<MonsterSortOption, string>>();
      private readonly List<KeyValuePair<CreatureSize, string>> _sizes = new List<KeyValuePair<CreatureSize, string>>();
      private readonly List<KeyValuePair<Alignment, string>> _alignments = new List<KeyValuePair<Alignment, string>>();
      private readonly List<KeyValuePair<string, string>> _types = new List<KeyValuePair<string, string>>();
      private readonly List<KeyValuePair<string, string>> _crs = new List<KeyValuePair<string, string>>();
      private readonly List<KeyValuePair<string, string>> _environments = new List<KeyValuePair<string, string>>();

      private string _searchText;
      private bool _sortAndFiltersExpanded;
      private KeyValuePair<MonsterSortOption, string> _sortOption;
      private KeyValuePair<CreatureSize, string> _size;
      private KeyValuePair<Alignment, string> _alignment;
      private KeyValuePair<string, string> _type;
      private KeyValuePair<string, string> _cr;
      private KeyValuePair<string, string> _environment;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="MonsterSearchInput"/>
      /// </summary>
      public MonsterSearchInput(Compendium compendium, StringService stringService, StatService statService)
      {
         _compendium = compendium;
         _stringService = stringService;
         _statService = statService;

         foreach (MonsterSortOption sort in Enum.GetValues(typeof(MonsterSortOption)))
         {
            _sortOptions.Add(new KeyValuePair<MonsterSortOption, string>(sort, sort.ToString().Replace("_", " ")));
         }

         _sizes.Add(new KeyValuePair<CreatureSize, string>(CreatureSize.None, "Any Size"));
         foreach (CreatureSize size in Enum.GetValues(typeof(CreatureSize)))
         {
            if (size != CreatureSize.None)
            {
               _sizes.Add(new KeyValuePair<CreatureSize, string>(size, _stringService.GetString(size)));
            }
         }

         _alignments.Add(new KeyValuePair<Alignment, string>(Enums.Alignment.None, "Any Alignment"));
         foreach (Alignment alignment in Enum.GetValues(typeof(Alignment)))
         {
            if (alignment != Enums.Alignment.None)
            {
               _alignments.Add(new KeyValuePair<Alignment, string>(alignment, _stringService.GetString(alignment)));
            }
         }

         foreach (MonsterModel monster in _compendium.Monsters)
         {
            if (!_types.Any(x => x.Value.ToLower() == monster.Type.ToLower()))
            {
               _types.Add(new KeyValuePair<string, string>(monster.Type, _stringService.CapitalizeWords(monster.Type)));
            }

            int number = 0;
            if (!_crs.Any(x => x.Value == monster.CR) && (monster.CR.Contains("/") || Int32.TryParse(monster.CR, out number)))
            {
               _crs.Add(new KeyValuePair<string, string>(monster.CR, monster.CR));
            }

            if (!String.IsNullOrWhiteSpace(monster.Environment))
            {
               foreach (string environmentSplit in monster.Environment.Split(new char[] { ',' }))
               {
                  string environment = environmentSplit.Trim();
                  if (!_environments.Any(x => x.Value.ToLower() == environment.ToLower()))
                  {
                     _environments.Add(new KeyValuePair<string, string>(environment, _stringService.CapitalizeWords(environment)));
                  }
               }
            }
         }

         _types = _types.OrderBy(x => x.Value).ToList();
         _types.Insert(0, new KeyValuePair<string, string>(null, "Any Type"));

         _crs = _crs.OrderBy(x => _statService.CRToFloat(x.Value)).ToList();
         _crs.Insert(0, new KeyValuePair<string, string>("-", "Unknown"));
         _crs.Insert(0, new KeyValuePair<string, string>(null, "Any CR"));

         _environments = _environments.OrderBy(x => x.Value).ToList();
         _environments.Insert(0, new KeyValuePair<string, string>(null, "Any Environment"));

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
            if (_size.Key != _sizes[0].Key)
            {
               count++;
            }
            if (_alignment.Key != _alignments[0].Key)
            {
               count++;
            }
            if (_type.Key != _types[0].Key)
            {
               count++;
            }
            if (_cr.Key != _crs[0].Key)
            {
               count++;
            }
            if (_environment.Key != _environments[0].Key)
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
      public KeyValuePair<MonsterSortOption, string> SortOption
      {
         get { return _sortOption; }
         set { _sortOption = value; }
      }

      /// <summary>
      /// Gets or sets size
      /// </summary>
      public KeyValuePair<CreatureSize, string> Size
      {
         get { return _size; }
         set { _size = value; }
      }

      /// <summary>
      /// Gets or sets alignment
      /// </summary>
      public KeyValuePair<Alignment, string> Alignment
      {
         get { return _alignment; }
         set { _alignment = value; }
      }

      /// <summary>
      /// Gets or sets type
      /// </summary>
      public KeyValuePair<string, string> Type
      {
         get { return _type; }
         set { _type = value; }
      }

      /// <summary>
      /// Gets or sets cr
      /// </summary>
      public KeyValuePair<string, string> CR
      {
         get { return _cr; }
         set { _cr = value; }
      }

      /// <summary>
      /// Gets or sets environment
      /// </summary>
      public KeyValuePair<string, string> Environment
      {
         get { return _environment; }
         set { _environment = value; }
      }

      /// <summary>
      /// Gets sort options
      /// </summary>
      public List<KeyValuePair<MonsterSortOption, string>> SortOptions
      {
         get { return _sortOptions; }
      }

      /// <summary>
      /// List of sizes
      /// </summary>
      public List<KeyValuePair<CreatureSize, string>> Sizes
      {
         get { return _sizes; }
      }

      /// <summary>
      /// List of alignments
      /// </summary>
      public List<KeyValuePair<Alignment, string>> Alignments
      {
         get { return _alignments; }
      }

      /// <summary>
      /// List of types
      /// </summary>
      public List<KeyValuePair<string, string>> Types
      {
         get { return _types; }
      }

      /// <summary>
      /// List of crs
      /// </summary>
      public List<KeyValuePair<string, string>> CRs
      {
         get { return _crs; }
      }

      /// <summary>
      /// List of environments
      /// </summary>
      public List<KeyValuePair<string, string>> Environments
      {
         get { return _environments; }
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
         _size = _sizes[0];
         _alignment = _alignments[0];
         _type = _types[0];
         _cr = _crs[0];
         _environment = _environments[0];
      }

      #endregion
   }
}
