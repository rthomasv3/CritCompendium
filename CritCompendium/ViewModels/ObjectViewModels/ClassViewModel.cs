using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class ClassViewModel
   {
      #region Fields

      private readonly ClassModel _classModel;
      private readonly string _hitDie;
      private readonly string _abilities;
      private readonly string _spellAbility;
      private readonly bool _spellTableVisible;
      private readonly List<string> _spellTableHeaders = new List<string>();
      private readonly List<List<string>> _spellSlotTable = new List<List<string>>();
      private readonly Dictionary<int, List<FeatureViewModel>> _featuresByLevel = new Dictionary<int, List<FeatureViewModel>>();
      private readonly List<Tuple<string, List<string>>> _tableFeatures = new List<Tuple<string, List<string>>>();
      private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="ClassViewModel"/>
      /// </summary>
      public ClassViewModel(ClassModel classModel)
      {
         _classModel = classModel;

         _hitDie = "d" + _classModel.HitDie.ToString();

         List<string> abilities = new List<string>();
         for (int i = 0; i < _classModel.AbilityProficiencies.Count; ++i)
         {
            abilities.Add(_stringService.GetString(_classModel.AbilityProficiencies[i]));
         }
         _abilities = String.Join(", ", abilities);

         _spellAbility = _classModel.SpellAbility == Ability.None ? "None" : _stringService.GetString(_classModel.SpellAbility);

         _spellTableVisible = _classModel.SpellAbility != Ability.None && _classModel.SpellSlots.Any();

         int max = 0;
         foreach (string spellSlot in _classModel.SpellSlots)
         {
            max = Math.Max(max, spellSlot.Split(new char[] { ',' }).Length);
         }

         if (max > 0)
         {
            _spellTableHeaders.Add("Lvl");
            _spellTableHeaders.Add("C");
            for (int i = 1; i < max; ++i)
            {
               _spellTableHeaders.Add(NumberToOrdinal(i));
            }

            for (int i = 0; i < _classModel.SpellSlots.Count; ++i)
            {
               List<string> slotsAtLevel = new List<string>();

               slotsAtLevel.Add((_classModel.SpellStartLevel + i).ToString());

               string[] slotStrings = _classModel.SpellSlots[i].Split(new char[] { ',' });
               for (int j = 0; j < max; ++j)
               {
                  if (j < slotStrings.Length)
                  {
                     string slot = slotStrings[j].Trim();
                     slotsAtLevel.Add(String.IsNullOrWhiteSpace(slot) ? "0" : slot);
                  }
                  else
                  {
                     slotsAtLevel.Add("0");
                  }
               }

               _spellSlotTable.Add(slotsAtLevel);
            }
         }

         foreach (AutoLevelModel autoLevel in _classModel.AutoLevels)
         {
            if (autoLevel.Features.Count > 0)
            {
               List<FeatureViewModel> features = new List<FeatureViewModel>();
               foreach (FeatureModel featureModel in autoLevel.Features.OrderBy(x => x.Name))
               {
                  features.Add(new FeatureViewModel(featureModel));
               }

               if (!_featuresByLevel.ContainsKey(autoLevel.Level))
               {
                  _featuresByLevel[autoLevel.Level] = features;
               }
               else
               {
                  _featuresByLevel[autoLevel.Level].AddRange(features);
               }

               _featuresByLevel[autoLevel.Level] = _featuresByLevel[autoLevel.Level].OrderBy(x => x.Name).ToList();
            }
         }

         foreach (FeatureModel featureModel in _classModel.TableFeatures)
         {
            string name = featureModel.Name;
            List<string> values = new List<string>();
            foreach (string value in featureModel.Text.Split(new char[] { ',' }))
            {
               values.Add(value);
            }
            while (values.Count < 20)
            {
               values.Add("-");
            }

            _tableFeatures.Add(new Tuple<string, List<string>>(name, values));
         }
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets class model
      /// </summary>
      public ClassModel ClassModel
      {
         get { return _classModel; }
      }

      /// <summary>
      /// Gets name
      /// </summary>
      public string Name
      {
         get { return _classModel.Name; }
      }

      /// <summary>
      /// Gets hitDie
      /// </summary>
      public string HitDie
      {
         get { return _hitDie; }
      }

      /// <summary>
      /// Gets abilities
      /// </summary>
      public string Abilities
      {
         get { return _abilities; }
      }

      /// <summary>
      /// Gets spell ability
      /// </summary>
      public string SpellAbility
      {
         get { return _spellAbility; }
      }

      /// <summary>
      /// Gets spell table visible
      /// </summary>
      public bool SpellTableVisible
      {
         get { return _spellTableVisible; }
      }

      /// <summary>
      /// Gets spell table headers
      /// </summary>
      public List<string> SpellTableHeaders
      {
         get { return _spellTableHeaders; }
      }

      /// <summary>
      /// Gets spell slot table
      /// </summary>
      public List<List<string>> SpellSlotTable
      {
         get { return _spellSlotTable; }
      }

      /// <summary>
      /// Gets features by level
      /// </summary>
      public Dictionary<int, List<FeatureViewModel>> FeaturesByLevel
      {
         get { return _featuresByLevel; }
      }


      /// <summary>
      /// Gets class table visible
      /// </summary>
      public bool ClassTableVisible
      {
         get { return _tableFeatures.Count > 0; }
      }

      /// <summary>
      /// Gets table features
      /// </summary>
      public List<Tuple<string, List<string>>> TableFeatures
      {
         get { return _tableFeatures; }
      }

      /// <summary>
      /// Gets XML
      /// </summary>
      public string XML
      {
         get { return _classModel.XML; }
      }

      #endregion

      #region Non-Public Methods

      public string NumberToOrdinal(int value)
      {
         string extension = "th";

         int lastDigits = value % 100;

         if (lastDigits < 11 || lastDigits > 13)
         {
            switch (lastDigits % 10)
            {
               case 1:
                  extension = "st";
                  break;
               case 2:
                  extension = "nd";
                  break;
               case 3:
                  extension = "rd";
                  break;
            }
         }

         return value.ToString() + extension;
      }

      #endregion
   }
}
