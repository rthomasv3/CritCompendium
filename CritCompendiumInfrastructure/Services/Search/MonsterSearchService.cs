using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services.Search.Input;

namespace CritCompendiumInfrastructure.Services.Search
{
	public sealed class MonsterSearchService
	{
		#region Fields

		private readonly Compendium _compendium;
		private readonly StringService _stringService;
        private readonly StatService _statService;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="MonsterSearchService"/>
        /// </summary>
        public MonsterSearchService(Compendium compendium, StringService stringService, StatService statService)
		{
			_compendium = compendium;
			_stringService = stringService;
            _statService = statService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches the compendium for monsters matching the search input
        /// </summary>
        public List<MonsterModel> Search(MonsterSearchInput searchInput)
		{
			return Sort(_compendium.Monsters.Where(x => SearchInputApplies(searchInput, x)), searchInput.SortOption.Key);
        }

        /// <summary>
        /// True if the search input applies to the model
        /// </summary>
        public bool SearchInputApplies(MonsterSearchInput searchInput, MonsterModel monsterModel)
        {
            return HasSearchText(monsterModel, searchInput.SearchText) &&
                   IsSize(monsterModel, searchInput.Size.Key) &&
                   IsAlignment(monsterModel, searchInput.Alignment.Key) &&
                   IsType(monsterModel, searchInput.Type.Key) &&
                   IsCR(monsterModel, searchInput.CR.Key) &&
                   IsInEnvironment(monsterModel, searchInput.Environment.Key);
        }

        /// <summary>
        /// Sorts using the selected option
        /// </summary>
        public List<MonsterModel> Sort(IEnumerable<MonsterModel> monsters, MonsterSortOption sortOption)
        {
            if (sortOption == MonsterSortOption.Name_Ascending)
            {
                monsters = monsters.OrderBy(x => x.Name);
            }
            else if (sortOption == MonsterSortOption.Name_Descending)
            {
                monsters = monsters.OrderByDescending(x => x.Name);
            }
            else if (sortOption == MonsterSortOption.Size_Ascending)
            {
                monsters = monsters.OrderBy(x => x.Size);
            }
            else if (sortOption == MonsterSortOption.Size_Descending)
            {
                monsters = monsters.OrderByDescending(x => x.Size);
            }
            else if (sortOption == MonsterSortOption.AC_Ascending)
            {
                monsters = monsters.OrderBy(x => ACToInt(x.AC));
            }
            else if (sortOption == MonsterSortOption.AC_Descending)
            {
                monsters = monsters.OrderByDescending(x => ACToInt(x.AC));
            }
            else if (sortOption == MonsterSortOption.Type_Ascending)
            {
                monsters = monsters.OrderBy(x => x.Type);
            }
            else if (sortOption == MonsterSortOption.Type_Descending)
            {
                monsters = monsters.OrderByDescending(x => x.Type);
            }
            else if (sortOption == MonsterSortOption.CR_Ascending)
            {
                monsters = monsters.OrderBy(x => _statService.CRToFloat(x.CR));
            }
            else if (sortOption == MonsterSortOption.CR_Descending)
            {
                monsters = monsters.OrderByDescending(x => _statService.CRToFloat(x.CR));
            }

            return monsters.ToList();
        }

        #endregion

        #region Non-Public Methods

        private bool HasSearchText(MonsterModel monsterModel, string searchText)
		{
			return String.IsNullOrWhiteSpace(searchText) ||
					 monsterModel.Name.ToLower().Contains(searchText.ToLower());
		}

		private bool IsSize(MonsterModel monsterModel, CreatureSize size)
		{
			return size == CreatureSize.None || monsterModel.Size == size;
		}

		private bool IsAlignment(MonsterModel monsterModel, Alignment alignment)
		{
			string alignmentString = _stringService.GetString(alignment).ToLower();
			return alignment == Alignment.None || monsterModel.Alignment.ToLower() == alignmentString;
		}

		private bool IsType(MonsterModel monsterModel, string type)
		{
			return type == null || monsterModel.Type.ToLower() == type.ToLower();
		}

		private bool IsCR(MonsterModel monsterModel, string cr)
		{
			return cr == null || monsterModel.CR.ToLower() == cr.ToLower() || (cr == "-" && String.IsNullOrWhiteSpace(monsterModel.CR));
		}

		private bool IsInEnvironment(MonsterModel monsterModel, string environment)
		{
			return environment == null || (monsterModel.Environment != null && monsterModel.Environment.ToLower().Contains(environment));
		}

        private int ACToInt(string acString)
        {
            int ac = 0;

            if (!int.TryParse(acString, out ac))
            {
                if (acString.Contains(" "))
                {
                    string[] split = acString.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (split.Length > 0)
                    {
                        int.TryParse(split[0], out ac);
                    }
                }
            }

            return ac;
        }

        #endregion
    }
}
