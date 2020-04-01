using System;
using System.Windows.Input;
using CritCompendium.Business;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class EncounterMonsterViewModel : EncounterCreatureViewModel
   {
      #region Fields

      private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();
      private readonly ICommand _viewMonsterCommand;
      private EncounterMonsterModel _encounterMonsterModel;
      private MonsterViewModel _monsterViewModel;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="EncounterMonsterViewModel"/>
      /// </summary>
      public EncounterMonsterViewModel(EncounterMonsterModel encounterMonsterModel)
          : base(encounterMonsterModel)
      {
         _encounterMonsterModel = encounterMonsterModel;
         _monsterViewModel = new MonsterViewModel(encounterMonsterModel.MonsterModel);

         _viewMonsterCommand = new RelayCommand(obj => true, obj => OpenMonsterDetailDialog());
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets encounter monster model
      /// </summary>
      public EncounterMonsterModel EncounterMonsterModel
      {
         get { return _encounterMonsterModel; }
      }

      /// <summary>
      /// Gets monster view model
      /// </summary>
      public MonsterViewModel MonsterViewModel
      {
         get { return _monsterViewModel; }
      }

      /// <summary>
      /// Gets or sets quantity
      /// </summary>
      public int Quantity
      {
         get { return _encounterMonsterModel.Quantity; }
         set { _encounterMonsterModel.Quantity = value; }
      }

      /// <summary>
      /// Gets or sets average damage per turn
      /// </summary>
      public int AverageDamageTurn
      {
         get { return _encounterMonsterModel.AverageDamageTurn; }
         set { _encounterMonsterModel.AverageDamageTurn = value; }
      }

      /// <summary>
      /// Gets or sets cr
      /// </summary>
      public string CR
      {
         get { return String.IsNullOrWhiteSpace(_encounterMonsterModel.CR) ? "0" : _encounterMonsterModel.CR; }
         set { _encounterMonsterModel.CR = value; }
      }

      /// <summary>
      /// Gets or sets damage vulnerabilities
      /// </summary>
      public string DamageVulnerabilities
      {
         get { return String.IsNullOrWhiteSpace(_encounterMonsterModel.DamageVulnerabilities) ? "None" : _encounterMonsterModel.DamageVulnerabilities; }
         set { _encounterMonsterModel.DamageVulnerabilities = value; }
      }

      /// <summary>
      /// Gets or sets damage resistances
      /// </summary>
      public string DamageResistances
      {
         get { return String.IsNullOrWhiteSpace(_encounterMonsterModel.DamageResistances) ? "None" : _encounterMonsterModel.DamageResistances; }
         set { _encounterMonsterModel.DamageResistances = value; }
      }

      /// <summary>
      /// Gets or sets damage immunities
      /// </summary>
      public string DamageImmunities
      {
         get { return String.IsNullOrWhiteSpace(_encounterMonsterModel.DamageImmunities) ? "None" : _encounterMonsterModel.DamageImmunities; }
         set { _encounterMonsterModel.DamageImmunities = value; }
      }

      /// <summary>
      /// Gets or sets condition immunities
      /// </summary>
      public string ConditionImmunities
      {
         get { return String.IsNullOrWhiteSpace(_encounterMonsterModel.ConditionImmunities) ? "None" : _encounterMonsterModel.ConditionImmunities; }
         set { _encounterMonsterModel.ConditionImmunities = value; }
      }

      /// <summary>
      /// Gets view monster command
      /// </summary>
      public ICommand ViewMonsterCommand
      {
         get { return _viewMonsterCommand; }
      }

      #endregion

      #region Private Methods

      private void OpenMonsterDetailDialog()
      {
         _dialogService.ShowDetailsDialog(_monsterViewModel);
      }

      #endregion
   }
}
