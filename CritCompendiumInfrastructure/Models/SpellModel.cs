using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class SpellModel : CompendiumEntryModel
   {
      #region Fields

      private int _level;
      private SpellSchool _spellSchool;
      private bool _isRitual;
      private string _castingTime;
      private string _range;
      private string _components;
      private string _duration;
      private string _classes;
      private List<string> _textCollection = new List<string>();
      private List<string> _rolls = new List<string>();
      private string _xml;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a default instance of <see cref="SpellModel"/>
      /// </summary>
      public SpellModel()
      {
      }

      /// <summary>
      /// Creates a copy of <see cref="SpellModel"/>
      /// </summary>
      public SpellModel(SpellModel spellModel) : base(spellModel)
      {
         _level = spellModel.Level;
         _spellSchool = spellModel.SpellSchool;
         _isRitual = spellModel.IsRitual;
         _castingTime = spellModel.CastingTime;
         _range = spellModel.Range;
         _components = spellModel.Components;
         _duration = spellModel.Duration;
         _classes = spellModel.Classes;
         List<string> _text = new List<string>(spellModel.TextCollection);
         List<string> _rolls = new List<string>(spellModel.Rolls);

         _xml = spellModel.XML;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets level
      /// </summary>
      public int Level
      {
         get { return _level; }
         set { _level = value; }
      }

      /// <summary>
      /// Gets or sets spell school
      /// </summary>
      public SpellSchool SpellSchool
      {
         get { return _spellSchool; }
         set { _spellSchool = value; }
      }

      /// <summary>
      /// Gets or sets is ritual
      /// </summary>
      public bool IsRitual
      {
         get { return _isRitual; }
         set { _isRitual = value; }
      }

      /// <summary>
      /// Gets or sets casting time
      /// </summary>
      public string CastingTime
      {
         get { return _castingTime; }
         set { _castingTime = value; }
      }

      /// <summary>
      /// Gets or sets range
      /// </summary>
      public string Range
      {
         get { return _range; }
         set { _range = value; }
      }

      /// <summary>
      /// Gets or sets components
      /// </summary>
      public string Components
      {
         get { return _components; }
         set { _components = value; }
      }

      /// <summary>
      /// Gets or sets duration
      /// </summary>
      public string Duration
      {
         get { return _duration; }
         set { _duration = value; }
      }

      /// <summary>
      /// Gets or sets classes
      /// </summary>
      public string Classes
      {
         get { return _classes; }
         set { _classes = value; }
      }

      /// <summary>
      /// Gets or sets text
      /// </summary>
      public List<string> TextCollection
      {
         get { return _textCollection; }
         set { _textCollection = value; }
      }

      /// <summary>
      /// Gets or sets rolls
      /// </summary>
      public List<string> Rolls
      {
         get { return _rolls; }
         set { _rolls = value; }
      }

      /// <summary>
      /// Gets or sets xml
      /// </summary>
      public string XML
      {
         get { return _xml; }
         set { _xml = value; }
      }

      #endregion
   }
}
