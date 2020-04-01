using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;

namespace CritCompendiumInfrastructure.Persistence.Version2
{
   public sealed class CharacterPersister : ICharacterPersister
   {
      #region Fields

      private readonly int _version = 2;

      #endregion

      #region Public Methods

      /// <inheritdoc />
      public byte[] GetBytes(IEnumerable<CharacterModel> characters)
      {
         List<byte> characterBytes = new List<byte>();

         characterBytes.AddRange(BitConverter.GetBytes(_version));

         characterBytes.AddRange(BitConverter.GetBytes(characters.Count()));
         foreach (CharacterModel character in characters)
         {
            characterBytes.AddRange(character.ID.ToByteArray());

            characterBytes.AddRange(StringBytes(character.Name));

            characterBytes.AddRange(BitConverter.GetBytes(character.CurrentHP));
            characterBytes.AddRange(BitConverter.GetBytes(character.TempHP));
            characterBytes.AddRange(BitConverter.GetBytes(character.DeathSaveSuccesses));
            characterBytes.AddRange(BitConverter.GetBytes(character.DeathSaveFailures));

            if (character.Race != null)
            {
               characterBytes.AddRange(character.Race.ID.ToByteArray());
               characterBytes.AddRange(StringBytes(character.Race.Name));
            }
            else
            {
               characterBytes.AddRange(Guid.Empty.ToByteArray());
               characterBytes.AddRange(BitConverter.GetBytes(0));
            }

            if (character.Background != null)
            {
               characterBytes.AddRange(character.Background.ID.ToByteArray());
               characterBytes.AddRange(StringBytes(character.Background.Name));
            }
            else
            {
               characterBytes.AddRange(Guid.Empty.ToByteArray());
               characterBytes.AddRange(BitConverter.GetBytes(0));
            }

            characterBytes.AddRange(BitConverter.GetBytes((int)character.Alignment));

            characterBytes.AddRange(character.MovementModel.ID.ToByteArray());
            characterBytes.AddRange(BitConverter.GetBytes(character.MovementModel.WalkSpeed));
            characterBytes.AddRange(BitConverter.GetBytes(character.MovementModel.SwimSpeed));
            characterBytes.AddRange(BitConverter.GetBytes(character.MovementModel.ClimbSpeed));
            characterBytes.AddRange(BitConverter.GetBytes(character.MovementModel.CrawlSpeed));
            characterBytes.AddRange(BitConverter.GetBytes(character.MovementModel.FlySpeed));
            characterBytes.AddRange(BitConverter.GetBytes(character.MovementModel.ApplyEncumbrance));

            characterBytes.AddRange(BitConverter.GetBytes(character.Inspiration));
            characterBytes.AddRange(StringBytes(character.Deity));
            characterBytes.AddRange(StringBytes(character.Gender));
            characterBytes.AddRange(BitConverter.GetBytes(character.Age));
            characterBytes.AddRange(BitConverter.GetBytes(character.HeightFeet));
            characterBytes.AddRange(BitConverter.GetBytes(character.HeightInches));
            characterBytes.AddRange(BitConverter.GetBytes(character.Weight));
            characterBytes.AddRange(StringBytes(character.Hair));
            characterBytes.AddRange(StringBytes(character.Eyes));
            characterBytes.AddRange(StringBytes(character.Skin));
            characterBytes.AddRange(BitConverter.GetBytes(character.Experience));

            characterBytes.AddRange(BitConverter.GetBytes(character.BaseStrength));
            characterBytes.AddRange(BitConverter.GetBytes(character.BaseDexterity));
            characterBytes.AddRange(BitConverter.GetBytes(character.BaseConstitution));
            characterBytes.AddRange(BitConverter.GetBytes(character.BaseIntelligence));
            characterBytes.AddRange(BitConverter.GetBytes(character.BaseWisdom));
            characterBytes.AddRange(BitConverter.GetBytes(character.BaseCharisma));

            characterBytes.AddRange(BitConverter.GetBytes(character.BonusStrength));
            characterBytes.AddRange(BitConverter.GetBytes(character.BonusDexterity));
            characterBytes.AddRange(BitConverter.GetBytes(character.BonusConstitution));
            characterBytes.AddRange(BitConverter.GetBytes(character.BonusIntelligence));
            characterBytes.AddRange(BitConverter.GetBytes(character.BonusWisdom));
            characterBytes.AddRange(BitConverter.GetBytes(character.BonusCharisma));

            characterBytes.AddRange(StringBytes(character.PersonalityTraits));
            characterBytes.AddRange(StringBytes(character.Ideals));
            characterBytes.AddRange(StringBytes(character.Bonds));
            characterBytes.AddRange(StringBytes(character.Flaws));
            characterBytes.AddRange(StringBytes(character.Backstory));

            characterBytes.AddRange(BitConverter.GetBytes(character.Levels.Count));
            foreach (LevelModel level in character.Levels)
            {
               characterBytes.AddRange(level.ID.ToByteArray());
               characterBytes.AddRange(BitConverter.GetBytes(level.Level));
               characterBytes.AddRange(BitConverter.GetBytes(level.LevelOfClass));

               if (level.Class != null)
               {
                  characterBytes.AddRange(level.Class.ID.ToByteArray());
                  characterBytes.AddRange(StringBytes(level.Class.Name));
               }
               else
               {
                  characterBytes.AddRange(Guid.Empty.ToByteArray());
                  characterBytes.AddRange(BitConverter.GetBytes(0));
               }

               characterBytes.AddRange(BitConverter.GetBytes(level.Features.Count));
               foreach (FeatureModel feature in level.Features)
               {
                  characterBytes.AddRange(feature.ID.ToByteArray());
                  characterBytes.AddRange(StringBytes(feature.Name));
               }

               characterBytes.AddRange(BitConverter.GetBytes(level.Feats.Count));
               foreach (FeatModel feat in level.Feats)
               {
                  characterBytes.AddRange(feat.ID.ToByteArray());
                  characterBytes.AddRange(StringBytes(feat.Name));
               }

               characterBytes.AddRange(BitConverter.GetBytes(level.HitDieResult));
               characterBytes.AddRange(BitConverter.GetBytes(level.HitDieUsed));
               characterBytes.AddRange(BitConverter.GetBytes(level.HitDieRestRoll));
               characterBytes.AddRange(BitConverter.GetBytes(level.AdditionalHP));

               characterBytes.AddRange(BitConverter.GetBytes(level.AbilityScoreImprovements.Count));
               foreach (KeyValuePair<Ability, int> abilityPair in level.AbilityScoreImprovements)
               {
                  characterBytes.AddRange(BitConverter.GetBytes((int)abilityPair.Key));
                  characterBytes.AddRange(BitConverter.GetBytes(abilityPair.Value));
               }
            }

            characterBytes.AddRange(BitConverter.GetBytes((int)character.AbilityRollMethod));
            characterBytes.AddRange(BitConverter.GetBytes(character.TimesAbilityScoresRolled));

            characterBytes.AddRange(BitConverter.GetBytes(character.AbilitySaveProficiencies.Count));
            foreach (AbilityModel ability in character.AbilitySaveProficiencies)
            {
               characterBytes.AddRange(BitConverter.GetBytes((int)ability.Ability));
               characterBytes.AddRange(BitConverter.GetBytes(ability.SaveBonus));
               characterBytes.AddRange(BitConverter.GetBytes(ability.CheckBonus));
               characterBytes.AddRange(BitConverter.GetBytes(ability.Proficient));
            }

            characterBytes.AddRange(BitConverter.GetBytes(character.SkillProficiencies.Count));
            foreach (SkillModel skill in character.SkillProficiencies)
            {
               characterBytes.AddRange(BitConverter.GetBytes((int)skill.Skill));
               characterBytes.AddRange(BitConverter.GetBytes(skill.Bonus));
               characterBytes.AddRange(BitConverter.GetBytes(skill.Proficient));
               characterBytes.AddRange(BitConverter.GetBytes(skill.Expertise));
            }

            characterBytes.AddRange(BitConverter.GetBytes(character.Languages.Count));
            foreach (LanguageModel language in character.Languages)
            {
               characterBytes.AddRange(language.ID.ToByteArray());
               characterBytes.AddRange(StringBytes(language.Name));
            }

            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.LightArmorProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.MediumArmorProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.HeavyArmorProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.ShieldsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.PaddedProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.LeatherProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.StuddedLeatherProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.HideProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.ChainShirtProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.ScaleMailProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.BreastplateProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.HalfPlateProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.RingMailProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.ChainMailProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.SplintProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.PlateProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorProficiency.ShieldProficiency));

            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.SimpleWeaponsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.MartialWeaponsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.ClubProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.DaggerProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.GreatclubProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.HandaxeProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.JavelinProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.LightHammerProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.MaceProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.QuarterstaffProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.SickleProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.SpearProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.CrossbowLightProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.DartProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.ShortbowProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.SlingProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.BattleaxeProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.FlailProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.GlaiveProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.GreataxeProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.GreatswordProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.HalberdProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.LanceProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.LongswordProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.MaulProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.MorningstarProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.PikeProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.RapierProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.ScimitarProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.ShortswordProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.TridentProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.WarPickProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.WarhammerProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.WhipProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.BlowgunProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.CrossbowHandProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.CrossbowHeavyProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.LongbowProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.WeaponProficiency.NetProficiency));

            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.AlchemistsSuppliesProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.BrewersSuppliesProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.CalligraphersSuppliesProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.CarpentersToolsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.CartographersToolsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.CobblersToolsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.CooksUtensilsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.GlassblowersToolsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.JewelerssToolsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.LeatherworkersToolsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.MasonsToolsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.PaintersSuppliesProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.PottersToolsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.SmithsToolsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.TinkersToolsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.WeaversToolsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.WoodcarversToolsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.DiceSetProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.PlayingCardSetProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.DisguiseKitProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.ForgeryKitProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.HerbalismKitProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.PoisonersKitProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.BagpipesProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.DrumProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.DulcimerProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.FluteProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.LuteProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.LyreProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.HornProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.PanFluteProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.ShawmProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.ViolProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.NavigatorsToolsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.ThievesToolsProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.LandVehiclesProficiency));
            characterBytes.AddRange(BitConverter.GetBytes(character.ToolProficiency.WaterVehiclesProficiency));

            characterBytes.AddRange(BitConverter.GetBytes(character.Conditions.Count));
            foreach (AppliedConditionModel condition in character.Conditions)
            {
               characterBytes.AddRange(condition.ID.ToByteArray());
               if (condition.ConditionModel != null)
               {
                  characterBytes.AddRange(condition.ConditionModel.ID.ToByteArray());
                  characterBytes.AddRange(StringBytes(condition.ConditionModel.Name));
               }
               else
               {
                  characterBytes.AddRange(Guid.Empty.ToByteArray());
                  characterBytes.AddRange(BitConverter.GetBytes(0));
               }
               characterBytes.AddRange(StringBytes(condition.Name));
               characterBytes.AddRange(BitConverter.GetBytes(condition.Level.HasValue));
               characterBytes.AddRange(BitConverter.GetBytes(condition.Level.HasValue ? condition.Level.Value : 0));
               characterBytes.AddRange(StringBytes(condition.Notes));
            }

            characterBytes.AddRange(StringBytes(character.SavingThrowNotes));

            characterBytes.AddRange(BitConverter.GetBytes(character.Attacks.Count));
            foreach (AttackModel attack in character.Attacks)
            {
               characterBytes.AddRange(attack.ID.ToByteArray());
               characterBytes.AddRange(StringBytes(attack.Name));
               characterBytes.AddRange(BitConverter.GetBytes(attack.NumberOfDamageDice));
               characterBytes.AddRange(StringBytes(attack.DamageDie));
               characterBytes.AddRange(BitConverter.GetBytes((int)attack.Ability));
               characterBytes.AddRange(BitConverter.GetBytes(attack.Proficient));
               characterBytes.AddRange(BitConverter.GetBytes((int)attack.DamageType));
               characterBytes.AddRange(BitConverter.GetBytes(attack.AdditionalToHitBonus));
               characterBytes.AddRange(BitConverter.GetBytes(attack.AdditionalDamageBonus));
               characterBytes.AddRange(StringBytes(attack.Range));
               characterBytes.AddRange(StringBytes(attack.Notes));
               characterBytes.AddRange(BitConverter.GetBytes(attack.ShowToHit));
               characterBytes.AddRange(BitConverter.GetBytes(attack.ShowDamage));
               characterBytes.AddRange(StringBytes(attack.ToHit));
               characterBytes.AddRange(StringBytes(attack.Damage));
            }

            characterBytes.AddRange(BitConverter.GetBytes(character.Counters.Count));
            foreach (CounterModel counter in character.Counters)
            {
               characterBytes.AddRange(counter.ID.ToByteArray());
               characterBytes.AddRange(StringBytes(counter.Name));
               characterBytes.AddRange(BitConverter.GetBytes(counter.CurrentValue));
               characterBytes.AddRange(BitConverter.GetBytes(counter.MaxValue));
               characterBytes.AddRange(BitConverter.GetBytes(counter.ResetOnShortRest));
               characterBytes.AddRange(BitConverter.GetBytes(counter.ResetOnLongRest));
               characterBytes.AddRange(StringBytes(counter.Notes));
            }

            characterBytes.AddRange(BitConverter.GetBytes(character.Companions.Count));
            foreach (CompanionModel companion in character.Companions)
            {
               characterBytes.AddRange(companion.ID.ToByteArray());
               if (companion.MonsterModel != null)
               {
                  characterBytes.AddRange(companion.MonsterModel.ID.ToByteArray());
                  characterBytes.AddRange(StringBytes(companion.MonsterModel.Name));
               }
               else
               {
                  characterBytes.AddRange(Guid.Empty.ToByteArray());
                  characterBytes.AddRange(BitConverter.GetBytes(0));
               }
               characterBytes.AddRange(StringBytes(companion.Name));
               characterBytes.AddRange(BitConverter.GetBytes(companion.CurrentHP));
               characterBytes.AddRange(BitConverter.GetBytes(companion.MaxHP));
               characterBytes.AddRange(StringBytes(companion.Notes));
            }

            characterBytes.AddRange(BitConverter.GetBytes(character.Bags.Count));
            foreach (BagModel bag in character.Bags)
            {
               characterBytes.AddRange(bag.ID.ToByteArray());
               characterBytes.AddRange(StringBytes(bag.Name));
               characterBytes.AddRange(BitConverter.GetBytes(bag.FixedWeight));
               characterBytes.AddRange(BitConverter.GetBytes(bag.FixedWeightValue));
               characterBytes.AddRange(BitConverter.GetBytes(bag.Copper));
               characterBytes.AddRange(BitConverter.GetBytes(bag.Silver));
               characterBytes.AddRange(BitConverter.GetBytes(bag.Electrum));
               characterBytes.AddRange(BitConverter.GetBytes(bag.Gold));
               characterBytes.AddRange(BitConverter.GetBytes(bag.Platinum));

               characterBytes.AddRange(BitConverter.GetBytes(bag.Equipment.Count));
               foreach (EquipmentModel equipment in bag.Equipment)
               {
                  characterBytes.AddRange(equipment.ID.ToByteArray());
                  if (equipment.Item != null)
                  {
                     characterBytes.AddRange(equipment.Item.ID.ToByteArray());
                     characterBytes.AddRange(StringBytes(equipment.Item.Name));
                  }
                  else
                  {
                     characterBytes.AddRange(Guid.Empty.ToByteArray());
                     characterBytes.AddRange(BitConverter.GetBytes(0));
                  }
                  characterBytes.AddRange(StringBytes(equipment.Name));
                  characterBytes.AddRange(BitConverter.GetBytes(equipment.Equipped));
                  characterBytes.AddRange(BitConverter.GetBytes(equipment.Quantity));
                  characterBytes.AddRange(StringBytes(equipment.Notes));
               }
            }

            characterBytes.AddRange(BitConverter.GetBytes(character.Spellbooks.Count));
            foreach (SpellbookModel spellbook in character.Spellbooks)
            {
               characterBytes.AddRange(spellbook.ID.ToByteArray());
               characterBytes.AddRange(StringBytes(spellbook.Name));

               characterBytes.AddRange(BitConverter.GetBytes(spellbook.BasedOnClass));
               if (spellbook.Class != null)
               {
                  characterBytes.AddRange(spellbook.Class.ID.ToByteArray());
                  characterBytes.AddRange(StringBytes(spellbook.Class.Name));
               }
               else
               {
                  characterBytes.AddRange(Guid.Empty.ToByteArray());
                  characterBytes.AddRange(BitConverter.GetBytes(0));
               }

               characterBytes.AddRange(BitConverter.GetBytes(spellbook.BasedOnRace));
               if (spellbook.Race != null)
               {
                  characterBytes.AddRange(spellbook.Race.ID.ToByteArray());
                  characterBytes.AddRange(StringBytes(spellbook.Race.Name));
               }
               else
               {
                  characterBytes.AddRange(Guid.Empty.ToByteArray());
                  characterBytes.AddRange(BitConverter.GetBytes(0));
               }

               characterBytes.AddRange(BitConverter.GetBytes((int)spellbook.Ability));
               characterBytes.AddRange(BitConverter.GetBytes(spellbook.AdditionalDCBonus));
               characterBytes.AddRange(BitConverter.GetBytes(spellbook.AdditionalToHitBonus));
               characterBytes.AddRange(BitConverter.GetBytes(spellbook.ResetOnShortRest));
               characterBytes.AddRange(BitConverter.GetBytes(spellbook.ResetOnLongRest));

               characterBytes.AddRange(BitConverter.GetBytes(spellbook.Spells.Count));
               foreach (SpellbookEntryModel spellbookEntry in spellbook.Spells)
               {
                  characterBytes.AddRange(spellbookEntry.ID.ToByteArray());
                  characterBytes.AddRange(BitConverter.GetBytes(spellbookEntry.Prepared));
                  characterBytes.AddRange(BitConverter.GetBytes(spellbookEntry.Used));
                  if (spellbookEntry.Spell != null)
                  {
                     characterBytes.AddRange(spellbookEntry.Spell.ID.ToByteArray());
                     characterBytes.AddRange(StringBytes(spellbookEntry.Spell.Name));
                  }
                  else
                  {
                     characterBytes.AddRange(Guid.Empty.ToByteArray());
                     characterBytes.AddRange(BitConverter.GetBytes(0));
                  }
               }

               characterBytes.AddRange(BitConverter.GetBytes(spellbook.CurrentFirstLevelSpellSlots));
               characterBytes.AddRange(BitConverter.GetBytes(spellbook.CurrentSecondLevelSpellSlots));
               characterBytes.AddRange(BitConverter.GetBytes(spellbook.CurrentThirdLevelSpellSlots));
               characterBytes.AddRange(BitConverter.GetBytes(spellbook.CurrentFourthLevelSpellSlots));
               characterBytes.AddRange(BitConverter.GetBytes(spellbook.CurrentFifthLevelSpellSlots));
               characterBytes.AddRange(BitConverter.GetBytes(spellbook.CurrentSixthLevelSpellSlots));
               characterBytes.AddRange(BitConverter.GetBytes(spellbook.CurrentSeventhLevelSpellSlots));
               characterBytes.AddRange(BitConverter.GetBytes(spellbook.CurrentEighthLevelSpellSlots));
               characterBytes.AddRange(BitConverter.GetBytes(spellbook.CurrentNinthLevelSpellSlots));
            }

            characterBytes.AddRange(BitConverter.GetBytes(character.StatModifications.Count));
            foreach (StatModificationModel statModification in character.StatModifications)
            {
               characterBytes.AddRange(statModification.ID.ToByteArray());
               characterBytes.AddRange(BitConverter.GetBytes((int)statModification.ModificationOption));
               characterBytes.AddRange(BitConverter.GetBytes(statModification.FixedValue));
               characterBytes.AddRange(BitConverter.GetBytes(statModification.Value));
               characterBytes.AddRange(StringBytes(statModification.Notes));
            }

            characterBytes.AddRange(character.ArmorClassModel.ID.ToByteArray());
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorClassModel.ArmorBonus));
            characterBytes.AddRange(BitConverter.GetBytes((int)character.ArmorClassModel.ArmorType));
            characterBytes.AddRange(BitConverter.GetBytes((int)character.ArmorClassModel.FirstAbility));
            characterBytes.AddRange(BitConverter.GetBytes((int)character.ArmorClassModel.SecondAbility));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorClassModel.ItemBonus));
            characterBytes.AddRange(BitConverter.GetBytes(character.ArmorClassModel.AdditionalBonus));
         }

         return characterBytes.ToArray();
      }

      /// <inheritdoc />
      public IEnumerable<CharacterModel> GetCharacters(byte[] characterBytes,
          IEnumerable<BackgroundModel> backgrounds, IEnumerable<ClassModel> classes, IEnumerable<ConditionModel> conditions, IEnumerable<FeatModel> feats, IEnumerable<ItemModel> items,
          IEnumerable<LanguageModel> languages, IEnumerable<MonsterModel> monsters, IEnumerable<RaceModel> races, IEnumerable<SpellModel> spells)
      {
         List<CharacterModel> characters = new List<CharacterModel>();

         using (MemoryStream stream = new MemoryStream(characterBytes))
         {
            using (BinaryReader reader = new BinaryReader(stream))
            {
               int version = BitConverter.ToInt32(reader.ReadBytes(4), 0);
               if (version == _version)
               {
                  int characterCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                  for (int i = 0; i < characterCount; ++i)
                  {
                     CharacterModel character = new CharacterModel();

                     character.ID = new Guid(reader.ReadBytes(16));
                     character.Name = ReadNextString(reader);

                     character.CurrentHP = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.TempHP = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.DeathSaveSuccesses = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.DeathSaveFailures = BitConverter.ToInt32(reader.ReadBytes(4), 0);

                     Guid raceID = new Guid(reader.ReadBytes(16));
                     string raceName = ReadNextString(reader);
                     RaceModel race = races.FirstOrDefault(x => x.ID == raceID);
                     if (race == null)
                     {
                        race = races.FirstOrDefault(x => x.Name.Equals(raceName, StringComparison.CurrentCultureIgnoreCase));
                     }
                     character.Race = race;

                     Guid backgroundID = new Guid(reader.ReadBytes(16));
                     string backgroundName = ReadNextString(reader);
                     BackgroundModel background = backgrounds.FirstOrDefault(x => x.ID == backgroundID);
                     if (background == null)
                     {
                        background = backgrounds.FirstOrDefault(x => x.Name.Equals(backgroundName, StringComparison.CurrentCultureIgnoreCase));
                     }
                     character.Background = background;

                     character.Alignment = (Alignment)BitConverter.ToInt32(reader.ReadBytes(4), 0);

                     MovementModel movementModel = new MovementModel();
                     movementModel.ID = new Guid(reader.ReadBytes(16));
                     movementModel.WalkSpeed = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     movementModel.SwimSpeed = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     movementModel.ClimbSpeed = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     movementModel.CrawlSpeed = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     movementModel.FlySpeed = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     movementModel.ApplyEncumbrance = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.MovementModel = movementModel;

                     character.Inspiration = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.Deity = ReadNextString(reader);
                     character.Gender = ReadNextString(reader);
                     character.Age = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.HeightFeet = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.HeightInches = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.Weight = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.Hair = ReadNextString(reader);
                     character.Eyes = ReadNextString(reader);
                     character.Skin = ReadNextString(reader);
                     character.Experience = BitConverter.ToInt32(reader.ReadBytes(4), 0);

                     character.BaseStrength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.BaseDexterity = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.BaseConstitution = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.BaseIntelligence = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.BaseWisdom = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.BaseCharisma = BitConverter.ToInt32(reader.ReadBytes(4), 0);

                     character.BonusStrength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.BonusDexterity = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.BonusConstitution = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.BonusIntelligence = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.BonusWisdom = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.BonusCharisma = BitConverter.ToInt32(reader.ReadBytes(4), 0);

                     character.PersonalityTraits = ReadNextString(reader);
                     character.Ideals = ReadNextString(reader);
                     character.Bonds = ReadNextString(reader);
                     character.Flaws = ReadNextString(reader);
                     character.Backstory = ReadNextString(reader);

                     character.Levels = new List<LevelModel>();
                     int levelCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     for (int j = 0; j < levelCount; ++j)
                     {
                        LevelModel level = new LevelModel();

                        level.ID = new Guid(reader.ReadBytes(16));
                        level.Level = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        level.LevelOfClass = BitConverter.ToInt32(reader.ReadBytes(4), 0);

                        Guid classID = new Guid(reader.ReadBytes(16));
                        string className = ReadNextString(reader);
                        ClassModel classModel = classes.FirstOrDefault(x => x.ID == classID);
                        if (classModel == null)
                        {
                           classModel = classes.FirstOrDefault(x => x.Name.Equals(className, StringComparison.CurrentCultureIgnoreCase));
                        }
                        level.Class = classModel;

                        level.Features = new List<FeatureModel>();
                        int featuresCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        for (int k = 0; k < featuresCount; ++k)
                        {
                           FeatureModel featureModel = new FeatureModel();
                           featureModel.ID = new Guid(reader.ReadBytes(16));
                           featureModel.Name = ReadNextString(reader);

                           if (level.Class != null)
                           {
                              bool found = false;

                              foreach (AutoLevelModel autoLevelModel in level.Class.AutoLevels)
                              {
                                 foreach (FeatureModel feature in autoLevelModel.Features)
                                 {
                                    if (feature.ID == featureModel.ID || feature.Name.Equals(featureModel.Name, StringComparison.CurrentCultureIgnoreCase))
                                    {
                                       featureModel = feature;
                                       found = true;
                                       break;
                                    }
                                 }

                                 if (found)
                                 {
                                    break;
                                 }
                              }
                           }

                           level.Features.Add(featureModel);
                        }

                        level.Feats = new List<FeatModel>();
                        int featsCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        for (int k = 0; k < featsCount; ++k)
                        {
                           Guid featID = new Guid(reader.ReadBytes(16));
                           string featName = ReadNextString(reader);
                           FeatModel feat = feats.FirstOrDefault(x => x.ID == featID);
                           if (feat == null)
                           {
                              feat = feats.FirstOrDefault(x => x.Name.Equals(featName, StringComparison.CurrentCultureIgnoreCase));
                           }
                           if (feat != null)
                           {
                              level.Feats.Add(feat);
                           }
                        }

                        level.HitDieResult = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        level.HitDieUsed = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                        level.HitDieRestRoll = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        level.AdditionalHP = BitConverter.ToInt32(reader.ReadBytes(4), 0);

                        int abilityImprovementCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        for (int k = 0; k < abilityImprovementCount; ++k)
                        {
                           Ability ability = (Ability)BitConverter.ToInt32(reader.ReadBytes(4), 0);
                           int value = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                           level.AbilityScoreImprovements.Add(new KeyValuePair<Ability, int>(ability, value));
                        }

                        character.Levels.Add(level);
                     }

                     character.AbilityRollMethod = (AbilityRollMethod)BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.TimesAbilityScoresRolled = BitConverter.ToInt32(reader.ReadBytes(4), 0);

                     character.AbilitySaveProficiencies = new List<AbilityModel>();
                     int abilityProficiencyCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     for (int j = 0; j < abilityProficiencyCount; ++j)
                     {
                        Ability ability = (Ability)BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        int saveBonus = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        int checkBonus = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        bool proficient = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                        character.AbilitySaveProficiencies.Add(new AbilityModel(ability, saveBonus, checkBonus, proficient));
                     }

                     character.SkillProficiencies = new List<SkillModel>();
                     int skillProficiencyCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     for (int j = 0; j < skillProficiencyCount; ++j)
                     {
                        Skill skill = (Skill)BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        int bonus = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        bool proficient = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                        bool expertise = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                        character.SkillProficiencies.Add(new SkillModel(skill, bonus, proficient, expertise));
                     }

                     character.Languages = new List<LanguageModel>();
                     int languageCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     for (int j = 0; j < languageCount; ++j)
                     {
                        Guid languageID = new Guid(reader.ReadBytes(16));
                        string languageName = ReadNextString(reader);

                        LanguageModel language = languages.FirstOrDefault(x => x.ID == languageID);
                        if (language == null)
                        {
                           language = languages.FirstOrDefault(x => x.Name.Equals(languageName, StringComparison.CurrentCultureIgnoreCase));
                           if (language == null)
                           {
                              language = new LanguageModel();
                              language.ID = languageID;
                              language.Name = languageName;
                           }
                        }
                        character.Languages.Add(language);
                     }

                     character.ArmorProficiency.LightArmorProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.MediumArmorProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.HeavyArmorProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.ShieldsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.PaddedProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.LeatherProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.StuddedLeatherProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.HideProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.ChainShirtProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.ScaleMailProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.BreastplateProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.HalfPlateProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.RingMailProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.ChainMailProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.SplintProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.PlateProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ArmorProficiency.ShieldProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);

                     character.WeaponProficiency.SimpleWeaponsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.MartialWeaponsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.ClubProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.DaggerProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.GreatclubProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.HandaxeProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.JavelinProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.LightHammerProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.MaceProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.QuarterstaffProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.SickleProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.SpearProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.CrossbowLightProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.DartProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.ShortbowProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.SlingProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.BattleaxeProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.FlailProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.GlaiveProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.GreataxeProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.GreatswordProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.HalberdProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.LanceProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.LongswordProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.MaulProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.MorningstarProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.PikeProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.RapierProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.ScimitarProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.ShortswordProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.TridentProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.WarPickProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.WarhammerProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.WhipProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.BlowgunProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.CrossbowHandProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.CrossbowHeavyProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.LongbowProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.WeaponProficiency.NetProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);

                     character.ToolProficiency.AlchemistsSuppliesProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.BrewersSuppliesProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.CalligraphersSuppliesProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.CarpentersToolsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.CartographersToolsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.CobblersToolsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.CooksUtensilsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.GlassblowersToolsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.JewelerssToolsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.LeatherworkersToolsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.MasonsToolsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.PaintersSuppliesProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.PottersToolsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.SmithsToolsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.TinkersToolsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.WeaversToolsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.WoodcarversToolsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.DiceSetProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.PlayingCardSetProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.DisguiseKitProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.ForgeryKitProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.HerbalismKitProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.PoisonersKitProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.BagpipesProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.DrumProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.DulcimerProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.FluteProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.LuteProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.LyreProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.HornProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.PanFluteProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.ShawmProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.ViolProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.NavigatorsToolsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.ThievesToolsProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.LandVehiclesProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                     character.ToolProficiency.WaterVehiclesProficiency = BitConverter.ToBoolean(reader.ReadBytes(1), 0);

                     character.Conditions = new List<AppliedConditionModel>();
                     int conditionCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     for (int j = 0; j < conditionCount; ++j)
                     {
                        AppliedConditionModel appliedCondition = new AppliedConditionModel();
                        appliedCondition.ID = new Guid(reader.ReadBytes(16));

                        Guid conditionID = new Guid(reader.ReadBytes(16));
                        string conditionName = ReadNextString(reader);
                        ConditionModel conditionModel = conditions.FirstOrDefault(x => x.ID == conditionID);
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

                        character.Conditions.Add(appliedCondition);
                     }

                     character.SavingThrowNotes = ReadNextString(reader);

                     character.Attacks = new List<AttackModel>();
                     int attackCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     for (int j = 0; j < attackCount; ++j)
                     {
                        AttackModel attack = new AttackModel();

                        attack.ID = new Guid(reader.ReadBytes(16));
                        attack.Name = ReadNextString(reader);
                        attack.NumberOfDamageDice = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        attack.DamageDie = ReadNextString(reader);
                        attack.Ability = (Ability)BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        attack.Proficient = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                        attack.DamageType = (DamageType)BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        attack.AdditionalToHitBonus = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        attack.AdditionalDamageBonus = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        attack.Range = ReadNextString(reader);
                        attack.Notes = ReadNextString(reader);
                        attack.ShowToHit = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                        attack.ShowDamage = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                        attack.ToHit = ReadNextString(reader);
                        attack.Damage = ReadNextString(reader);

                        character.Attacks.Add(attack);
                     }

                     character.Counters = new List<CounterModel>();
                     int counterCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     for (int j = 0; j < counterCount; ++j)
                     {
                        CounterModel counter = new CounterModel();

                        counter.ID = new Guid(reader.ReadBytes(16));
                        counter.Name = ReadNextString(reader);
                        counter.CurrentValue = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        counter.MaxValue = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        counter.ResetOnShortRest = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                        counter.ResetOnLongRest = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                        counter.Notes = ReadNextString(reader);

                        character.Counters.Add(counter);
                     }

                     character.Companions = new List<CompanionModel>();
                     int companionCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     for (int j = 0; j < companionCount; ++j)
                     {
                        CompanionModel companion = new CompanionModel();

                        companion.ID = new Guid(reader.ReadBytes(16));

                        Guid monsterID = new Guid(reader.ReadBytes(16));
                        string monsterName = ReadNextString(reader);
                        MonsterModel monster = monsters.FirstOrDefault(x => x.ID == monsterID);
                        if (monster == null)
                        {
                           monster = monsters.FirstOrDefault(x => x.Name.Equals(monsterName, StringComparison.CurrentCultureIgnoreCase));
                        }
                        companion.MonsterModel = monster;

                        companion.Name = ReadNextString(reader);
                        companion.CurrentHP = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        companion.MaxHP = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        companion.Notes = ReadNextString(reader);

                        character.Companions.Add(companion);
                     }

                     character.Bags = new List<BagModel>();
                     int bagCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     for (int j = 0; j < bagCount; ++j)
                     {
                        BagModel bag = new BagModel();

                        bag.ID = new Guid(reader.ReadBytes(16));
                        bag.Name = ReadNextString(reader);
                        bag.FixedWeight = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                        bag.FixedWeightValue = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        bag.Copper = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        bag.Silver = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        bag.Electrum = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        bag.Gold = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        bag.Platinum = BitConverter.ToInt32(reader.ReadBytes(4), 0);

                        bag.Equipment = new List<EquipmentModel>();
                        int equipmentCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        for (int k = 0; k < equipmentCount; ++k)
                        {
                           EquipmentModel equipment = new EquipmentModel();

                           equipment.ID = new Guid(reader.ReadBytes(16));

                           Guid itemID = new Guid(reader.ReadBytes(16));
                           string itemName = ReadNextString(reader);
                           ItemModel item = items.FirstOrDefault(x => x.ID == itemID);
                           if (item == null)
                           {
                              item = items.FirstOrDefault(x => x.Name.Equals(itemName, StringComparison.CurrentCultureIgnoreCase));
                           }
                           equipment.Item = item;

                           equipment.Name = ReadNextString(reader);
                           equipment.Equipped = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                           equipment.Quantity = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                           equipment.Notes = ReadNextString(reader);

                           bag.Equipment.Add(equipment);
                        }

                        character.Bags.Add(bag);
                     }

                     character.Spellbooks = new List<SpellbookModel>();
                     int spellbookCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     for (int j = 0; j < spellbookCount; ++j)
                     {
                        SpellbookModel spellbook = new SpellbookModel();

                        spellbook.ID = new Guid(reader.ReadBytes(16));
                        spellbook.Name = ReadNextString(reader);

                        spellbook.BasedOnClass = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                        Guid classID = new Guid(reader.ReadBytes(16));
                        string className = ReadNextString(reader);
                        if (classID != Guid.Empty)
                        {
                           ClassModel classModel = classes.FirstOrDefault(x => x.ID == classID);
                           if (classModel == null)
                           {
                              classModel = classes.FirstOrDefault(x => x.Name.Equals(className, StringComparison.CurrentCultureIgnoreCase));
                           }
                           spellbook.Class = classModel;
                        }

                        spellbook.BasedOnRace = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                        raceID = new Guid(reader.ReadBytes(16));
                        raceName = ReadNextString(reader);
                        if (raceID != Guid.Empty)
                        {
                           RaceModel raceModel = races.FirstOrDefault(x => x.ID == raceID);
                           if (raceModel == null)
                           {
                              race = races.FirstOrDefault(x => x.Name.Equals(raceName, StringComparison.CurrentCultureIgnoreCase));
                           }
                           spellbook.Race = raceModel;
                        }

                        spellbook.Ability = (Ability)BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        spellbook.AdditionalDCBonus = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        spellbook.AdditionalToHitBonus = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        spellbook.ResetOnShortRest = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                        spellbook.ResetOnLongRest = BitConverter.ToBoolean(reader.ReadBytes(1), 0);

                        spellbook.Spells = new List<SpellbookEntryModel>();
                        int spellCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        for (int k = 0; k < spellCount; ++k)
                        {
                           SpellbookEntryModel spellbookEntry = new SpellbookEntryModel();

                           spellbookEntry.ID = new Guid(reader.ReadBytes(16));
                           spellbookEntry.Prepared = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                           spellbookEntry.Used = BitConverter.ToBoolean(reader.ReadBytes(1), 0);

                           Guid spellID = new Guid(reader.ReadBytes(16));
                           string spellName = ReadNextString(reader);
                           SpellModel spellModel = spells.FirstOrDefault(x => x.ID == spellID);
                           if (spellModel == null)
                           {
                              spellModel = spells.FirstOrDefault(x => x.Name.Equals(spellName, StringComparison.CurrentCultureIgnoreCase));
                           }

                           if (spellModel != null)
                           {
                              spellbookEntry.Spell = spellModel;
                              spellbook.Spells.Add(spellbookEntry);
                           }
                        }

                        spellbook.CurrentFirstLevelSpellSlots = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        spellbook.CurrentSecondLevelSpellSlots = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        spellbook.CurrentThirdLevelSpellSlots = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        spellbook.CurrentFourthLevelSpellSlots = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        spellbook.CurrentFifthLevelSpellSlots = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        spellbook.CurrentSixthLevelSpellSlots = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        spellbook.CurrentSeventhLevelSpellSlots = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        spellbook.CurrentEighthLevelSpellSlots = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        spellbook.CurrentNinthLevelSpellSlots = BitConverter.ToInt32(reader.ReadBytes(4), 0);

                        character.Spellbooks.Add(spellbook);
                     }

                     character.StatModifications = new List<StatModificationModel>();
                     int statModCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     for (int j = 0; j < statModCount; ++j)
                     {
                        StatModificationModel statModification = new StatModificationModel();

                        statModification.ID = new Guid(reader.ReadBytes(16));
                        statModification.ModificationOption = (StatModificationOption)BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        statModification.FixedValue = BitConverter.ToBoolean(reader.ReadBytes(1), 0);
                        statModification.Value = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                        statModification.Notes = ReadNextString(reader);

                        character.StatModifications.Add(statModification);
                     }

                     ArmorClassModel armorClassModel = new ArmorClassModel();
                     armorClassModel.ID = new Guid(reader.ReadBytes(16));
                     armorClassModel.ArmorBonus = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     armorClassModel.ArmorType = (ArmorType)BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     armorClassModel.FirstAbility = (Ability)BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     armorClassModel.SecondAbility = (Ability)BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     armorClassModel.ItemBonus = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     armorClassModel.AdditionalBonus = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                     character.ArmorClassModel = armorClassModel;

                     characters.Add(character);
                  }
               }
            }
         }

         return characters;
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
