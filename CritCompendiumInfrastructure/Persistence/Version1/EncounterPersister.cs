using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;

namespace CritCompendiumInfrastructure.Persistence.Version1
{
   public sealed class EncounterPersister : IEncounterPersister
   {
      #region Fields

      private readonly int _version = 1;

      #endregion

      #region Public Methods

      /// <inheritdoc />
      public byte[] GetBytes(IEnumerable<EncounterModel> encounters)
      {
         List<byte> encounterBytes = new List<byte>();

         encounterBytes.AddRange(BitConverter.GetBytes(_version));
         encounterBytes.AddRange(BitConverter.GetBytes(encounters.Count()));

         foreach (EncounterModel encounter in encounters)
         {
            encounterBytes.AddRange(encounter.Id.ToByteArray());
            encounterBytes.AddRange(StringBytes(encounter.Name));

            encounterBytes.AddRange(BitConverter.GetBytes(encounter.Creatures.Count()));
            foreach (EncounterCreatureModel encounterCreature in encounter.Creatures)
            {
               if (encounterCreature is EncounterCharacterModel)
               {
                  encounterBytes.AddRange(BitConverter.GetBytes(true));
               }
               else if (encounterCreature is EncounterMonsterModel)
               {
                  encounterBytes.AddRange(BitConverter.GetBytes(false));
               }

               encounterBytes.AddRange(encounterCreature.ID.ToByteArray());
               encounterBytes.AddRange(StringBytes(encounterCreature.Name));
               encounterBytes.AddRange(BitConverter.GetBytes(encounterCreature.CurrentHP));
               encounterBytes.AddRange(BitConverter.GetBytes(encounterCreature.MaxHP));
               encounterBytes.AddRange(BitConverter.GetBytes(encounterCreature.AC));
               encounterBytes.AddRange(BitConverter.GetBytes(encounterCreature.SpellSaveDC));
               encounterBytes.AddRange(BitConverter.GetBytes(encounterCreature.PassivePerception));
               encounterBytes.AddRange(BitConverter.GetBytes(encounterCreature.InitiativeBonus));
               encounterBytes.AddRange(BitConverter.GetBytes(encounterCreature.Initiative != null));
               encounterBytes.AddRange(BitConverter.GetBytes(encounterCreature.Initiative.HasValue ? encounterCreature.Initiative.Value : 0));
               encounterBytes.AddRange(BitConverter.GetBytes(encounterCreature.Selected));

               encounterBytes.AddRange(BitConverter.GetBytes(encounterCreature.Conditions.Count));
               foreach (AppliedConditionModel condition in encounterCreature.Conditions)
               {
                  encounterBytes.AddRange(condition.ID.ToByteArray());
                  if (condition.ConditionModel != null)
                  {
                     encounterBytes.AddRange(condition.ConditionModel.Id.ToByteArray());
                     encounterBytes.AddRange(StringBytes(condition.ConditionModel.Name));
                  }
                  else
                  {
                     encounterBytes.AddRange(Guid.Empty.ToByteArray());
                     encounterBytes.AddRange(BitConverter.GetBytes(0));
                  }
                  encounterBytes.AddRange(StringBytes(condition.Name));
                  encounterBytes.AddRange(BitConverter.GetBytes(condition.Level.HasValue));
                  encounterBytes.AddRange(BitConverter.GetBytes(condition.Level.HasValue ? condition.Level.Value : 0));
                  encounterBytes.AddRange(StringBytes(condition.Notes));
               }

               if (encounterCreature is EncounterCharacterModel encounterCharacter)
               {
                  if (encounterCharacter.CharacterModel != null)
                  {
                     encounterBytes.AddRange(encounterCharacter.CharacterModel.Id.ToByteArray());
                     encounterBytes.AddRange(StringBytes(encounterCharacter.CharacterModel.Name));
                  }
                  else
                  {
                     encounterBytes.AddRange(Guid.Empty.ToByteArray());
                     encounterBytes.AddRange(BitConverter.GetBytes(0));
                  }

                  encounterBytes.AddRange(BitConverter.GetBytes(encounterCharacter.Level));
                  encounterBytes.AddRange(BitConverter.GetBytes(encounterCharacter.PassiveInvestigation));
               }
               else if (encounterCreature is EncounterMonsterModel encounterMonster)
               {
                  if (encounterMonster.MonsterModel != null)
                  {
                     encounterBytes.AddRange(encounterMonster.MonsterModel.Id.ToByteArray());
                     encounterBytes.AddRange(StringBytes(encounterMonster.MonsterModel.Name));
                  }
                  else
                  {
                     encounterBytes.AddRange(Guid.Empty.ToByteArray());
                     encounterBytes.AddRange(BitConverter.GetBytes(0));
                  }

                  encounterBytes.AddRange(BitConverter.GetBytes(encounterMonster.Quantity));
                  encounterBytes.AddRange(BitConverter.GetBytes(encounterMonster.AverageDamageTurn));
                  encounterBytes.AddRange(StringBytes(encounterMonster.CR));
                  encounterBytes.AddRange(StringBytes(encounterMonster.DamageVulnerabilities));
                  encounterBytes.AddRange(StringBytes(encounterMonster.DamageResistances));
                  encounterBytes.AddRange(StringBytes(encounterMonster.DamageImmunities));
                  encounterBytes.AddRange(StringBytes(encounterMonster.ConditionImmunities));
               }
            }

            encounterBytes.AddRange(BitConverter.GetBytes(encounter.Round));
            encounterBytes.AddRange(BitConverter.GetBytes((int)encounter.EncounterChallenge));
            encounterBytes.AddRange(BitConverter.GetBytes(encounter.TotalCharacterHP));
            encounterBytes.AddRange(BitConverter.GetBytes(encounter.TotalMonsterHP));
            encounterBytes.AddRange(BitConverter.GetBytes(encounter.TimeElapsed));
            encounterBytes.AddRange(BitConverter.GetBytes(encounter.CurrentTurn));
            encounterBytes.AddRange(BitConverter.GetBytes((int)encounter.EncounterState));
            encounterBytes.AddRange(StringBytes(encounter.Notes));
         }

         return encounterBytes.ToArray();
      }

