using System;
using System.Collections.Generic;
using System.Text;

namespace CriticalCompendiumInfrastructure.Persistence
{
   /// <summary>
   /// Class used to save language information.
   /// </summary>
   public sealed class LanguageRecord
   {
		/// <summary>
		/// Gets or sets id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets language name
		/// </summary>
		public string Name { get; set; }
	}
}
