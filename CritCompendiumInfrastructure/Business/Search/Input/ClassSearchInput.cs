using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Business.Search.Input
{
   public sealed class ClassSearchInput
   {
      #region Fields

      private readonly StringService _stringService;

      private readonly List<KeyValuePair<ClassSortOption, string>> _sortOptions = new List<KeyValuePair<ClassSortOption, string>>();
      private readonly List<KeyValuePair<Ability, string>> _abilities = new List<KeyValuePair<Ability, string>>();
      private readonly List<KeyValuePair<ArmorType, string>> _armors = new List<KeyValuePair<ArmorType, string>>();
      private readonly List<KeyValuePair<WeaponType, string>> _weapons = new List<KeyValuePair<WeaponType, string>>();
      private readonly List<KeyValuePair<Enum, string>> _tools = new List<KeyValuePair<Enum, string>>();
      private readonly List<KeyValuePair<Skill, string>> _skills = new List<KeyValuePair<Skill, string>>();

      private string _searchText;
      private bool _sortAndFiltersExpanded;
      private KeyValuePair<ClassSortOption, string> _sortOption;
      private KeyValuePair<Ability, string> _ability;
      private KeyValuePair<ArmorType, string> _armor;
      private KeyValuePair<WeaponType, string> _weapon;
      private KeyValuePair<Enum, string> _tool;
      private KeyValuePair<Skill, string> _skill;

      #endregion

      #region Constructor

      public ClassSearchInput(StringService stringService)
      {
         _stringService = stringService;

         foreach (ClassSortOption sort in Enum.GetValues(typeof(ClassSortOption)))
         {
            _sortOptions.Add(new KeyValuePair<ClassSortOption, string>(sort, sort.ToString().Replace("_", " ")));
         }

         _abilities.Add(new KeyValuePair<Ability, string>(Enums.Ability.None, "Any Ability"));
         foreach (Ability ability in Enum.GetValues(typeof(Ability)))
         {
            if (ability != Enums.Ability.None)
            {
               _abilities.Add(new KeyValuePair<Ability, string>(ability, _stringService.GetString(ability)));
            }
         }

         _armors.Add(new KeyValuePair<ArmorType, string>(ArmorType.None, "Any Armor"));
         foreach (ArmorType armor in Enum.GetValues(typeof(ArmorType)))
         {
            if (armor != ArmorType.None)
            {
               _armors.Add(new KeyValuePair<ArmorType, string>(armor, _stringService.GetString(armor)));
            }
         }

         _weapons.Add(new KeyValuePair<WeaponType, string>(WeaponType.None, "Any Weapon"));
         foreach (WeaponType weapon in Enum.GetValues(typeof(WeaponType)))
         {
            if (weapon != WeaponType.None)
            {
               _weapons.Add(new KeyValuePair<WeaponType, string>(weapon, _stringService.GetString(weapon)));
            }
         }

         foreach (ArtisanTool tool in Enum.GetValues(typeof(ArtisanTool)))
         {
            if (tool != ArtisanTool.None)
            {
               _tools.Add(new KeyValuePair<Enum, string>(tool, _stringService.GetString(tool)));
            }
         }
         foreach (GamingSet game in Enum.GetValues(typeof(GamingSet)))
         {
            if (game != GamingSet.None)
            {
               _tools.Add(new KeyValuePair<Enum, string>(game, _stringService.GetString(game)));
            }
         }
         foreach (Kit kit in Enum.GetValues(typeof(Kit)))
         {
            if (kit != Kit.None)
            {
               _tools.Add(new KeyValuePair<Enum, string>(kit, _stringService.GetString(kit)));
            }
         }
         foreach (MusicalInstrument instrument in Enum.GetValues(typeof(MusicalInstrument)))
         {
            if (instrument != MusicalInstrument.None)
            {
               _tools.Add(new KeyValuePair<Enum, string>(instrument, _stringService.GetString(instrument)));
            }
         }
         foreach (Tool tool in Enum.GetValues(typeof(Tool)))
         {
            if (tool != Enums.Tool.None)
            {
               _tools.Add(new KeyValuePair<Enum, string>(tool, _stringService.GetString(tool)));
            }
         }
         foreach (Vehicle vehicle in Enum.GetValues(typeof(Vehicle)))
         {
            if (vehicle != Vehicle.None)
            {
               _tools.Add(new KeyValuePair<Enum, string>(vehicle, _stringService.GetString(vehicle)));
            }
         }
         _tools = _tools.OrderBy(x => x.Value).ToList();
         _tools.Insert(0, new KeyValuePair<Enum, string>(null, "Any Tool"));

         _skills.Add(new KeyValuePair<Skill, string>(Enums.Skill.None, "Any Skill"));
         foreach (Skill skill in Enum.GetValues(typeof(Skill)))
         {
            if (skill != Enums.Skill.None)
            {
               _skills.Add(new KeyValuePair<Skill, string>(skill, _stringService.GetString(skill)));
            }
         }

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
            if (_ability.Key != _abilities[0].Key)
            {
               count++;
            }
            if (_armor.Key != _armors[0].Key)
            {
               count++;
            }
            if (_weapon.Key != _weapons[0].Key)
            {
               count++;
            }
            if (_tool.Key != _tools[0].Key)
            {
               count++;
            }
            if (_skill.Key != _skills[0].Key)
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
      public KeyValuePair<ClassSortOption, string> SortOption
      {
         get { return _sortOption; }
         set { _sortOption = value; }
      }

      /// <summary>
      /// Gets or sets ability
      /// </summary>
      public KeyValuePair<Ability, string> Ability
      {
         get { return _ability; }
         set { _ability = value; }
      }

      /// <summary>
      /// Gets or sets armor
      /// </summary>
      public KeyValuePair<ArmorType, string> Armor
      {
         get { return _armor; }
         set { _armor = value; }
      }

      /// <summary>
      /// Gets or sets weapon
      /// </summary>
      public KeyValuePair<WeaponType, string> Weapon
      {
         get { return _weapon; }
         set { _weapon = value; }
      }

      /// <summary>
      /// Gets or sets tool
      /// </summary>
      public KeyValuePair<Enum, string> Tool
      {
         get { return _tool; }
         set { _tool = value; }
      }

      /// <summary>
      /// Gets or sets skill
      /// </summary>
      public KeyValuePair<Skill, string> Skill
      {
         get { return _skill; }
         set { _skill = value; }
      }

      /// <summary>
      /// Gets sort options
      /// </summary>
      public List<KeyValuePair<ClassSortOption, string>> SortOptions
      {
         get { return _sortOptions; }
      }

      /// <summary>
      /// Gets abilities
      /// </summary>
      public List<KeyValuePair<Ability, string>> Abilities
      {
         get { return _abilities; }
      }

      /// <summary>
      /// Gets armors
      /// </summary>
      public List<KeyValuePair<ArmorType, string>> Armors
      {
         get { return _armors; }
      }

      /// <summary>
      /// Gets weapons
      /// </summary>
      public List<KeyValuePair<WeaponType, string>> Weapons
      {
         get { return _weapons; }
      }

      /// <summary>
      /// Gets tools
      /// </summary>
      public List<KeyValuePair<Enum, string>> Tools
      {
         get { return _tools; }
      }

      /// <summary>
      /// Gets skills
      /// </summary>
      public List<KeyValuePair<Skill, string>> Skills
      {
         get { return _skills; }
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
         _ability = _abilities[0];
         _armor = _armors[0];
         _weapon = _weapons[0];
         _tool = _tools[0];
         _skill = _skills[0];
      }

      #endregion
   }
}
