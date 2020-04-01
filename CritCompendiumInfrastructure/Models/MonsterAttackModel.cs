using System;

namespace CritCompendiumInfrastructure.Models
{
	public sealed class MonsterAttackModel
	{
		#region Fields

		private Guid _id;
		private string _name;
		private string _toHit;
		private string _roll;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a default instance of <see cref="MonsterAttackModel"/>
		/// </summary>
		public MonsterAttackModel()
		{
			_id = Guid.NewGuid();
		}

		/// <summary>
		/// Creates a copy of <see cref="MonsterAttackModel"/>
		/// </summary>
		public MonsterAttackModel(MonsterAttackModel monsterAttackModel)
		{
			_id = monsterAttackModel.ID;
			_name = monsterAttackModel.Name;
			_toHit = monsterAttackModel.ToHit;
			_roll = monsterAttackModel.Roll;
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
		/// Gets or sets name
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// Gets or sets to hit
		/// </summary>
		public string ToHit
		{
			get { return _toHit; }
			set { _toHit = value; }
		}

		/// <summary>
		/// Gets or sets roll
		/// </summary>
		public string Roll
		{
			get { return _roll; }
			set { _roll = value; }
		}

		#endregion
	}
}
