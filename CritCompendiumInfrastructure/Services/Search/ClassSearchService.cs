using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services.Search.Input;

namespace CritCompendiumInfrastructure.Services.Search
{
	public sealed class ClassSearchService
	{
		#region Fields

		private readonly Compendium _compendium;
		private readonly StringService _stringService;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new instance of <see cref="ClassSearchService"/>
		/// </summary>
		public ClassSearchService(Compendium compendium, StringService stringService)
		{
			_compendium = compendium;
			_stringService = stringService;
		}

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches the compendium for classes matching the search input
        /// </summary>
        public List<ClassModel> Search(ClassSearchInput searchInput)
		{
			return Sort(_compendium.Classes.Where(x => SearchInputApplies(searchInput, x)), searchInput.SortOption.Key);
        }

        /// <summary>
        /// True if the search input applies to the model
        /// </summary>
        public bool SearchInputApplies(ClassSearchInput searchInput, ClassModel classModel)
        {
            return HasSearchText(classModel, searchInput.SearchText) &&
                   HasAbility(classModel, searchInput.Ability.Key) &&
                   HasArmor(classModel, searchInput.Armor.Key) &&
                   HasWeapon(classModel, searchInput.Weapon.Key) &&
                   HasTool(classModel, searchInput.Tool.Key) &&
                   HasSkill(classModel, searchInput.Skill.Key);
        }

        /// <summary>
        /// Sorts classes
        /// </summary>
        public List<ClassModel> Sort(IEnumerable<ClassModel> classes, ClassSortOption sortOption)
        {
            if (sortOption == ClassSortOption.Name_Ascending)
            {
                classes = classes.OrderBy(x => x.Name);
            }
            else if (sortOption == ClassSortOption.Name_Descending)
            {
                classes = classes.OrderByDescending(x => x.Name);
            }
            else if (sortOption == ClassSortOption.Hit_Die_Ascending)
            {
                classes = classes.OrderBy(x => x.HitDie);
            }
            else if (sortOption == ClassSortOption.Hit_Die_Descending)
            {
                classes = classes.OrderByDescending(x => x.HitDie);
            }

            return classes.ToList();
        }

        #endregion

        #region Non-Public Methods

        private bool HasSearchText(ClassModel classModel, string searchText)
		{
			return String.IsNullOrWhiteSpace(searchText) || 
				    classModel.Name.ToLower().Contains(searchText.ToLower());
		}

		private bool HasSkill(ClassModel classModel, Skill skill)
		{
			return skill == Skill.None || 
					 classModel.SkillProficiencies.ToLower().Contains(_stringService.GetString(skill).ToLower());
		}

		private bool HasAbility(ClassModel classModel, Ability ability)
		{
			return ability == Ability.None || classModel.AbilityProficiencies.Contains(ability);
		}

		private bool HasArmor(ClassModel classModel, ArmorType armor)
		{
			string armorString = _stringService.GetString(armor).ToLower();
			return armor == ArmorType.None || classModel.ArmorProficiencies.ToLower().Contains(armorString);
		}

		private bool HasWeapon(ClassModel classModel, WeaponType weapon)
		{
			string weaponString = _stringService.GetString(weapon).ToLower();
			return weapon == WeaponType.None || classModel.WeaponProficiencies.ToLower().Contains(weaponString);
		}

		private bool HasTool(ClassModel classModel, Enum tool)
		{
			bool hasTool = tool == null;

			if (tool != null)
			{
				string toolString = _stringService.GetString(tool).ToLower();
				hasTool = classModel.ToolProficiencies.ToLower().Contains(toolString);
			}

			return hasTool;
		}

		#endregion
	}
}
