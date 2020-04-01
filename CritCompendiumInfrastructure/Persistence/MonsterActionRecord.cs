using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Persistence
{
	/// <summary>
	/// Class used to store monster action information.
	/// </summary>
	public sealed class MonsterActionRecord
   {
		/// <summary>
		/// Gets or sets id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets text.
		/// </summary>
		public List<string> TextCollection { get; set; }

		/// <summary>
		/// Gets or sets attacks.
		/// </summary>
		public List<MonsterAttackRecord> Attacks { get; set; }

		/// <summary>
		/// Gets or sets is legendary.
		/// </summary>
		public bool IsLegendary { get; set; }
	}
}
