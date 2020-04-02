namespace CritCompendiumInfrastructure.Models
{
   public sealed class LanguageModel : CompendiumEntryModel
   {
      #region Fields

      private string _description;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a default instance of <see cref="LanguageModel"/>
      /// </summary>
      public LanguageModel()
      {
      }

      /// <summary>
      /// Creates a copy of <see cref="LanguageModel"/>
      /// </summary>
      public LanguageModel(LanguageModel languageModel) : base(languageModel)
      {
         _description = languageModel.Description;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets language description.
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }

      #endregion
   }
}
