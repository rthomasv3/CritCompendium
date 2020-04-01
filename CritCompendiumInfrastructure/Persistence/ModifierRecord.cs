using System;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Persistence
{
	/// <summary>
	/// Class used to store modifier information.
	/// </summary>
	public sealed class ModifierRecord
   {
		/// <summary>
		/// Gets or sets id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets text.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets modifier category.
		/// </summary>
		public ModifierCategory ModifierCategory { get; set; }

		/// <summary>
		/// Gets or sets value.
		/// </summary>
		public int Value { get; set; }

		/// <summary>
		/// Gets or sets ability.
		/// </summary>
		public Ability Ability { get; set; }
	}
}
