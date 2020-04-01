using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CritCompendiumInfrastructure.Business;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class EncounterMonsterModel : EncounterCreatureModel
   {
      #region Fields

      private Compendium _compendium = DependencyResolver.Resolve<Compendium>();
      private DiceService _diceService = DependencyResolver.Resolve<DiceService>();
      private StatService _statService = DependencyResolver.Resolve<StatService>();
      private StringService _stringService = DependencyResolver.Resolve<StringService>();

      private MonsterModel _monsterModel;
      private int _quantity = 1;
      private int _averageDamageTurn;
      private string _cr;
      private string _damageVulnerabilities;
      private string _damageResistances;
      private string _damageImmunities;
      private string _conditionImmunities;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates an instance of <see cref="EncounterMonsterModel"/>
      /// </summary>
      public EncounterMonsterModel()
          : base()
      {
      }

      /// <summary>
      /// Creates an instance of <see cref="EncounterMonsterModel"/>
      /// </summary>
      public EncounterMonsterModel(EncounterMonsterModel encounterMonsterModel)
          : base(encounterMonsterModel)
      {
         _monsterModel = encounterMonsterModel.MonsterModel;
         _averageDamageTurn = encounterMonsterModel.AverageDamageTurn;
         _quantity = encounterMonsterModel.Quantity;
         _cr = encounterMonsterModel.CR;
         _damageVulnerabilities = encounterMonsterModel.DamageVulnerabilities;
         _damageResistances = encounterMonsterModel.DamageResistances;
         _damageImmunities = encounterMonsterModel.DamageImmunities;
         _conditionImmunities = encounterMonsterModel.ConditionImmunities;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets monster model
      /// </summary>
      public MonsterModel MonsterModel
      {
         get { return _monsterModel; }
         set
         {
            _monsterModel = value;
            InitializeFromMonsterModel();
         }
      }

      /// <summary>
      /// Gets or sets quantity
      /// </summary>
      public int Quantity
      {
         get { return _quantity; }
         set { _quantity = value; }
      }

      /// <summary>
      /// Gets or sets average damage per turn
      /// </summary>
      public int AverageDamageTurn
      {
         get { return _averageDamageTurn; }
         set { _averageDamageTurn = value; }
      }

      /// <summary>
      /// Gets or sets cr
      /// </summary>
      public string CR
      {
         get { return _cr; }
         set { _cr = value; }
      }

      /// <summary>
      /// Gets or sets damage vulnerabilities
      /// </summary>
      public string DamageVulnerabilities
      {
         get { return _damageVulnerabilities; }
         set { _damageVulnerabilities = value; }
      }

      /// <summary>
      /// Gets or sets damage resistances
      /// </summary>
      public string DamageResistances
      {
         get { return _damageResistances; }
         set { _damageResistances = value; }
      }

      /// <summary>
      /// Gets or sets damage immunities
      /// </summary>
      public string DamageImmunities
      {
         get { return _damageImmunities; }
         set { _damageImmunities = value; }
      }

      /// <summary>
      /// Gets or sets condition immunities
      /// </summary>
      public string ConditionImmunities
      {
         get { return _conditionImmunities; }
         set { _conditionImmunities = value; }
      }

      #endregion

      #region Private Methods

      private void InitializeFromMonsterModel()
      {
         if (_monsterModel != null)
         {
            Name = _monsterModel.Name;

            string hpString = _monsterModel.HP;
            if (hpString.Contains(" "))
            {
               hpString = hpString.Split(new char[] { ' ' })[0];
            }
            int hp = 0;
            if (int.TryParse(hpString, out hp))
            {
               MaxHP = hp;
            }

            PassivePerception = _monsterModel.PassivePerception;

            string acString = _monsterModel.AC;
            if (acString.Contains(" "))
            {
               acString = acString.Split(new char[] { ' ' })[0];
            }
            int ac = 0;
            if (int.TryParse(acString, out ac))
            {
               AC = ac;
            }

            InitiativeBonus = _statService.GetStatBonus(_monsterModel.Dexterity);

            _cr = _monsterModel.CR;

            List<MonsterAttackModel> attacks = new List<MonsterAttackModel>();
            foreach (MonsterActionModel monsterActionModel in _monsterModel.Actions)
            {
               foreach (MonsterAttackModel monsterAttack in monsterActionModel.Attacks)
               {
                  attacks.Add(monsterAttack);
               }
            }

            int numberOfAttacks = 1;
            List<KeyValuePair<MonsterAttackModel, int>> multiattacks = new List<KeyValuePair<MonsterAttackModel, int>>();
            foreach (MonsterActionModel monsterActionModel in _monsterModel.Actions)
            {
               if (monsterActionModel.Name.ToLower().Contains("multiattack") ||
                   monsterActionModel.Name.ToLower().Contains("multi-attack"))
               {
                  string multiattackText = String.Join(" ", monsterActionModel.TextCollection).ToLower();
                  int attacksIndex = multiattackText.IndexOf("attacks");
                  if (attacksIndex != -1)
                  {
                     string upToAttacks = multiattackText.Substring(0, attacksIndex);
                     foreach (string word in upToAttacks.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Reverse())
                     {
                        if (word == "one")
                        {
                           numberOfAttacks = 1;
                           break;
                        }
                        else if (word == "two")
                        {
                           numberOfAttacks = 2;
                           break;
                        }
                        else if (word == "three")
                        {
                           numberOfAttacks = 3;
                           break;
                        }
                        else if (word == "four")
                        {
                           numberOfAttacks = 4;
                           break;
                        }
                        else if (word == "five")
                        {
                           numberOfAttacks = 5;
                           break;
                        }
                     }
                  }

                  foreach (MonsterAttackModel monsterAttackModel in attacks)
                  {
                     int nameIndex = multiattackText.IndexOf(monsterAttackModel.Name.ToLower());
                     if (nameIndex != -1)
                     {
                        string upToAttackName = multiattackText.Substring(0, nameIndex);
                        foreach (string word in upToAttackName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Reverse())
                        {
                           if (word == "one")
                           {
                              multiattacks.Add(new KeyValuePair<MonsterAttackModel, int>(monsterAttackModel, 1));
                              break;
                           }
                           else if (word == "two")
                           {
                              multiattacks.Add(new KeyValuePair<MonsterAttackModel, int>(monsterAttackModel, 2));
                              break;
                           }
                           else if (word == "three")
                           {
                              multiattacks.Add(new KeyValuePair<MonsterAttackModel, int>(monsterAttackModel, 3));
                              break;
                           }
                           else if (word == "four")
                           {
                              multiattacks.Add(new KeyValuePair<MonsterAttackModel, int>(monsterAttackModel, 4));
                              break;
                           }
                           else if (word == "five")
                           {
                              multiattacks.Add(new KeyValuePair<MonsterAttackModel, int>(monsterAttackModel, 5));
                              break;
                           }
                        }
                     }
                  }
               }
            }

            int averageMultiAttackDamage = 0;
            foreach (KeyValuePair<MonsterAttackModel, int> multiAttackPair in multiattacks)
            {
               if (attacks.Contains(multiAttackPair.Key))
               {
                  attacks.Remove(multiAttackPair.Key);
               }

               if (!String.IsNullOrEmpty(multiAttackPair.Key.Roll))
               {
                  (double, double, double) avgMinMax = _diceService.EvaluateExpressionAvgMinMax(multiAttackPair.Key.Roll);
                  averageMultiAttackDamage += (int)avgMinMax.Item1 * multiAttackPair.Value;
               }
            }

            int averageSingleAttackDamage = 0;
            int totalAverageSingleAttackDamage = 0;
            int attacksWithRolls = 0;
            foreach (MonsterAttackModel monsterAttackModel in attacks)
            {
               if (!String.IsNullOrEmpty(monsterAttackModel.Roll))
               {
                  (double, double, double) avgMinMax = _diceService.EvaluateExpressionAvgMinMax(monsterAttackModel.Roll);
                  totalAverageSingleAttackDamage += (int)avgMinMax.Item1;
                  attacksWithRolls++;
               }
            }
            if (attacksWithRolls > 0)
            {
               averageSingleAttackDamage = totalAverageSingleAttackDamage / attacksWithRolls;
            }

            int averageSpellDamage = 0;
            int totalAverageSpellDamage = 0;
            int spellsWithRolls = 0;
            foreach (string spellName in _monsterModel.Spells)
            {
               SpellModel spellModel = _compendium.Spells.FirstOrDefault(x => x.Name.ToLower() == spellName.ToLower());
               if (spellModel != null && spellModel.Rolls.Any())
               {
                  int maxRoll = 0;
                  foreach (string roll in spellModel.Rolls)
                  {
                     (double, double, double) avgMinMax = _diceService.EvaluateExpressionAvgMinMax(roll);
                     maxRoll = Math.Max((int)avgMinMax.Item1, maxRoll);
                  }
                  totalAverageSpellDamage += maxRoll;
                  spellsWithRolls++;
               }
            }
            if (spellsWithRolls > 0)
            {
               averageSpellDamage = totalAverageSpellDamage / spellsWithRolls;
            }

            _averageDamageTurn = Math.Max(Math.Max(averageMultiAttackDamage, averageSingleAttackDamage), averageSpellDamage);

            _damageVulnerabilities = _stringService.CapitalizeWords(_monsterModel.Vulnerabilities);

            _damageResistances = _stringService.CapitalizeWords(_monsterModel.Resistances);

            _damageImmunities = _stringService.CapitalizeWords(_monsterModel.Immunities);

            _conditionImmunities = _stringService.CapitalizeWords(_monsterModel.ConditionImmunities);
         }
         else
         {
            Name = String.Empty;
            PassivePerception = 0;
            InitiativeBonus = 0;
            _averageDamageTurn = 0;
            _quantity = 1;
            _cr = String.Empty;
            _damageVulnerabilities = String.Empty;
            _damageResistances = String.Empty;
            _damageImmunities = String.Empty;
            _conditionImmunities = String.Empty;
         }
      }

      #endregion
   }
}
