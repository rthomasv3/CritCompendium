using System;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class AppliedConditionModel
   {
      #region Fields

      private Guid _id;
      private ConditionModel _conditionModel;
      private string _name;
      private int? _level;
      private string _notes;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="AppliedConditionModel"/>
      /// </summary>
      public AppliedConditionModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates an instance of <see cref="AppliedConditionModel"/>
      /// </summary>
      public AppliedConditionModel(AppliedConditionModel appliedConditionModel)
      {
         _id = appliedConditionModel.ID;
         if (appliedConditionModel.ConditionModel != null)
         {
            _conditionModel = new ConditionModel(appliedConditionModel.ConditionModel);
         }
         _name = appliedConditionModel.Name;
         _level = appliedConditionModel.Level;
         _notes = appliedConditionModel.Notes;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets ID
      /// </summary>
      public Guid ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Gets or sets condition model
      /// </summary>
      public ConditionModel ConditionModel
      {
         get { return _conditionModel; }
         set { _conditionModel = value; }
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
      /// Gets or sets level
      /// </summary>
      public int? Level
      {
         get { return _level; }
         set { _level = value; }
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
