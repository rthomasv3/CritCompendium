using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Business;
using CritCompendium.ViewModels.DialogViewModels;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class AppliedConditionViewModel : NotifyPropertyChanged, IConfirmation
   {
      #region Events

      public event EventHandler AcceptSelected;
      public event EventHandler RejectSelected;
      public event EventHandler CancelSelected;

      #endregion

      #region Fields

      private readonly Compendium _compendium = DependencyResolver.Resolve<Compendium>();
      private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();

      private readonly AppliedConditionModel _appliedConditionModel;
      private readonly List<KeyValuePair<ConditionModel, string>> _conditions = new List<KeyValuePair<ConditionModel, string>>();
      private readonly ICommand _viewConditionCommand;
      private readonly ICommand _acceptCommand;
      private readonly ICommand _rejectCommand;

      private KeyValuePair<ConditionModel, string> _selectedCondition;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="AppliedConditionViewModel"/>
      /// </summary>
      public AppliedConditionViewModel(AppliedConditionModel appliedConditionModel)
      {
         _appliedConditionModel = appliedConditionModel;

         foreach (ConditionModel conditionModel in _compendium.Conditions)
         {
            _conditions.Add(new KeyValuePair<ConditionModel, string>(conditionModel, conditionModel.Name));
         }

         if (_conditions.Any())
         {
            if (appliedConditionModel.ConditionModel == null)
            {
               _selectedCondition = _conditions.First();
               _appliedConditionModel.ConditionModel = _selectedCondition.Key;
               _appliedConditionModel.Name = _selectedCondition.Key.Name;
            }
            else
            {
               _selectedCondition = _conditions.FirstOrDefault(x => x.Key.Id == appliedConditionModel.ConditionModel.Id);
            }
         }

         _viewConditionCommand = new RelayCommand(obj => true, obj => ViewConditon());
         _acceptCommand = new RelayCommand(obj => true, obj => OnAccept());
         _rejectCommand = new RelayCommand(obj => true, obj => OnReject());
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets ID
      /// </summary>
      public Guid ID
      {
         get { return _appliedConditionModel.ID; }
      }

      /// <summary>
      /// Gets conditions
      /// </summary>
      public List<KeyValuePair<ConditionModel, string>> ConditionOptions
      {
         get { return _conditions; }
      }

      /// <summary>
      /// Gets applied condition model
      /// </summary>
      public AppliedConditionModel AppliedConditionModel
      {
         get { return _appliedConditionModel; }
      }

      /// <summary>
      /// Gets or sets selected condition
      /// </summary>
      public KeyValuePair<ConditionModel, string> SelectedConditionOption
      {
         get { return _selectedCondition; }
         set
         {
            _selectedCondition = value;
            _appliedConditionModel.ConditionModel = value.Key;
            _appliedConditionModel.Name = value.Key.Name;

            OnPropertyChanged(nameof(Condition));
            OnPropertyChanged(nameof(Name));
         }
      }

      /// <summary>
      /// Gets or sets condition model
      /// </summary>
      public ConditionModel Condition
      {
         get { return _appliedConditionModel.ConditionModel; }
         set { _appliedConditionModel.ConditionModel = value; }
      }

      /// <summary>
      /// Gets or sets name
      /// </summary>
      public string Name
      {
         get { return _appliedConditionModel.Name; }
         set { _appliedConditionModel.Name = value; }
      }

      public string NameAndLevel
      {
         get
         {
            string nameAndLevel = _appliedConditionModel.Name;

            if (_appliedConditionModel.Level.HasValue)
            {
               nameAndLevel += $" ({_appliedConditionModel.Level.Value})";
            }

            return nameAndLevel;
         }
      }

      /// <summary>
      /// Gets or sets level
      /// </summary>
      public int? Level
      {
         get { return _appliedConditionModel.Level; }
         set { _appliedConditionModel.Level = value; }
      }

      /// <summary>
      /// Gets or sets notes
      /// </summary>
      public string Notes
      {
         get { return _appliedConditionModel.Notes; }
         set { _appliedConditionModel.Notes = value; }
      }

      /// <summary>
      /// Gets view condition command
      /// </summary>
      public ICommand ViewConditionCommand
      {
         get { return _viewConditionCommand; }
      }

      /// <summary>
      /// Gets accept command
      /// </summary>
      public ICommand AcceptCommand
      {
         get { return _acceptCommand; }
      }

      /// <summary>
      /// Gets reject command
      /// </summary>
      public ICommand RejectCommand
      {
         get { return _rejectCommand; }
      }

      #endregion

      #region Private Methods

      private void ViewConditon()
      {
         if (_appliedConditionModel.ConditionModel != null)
         {
            _dialogService.ShowDetailsDialog(new ConditionViewModel(_appliedConditionModel.ConditionModel));
         }
      }

      private void OnAccept()
      {
         AcceptSelected?.Invoke(this, EventArgs.Empty);
      }

      private void OnReject()
      {
         RejectSelected?.Invoke(this, EventArgs.Empty);
      }

      #endregion
   }
}
