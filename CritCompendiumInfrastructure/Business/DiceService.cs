using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Business
{
   public sealed class DiceService
   {
      #region Fields

      private readonly Regex _numDiceRegEx = new Regex(@"\d+d\d+", RegexOptions.Compiled);
      private readonly Regex _singleDiceRegEx = new Regex(@"\D+(d\d+)", RegexOptions.Compiled);
      private readonly Regex _startSingleDiceRegEx = new Regex("^\\s*[d]\\d+", RegexOptions.Compiled);

      #endregion

      #region Public Methods

      /// <summary>
      /// Evaluates the roll expression
      /// </summary>
      public (double, string) EvaluateExpression(string expression)
      {
         double result = 0;

         string evaluated = EvaluateExpressionRolls(expression, RollType.Random).Replace('x', '*');

         try
         {
            result = Double.Parse(new DataTable().Compute(evaluated, null).ToString());
            evaluated += " = " + result;
            evaluated = evaluated.Replace('*', 'x');
         }
         catch (Exception)
         {
            evaluated = "Invalid Expression";
         }

         return (result, evaluated);
      }

      /// <summary>
      /// Evaluates the average, min, and max of the roll expression
      /// </summary>
      public (double, double, double) EvaluateExpressionAvgMinMax(string expression)
      {
         double avg = 0;
         double min = 0;
         double max = 0;
         string minExpression = EvaluateExpressionRolls(expression, RollType.Minimum).Replace('x', '*');
         string maxExpression = EvaluateExpressionRolls(expression, RollType.Maximum).Replace('x', '*');

         try
         {
            min = Double.Parse(new DataTable().Compute(minExpression, null).ToString());
            max = Double.Parse(new DataTable().Compute(maxExpression, null).ToString());
            avg = ((double)((min + max) / 2));
         }
         catch (Exception) { }

         return (avg, min, max);
      }

      /// <summary>
      /// Gets a number within the range (inclusive)
      /// </summary>
      public int RandomNumber(int min, int max)
      {
         int randomNumber = 0;

         try
         {
            using (RNGCryptoServiceProvider rngCryptoService = new RNGCryptoServiceProvider())
            {
               int seed = 0;
               byte[] seedBytes = new byte[4];

               rngCryptoService.GetBytes(seedBytes);
               seed = BitConverter.ToInt32(seedBytes, 0);

               randomNumber = new Random(seed).Next(min, max + 1);
            }
         }
         catch (Exception)
         {
            int seed = new Random((int)DateTime.Now.Ticks).Next();
            randomNumber = new Random(seed).Next(min, max + 1);
         }

         return randomNumber;
      }

      #endregion

      #region Private Methods

      public string EvaluateExpressionRolls(string expression, RollType rollType)
      {
         string evaluated = expression;

         if (evaluated != null)
         {
            Match match = null;

            do
            {
               match = _numDiceRegEx.Match(evaluated);

               if (match.Success)
               {
                  string[] parts = match.Value.Split(new char[] { 'd' });

                  if (parts.Length == 2)
                  {
                     int numberOfDice = 0;
                     if (Int32.TryParse(parts[0], out numberOfDice))
                     {
                        int die = 0;
                        if (Int32.TryParse(parts[1], out die))
                        {
                           evaluated = evaluated.Remove(match.Index, match.Length);

                           List<string> rolls = new List<string>();
                           for (int i = 0; i < numberOfDice; ++i)
                           {
                              if (rollType == RollType.Random)
                              {
                                 rolls.Add(RandomNumber(1, die).ToString());
                              }
                              else if (rollType == RollType.Minimum)
                              {
                                 rolls.Add("1");
                              }
                              else if (rollType == RollType.Maximum)
                              {
                                 rolls.Add(die.ToString());
                              }
                           }

                           evaluated = evaluated.Insert(match.Index, "(" + String.Join("+", rolls) + ")");
                        }
                     }
                  }
               }
            }
            while (match.Success);

            do
            {
               match = _singleDiceRegEx.Match(evaluated);

               if (match.Success)
               {
                  int die = 0;
                  if (Int32.TryParse(match.Groups[1].Value.Substring(1), out die))
                  {
                     if (rollType == RollType.Random)
                     {
                        int number = RandomNumber(1, die);
                        evaluated = evaluated.Remove(match.Groups[1].Index, match.Groups[1].Length).Insert(match.Groups[1].Index, number.ToString());
                     }
                     else if (rollType == RollType.Minimum)
                     {
                        evaluated = evaluated.Remove(match.Groups[1].Index, match.Groups[1].Length).Insert(match.Groups[1].Index, "1");
                     }
                     else if (rollType == RollType.Maximum)
                     {
                        evaluated = evaluated.Remove(match.Groups[1].Index, match.Groups[1].Length).Insert(match.Groups[1].Index, die.ToString());
                     }
                  }
               }
            }
            while (match.Success);

            do
            {
               match = _startSingleDiceRegEx.Match(evaluated);

               if (match.Success)
               {
                  int die = 0;
                  if (Int32.TryParse(match.Value.Substring(1), out die))
                  {
                     if (rollType == RollType.Random)
                     {
                        int number = RandomNumber(1, die);
                        evaluated = evaluated.Remove(match.Index, match.Length).Insert(match.Index, number.ToString());
                     }
                     else if (rollType == RollType.Minimum)
                     {
                        evaluated = evaluated.Remove(match.Index, match.Length).Insert(match.Index, "1");
                     }
                     else if (rollType == RollType.Maximum)
                     {
                        evaluated = evaluated.Remove(match.Index, match.Length).Insert(match.Index, die.ToString());
                     }
                  }
               }
            }
            while (match.Success);
         }

         return evaluated != null ? evaluated : String.Empty;
      }

      #endregion
   }
}
