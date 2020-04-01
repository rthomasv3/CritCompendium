using CritCompendiumInfrastructure.Services;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class EncounterCharacterModel : EncounterCreatureModel
   {
      #region Fields

      private readonly StatService _statService = DependencyResolver.Resolve<StatService>();

      private CharacterModel _characterModel;
      private int _level;
      private int _passiveInvestigation;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates an instance of <see cref="EncounterCharacterModel"/>
      /// </summary>
      public EncounterCharacterModel()
          : base()
      {
      }

      /// <summary>
      /// Creates an instance of <see cref="EncounterCharacterModel"/>
      /// </summary>
      public EncounterCharacterModel(EncounterCharacterModel encounterCharacterModel)
          : base(encounterCharacterModel)
      {
         _characterModel = encounterCharacterModel.CharacterModel;
         _level = encounterCharacterModel.Level;
         _passiveInvestigation = encounterCharacterModel.PassiveInvestigation;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets character model
      /// </summary>
      public CharacterModel CharacterModel
      {
         get { return _characterModel; }
         set { _characterModel = value; }
      }

      /// <summary>
      /// Gets or sets level
      /// </summary>
      public int Level
      {
         get { return _level; }
         set { _level = value; }
      }

      /// <summary>
      /// Gets or sets passive investigation
      /// </summary>
      public int PassiveInvestigation
      {
         get { return _passiveInvestigation; }
         set { _passiveInvestigation = value; }
      }

      #endregion
   }
}
