using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class MonsterActionModel
   {
      #region Fields

      private Guid _id;
      private string _name;
      private List<string> _textCollection = new List<string>();
      private List<MonsterAttackModel> _attacks = new List<MonsterAttackModel>();
      private bool _isLegendary;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a default instance of <see cref="MonsterActionModel"/>
      /// </summary>
      public MonsterActionModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates a copy of <see cref="MonsterActionModel"/>
      /// </summary>
      public MonsterActionModel(MonsterActionModel monsterActionModel)
      {
         _id = monsterActionModel.ID;
         _name = monsterActionModel.Name;
         _textCollection = new List<string>(monsterActionModel.TextCollection);
         _isLegendary = monsterActionModel.IsLegendary;

         _attacks = new List<MonsterAttackModel>();
         foreach (MonsterAttackModel monsterAttackModel in monsterActionModel.Attacks)
         {
            _attacks.Add(new MonsterAttackModel(monsterAttackModel));
         }
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
      /// Gets or sets text
      /// </summary>
      public List<string> TextCollection
      {
         get { return _textCollection; }
         set { _textCollection = value; }
      }

      /// <summary>
      /// Gets or sets attacks
      /// </summary>
      public List<MonsterAttackModel> Attacks
      {
         get { return _attacks; }
         set { _attacks = value; }
      }

      /// <summary>
      /// Gets or sets is legendary
      /// </summary>
      public bool IsLegendary
      {
         get { return _isLegendary; }
         set { _isLegendary = value; }
      }

      #endregion
   }
}
