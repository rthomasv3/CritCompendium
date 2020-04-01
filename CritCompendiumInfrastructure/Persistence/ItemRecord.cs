using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Persistence
{
	/// <summary>
	/// Class used to store item information.
	/// </summary>
	public sealed class ItemRecord
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
		/// Gets or sets type.
		/// </summary>
		public ItemType Type { get; set; }

		/// <summary>
		/// Gets or sets magic.
		/// </summary>
		public bool Magic { get; set; }

		/// <summary>
		/// Gets or sets requresAttunement.
		/// </summary>
		public bool RequiresAttunement { get; set; }

		/// <summary>
		/// Gets or sets value.
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// Gets or sets weight.
		/// </summary>
		public string Weight { get; set; }

		/// <summary>
		/// Gets or sets dmg1.
		/// </summary>
		public string Dmg1 { get; set; }

		/// <summary>
		/// Gets or sets dmg2.
		/// </summary>
		public string Dmg2 { get; set; }

		/// <summary>
		/// Gets or sets dmgType.
		/// </summary>
		public string DmgType { get; set; }

		/// <summary>
		/// Gets or sets properties.
		/// </summary>
		public string Properties { get; set; }

		/// <summary>
		/// Gets or sets rarity.
		/// </summary>
		public Rarity Rarity { get; set; }

		/// <summary>
		/// Gets or sets ac.
		/// </summary>
		public string AC { get; set; }

		/// <summary>
		/// Gets or sets strength requirement.
		/// </summary>
		public string StrengthRequirement { get; set; }

		/// <summary>
		/// Gets or sets stealth disadvantage.
		/// </summary>
		public bool StealthDisadvantage { get; set; }

		/// <summary>
		/// Gets or sets range.
		/// </summary>
		public string Range { get; set; }

		/// <summary>
		/// Gets or sets text.
		/// </summary>
		public List<string> TextCollection { get; set; }

		/// <summary>
		/// Gets or sets modifiers.
		/// </summary>
		public List<ModifierRecord> Modifiers { get; set; }

		/// <summary>
		/// Gets or sets rolls.
		/// </summary>
		public List<string> Rolls { get; set; }
	}
}
