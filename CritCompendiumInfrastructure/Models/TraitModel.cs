using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class TraitModel
   {
      #region Fields

      private Guid _id;
      private string _name;
      private List<string> _textCollection = new List<string>();
      private TraitType _traitType;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a default instance of <see cref="TraitModel"/>
      /// </summary>
      public TraitModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates a copy of <see cref="TraitModel"/>
      /// </summary>
      public TraitModel(TraitModel traitModel)
      {
         _id = traitModel.ID;
         _name = traitModel.Name;
         _textCollection = new List<string>(traitModel.TextCollection);

         string lowerName = _name.ToLower();

         if (lowerName == "languages" || lowerName == "language")
         {
            _traitType = TraitType.Language;
         }
         else if (lowerName == "tool proficiencies" || lowerName == "tool proficiency")
         {
            _traitType = TraitType.Tool_Proficiency;
         }
         else if (lowerName == "skill proficiencies" || lowerName == "skill proficiency")
         {
            _traitType = TraitType.Skill_Proficiency;
         }
         else if (lowerName == "starting proficiencies" || lowerName == "starting proficiency")
         {
            _traitType = TraitType.Starting_Proficiency;
         }
         else
         {
            _traitType = TraitType.Information;
         }
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets id
      /// </summary>
      public Guid ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Gets or sets name
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Gets or sets text collection
      /// </summary>
      public List<string> TextCollection
      {
         get { return _textCollection; }
         set { _textCollection = value; }
      }

      /// <summary>
      /// Gets or sets trait type
      /// </summary>
      public TraitType TraitType
      {
         get { return _traitType; }
         set { _traitType = value; }
      }

      /// <summary>
      /// Gets the joined text
      /// </summary>
      public string Text
      {
         get { return String.Join(Environment.NewLine, _textCollection); }
      }

      #endregion
   }
}
