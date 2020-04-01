using System;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class StatModificationModel
   {
      #region Fields

      private Guid _id;
      private StatModificationOption _modificationOption;
      private bool _fixedValue;
      private int _value;
      private string _notes;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="StatModificationModel"/>
      /// </summary>
      public StatModificationModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates an instance of <see cref="StatModificationModel"/>
      /// </summary>
      public StatModificationModel(StatModificationModel statModificationModel)
      {
         _id = statModificationModel.ID;
         _modificationOption = statModificationModel.ModificationOption;
         _fixedValue = statModificationModel.FixedValue;
         _value = statModificationModel.Value;
         _notes = statModificationModel.Notes;
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
      /// Gets or sets modification option
      /// </summary>
      public StatModificationOption ModificationOption
      {
         get { return _modificationOption; }
         set { _modificationOption = value; }
      }

      /// <summary>
      /// Gets or sets fixed value
      /// </summary>
      public bool FixedValue
      {
         get { return _fixedValue; }
         set { _fixedValue = value; }
      }

      /// <summary>
      /// Gets or sets value
      /// </summary>
      public int Value
      {
         get { return _value; }
         set { _value = value; }
      }

      /// <summary>
      /// Gets or sets notes
      /// </summary>
      public string Notes
      {
         get { return _notes; }
         set { _notes = value; }
      }

      #endregion
   }
}
