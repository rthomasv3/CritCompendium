using System;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class MonsterAttackViewModel
   {
      #region Fields

      private readonly StatService _statService = DependencyResolver.Resolve<StatService>();

      private readonly string _name;
      private readonly string _toHit;
      private readonly string _roll;
      private readonly bool _toHitVisible;
      private readonly bool _rollVisible;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="MonsterAttackViewModel"/>
      /// </summary>
      public MonsterAttackViewModel(MonsterAttackModel monsterAttackModel)
      {
         _name = monsterAttackModel.Name;
         _toHit = monsterAttackModel.ToHit.Contains("+") ? monsterAttackModel.ToHit : "+" + monsterAttackModel.ToHit;
         _roll = monsterAttackModel.Roll;
         _toHitVisible = !String.IsNullOrWhiteSpace(monsterAttackModel.ToHit);
         _rollVisible = !String.IsNullOrWhiteSpace(monsterAttackModel.Roll);
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
      /// Gets to hit
      /// </summary>
      public string ToHit
      {
         get { return _toHit; }
      }

      /// <summary>
      /// Gets to hit visible
      /// </summary>
      public bool ToHitVisible
      {
         get { return _toHitVisible; }
      }

      /// <summary>
      /// Gets roll
      /// </summary>
      public string Roll
      {
         get { return _roll; }
      }

      /// <summary>
      /// Gets roll visible
      /// </summary>
      public bool RollVisible
      {
         get { return _rollVisible; }
      }

      #endregion
   }
}
