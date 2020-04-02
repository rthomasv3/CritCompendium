using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Business;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Persistence;
using Version1 = CritCompendiumInfrastructure.Persistence.Version1;
using Version2 = CritCompendiumInfrastructure.Persistence.Version2;

namespace CritCompendium
{
   public sealed class DataManager : IDataManager
   {
      #region Fields

      private static readonly int _version = 1;
      private static readonly string _dataSaveFileName = "cc.data";
      private static readonly string _characterSaveFileName = "characters.ccc";
      private static readonly string _encountersSaveFileName = "encounters.cce";
      private static readonly string _backgroundsSaveFileName = "backgrounds.xml";
      private static readonly string _classesSaveFileName = "classes.xml";
      private static readonly string _conditionsSaveFileName = "conditions.xml";
      private static readonly string _featsSaveFileName = "feats.xml";
      private static readonly string _itemsSaveFileName = "items.xml";
      private static readonly string _languagesSaveFileName = "languages.csv";
      private static readonly string _monstersSaveFileName = "monsters.xml";
      private static readonly string _racesSaveFileName = "races.xml";
      private static readonly string _spellsSaveFileName = "spells.xml";
      private static readonly string _themeSaveFileName = "theme.data";
      private readonly XMLImporter _xmlImporter;
      private readonly XMLExporter _xmlExporter;
      private readonly string _saveDataFolder;
      private DateTime _lastDateLaunched;
      private string _lastVersionLaunched;
      private int _runCount;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="DataManager"/>
      /// </summary>
      public DataManager(XMLImporter xmlImporter, XMLExporter xmlExporter)
      {
         _xmlImporter = xmlImporter;
         _xmlExporter = xmlExporter;
         _saveDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Crit Compendium");

         _lastVersionLaunched = Assembly.GetExecutingAssembly().GetName().Version.ToString();
         _runCount = 0;

         CheckFirstLaunch();
      }

      #endregion

      #region Properties

      /// <summary>
      /// True if first launch
      /// </summary>
      public bool FirstLaunch
      {
         get { return _runCount == 0; }
      }