      /// <inheritdoc />
      public IEnumerable<EncounterModel> GetEncounters(byte[] encounterBytes, IEnumerable<CharacterModel> characters, IEnumerable<ConditionModel> conditions, IEnumerable<MonsterModel> monsters)
      {
         List<EncounterModel> encounters = new List<EncounterModel>();

         using (MemoryStream memoryStream = new MemoryStream(encounterBytes))
         {
            using (BinaryReader reader = new BinaryReader(memoryStream))
            {
               int version = BitConverter.ToInt16(reader.ReadBytes(4), 0);
               if (version == _version)
               {
                  int encounterCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                  for (int i = 0; i < encounterCount; ++i)
                  {
                     EncounterModel encounter = new EncounterModel();

                     encounter.Id = new Guid(reader.ReadBytes(16));
                     encounter.Name = ReadNextString(reader);

                     encounter.Creatures = new List<EncounterCreatureModel>();
                     int creatureCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     for (int j = 0; j < creatureCount; ++j)
                     {
                        bool isCharacter = BitConverter.ToBoolean(reader.ReadBytes(1), 0);

                        EncounterCreatureModel encounterCreature = isCharacter ? new EncounterCharacterModel() : new EncounterMonsterModel() as EncounterCreatureModel;
                        encounterCreature.ID = new Guid(reader.ReadBytes(16));
                        encounterCreature.Name = ReadNextString(reader);
                        encounterCreature.CurrentHP = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        encounterCreature.MaxHP = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        encounterCreature.AC = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        encounterCreature.SpellSaveDC = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        encounterCreature.PassivePerception = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        encounterCreature.InitiativeBonus = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        bool initiativeSet = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                        int? initiative = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        encounterCreature.Initiative = initiativeSet ? initiative : null;
                        encounterCreature.Selected = BitConverter.ToBoolean(reader.ReadBytes(1), 0);

                        encounterCreature.Conditions = new List<AppliedConditionModel>();
                        int conditionCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        for (int k = 0; k < conditionCount; ++k)
                        {
                           AppliedConditionModel appliedCondition = new AppliedConditionModel();
                           appliedCondition.ID = new Guid(reader.ReadBytes(16));

                           Guid conditionID = new Guid(reader.ReadBytes(16));
                           string conditionName = ReadNextString(reader);
                           ConditionModel conditionModel = conditions.FirstOrDefault(x => x.Id == conditionID);
                           if (conditionModel == null)
                           {
                              conditionModel = conditions.FirstOrDefault(x => x.Name.Equals(conditionName, StringComparison.CurrentCultureIgnoreCase));
                           }
                           appliedCondition.ConditionModel = conditionModel;

                           appliedCondition.Name = ReadNextString(reader);
                           bool hasLevel = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                           int? level = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                           appliedCondition.Level = hasLevel ? level : null;
                           appliedCondition.Notes = ReadNextString(reader);

                           encounterCreature.Conditions.Add(appliedCondition);
                        }

                        if (isCharacter)
                        {
                           Guid characterID = new Guid(reader.ReadBytes(16));
                           string characterName = ReadNextString(reader);
                           CharacterModel character = characters.FirstOrDefault(x => x.Id == characterID);
                           if (character == null)
                           {
                              character = characters.FirstOrDefault(x => x.Name.Equals(characterName, StringComparison.CurrentCultureIgnoreCase));
                           }
                                ((EncounterCharacterModel)encounterCreature).CharacterModel = character;
                           ((EncounterCharacterModel)encounterCreature).Level = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                           ((EncounterCharacterModel)encounterCreature).PassiveInvestigation = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        }
                        else
                        {
                           Guid monsterID = new Guid(reader.ReadBytes(16));
                           string monsterName = ReadNextString(reader);
                           MonsterModel monster = monsters.FirstOrDefault(x => x.Id == monsterID);
                           if (monster == null)
                           {
                              monster = monsters.FirstOrDefault(x => x.Name.Equals(monsterName, StringComparison.CurrentCultureIgnoreCase));
                           }
                            ((EncounterMonsterModel)encounterCreature).MonsterModel = monster;
                           ((EncounterMonsterModel)encounterCreature).Quantity = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                           ((EncounterMonsterModel)encounterCreature).AverageDamageTurn = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                           ((EncounterMonsterModel)encounterCreature).CR = ReadNextString(reader);
                           ((EncounterMonsterModel)encounterCreature).DamageVulnerabilities = ReadNextString(reader);
                           ((EncounterMonsterModel)encounterCreature).DamageResistances = ReadNextString(reader);
                           ((EncounterMonsterModel)encounterCreature).DamageImmunities = ReadNextString(reader);
                           ((EncounterMonsterModel)encounterCreature).ConditionImmunities = ReadNextString(reader);
                        }

                        encounter.Creatures.Add(encounterCreature);
                     }

                     encounter.Round = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     encounter.EncounterChallenge = (EncounterChallenge)BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     encounter.TotalCharacterHP = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     encounter.TotalMonsterHP = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     encounter.TimeElapsed = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     encounter.CurrentTurn = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     encounter.EncounterState = (EncounterState)BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     encounter.Notes = ReadNextString(reader);

                     encounters.Add(encounter);
                  }
               }
            }
         }

         return encounters;
      }

      #endregion

      #region Private Methods

      private byte[] StringBytes(string s)
      {
         List<byte> bytes = new List<byte>();

         if (!String.IsNullOrWhiteSpace(s))
         {
            byte[] sBytes = Encoding.UTF8.GetBytes(s);
            bytes.AddRange(BitConverter.GetBytes(sBytes.Length));
            bytes.AddRange(sBytes);
         }
         else
         {
            bytes.AddRange(BitConverter.GetBytes(0));
         }

         return bytes.ToArray();
      }

      private string ReadNextString(BinaryReader reader)
      {
         string s = String.Empty;

         int length = BitConverter.ToInt32(reader.ReadBytes(4), 0);
         if (length > 0)
         {
            s = Encoding.UTF8.GetString(reader.ReadBytes(length));
         }

         return s;
      }

      #endregion
   }
}
