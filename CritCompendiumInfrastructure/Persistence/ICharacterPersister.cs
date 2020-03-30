using System.Collections.Generic;
using CriticalCompendiumInfrastructure.Models;

namespace CriticalCompendiumInfrastructure.Persistence
{
    public interface ICharacterPersister
    {
        /// <summary>
        /// Gets character bytes
        /// </summary>
        byte[] GetBytes(IEnumerable<CharacterModel> characters);
        
        /// <summary>
        /// Gets characters from bytes
        /// </summary>
        IEnumerable<CharacterModel> GetCharacters(byte[] bytes,
            IEnumerable<BackgroundModel> backgrounds, IEnumerable<ClassModel> classes, IEnumerable<ConditionModel> conditions, IEnumerable<FeatModel> feats, IEnumerable<ItemModel> items,
            IEnumerable<LanguageModel> languages, IEnumerable<MonsterModel> monsters, IEnumerable<RaceModel> races, IEnumerable<SpellModel> spells);
    }
}
