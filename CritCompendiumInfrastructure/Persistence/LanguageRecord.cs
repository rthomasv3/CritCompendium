namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to save language information.
   /// </summary>
   public sealed class LanguageRecord : CompendiumEntryRecord
   {
      /// <summary>
      /// Gets or sets language description.
      /// </summary>
      public string Description { get; set; }
   }
}
