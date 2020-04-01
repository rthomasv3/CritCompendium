using System;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business;

namespace CritCompendium.ViewModels.ListItemViewModels
{
   public sealed class MonsterListItemViewModel : NotifyPropertyChanged
   {
      #region Fields

      private MonsterModel _monsterModel;
      private StringService _stringService;
      private string _details;
      private bool _isSelected = false;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates an instance of <see cref="MonsterListItemViewModel"/>
      /// </summary>
      public MonsterListItemViewModel(MonsterModel monsterModel, StringService stringService)
      {
         _monsterModel = monsterModel;
         _stringService = stringService;

         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Monster model
      /// </summary>
      public MonsterModel MonsterModel
      {
         get { return _monsterModel; }
      }

      /// <summary>
      /// Monster name
      /// </summary>
      public string Name
      {
         get { return _monsterModel.Name; }
      }

      /// <summary>
      /// Monster details
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
      public void UpdateModel(MonsterModel monsterModel)
      {
         _monsterModel = monsterModel;

         Initialize();

         OnPropertyChanged("");
      }

      #endregion

      #region Non-Public Methods

      private void Initialize()
      {
         if (_monsterModel.Size != CreatureSize.None && !String.IsNullOrWhiteSpace(_monsterModel.Type) && !String.IsNullOrWhiteSpace(_monsterModel.Alignment))
         {
            string sizeString = _stringService.GetString(_monsterModel.Size);
            _details = _stringService.CapitalizeWords(sizeString + " " + _monsterModel.Type + ", " + _monsterModel.Alignment);
         }
         else if (_monsterModel.Size != CreatureSize.None && !String.IsNullOrWhiteSpace(_monsterModel.Type))
         {
            string sizeString = _stringService.GetString(_monsterModel.Size);
            _details = _stringService.CapitalizeWords(sizeString + " " + _monsterModel.Type);
         }
         else if (_monsterModel.Size != CreatureSize.None)
         {
            _details = _stringService.GetString(_monsterModel.Size);
         }
         else if (!String.IsNullOrWhiteSpace(_monsterModel.Type))
         {
            _details = _monsterModel.Type;
         }
         else
         {
            _details = "Unknown";
         }
      }

      #endregion
   }
}
