using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Persistence;
using CritCompendiumInfrastructure.Services;
using Microsoft.Win32;

namespace CritCompendium.ViewModels
{
   public sealed class ImportViewModel : ObjectViewModel
   {
      #region Fields

      private readonly Compendium _compendium;
      private readonly StringService _stringService;
      private readonly DataManager _dataManager;
      private readonly DialogService _dialogService;

      private string _filePath;
      private List<byte[]> _characters = new List<byte[]>();
      private List<byte[]> _encounters = new List<byte[]>();
      private List<BackgroundModel> _backgrounds = new List<BackgroundModel>();
      private List<ClassModel> _classes = new List<ClassModel>();
      private List<ConditionModel> _conditions = new List<ConditionModel>();
      private List<FeatModel> _feats = new List<FeatModel>();
      private List<ItemModel> _items = new List<ItemModel>();
      private List<LanguageModel> _languages = new List<LanguageModel>();
      private List<MonsterModel> _monsters = new List<MonsterModel>();
      private List<RaceModel> _races = new List<RaceModel>();
      private List<SpellModel> _spells = new List<SpellModel>();
      private bool _addAllEntries;
      private bool _skipDuplicateEntries;
      private bool _replaceExistingEntries = true;

      private readonly ICommand _browseCommand;
      private readonly ICommand _restoreDefaultCompendiumCommand;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="ImportViewModel"/>
      /// </summary>
      public ImportViewModel(Compendium compendium, StringService stringService, DataManager dataManager, DialogService dialogService)
      {
         _compendium = compendium;
         _stringService = stringService;
         _dataManager = dataManager;
         _dialogService = dialogService;

         _browseCommand = new RelayCommand(obj => true, obj => Browse());
         _restoreDefaultCompendiumCommand = new RelayCommand(obj => true, obj => RestoreDefaultCompendium());
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets file path
      /// </summary>
      public string FilePath
      {
         get { return _filePath; }
         set { _filePath = value; }
      }

      /// <summary>
      /// Gets character count
      /// </summary>
      public int CharacterCount
      {
         get { return _characters.Count; }
      }

      /// <summary>
      /// Gets encounter count
      /// </summary>
      public int EncounterCount
      {
         get { return _encounters.Count; }
      }

      /// <summary>
      /// Gets background count
      /// </summary>
      public int BackgroundCount
      {
         get { return _backgrounds.Count; }
      }

      /// <summary>
      /// Gets class count
      /// </summary>
      public int ClassCount
      {
         get { return _classes.Count; }
      }

      /// <summary>
      /// Gets condition count
      /// </summary>
      public int ConditionCount
      {
         get { return _conditions.Count; }
      }

      /// <summary>
      /// Gets feat count
      /// </summary>
      public int FeatCount
      {
         get { return _feats.Count; }
      }

      /// <summary>
      /// Gets item count
      /// </summary>
      public int ItemCount
      {
         get { return _items.Count; }
      }

      /// <summary>
      /// Gets language count
      /// </summary>
      public int LanguageCount
      {
         get { return _languages.Count; }
      }

      /// <summary>
      /// Gets monster count
      /// </summary>
      public int MonsterCount
      {
         get { return _monsters.Count; }
      }

      /// <summary>
      /// Gets race count
      /// </summary>
      public int RaceCount
      {
         get { return _races.Count; }
      }

      /// <summary>
      /// Gets spell count
      /// </summary>
      public int SpellCount
      {
         get { return _spells.Count; }
      }
      /// <summary>
      /// Gets or sets add all entries
      /// </summary>
      public bool AddAllEntries
      {
         get { return _addAllEntries; }
         set { _addAllEntries = value; }
      }

      /// <summary>
      /// Gets or sets skip duplicate entries
      /// </summary>
      public bool SkipDuplicateEntries
      {
         get { return _skipDuplicateEntries; }
         set { _skipDuplicateEntries = value; }
      }

      /// <summary>
      /// Gets or sets replace existing entries
      /// </summary>
      public bool ReplaceExistingEntries
      {
         get { return _replaceExistingEntries; }
         set { _replaceExistingEntries = value; }
      }

      /// <summary>
      /// Gets browse command
      /// </summary>
      public ICommand BrowseCommand
      {
         get { return _browseCommand; }
      }

      /// <summary>
      /// Gets restore default compendium command
      /// </summary>
      public ICommand RestoreDefaultCompendiumCommand
      {
         get { return _restoreDefaultCompendiumCommand; }
      }

      #endregion

      #region Private Methods

      private void Browse()
      {
         OpenFileDialog fileDialog = new OpenFileDialog();
         fileDialog.Filter = "Compendium Files|*.xml;*.ccca;*.ccea|XML Files|*.xml|Character Archives|*.ccca|Encounter Archives|*.ccea";

         if (fileDialog.ShowDialog() == true)
         {
            _characters.Clear();
            _encounters.Clear();
            _backgrounds.Clear();
            _classes.Clear();
            _conditions.Clear();
            _feats.Clear();
            _items.Clear();
            _monsters.Clear();
            _races.Clear();
            _spells.Clear();
            _languages.Clear();

            string ext = Path.GetExtension(fileDialog.FileName);

            try
            {
               if (ext == ".xml")
               {
                  ReadXMLFile(fileDialog.FileName);
               }
               else if (ext == ".ccca")
               {
                  ReadCharacterArchive(fileDialog.FileName);
               }
               else if (ext == ".ccea")
               {
                  ReadEncounterArchive(fileDialog.FileName);
               }
               else
               {
                  _dialogService.ShowConfirmationDialog("File Error", "The selected file type is unsupported.", "OK", null, null);
               }
            }
            catch (Exception e)
            {
               _dialogService.ShowConfirmationDialog("Import Error", "An error occurred when attempting to import the file." + Environment.NewLine + Environment.NewLine + e.Message, "OK", null, null);
            }

            _filePath = fileDialog.FileName;

            OnPropertyChanged(nameof(FilePath));
         }
      }

      private void RestoreDefaultCompendium()
      {
         string message = "IMPORTANT!" + Environment.NewLine + Environment.NewLine +
                          "This cannot be undone!" + Environment.NewLine + Environment.NewLine +
                          "Restoring your default compendium will remove any custom or imported entries to your compendium. " +
                          "Characters, Encounters, and any custom content they contain will not be removed but may be replaced." + Environment.NewLine + Environment.NewLine +
                          "Are you sure you want to restore the default compendium?";

         bool? result = _dialogService.ShowConfirmationDialog("Restore Default Compendium", message, "Yes", "No", null);
         if (result == true)
         {
            XMLImporter importer = new XMLImporter(_stringService);
            importer.LoadManifestResourceXML("default_compendium.xml");

            List<BackgroundModel> backgrounds = importer.ReadBackgrounds();
            List<ClassModel> classes = importer.ReadClasses();
            List<ConditionModel> conditions = importer.ReadConditions();
            List<FeatModel> feats = importer.ReadFeats();
            List<ItemModel> items = importer.ReadItems();
            List<MonsterModel> monsters = importer.ReadMonsters();
            List<RaceModel> races = importer.ReadRaces();
            List<SpellModel> spells = importer.ReadSpells();
            List<LanguageModel> languages = importer.LanguagesFound;

            foreach (CharacterModel character in _compendium.Characters)
            {
               if (character.Background != null &&
                   !backgrounds.Any(x => x.Name.Equals(character.Background.Name, StringComparison.CurrentCultureIgnoreCase)))
               {
                  backgrounds.Add(character.Background);
               }

               if (character.Race != null &&
                   !races.Any(x => x.Name.Equals(character.Race.Name, StringComparison.CurrentCultureIgnoreCase)))
               {
                  races.Add(character.Race);
               }

               foreach (LevelModel level in character.Levels)
               {
                  if (level.Class != null)
                  {
                     ClassModel defaultClass = classes.FirstOrDefault(x => x.Name.Equals(level.Class.Name, StringComparison.CurrentCultureIgnoreCase));
                     if (defaultClass == null)
                     {
                        classes.Add(level.Class);
                     }
                     else
                     {
                        if (defaultClass.AutoLevels.Count != level.Class.AutoLevels.Count)
                        {
                           classes.Add(level.Class);
                        }
                        else
                        {
                           for (int i = 0; i < level.Class.AutoLevels.Count; ++i)
                           {
                              if (level.Class.AutoLevels[i].Features.Count != defaultClass.AutoLevels[i].Features.Count)
                              {
                                 classes.Add(level.Class);
                                 break;
                              }
                           }
                        }
                     }
                  }

                  foreach (FeatModel feat in level.Feats)
                  {
                     if (!feats.Any(x => x.Name.Equals(feat.Name, StringComparison.CurrentCultureIgnoreCase)))
                     {
                        feats.Add(feat);
                     }
                  }
               }

               foreach (LanguageModel language in character.Languages)
               {
                  if (!languages.Any(x => x.Name.Equals(language.Name, StringComparison.CurrentCultureIgnoreCase)))
                  {
                     languages.Add(language);
                  }
               }

               foreach (AppliedConditionModel appliedCondition in character.Conditions)
               {
                  if (appliedCondition.ConditionModel != null &&
                      !conditions.Any(x => x.Name.Equals(appliedCondition.ConditionModel.Name, StringComparison.CurrentCultureIgnoreCase)))
                  {
                     conditions.Add(appliedCondition.ConditionModel);
                  }
               }

               foreach (CompanionModel companion in character.Companions)
               {
                  if (companion.MonsterModel != null &&
                      !monsters.Any(x => x.Name.Equals(companion.MonsterModel.Name, StringComparison.CurrentCultureIgnoreCase)))
                  {
                     monsters.Add(companion.MonsterModel);
                  }
               }

               foreach (BagModel bag in character.Bags)
               {
                  foreach (EquipmentModel equipment in bag.Equipment)
                  {
                     if (equipment.Item != null &&
                         !items.Any(x => x.Name.Equals(equipment.Item.Name, StringComparison.CurrentCultureIgnoreCase)))
                     {
                        items.Add(equipment.Item);
                     }
                  }
               }

               foreach (SpellbookModel spellbook in character.Spellbooks)
               {
                  foreach (SpellbookEntryModel spellbookEntry in spellbook.Spells)
                  {
                     if (spellbookEntry.Spell != null &&
                         !spells.Any(x => x.Name.Equals(spellbookEntry.Spell.Name, StringComparison.CurrentCultureIgnoreCase)))
                     {
                        spells.Add(spellbookEntry.Spell);
                     }
                  }
               }
            }

            foreach (EncounterModel encounter in _compendium.Encounters)
            {
               foreach (EncounterMonsterModel encounterMonster in encounter.Creatures.Where(x => x is EncounterMonsterModel))
               {
                  if (encounterMonster.MonsterModel != null &&
                      !monsters.Any(x => x.Name.Equals(encounterMonster.MonsterModel.Name, StringComparison.CurrentCultureIgnoreCase)))
                  {
                     monsters.Add(encounterMonster.MonsterModel);
                  }
               }
            }

            _compendium.SetCompendium(backgrounds, classes, conditions, feats, items, languages, monsters, races, spells);

            _compendium.SaveBackgrounds();
            _compendium.SaveClasses();
            _compendium.SaveConditions();
            _compendium.SaveFeats();
            _compendium.SaveItems();
            _compendium.SaveMonsters();
            _compendium.SaveRaces();
            _compendium.SaveSpells();
            _compendium.SaveLanguages();

            _compendium.NotifyImportComplete();

            base.OnAccept();
         }
      }

      private void ReadXMLFile(string fileLocation)
      {
         XMLImporter xmlImporter = new XMLImporter(_stringService);

         xmlImporter.LoadFileXML(fileLocation);

         _backgrounds = xmlImporter.ReadBackgrounds();
         _classes = xmlImporter.ReadClasses();
         _conditions = xmlImporter.ReadConditions();
         _feats = xmlImporter.ReadFeats();
         _items = xmlImporter.ReadItems();
         _monsters = xmlImporter.ReadMonsters();
         _races = xmlImporter.ReadRaces();
         _spells = xmlImporter.ReadSpells();

         _languages = xmlImporter.LanguagesFound;

         OnPropertyChanged(nameof(BackgroundCount));
         OnPropertyChanged(nameof(ClassCount));
         OnPropertyChanged(nameof(ConditionCount));
         OnPropertyChanged(nameof(FeatCount));
         OnPropertyChanged(nameof(ItemCount));
         OnPropertyChanged(nameof(LanguageCount));
         OnPropertyChanged(nameof(MonsterCount));
         OnPropertyChanged(nameof(RaceCount));
         OnPropertyChanged(nameof(SpellCount));
      }

      private void ReadCharacterArchive(string fileLocation)
      {
         XMLImporter xmlImporter = new XMLImporter(_stringService);

         using (FileStream fileStream = File.Open(fileLocation, FileMode.Open))
         {
            ReadCharacterArchive(fileStream);
         }

         OnPropertyChanged(nameof(CharacterCount));
         OnPropertyChanged(nameof(BackgroundCount));
         OnPropertyChanged(nameof(ClassCount));
         OnPropertyChanged(nameof(ConditionCount));
         OnPropertyChanged(nameof(FeatCount));
         OnPropertyChanged(nameof(ItemCount));
         OnPropertyChanged(nameof(LanguageCount));
         OnPropertyChanged(nameof(MonsterCount));
         OnPropertyChanged(nameof(RaceCount));
         OnPropertyChanged(nameof(SpellCount));
      }

      private void ReadCharacterArchive(Stream stream)
      {
         XMLImporter xmlImporter = new XMLImporter(_stringService);

         using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read))
         {
            ZipArchiveEntry backgroundsEntry = archive.GetEntry("resources/backgrounds.xml");
            using (StreamReader reader = new StreamReader(backgroundsEntry.Open(), Encoding.UTF8))
            {
               string xml = reader.ReadToEnd();
               _backgrounds.AddRange(xmlImporter.ReadBackgrounds(xml));
            }

            ZipArchiveEntry classesEntry = archive.GetEntry("resources/classes.xml");
            using (StreamReader reader = new StreamReader(classesEntry.Open(), Encoding.UTF8))
            {
               string xml = reader.ReadToEnd();
               _classes.AddRange(xmlImporter.ReadClasses(xml));
            }

            ZipArchiveEntry conditionsEntry = archive.GetEntry("resources/conditions.xml");
            using (StreamReader reader = new StreamReader(conditionsEntry.Open(), Encoding.UTF8))
            {
               string xml = reader.ReadToEnd();
               _conditions.AddRange(xmlImporter.ReadConditions(xml));
            }

            ZipArchiveEntry featsEntry = archive.GetEntry("resources/feats.xml");
            using (StreamReader reader = new StreamReader(featsEntry.Open(), Encoding.UTF8))
            {
               string xml = reader.ReadToEnd();
               _feats.AddRange(xmlImporter.ReadFeats(xml));
            }

            ZipArchiveEntry itemsEntry = archive.GetEntry("resources/items.xml");
            using (StreamReader reader = new StreamReader(itemsEntry.Open(), Encoding.UTF8))
            {
               string xml = reader.ReadToEnd();
               _items.AddRange(xmlImporter.ReadItems(xml));
            }

            ZipArchiveEntry languagesEntry = archive.GetEntry("resources/languages.csv");
            using (StreamReader reader = new StreamReader(languagesEntry.Open(), Encoding.UTF8))
            {
               while (!reader.EndOfStream)
               {
                  string languageLine = reader.ReadLine();
                  if (!String.IsNullOrWhiteSpace(languageLine))
                  {
                     string[] parts = languageLine.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                     if (parts.Length == 2)
                     {
                        if (Guid.TryParse(parts[0], out Guid id))
                        {
                           LanguageModel languageModel = new LanguageModel();
                           languageModel.ID = id;
                           languageModel.Name = parts[1].Trim();
                           _languages.Add(languageModel);
                        }
                     }
                  }
               }
            }

            ZipArchiveEntry monstersEntry = archive.GetEntry("resources/monsters.xml");
            using (StreamReader reader = new StreamReader(monstersEntry.Open(), Encoding.UTF8))
            {
               string xml = reader.ReadToEnd();
               _monsters.AddRange(xmlImporter.ReadMonsters(xml));
            }

            ZipArchiveEntry racesEntry = archive.GetEntry("resources/races.xml");
            using (StreamReader reader = new StreamReader(racesEntry.Open(), Encoding.UTF8))
            {
               string xml = reader.ReadToEnd();
               _races.AddRange(xmlImporter.ReadRaces(xml));
            }

            ZipArchiveEntry spellsEntry = archive.GetEntry("resources/spells.xml");
            using (StreamReader reader = new StreamReader(spellsEntry.Open(), Encoding.UTF8))
            {
               string xml = reader.ReadToEnd();
               _spells.AddRange(xmlImporter.ReadSpells(xml));
            }

            ZipArchiveEntry characterEntry = archive.GetEntry("character.ccc");
            using (MemoryStream memoryStream = new MemoryStream())
            {
               using (Stream characterEntryStream = characterEntry.Open())
               {
                  characterEntryStream.CopyTo(memoryStream);
                  _characters.Add(memoryStream.ToArray());
               }
            }
         }
      }

