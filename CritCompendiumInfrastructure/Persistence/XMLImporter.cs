using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendiumInfrastructure.Persistence
{
	public class XMLImporter
	{
		#region Fields

		private readonly StringService _stringService;
		private readonly List<LanguageModel> _languages = new List<LanguageModel>();
		private XmlDocument _doc;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new instance of <see cref="XMLImporter"/>
		/// </summary>
		/// <param name="stringService"></param>
		public XMLImporter(StringService stringService)
		{
			_stringService = stringService;
		}

		#endregion

		#region Properties

		/// <summary>
		/// List of languges found when reading all other data
		/// </summary>
		public List<LanguageModel> LanguagesFound
		{
			get { return _languages.OrderBy(x => x.Name).ToList(); }
		}

		#endregion

		#region Public Methods
        
		/// <summary>
		/// Loads the xml from the manifest resource
		/// </summary>
		public void LoadManifestResourceXML(string resource)
		{
            string xml = String.Empty;

            using (Stream stream = GetType().Assembly.GetManifestResourceStream("CritCompendiumInfrastructure.Resources." + resource))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    xml = sr.ReadToEnd();
                }
            }

            _doc = new XmlDocument();
            _doc.LoadXml(xml);
        }

        /// <summary>
        /// Loads the xml from the file
        /// </summary>
        public void LoadFileXML(string fileLocation)
        {
            string xml = String.Empty;

            if (File.Exists(fileLocation))
            {
                using (Stream stream = File.OpenRead(fileLocation))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        xml = sr.ReadToEnd();
                    }
                }
            }

            _doc = new XmlDocument();
            _doc.LoadXml(xml);
        }

		/// <summary>
		/// Formats the xml string
		/// </summary>
		public string FormatXML(string xml)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml("<default>" + xml + "</default>");

			using (MemoryStream stream = new MemoryStream())
			{
				XmlTextWriter xmlWriter = new XmlTextWriter(stream, null);

				xmlWriter.Indentation = 4;
				xmlWriter.Formatting = Formatting.Indented;
				doc.SelectSingleNode("default").WriteContentTo(xmlWriter);
				xmlWriter.Flush();
				stream.Flush();
				stream.Position = 0;

				using (StreamReader streamReader = new StreamReader(stream))
				{
					return streamReader.ReadToEnd();
				}
			}
		}

		/// <summary>
		/// Formats the node xml
		/// </summary>
		public string FormatXML(XmlNode node)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				XmlTextWriter xmlWriter = new XmlTextWriter(memoryStream, Encoding.Unicode);

				xmlWriter.Formatting = Formatting.Indented;
				xmlWriter.Indentation = 4;
				node.WriteContentTo(xmlWriter);
				xmlWriter.Flush();
				memoryStream.Flush();
				memoryStream.Position = 0;

				using (StreamReader streamReader = new StreamReader(memoryStream))
				{
					return streamReader.ReadToEnd();
				}
			}
		}

		/// <summary>
		/// Reads backgrounds
		/// </summary>
		public List<BackgroundModel> ReadBackgrounds()
		{
			List<BackgroundModel> backgrounds = new List<BackgroundModel>();

            if (_doc.DocumentElement.Name.ToLower() == "background")
            {
                BackgroundModel backgroundModel = GetBackground(_doc.DocumentElement);
                if (backgroundModel != null)
                {
                    backgrounds.Add(backgroundModel);
                }
            }
            else
            {
                foreach (XmlNode backgroundNode in _doc.DocumentElement.SelectNodes("descendant::background"))
                {
                    BackgroundModel backgroundModel = GetBackground(backgroundNode);
                    if (backgroundModel != null)
                    {
                        backgrounds.Add(backgroundModel);
                    }
                }
            }

			return backgrounds.OrderBy(x => x.Name).ToList();
		}

        /// <summary>
		/// Reads backgrounds
		/// </summary>
		public List<BackgroundModel> ReadBackgrounds(string xml)
        {
            List<BackgroundModel> backgrounds = new List<BackgroundModel>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode node in doc.DocumentElement.SelectNodes("descendant::background"))
            {
                BackgroundModel backgroundModel = GetBackground(node);
                if (backgroundModel != null)
                {
                    backgrounds.Add(backgroundModel);
                }
            }

            return backgrounds.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Gets a background model from the xml
        /// </summary>
        public BackgroundModel GetBackground(string xml)
		{
			string enclosedXML = "<background>" + xml + "</background>";
			XmlDocument document = new XmlDocument();
			document.LoadXml(enclosedXML);
			return GetBackground(document.SelectSingleNode("background"));
		}

		/// <summary>
		/// Gets a background from the node
		/// </summary>
		public BackgroundModel GetBackground(XmlNode backgroundNode)
		{
			BackgroundModel backgroundModel = null;

            XmlNode idNode = backgroundNode["id"];
            XmlNode nameNode = backgroundNode["name"];
			XmlNode profNode = backgroundNode["proficiency"];
			XmlNodeList traitNodes = backgroundNode.SelectNodes("trait");

			if (nameNode != null)
			{
				backgroundModel = new BackgroundModel();

                if (idNode != null)
                {
                    if (Guid.TryParse(idNode.InnerText, out Guid id))
                    {
                        backgroundModel.ID = id;
                    }
                    backgroundNode.RemoveChild(idNode);
                }

				backgroundModel.Name = nameNode.InnerText;

				if (profNode != null)
				{
					string[] profs = profNode.InnerText.Split(',');
					for (int i = 0; i < profs.Length; ++i)
					{
						Skill skill = _stringService.GetEnum<Skill>(profs[i]);
						if (skill != Skill.None)
						{
							backgroundModel.Skills.Add(skill);
						}
					}
				}

				backgroundModel.Traits = GetTraits(traitNodes);

				backgroundModel.LanguagesTraitIndex = backgroundModel.Traits.FindIndex(x => x.TraitType == TraitType.Language);
				backgroundModel.ToolsTraitIndex = backgroundModel.Traits.FindIndex(x => x.TraitType == TraitType.Tool_Proficiency);
				backgroundModel.SkillsTraitIndex = backgroundModel.Traits.FindIndex(x => x.TraitType == TraitType.Skill_Proficiency);
				backgroundModel.StartingTraitIndex = backgroundModel.Traits.FindIndex(x => x.TraitType == TraitType.Starting_Proficiency);
                
				backgroundModel.XML = FormatXML(backgroundNode);
			}

			return backgroundModel;
		}

		/// <summary>
		/// Reads classes
		/// </summary>
		public List<ClassModel> ReadClasses()
		{
			List<ClassModel> classes = new List<ClassModel>();

            if (_doc.DocumentElement.Name.ToLower() == "class")
            {
                ClassModel classModel = GetClass(_doc.DocumentElement);
                if (classModel != null)
                {
                    classes.Add(classModel);
                }
            }
            else
            {
                foreach (XmlNode classNode in _doc.DocumentElement.SelectNodes("descendant::class"))
                {
                    ClassModel classModel = GetClass(classNode);
                    if (classModel != null)
                    {
                        classes.Add(classModel);
                    }
                }
            }

			return classes.OrderBy(x => x.Name).ToList();
		}

        /// <summary>
		/// Reads classes
		/// </summary>
		public List<ClassModel> ReadClasses(string xml)
        {
            List<ClassModel> classes = new List<ClassModel>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode node in doc.DocumentElement.SelectNodes("descendant::class"))
            {
                ClassModel classModel = GetClass(node);

                if (classModel != null)
                {
                    classes.Add(classModel);
                }
            }

            return classes.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Gets a class model from the xml
        /// </summary>
        public ClassModel GetClass(string xml)
		{
			string enclosedXML = "<class>" + xml + "</class>";
			XmlDocument document = new XmlDocument();
			document.LoadXml(enclosedXML);
			return GetClass(document.SelectSingleNode("class"));
		}

		/// <summary>
		/// Gets a class model from the xml node
		/// </summary>
		public ClassModel GetClass(XmlNode classNode)
		{
			ClassModel classModel = null;

            XmlNode idNode = classNode["id"];
			XmlNode nameNode = classNode["name"];
			XmlNode hitDieNode = classNode["hd"];
			XmlNode proficiencyNode = classNode["proficiency"];
			XmlNode spellAbilityNode = classNode["spellAbility"];
			XmlNodeList autoLevelNodes = classNode.SelectNodes("autolevel");
			XmlNodeList tableNodes = classNode.SelectNodes("table");

			if (nameNode != null && hitDieNode != null)
			{
				classModel = new ClassModel();

                if (idNode != null)
                {
                    if (Guid.TryParse(idNode.InnerText, out Guid id))
                    {
                        classModel.ID = id;
                    }
                    classNode.RemoveChild(idNode);
                }

                classModel.Name = nameNode.InnerText;
                if (Int32.TryParse(hitDieNode.InnerText, out int hitDie))
                {
                    classModel.HitDie = hitDie;
                }
				classModel.SpellStartLevel = 0;

				if (proficiencyNode != null)
				{
					string[] proficiencies = proficiencyNode.InnerText.Split(',');
					foreach (string prof in proficiencies)
					{
						Ability ability = _stringService.GetEnum<Ability>(prof);
						if (ability != Ability.None && !classModel.AbilityProficiencies.Contains(ability))
						{
							classModel.AbilityProficiencies.Add(ability);
						}
					}
				}

                if (spellAbilityNode != null)
                {
                    classModel.SpellAbility = _stringService.GetEnum<Ability>(spellAbilityNode.InnerText);
                }
                else
                {
                    classModel.SpellAbility = Ability.None;
                }

				foreach (XmlNode autoLevelNode in autoLevelNodes)
				{
					XmlAttribute levelAttribute = autoLevelNode.Attributes["level"];
					XmlAttribute scoreImprovementAttribute = autoLevelNode.Attributes["scoreImprovement"];

					if (levelAttribute != null)
					{
						AutoLevelModel autoLevelModel = new AutoLevelModel();

                        if (Int32.TryParse(levelAttribute.Value, out int value))
                        {
                            autoLevelModel.Level = value;
                        }

						if (scoreImprovementAttribute != null)
						{
							autoLevelModel.ScoreImprovement = scoreImprovementAttribute.Value.ToLower() == "yes";
						}

						XmlNode spellSlotNode = autoLevelNode["slots"];
						if (spellSlotNode != null)
						{
							if (classModel.SpellStartLevel == 0)
							{
								classModel.SpellStartLevel = autoLevelModel.Level;
							}

							classModel.SpellSlots.Add(spellSlotNode.InnerText);
						}

						XmlNodeList featureNodes = autoLevelNode.SelectNodes("feature");
						foreach (XmlNode featureNode in featureNodes)
						{
							XmlNode featureNameNode = featureNode["name"];
							XmlNodeList featureTextNodes = featureNode.SelectNodes("text");
							XmlAttribute optionalAttribute = featureNode.Attributes["optional"];

							if (featureNameNode != null)
							{
								FeatureModel featureModel = new FeatureModel();

								featureModel.Name = featureNameNode.InnerText;

								if (!autoLevelModel.ScoreImprovement && 
									 featureNameNode.InnerText.ToLower().Contains("ability score improvement"))
								{
									autoLevelModel.ScoreImprovement = true;
								}

								featureModel.Optional = false;

								foreach (XmlNode featureTextNode in featureTextNodes)
								{
									featureModel.TextCollection.Add(featureTextNode.InnerText.Replace("•", "").Trim());
								}

								if (optionalAttribute != null)
								{
									featureModel.Optional = optionalAttribute.InnerText.ToLower().Trim() == "yes";
								}

								if (featureModel.Name.ToLower().Contains("starting"))
								{
									foreach (string featureText in featureModel.TextCollection)
									{
										if (featureText.Length > 6 && featureText.Substring(0, 6).ToLower() == "armor:")
										{
											string armor = featureText.ToLower().Replace("armor:", "").Trim();
											classModel.ArmorProficiencies = _stringService.CapitalizeWords(armor);
										}
										else if (featureText.Length > 8 && featureText.Substring(0, 8).ToLower() == "weapons:")
										{
											string weapons = featureText.ToLower().Replace("weapons:", "").Trim();
											classModel.WeaponProficiencies = _stringService.CapitalizeWords(weapons);
										}
										else if (featureText.Length > 6 && featureText.Substring(0, 6).ToLower() == "tools:")
										{
											string tools = featureText.ToLower().Replace("tools:", "").Trim();
											classModel.ToolProficiencies = _stringService.CapitalizeWords(tools);
										}
										else if (featureText.Length > 7 && featureText.Substring(0, 7).ToLower() == "skills:")
										{
											string skills = featureText.ToLower().Replace("skills:", "").Trim();
											classModel.SkillProficiencies = _stringService.CapitalizeWords(skills);
										}
									}
								}

								autoLevelModel.Features.Add(featureModel);
							}
						}

						classModel.AutoLevels.Add(autoLevelModel);
					}
				}

				foreach (XmlNode tableNode in tableNodes)
				{
					XmlNode tableNameNode = tableNode["name"];
					XmlNode tableTextNode = tableNode["text"];

					if (tableNameNode != null && tableTextNode != null)
					{
						FeatureModel featureModel = new FeatureModel();

						featureModel.Name = tableNameNode.InnerText;
						featureModel.TextCollection.Add(tableTextNode.InnerText.Trim());

						classModel.TableFeatures.Add(featureModel);
					}
				}

				classModel.AutoLevels = classModel.AutoLevels.OrderBy(x => x.Level).ToList();

				classModel.XML = FormatXML(classNode);
			}

			return classModel;
		}

		/// <summary>
		/// Reads conditions
		/// </summary>
		public List<ConditionModel> ReadConditions()
		{
			List<ConditionModel> conditions = new List<ConditionModel>();

            if (_doc.DocumentElement.Name.ToLower() == "condition")
            {
                ConditionModel conditionModel = GetCondition(_doc.DocumentElement);
                if (conditionModel != null)
                {
                    conditions.Add(conditionModel);
                }
            }
            else
            {
                foreach (XmlNode conditionNode in _doc.DocumentElement.SelectNodes("descendant::condition"))
                {
                    ConditionModel conditionModel = GetCondition(conditionNode);
                    if (conditionNode != null)
                    {
                        conditions.Add(conditionModel);
                    }
                }
            }

			return conditions.OrderBy(x => x.Name).ToList();
		}

        /// <summary>
		/// Reads conditions
		/// </summary>
		public List<ConditionModel> ReadConditions(string xml)
        {
            List<ConditionModel> conditions = new List<ConditionModel>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode node in doc.DocumentElement.SelectNodes("descendant::condition"))
            {
                ConditionModel condition = GetCondition(node);

                if (condition != null)
                {
                    conditions.Add(condition);
                }
            }

            return conditions.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Gets a condition model from the xml
        /// </summary>
        public ConditionModel GetCondition(string xml)
		{
			string enclosedXML = "<condition>" + xml + "</condition>";
			XmlDocument document = new XmlDocument();
			document.LoadXml(enclosedXML);
			return GetCondition(document.SelectSingleNode("condition"));
		}

		/// <summary>
		/// Gets a condition model from the xml node
		/// </summary>
		public ConditionModel GetCondition(XmlNode conditionNode)
		{
			ConditionModel conditionModel = null;

            XmlNode idNode = conditionNode["id"];
			XmlNode nameNode = conditionNode["name"];
			XmlAttribute levelsAttribute = conditionNode.Attributes["levels"];
			XmlNode pretextNode = conditionNode["pretext"];
			XmlNode tableNode = conditionNode["table"];
			XmlNodeList textNodes = conditionNode.SelectNodes("text");

			if (nameNode != null)
			{
				conditionModel = new ConditionModel();

                if (idNode != null)
                {
                    if (Guid.TryParse(idNode.InnerText, out Guid id))
                    {
                        conditionModel.ID = id;
                    }
                    conditionNode.RemoveChild(idNode);
                }

                conditionModel.Name = nameNode.InnerText;
				conditionModel.Levels = 0;

				if (levelsAttribute != null)
				{
                    if (Int32.TryParse(levelsAttribute.Value, out int value))
                    {
                        conditionModel.Levels = value;
                    }
				}

				if (pretextNode != null)
				{
					conditionModel.Description = pretextNode.InnerText;
				}

				if (tableNode != null)
				{
					XmlAttribute colsAttribute = tableNode.Attributes["cols"];

					if (colsAttribute != null)
					{
                        if (Int32.TryParse(colsAttribute.Value, out int count))
                        {
                            for (int i = 0; i < count; ++i)
                            {
                                string headerAttributeName = "h" + (i + 1).ToString();
                                XmlAttribute headerAttribute = tableNode.Attributes[headerAttributeName];
                                if (headerAttribute != null)
                                {
                                    conditionModel.Headers.Add(headerAttribute.Value);
                                }
                            }

                            foreach (XmlNode tableElementNode in tableNode.SelectNodes("element"))
                            {
                                conditionModel.Elements.Add(tableElementNode.InnerText);
                            }
                        }
					}
				}

				foreach (XmlNode textNode in textNodes)
				{
					conditionModel.TextCollection.Add(textNode.InnerText);
				}

				conditionModel.XML = FormatXML(conditionNode);
			}

			return conditionModel;
		}

		/// <summary>
		/// Reads feats
		/// </summary>
		public List<FeatModel> ReadFeats()
		{
			List<FeatModel> feats = new List<FeatModel>();

            if (_doc.DocumentElement.Name.ToLower() == "feat")
            {
                FeatModel featModel = GetFeat(_doc.DocumentElement);
                if (featModel != null)
                {
                    feats.Add(featModel);
                }
            }
            else
            {
                foreach (XmlNode featNode in _doc.DocumentElement.SelectNodes("descendant::feat"))
                {
                    FeatModel featModel = GetFeat(featNode);
                    if (featModel != null)
                    {
                        feats.Add(featModel);
                    }
                }
            }

			return feats.OrderBy(x => x.Name).ToList();
		}

        /// <summary>
		/// Reads feats
		/// </summary>
		public List<FeatModel> ReadFeats(string xml)
        {
            List<FeatModel> feats = new List<FeatModel>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode node in doc.DocumentElement.SelectNodes("descendant::feat"))
            {
                FeatModel feat = GetFeat(node);

                if (feat != null)
                {
                    feats.Add(feat);
                }
            }

            return feats.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Gets a feat model from the xml
        /// </summary>
        public FeatModel GetFeat(string xml)
		{
			string enclosedXML = "<feat>" + xml + "</feat>";
			XmlDocument document = new XmlDocument();
			document.LoadXml(enclosedXML);
			return GetFeat(document.SelectSingleNode("feat"));
		}

        /// <summary>
        /// Gets a feat model from the xml node
        /// </summary>
        public FeatModel GetFeat(XmlNode featNode)
        {
            FeatModel featModel = null;

            XmlNode idNode = featNode["id"];
			XmlNode nameNode = featNode["name"];
			XmlNode prerequisiteNode = featNode["prerequisite"];
			XmlNodeList textNodes = featNode.SelectNodes("text");
			XmlNodeList modifierNodes = featNode.SelectNodes("modifier");

			if (nameNode != null)
			{
				featModel = new FeatModel();

                if (idNode != null)
                {
                    if (Guid.TryParse(idNode.InnerText, out Guid id))
                    {
                        featModel.ID = id;
                    }
                    featNode.RemoveChild(idNode);
                }

                featModel.Name = nameNode.InnerText;
				featModel.Modifiers = GetModifiers(modifierNodes);

				if (prerequisiteNode != null)
				{
					featModel.Prerequisite = prerequisiteNode.InnerText;
				}

				foreach (XmlNode textNode in textNodes)
				{
					featModel.TextCollection.Add(textNode.InnerText.Replace("•", "").Trim());
				}

				featModel.XML = FormatXML(featNode);
			}

			return featModel;
		}

		/// <summary>
		/// Reads items
		/// </summary>
		public List<ItemModel> ReadItems()
		{
			List<ItemModel> items = new List<ItemModel>();

            if (_doc.DocumentElement.Name.ToLower() == "item")
            {
                ItemModel itemModel = GetItem(_doc.DocumentElement);
                if (itemModel != null)
                {
                    items.Add(itemModel);
                }
            }
            else
            {
                foreach (XmlNode itemNode in _doc.DocumentElement.SelectNodes("descendant::item"))
                {
                    ItemModel itemModel = GetItem(itemNode);
                    if (itemModel != null)
                    {
                        items.Add(itemModel);
                    }
                }
            }

			return items.OrderBy(x => x.Name).ToList();
		}

        /// <summary>
		/// Reads items
		/// </summary>
		public List<ItemModel> ReadItems(string xml)
        {
            List<ItemModel> items = new List<ItemModel>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode node in doc.DocumentElement.SelectNodes("descendant::item"))
            {
                ItemModel item = GetItem(node);

                if (item != null)
                {
                    items.Add(item);
                }
            }

            return items.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Gets item a model from the xml
        /// </summary>
        public ItemModel GetItem(string xml)
		{
			string enclosedXML = "<item>" + xml + "</item>";
			XmlDocument document = new XmlDocument();
			document.LoadXml(enclosedXML);
			return GetItem(document.SelectSingleNode("item"));
		}

		/// <summary>
		/// Gets a item model from the xml node
		/// </summary>
		public ItemModel GetItem(XmlNode itemNode)
		{
			ItemModel itemModel = null;

            XmlNode idNode = itemNode["id"];
			XmlNode nameNode = itemNode["name"];
			XmlNode typeNode = itemNode["type"];
			XmlNode magicNode = itemNode["magic"];
			XmlNode valueNode = itemNode["value"];
			XmlNode weightNode = itemNode["weight"];
			XmlNode dmg1Node = itemNode["dmg1"];
			XmlNode dmg2Node = itemNode["dmg2"];
			XmlNode dmgTypeNode = itemNode["dmgType"];
			XmlNode propertyNode = itemNode["property"];
			XmlNode rarityNode = itemNode["rarity"];
			XmlNode detailNode = itemNode["detail"];
			XmlNode acNode = itemNode["ac"];
			XmlNode strengthNode = itemNode["strength"];
			XmlNode stealthNode = itemNode["stealth"];
			XmlNode rangeNode = itemNode["range"];
			XmlNodeList textNodes = itemNode.SelectNodes("text");
			XmlNodeList modifierNodes = itemNode.SelectNodes("modifier");
			XmlNodeList rollNodes = itemNode.SelectNodes("roll");

			if (nameNode != null)
			{
				itemModel = new ItemModel();

                if (idNode != null)
                {
                    if (Guid.TryParse(idNode.InnerText, out Guid id))
                    {
                        itemModel.ID = id;
                    }
                    itemNode.RemoveChild(idNode);
                }

                itemModel.Name = nameNode.InnerText;
				itemModel.Modifiers = GetModifiers(modifierNodes);

				if (typeNode != null)
				{
					itemModel.Type = _stringService.GetEnum<ItemType>(typeNode.InnerText);
				}

				if (magicNode != null)
				{
					itemModel.Magic = magicNode.InnerText == "1";
				}

				if (valueNode != null)
				{
					itemModel.Value = valueNode.InnerText;
				}

				if (weightNode != null)
				{
					itemModel.Weight = weightNode.InnerText;
				}

				if (dmg1Node != null)
				{
					itemModel.Dmg1 = dmg1Node.InnerText;
				}

				if (dmg2Node != null)
				{
					itemModel.Dmg2 = dmg2Node.InnerText;
				}

				if (dmgTypeNode != null)
				{
					DamageType damageType = _stringService.GetEnum<DamageType>(dmgTypeNode.InnerText);
					if (damageType != DamageType.None)
					{
						itemModel.DmgType = _stringService.GetString(damageType);
					}
					else
					{
						itemModel.DmgType = dmgTypeNode.InnerText;
					}
				}

				if (propertyNode != null)
				{
					itemModel.Properties = propertyNode.InnerText.Trim(',');
				}

				if (rarityNode != null)
				{
					itemModel.Rarity = _stringService.GetEnum<Rarity>(rarityNode.InnerText);
				}

				if (detailNode != null)
				{
					string det = detailNode.InnerText;
					if (det.ToLower().Trim().Contains("requires attunement"))
					{
						itemModel.RequiresAttunement = true;
					}

					if (rarityNode == null)
					{
						det = det.ToLower().Replace("(requires attunement)", "").Replace("requires attunement", "").Trim();
                        if (!String.IsNullOrWhiteSpace(det))
                        {
                            itemModel.Rarity = _stringService.GetEnum<Rarity>(det);
                        }

                        if (itemModel.Rarity == Rarity.None)
                        {
                            if (det.Contains(_stringService.GetString(Rarity.Uncommon).ToLower()))
                            {
                                itemModel.Rarity = Rarity.Uncommon;
                            }
                            else if (det.Contains(_stringService.GetString(Rarity.Common).ToLower()))
                            {
                                itemModel.Rarity = Rarity.Common;
                            }
                            else if (det.Contains(_stringService.GetString(Rarity.Very_Rare).ToLower()))
                            {
                                itemModel.Rarity = Rarity.Very_Rare;
                            }
                            else if (det.Contains(_stringService.GetString(Rarity.Rare).ToLower()))
                            {
                                itemModel.Rarity = Rarity.Rare;
                            }
                            else if (det.Contains(_stringService.GetString(Rarity.Legendary).ToLower()))
                            {
                                itemModel.Rarity = Rarity.Legendary;
                            }
                            else if (det.Contains(_stringService.GetString(Rarity.Artifact).ToLower()))
                            {
                                itemModel.Rarity = Rarity.Artifact;
                            }
                        }
					}
				}

				if (acNode != null)
				{
					itemModel.AC = acNode.InnerText;
				}

				if (strengthNode != null)
				{
					itemModel.StrengthRequirement = strengthNode.InnerText;
				}

				if (stealthNode != null)
				{
					itemModel.StealthDisadvantage = stealthNode.InnerText.ToLower().Trim() == "yes";
				}

				if (rangeNode != null)
				{
					itemModel.Range = rangeNode.InnerText;
				}

				foreach (XmlNode textNode in textNodes)
				{
                    if (!String.IsNullOrWhiteSpace(textNode.InnerText))
                    {
                        itemModel.TextCollection.Add(textNode.InnerText);
                    }
				}

				foreach (XmlNode rollNode in rollNodes)
				{
					itemModel.Rolls.Add(rollNode.InnerText);
				}

				itemModel.XML = FormatXML(itemNode);
			}

			return itemModel;
		}

		/// <summary>
		/// Reads monsters
		/// </summary>
		public List<MonsterModel> ReadMonsters()
		{
			List<MonsterModel> monsters = new List<MonsterModel>();

            if (_doc.DocumentElement.Name.ToLower() == "monster")
            {
                MonsterModel monsterModel = GetMonster(_doc.DocumentElement);
                if (monsterModel != null)
                {
                    monsters.Add(monsterModel);
                }
            }
            else
            {
                foreach (XmlNode monsterNode in _doc.DocumentElement.SelectNodes("descendant::monster"))
                {
                    MonsterModel monsterModel = GetMonster(monsterNode);
                    if (monsterModel != null)
                    {
                        monsters.Add(monsterModel);
                    }
                }
            }

			return monsters.OrderBy(x => x.Name).ToList();
		}

        /// <summary>
		/// Reads monsters
		/// </summary>
		public List<MonsterModel> ReadMonsters(string xml)
        {
            List<MonsterModel> monsters = new List<MonsterModel>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode node in doc.DocumentElement.SelectNodes("descendant::monster"))
            {
                MonsterModel monster = GetMonster(node);

                if (monster != null)
                {
                    monsters.Add(monster);
                }
            }

            return monsters.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Gets a monster model from the xml
        /// </summary>
        public MonsterModel GetMonster(string xml)
		{
			string enclosedXML = "<monster>" + xml + "</monster>";
			XmlDocument document = new XmlDocument();
			document.LoadXml(enclosedXML);
			return GetMonster(document.SelectSingleNode("monster"));
		}

		/// <summary>
		/// Gets a monster model from the xml node
		/// </summary>
		public MonsterModel GetMonster(XmlNode monsterNode)
		{
            MonsterModel monsterModel = null;

            XmlNode idNode = monsterNode["id"];
			XmlNode nameNode = monsterNode["name"];
			XmlNode sizeNode = monsterNode["size"];
			XmlNode typeNode = monsterNode["type"];
			XmlNode alignmentNode = monsterNode["alignment"];
			XmlNode acNode = monsterNode["ac"];
			XmlNode hpNode = monsterNode["hp"];
			XmlNode speedNode = monsterNode["speed"];
			XmlNode strNode = monsterNode["str"];
			XmlNode dexNode = monsterNode["dex"];
			XmlNode conNode = monsterNode["con"];
			XmlNode intNode = monsterNode["int"];
			XmlNode wisNode = monsterNode["wis"];
			XmlNode chaNode = monsterNode["cha"];
			XmlNode descriptionNode = monsterNode["description"];
			XmlNode saveNode = monsterNode["save"];
			XmlNode skillNode = monsterNode["skill"];
			XmlNode resistNode = monsterNode["resist"];
			XmlNode vulnerableNode = monsterNode["vulnerable"];
			XmlNode immuneNode = monsterNode["immune"];
			XmlNode conditionImmuneNode = monsterNode["conditionImmune"];
			XmlNode sensesNode = monsterNode["senses"];
			XmlNode passiveNode = monsterNode["passive"];
			XmlNode languagesNode = monsterNode["languages"];
			XmlNode crNode = monsterNode["cr"];
			XmlNode spellsNode = monsterNode["spells"];
			XmlNode slotsNode = monsterNode["slots"];
			XmlNode environmentNode = monsterNode["environment"];

			XmlNodeList traitNodes = monsterNode.SelectNodes("trait");
			XmlNodeList actionNodes = monsterNode.SelectNodes("action");
			XmlNodeList reactionNodes = monsterNode.SelectNodes("reaction");
            XmlNodeList legendaryNodes = monsterNode.SelectNodes("legendary");

			if (nameNode != null)
			{
				monsterModel = new MonsterModel();

                if (idNode != null)
                {
                    if (Guid.TryParse(idNode.InnerText, out Guid id))
                    {
                        monsterModel.ID = id;
                    }
                    monsterNode.RemoveChild(idNode);
                }

                monsterModel.Name = nameNode.InnerText;

				if (sizeNode != null)
				{
					monsterModel.Size = _stringService.GetEnum<CreatureSize>(sizeNode.InnerText);
				}
				if (typeNode != null)
				{
					monsterModel.Type = typeNode.InnerText;
				}
				if (alignmentNode != null)
				{
					monsterModel.Alignment = alignmentNode.InnerText;
				}
				if (acNode != null)
				{
					monsterModel.AC = acNode.InnerText;
				}
				if (hpNode != null)
				{
					monsterModel.HP = hpNode.InnerText;
				}
				if (speedNode != null)
				{
					monsterModel.Speed = speedNode.InnerText;
				}
				if (strNode != null)
				{
					int parseDummy = 0;
					if (Int32.TryParse(strNode.InnerText, out parseDummy))
					{
						monsterModel.Strength = parseDummy;
					}
				}
				if (dexNode != null)
				{
					int parseDummy = 0;
					if (Int32.TryParse(dexNode.InnerText, out parseDummy))
					{
						monsterModel.Dexterity = parseDummy;
					}
				}
				if (conNode != null)
				{
					int parseDummy = 0;
					if (Int32.TryParse(conNode.InnerText, out parseDummy))
					{
						monsterModel.Constitution = parseDummy;
					}
				}
				if (intNode != null)
				{
					int parseDummy = 0;
					if (Int32.TryParse(intNode.InnerText, out parseDummy))
					{
						monsterModel.Intelligence = parseDummy;
					}
				}
				if (wisNode != null)
				{
					int parseDummy = 0;
					if (Int32.TryParse(wisNode.InnerText, out parseDummy))
					{
						monsterModel.Wisdom = parseDummy;
					}
				}
				if (chaNode != null)
				{
					int parseDummy = 0;
					if (Int32.TryParse(chaNode.InnerText, out parseDummy))
					{
						monsterModel.Charisma = parseDummy;
					}
				}

				if (descriptionNode != null)
				{
					monsterModel.Description = descriptionNode.InnerText;
				}

				if (saveNode != null)
				{
					string[] saves = saveNode.InnerText.Split(',');
					foreach (string s in saves)
					{
						string[] abilAndAmt = s.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
						if (abilAndAmt.Length == 2)
						{
							Ability ability = _stringService.GetEnum<Ability>(abilAndAmt[0]);
                            if (Int32.TryParse(abilAndAmt[1].Trim(), out int value))
                            {
                                monsterModel.Saves.Add(ability, value);
                            }
						}
					}
				}

				if (skillNode != null)
				{
					string[] skills = skillNode.InnerText.Split(',');
					foreach (string s in skills)
					{
						string[] skillAndAmt = s.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
						if (skillAndAmt.Length == 2)
						{
							Skill skill = _stringService.GetEnum<Skill>(skillAndAmt[0].Trim());
                            if (Int32.TryParse(skillAndAmt[1].Trim(), out int value))
                            {
                                monsterModel.Skills.Add(skill, value);
                            }
						}
					}
				}

				if (resistNode != null)
				{
					monsterModel.Resistances = resistNode.InnerText;
				}

				if (vulnerableNode != null)
				{
					monsterModel.Vulnerabilities = vulnerableNode.InnerText;
				}

				if (immuneNode != null)
				{
					monsterModel.Immunities = immuneNode.InnerText;
				}

				if (conditionImmuneNode != null)
				{
					monsterModel.ConditionImmunities = conditionImmuneNode.InnerText;
				}

				if (sensesNode != null)
				{
					monsterModel.Senses = sensesNode.InnerText;
				}

				if (passiveNode != null)
				{
                    int parseDummy = 0;
					if (Int32.TryParse(passiveNode.InnerText, out parseDummy))
					{
						monsterModel.PassivePerception = parseDummy;
					}
				}

				if (languagesNode != null)
				{
					foreach (string language in languagesNode.InnerText.Split(','))
					{
						if (!String.IsNullOrWhiteSpace(language))
						{
							monsterModel.Languages.Add(language.Trim());
							CheckLanguage(language.Trim());
						}
					}
				}

				if (crNode != null)
				{
					monsterModel.CR = crNode.InnerText;
				}

				if (spellsNode != null)
				{
					foreach (string spell in spellsNode.InnerText.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
					{
						monsterModel.Spells.Add(_stringService.CapitalizeWords(spell.Trim()));
					}
				}

				if (slotsNode != null)
				{
					foreach (string slot in slotsNode.InnerText.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
					{
						monsterModel.SpellSlots.Add(slot.Trim());
					}
				}

				if (environmentNode != null)
				{
					monsterModel.Environment = environmentNode.InnerText;
				}

				monsterModel.Traits = GetTraits(traitNodes);

				foreach (XmlNode actionNode in actionNodes)
				{
					XmlNode actionNameNode = actionNode["name"];
					XmlNodeList actionTextNodes = actionNode.SelectNodes("text");
					XmlNodeList actionAttackNodes = actionNode.SelectNodes("attack");

					if (actionNameNode != null && !String.IsNullOrWhiteSpace(actionNameNode.InnerText) && actionTextNodes.Count > 0)
					{
						MonsterActionModel monsterActionModel = new MonsterActionModel();

						monsterActionModel.Name = actionNameNode.InnerText;

						foreach (XmlNode actionTextNode in actionTextNodes)
						{
							monsterActionModel.TextCollection.Add(actionTextNode.InnerText);
						}

						foreach (XmlNode actionAttackNode in actionAttackNodes)
						{
							string[] attackParts = actionAttackNode.InnerText.Split('|');
							if (attackParts.Length > 2 && !String.IsNullOrWhiteSpace(attackParts[0]))
							{
								MonsterAttackModel monsterAttackModel = new MonsterAttackModel();

								monsterAttackModel.Name = attackParts[0];
								monsterAttackModel.ToHit = attackParts[1];
								monsterAttackModel.Roll = attackParts[2];

								monsterActionModel.Attacks.Add(monsterAttackModel);
							}
						}

						monsterModel.Actions.Add(monsterActionModel);
					}
				}

                foreach (XmlNode reactionNode in reactionNodes)
                {
                    XmlNode reactionNameNode = reactionNode["name"];
                    XmlNodeList reactionTextNodes = reactionNode.SelectNodes("text");
                    XmlNodeList reactionAttackNodes = reactionNode.SelectNodes("attack");

                    if (reactionNameNode != null && !String.IsNullOrWhiteSpace(reactionNameNode.InnerText) && reactionTextNodes.Count > 0)
                    {
                        MonsterActionModel monsterActionModel = new MonsterActionModel();

                        monsterActionModel.Name = reactionNameNode.InnerText;

                        foreach (XmlNode actionTextNode in reactionTextNodes)
                        {
                            monsterActionModel.TextCollection.Add(actionTextNode.InnerText);
                        }

                        foreach (XmlNode actionAttackNode in reactionAttackNodes)
                        {
                            string[] attackParts = actionAttackNode.InnerText.Split('|');
                            if (attackParts.Length > 2 && !String.IsNullOrWhiteSpace(attackParts[0]))
                            {
                                MonsterAttackModel monsterAttackModel = new MonsterAttackModel();

                                monsterAttackModel.Name = attackParts[0];
                                monsterAttackModel.ToHit = attackParts[1];
                                monsterAttackModel.Roll = attackParts[2];

                                monsterActionModel.Attacks.Add(monsterAttackModel);
                            }
                        }

                        monsterModel.Reactions.Add(monsterActionModel);
                    }
                }

                foreach (XmlNode legendaryNode in legendaryNodes)
				{
					XmlNode legendaryNameNode = legendaryNode["name"];
					XmlNodeList legendaryTextNodes = legendaryNode.SelectNodes("text");
					XmlNodeList legendaryAttackNodes = legendaryNode.SelectNodes("attack");

					if (legendaryNameNode != null && !String.IsNullOrWhiteSpace(legendaryNameNode.InnerText) && legendaryTextNodes.Count > 0)
					{
						MonsterActionModel monsterActionModel = new MonsterActionModel() { IsLegendary = true };

						monsterActionModel.Name = legendaryNameNode.InnerText;

						foreach (XmlNode legendaryTextNode in legendaryTextNodes)
						{
							monsterActionModel.TextCollection.Add(legendaryTextNode.InnerText);
						}

						foreach (XmlNode legendaryAttackNode in legendaryAttackNodes)
						{
							string[] attackParts = legendaryAttackNode.InnerText.Split('|');
							if (attackParts.Length > 2 && !String.IsNullOrWhiteSpace(attackParts[0]))
							{
								MonsterAttackModel monsterAttackModel = new MonsterAttackModel();

								monsterAttackModel.Name = attackParts[0];
								monsterAttackModel.ToHit = attackParts[1];
								monsterAttackModel.Roll = attackParts[2];

								monsterActionModel.Attacks.Add(monsterAttackModel);
							}
						}

						monsterModel.LegendaryActions.Add(monsterActionModel);
					}
				}

				monsterModel.XML = FormatXML(monsterNode);
			}

			return monsterModel;
		}

		/// <summary>
		/// Reads races
		/// </summary>
		public List<RaceModel> ReadRaces()
		{
			List<RaceModel> races = new List<RaceModel>();

            if (_doc.DocumentElement.Name.ToLower() == "race")
            {
                RaceModel raceModel = GetRace(_doc.DocumentElement);
                if (raceModel != null)
                {
                    races.Add(raceModel);
                }
            }
            else
            {
                foreach (XmlNode raceNode in _doc.DocumentElement.SelectNodes("descendant::race"))
                {
                    RaceModel raceModel = GetRace(raceNode);
                    if (raceModel != null)
                    {
                        races.Add(raceModel);
                    }
                }
            }

			return races.OrderBy(x => x.Name).ToList();
		}

        /// <summary>
		/// Reads races
		/// </summary>
		public List<RaceModel> ReadRaces(string xml)
        {
            List<RaceModel> races = new List<RaceModel>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode node in doc.DocumentElement.SelectNodes("descendant::race"))
            {
                RaceModel race = GetRace(node);

                if (race != null)
                {
                    races.Add(race);
                }
            }

            return races.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Gets a race model from the xml
        /// </summary>
        public RaceModel GetRace(string xml)
		{
			string enclosedXML = "<race>" + xml + "</race>";
			XmlDocument document = new XmlDocument();
			document.LoadXml(enclosedXML);
			return GetRace(document.SelectSingleNode("race"));
		}

		/// <summary>
		/// Gets a race model from the xml node
		/// </summary>
		public RaceModel GetRace(XmlNode raceNode)
		{
			RaceModel raceModel = null;

            XmlNode idNode = raceNode["id"];
			XmlNode nameNode = raceNode["name"];
			XmlNode sizeNode = raceNode["size"];
			XmlNode speedNode = raceNode["speed"];
			XmlNode abilityNode = raceNode["ability"];
			XmlNode proficiencyNode = raceNode["proficiency"];
			XmlNode languagesNode = raceNode["languages"];
			XmlNode weaponsNode = raceNode["weapons"];
			XmlNode armorNode = raceNode["armor"];
			XmlNode toolsNode = raceNode["tools"];
			XmlNodeList traitNodes = raceNode.SelectNodes("trait");

			if (nameNode != null && sizeNode != null && speedNode != null && traitNodes.Count > 0)
			{
				raceModel = new RaceModel();

                if (idNode != null)
                {
                    if (Guid.TryParse(idNode.InnerText, out Guid id))
                    {
                        raceModel.ID = id;
                    }
                    raceNode.RemoveChild(idNode);
                }

                raceModel.Name = nameNode.InnerText;
				raceModel.Size = _stringService.GetEnum<CreatureSize>(sizeNode.InnerText);

				int parseDummy = 0;
				if (Int32.TryParse(speedNode.InnerText, out parseDummy))
				{
					raceModel.WalkSpeed = parseDummy;
				}

				if (abilityNode != null)
				{
					string[] abilityStrings = abilityNode.InnerText.Split(',');
					foreach (string abilityString in abilityStrings)
					{
						string[] abilityAndValue = abilityString.Trim().Split(' ');
						if (abilityAndValue.Length == 2)
						{
							Ability ability = _stringService.GetEnum<Ability>(abilityAndValue[0]);
                            if (Int32.TryParse(abilityAndValue[1].Trim(), out int value))
                            {
                                raceModel.Abilities.Add(ability, value);
                            }
						}
					}
				}

				if (proficiencyNode != null)
				{
					string[] proficiencies = proficiencyNode.InnerText.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string proficiency in proficiencies)
					{
						Skill skill = _stringService.GetEnum<Skill>(proficiency);
						raceModel.SkillProficiencies.Add(skill);
					}
				}

				if (languagesNode != null)
				{
					string[] languages = languagesNode.InnerText.Split(',');
					foreach (string language in languages)
					{
						string formattedLanguage = _stringService.CapitalizeWords(language.Trim());
						raceModel.Languages.Add(formattedLanguage);

						CheckLanguage(formattedLanguage);
					}
				}

				if (weaponsNode != null)
				{
					string[] weapons = weaponsNode.InnerText.Split(',');
					foreach (string weapon in weapons)
					{
						raceModel.WeaponProficiencies.Add(_stringService.CapitalizeWords(weapon.Trim()));
					}
				}

				if (armorNode != null)
				{
					string[] armors = armorNode.InnerText.Split(',');
					foreach (string armor in armors)
					{
						raceModel.ArmorProficiencies.Add(_stringService.CapitalizeWords(armor.Trim()));
					}
				}

				if (toolsNode != null)
				{
					string[] tools = toolsNode.InnerText.Split(',');
					foreach (string tool in tools)
					{
						raceModel.ToolProficiencies.Add(_stringService.CapitalizeWords(tool.Trim()));
					}
				}

				raceModel.Traits = GetTraits(traitNodes);

				raceModel.XML = FormatXML(raceNode);
			}

			return raceModel;
		}

		/// <summary>
		/// Reads spells
		/// </summary>
		public List<SpellModel> ReadSpells()
		{
			List<SpellModel> spells = new List<SpellModel>();

            if (_doc.DocumentElement.Name.ToLower() == "spell")
            {
                SpellModel spellModel = GetSpell(_doc.DocumentElement);
                if (spellModel != null)
                {
                    spells.Add(spellModel);
                }
            }
            else
            {
                foreach (XmlNode spellNode in _doc.DocumentElement.SelectNodes("descendant::spell"))
                {
                    SpellModel spellModel = GetSpell(spellNode);
                    if (spellModel != null)
                    {
                        spells.Add(spellModel);
                    }
                }
            }

			//return spells.OrderBy(x => x.Level).ThenBy(x => x.Name).ToList();
			return spells.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
		/// Reads spells
		/// </summary>
		public List<SpellModel> ReadSpells(string xml)
        {
            List<SpellModel> spells = new List<SpellModel>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode node in doc.DocumentElement.SelectNodes("descendant::spell"))
            {
                SpellModel spell = GetSpell(node);

                if (spell != null)
                {
                    spells.Add(spell);
                }
            }

            return spells.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Gets a spell model from the xml
        /// </summary>
        public SpellModel GetSpell(string xml)
		{
			string enclosedXML = "<spell>" + xml + "</spell>";
			XmlDocument document = new XmlDocument();
			document.LoadXml(enclosedXML);
			return GetSpell(document.SelectSingleNode("spell"));
		}

		/// <summary>
		/// Gets a spell model from the xml node
		/// </summary>
		public SpellModel GetSpell(XmlNode spellNode)
		{
			SpellModel spellModel = null;

            XmlNode idNode = spellNode["id"];
			XmlNode nameNode = spellNode["name"];
			XmlNode levelNode = spellNode["level"];
			XmlNode schoolNode = spellNode["school"];
			XmlNode ritualNode = spellNode["ritual"];
			XmlNode timeNode = spellNode["time"];
			XmlNode rangeNode = spellNode["range"];
			XmlNode componentsNode = spellNode["components"];
			XmlNode durationNode = spellNode["duration"];
			XmlNode classesNode = spellNode["classes"];
			XmlNodeList textNodes = spellNode.SelectNodes("text");
			XmlNodeList rollNodes = spellNode.SelectNodes("roll");

			if (nameNode != null && levelNode != null)
			{
				spellModel = new SpellModel();

                if (idNode != null)
                {
                    if (Guid.TryParse(idNode.InnerText, out Guid id))
                    {
                        spellModel.ID = id;
                    }
                    spellNode.RemoveChild(idNode);
                }

                spellModel.Name = nameNode.InnerText.Trim();

				int parseDummy = 0;
                if (Int32.TryParse(levelNode.InnerText, out parseDummy))
                {
                    spellModel.Level = parseDummy;
                }
                else
                {
                    spellModel.Level = -1;
                }

                if (schoolNode != null)
                {
                    spellModel.SpellSchool = _stringService.GetEnum<SpellSchool>(schoolNode.InnerText);
                }
                else
                {
                    spellModel.SpellSchool = SpellSchool.None;
                }

				if (ritualNode != null)
				{
					spellModel.IsRitual = ritualNode.InnerText.ToLower() == "yes";
				}

				if (timeNode != null)
				{
					spellModel.CastingTime = timeNode.InnerText;
				}

				if (rangeNode != null)
				{
					spellModel.Range = rangeNode.InnerText;
				}

				if (componentsNode != null)
				{
					spellModel.Components = componentsNode.InnerText;
				}

				if (durationNode != null)
				{
					spellModel.Duration = durationNode.InnerText;
				}

				if (classesNode != null)
				{
					spellModel.Classes = classesNode.InnerText;
				}

				foreach (XmlNode textNode in textNodes)
				{
					if (!String.IsNullOrEmpty(textNode.InnerText))
					{
						spellModel.TextCollection.Add(textNode.InnerText);
					}
				}

				foreach (XmlNode rollNode in rollNodes)
				{
					if (!String.IsNullOrEmpty(rollNode.InnerText))
					{
						spellModel.Rolls.Add(rollNode.InnerText);
					}
				}

				spellModel.XML = FormatXML(spellNode);
			}

			return spellModel;
		}

		#endregion

		#region Private Methods

		void CheckLanguage(string str)
		{
			if (str.Length > 0 && Char.IsUpper(str[0]) && !str.Contains(" "))
			{
				if (!_languages.Any(x => x.Name.ToLower() == str.ToLower()))
				{
					_languages.Add(new LanguageModel() { Name = str });
				}
			}
			else
			{
				string[] languages = new string[] { };

				if (str.ToLower().Contains("you can speak, read, and write "))
				{
					string[] sentences = str.Split('.');
					if (sentences.Length > 0)
					{
						string first = sentences[0].ToLower().Replace("you can speak, read, and write ", "");

						if (first.ToLower().Contains("you can read and write "))
						{
							first = first.ToLower().Replace("you can read and write ", "");
						}

						if (first.Contains(','))
						{
							languages = first.Split(',');
						}
						else if (first.Contains("and"))
						{
							languages = first.Split(new string[] { "and" }, StringSplitOptions.RemoveEmptyEntries);
						}
					}
				}
				else if (str.ToLower().Contains("languages:"))
				{
					string langugeString = str.Substring(str.ToLower().IndexOf("languages:"));
					langugeString = langugeString.ToLower().Replace("languages:", "").Trim();

					if (langugeString.Contains(Environment.NewLine))
					{
						langugeString = langugeString.Substring(0, langugeString.IndexOf(Environment.NewLine));
					}
					else if (langugeString.Contains("\r\n"))
					{
						langugeString = langugeString.Substring(0, langugeString.IndexOf("\r\n"));
					}

					if (langugeString.Contains("(") && langugeString.Contains(")") && langugeString.Contains(','))
					{
						langugeString = langugeString.Substring(langugeString.IndexOf("(") + 1);
						langugeString = langugeString.Substring(0, langugeString.IndexOf(")"));
						languages = langugeString.Split(',');
					}
					else if (langugeString.Contains(','))
					{
						languages = langugeString.Split(',');
					}
					else if (langugeString.Contains("and"))
					{
						languages = langugeString.Split(new string[] { "and" }, StringSplitOptions.RemoveEmptyEntries);
					}
					else if (!langugeString.Contains(" ") && langugeString.ToLower() != "none")
					{
						languages = new string[] { langugeString };
					}
				}

				foreach (string lang in languages)
				{
					string langFormated = lang.Replace(",", "").Replace("and ", "").Replace("or ", "").Trim();
					if (langFormated.Length > 1 && !langFormated.Contains(" "))
					{
						langFormated = _stringService.CapitalizeWords(langFormated);
						if (!_languages.Any(x => x.Name.ToLower() == langFormated.ToLower()))
						{
							_languages.Add(new LanguageModel() { Name = langFormated });
						}
					}
				}
			}
		}

		private List<TraitModel> GetTraits(XmlNodeList traitNodes)
		{
			List<TraitModel> traitModels = new List<TraitModel>();

			foreach (XmlNode traitNode in traitNodes)
			{
				XmlNode tName = traitNode["name"];
				XmlNodeList tTextNodes = traitNode.SelectNodes("text");

				if (tName != null && !String.IsNullOrWhiteSpace(tName.InnerText) && tTextNodes.Count > 0)
				{
					TraitModel traitModel = new TraitModel();

					traitModel.Name = tName.InnerText;

					foreach (XmlNode ttNode in tTextNodes)
					{
						if (ttNode != null)
						{
							traitModel.TextCollection.Add(ttNode.InnerText);
						}
					}

                    if (traitModel.TextCollection.Count > 0 && 
                        String.IsNullOrWhiteSpace(traitModel.TextCollection.Last()))
                    {
                        traitModel.TextCollection.Remove(traitModel.TextCollection.Last());
                    }

					string lowerName = traitModel.Name.ToLower();

					if (lowerName == "languages" || lowerName == "language")
					{
						traitModel.TraitType = TraitType.Language;
					}
					else if (lowerName == "tool proficiencies" || lowerName == "tool proficiency")
					{
						traitModel.TraitType = TraitType.Tool_Proficiency;
					}
					else if (lowerName == "skill proficiencies" || lowerName == "skill proficiency")
					{
						traitModel.TraitType = TraitType.Skill_Proficiency;
					}
					else if (lowerName == "starting proficiencies" || lowerName == "starting proficiency")
					{
						traitModel.TraitType = TraitType.Starting_Proficiency;
					}
					else
					{
						traitModel.TraitType = TraitType.Information;
					}

					if (traitModel.TraitType == TraitType.Language || traitModel.TraitType == TraitType.Starting_Proficiency)
					{
						CheckLanguage(traitModel.Text);
					}

					traitModels.Add(traitModel);
				}
			}

			return traitModels;
		}

		private List<ModifierModel> GetModifiers(XmlNodeList modifierNodes)
		{
			List<ModifierModel> modifierModels = new List<ModifierModel>();

			foreach (XmlNode modNode in modifierNodes)
			{
				try
				{
					string modFullText = modNode.InnerText;
					XmlAttribute modCategoryAttribute = modNode.Attributes["category"];

					if (modCategoryAttribute != null)
					{
						ModifierModel modifierModel = new ModifierModel();

						modifierModel.ModifierCategory = _stringService.GetEnum<ModifierCategory>(modCategoryAttribute.Value);
						modifierModel.Ability = Ability.None;

						if (modFullText.Contains("+"))
						{
							string[] modPieces = modFullText.Split('+');
							if (modPieces.Length == 2)
							{
								modifierModel.Text = _stringService.CapitalizeWords(modPieces[0]);
                                if (Int32.TryParse(modPieces[1], out int value))
                                {
                                    modifierModel.Value = value;
                                }
							}
						}

						if (modifierModel.ModifierCategory == ModifierCategory.Ability_Score)
						{
							modifierModel.Ability = _stringService.GetEnum<Ability>(modifierModel.Text);
						}

						modifierModels.Add(modifierModel);
					}
				}
				catch (Exception ex)
				{
					// log "error parsing modifier" with modNode.InnerText and exception
				}
			}

			return modifierModels;
		}

		#endregion
	}
}