      /// <summary>
      /// Gets run count
      /// </summary>
      public int RunCount
      {
         get { return _runCount; }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Saves characters to the default save file location
      /// </summary>
      public void SaveCharacters(IEnumerable<CharacterModel> characters)
      {
         string path = Path.Combine(_saveDataFolder, _characterSaveFileName);

         ICharacterPersister characterPersister = DependencyResolver.Resolve<Version2.CharacterPersister>();

         byte[] characterBytes = characterPersister.GetBytes(characters);
         File.WriteAllBytes(path, characterBytes);
      }

      /// <summary>
      /// Gets character bytes
      /// </summary>
      public byte[] GetCharacterBytes(CharacterModel character)
      {
         byte[] characterBytes = null;
         ICharacterPersister characterPersister = DependencyResolver.Resolve<Version2.CharacterPersister>();
         characterBytes = characterPersister.GetBytes(new CharacterModel[] { character });
         return characterBytes;
      }

      /// <summary>
      /// Creates a character archive from the character model
      /// </summary>
      public byte[] CreateCharacterArchive(CharacterModel characterModel)
      {
         byte[] bytes = null;

         using (MemoryStream stream = new MemoryStream())
         {
            using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create))
            {
               ZipArchiveEntry entry = archive.CreateEntry("character.ccc");
               using (BinaryWriter writer = new BinaryWriter(entry.Open()))
               {
                  byte[] characterBytes = GetCharacterBytes(characterModel);
                  writer.Write(characterBytes);
               }

               archive.CreateEntry("resources/");

               ZipArchiveEntry backgrounds = archive.CreateEntry("resources/backgrounds.xml");
               using (StreamWriter writer = new StreamWriter(backgrounds.Open(), Encoding.UTF8))
               {
                  string xml = _xmlExporter.GetXML(characterModel.Background);
                  xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);
                  writer.Write(xml);
               }

               ZipArchiveEntry classes = archive.CreateEntry("resources/classes.xml");
               using (StreamWriter writer = new StreamWriter(classes.Open(), Encoding.UTF8))
               {
                  string xml = String.Empty;

                  List<Guid> ids = new List<Guid>();
                  foreach (LevelModel levelModel in characterModel.Levels)
                  {
                     if (levelModel.Class != null && !ids.Any(x => x == levelModel.Class.Id))
                     {
                        ids.Add(levelModel.Class.Id);

                        xml += _xmlExporter.GetXML(levelModel.Class);
                     }
                  }

                  xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

                  writer.Write(xml);
               }

               ZipArchiveEntry conditions = archive.CreateEntry("resources/conditions.xml");
               using (StreamWriter writer = new StreamWriter(conditions.Open(), Encoding.UTF8))
               {
                  string xml = String.Empty;

                  List<Guid> ids = new List<Guid>();
                  foreach (AppliedConditionModel appliedCondition in characterModel.Conditions)
                  {
                     if (appliedCondition.ConditionModel != null && !ids.Any(x => x == appliedCondition.ConditionModel.Id))
                     {
                        ids.Add(appliedCondition.ConditionModel.Id);

                        xml += _xmlExporter.GetXML(appliedCondition.ConditionModel);
                     }
                  }

                  xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

                  writer.Write(xml);
               }

               ZipArchiveEntry feats = archive.CreateEntry("resources/feats.xml");
               using (StreamWriter writer = new StreamWriter(feats.Open(), Encoding.UTF8))
               {
                  string xml = String.Empty;

                  List<Guid> ids = new List<Guid>();
                  foreach (LevelModel levelModel in characterModel.Levels)
                  {
                     foreach (FeatModel feat in levelModel.Feats)
                     {
                        if (!ids.Any(x => x == feat.Id))
                        {
                           ids.Add(feat.Id);

                           xml += _xmlExporter.GetXML(feat);
                        }
                     }
                  }

                  xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

                  writer.Write(xml);
               }

               ZipArchiveEntry items = archive.CreateEntry("resources/items.xml");
               using (StreamWriter writer = new StreamWriter(items.Open(), Encoding.UTF8))
               {
                  string xml = String.Empty;

                  List<Guid> ids = new List<Guid>();
                  foreach (BagModel bagModel in characterModel.Bags)
                  {
                     foreach (EquipmentModel equipmentModel in bagModel.Equipment)
                     {
                        if (equipmentModel.Item != null && !ids.Any(x => x == equipmentModel.Item.Id))
                        {
                           ids.Add(equipmentModel.Item.Id);

                           xml += _xmlExporter.GetXML(equipmentModel.Item);
                        }
                     }
                  }

                  xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

                  writer.Write(xml);
               }

               ZipArchiveEntry languages = archive.CreateEntry("resources/languages.csv");
               using (StreamWriter writer = new StreamWriter(languages.Open(), Encoding.UTF8))
               {
                  foreach (LanguageModel language in characterModel.Languages)
                  {
                     writer.WriteLine($"{language.Id},{language.Name}");
                  }
               }

               ZipArchiveEntry monsters = archive.CreateEntry("resources/monsters.xml");
               using (StreamWriter writer = new StreamWriter(monsters.Open(), Encoding.UTF8))
               {
                  string xml = String.Empty;

                  List<Guid> ids = new List<Guid>();
                  foreach (CompanionModel companionModel in characterModel.Companions)
                  {
                     if (companionModel.MonsterModel != null && !ids.Any(x => x == companionModel.MonsterModel.Id))
                     {
                        ids.Add(companionModel.MonsterModel.Id);

                        xml += _xmlExporter.GetXML(companionModel.MonsterModel);
                     }
                  }

                  xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

                  writer.Write(xml);
               }

               ZipArchiveEntry races = archive.CreateEntry("resources/races.xml");
               using (StreamWriter writer = new StreamWriter(races.Open(), Encoding.UTF8))
               {
                  string xml = _xmlExporter.GetXML(characterModel.Race);
                  xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);
                  writer.Write(xml);
               }