      private void ReadEncounterArchive(string fileLocation)
      {
         XMLImporter xmlImporter = new XMLImporter(_stringService);

         using (FileStream fileStream = File.Open(fileLocation, FileMode.Open))
         {
            using (ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Read))
            {
               ZipArchiveEntry conditionsEntry = archive.GetEntry("resources/conditions.xml");
               using (StreamReader reader = new StreamReader(conditionsEntry.Open(), Encoding.UTF8))
               {
                  string xml = reader.ReadToEnd();
                  _conditions = xmlImporter.ReadConditions(xml);
               }

               ZipArchiveEntry monstersEntry = archive.GetEntry("resources/monsters.xml");
               using (StreamReader reader = new StreamReader(monstersEntry.Open(), Encoding.UTF8))
               {
                  string xml = reader.ReadToEnd();
                  _monsters = xmlImporter.ReadMonsters(xml);
               }

               foreach (ZipArchiveEntry characterEntry in archive.Entries.Where(x => x.Name.Contains(".ccca")))
               {
                  ReadCharacterArchive(characterEntry.Open());
               }

               ZipArchiveEntry encounterEntry = archive.GetEntry("encounter.cce");
               using (MemoryStream memoryStream = new MemoryStream())
               {
                  using (Stream stream = encounterEntry.Open())
                  {
                     stream.CopyTo(memoryStream);
                     _encounters.Add(memoryStream.ToArray());
                  }
               }
            }
         }

