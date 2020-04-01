using System;
using System.Collections.Generic;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class SpellViewModel
   {
      #region Fields

      private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
      private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();

      private readonly SpellModel _spellModel;
      private readonly string _level;
      private readonly string _school;
      private readonly string _range;
      private readonly string _ritual;
      private readonly string _time;
      private readonly string _components;
      private readonly string _duration;
      private readonly string _classes;
      private readonly string _text;
      private readonly List<string> _rolls = new List<string>();
      private readonly RelayCommand _rollCommand;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="SpellViewModel"/>
      /// </summary>
      public SpellViewModel(SpellModel spellModel)
      {
         _spellModel = spellModel;

         if (_spellModel.Level == -1)
         {
            _level = "Unknown";
         }
         else if (_spellModel.Level == 0)
         {
            _level = "Cantrip";
         }
         else
         {
            _level = _spellModel.Level.ToString();
         }
         _school = _spellModel.SpellSchool != SpellSchool.None ? _stringService.GetString(_spellModel.SpellSchool) : "Unknown";
         _range = !String.IsNullOrWhiteSpace(_spellModel.Range) ? _spellModel.Range : "Unknown";
         _ritual = _spellModel.IsRitual ? "Yes" : "No";
         _time = !String.IsNullOrWhiteSpace(_spellModel.CastingTime) ? _spellModel.CastingTime : "Unknown";
         _components = !String.IsNullOrWhiteSpace(_spellModel.Components) ? _spellModel.Components : "None";
         _duration = !String.IsNullOrWhiteSpace(_spellModel.Duration) ? _spellModel.Duration : "Unknown";
         _classes = !String.IsNullOrWhiteSpace(_spellModel.Classes) ? _spellModel.Classes : "Unknown";

         List<string> text = new List<string>();
         foreach (string s in _spellModel.TextCollection)
         {
            text.Add(s.Replace("\t", "").Trim());
         }
         _text = String.Join(Environment.NewLine + Environment.NewLine, text);

         _rolls = new List<string>(_spellModel.Rolls);

         _rollCommand = new RelayCommand(obj => true, obj => Roll((string)obj));
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets spell model
      /// </summary>
      public SpellModel SpellModel
      {
         get { return _spellModel; }
      }

      /// <summary>
      /// Gets name
      /// </summary>
      public string Name
      {
         get { return _spellModel.Name; }
      }

      /// <summary>
      /// Gets level
      /// </summary>
      public string Level
      {
         get { return _level; }
      }

      /// <summary>
      /// Gets school
      /// </summary>
      public string School
      {
         get { return _school; }
      }

      /// <summary>
      /// Gets range
      /// </summary>
      public string Range
      {
         get { return _range; }
      }

      /// <summary>
      /// Gets ritual
      /// </summary>
      public string Ritual
      {
         get { return _ritual; }
      }

      /// <summary>
      /// Gets time
      /// </summary>
      public string Time
      {
         get { return _time; }
      }

      /// <summary>
      /// Gets components
      /// </summary>
      public string Components
      {
         get { return _components; }
      }

      /// <summary>
      /// Gets duration
      /// </summary>
      public string Duration
      {
         get { return _duration; }
      }

      /// <summary>
      /// Gets classes
      /// </summary>
      public string Classes
      {
         get { return _classes; }
      }

      /// <summary>
      /// Gets text
      /// </summary>
      public string Text
      {
         get { return _text; }
      }

      /// <summary>
      /// Gets rolls
      /// </summary>
      public List<string> Rolls
      {
         get { return _rolls; }
      }

      /// <summary>
      /// Gets xml
      /// </summary>
      public string XML
      {
         get { return _spellModel.XML; }
      }

      /// <summary>
      /// Gets roll command
      /// </summary>
      public ICommand RollCommand
      {
         get { return _rollCommand; }
      }

      #endregion

      #region Private Methods

      private void Roll(string roll)
      {
         _dialogService.ShowDiceRollDialog("Roll " + roll, roll);
      }

      #endregion
   }
}
