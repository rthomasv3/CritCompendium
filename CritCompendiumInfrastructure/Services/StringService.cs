using System;
using System.Linq;
using System.Collections.Generic;
using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Enums;

namespace CriticalCompendiumInfrastructure.Services
{
	public sealed class StringService
	{
		#region Fields

		private readonly Dictionary<Type, List<string>> _stringDictionary;
		private readonly Dictionary<Type, List<string>> _stringAbbreviationDictionary;

		private readonly List<string> _skillStrings;
		private readonly List<string> _abilityStrings;
		private readonly List<string> _abilityStringAbbreviations;
		private readonly List<string> _traitTypeStrings;
		private readonly List<string> _spellSchoolStrings;
		private readonly List<string> _spellSchoolStringAbbreviations;
		private readonly List<string> _itemTypeStrings;
		private readonly List<string> _itemTypeStringAbbreviations;
		private readonly List<string> _alignmentStrings;
		private readonly List<string> _creatureSizeStrings;
		private readonly List<string> _creatureSizeStringAbbreviations;
		private readonly List<string> _damageTypeStrings;
		private readonly List<string> _damageTypeStringAbbreviations;
		private readonly List<string> _currencyStrings;
		private readonly List<string> _modifierCategoryStrings;
		private readonly List<string> _lightArmor;
		private readonly List<string> _mediumArmor;
		private readonly List<string> _heavyArmor;
		private readonly List<string> _shields;
		private readonly List<string> _simpleWeapons;
		private readonly List<string> _martialWeapons;
        private readonly List<string> _artisanToolStrings;
        private readonly List<string> _toolStrings;
		private readonly List<string> _kitStrings;
		private readonly List<string> _gamingKitStrings;
		private readonly List<string> _musicalInstrumentStrings;
		private readonly List<string> _vehicleStrings;
		private readonly List<string> _armorStrings;
		private readonly List<string> _weaponStrings;
		private readonly List<string> _rarityStrings;
		private readonly List<string> _crStrings;
		private readonly Dictionary<string, string> _crXPStrings;
        private readonly List<string> _encounterChallengeStrings;
        private readonly List<string> _statModificationStrings;
        private readonly string _longRestDescription;
        private readonly string _shortRestDescription;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="StringService"/>
        /// </summary>
        public StringService()
		{
			_skillStrings = new List<string> { "Acrobatics", "Animal Handling", "Arcana", "Athletics", "Deception",
                                               "History", "Insight", "Intimidation", "Investigation", "Medicine",
                                               "Nature", "Perception", "Performance", "Persuasion", "Religion",
                                               "Sleight of Hand", "Stealth", "Survival" };

			_abilityStrings = new List<string> { "Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma" };

			_abilityStringAbbreviations = new List<string> { "Str", "Dex", "Con", "Int", "Wis", "Cha" };

			_traitTypeStrings = new List<string> { "Information", "Language", "Tool Proficiency", "Skill Proficiency", "Starting Proficiency" };

			_spellSchoolStrings = new List<string> { "Abjuration", "Conjuration", "Divination", "Enchantment",
                                                     "Evocation", "Illusion", "Necromancy", "Transmutation" };

			_spellSchoolStringAbbreviations = new List<string>{ "A", "C", "D", "EN",
																				 "EV", "I", "N", "T" };

			_itemTypeStrings = new List<string> { "Ammunition", "Gear", "Heavy Armor", "Light Armor", "Melee Weapon", "Medium Armor",
                                                  "Potion", "Ranged Weapon", "Rod", "Ring", "Shield", "Scroll", "Staff", "Treasure",
                                                  "Wonderous", "Wand" };

			_itemTypeStringAbbreviations = new List<string> { "A", "G", "HA", "LA", "M", "MA",
                                                              "P", "R", "RD", "RG", "S", "SC", "ST", "$",
                                                              "W", "WD" };

			_alignmentStrings = new List<string> { "Lawful Good", "Neutral Good", "Chaotic Good", "Lawful Neutral", "Neutral",
                                                   "Chaotic Neutral", "Lawful Evil", "Neutral Evil", "Chaotic Evil", "Unaligned" };

			_creatureSizeStrings = new List<string> { "Fine", "Diminutive", "Tiny", "Small", "Medium", "Large", "Huge",
													  "Gargantuan", "Colossal", "Colossal Plus" };

			_creatureSizeStringAbbreviations = new List<string> { "F", "D", "T", "S", "M", "L", "H",
																  "G", "C", "CP" };

			_damageTypeStrings = new List<string> { "Acid", "Bludgeoning", "Cold", "Fire", "Force", "Lightning", "Necrotic",
													"Piercing", "Poison", "Psychic", "Radiant", "Slashing", "Thunder" };

			_damageTypeStringAbbreviations = new List<string> { "A", "B", "C", "", "", "L", "N",
																"P", "", "", "R", "S", "T" };

			_currencyStrings = new List<string> { "Copper", "Silver", "Electrum", "Gold", "Platinum" };

			_modifierCategoryStrings = new List<string> { "Ability Score", "Bonus" };

            _lightArmor = new List<string> { "Light Armor", "Padded", "Leather", "Studded Leather" };

            _mediumArmor = new List<string> { "Medium Armor", "Hide", "Chain Shirt", "Scale Mail", "Breastplate", "Half Plate" };

            _heavyArmor = new List<string> { "Heavy Armor", "Ring Mail", "Chain Mail", "Splint", "Plate" };

            _shields = new List<string> { "Shields", "Shield" };

            _simpleWeapons = new List<string> { "Simple Weapons", "Club", "Dagger", "Greatclub", "Handaxe", "Javelin", "Light Hammer", "Mace",
                                                "Quarterstaff", "Sickle", "Spear", "Light Crossbow", "Dart", "Shortbow", "Sling" };

            _martialWeapons = new List<string> { "Martial Weapons", "Battleaxe", "Flail", "Glaive", "Greataxe", "Greatsword",
                                                 "Halberd", "Lance", "Longsword", "Maul", "Morningstar", "Pike", "Rapier",
                                                 "Scimitar", "Shortsword", "Trident", "War Pick", "Warhammer", "Whip",
                                                 "Blowgun", "Hand Crossbow", "Heavy Crossbow", "Longbow", "Net" };

            _artisanToolStrings = new List<string> { "Alchemist's Supplies", "Brewer's Supplies",
													 "Calligrapher's Supplies", "Carpenter's Tools",
													 "Cartographer's Tools",  "Cobbler's Tools",
													 "Cook's Utensils", "Glassblower's Tools",
													 "Jewelers's Tools", "Leatherworker's Tools",
													 "Mason's Tools", "Painter's Supplies",
													 "Potter's Tools", "Smith's Tools",
													 "Tinker's Tools", "Weaver's Tools",
													 "Woodcarver's Tools" };

			_toolStrings = new List<string> { "Navigator's Tools", "Thieves' Tools", };

			_kitStrings = new List<string> { "Disguise Kit", "Forgery Kit", "Herbalism Kit", "Poisoner's Kit" };

			_gamingKitStrings = new List<string> { "Dice Set", "Dragonchess Set", "Playing Card Set", "Three-Dragon Ante Set" };

			_musicalInstrumentStrings = new List<string> { "Bagpipes", "Drum", "Dulcimer", "Flute", "Lute",
														   "Lyre", "Horn", "Pan Flute", "Shawm", "Viol" };

			_vehicleStrings = new List<string> { "Land Vehicles", "Water Vehicles", };

			_armorStrings = new List<string> { "Light Armor", "Medium Armor", "Heavy Armor", "Shield" };

			_weaponStrings = new List<string> { "Simple Weapon", "Martial Weapon" };

			_rarityStrings = new List<string> { "Common", "Uncommon", "Rare", "Very Rare", "Legendary", "Artifact" };

			_crStrings = new List<string> { "0", "1/8", "1/4", "1/2", "1", "2", "3", "4", "5",
                                            "6", "7", "8", "9", "10", "11", "12", "13", "14",
                                            "15", "16", "17", "18", "19", "20", "21", "22",
                                            "23", "24", "25", "26", "27", "28", "29", "30" };

			_crXPStrings = new Dictionary<string, string>(){ { "0", "0"}, { "1/8", "25"}, { "1/4", "50"}, { "1/2", "100"},
                                                             { "1", "200"}, { "2", "450"}, { "3", "700"}, { "4", "1,100"},
                                                             { "5", "1,800"}, { "6", "2,300"}, { "7", "2,900"}, { "8", "3,900"},
                                                             { "9", "5,000"}, { "10", "5,900"}, { "11", "7,200"}, { "12", "8,400"},
                                                             { "13", "10,000"}, { "14", "11,500"}, { "15", "13,000"}, { "16", "15,000"},
                                                             { "17", "18,000"}, { "18", "20,000"}, { "19", "22,000"}, { "20", "25,000"},
                                                             { "21", "33,000"}, { "22", "41,000"}, { "23", "50,000"}, { "24", "62,000"},
                                                             { "25", "75,000"}, { "26", "90,000"}, { "27", "105,000"}, { "28", "120,000"},
                                                             { "29", "135,000"}, { "30", "155,000"}};

            _encounterChallengeStrings = new List<string> { "Unknown", "Easy", "Medium", "Hard", "Deadly", "TPK" };

            _statModificationStrings = new List<string> { "Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma", "Initiative",
                                                          "Passive Perception", "Passive Investigation", "Saving Throws", "Non-Proficient Ability Checks" };

            _stringDictionary = new Dictionary<Type, List<string>>
			{
				{ typeof(Skill), _skillStrings },
				{ typeof(Ability), _abilityStrings },
				{ typeof(TraitType), _traitTypeStrings },
				{ typeof(SpellSchool), _spellSchoolStrings },
				{ typeof(ItemType), _itemTypeStrings },
				{ typeof(Alignment), _alignmentStrings },
				{ typeof(CreatureSize), _creatureSizeStrings },
				{ typeof(DamageType), _damageTypeStrings },
				{ typeof(Currency), _currencyStrings },
				{ typeof(ModifierCategory), _modifierCategoryStrings },
				{ typeof(LightArmor), _lightArmor },
				{ typeof(MediumArmor), _mediumArmor },
				{ typeof(HeavyArmor), _heavyArmor },
				{ typeof(Shield), _shields },
				{ typeof(SimpleWeapon), _simpleWeapons },
				{ typeof(MartialWeapon), _martialWeapons },
                { typeof(ArtisanTool), _artisanToolStrings },
                { typeof(Tool), _toolStrings },
				{ typeof(Kit), _kitStrings },
				{ typeof(GamingSet), _gamingKitStrings },
				{ typeof(MusicalInstrument), _musicalInstrumentStrings },
				{ typeof(Vehicle), _vehicleStrings },
				{ typeof(ArmorType), _armorStrings },
				{ typeof(WeaponType), _weaponStrings },
				{ typeof(Rarity), _rarityStrings },
				{ typeof(EncounterChallenge), _encounterChallengeStrings },
				{ typeof(StatModificationOption), _statModificationStrings }
            };

			_stringAbbreviationDictionary = new Dictionary<Type, List<string>>
			{
				{ typeof(Ability), _abilityStringAbbreviations },
				{ typeof(SpellSchool), _spellSchoolStringAbbreviations },
				{ typeof(ItemType), _itemTypeStringAbbreviations },
				{ typeof(CreatureSize), _creatureSizeStringAbbreviations },
				{ typeof(DamageType), _damageTypeStringAbbreviations }
			};

            _longRestDescription = @"A long rest is a period of extended downtime, at least 8 hours long, during which a character sleeps or performs light activity: reading, talking, eating, or standing watch for no more than 2 hours. If the rest is interrupted by a period of strenuous activity - at least 1 hour of walking, fighting, casting spells, or similar adventuring activity - the characters must begin the rest again to gain any benefit from it.

At the end of a long rest, a character regains all lost hit points. The character also regains spent Hit Dice, up to a number of dice equal to half of the character's total number of them (minimum of one die). For example, if a character has eight Hit Dice, he or she can regain four spent Hit Dice upon finishing a long rest.

A character can't benefit from more than one long rest in a 24-hour period, and a character must have at least 1 hit point at the start of the rest to gain its benefits.

Unless a feature that grants you temporary hit points has a duration, they last until they're depleted or you finish a long rest.

Companions will also regain all lost hit points.";

            _shortRestDescription = @"A short rest is a period of downtime, at least 1 hour long, during which a character does nothing more strenuous than eating, drinking, reading, and tending to wounds.

A character can spend one or more Hit Dice at the end of a short rest, up to the character’s maximum number of Hit Dice, which is equal to the character’s level. For each Hit Die spent in this way, the player rolls the die and adds the character’s Constitution modifier to it. The character regains hit points equal to the total. The player can decide to spend an additional Hit Die after each roll. A character regains some spent Hit Dice upon finishing a long rest.";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets long rest description
        /// </summary>
        public string LongRestDescription
        {
            get { return _longRestDescription; }
        }

        /// <summary>
        /// Gets short rest description
        /// </summary>
        public string ShortRestDescription
        {
            get { return _shortRestDescription; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Capitalizes all words in string
        /// </summary>
        public string CapitalizeWords(string s)
		{
			string capital = String.Empty;

            if (s != null)
            {
                if (s.Contains(" "))
                {
                    foreach (string word in s.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string lowerWord = word.ToLower();

                        if (word.Length > 1 && Char.IsLetter(word[0]) && char.IsLower(word[0]) && WordIsSignificant(word))
                        {
                            capital += Char.ToUpper(word[0]) + word.Substring(1) + " ";
                        }
                        else if (word.Length > 2 && word[0] == '(' && WordIsSignificant(word))
                        {
                            capital += word[0].ToString() + Char.ToUpper(word[1]) + word.Substring(2) + " ";
                        }
                        else
                        {
                            capital += word + " ";
                        }
                    }
                }
                else if (s.Length > 1 && Char.IsLetter(s[0]) && char.IsLower(s[0]))
                {
                    capital = Char.ToUpper(s[0]) + s.Substring(1);
                }
                else
                {
                    capital = s;
                }
            }

			return capital.Trim();
		}

		/// <summary>
		/// Gets the string representation of the enum
		/// </summary>
		public string GetString(Enum e)
		{
			string s = String.Empty;

			if (e != null)
			{
				Type type = e.GetType();
				int index = Convert.ToInt32(e);

				if (_stringDictionary.ContainsKey(type))
				{
					if (index > -1 && index < _stringDictionary[type].Count)
					{
						s = _stringDictionary[type][index];
					}
				}
				else if (_stringAbbreviationDictionary.ContainsKey(type))
				{
					if (index > -1 && index < _stringAbbreviationDictionary[type].Count)
					{
						if (!String.IsNullOrWhiteSpace(_stringAbbreviationDictionary[type][index]))
						{
							s = _stringAbbreviationDictionary[type][index];
						}
					}
				}
			}

			return s;
		}

        /// <summary>
        /// Gets the abbreviated string representation of the enum 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public string GetAbbreviationString(Enum e)
        {
            string s = String.Empty;

            if (e != null)
            {
                Type type = e.GetType();
                int index = Convert.ToInt32(e);

                if (_stringAbbreviationDictionary.ContainsKey(type))
				{
					if (index > -1 && index < _stringAbbreviationDictionary[type].Count)
					{
						if (!String.IsNullOrWhiteSpace(_stringAbbreviationDictionary[type][index]))
						{
							s = _stringAbbreviationDictionary[type][index];
						}
					}
				}
            }

            return s;
        }

		/// <summary>
		/// Gets the enum from the string
		/// </summary>
		public T GetEnum<T>(string enumString)
		{
			Type type = typeof(T);

			T result = (T)Enum.ToObject(type, -1);

			if (enumString != null)
			{
				if (_stringDictionary.ContainsKey(type))
				{
					string s = enumString.Trim().ToLower();

					string found = _stringDictionary[type].FirstOrDefault(x => x.ToLower() == s);
					if (found != null)
					{
						result = (T)Enum.ToObject(type, _stringDictionary[type].IndexOf(found));
					}
					else if (_stringAbbreviationDictionary.ContainsKey(type))
					{
						found = _stringAbbreviationDictionary[type].FirstOrDefault(x => x.ToLower() == s);
						if (found != null)
						{
							result = (T)Enum.ToObject(type, _stringAbbreviationDictionary[type].IndexOf(found));
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Gets the CR XP string
		/// </summary>
		public string CRXPString(string cr)
		{
			string crXP = String.Empty;

			if (_crXPStrings.ContainsKey(cr))
			{
				crXP = _crXPStrings[cr];
			}

			return crXP;
		}

        /// <summary>
        /// Converts a number to ordinal string
        /// </summary>
        public string NumberToOrdinal(int number)
        {
            string extension = "th";
            
            int last_digits = number % 100;
            
            if (last_digits < 11 || last_digits > 13)
            {
                switch (last_digits % 10)
                {
                    case 1:
                        extension = "st";
                        break;
                    case 2:
                        extension = "nd";
                        break;
                    case 3:
                        extension = "rd";
                        break;
                }
            }

            return number + extension;
        }

        /// <summary>
        /// Gets armor proficiency string
        /// </summary>
        public string GetArmorProficiencyString(ArmorProficiencyModel armorProficiency)
        {
            List<string> armors = new List<string>();

            if (armorProficiency.LightArmorProficiency)
            {
                armors.Add(GetString(LightArmor.All));
            }
            else
            {
                if (armorProficiency.PaddedProficiency)
                {
                    armors.Add(GetString(LightArmor.Padded));
                }
                if (armorProficiency.LeatherProficiency)
                {
                    armors.Add(GetString(LightArmor.Leather));
                }
                if (armorProficiency.StuddedLeatherProficiency)
                {
                    armors.Add(GetString(LightArmor.StuddedLeather));
                }
            }

            if (armorProficiency.MediumArmorProficiency)
            {
                armors.Add(GetString(MediumArmor.All));
            }
            else
            {
                if (armorProficiency.HideProficiency)
                {
                    armors.Add(GetString(MediumArmor.Hide));
                }
                if (armorProficiency.ChainShirtProficiency)
                {
                    armors.Add(GetString(MediumArmor.ChainShirt));
                }
                if (armorProficiency.ScaleMailProficiency)
                {
                    armors.Add(GetString(MediumArmor.ScaleMail));
                }
                if (armorProficiency.BreastplateProficiency)
                {
                    armors.Add(GetString(MediumArmor.Breastplate));
                }
                if (armorProficiency.HalfPlateProficiency)
                {
                    armors.Add(GetString(MediumArmor.HalfPlate));
                }
            }

            if (armorProficiency.HeavyArmorProficiency)
            {
                armors.Add(GetString(HeavyArmor.All));
            }
            else
            {
                if (armorProficiency.RingMailProficiency)
                {
                    armors.Add(GetString(HeavyArmor.RingMail));
                }
                if (armorProficiency.ChainMailProficiency)
                {
                    armors.Add(GetString(HeavyArmor.ChainMail));
                }
                if (armorProficiency.SplintProficiency)
                {
                    armors.Add(GetString(HeavyArmor.Splint));
                }
                if (armorProficiency.PlateProficiency)
                {
                    armors.Add(GetString(HeavyArmor.Plate));
                }
            }

            if (armorProficiency.ShieldsProficiency)
            {
                armors.Add(GetString(Shield.All));
            }
            else
            {
                if (armorProficiency.ShieldProficiency)
                {
                    armors.Add(GetString(Shield.Shield));
                }
            }

            return armors.Any() ? String.Join(", ", armors) : "None";
        }

        /// <summary>
        /// Gets weapon proficiency string
        /// </summary>
        public string GetWeaponProficiencyString(WeaponProficiencyModel weaponProficiency)
        {
            List<string> weapons = new List<string>();

            if (weaponProficiency.SimpleWeaponsProficiency)
            {
                weapons.Add(GetString(SimpleWeapon.All));
            }
            else
            {
                if (weaponProficiency.ClubProficiency)
                {
                    weapons.Add(GetString(SimpleWeapon.Club));
                }
                if (weaponProficiency.DaggerProficiency)
                {
                    weapons.Add(GetString(SimpleWeapon.Dagger));
                }
                if (weaponProficiency.GreatclubProficiency)
                {
                    weapons.Add(GetString(SimpleWeapon.Greatclub));
                }
                if (weaponProficiency.HandaxeProficiency)
                {
                    weapons.Add(GetString(SimpleWeapon.Handaxe));
                }
                if (weaponProficiency.JavelinProficiency)
                {
                    weapons.Add(GetString(SimpleWeapon.Javelin));
                }
                if (weaponProficiency.LightHammerProficiency)
                {
                    weapons.Add(GetString(SimpleWeapon.LightHammer));
                }
                if (weaponProficiency.MaceProficiency)
                {
                    weapons.Add(GetString(SimpleWeapon.Mace));
                }
                if (weaponProficiency.QuarterstaffProficiency)
                {
                    weapons.Add(GetString(SimpleWeapon.Quarterstaff));
                }
                if (weaponProficiency.SickleProficiency)
                {
                    weapons.Add(GetString(SimpleWeapon.Sickle));
                }
                if (weaponProficiency.SpearProficiency)
                {
                    weapons.Add(GetString(SimpleWeapon.Spear));
                }
                if (weaponProficiency.CrossbowLightProficiency)
                {
                    weapons.Add(GetString(SimpleWeapon.CrossbowLight));
                }
                if (weaponProficiency.DartProficiency)
                {
                    weapons.Add(GetString(SimpleWeapon.Dart));
                }
                if (weaponProficiency.ShortbowProficiency)
                {
                    weapons.Add(GetString(SimpleWeapon.Shortbow));
                }
                if (weaponProficiency.SlingProficiency)
                {
                    weapons.Add(GetString(SimpleWeapon.Sling));
                }
            }

            if (weaponProficiency.MartialWeaponsProficiency)
            {
                weapons.Add(GetString(MartialWeapon.All));
            }
            else
            {
                if (weaponProficiency.BattleaxeProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Battleaxe));
                }
                if (weaponProficiency.FlailProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Flail));
                }
                if (weaponProficiency.GlaiveProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Glaive));
                }
                if (weaponProficiency.GreataxeProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Greataxe));
                }
                if (weaponProficiency.GreatswordProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Greatsword));
                }
                if (weaponProficiency.HalberdProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Halberd));
                }
                if (weaponProficiency.LanceProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Lance));
                }
                if (weaponProficiency.LongswordProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Longsword));
                }
                if (weaponProficiency.MaulProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Maul));
                }
                if (weaponProficiency.MorningstarProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Morningstar));
                }
                if (weaponProficiency.PikeProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Pike));
                }
                if (weaponProficiency.RapierProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Rapier));
                }
                if (weaponProficiency.ScimitarProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Scimitar));
                }
                if (weaponProficiency.ShortswordProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Shortsword));
                }
                if (weaponProficiency.TridentProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Trident));
                }
                if (weaponProficiency.WarPickProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.WarPick));
                }
                if (weaponProficiency.WarhammerProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Warhammer));
                }
                if (weaponProficiency.WhipProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Whip));
                }
                if (weaponProficiency.BlowgunProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Blowgun));
                }
                if (weaponProficiency.CrossbowHandProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.CrossbowHand));
                }
                if (weaponProficiency.CrossbowHeavyProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.CrossbowHeavy));
                }
                if (weaponProficiency.LongbowProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Longbow));
                }
                if (weaponProficiency.NetProficiency)
                {
                    weapons.Add(GetString(MartialWeapon.Net));
                }
            }

            return weapons.Any() ? String.Join(", ", weapons) : "None";
        }

        /// <summary>
        /// Gets tool proficiency string
        /// </summary>
        public string GetToolProficiencyString(ToolProficiencyModel toolProficiency)
        {
            List<string> tools = new List<string>();

            if (toolProficiency.AlchemistsSuppliesProficiency)
            {
                tools.Add(GetString(ArtisanTool.Alchemists_Supplies));
            }
            if (toolProficiency.BrewersSuppliesProficiency)
            {
                tools.Add(GetString(ArtisanTool.Brewers_Supplies));
            }
            if (toolProficiency.CalligraphersSuppliesProficiency)
            {
                tools.Add(GetString(ArtisanTool.Calligraphers_Supplies));
            }
            if (toolProficiency.CarpentersToolsProficiency)
            {
                tools.Add(GetString(ArtisanTool.Carpenters_Tools));
            }
            if (toolProficiency.CartographersToolsProficiency)
            {
                tools.Add(GetString(ArtisanTool.Cartographers_Tools));
            }
            if (toolProficiency.CobblersToolsProficiency)
            {
                tools.Add(GetString(ArtisanTool.Cobblers_Tools));
            }
            if (toolProficiency.CooksUtensilsProficiency)
            {
                tools.Add(GetString(ArtisanTool.Cooks_Utensils));
            }
            if (toolProficiency.GlassblowersToolsProficiency)
            {
                tools.Add(GetString(ArtisanTool.Glassblowers_Tools));
            }
            if (toolProficiency.JewelerssToolsProficiency)
            {
                tools.Add(GetString(ArtisanTool.Jewelerss_Tools));
            }
            if (toolProficiency.LeatherworkersToolsProficiency)
            {
                tools.Add(GetString(ArtisanTool.Leatherworkers_Tools));
            }
            if (toolProficiency.MasonsToolsProficiency)
            {
                tools.Add(GetString(ArtisanTool.Masons_Tools));
            }
            if (toolProficiency.PaintersSuppliesProficiency)
            {
                tools.Add(GetString(ArtisanTool.Painters_Supplies));
            }
            if (toolProficiency.PottersToolsProficiency)
            {
                tools.Add(GetString(ArtisanTool.Potters_Tools));
            }
            if (toolProficiency.SmithsToolsProficiency)
            {
                tools.Add(GetString(ArtisanTool.Smiths_Tools));
            }
            if (toolProficiency.TinkersToolsProficiency)
            {
                tools.Add(GetString(ArtisanTool.Tinkers_Tools));
            }
            if (toolProficiency.WeaversToolsProficiency)
            {
                tools.Add(GetString(ArtisanTool.Weavers_Tools));
            }
            if (toolProficiency.WoodcarversToolsProficiency)
            {
                tools.Add(GetString(ArtisanTool.Woodcarvers_Tools));
            }

            if (toolProficiency.DiceSetProficiency)
            {
                tools.Add(GetString(GamingSet.Dice_Set));
            }
            if (toolProficiency.PlayingCardSetProficiency)
            {
                tools.Add(GetString(GamingSet.Playing_Card_Set));
            }

            if (toolProficiency.DisguiseKitProficiency)
            {
                tools.Add(GetString(Kit.Disguise_Kit));
            }
            if (toolProficiency.ForgeryKitProficiency)
            {
                tools.Add(GetString(Kit.Forgery_Kit));
            }
            if (toolProficiency.HerbalismKitProficiency)
            {
                tools.Add(GetString(Kit.Herbalism_Kit));
            }
            if (toolProficiency.PoisonersKitProficiency)
            {
                tools.Add(GetString(Kit.Poisoners_Kit));
            }

            if (toolProficiency.BagpipesProficiency)
            {
                tools.Add(GetString(MusicalInstrument.Bagpipes));
            }
            if (toolProficiency.DrumProficiency)
            {
                tools.Add(GetString(MusicalInstrument.Drum));
            }
            if (toolProficiency.DulcimerProficiency)
            {
                tools.Add(GetString(MusicalInstrument.Dulcimer));
            }
            if (toolProficiency.FluteProficiency)
            {
                tools.Add(GetString(MusicalInstrument.Flute));
            }
            if (toolProficiency.LuteProficiency)
            {
                tools.Add(GetString(MusicalInstrument.Lute));
            }
            if (toolProficiency.LyreProficiency)
            {
                tools.Add(GetString(MusicalInstrument.Lyre));
            }
            if (toolProficiency.HornProficiency)
            {
                tools.Add(GetString(MusicalInstrument.Horn));
            }
            if (toolProficiency.PanFluteProficiency)
            {
                tools.Add(GetString(MusicalInstrument.Pan_Flute));
            }
            if (toolProficiency.ShawmProficiency)
            {
                tools.Add(GetString(MusicalInstrument.Shawm));
            }
            if (toolProficiency.ViolProficiency)
            {
                tools.Add(GetString(MusicalInstrument.Viol));
            }

            if (toolProficiency.NavigatorsToolsProficiency)
            {
                tools.Add(GetString(Tool.Navigators_Tools));
            }
            if (toolProficiency.ThievesToolsProficiency)
            {
                tools.Add(GetString(Tool.Thieves_Tools));
            }

            if (toolProficiency.LandVehiclesProficiency)
            {
                tools.Add(GetString(Vehicle.Land_Vehicles));
            }
            if (toolProficiency.WaterVehiclesProficiency)
            {
                tools.Add(GetString(Vehicle.Water_Vehicles));
            }

            return tools.Any() ? String.Join(", ", tools) : "None";
        }

        /// <summary>
        /// Returns "Unknown" if the value is null, empty, or whitespace
        /// </summary>
        public string UnknownIfNullOrEmpty(string value)
        {
            return String.IsNullOrWhiteSpace(value) ? "Unknown" : value;
        }

        #endregion

        #region Private Methods

        private bool WordIsSignificant(string word)
		{
			string lowerWord = word.ToLower();
			return lowerWord.Length > 1 && lowerWord != "of" && lowerWord != "the" && lowerWord != "ft." && lowerWord != "ft" && lowerWord != "with" && lowerWord != "or" && lowerWord != "to";
		}

		#endregion
	}
}
