using System;

namespace CritCompendiumInfrastructure.Models
{
	public sealed class LanguageModel
	{
		#region Fields

		private Guid _id;
		private string _name;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a default instance of <see cref="LanguageModel"/>
		/// </summary>
		public LanguageModel()
		{
			_id = Guid.NewGuid();
		}

		/// <summary>
		/// Creates a copy of <see cref="LanguageModel"/>
		/// </summary>
		public LanguageModel(LanguageModel languageModel)
		{
			_id = languageModel.ID;
			_name = languageModel.Name;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets id
		/// </summary>
		public Guid ID
		{
			get { return _id; }
			set { _id = value; }
		}

		/// <summary>
		/// Gets or sets language name
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		#endregion
	}
}
