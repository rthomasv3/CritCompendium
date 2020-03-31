using System;
using System.Collections.Generic;
using CriticalCompendiumInfrastructure.Enums;

namespace CriticalCompendiumInfrastructure.Persistence
{
	/// <summary>
	/// Class used to store trait information.
	/// </summary>
	public sealed class TraitRecord
   {
		/// <summary>
		/// Gets or sets id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets text collection.
		/// </summary>
		public List<string> TextCollection { get; set; }

		/// <summary>
		/// Gets or sets trait type.
		/// </summary>
		public TraitType TraitType { get; set; }
	}
}
