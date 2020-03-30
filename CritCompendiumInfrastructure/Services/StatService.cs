using System;
using System.Collections.Generic;
using System.Linq;
using CriticalCompendiumInfrastructure.Enums;
using CriticalCompendiumInfrastructure.Models;

namespace CriticalCompendiumInfrastructure.Services
{
    public sealed class StatService
	{
        #region Fields

        private readonly StringService _stringService;

        private readonly List<Tuple<string, Ability>> _abilities = new List<Tuple<string, Ability>>();
        private readonly List<Tuple<string, Skill>> _skills = new List<Tuple<string, Skill>>();
        private readonly Dictionary<Skill, Ability> _skillAbilityMap = new Dictionary<Skill, Ability>();
        private readonly List<int> _xpByLevel = new List<int>(){ 0, 300, 900, 2700, 6500, 14000, 23000, 34000,
                                                                 48000, 64000, 85000, 100000, 120000, 140000,
                                                                 165000, 195000, 225000, 265000, 305000, 355000, };
        private readonly List<KeyValuePair<DamageType, string>> _damageTypes = new List<KeyValuePair<DamageType, string>>();
        private readonly List<string> _dice = new List<string>() { "d2", "d4", "d6", "d8", "d10", "d12", "d20", "d100" };
        private readonly Dictionary<int, int> _baseXPThresholds = new Dictionary<int, int>()
        {
            {1, 25 }, {2, 50 }, {3, 75 }, {4, 125 }, {5, 250 }, {6, 300 }, {7, 350 }, {8, 450 }, {9, 550 }, {10, 600 }, {11, 800 },
            { 12, 1000 }, {13, 1100 }, {14, 1250 }, {15, 1400 }, {16, 1600 }, {17, 2000 }, {18, 2100 }, {19, 2400 }, {20, 2800 },
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a instance of <see cref="AbilityManager"/>
        /// </summary>
        public StatService(StringService stringService)
        {
            _stringService = stringService;

            foreach (Ability ability in Enum.GetValues(typeof(Ability)))
            {
                if (ability != Ability.None)
                {
                    _abilities.Add(new Tuple<string, Ability>(_stringService.GetString(ability), ability));
                }
            }

            foreach (Skill skill in Enum.GetValues(typeof(Skill)))
            {
                if (skill != Skill.None)
                {
                    _skills.Add(new Tuple<string, Skill>(_stringService.GetString(skill), skill));
                }
            }

            _skillAbilityMap.Add(Skill.Acrobatics, Ability.Dexterity);
            _skillAbilityMap.Add(Skill.Animal_Handling, Ability.Wisdom);
            _skillAbilityMap.Add(Skill.Arcana, Ability.Intelligence);
            _skillAbilityMap.Add(Skill.Athletics, Ability.Strength);
            _skillAbilityMap.Add(Skill.Deception, Ability.Charisma);
            _skillAbilityMap.Add(Skill.History, Ability.Intelligence);
            _skillAbilityMap.Add(Skill.Insight, Ability.Wisdom);
            _skillAbilityMap.Add(Skill.Intimidation, Ability.Charisma);
            _skillAbilityMap.Add(Skill.Investigation, Ability.Intelligence);
            _skillAbilityMap.Add(Skill.Medicine, Ability.Wisdom);
            _skillAbilityMap.Add(Skill.Nature, Ability.Intelligence);
            _skillAbilityMap.Add(Skill.Perception, Ability.Wisdom);
            _skillAbilityMap.Add(Skill.Performance, Ability.Charisma);
            _skillAbilityMap.Add(Skill.Persuasion, Ability.Charisma);
            _skillAbilityMap.Add(Skill.Religion, Ability.Intelligence);
            _skillAbilityMap.Add(Skill.Sleight_of_Hand, Ability.Dexterity);
            _skillAbilityMap.Add(Skill.Stealth, Ability.Dexterity);
            _skillAbilityMap.Add(Skill.Survival, Ability.Wisdom);

            foreach (DamageType damageType in Enum.GetValues(typeof(DamageType)))
            {
                if (damageType != DamageType.None)
                {
                    _damageTypes.Add(new KeyValuePair<DamageType, string>(damageType, _stringService.GetString(damageType)));
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets list of abilities
        /// </summary>
        public IEnumerable<Tuple<string, Ability>> Abilities
        {
            get { return _abilities; }
        }

        /// <summary>
        /// Gets list of skills
        /// </summary>
        public IEnumerable<Tuple<string, Skill>> Skills
        {
            get { return _skills; }
        }

        /// <summary>
        /// Gets damage types
        /// </summary>
        public List<KeyValuePair<DamageType, string>> DamageTypes
        {
            get { return _damageTypes; }
        }

        /// <summary>
        /// Gets dice
        /// </summary>
        public List<string> Dice
        {
            get { return _dice; }
        }

        /// <summary>
        /// Gets base xp thresholds
        /// </summary>
        public Dictionary<int, int> BaseXPThresholds
        {
            get { return _baseXPThresholds; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the bonus based on the stat score
        /// </summary>
        public int GetStatBonus(int value)
		{
			return (int)Math.Floor((float)(value - 10) / 2f);
		}

		/// <summary>
		/// Gets the bonus based on the stat score as a string
		/// </summary>
		public string GetStatBonusString(int value)
		{
			return AddPlusOrMinus(GetStatBonus(value));
		}

		/// <summary>
		/// String representation of the value with a plus or minus sign
		/// </summary>
		public string AddPlusOrMinus(int value)
		{
			return value >= 0 ? "+" + value.ToString() : value.ToString();
		}

        /// <summary>
        /// Converts cr to float
        /// </summary>
        public float CRToFloat(string crString)
        {
            float cr = 0.0f;

            if (!float.TryParse(crString, out cr) && crString.Contains("/"))
            {
                string[] parts = crString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    float first = 0, second = 0;
                    if (float.TryParse(parts[0], out first) && float.TryParse(parts[1], out second))
                    {
                        cr = first / second;
                    }
                }
            }

            return cr;
        }

        /// <summary>
        /// Gets the rquired xp for level
        /// </summary>
        public int GetXPForLevel(int level)
        {
            int xp = 0;

            int levelIndex = level - 1;
            if (levelIndex > -1 && levelIndex < _xpByLevel.Count)
            {
                xp = _xpByLevel[levelIndex];
            }

            return xp;
        }

        /// <summary>
        /// Gets the ability associated with the skill
        /// </summary>
        public Ability GetSkillAbility(Skill skill)
        {
            Ability ability = Ability.None;

            if (_skillAbilityMap.ContainsKey(skill))
            {
                ability = _skillAbilityMap[skill];
            }

            return ability;
        }

        /// <summary>
        /// Gets monster xp
        /// </summary>
        public int GetMonsterXP(MonsterModel monsterModel)
        {
            int xp = 0;

            string xpString = _stringService.CRXPString(monsterModel.CR);
            if (!String.IsNullOrWhiteSpace(xpString))
            {
                if (Int32.TryParse(xpString.Replace(",", ""), out int parsedXP))
                {
                    xp = parsedXP;
                }
            }

            return xp;
        }

        /// <summary>
        /// Gets xp multiplier for encounter challenge estimates
        /// </summary>
        public float GetXPMultiplier(int totalMonsters)
        {
            float multiplier = 1.0f;

            if (totalMonsters == 2)
            {
                multiplier = 1.5f;
            }
            else if (totalMonsters > 2 && totalMonsters < 7)
            {
                multiplier = 2.0f;
            }
            else if (totalMonsters > 6 && totalMonsters < 11)
            {
                multiplier = 2.5f;
            }
            else if (totalMonsters > 10 && totalMonsters < 15)
            {
                multiplier = 3.0f;
            }
            else if (totalMonsters >= 15)
            {
                multiplier = 4.0f;
            }

            return multiplier;
        }

        /// <summary>
        /// Gets an estimate of the challenge for an encounter.
        /// </summary>
        public EncounterChallenge EstimateEncounterChallenge(List<EncounterCharacterModel> characters, List<EncounterMonsterModel> monsters)
        {
            IEnumerable<EncounterCreatureModel> encounterCharacters = characters.Where(x => x is EncounterCreatureModel);
            IEnumerable<EncounterCreatureModel> encounterMonsters = monsters.Where(x => x is EncounterCreatureModel);

            return EstimateEncounterChallenge(encounterCharacters, encounterMonsters);
        }

        /// <summary>
        /// Gets an estimate of the challenge for an encounter.
        /// </summary>
        public EncounterChallenge EstimateEncounterChallenge(IEnumerable<EncounterCreatureModel> characters, IEnumerable<EncounterCreatureModel> monsters)
        {
            EncounterChallenge challenge = EncounterChallenge.Unknown;

            int easyThreshold = 0;
            int mediumThreshold = 0;
            int hardThreshold = 0;
            int deadlyThreshold = 0;
            int tpkThreshold = 0;
            int totalMonsters = 0;
            int totalMonsterXP = 0;

            foreach (EncounterCharacterModel character in characters)
            {
                int baseThreshold = _baseXPThresholds[1];
                if (_baseXPThresholds.ContainsKey(character.Level))
                {
                    baseThreshold = _baseXPThresholds[character.Level];
                }

                easyThreshold += baseThreshold;
                mediumThreshold += (2 * baseThreshold);
                hardThreshold += (3 * baseThreshold);
                deadlyThreshold += (4 * baseThreshold);
                tpkThreshold += (5 * baseThreshold);
            }

            foreach (EncounterMonsterModel monster in monsters)
            {
                totalMonsterXP += (GetMonsterXP(monster.MonsterModel) * monster.Quantity);
                totalMonsters += monster.Quantity;
            }

            float adjustedMonsterXP = totalMonsterXP * GetXPMultiplier(totalMonsters);

            if (adjustedMonsterXP > 0)
            {
                challenge = EncounterChallenge.Easy;

                if (adjustedMonsterXP > easyThreshold)
                {
                    if (adjustedMonsterXP < mediumThreshold)
                    {
                        challenge = EncounterChallenge.Easy;
                    }
                    else if (adjustedMonsterXP < hardThreshold)
                    {
                        challenge = EncounterChallenge.Medium;
                    }
                    else if (adjustedMonsterXP < deadlyThreshold)
                    {
                        challenge = EncounterChallenge.Hard;
                    }
                    else if (adjustedMonsterXP < tpkThreshold)
                    {
                        challenge = EncounterChallenge.Deadly;
                    }
                    else
                    {
                        challenge = EncounterChallenge.TPK;
                    }
                }
            }

            return challenge;
        }

        #endregion
    }
}
