using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class StatModificationViewModel : ObjectViewModel
   {
      #region Fields

      private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
      private readonly StatService _statService = DependencyResolver.Resolve<StatService>();
      private readonly List<KeyValuePair<StatModificationOption, string>> _modificationOptions = new List<KeyValuePair<StatModificationOption, string>>();
      private KeyValuePair<StatModificationOption, string> _selectedModificationOption;
      private readonly StatModificationModel _statModificationModel;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="StatModificationViewModel"/>
      /// </summary>
      public StatModificationViewModel(StatModificationModel statModificationModel) : base()
      {
         _statModificationModel = statModificationModel;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets id
      /// </summary>
      public Guid ID
      {
         get { return _statModificationModel.ID; }
      }

      /// <summary>
      /// Gets stat modification model
      /// </summary>
      public StatModificationModel StatModificationModel
      {
         get { return _statModificationModel; }
      }

      /// <summary>
      /// Gets modification options
      /// </summary>
      public List<KeyValuePair<StatModificationOption, string>> ModificationOptions
      {
         get { return _modificationOptions; }
      }

      public KeyValuePair<StatModificationOption, string> SelectedModificationOption
      {
         get { return _selectedModificationOption; }
         set
         {
            _selectedModificationOption = value;
            _statModificationModel.ModificationOption = value.Key;
            OnPropertyChanged(nameof(ModificationOption));
            OnPropertyChanged(nameof(ModificationOptionDisplay));
         }
      }

      /// <summary>
      /// Gets or sets modification option
      /// </summary>
      public StatModificationOption ModificationOption
      {
         get { return _statModificationModel.ModificationOption; }
         set { _statModificationModel.ModificationOption = value; }
      }

      /// <summary>
      /// Gets modification option display
      /// </summary>
      public string ModificationOptionDisplay
      {
         get { return _stringService.GetString(_statModificationModel.ModificationOption); }
      }

      /// <summary>
      /// Gets or sets fixed value
      /// </summary>
      public bool FixedValue
      {
         get { return _statModificationModel.FixedValue; }
         set { _statModificationModel.FixedValue = value; }
      }

      /// <summary>
      /// Gets or sets value
      /// </summary>
      public int Value
      {
         get { return _statModificationModel.Value; }
         set
         {
            _statModificationModel.Value = value;
            OnPropertyChanged(nameof(ValueDisplay));
         }
      }

      /// <summary>
      /// Gets value display
      /// </summary>
      public string ValueDisplay
      {
         get
         {
            return _statModificationModel.FixedValue ? $"{_statModificationModel.Value} (Fixed)" :
                                                       _statService.AddPlusOrMinus(_statModificationModel.Value);
         }
      }

      /// <summary>
      /// Gets or sets notes
      /// </summary>
      public string Notes
      {
         get { return _statModificationModel.Notes; }
         set { _statModificationModel.Notes = value; }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Initializes options
      /// </summary>
      public void InitializeOptions()
      {
         foreach (StatModificationOption statOption in Enum.GetValues(typeof(StatModificationOption)))
         {
            _modificationOptions.Add(new KeyValuePair<StatModificationOption, string>(statOption, _stringService.GetString(statOption)));
         }

         KeyValuePair<StatModificationOption, string> selected = _modificationOptions.FirstOrDefault(x => x.Key == _statModificationModel.ModificationOption);
         if (!selected.Equals(default(KeyValuePair<StatModificationOption, string>)))
         {
            _selectedModificationOption = selected;
         }
         else
         {
            _selectedModificationOption = _modificationOptions[0];
         }
      }

      #endregion
   }
}
