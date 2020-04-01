using System;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ListItemViewModels
{
   public sealed class ConditionListItemViewModel : NotifyPropertyChanged
   {
      #region Fields

      private ConditionModel _conditionModel;
      private string _details;
      private bool _isSelected = false;

      #endregion

      #region Constructors

      public ConditionListItemViewModel(ConditionModel conditionModel)
      {
         _conditionModel = conditionModel;

         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Condition model
      /// </summary>
      public ConditionModel ConditionModel
      {
         get { return _conditionModel; }
      }

      /// <summary>
      /// Name
      /// </summary>
      public string Name
      {
         get { return _conditionModel.Name; }
      }

      /// <summary>
      /// Gets details
      /// </summary>
      public string Details
      {
         get { return _details; }
      }

      /// <summary>
      /// True if selected
      /// </summary>
      public bool IsSelected
      {
         get { return _isSelected; }
         set { Set(ref _isSelected, value); }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Updates the model
      /// </summary>
      public void UpdateModel(ConditionModel conditionModel)
      {
         _conditionModel = conditionModel;

         Initialize();

         OnPropertyChanged("");
      }

      #endregion

      #region Non-Public Methods

      private void Initialize()
      {
         if (!String.IsNullOrWhiteSpace(_conditionModel.Description))
         {
            _details = _conditionModel.Description;
         }
         else if (_conditionModel.TextCollection.Count > 0)
         {
            _details = _conditionModel.TextCollection[0];
         }
         else
         {
            _details = "None";
         }
      }

      #endregion
   }
}