         OnPropertyChanged(nameof(CharacterCount));
         OnPropertyChanged(nameof(EncounterCount));
         OnPropertyChanged(nameof(BackgroundCount));
         OnPropertyChanged(nameof(ClassCount));
         OnPropertyChanged(nameof(ConditionCount));
         OnPropertyChanged(nameof(FeatCount));
         OnPropertyChanged(nameof(ItemCount));
         OnPropertyChanged(nameof(LanguageCount));
         OnPropertyChanged(nameof(MonsterCount));
         OnPropertyChanged(nameof(RaceCount));
         OnPropertyChanged(nameof(SpellCount));
      }

      private void ImportCharacters()
      {
         foreach (byte[] characterBytes in _characters)
         {
            CharacterModel characterModel = _dataManager.GetCharacter(characterBytes, _compendium.Backgrounds, _compendium.Classes, _compendium.Conditions,
                _compendium.Feats, _compendium.Items, _compendium.Languages, _compendium.Monsters, _compendium.Races, _compendium.Spells);

            if (_addAllEntries)
            {
               _compendium.AddCharacter(characterModel);
            }
            else if (_skipDuplicateEntries)
            {
               if (!_compendium.Characters.Any(x => x.Name.Equals(characterModel.Name, StringComparison.CurrentCultureIgnoreCase)))
               {
                  _compendium.AddCharacter(characterModel);
               }
            }
            else if (_replaceExistingEntries)
            {
               CharacterModel existing = _compendium.Characters.FirstOrDefault(x => x.Name.Equals(characterModel.Name, StringComparison.CurrentCultureIgnoreCase));
               if (existing == null)
               {
                  _compendium.AddCharacter(characterModel);
               }
               else
               {
                  characterModel.ID = existing.ID;
                  _compendium.UpdateCharacter(characterModel);
               }
            }
         }
      }

      private void ImportEncounters()
      {
         foreach (byte[] encounterBytes in _encounters)
         {
            EncounterModel encounterModel = _dataManager.GetEncounter(encounterBytes, _compendium.Characters, _compendium.Conditions, _compendium.Monsters);

            if (_addAllEntries)
            {
               _compendium.AddEncounter(encounterModel);
            }
            else if (_skipDuplicateEntries)
            {
               if (!_compendium.Encounters.Any(x => x.Name.Equals(encounterModel.Name, StringComparison.CurrentCultureIgnoreCase)))
               {
                  _compendium.AddEncounter(encounterModel);
               }
            }
            else if (_replaceExistingEntries)
            {
               EncounterModel existing = _compendium.Encounters.FirstOrDefault(x => x.Name.Equals(encounterModel.Name, StringComparison.CurrentCultureIgnoreCase));
               if (existing == null)
               {
                  _compendium.AddEncounter(encounterModel);
               }
               else
               {
                  encounterModel.ID = existing.ID;
                  _compendium.UpdateEncounter(encounterModel);
               }
            }
         }
      }

      private void ImportBackgrounds()
      {
         foreach (BackgroundModel backgroundModel in _backgrounds)
         {
            if (_addAllEntries)
            {
               _compendium.AddBackground(backgroundModel);
            }
            else if (_skipDuplicateEntries)
            {
               if (!_compendium.Backgrounds.Any(x => x.Name.Equals(backgroundModel.Name, StringComparison.CurrentCultureIgnoreCase)))
               {
                  _compendium.Backgrounds.Add(backgroundModel);
               }
            }
            else if (_replaceExistingEntries)
            {
               BackgroundModel existing = _compendium.Backgrounds.FirstOrDefault(x => x.Name.Equals(backgroundModel.Name, StringComparison.CurrentCultureIgnoreCase));
               if (existing == null)
               {
                  _compendium.AddBackground(backgroundModel);
               }
               else
               {
                  backgroundModel.ID = existing.ID;
                  _compendium.UpdateBackground(backgroundModel);
               }
            }
         }
      }

      private void ImportClasses()
      {
         foreach (ClassModel classModel in _classes)
         {
            if (_addAllEntries)
            {
               _compendium.AddClass(classModel);
            }
            else if (_skipDuplicateEntries)
            {
               if (!_compendium.Classes.Any(x => x.Name.Equals(classModel.Name, StringComparison.CurrentCultureIgnoreCase)))
               {
                  _compendium.AddClass(classModel);
               }
            }
            else if (_replaceExistingEntries)
            {
               ClassModel existing = _compendium.Classes.FirstOrDefault(x => x.Name.Equals(classModel.Name, StringComparison.CurrentCultureIgnoreCase));
               if (existing == null)
               {
                  _compendium.AddClass(classModel);
               }
               else
               {
                  classModel.ID = existing.ID;
                  _compendium.UpdateClass(classModel);
               }
            }
         }
      }

      private void ImportConditons()
      {
         foreach (ConditionModel conditionModel in _conditions)
         {
            if (_addAllEntries)
            {
               _compendium.AddCondition(conditionModel);
            }
            else if (_skipDuplicateEntries)
            {
               if (!_compendium.Conditions.Any(x => x.Name.Equals(conditionModel.Name, StringComparison.CurrentCultureIgnoreCase)))
               {
                  _compendium.AddCondition(conditionModel);
               }
            }
            else if (_replaceExistingEntries)
            {
               ConditionModel existing = _compendium.Conditions.FirstOrDefault(x => x.Name.Equals(conditionModel.Name, StringComparison.CurrentCultureIgnoreCase));
               if (existing == null)
               {
                  _compendium.AddCondition(conditionModel);
               }
               else
               {
                  conditionModel.ID = existing.ID;
                  _compendium.UpdateCondition(conditionModel);
               }
            }
         }
      }

      private void ImportFeats()
      {
         foreach (FeatModel featModel in _feats)
         {
            if (_addAllEntries)
            {
               _compendium.AddFeat(featModel);
            }
            else if (_skipDuplicateEntries)
            {
               if (!_compendium.Feats.Any(x => x.Name.Equals(featModel.Name, StringComparison.CurrentCultureIgnoreCase)))
               {
                  _compendium.AddFeat(featModel);
               }
            }
            else if (_replaceExistingEntries)
            {
               FeatModel existing = _compendium.Feats.FirstOrDefault(x => x.Name.Equals(featModel.Name, StringComparison.CurrentCultureIgnoreCase));
               if (existing == null)
               {
                  _compendium.AddFeat(featModel);
               }
               else
               {
                  featModel.ID = existing.ID;
                  _compendium.UpdateFeat(featModel);
               }
            }
         }
      }

      private void ImportItems()
      {
         foreach (ItemModel itemModel in _items)
         {
            if (_addAllEntries)
            {
               _compendium.AddItem(itemModel);
            }
            else if (_skipDuplicateEntries)
            {
               if (!_compendium.Items.Any(x => x.Name.Equals(itemModel.Name, StringComparison.CurrentCultureIgnoreCase)))
               {
                  _compendium.AddItem(itemModel);
               }
            }
            else if (_replaceExistingEntries)
            {
               ItemModel existing = _compendium.Items.FirstOrDefault(x => x.Name.Equals(itemModel.Name, StringComparison.CurrentCultureIgnoreCase));
               if (existing == null)
               {
                  _compendium.AddItem(itemModel);
               }
               else
               {
                  itemModel.ID = existing.ID;
                  _compendium.UpdateItem(itemModel);
               }
            }
         }
      }

      private void ImportLanguages()
      {
         foreach (LanguageModel languageModel in _languages)
         {
            if (!_compendium.Languages.Any(x => x.Name.Equals(languageModel.Name, StringComparison.CurrentCultureIgnoreCase)))
            {
               _compendium.AddLanguage(languageModel);
            }
         }
      }

      private void ImportMonsters()
      {
         foreach (MonsterModel monsterModel in _monsters)
         {
            if (_addAllEntries)
            {
               _compendium.AddMonster(monsterModel);
            }
            else if (_skipDuplicateEntries)
            {
               if (!_compendium.Monsters.Any(x => x.Name.Equals(monsterModel.Name, StringComparison.CurrentCultureIgnoreCase)))
               {
                  _compendium.AddMonster(monsterModel);
               }
            }
            else if (_replaceExistingEntries)
            {
               MonsterModel existing = _compendium.Monsters.FirstOrDefault(x => x.Name.Equals(monsterModel.Name, StringComparison.CurrentCultureIgnoreCase));
               if (existing == null)
               {
                  _compendium.AddMonster(monsterModel);
               }
               else
               {
                  monsterModel.ID = existing.ID;
                  _compendium.UpdateMonster(monsterModel);
               }
            }
         }
      }

      private void ImportRaces()
      {
         foreach (RaceModel raceModel in _races)
         {
            if (_addAllEntries)
            {
               _compendium.AddRace(raceModel);
            }
            else if (_skipDuplicateEntries)
            {
               if (!_compendium.Races.Any(x => x.Name.Equals(raceModel.Name, StringComparison.CurrentCultureIgnoreCase)))
               {
                  _compendium.AddRace(raceModel);
               }
            }
            else if (_replaceExistingEntries)
            {
               RaceModel existing = _compendium.Races.FirstOrDefault(x => x.Name.Equals(raceModel.Name, StringComparison.CurrentCultureIgnoreCase));
               if (existing == null)
               {
                  _compendium.AddRace(raceModel);
               }
               else
               {
                  raceModel.ID = existing.ID;
                  _compendium.UpdateRace(raceModel);
               }
            }
         }
      }

      private void ImportSpells()
      {
         foreach (SpellModel spellModel in _spells)
         {
            if (_addAllEntries)
            {
               _compendium.AddSpell(spellModel);
            }
            else if (_skipDuplicateEntries)
            {
               if (!_compendium.Spells.Any(x => x.Name.Equals(spellModel.Name, StringComparison.CurrentCultureIgnoreCase)))
               {
                  _compendium.AddSpell(spellModel);
               }
            }
            else if (_replaceExistingEntries)
            {
               SpellModel existing = _compendium.Spells.FirstOrDefault(x => x.Name.Equals(spellModel.Name, StringComparison.CurrentCultureIgnoreCase));
               if (existing == null)
               {
                  _compendium.AddSpell(spellModel);
               }
               else
               {
                  spellModel.ID = existing.ID;
                  _compendium.UpdateSpell(spellModel);
               }
            }
         }
      }

      #endregion

      #region Protected Methods

      protected override void OnAccept()
      {
         Mouse.OverrideCursor = Cursors.Wait;

         if (_backgrounds.Any())
         {
            ImportBackgrounds();
            _compendium.SaveBackgrounds();
         }
         if (_classes.Any())
         {
            ImportClasses();
            _compendium.SaveClasses();
         }
         if (_conditions.Any())
         {
            ImportConditons();
            _compendium.SaveConditions();
         }
         if (_feats.Any())
         {
            ImportFeats();
            _compendium.SaveFeats();
         }
         if (_items.Any())
         {
            ImportItems();
            _compendium.SaveItems();
         }
         if (_languages.Any())
         {
            ImportLanguages();
            _compendium.SaveLanguages();
         }
         if (_monsters.Any())
         {
            ImportMonsters();
            _compendium.SaveMonsters();
         }
         if (_races.Any())
         {
            ImportRaces();
            _compendium.SaveRaces();
         }
         if (_spells.Any())
         {
            ImportSpells();
            _compendium.SaveSpells();
         }
         if (_characters.Any())
         {
            ImportCharacters();
            _compendium.SaveCharacters();
         }
         if (_encounters.Any())
         {
            ImportEncounters();
            _compendium.SaveEncounters();
         }

         _compendium.NotifyImportComplete();

         Mouse.OverrideCursor = null;

         base.OnAccept();
      }

      #endregion
   }
}
