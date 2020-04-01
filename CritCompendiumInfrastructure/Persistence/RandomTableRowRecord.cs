namespace CritCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to store random table row information.
   /// </summary>
   public sealed class RandomTableRowRecord
   {
      /// <summary>
      /// Gets or sets min.
      /// </summary>
      public int Min { get; set; }

      /// <summary>
      /// Gets or sets max.
      /// </summary>
      public int Max { get; set; }

      /// <summary>
      /// Gets or sets value.
      /// </summary>
      public string Value { get; set; }
   }
}
