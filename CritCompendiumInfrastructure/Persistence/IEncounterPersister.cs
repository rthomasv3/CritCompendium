using System.Collections.Generic;
using CriticalCompendiumInfrastructure.Models;

namespace CriticalCompendiumInfrastructure.Persistence
{
    public interface IEncounterPersister
    {
        /// <summary>
        /// Gets character bytes
        /// </summary>
        byte[] GetBytes(IEnumerable<EncounterModel> encounters);

        /// <summary>
        /// Gets encounters from bytes
        /// </summary>
        IEnumerable<EncounterModel> GetEncounters(byte[] bytes, IEnumerable<CharacterModel> characters, IEnumerable<ConditionModel> conditions, IEnumerable<MonsterModel> monsters);
    }
}
