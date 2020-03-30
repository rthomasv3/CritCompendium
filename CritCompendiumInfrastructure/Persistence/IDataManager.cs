using System.Collections.Generic;
using CriticalCompendiumInfrastructure.Models;

namespace CriticalCompendiumInfrastructure.Persistence
{
    public interface IDataManager
    {
        #region Properties

        /// <summary>
        /// True if first launch
        /// </summary>
        bool FirstLaunch
        {
            get;
        }

        /// <summary>
        /// Gets run count
        /// </summary>
        int RunCount
        {
            get;
        }

        #endregion

        #region Characters

        /// <summary>
        /// Saves characters to the default save file location
        /// </summary>
        void SaveCharacters(IEnumerable<CharacterModel> characters);

        /// <summary>
        /// Gets character bytes
        /// </summary>
        byte[] GetCharacterBytes(CharacterModel character);

        /// <summary>
        /// Creates a character archive from the character model
        /// </summary>
        byte[] CreateCharacterArchive(CharacterModel characterModel);

        /// <summary>
        /// Loads characters from the default save file location
        /// </summary>
        IEnumerable<CharacterModel> LoadCharacters(IEnumerable<BackgroundModel> backgrounds, IEnumerable<ClassModel> classes, IEnumerable<ConditionModel> conditions, IEnumerable<FeatModel> feats,
                                                   IEnumerable<ItemModel> items, IEnumerable<LanguageModel> languages, IEnumerable<MonsterModel> monsters, IEnumerable<RaceModel> races, IEnumerable<SpellModel> spells);

        /// <summary>
        /// Gets a character from the byte array using selected resources
        /// </summary>
        CharacterModel GetCharacter(byte[] characterBytes, IEnumerable<BackgroundModel> backgrounds, IEnumerable<ClassModel> classes, IEnumerable<ConditionModel> conditions, IEnumerable<FeatModel> feats,
                                    IEnumerable<ItemModel> items, IEnumerable<LanguageModel> languages, IEnumerable<MonsterModel> monsters, IEnumerable<RaceModel> races, IEnumerable<SpellModel> spells);

        #endregion

        #region Adventures

        /// <summary>
        /// Saves adventures to the default save file location
        /// </summary>
        void SaveAdventures(IEnumerable<AdventureModel> adventures);

        /// <summary>
        /// Gets adventure bytes
        /// </summary>
        byte[] GetAdventureBytes(AdventureModel adventure);

        /// <summary>
        /// Creates a adventure archive from the adventure model
        /// </summary>
        byte[] CreateAdventureArchive(AdventureModel adventure);

        /// <summary>
        /// Loads adventures from the default save file location
        /// </summary>
        IEnumerable<AdventureModel> LoadAdventures(IEnumerable<NPCModel> npcs, IEnumerable<LocationModel> locations, IEnumerable<EncounterModel> encounters);

        /// <summary>
        /// Gets a adventure from the byte array using selected resources
        /// </summary>
        AdventureModel GetAdventure(byte[] adventureBytes, IEnumerable<NPCModel> npcs, IEnumerable<LocationModel> locations, IEnumerable<EncounterModel> encounters);

        #endregion

        #region Encounters

        /// <summary>
        /// Saves encounters to the default save file location
        /// </summary>
        void SaveEncounters(IEnumerable<EncounterModel> encounters);

        /// <summary>
        /// Gets encounter bytes
        /// </summary>
        byte[] GetEncounterBytes(EncounterModel encounter);

        /// <summary>
        /// Creates a encounter archive from the encounter model
        /// </summary>
        byte[] CreateEncounterArchive(EncounterModel encounterModel);

        /// <summary>
        /// Loads encounters from the default save file location
        /// </summary>
        IEnumerable<EncounterModel> LoadEncounters(IEnumerable<CharacterModel> characters, IEnumerable<ConditionModel> conditions, IEnumerable<MonsterModel> monsters);

        /// <summary>
        /// Gets a encounter from the byte array using selected resources
        /// </summary>
        EncounterModel GetEncounter(byte[] encounterBytes, IEnumerable<CharacterModel> characters, IEnumerable<ConditionModel> conditions, IEnumerable<MonsterModel> monsters);

        #endregion

        #region Locations

        /// <summary>
        /// Saves locations to the default save file location
        /// </summary>
        void SaveLocations(IEnumerable<LocationModel> locations);

        /// <summary>
        /// Gets location bytes
        /// </summary>
        byte[] GetLocationBytes(LocationModel location);

        /// <summary>
        /// Creates a location archive from the location model
        /// </summary>
        byte[] CreateLocationArchive(LocationModel location);

        /// <summary>
        /// Loads locations from the default save file location
        /// </summary>
        IEnumerable<LocationModel> LoadLocations();

        /// <summary>
        /// Gets a location from the byte array
        /// </summary>
        LocationModel GetLocation(byte[] locationBytes);

