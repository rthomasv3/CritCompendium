using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Persistence;

namespace CritCompendiumInfrastructure.Business
{
   public class Compendium
   {
      #region Events

      public event EventHandler ImportComplete;
      public event EventHandler<CompendiumChangeEventArgs> CharacterChanged;
      public event EventHandler<CompendiumChangeEventArgs> EncounterChanged;
      public event EventHandler TagsChanged;

      #endregion

      #region Fields

      private readonly IDataManager _dataManager;

      private readonly List<string> _simpleWeapons = new List<string> { "Club", "Dagger", "Greatclub", "Handaxe", "Javelin", "Light hammer", "Mace", "Quarterstaff",
                                                                          "Sickle", "Spear", "Crossbow, light", "Dart", "Shortbow", "Sling" };
      private readonly List<string> _martialWeapons = new List<string> { "Battleaxe", "Flail", "Glaive", "Greataxe", "Greatsword", "Halberd", "Lance", "Longsword",
                                                                           "Maul", "Morningstar", "Pike", "Rapier", "Scimitar", "Shortsword", "Trident", "War pick",
                                                                           "Warhammer", "Whip", "Blowgun", "Crossbow, hand", "Crossbow, heavy", "Longsword", "Net" };
      private readonly List<string> _lightArmor = new List<string> { "Padded", "Leather", "Studded leather" };
      private readonly List<string> _mediumArmor = new List<string> { "Hide", "Chain shirt", "Scale mail", "Breastplate", "Half plate" };
      private readonly List<string> _heavyArmor = new List<string> { "Ring mail", "Chain mail", "Splint", "Plate" };
      private readonly List<string> _shields = new List<string> { "Shield" };
      private readonly List<string> _artisansTools = new List<string> { "Alchemist's supplies", "Brewer's supplies", "Calligrapher's supplies", "Carpenter's tools",
                                                                          "Cartographer's tools",  "Cobbler's tools", "Cook's utensils", "Glassblower's tools",
                                                                          "Jewelers's tools", "Leatherworker's tools", "Mason's tools", "Painter's supplies",
                                                                          "Potter's tools", "Smith's tools", "Tinker's tools", "Weaver's tools",
                                                                          "Woodcarver's tools" };
      private readonly List<string> _gamaingSets = new List<string> { "Dice set", "Playing card set" };
      private readonly List<string> _kits = new List<string> { "Disguise kit", "Forgery kit", "Herbalism kit", "Poisoner's kit " };
      private readonly List<string> _instruments = new List<string> { "Bagpipes", "Drum", "Dulcimer", "Flute", "Lute", "Lyre", "Horn", "Pan flute", "Shawm", "Viol" };
      private readonly List<string> _professionalTools = new List<string> { "Navigator's tools", "Thieves' tools" };
      private readonly List<string> _vehicles = new List<string> { "Land Vehicles", "Water Vehicles" };

      private List<CharacterModel> _characters = new List<CharacterModel>();
      private List<AdventureModel> _adventures = new List<AdventureModel>();
      private List<EncounterModel> _encounters = new List<EncounterModel>();
      private List<LocationModel> _locations = new List<LocationModel>();
      private List<NPCModel> _npcs = new List<NPCModel>();
      private List<RandomTableModel> _tables = new List<RandomTableModel>();
      private List<BackgroundModel> _backgrounds = new List<BackgroundModel>();
      private List<ClassModel> _classes = new List<ClassModel>();
      private List<ConditionModel> _conditions = new List<ConditionModel>();
      private List<FeatModel> _feats = new List<FeatModel>();
      private List<ItemModel> _items = new List<ItemModel>();
      private List<LanguageModel> _languages = new List<LanguageModel>();
      private List<MonsterModel> _monsters = new List<MonsterModel>();
      private List<RaceModel> _races = new List<RaceModel>();
      private List<SpellModel> _spells = new List<SpellModel>();

      private List<string> _tags = new List<string>();

