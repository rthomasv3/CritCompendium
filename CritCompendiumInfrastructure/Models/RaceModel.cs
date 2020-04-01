using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class RaceModel
   {
      #region Fields

      private Guid _id;
      private string _name;
      private CreatureSize _size;
      private int _walkSpeed;
      private int _flySpeed;
      private Dictionary<Ability, int> _abilities = new Dictionary<Ability, int>();
      private List<Skill> _skillProficiencies = new List<Skill>();
      private List<string> _languages = new List<string>();
      private List<string> _weaponProficiencies = new List<string>();
      private List<string> _armorProficiencies = new List<string>();
      private List<string> _toolProficiencies = new List<string>();
      private List<TraitModel> _traits = new List<TraitModel>();
      private int _languageTrait;
      private string _xml;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a default instance of <see cref="RaceModel"/>
      /// </summary>
      public RaceModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates a copy of <see cref="RaceModel"/>
      /// </summary>
      public RaceModel(RaceModel raceModel)
      {
         _id = raceModel.ID;
         _name = raceModel.Name;
         _size = raceModel.Size;
         _walkSpeed = raceModel.WalkSpeed;
         _flySpeed = raceModel.FlySpeed;
         _abilities = new Dictionary<Ability, int>(raceModel.Abilities);
         _skillProficiencies = new List<Skill>(raceModel.SkillProficiencies);
         _languages = new List<string>(raceModel.Languages);
         _weaponProficiencies = new List<string>(raceModel.WeaponProficiencies);
         _armorProficiencies = new List<string>(raceModel.ArmorProficiencies);
         _toolProficiencies = new List<string>(raceModel.ToolProficiencies);
         _languageTrait = raceModel.LanguageTrait;

         _traits = new List<TraitModel>();
         foreach (TraitModel traitModel in raceModel.Traits)
         {
            _traits.Add(new TraitModel(traitModel));
         }

         _xml = raceModel.XML;
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
      /// Gets or sets size
      /// </summary>
      public CreatureSize Size
      {
         get { return _size; }
         set { _size = value; }
      }

      /// <summary>
      /// Gets or sets walk speed
      /// </summary>
      public int WalkSpeed
      {
         get { return _walkSpeed; }
         set { _walkSpeed = value; }
      }

      /// <summary>
      /// Gets or sets fly speed
      /// </summary>
      public int FlySpeed
      {
         get { return _flySpeed; }
         set { _flySpeed = value; }
      }

      /// <summary>
      /// Gets or sets abilities
      /// </summary>
      public Dictionary<Ability, int> Abilities
      {
         get { return _abilities; }
         set { _abilities = value; }
      }

      /// <summary>
      /// Gets or sets skill proficiencies
      /// </summary>
      public List<Skill> SkillProficiencies
      {
         get { return _skillProficiencies; }
         set { _skillProficiencies = value; }
      }

      /// <summary>
      /// Gets or sets languages
      /// </summary>
      public List<string> Languages
      {
         get { return _languages; }
         set { _languages = value; }
      }

      /// <summary>
      /// Gets or sets weapon proficiencies
      /// </summary>
      public List<string> WeaponProficiencies
      {
         get { return _weaponProficiencies; }
         set { _weaponProficiencies = value; }
      }

      /// <summary>
      /// Gets or sets armor proficiencies
      /// </summary>
      public List<string> ArmorProficiencies
      {
         get { return _armorProficiencies; }
         set { _armorProficiencies = value; }
      }

      /// <summary>
      /// Gets or sets tool proficiencies
      /// </summary>
      public List<string> ToolProficiencies
      {
         get { return _toolProficiencies; }
         set { _toolProficiencies = value; }
      }

      /// <summary>
      /// Gets or sets traits
      /// </summary>
      public List<TraitModel> Traits
      {
         get { return _traits; }
         set { _traits = value; }
      }

      /// <summary>
      /// Gets or sets language trait
      /// </summary>
      public int LanguageTrait
      {
         get { return _languageTrait; }
         set { _languageTrait = value; }
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
