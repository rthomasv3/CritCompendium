using System;
using System.Collections.Generic;
using CriticalCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
	public sealed class MonsterActionViewModel
	{
		#region Fields

		private readonly string _name;
		private readonly string _text;
		private readonly List<MonsterAttackViewModel> _attacks = new List<MonsterAttackViewModel>();

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance of <see cref="MonsterActionViewModel"/>
		/// </summary>
		public MonsterActionViewModel(MonsterActionModel monsterActionModel)
		{
			_name = monsterActionModel.Name;
			_text = String.Join(Environment.NewLine, monsterActionModel.TextCollection);

			foreach (MonsterAttackModel monsterAttack in monsterActionModel.Attacks)
			{
				_attacks.Add(new MonsterAttackViewModel(monsterAttack));
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets name
		/// </summary>
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Gets text
		/// </summary>
		public string Text
		{
			get { return _text; }
		}

		/// <summary>
		/// Gets attacks
		/// </summary>
		public List<MonsterAttackViewModel> Attacks
		{
			get { return _attacks; }
		}

		#endregion
	}
}
