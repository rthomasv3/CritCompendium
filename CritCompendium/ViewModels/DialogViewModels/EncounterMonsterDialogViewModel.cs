using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Business;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business;

namespace CritCompendium.ViewModels.DialogViewModels
{
   public class EncounterMonsterDialogViewModel : NotifyPropertyChanged, IConfirmation
   {
      #region Events

      public event EventHandler AcceptSelected;
      public event EventHandler RejectSelected;
      public event EventHandler CancelSelected;

      #endregion

      #region Fields

      private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();
      private readonly Compendium _compendium = DependencyResolver.Resolve<Compendium>();

      private readonly EncounterMonsterModel _encounterCreatureModel;
      private readonly List<KeyValuePair<MonsterModel, string>> _monsterOptions = new List<KeyValuePair<MonsterModel, string>>();
      private KeyValuePair<MonsterModel, string> _selectedMonsterOption;
      private readonly ICommand _searchMonstersCommand;
      private readonly ICommand _viewMonsterCommand;
      private readonly ICommand _acceptCommand;
      private readonly ICommand _rejectCommand;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="EncounterMonsterDialogViewModel"/>
      /// </summary>
      public EncounterMonsterDialogViewModel(EncounterMonsterModel encounterMonster)
      {
         _encounterCreatureModel = encounterMonster;

         _monsterOptions.Clear();
         _monsterOptions.Add(new KeyValuePair<MonsterModel, string>(null, "None"));
         foreach (MonsterModel monsterModel in _compendium.Monsters)
         {
            _monsterOptions.Add(new KeyValuePair<MonsterModel, string>(monsterModel, monsterModel.Name));
         }

         if (encounterMonster.MonsterModel != null)
         {
            _selectedMonsterOption = _monsterOptions.FirstOrDefault(x => x.Key != null && x.Key.ID == encounterMonster.MonsterModel.ID);
         }

         if (_selectedMonsterOption.Equals(default(KeyValuePair<MonsterModel, string>)))
         {
            _selectedMonsterOption = _monsterOptions[0];
         }

         _searchMonstersCommand = new RelayCommand(obj => true, obj => SearchMonsters());
         _viewMonsterCommand = new RelayCommand(obj => true, obj => ViewMonster());
         _acceptCommand = new RelayCommand(obj => true, obj => OnAccept());
         _rejectCommand = new RelayCommand(obj => true, obj => OnReject());
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets encounter monster model
      /// </summary>
      public EncounterMonsterModel EncounterMonsterModel
      {
         get { return _encounterCreatureModel; }
      }

      /// <summary>
      /// Gets monster options
      /// </summary>
      public IEnumerable<KeyValuePair<MonsterModel, string>> MonsterOptions
      {
         get { return _monsterOptions; }
      }

      /// <summary>
      /// Gets selected mosnter option
      /// </summary>
      public KeyValuePair<MonsterModel, string> SelectedMonsterOption
      {
         get { return _selectedMonsterOption; }
         set
         {
            _selectedMonsterOption = value;
            _encounterCreatureModel.MonsterModel = value.Key;
            OnPropertyChanged(String.Empty);
         }
      }

      /// <summary>
      /// Gets selected monster name
      /// </summary>
      public string SelectedMonsterName
      {
         get { return _encounterCreatureModel.MonsterModel != null ? _encounterCreatureModel.MonsterModel.Name : String.Empty; }
      }

      /// <summary>
      /// Gets or sets monster model
      /// </summary>
      public MonsterModel MonsterModel
      {
         get { return _encounterCreatureModel.MonsterModel; }
         set
         {
            _encounterCreatureModel.MonsterModel = value;
            OnPropertyChanged(String.Empty);
         }
      }

      /// <summary>
      /// Gets or sets name
      /// </summary>
      public string Name
      {
         get { return _encounterCreatureModel.Name; }
         set { _encounterCreatureModel.Name = value; }
      }

      /// <summary>
      /// Gets or sets current hp
      /// </summary>
      public int CurrentHP
      {
         get { return _encounterCreatureModel.CurrentHP; }
         set { _encounterCreatureModel.CurrentHP = value; }
      }

      /// <summary>
      /// Gets or sets quantity
      /// </summary>
      public int Quantity
      {
         get { return _encounterCreatureModel.Quantity; }
         set { _encounterCreatureModel.Quantity = value; }
      }

      /// <summary>
      /// Gets or sets cr
      /// </summary>
      public string CR
      {
         get { return String.IsNullOrWhiteSpace(_encounterCreatureModel.CR) ? "0" : _encounterCreatureModel.CR; }
         set { _encounterCreatureModel.CR = value; }
      }

      /// <summary>
      /// Gets or sets max hp
      /// </summary>
      public int MaxHP
      {
         get { return _encounterCreatureModel.MaxHP; }
         set { _encounterCreatureModel.MaxHP = value; }
      }

      /// <summary>
      /// Gets or sets ac
      /// </summary>
      public int AC
      {
         get { return _encounterCreatureModel.AC; }
         set { _encounterCreatureModel.AC = value; }
      }

      /// <summary>
      /// Gets or sets spell save dc
      /// </summary>
      public int SpellSaveDC
      {
         get { return _encounterCreatureModel.SpellSaveDC; }
         set { _encounterCreatureModel.SpellSaveDC = value; }
      }

      /// <summary>
      /// Gets or sets initiative bonus
      /// </summary>
      public int InitiativeBonus
      {
         get { return _encounterCreatureModel.InitiativeBonus; }
         set { _encounterCreatureModel.InitiativeBonus = value; }
      }

      /// <summary>
      /// Gets or sets initiative
      /// </summary>
      public int? Initiative
      {
         get { return _encounterCreatureModel.Initiative; }
         set { _encounterCreatureModel.Initiative = value; }
      }

      /// <summary>
      /// Gets or sets passive perception
      /// </summary>
      public int PassivePerception
      {
         get { return _encounterCreatureModel.PassivePerception; }
         set { _encounterCreatureModel.PassivePerception = value; }
      }

      /// <summary>
      /// Gets or sets average damage per turn
      /// </summary>
      public int AverageDamageTurn
      {
         get { return _encounterCreatureModel.AverageDamageTurn; }
         set { _encounterCreatureModel.AverageDamageTurn = value; }
      }

      /// <summary>
      /// Gets or sets damage vulnerabilities
      /// </summary>
      public string DamageVulnerabilities
      {
         get { return String.IsNullOrWhiteSpace(_encounterCreatureModel.DamageVulnerabilities) ? "None" : _encounterCreatureModel.DamageVulnerabilities; }
         set { _encounterCreatureModel.DamageVulnerabilities = value; }
      }

      /// <summary>
      /// Gets or sets damage resistances
      /// </summary>
      public string DamageResistances
      {
         get { return String.IsNullOrWhiteSpace(_encounterCreatureModel.DamageResistances) ? "None" : _encounterCreatureModel.DamageResistances; }
         set { _encounterCreatureModel.DamageResistances = value; }
      }

      /// <summary>
      /// Gets or sets damage immunities
      /// </summary>
      public string DamageImmunities
      {
         get { return String.IsNullOrWhiteSpace(_encounterCreatureModel.DamageImmunities) ? "None" : _encounterCreatureModel.DamageImmunities; }
         set { _encounterCreatureModel.DamageImmunities = value; }
      }

      /// <summary>
      /// Gets or sets condition immunities
      /// </summary>
      public string ConditionImmunities
      {
         get { return String.IsNullOrWhiteSpace(_encounterCreatureModel.ConditionImmunities) ? "None" : _encounterCreatureModel.ConditionImmunities; }
         set { _encounterCreatureModel.ConditionImmunities = value; }
      }

      /// <summary>
      /// Gets search monsters command
      /// </summary>
      public ICommand SearchMonstersCommand
      {
         get { return _searchMonstersCommand; }
      }

      /// <summary>
      /// Gets view monster command
      /// </summary>
      public ICommand ViewMonsterCommand
      {
         get { return _viewMonsterCommand; }
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

      #region Non-Public Methods

      private void SearchMonsters()
      {
         IEnumerable<MonsterModel> monsters = _dialogService.ShowMonsterSearchDialog(false);
         if (monsters.Any())
         {
            MonsterModel = monsters.First();

            KeyValuePair<MonsterModel, string> monsterPair = _monsterOptions.FirstOrDefault(x => x.Key != null && x.Key.ID == _encounterCreatureModel.MonsterModel.ID);
            if (!monsterPair.Equals(default(KeyValuePair<MonsterModel, string>)))
            {
               _selectedMonsterOption = monsterPair;
               OnPropertyChanged(nameof(SelectedMonsterOption));
            }
         }
      }

      private void ViewMonster()
      {
         if (_encounterCreatureModel.MonsterModel != null)
         {
            _dialogService.ShowDetailsDialog(new MonsterViewModel(_encounterCreatureModel.MonsterModel));
         }
      }

      private void OnAccept()
      {
         if (_encounterCreatureModel.MonsterModel == null)
         {
            _dialogService.ShowConfirmationDialog("Required Field", "Monster is required.", "OK", null, null);
         }
         else if (String.IsNullOrWhiteSpace(_encounterCreatureModel.Name))
         {
            _dialogService.ShowConfirmationDialog("Required Field", "Name is required.", "OK", null, null);
         }
         else
         {
            AcceptSelected?.Invoke(this, EventArgs.Empty);
         }
      }

      private void OnReject()
      {
         RejectSelected?.Invoke(this, EventArgs.Empty);
      }

      #endregion
   }
}