        #endregion

        #region NPCs

        /// <summary>
        /// Saves npcs to the default save file location
        /// </summary>
        void SaveNPCs(IEnumerable<NPCModel> npcs);

        /// <summary>
        /// Gets npcs bytes
        /// </summary>
        byte[] GetNPCBytes(NPCModel npcModel);

        /// <summary>
        /// Creates a npc archive from the npc model
        /// </summary>
        byte[] CreateNPCArchive(NPCModel npcModel);

        /// <summary>
        /// Loads npcs from the default save file location
        /// </summary>
        IEnumerable<NPCModel> LoadNPCs();

        /// <summary>
        /// Gets a npc from the byte array
        /// </summary>
        NPCModel GetNPC(byte[] npcBytes);

        #endregion

        #region Tables

        /// <summary>
        /// Saves tables to the default save file location
        /// </summary>
        void SaveTables(IEnumerable<RandomTableModel> randomTables);

        /// <summary>
        /// Gets table bytes
        /// </summary>
        byte[] GetTableBytes(RandomTableModel randomTableModel);

        /// <summary>
        /// Creates a table archive from the table model
        /// </summary>
        byte[] CreateTableArchive(RandomTableModel randomTableModel);

        /// <summary>
        /// Loads tables from the default save file location
        /// </summary>
        IEnumerable<RandomTableModel> LoadTables();

        /// <summary>
        /// Gets a table from the byte array
        /// </summary>
        RandomTableModel GetTable(byte[] tableBytes);

        #endregion

        #region Backgrounds

        /// <summary>
        /// Saves backgrounds to the default save file location
        /// </summary>
        void SaveBackgrounds(IEnumerable<BackgroundModel> backgrounds);

        /// <summary>
        /// Loads backgrounds from the default save file location
        /// </summary>
        IEnumerable<BackgroundModel> LoadBackgrounds();

        #endregion

        #region Classes

        /// <summary>
        /// Saves classes to the default save file location
        /// </summary>
        void SaveClasses(IEnumerable<ClassModel> classes);

        /// <summary>
        /// Loads classes from the default save file location
        /// </summary>
        IEnumerable<ClassModel> LoadClasses();

        #endregion

        #region Conditions

        /// <summary>
        /// Saves conditions to the default save file location
        /// </summary>
        void SaveConditions(IEnumerable<ConditionModel> conditions);

        /// <summary>
        /// Loads conditions from the default save file location
        /// </summary>
        IEnumerable<ConditionModel> LoadConditions();

        #endregion

        #region Feates

        /// <summary>
        /// Saves feats to the default save file location
        /// </summary>
        void SaveFeats(IEnumerable<FeatModel> feats);

        /// <summary>
        /// Loads feats from the default save file location
        /// </summary>
        IEnumerable<FeatModel> LoadFeats();

        #endregion

        #region Items

        /// <summary>
        /// Saves items to the default save file location
        /// </summary>
        void SaveItems(IEnumerable<ItemModel> items);

        /// <summary>
        /// Loads items from the default save file location
        /// </summary>
        IEnumerable<ItemModel> LoadItems();

        #endregion

        #region Languages

        /// <summary>
        /// Saves languages to the default save file location
        /// </summary>
        void SaveLanguages(IEnumerable<LanguageModel> languages);

        /// <summary>
        /// Loads languages from the default save file location
        /// </summary>
        IEnumerable<LanguageModel> LoadLanguages();

        #endregion

        #region Monsters

        /// <summary>
        /// Saves monsters to the default save file location
        /// </summary>
        void SaveMonsters(IEnumerable<MonsterModel> monsters);

        /// <summary>
        /// Loads monsters from the default save file location
        /// </summary>
        IEnumerable<MonsterModel> LoadMonsters();

        #endregion

        #region Races

        /// <summary>
        /// Saves races to the default save file location
        /// </summary>
        void SaveRaces(IEnumerable<RaceModel> races);

        /// <summary>
        /// Loads races from the default save file location
        /// </summary>
        IEnumerable<RaceModel> LoadRacess();

        #endregion

        #region Spells

        /// <summary>
        /// Saves spells to the default save file location
        /// </summary>
        void SaveSpells(IEnumerable<SpellModel> spells);

        /// <summary>
        /// Loads spells from the default save file location
        /// </summary>
        IEnumerable<SpellModel> LoadSpells();

        #endregion

        #region Theme

        /// <summary>
        /// Saves the theme
        /// </summary>
        void SaveTheme(byte[] themeBytes);

        /// <summary>
        /// Gets theme file bytes
        /// </summary>
        byte[] LoadTheme();

        #endregion

        #region Private Methods

        /// <summary>
        /// Saves the launch information data file
        /// </summary>
        void SaveLaunchData();

        #endregion
    }
}
