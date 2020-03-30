using System;
using System.Collections.Generic;
using CriticalCompendiumInfrastructure.Enums;

namespace CriticalCompendiumInfrastructure.Persistence
{
	/// <summary>
	/// Class used to save level information.
	/// </summary>
	public sealed class LevelRecord
   {
		/// <summary>
		/// Gets or sets id.
		/// </summary>
		public Guid ID { get; set; }

		/// <summary>
		/// Gets or sets level.
		/// </summary>
		public int Level { get; set; }

		/// <summary>
		/// Gets or sets level of class.
		/// </summary>
		public int LevelOfClass { get; set; }

		/// <summary>
		/// Gets or sets class.
		/// </summary>
		public Guid Class { get; set; }

		/// <summary>
		/// Gets or sets features.
		/// </summary>
		public List<FeatureRecord> Features { get; set; }

		/// <summary>
		/// Gets or sets feats.
		/// </summary>
		public List<Guid> Feats { get; set; }

		/// <summary>
		/// Gets or sets hit die result.
		/// </summary>
		public int HitDieResult { get; set; }

		/// <summary>
		/// Gets or sets hit die used.
		/// </summary>
		public bool HitDieUsed { get; set; }

		/// <summary>
		/// Gets or sets hit die rest result.
		/// </summary>
		public int HitDieRestRoll { get; set; }

		/// <summary>
		/// Gets or sets additional HP.
		/// </summary>
		public int AdditionalHP { get; set; }

		/// <summary>
		/// Gets or sets ability score improvements
		/// </summary>
		public List<KeyValuePair<Ability, int>> AbilityScoreImprovements { get; set; }
	}
}