      private List<PackModel> _packs = new List<PackModel>();

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a new instance of <see cref="Compendium"/>
      /// </summary>
      public Compendium(IDataManager dataManager)
      {
         _dataManager = dataManager;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets characters
      /// </summary>
      public List<CharacterModel> Characters
      {
         get { return _characters; }
      }

      /// <summary>
      /// Gets adventures
      /// </summary>
      public List<AdventureModel> Adventures
      {
         get { return _adventures; }
      }

      /// <summary>
      /// Gets encounters
      /// </summary>
      public List<EncounterModel> Encounters
      {
         get { return _encounters; }
      }

      /// <summary>
      /// Gets locations
      /// </summary>
      public List<LocationModel> Locations
      {
         get { return _locations; }
      }

      /// <summary>
      /// Gets npcs
      /// </summary>
      public List<NPCModel> NPCs
      {
         get { return _npcs; }
      }

      /// <summary>
      /// Gets tables
      /// </summary>
      public List<RandomTableModel> Tables
      {
         get { return _tables; }
      }

      /// <summary>
      /// Gets backgrounds
      /// </summary>
      public List<BackgroundModel> Backgrounds
      {
         get { return _backgrounds; }
      }

      /// <summary>
      /// Gets classes
      /// </summary>
      public List<ClassModel> Classes
      {
         get { return _classes; }
      }

      /// <summary>
      /// Gets conditions
      /// </summary>
      public List<ConditionModel> Conditions
      {
         get { return _conditions; }
      }

      /// <summary>
      /// Gets feats
      /// </summary>
      public List<FeatModel> Feats
      {
         get { return _feats; }
      }

      /// <summary>
      /// Gets items
      /// </summary>
      public List<ItemModel> Items
      {
         get { return _items; }
      }

      /// <summary>
      /// Gets languages
      /// </summary>
      public List<LanguageModel> Languages
      {
         get { return _languages; }
      }

      /// <summary>
      /// Gets monsters
      /// </summary>
      public List<MonsterModel> Monsters
      {
         get { return _monsters; }
      }

      /// <summary>
      /// Gets races
      /// </summary>
      public List<RaceModel> Races
      {
         get { return _races; }
      }

      /// <summary>
      /// Gets spells
      /// </summary>
      public List<SpellModel> Spells
      {
         get { return _spells; }
      }

      /// <summary>
      /// Gets tags
      /// </summary>
      public List<string> Tags
      {
         get { return _tags; }
      }

      /// <summary>
      /// Gets simple weapons
      /// </summary>
      public List<string> SimpleWeapons
      {
         get { return _simpleWeapons; }
      }

      /// <summary>
      /// Gets martial weapons
      /// </summary>
      public List<string> MartialWeapons
      {
         get { return _martialWeapons; }
      }

      /// <summary>
      /// Gets light armor
      /// </summary>
      public List<string> LightArmor
      {
         get { return _lightArmor; }
      }

      /// <summary>
      /// Gets medium armor
      /// </summary>
      public List<string> MediumArmor
      {
         get { return _mediumArmor; }
      }

      /// <summary>
      /// Gets heavy armor
      /// </summary>
      public List<string> HeavyArmor
      {
         get { return _heavyArmor; }
      }

      /// <summary>
      /// Gets shields
      /// </summary>
      public List<string> Shields
      {
         get { return _shields; }
      }

      /// <summary>
      /// Gets artisans tools
      /// </summary>
      public List<string> ArtisansTools
      {
         get { return _artisansTools; }
      }

      /// <summary>
      /// Gets gamaing sets
      /// </summary>
      public List<string> GamaingSets
      {
         get { return _gamaingSets; }
      }

      /// <summary>
      /// Gets kits
      /// </summary>
      public List<string> Kits
      {
         get { return _kits; }
      }

      /// <summary>
      /// Gets instruments
      /// </summary>
      public List<string> Instruments
      {
         get { return _instruments; }
      }

      /// <summary>
      /// Gets professional tools
      /// </summary>
      public List<string> ProfessionalTools
      {
         get { return _professionalTools; }
      }

      /// <summary>
      /// Gets vehicles
      /// </summary>
      public List<string> Vehicles
      {
         get { return _vehicles; }
      }

      /// <summary>
      /// Gets packs
      /// </summary>
      public List<PackModel> Packs
      {
         get { return _packs; }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Loads the default compendium.
      /// </summary>
      public void LoadDefaultCompendium()
      {
         XMLImporter importer = DependencyResolver.Resolve<XMLImporter>();
         importer.LoadManifestResourceXML("default_compendium.xml");

         _backgrounds = importer.ReadBackgrounds();
         _classes = importer.ReadClasses();
         _conditions = importer.ReadConditions();
         _feats = importer.ReadFeats();
         _items = importer.ReadItems();
         _monsters = importer.ReadMonsters();
         _races = importer.ReadRaces();
         _spells = importer.ReadSpells();

         _languages = importer.LanguagesFound;
      }

      public void SetCompendium(IEnumerable<BackgroundModel> backgrounds, IEnumerable<ClassModel> classes, IEnumerable<ConditionModel> conditions, IEnumerable<FeatModel> feats,
          IEnumerable<ItemModel> items, IEnumerable<LanguageModel> languages, IEnumerable<MonsterModel> monsters, IEnumerable<RaceModel> races, IEnumerable<SpellModel> spells)
      {
         _backgrounds = backgrounds.ToList();
         _classes = classes.ToList();
         _conditions = conditions.ToList();
         _feats = feats.ToList();
         _items = items.ToList();
         _languages = languages.ToList();
         _monsters = monsters.ToList();
         _races = races.ToList();
         _spells = spells.ToList();
      }

      /// <summary>
      /// Loads the compendium
      /// </summary>
      public void LoadCompendium()
      {
         if (_dataManager.FirstLaunch)
         {
            LoadDefaultCompendium();

            SaveBackgrounds();
            SaveClasses();
            SaveConditions();
            SaveFeats();
            SaveItems();
            SaveLanguages();
            SaveMonsters();
            SaveRaces();
            SaveSpells();
         }
         else
         {
            _backgrounds = _dataManager.LoadBackgrounds().ToList();
            _classes = _dataManager.LoadClasses().ToList();
            _conditions = _dataManager.LoadConditions().ToList();
            _feats = _dataManager.LoadFeats().ToList();
            _items = _dataManager.LoadItems().ToList();
            _languages = _dataManager.LoadLanguages().ToList();
            _monsters = _dataManager.LoadMonsters().ToList();
            _races = _dataManager.LoadRacess().ToList();
            _spells = _dataManager.LoadSpells().ToList();

            _characters = _dataManager.LoadCharacters(_backgrounds, _classes, _conditions, _feats, _items, _languages, _monsters, _races, _spells).ToList();
            _encounters = _dataManager.LoadEncounters(_characters, _conditions, _monsters).ToList();
            _locations = _dataManager.LoadLocations().ToList();
            _npcs = _dataManager.LoadNPCs().ToList();
            _adventures = _dataManager.LoadAdventures(_npcs, _locations, _encounters).ToList();
            _tables = _dataManager.LoadTables().ToList();
         }

         PopulatePacks();

         UpdateTags();
      }

      /// <summary>
      /// Adds a character
      /// </summary>
      public void AddCharacter(CharacterModel characterModel)
      {
         if (!_characters.Any(x => x.ID == characterModel.ID))
         {
            _characters.Add(characterModel);
         }
      }

      /// <summary>
      /// Deletes character with matching id
      /// </summary>
      public void DeleteCharacter(Guid id)
      {
         if (_characters.Any(x => x.ID == id))
         {
            _characters.RemoveAll(x => x.ID == id);
         }
      }

      /// <summary>
      /// Updates the character with id matching the parameter's id
      /// </summary>
      public void UpdateCharacter(CharacterModel model)
      {
         CharacterModel currentModel = _characters.FirstOrDefault(x => x.ID == model.ID);
         if (currentModel != null)
         {
            string oldName = currentModel.Name;

            _characters[_characters.IndexOf(currentModel)] = model;

            List<Guid> encounterIDs = new List<Guid>();
            foreach (EncounterModel encounter in _encounters)
            {
               foreach (EncounterCharacterModel encounterCharacter in encounter.Creatures.Where(x => x is EncounterCharacterModel))
               {
                  if (encounterCharacter.CharacterModel != null && encounterCharacter.CharacterModel.ID == model.ID)
                  {
                     encounterCharacter.CharacterModel = model;
                     if (encounterCharacter.Name.Equals(oldName, StringComparison.CurrentCultureIgnoreCase))
                     {
                        encounterCharacter.Name = model.Name;
                     }
                     encounterIDs.Add(encounter.ID);
                  }
               }
            }

            if (encounterIDs.Any())
            {
               EncounterChanged?.Invoke(this, new CompendiumChangeEventArgs(encounterIDs.Distinct()));
            }
         }
      }

      /// <summary>
      /// Adds an adventure
      /// </summary>
      public void AddAdventure(AdventureModel adventureModel)
      {
         if (!_adventures.Any(x => x.ID == adventureModel.ID))
         {
            _adventures.Add(adventureModel);

            //UpdateTags();
         }
      }

      /// <summary>
      /// Deletes adventure with matching id
      /// </summary>
      public void DeleteAdventure(Guid id)
      {
         if (_adventures.Any(x => x.ID == id))
         {
            _adventures.RemoveAll(x => x.ID == id);

            UpdateTags();
         }
      }

      /// <summary>
      /// Updates the adventure with id matching the parameter's id
      /// </summary>
      public void UpdateAdventure(AdventureModel model)
      {
         AdventureModel currentModel = _adventures.FirstOrDefault(x => x.ID == model.ID);
         if (currentModel != null)
         {
            _adventures[_adventures.IndexOf(currentModel)] = model;

            UpdateTags();
         }
      }

      /// <summary>
      /// Adds an encounter
      /// </summary>
      public void AddEncounter(EncounterModel encounterModel)
      {
         if (!_encounters.Any(x => x.ID == encounterModel.ID))
         {
            _encounters.Add(encounterModel);
         }
      }

      /// <summary>
      /// Deletes encounter with matching id
      /// </summary>
      public void DeleteEncounter(Guid id)
      {
         if (_encounters.Any(x => x.ID == id))
         {
            _encounters.RemoveAll(x => x.ID == id);
         }
      }

      /// <summary>
      /// Updates the encounter with id matching the parameter's id
      /// </summary>
      public void UpdateEncounter(EncounterModel model)
      {
         EncounterModel currentModel = _encounters.FirstOrDefault(x => x.ID == model.ID);
         if (currentModel != null)
         {
            _encounters[_encounters.IndexOf(currentModel)] = model;
         }
      }

      /// <summary>
      /// Adds a location
      /// </summary>
      public void AddLocation(LocationModel locationModel)
      {
         if (!_locations.Any(x => x.ID == locationModel.ID))
         {
            _locations.Add(locationModel);

            //UpdateTags();
         }
      }

      /// <summary>
      /// Deletes location with matching id
      /// </summary>
      public void DeleteLocation(Guid id)
      {
         if (_locations.Any(x => x.ID == id))
         {
            _locations.RemoveAll(x => x.ID == id);

            UpdateTags();
         }
      }

      /// <summary>
      /// Updates the location with id matching the parameter's id
      /// </summary>
      public void UpdateLocation(LocationModel model)
      {
         LocationModel currentModel = _locations.FirstOrDefault(x => x.ID == model.ID);
         if (currentModel != null)
         {
            _locations[_locations.IndexOf(currentModel)] = model;

            UpdateTags();
         }
      }

      /// <summary>
      /// Adds an npc
      /// </summary>
      public void AddNPC(NPCModel npcModel)
      {
         if (!_npcs.Any(x => x.ID == npcModel.ID))
         {
            _npcs.Add(npcModel);

            //UpdateTags();
         }
      }

      /// <summary>
      /// Deletes npc with matching id
      /// </summary>
      public void DeleteNPC(Guid id)
      {
         if (_npcs.Any(x => x.ID == id))
         {
            _npcs.RemoveAll(x => x.ID == id);

            UpdateTags();
         }
      }

      /// <summary>
      /// Updates the npc with id matching the parameter's id
      /// </summary>
      public void UpdateNPC(NPCModel model)
      {
         NPCModel currentModel = _npcs.FirstOrDefault(x => x.ID == model.ID);
         if (currentModel != null)
         {
            _npcs[_npcs.IndexOf(currentModel)] = model;

            UpdateTags();
         }
      }

      /// <summary>
      /// Adds a table
      /// </summary>
      public void AddTable(RandomTableModel randomTableModel)
      {
         if (!_tables.Any(x => x.ID == randomTableModel.ID))
         {
            _tables.Add(randomTableModel);

            //UpdateTags();
         }
      }

      /// <summary>
      /// Deletes table with matching id
      /// </summary>
      public void DeleteTable(Guid id)
      {
         if (_tables.Any(x => x.ID == id))
         {
            _tables.RemoveAll(x => x.ID == id);

            UpdateTags();
         }
      }

      /// <summary>
      /// Updates the table with id matching the parameter's id
      /// </summary>
      public void UpdateTable(RandomTableModel model)
      {
         RandomTableModel currentModel = _tables.FirstOrDefault(x => x.ID == model.ID);
         if (currentModel != null)
         {
            _tables[_tables.IndexOf(currentModel)] = model;

            UpdateTags();
         }
      }

      /// <summary>
      /// Adds a background model
      /// </summary>
      public void AddBackground(BackgroundModel backgroundModel)
      {
         if (backgroundModel != null &&
             !_backgrounds.Any(x => x.ID == backgroundModel.ID))
         {
            _backgrounds.Add(backgroundModel);
         }
      }

      /// <summary>
      /// Deletes the background from the compendium
      /// </summary>
      public void DeleteBackground(Guid id)
      {
         if (_backgrounds.Any(x => x.ID == id))
         {
            _backgrounds.RemoveAll(x => x.ID == id);
         }
      }

      /// <summary>
      /// Updates the compendium background with id matching the parameter's id
      /// </summary>
      public void UpdateBackground(BackgroundModel model)
      {
         BackgroundModel currentModel = _backgrounds.FirstOrDefault(x => x.ID == model.ID);
         if (currentModel != null)
         {
            _backgrounds[_backgrounds.IndexOf(currentModel)] = model;

            List<Guid> characterIDs = new List<Guid>();
            foreach (CharacterModel character in _characters)
            {
               if (character.Background.ID == model.ID)
               {
                  character.Background = model;
                  characterIDs.Add(character.ID);
               }
            }
            if (characterIDs.Any())
            {
               CharacterChanged?.Invoke(this, new CompendiumChangeEventArgs(characterIDs.Distinct()));
            }
         }
      }

      /// <summary>
      /// Adds a class model
      /// </summary>
      public void AddClass(ClassModel classModel)
      {
         if (classModel != null &&
             !_classes.Any(x => x.ID == classModel.ID))
         {
            _classes.Add(classModel);
         }
      }

      /// <summary>
      /// Deletes the class from the compendium
      /// </summary>
      public void DeleteClass(Guid id)
      {
         if (_classes.Any(x => x.ID == id))
         {
            _classes.RemoveAll(x => x.ID == id);
         }
      }

      /// <summary>
      /// Updates the compendium class with id matching the parameter's id
      /// </summary>
      public void UpdateClass(ClassModel model)
      {
         ClassModel currentModel = _classes.FirstOrDefault(x => x.ID == model.ID);
         if (currentModel != null)
         {
            _classes[_classes.IndexOf(currentModel)] = model;

            List<Guid> characterIDs = new List<Guid>();
            foreach (CharacterModel character in _characters)
            {
               foreach (LevelModel level in character.Levels)
               {
                  if (level.Class.ID == model.ID)
                  {
                     level.Class = model;

                     IEnumerable<FeatureModel> allFeatures = model.AutoLevels.SelectMany(x => x.Features);
                     for (int i = 0; i < level.Features.Count; ++i)
                     {
                        FeatureModel feature = level.Features[i];
                        FeatureModel newFeature = allFeatures.FirstOrDefault(x => x.ID == feature.ID);
                        if (newFeature != null)
                        {
                           level.Features[i] = newFeature;
                        }
                        else
                        {
                           level.Features.Remove(feature);
                        }
                     }

                     characterIDs.Add(character.ID);
                  }
               }
            }
            if (characterIDs.Any())
            {
               CharacterChanged?.Invoke(this, new CompendiumChangeEventArgs(characterIDs.Distinct()));
            }
         }
      }

      /// <summary>
      /// Adds a condition model
      /// </summary>
      public void AddCondition(ConditionModel conditionModel)
      {
         if (conditionModel != null &&
             !_conditions.Any(x => x.ID == conditionModel.ID))
         {
            _conditions.Add(conditionModel);
         }
      }

      /// <summary>
      /// Deletes the condition from the compendium
      /// </summary>
      public void DeleteCondition(Guid id)
      {
         if (_conditions.Any(x => x.ID == id))
         {
            _conditions.RemoveAll(x => x.ID == id);
         }
      }

      /// <summary>
      /// Updates the compendium condition with id matching the parameter's id
      /// </summary>
      public void UpdateCondition(ConditionModel model)
      {
         ConditionModel currentModel = _conditions.FirstOrDefault(x => x.ID == model.ID);
         if (currentModel != null)
         {
            string oldName = currentModel.Name;

            _conditions[_conditions.IndexOf(currentModel)] = model;

            List<Guid> characterIDs = new List<Guid>();
            foreach (CharacterModel character in _characters)
            {
               foreach (AppliedConditionModel appliedCondition in character.Conditions)
               {
                  if (appliedCondition.ConditionModel != null && appliedCondition.ConditionModel.ID == model.ID)
                  {
                     appliedCondition.ConditionModel = model;
                     if (appliedCondition.Name.Equals(oldName, StringComparison.CurrentCultureIgnoreCase))
                     {
                        appliedCondition.Name = model.Name;
                     }
                     characterIDs.Add(character.ID);
                  }
               }
            }
            if (characterIDs.Any())
            {
               CharacterChanged?.Invoke(this, new CompendiumChangeEventArgs(characterIDs.Distinct()));
            }
         }
      }

      /// <summary>
      /// Adds a feat model
      /// </summary>
      public void AddFeat(FeatModel featModel)
      {
         if (featModel != null &&
             !_feats.Any(x => x.ID == featModel.ID))
         {
            _feats.Add(featModel);
         }
      }

      /// <summary>
      /// Deletes the feat from the compendium
      /// </summary>
      public void DeleteFeat(Guid id)
      {
         if (_feats.Any(x => x.ID == id))
         {
            _feats.RemoveAll(x => x.ID == id);
         }
      }

      /// <summary>
      /// Updates the compendium feat with id matching the parameter's id
      /// </summary>
      public void UpdateFeat(FeatModel model)
      {
         FeatModel currentModel = _feats.FirstOrDefault(x => x.ID == model.ID);
         if (currentModel != null)
         {
            _feats[_feats.IndexOf(currentModel)] = model;

            List<Guid> characterIDs = new List<Guid>();
            foreach (CharacterModel character in _characters)
            {
               foreach (LevelModel level in character.Levels)
               {
                  for (int i = 0; i < level.Feats.Count; ++i)
                  {
                     FeatModel feat = level.Feats[i];
                     if (feat.ID == model.ID)
                     {
                        level.Feats[i] = model;
                        characterIDs.Add(character.ID);
                     }
                  }
               }
            }
            if (characterIDs.Any())
            {
               CharacterChanged?.Invoke(this, new CompendiumChangeEventArgs(characterIDs.Distinct()));
            }
         }
      }

      /// <summary>
      /// Adds an item model
      /// </summary>
      public void AddItem(ItemModel itemModel)
      {
         if (itemModel != null &&
             !_items.Any(x => x.ID == itemModel.ID))
         {
            _items.Add(itemModel);
         }
      }

      /// <summary>
      /// Deletes the item from the compendium
      /// </summary>
      public void DeleteItem(Guid id)
      {
         if (_items.Any(x => x.ID == id))
         {
            _items.RemoveAll(x => x.ID == id);
         }
      }

      /// <summary>
      /// Updates the compendium item with id matching the parameter's id
      /// </summary>
      public void UpdateItem(ItemModel model)
      {
         ItemModel currentModel = _items.FirstOrDefault(x => x.ID == model.ID);
         if (currentModel != null)
         {
            string oldName = currentModel.Name;

            _items[_items.IndexOf(currentModel)] = model;

            List<Guid> characterIDs = new List<Guid>();
            foreach (CharacterModel character in _characters)
            {
               foreach (BagModel bag in character.Bags)
               {
                  foreach (EquipmentModel equipment in bag.Equipment)
                  {
                     if (equipment.Item != null && equipment.Item.ID == model.ID)
                     {
                        equipment.Item = model;
                        if (equipment.Name.Equals(oldName, StringComparison.CurrentCultureIgnoreCase))
                        {
                           equipment.Name = model.Name;
                        }
                        characterIDs.Add(character.ID);
                     }
                  }
               }
            }
            if (characterIDs.Any())
            {
               CharacterChanged?.Invoke(this, new CompendiumChangeEventArgs(characterIDs.Distinct()));
            }
         }
      }

      /// <summary>
      /// Adds a language model
      /// </summary>
      public void AddLanguage(LanguageModel languageModel)
      {
         if (languageModel != null &&
             !_languages.Any(x => x.ID == languageModel.ID))
         {
            _languages.Add(languageModel);
         }
      }

      /// <summary>
      /// Deletes the language from the compendium
      /// </summary>
      public void DeleteLanguage(Guid id)
      {
         if (_languages.Any(x => x.ID == id))
         {
            _languages.RemoveAll(x => x.ID == id);
         }
      }

      /// <summary>
      /// Updates the compendium language with id matching the parameter's id
      /// </summary>
      public void UpdateLanguage(LanguageModel model)
      {
         LanguageModel currentModel = _languages.FirstOrDefault(x => x.ID == model.ID);
         if (currentModel != null)
         {
            _languages[_languages.IndexOf(currentModel)] = model;
         }
      }

      /// <summary>
      /// Adds a monster model
      /// </summary>
      public void AddMonster(MonsterModel monsterModel)
      {
         if (monsterModel != null &&
             !_monsters.Any(x => x.ID == monsterModel.ID))
         {
            _monsters.Add(monsterModel);
         }
      }

      /// <summary>
      /// Deletes the monster from the compendium
      /// </summary>
      public void DeleteMonster(Guid id)
      {
         if (_monsters.Any(x => x.ID == id))
         {
            _monsters.RemoveAll(x => x.ID == id);
         }
      }

      /// <summary>
      /// Updates the compendium monster with id matching the parameter's id
      /// </summary>
      public void UpdateMonster(MonsterModel model)
      {
         MonsterModel currentModel = _monsters.FirstOrDefault(x => x.ID == model.ID);
         if (currentModel != null)
         {
            string oldName = currentModel.Name;

            _monsters[_monsters.IndexOf(currentModel)] = model;

            List<Guid> characterIDs = new List<Guid>();
            foreach (CharacterModel character in _characters)
            {
               foreach (CompanionModel companion in character.Companions)
               {
                  if (companion.MonsterModel != null && companion.MonsterModel.ID == model.ID)
                  {
                     companion.MonsterModel = model;
                     if (companion.Name.Equals(oldName, StringComparison.CurrentCultureIgnoreCase))
                     {
                        companion.Name = model.Name;
                     }
                     characterIDs.Add(character.ID);
                  }
               }
            }
            if (characterIDs.Any())
            {
               CharacterChanged?.Invoke(this, new CompendiumChangeEventArgs(characterIDs.Distinct()));
            }

            List<Guid> encounterIDs = new List<Guid>();
            foreach (EncounterModel encounter in _encounters)
            {
               foreach (EncounterMonsterModel encounterMonster in encounter.Creatures.Where(x => x is EncounterMonsterModel))
               {
                  if (encounterMonster.MonsterModel != null && encounterMonster.MonsterModel.ID == model.ID)
                  {
                     encounterMonster.MonsterModel = model;
                     if (encounter.Name.Equals(oldName, StringComparison.CurrentCultureIgnoreCase))
                     {
                        encounterMonster.Name = model.Name;
                     }
                     encounterIDs.Add(encounter.ID);
                  }
               }
            }
            if (encounterIDs.Any())
            {
               EncounterChanged?.Invoke(this, new CompendiumChangeEventArgs(encounterIDs.Distinct()));
            }
         }
      }

      /// <summary>
      /// Adds a race model
      /// </summary>
      public void AddRace(RaceModel raceModel)
      {
         if (raceModel != null &&
             !_races.Any(x => x.ID == raceModel.ID))
         {
            _races.Add(raceModel);
         }
      }

      /// <summary>
      /// Deletes the race from the compendium
      /// </summary>
      public void DeleteRace(Guid id)
      {
         if (_races.Any(x => x.ID == id))
         {
            _races.RemoveAll(x => x.ID == id);
         }
      }

      /// <summary>
      /// Updates the compendium race with id matching the parameter's id
      /// </summary>
      public void UpdateRace(RaceModel model)
      {
         RaceModel currentModel = _races.FirstOrDefault(x => x.ID == model.ID);
         if (currentModel != null)
         {
            _races[_races.IndexOf(currentModel)] = model;

            List<Guid> characterIDs = new List<Guid>();
            foreach (CharacterModel character in _characters)
            {
               if (character.Race.ID == model.ID)
               {
                  character.Race = model;
                  characterIDs.Add(character.ID);
               }
            }
            if (characterIDs.Any())
            {
               CharacterChanged?.Invoke(this, new CompendiumChangeEventArgs(characterIDs.Distinct()));
            }
         }
      }

      /// <summary>
      /// Adds a spell model
      /// </summary>
      public void AddSpell(SpellModel spellModel)
      {
         if (spellModel != null &&
             !_spells.Any(x => x.ID == spellModel.ID))
         {
            _spells.Add(spellModel);
         }
      }

      /// <summary>
      /// Deletes the spell from the compendium
      /// </summary>
      public void DeleteSpell(Guid id)
      {
         if (_spells.Any(x => x.ID == id))
         {
            _spells.RemoveAll(x => x.ID == id);
         }
      }

      /// <summary>
      /// Updates the compendium spell with id matching the parameter's id
      /// </summary>
      public void UpdateSpell(SpellModel model)
      {
         SpellModel currentModel = _spells.FirstOrDefault(x => x.ID == model.ID);
         if (currentModel != null)
         {
            _spells[_spells.IndexOf(currentModel)] = model;

            List<Guid> characterIDs = new List<Guid>();
            foreach (CharacterModel character in _characters)
            {
               foreach (SpellbookModel spellbook in character.Spellbooks)
               {
                  foreach (SpellbookEntryModel spellbookEntry in spellbook.Spells)
                  {
                     if (spellbookEntry.Spell != null && spellbookEntry.Spell.ID == model.ID)
                     {
                        spellbookEntry.Spell = model;
                        characterIDs.Add(character.ID);
                     }
                  }
               }
            }
            if (characterIDs.Any())
            {
               CharacterChanged?.Invoke(this, new CompendiumChangeEventArgs(characterIDs.Distinct()));
            }
         }
      }

      /// <summary>
      /// Saves to the default save location
      /// </summary>
      public void SaveCharacters()
      {
         _dataManager.SaveCharacters(_characters);
      }

      /// <summary>
      /// Saves to the default save location
      /// </summary>
      public void SaveAdventures()
      {
         _dataManager.SaveAdventures(_adventures);
      }

      /// <summary>
      /// Saves to the default save location
      /// </summary>
      public void SaveEncounters()
      {
         _dataManager.SaveEncounters(_encounters);
      }

      /// <summary>
      /// Saves to the default save location
      /// </summary>
      public void SaveLocations()
      {
         _dataManager.SaveLocations(_locations);
      }

      /// <summary>
      /// Saves to the default save location
      /// </summary>
      public void SaveNPCs()
      {
         _dataManager.SaveNPCs(_npcs);
      }

      /// <summary>
      /// Saves to the default save location
      /// </summary>
      public void SaveTables()
      {
         _dataManager.SaveTables(_tables);
      }

      /// <summary>
      /// Saves to the default save location
      /// </summary>
      public void SaveBackgrounds()
      {
         _dataManager.SaveBackgrounds(_backgrounds);
      }

      /// <summary>
      /// Saves to the default save location
      /// </summary>
      public void SaveClasses()
      {
         _dataManager.SaveClasses(_classes);
      }

      /// <summary>
      /// Saves to the default save location
      /// </summary>
      public void SaveConditions()
      {
         _dataManager.SaveConditions(_conditions);
      }

      /// <summary>
      /// Saves to the default save location
      /// </summary>
      public void SaveFeats()
      {
         _dataManager.SaveFeats(_feats);
      }

      /// <summary>
      /// Saves to the default save location
      /// </summary>
      public void SaveItems()
      {
         _dataManager.SaveItems(_items);
      }

      /// <summary>
      /// Saves to the default save location
      /// </summary>
      public void SaveLanguages()
      {
         _dataManager.SaveLanguages(_languages);
      }

      /// <summary>
      /// Saves to the default save location
      /// </summary>
      public void SaveMonsters()
      {
         _dataManager.SaveMonsters(_monsters);
      }

      /// <summary>
      /// Saves to the default save location
      /// </summary>
      public void SaveRaces()
      {
         _dataManager.SaveRaces(_races);
      }

      /// <summary>
      /// Saves to the default save location
      /// </summary>
      public void SaveSpells()
      {
         _dataManager.SaveSpells(_spells);
      }

      /// <summary>
      /// Invokes import complete event
      /// </summary>
      public void NotifyImportComplete()
      {
         ImportComplete?.Invoke(this, EventArgs.Empty);
      }

      #endregion

      #region Private Methods

      private void PopulatePacks()
      {
         PackModel burglarPack = new PackModel();
         burglarPack.Name = "Burglar's Pack";
         burglarPack.Items = new List<string>(new string[] { "Backpack", "Ball Bearings", "String (10 feet)", "Bell", "Candle:5", "Crowbar", "Hammer", "Piton:10",
                                                             "Hooded Lantern", "Oil (flask):2", "Rations (1 day):5", "Tinderbox", "Waterskin", "Hempen Rope (50 feet)" });
         _packs.Add(burglarPack);

         PackModel diplomatPack = new PackModel();
         diplomatPack.Name = "Diplomat's Pack";
         diplomatPack.Items = new List<string>(new string[] { "Chest", "Map or Scroll Case:2", "Fine Clothes", "Ink (1 ounce bottle)", "Ink Pen", "Lamp",
                                                              "Oil (flask):2", "Paper (one sheet):5", "Perfume (vial)", "Sealing Wax", "Soap" });
         _packs.Add(diplomatPack);

         PackModel dungeoneerPack = new PackModel();
         dungeoneerPack.Name = "Dungeoneer's Pack";
         dungeoneerPack.Items = new List<string>(new string[] { "Backpack", "Crowbar", "Hammer", "Piton:10", "Torch:10", "Tinderbox", "Rations (1 day):10", "Waterskin", "Hempen Rope (50 feet)" });
         _packs.Add(dungeoneerPack);

         PackModel entertainerPack = new PackModel();
         entertainerPack.Name = "Entertainer's Pack";
         entertainerPack.Items = new List<string>(new string[] { "Backpack", "Bedroll", "Costume Clothes:2", "Candle:5", "Rations (1 day):5", "Waterskin", "Disguise Kit" });
         _packs.Add(entertainerPack);

         PackModel explorerPack = new PackModel();
         explorerPack.Name = "Explorer's Pack";
         explorerPack.Items = new List<string>(new string[] { "Backpack", "Bedroll", "Mess Kit", "Tinderbox", "Torch:10", "Rations (1 day):10", "Waterskin", "Hempen Rope (50 feet)" });
         _packs.Add(explorerPack);

         PackModel priestPack = new PackModel();
         priestPack.Name = "Priest's Pack";
         priestPack.Items = new List<string>(new string[] { "Backpack", "Blanket", "Candle:10", "Tinderbox", "Alms Box", "Block of Incense:2", "Censer", "Vestments", "Rations (1 day):2", "Waterskin" });
         _packs.Add(priestPack);

         PackModel scholarPack = new PackModel();
         scholarPack.Name = "Scholar's Pack";
         scholarPack.Items = new List<string>(new string[] { "Backpack", "Book", "Ink (1 ounce bottle)", "Ink Pen", "Parchment (one sheet):10", "Little Bag of Sand", "Small Knife" });
         _packs.Add(scholarPack);
      }

      private void UpdateTags()
      {
         List<string> tags = new List<string>();

         foreach (CharacterModel characterModel in _characters)
         {
            //tags.AddRange(characterModel.Tags);
         }

         foreach (AdventureModel adventureModel in _adventures)
         {
            tags.AddRange(adventureModel.Tags);
         }

         foreach (EncounterModel encounterModel in _encounters)
         {
            //tags.AddRange(encounterModel.Tags);
         }

         foreach (LocationModel locationModel in _locations)
         {
            tags.AddRange(locationModel.Tags);
         }

         foreach (NPCModel npcModel in _npcs)
         {
            tags.AddRange(npcModel.Tags);
         }

         foreach (RandomTableModel tableModel in _tables)
         {
            tags.AddRange(tableModel.Tags);
         }

         foreach (BackgroundModel backgroundModel in _backgrounds)
         {
            //tags.AddRange(backgroundModel.Tags);
         }

         foreach (ClassModel classModel in _classes)
         {
            //tags.AddRange(classModel.Tags);
         }

         foreach (ConditionModel conditionModel in _conditions)
         {
            //tags.AddRange(conditionModel.Tags);
         }

         foreach (FeatModel featModel in _feats)
         {
            //tags.AddRange(featModel.Tags);
         }

         foreach (ItemModel itemModel in _items)
         {
            //tags.AddRange(itemModel.Tags);
         }

         foreach (MonsterModel monsterModel in _monsters)
         {
            //tags.AddRange(monsterModel.Tags);
         }

         foreach (RaceModel raceModel in _races)
         {
            //tags.AddRange(raceModel.Tags);
         }

         foreach (SpellModel spellModel in _spells)
         {
            //tags.AddRange(spellModel.Tags);
         }

         _tags = new List<string>(tags.Distinct().OrderBy(x => x));

         TagsChanged?.Invoke(this, EventArgs.Empty);
      }

      #endregion
   }
}