               ZipArchiveEntry spells = archive.CreateEntry("resources/spells.xml");
               using (StreamWriter writer = new StreamWriter(spells.Open(), Encoding.UTF8))
               {
                  string xml = String.Empty;

                  List<Guid> ids = new List<Guid>();
                  foreach (SpellbookModel spellbookModel in characterModel.Spellbooks)
                  {
                     foreach (SpellbookEntryModel spellbookEntryModel in spellbookModel.Spells)
                     {
                        if (spellbookEntryModel.Spell != null && !ids.Any(x => x == spellbookEntryModel.Spell.Id))
                        {
                           ids.Add(spellbookEntryModel.Spell.Id);

                           xml += _xmlExporter.GetXML(spellbookEntryModel.Spell);
                        }
                     }
                  }

                  xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

                  writer.Write(xml);
               }
            }

            bytes = stream.ToArray();
         }

         return bytes;
      }

      /// <summary>
      /// Loads characters from the default save file location
      /// </summary>
      public IEnumerable<CharacterModel> LoadCharacters(IEnumerable<BackgroundModel> backgrounds, IEnumerable<ClassModel> classes, IEnumerable<ConditionModel> conditions, IEnumerable<FeatModel> feats,
          IEnumerable<ItemModel> items, IEnumerable<LanguageModel> languages, IEnumerable<MonsterModel> monsters, IEnumerable<RaceModel> races, IEnumerable<SpellModel> spells)
      {
         List<CharacterModel> characters = new List<CharacterModel>();

         string path = Path.Combine(_saveDataFolder, _characterSaveFileName);

         if (File.Exists(path))
         {
            ICharacterPersister characterPersister = null;

            byte[] characterBytes = File.ReadAllBytes(path);
            int version = BitConverter.ToInt32(characterBytes.Take(4).ToArray(), 0);
            if (version == 1)
            {
               characterPersister = DependencyResolver.Resolve<Version1.CharacterPersister>();
            }
            else if (version == 2)
            {
               characterPersister = DependencyResolver.Resolve<Version2.CharacterPersister>();
            }

            if (characterPersister != null)
            {
               characters = characterPersister.GetCharacters(characterBytes, backgrounds, classes, conditions, feats, items, languages, monsters, races, spells).ToList();
            }
         }

         return characters;
      }

      /// <summary>
      /// Gets a character from the byte array using selected resources
      /// </summary>
      public CharacterModel GetCharacter(byte[] characterBytes, IEnumerable<BackgroundModel> backgrounds, IEnumerable<ClassModel> classes, IEnumerable<ConditionModel> conditions, IEnumerable<FeatModel> feats,
          IEnumerable<ItemModel> items, IEnumerable<LanguageModel> languages, IEnumerable<MonsterModel> monsters, IEnumerable<RaceModel> races, IEnumerable<SpellModel> spells)
      {
         CharacterModel character = null;

         ICharacterPersister characterPersister = null;
         int version = BitConverter.ToInt32(characterBytes.Take(4).ToArray(), 0);
         if (version == 1)
         {
            characterPersister = DependencyResolver.Resolve<Version1.CharacterPersister>();
         }
         else if (version == 2)
         {
            characterPersister = DependencyResolver.Resolve<Version2.CharacterPersister>();
         }

         if (characterPersister != null)
         {
            character = characterPersister.GetCharacters(characterBytes, backgrounds, classes, conditions, feats, items, languages, monsters, races, spells).First();
         }

         return character;
      }

      /// <summary>
      /// Saves adventures to the default save file location
      /// </summary>
      public void SaveAdventures(IEnumerable<AdventureModel> adventures)
      {

      }

      /// <summary>
      /// Gets adventure bytes
      /// </summary>
      public byte[] GetAdventureBytes(AdventureModel adventure)
      {
         return null;
      }

      /// <summary>
      /// Creates a adventure archive from the adventure model
      /// </summary>
      public byte[] CreateAdventureArchive(AdventureModel adventure)
      {
         return null;
      }

      /// <summary>
      /// Loads adventures from the default save file location
      /// </summary>
      public IEnumerable<AdventureModel> LoadAdventures(IEnumerable<NPCModel> npcs, IEnumerable<LocationModel> locations, IEnumerable<EncounterModel> encounters)
      {
         return Enumerable.Empty<AdventureModel>();
      }

      /// <summary>
      /// Gets a adventure from the byte array using selected resources
      /// </summary>
      public AdventureModel GetAdventure(byte[] adventureBytes, IEnumerable<NPCModel> npcs, IEnumerable<LocationModel> locations, IEnumerable<EncounterModel> encounters)
      {
         return null;
      }

      /// <summary>
      /// Saves encounters to the default save file location
      /// </summary>
      public void SaveEncounters(IEnumerable<EncounterModel> encounters)
      {
         string path = Path.Combine(_saveDataFolder, _encountersSaveFileName);
         IEncounterPersister encounterPersister = DependencyResolver.Resolve<Version1.EncounterPersister>();

         byte[] encounterBytes = encounterPersister.GetBytes(encounters);
         File.WriteAllBytes(path, encounterBytes);
      }

      /// <summary>
      /// Gets character bytes
      /// </summary>
      public byte[] GetEncounterBytes(EncounterModel encounter)
      {
         byte[] encounterBytes = null;
         IEncounterPersister encounterPersister = DependencyResolver.Resolve<Version1.EncounterPersister>();
         encounterBytes = encounterPersister.GetBytes(new EncounterModel[] { encounter });
         return encounterBytes;
      }

      /// <summary>
      /// Creates a character archive from the character model
      /// </summary>
      public byte[] CreateEncounterArchive(EncounterModel encounterModel)
      {
         byte[] bytes = null;

         using (MemoryStream stream = new MemoryStream())
         {
            using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create))
            {
               ZipArchiveEntry entry = archive.CreateEntry("encounter.cce");
               using (BinaryWriter writer = new BinaryWriter(entry.Open()))
               {
                  byte[] encounterBytes = GetEncounterBytes(encounterModel);
                  writer.Write(encounterBytes);
               }

               archive.CreateEntry("resources/");

               int characterIndex = 0;
               foreach (EncounterCharacterModel encounterCharacter in encounterModel.Creatures.Where(x => x is EncounterCharacterModel))
               {
                  if (encounterCharacter.CharacterModel != null)
                  {
                     ZipArchiveEntry character = archive.CreateEntry($"resources/character{characterIndex++}.ccca");
                     using (BinaryWriter writer = new BinaryWriter(character.Open(), Encoding.UTF8))
                     {
                        byte[] characterArchiveBytes = CreateCharacterArchive(encounterCharacter.CharacterModel);
                        writer.Write(characterArchiveBytes);
                     }
                  }
               }

               ZipArchiveEntry conditions = archive.CreateEntry("resources/conditions.xml");
               using (StreamWriter writer = new StreamWriter(conditions.Open(), Encoding.UTF8))
               {
                  string xml = String.Empty;

                  List<Guid> ids = new List<Guid>();
                  foreach (EncounterCreatureModel encounterCreatureModel in encounterModel.Creatures)
                  {
                     foreach (AppliedConditionModel appliedConditionModel in encounterCreatureModel.Conditions)
                     {
                        if (appliedConditionModel.ConditionModel != null)
                        {
                           if (!ids.Any(x => x == appliedConditionModel.ConditionModel.Id))
                           {
                              ids.Add(appliedConditionModel.ConditionModel.Id);
                              xml += _xmlExporter.GetXML(appliedConditionModel.ConditionModel);
                           }
                        }
                     }
                  }

                  xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

                  writer.Write(xml);
               }

               ZipArchiveEntry monsters = archive.CreateEntry("resources/monsters.xml");
               using (StreamWriter writer = new StreamWriter(monsters.Open(), Encoding.UTF8))
               {
                  string xml = String.Empty;

                  foreach (EncounterMonsterModel encounterMonster in encounterModel.Creatures.Where(x => x is EncounterMonsterModel))
                  {
                     if (encounterMonster.MonsterModel != null)
                     {
                        List<Guid> ids = new List<Guid>();
                        if (!ids.Any(x => x == encounterMonster.MonsterModel.Id))
                        {
                           ids.Add(encounterMonster.MonsterModel.Id);
                           xml += _xmlExporter.GetXML(encounterMonster.MonsterModel);
                        }
                     }
                  }

                  xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

                  writer.Write(xml);
               }
            }

            bytes = stream.ToArray();
         }

         return bytes;
      }

      /// <summary>
      /// Loads encounters from the default save file location
      /// </summary>
      public IEnumerable<EncounterModel> LoadEncounters(IEnumerable<CharacterModel> characters, IEnumerable<ConditionModel> conditions, IEnumerable<MonsterModel> monsters)
      {
         List<EncounterModel> encounters = new List<EncounterModel>();

         string path = Path.Combine(_saveDataFolder, _encountersSaveFileName);

         if (File.Exists(path))
         {
            IEncounterPersister encounterPersister = null;

            byte[] encounterBytes = File.ReadAllBytes(path);
            int version = BitConverter.ToInt32(encounterBytes.Take(4).ToArray(), 0);
            if (version == 1)
            {
               encounterPersister = DependencyResolver.Resolve<Version1.EncounterPersister>();
            }

            if (encounterPersister != null)
            {
               encounters = encounterPersister.GetEncounters(encounterBytes, characters, conditions, monsters).ToList();
            }
         }

         return encounters;
      }

      /// <summary>
      /// Gets a character from the byte array using selected resources
      /// </summary>
      public EncounterModel GetEncounter(byte[] encounterBytes, IEnumerable<CharacterModel> characters, IEnumerable<ConditionModel> conditions, IEnumerable<MonsterModel> monsters)
      {
         EncounterModel encounter = null;
         IEncounterPersister encounterPersister = DependencyResolver.Resolve<Version1.EncounterPersister>();
         encounter = encounterPersister.GetEncounters(encounterBytes, characters, conditions, monsters).First();
         return encounter;
      }

      /// <summary>
      /// Saves locations to the default save file location
      /// </summary>
      public void SaveLocations(IEnumerable<LocationModel> locations)
      {

      }

      /// <summary>
      /// Gets location bytes
      /// </summary>
      public byte[] GetLocationBytes(LocationModel location)
      {
         return null;
      }

      /// <summary>
      /// Creates a location archive from the location model
      /// </summary>
      public byte[] CreateLocationArchive(LocationModel location)
      {
         return null;
      }

      /// <summary>
      /// Loads locations from the default save file location
      /// </summary>
      public IEnumerable<LocationModel> LoadLocations()
      {
         return Enumerable.Empty<LocationModel>();
      }

      /// <summary>
      /// Gets a location from the byte array
      /// </summary>
      public LocationModel GetLocation(byte[] locationBytes)
      {
         return null;
      }

      /// <summary>
      /// Saves npcs to the default save file location
      /// </summary>
      public void SaveNPCs(IEnumerable<NPCModel> npcs)
      {

      }

      /// <summary>
      /// Gets npcs bytes
      /// </summary>
      public byte[] GetNPCBytes(NPCModel npcModel)
      {
         return null;
      }

      /// <summary>
      /// Creates a npc archive from the npc model
      /// </summary>
      public byte[] CreateNPCArchive(NPCModel npcModel)
      {
         return null;
      }

      /// <summary>
      /// Loads npcs from the default save file location
      /// </summary>
      public IEnumerable<NPCModel> LoadNPCs()
      {
         return Enumerable.Empty<NPCModel>();
      }

      /// <summary>
      /// Gets a npc from the byte array
      /// </summary>
      public NPCModel GetNPC(byte[] npcBytes)
      {
         return null;
      }

      /// <summary>
      /// Saves tables to the default save file location
      /// </summary>
      public void SaveTables(IEnumerable<RandomTableModel> randomTables)
      {

      }
      /// <summary>
      /// Gets table bytes
      /// </summary>
      public byte[] GetTableBytes(RandomTableModel randomTableModel)
      {
         return null;
      }

      /// <summary>
      /// Creates a table archive from the table model
      /// </summary>
      public byte[] CreateTableArchive(RandomTableModel randomTableModel)
      {
         return null;
      }

      /// <summary>
      /// Loads tables from the default save file location
      /// </summary>
      public IEnumerable<RandomTableModel> LoadTables()
      {
         return Enumerable.Empty<RandomTableModel>();
      }

      /// <summary>
      /// Gets a table from the byte array
      /// </summary>
      public RandomTableModel GetTable(byte[] tableBytes)
      {
         return null;
      }

      /// <summary>
      /// Saves backgrounds to the default save file location
      /// </summary>
      public void SaveBackgrounds(IEnumerable<BackgroundModel> backgrounds)
      {
         string xml = String.Empty;

         foreach (BackgroundModel background in backgrounds)
         {
            xml += _xmlExporter.GetXML(background);
         }

         xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

         File.WriteAllText(Path.Combine(_saveDataFolder, _backgroundsSaveFileName), xml, Encoding.UTF8);
      }

      /// <summary>
      /// Loads backgrounds from the default save file location
      /// </summary>
      public IEnumerable<BackgroundModel> LoadBackgrounds()
      {
         List<BackgroundModel> backgrounds = new List<BackgroundModel>();
         string path = Path.Combine(_saveDataFolder, _backgroundsSaveFileName);
         if (File.Exists(path))
         {
            string xml = File.ReadAllText(path, Encoding.UTF8);
            backgrounds = _xmlImporter.ReadBackgrounds(xml);
         }
         return backgrounds;
      }

      /// <summary>
      /// Saves classes to the default save file location
      /// </summary>
      public void SaveClasses(IEnumerable<ClassModel> classes)
      {
         string xml = String.Empty;

         foreach (ClassModel classModel in classes)
         {
            xml += _xmlExporter.GetXML(classModel);
         }

         xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

         File.WriteAllText(Path.Combine(_saveDataFolder, _classesSaveFileName), xml, Encoding.UTF8);
      }

      /// <summary>
      /// Loads classes from the default save file location
      /// </summary>
      public IEnumerable<ClassModel> LoadClasses()
      {
         List<ClassModel> classes = new List<ClassModel>();
         string path = Path.Combine(_saveDataFolder, _classesSaveFileName);
         if (File.Exists(path))
         {
            string xml = File.ReadAllText(path, Encoding.UTF8);
            classes = _xmlImporter.ReadClasses(xml);
         }
         return classes;
      }

      /// <summary>
      /// Saves conditions to the default save file location
      /// </summary>
      public void SaveConditions(IEnumerable<ConditionModel> conditions)
      {
         string xml = String.Empty;

         foreach (ConditionModel condition in conditions)
         {
            xml += _xmlExporter.GetXML(condition);
         }

         xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

         File.WriteAllText(Path.Combine(_saveDataFolder, _conditionsSaveFileName), xml, Encoding.UTF8);
      }

      /// <summary>
      /// Loads conditions from the default save file location
      /// </summary>
      public IEnumerable<ConditionModel> LoadConditions()
      {
         List<ConditionModel> conditions = new List<ConditionModel>();
         string path = Path.Combine(_saveDataFolder, _conditionsSaveFileName);
         if (File.Exists(path))
         {
            string xml = File.ReadAllText(path, Encoding.UTF8);
            conditions = _xmlImporter.ReadConditions(xml);
         }
         return conditions;
      }

      /// <summary>
      /// Saves feats to the default save file location
      /// </summary>
      public void SaveFeats(IEnumerable<FeatModel> feats)
      {
         string xml = String.Empty;

         foreach (FeatModel feat in feats)
         {
            xml += _xmlExporter.GetXML(feat);
         }

         xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

         File.WriteAllText(Path.Combine(_saveDataFolder, _featsSaveFileName), xml, Encoding.UTF8);
      }

      /// <summary>
      /// Loads feats from the default save file location
      /// </summary>
      public IEnumerable<FeatModel> LoadFeats()
      {
         List<FeatModel> feats = new List<FeatModel>();
         string path = Path.Combine(_saveDataFolder, _featsSaveFileName);
         if (File.Exists(path))
         {
            string xml = File.ReadAllText(path, Encoding.UTF8);
            feats = _xmlImporter.ReadFeats(xml);
         }
         return feats;
      }

      /// <summary>
      /// Saves items to the default save file location
      /// </summary>
      public void SaveItems(IEnumerable<ItemModel> items)
      {
         string xml = String.Empty;

         foreach (ItemModel item in items)
         {
            xml += _xmlExporter.GetXML(item);
         }

         xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

         File.WriteAllText(Path.Combine(_saveDataFolder, _itemsSaveFileName), xml, Encoding.UTF8);
      }

      /// <summary>
      /// Loads items from the default save file location
      /// </summary>
      public IEnumerable<ItemModel> LoadItems()
      {
         List<ItemModel> items = new List<ItemModel>();
         string path = Path.Combine(_saveDataFolder, _itemsSaveFileName);
         if (File.Exists(path))
         {
            string xml = File.ReadAllText(path, Encoding.UTF8);
            items = _xmlImporter.ReadItems(xml);
         }
         return items;
      }

      /// <summary>
      /// Saves languages to the default save file location
      /// </summary>
      public void SaveLanguages(IEnumerable<LanguageModel> languages)
      {
         string languagesString = String.Join(Environment.NewLine, languages.Select(x => $"{x.Id},{x.Name}"));
         File.WriteAllText(Path.Combine(_saveDataFolder, _languagesSaveFileName), languagesString, Encoding.UTF8);
      }

      /// <summary>
      /// Loads languages from the default save file location
      /// </summary>
      public IEnumerable<LanguageModel> LoadLanguages()
      {
         List<LanguageModel> languages = new List<LanguageModel>();

         foreach (string language in File.ReadAllLines(Path.Combine(_saveDataFolder, _languagesSaveFileName)))
         {
            string[] parts = language.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
               LanguageModel languageModel = new LanguageModel();
               if (Guid.TryParse(parts[0], out Guid id))
               {
                  languageModel.Id = id;
               }
               languageModel.Name = parts[1].Trim();

               languages.Add(languageModel);
            }
         }

         return languages;
      }

      /// <summary>
      /// Saves monsters to the default save file location
      /// </summary>
      public void SaveMonsters(IEnumerable<MonsterModel> monsters)
      {
         string xml = String.Empty;

         foreach (MonsterModel monster in monsters)
         {
            xml += _xmlExporter.GetXML(monster);
         }

         xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

         File.WriteAllText(Path.Combine(_saveDataFolder, _monstersSaveFileName), xml, Encoding.UTF8);
      }

      /// <summary>
      /// Loads monsters from the default save file location
      /// </summary>
      public IEnumerable<MonsterModel> LoadMonsters()
      {
         List<MonsterModel> monsters = new List<MonsterModel>();
         string path = Path.Combine(_saveDataFolder, _monstersSaveFileName);
         if (File.Exists(path))
         {
            string xml = File.ReadAllText(path, Encoding.UTF8);
            monsters = _xmlImporter.ReadMonsters(xml);
         }
         return monsters;
      }

      /// <summary>
      /// Saves races to the default save file location
      /// </summary>
      public void SaveRaces(IEnumerable<RaceModel> races)
      {
         string xml = String.Empty;

         foreach (RaceModel race in races)
         {
            xml += _xmlExporter.GetXML(race);
         }

         xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

         File.WriteAllText(Path.Combine(_saveDataFolder, _racesSaveFileName), xml, Encoding.UTF8);
      }

      /// <summary>
      /// Loads races from the default save file location
      /// </summary>
      public IEnumerable<RaceModel> LoadRacess()
      {
         List<RaceModel> races = new List<RaceModel>();
         string path = Path.Combine(_saveDataFolder, _racesSaveFileName);
         if (File.Exists(path))
         {
            string xml = File.ReadAllText(path, Encoding.UTF8);
            races = _xmlImporter.ReadRaces(xml);
         }
         return races;
      }

      /// <summary>
      /// Saves spells to the default save file location
      /// </summary>
      public void SaveSpells(IEnumerable<SpellModel> spells)
      {
         string xml = String.Empty;

         foreach (SpellModel spell in spells)
         {
            xml += _xmlExporter.GetXML(spell);
         }

         xml = _xmlExporter.WrapAndFormatXMLWithHeader(xml);

         File.WriteAllText(Path.Combine(_saveDataFolder, _spellsSaveFileName), xml, Encoding.UTF8);
      }

      /// <summary>
      /// Loads spells from the default save file location
      /// </summary>
      public IEnumerable<SpellModel> LoadSpells()
      {
         List<SpellModel> spells = new List<SpellModel>();
         string path = Path.Combine(_saveDataFolder, _spellsSaveFileName);
         if (File.Exists(path))
         {
            string xml = File.ReadAllText(path, Encoding.UTF8);
            spells = _xmlImporter.ReadSpells(xml);
         }
         return spells;
      }

      /// <summary>
      /// Saves the theme
      /// </summary>
      public void SaveTheme(byte[] themeBytes)
      {
         string path = Path.Combine(_saveDataFolder, _themeSaveFileName);
         File.WriteAllBytes(path, themeBytes);
      }

      /// <summary>
      /// Gets theme file bytes
      /// </summary>
      public byte[] LoadTheme()
      {
         byte[] bytes = null;

         string path = Path.Combine(_saveDataFolder, _themeSaveFileName);
         if (File.Exists(path))
         {
            bytes = File.ReadAllBytes(path);
         }

         return bytes;
      }

      /// <summary>
      /// Saves the launch information data file
      /// </summary>
      public void SaveLaunchData()
      {
         string dataPath = Path.Combine(_saveDataFolder, _dataSaveFileName);

         List<byte> bytes = new List<byte>();

         bytes.AddRange(BitConverter.GetBytes(_version));
         bytes.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));

         byte[] lastVersionBytes = Encoding.UTF8.GetBytes(_lastVersionLaunched);
         bytes.AddRange(BitConverter.GetBytes(lastVersionBytes.Length));
         bytes.AddRange(lastVersionBytes);
         bytes.AddRange(BitConverter.GetBytes(_runCount));

         File.WriteAllBytes(dataPath, bytes.ToArray());
      }

      #endregion

      #region Private Methods

      private void CheckFirstLaunch()
      {
         if (!Directory.Exists(_saveDataFolder))
         {
            Directory.CreateDirectory(_saveDataFolder);
         }

         if (File.Exists(Path.Combine(_saveDataFolder, _dataSaveFileName)))
         {
            ReadLaunchData();
         }
         else
         {
            SaveLaunchData();
         }
      }

      private void ReadLaunchData()
      {
         string dataPath = Path.Combine(_saveDataFolder, _dataSaveFileName);

         using (FileStream fileStream = new FileStream(dataPath, FileMode.Open))
         {
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
               int version = BitConverter.ToInt32(reader.ReadBytes(4), 0);
               if (version == 1)
               {
                  long lastDate = BitConverter.ToInt64(reader.ReadBytes(8), 0);
                  _lastDateLaunched = new DateTime(lastDate);

                  int lastVersionSize = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                  _lastVersionLaunched = Encoding.UTF8.GetString(reader.ReadBytes(lastVersionSize));

                  _runCount = BitConverter.ToInt32(reader.ReadBytes(4), 0) + 1;
               }
            }
         }
      }

      #endregion
   }
}
