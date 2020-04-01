using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class SpellbookModel
   {
      #region Fields

      private Guid _id;
      private string _name;
      private bool _basedOnClass = true;
      private ClassModel _class;
      private bool _basedOnRace;
      private RaceModel _race;
      private Ability _ability = Ability.None;
      private int _additionalDCBonus;
      private int _additionalToHitBonus;
      private bool _resetOnShortRest;
      private bool _resetOnLongRest = true;
      private List<SpellbookEntryModel> _spells = new List<SpellbookEntryModel>();
      private int _currentFirstLevelSpellSlots;
      private int _currentSecondLevelSpellSlots;
      private int _currentThirdLevelSpellSlots;
      private int _currentFourthLevelSpellSlots;
      private int _currentFifthLevelSpellSlots;
      private int _currentSixthLevelSpellSlots;
      private int _currentSeventhLevelSpellSlots;
      private int _currentEighthLevelSpellSlots;
      private int _currentNinthLevelSpellSlots;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates an instance of <see cref="SpellbookModel"/>
      /// </summary>
      public SpellbookModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates an instance of <see cref="SpellbookModel"/>
      /// </summary>
      public SpellbookModel(SpellbookModel spellbookModel)
      {
         _id = spellbookModel.ID;
         _name = spellbookModel.Name;
         _basedOnClass = spellbookModel.BasedOnClass;
         _basedOnRace = spellbookModel.BasedOnRace;
         if (spellbookModel.Class != null)
         {
            _class = new ClassModel(spellbookModel.Class);
         }
         if (spellbookModel.Race != null)
         {
            _race = new RaceModel(spellbookModel.Race);
         }
         _ability = spellbookModel.Ability;
         _additionalDCBonus = spellbookModel.AdditionalDCBonus;
         _additionalToHitBonus = spellbookModel.AdditionalToHitBonus;
         _resetOnShortRest = spellbookModel.ResetOnShortRest;
         _resetOnLongRest = spellbookModel.ResetOnLongRest;
         _spells = new List<SpellbookEntryModel>();
         foreach (SpellbookEntryModel spellbookEntryModel in spellbookModel.Spells)
         {
            _spells.Add(new SpellbookEntryModel(spellbookEntryModel));
         }
         _currentFirstLevelSpellSlots = spellbookModel.CurrentFirstLevelSpellSlots;
         _currentSecondLevelSpellSlots = spellbookModel.CurrentSecondLevelSpellSlots;
         _currentThirdLevelSpellSlots = spellbookModel.CurrentThirdLevelSpellSlots;
         _currentFourthLevelSpellSlots = spellbookModel.CurrentFourthLevelSpellSlots;
         _currentFifthLevelSpellSlots = spellbookModel.CurrentFifthLevelSpellSlots;
         _currentSixthLevelSpellSlots = spellbookModel.CurrentSixthLevelSpellSlots;
         _currentSeventhLevelSpellSlots = spellbookModel.CurrentSeventhLevelSpellSlots;
         _currentEighthLevelSpellSlots = spellbookModel.CurrentEighthLevelSpellSlots;
         _currentNinthLevelSpellSlots = spellbookModel.CurrentNinthLevelSpellSlots;
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
      /// Gets or sets based on class
      /// </summary>
      public bool BasedOnClass
      {
         get { return _basedOnClass; }
         set { _basedOnClass = value; }
      }

      /// <summary>
      /// Gets or sets based on race
      /// </summary>
      public bool BasedOnRace
      {
         get { return _basedOnRace; }
         set { _basedOnRace = value; }
      }

      /// <summary>
      /// Gets or sets class
      /// </summary>
      public ClassModel Class
      {
         get { return _class; }
         set { _class = value; }
      }

      /// <summary>
      /// Gets or sets race
      /// </summary>
      public RaceModel Race
      {
         get { return _race; }
         set { _race = value; }
      }

      /// <summary>
      /// Gets or sets ability
      /// </summary>
      public Ability Ability
      {
         get { return _ability; }
         set { _ability = value; }
      }

      /// <summary>
      /// Gets or sets additional dc bonus
      /// </summary>
      public int AdditionalDCBonus
      {
         get { return _additionalDCBonus; }
         set { _additionalDCBonus = value; }
      }

      /// <summary>
      /// Gets or sets additional to hit bonus
      /// </summary>
      public int AdditionalToHitBonus
      {
         get { return _additionalToHitBonus; }
         set { _additionalToHitBonus = value; }
      }

      /// <summary>
      /// Gets or sets reset on short rest
      /// </summary>
      public bool ResetOnShortRest
      {
         get { return _resetOnShortRest; }
         set { _resetOnShortRest = value; }
      }

      /// <summary>
      /// Gets or sets reset on long rest
      /// </summary>
      public bool ResetOnLongRest
      {
         get { return _resetOnLongRest; }
         set { _resetOnLongRest = value; }
      }

      /// <summary>
      /// Gets or sets spells
      /// </summary>
      public List<SpellbookEntryModel> Spells
      {
         get { return _spells; }
         set { _spells = value; }
      }

      /// <summary>
      /// Gets or sets current first level spell slots
      /// </summary>
      public int CurrentFirstLevelSpellSlots
      {
         get { return _currentFirstLevelSpellSlots; }
         set { _currentFirstLevelSpellSlots = value; }
      }

      /// <summary>
      /// Gets or sets current second level spell slots
      /// </summary>
      public int CurrentSecondLevelSpellSlots
      {
         get { return _currentSecondLevelSpellSlots; }
         set { _currentSecondLevelSpellSlots = value; }
      }

      /// <summary>
      /// Gets or sets current thrid level spell slots
      /// </summary>
      public int CurrentThirdLevelSpellSlots
      {
         get { return _currentThirdLevelSpellSlots; }
         set { _currentThirdLevelSpellSlots = value; }
      }

      /// <summary>
      /// Gets or sets current fourth level spell slots
      /// </summary>
      public int CurrentFourthLevelSpellSlots
      {
         get { return _currentFourthLevelSpellSlots; }
         set { _currentFourthLevelSpellSlots = value; }
      }

      /// <summary>
      /// Gets or sets current fifth level spell slots
      /// </summary>
      public int CurrentFifthLevelSpellSlots
      {
         get { return _currentFifthLevelSpellSlots; }
         set { _currentFifthLevelSpellSlots = value; }
      }

      /// <summary>
      /// Gets or sets current sixth level spell slots
      /// </summary>
      public int CurrentSixthLevelSpellSlots
      {
         get { return _currentSixthLevelSpellSlots; }
         set { _currentSixthLevelSpellSlots = value; }
      }

      /// <summary>
      /// Gets or sets current seventh level spell slots
      /// </summary>
      public int CurrentSeventhLevelSpellSlots
      {
         get { return _currentSeventhLevelSpellSlots; }
         set { _currentSeventhLevelSpellSlots = value; }
      }

      /// <summary>
      /// Gets or sets current eighth level spell slots
      /// </summary>
      public int CurrentEighthLevelSpellSlots
      {
         get { return _currentEighthLevelSpellSlots; }
         set { _currentEighthLevelSpellSlots = value; }
      }

      /// <summary>
      /// Gets or sets current ninth level spell slots
      /// </summary>
      public int CurrentNinthLevelSpellSlots
      {
         get { return _currentNinthLevelSpellSlots; }
         set { _currentNinthLevelSpellSlots = value; }
      }

      #endregion
   }
}
