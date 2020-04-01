using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.DialogViewModels
{
   public sealed class InitiativeViewModel : NotifyPropertyChanged, IConfirmation
   {
      #region Events

      public event EventHandler AcceptSelected;
      public event EventHandler RejectSelected;
      public event EventHandler CancelSelected;

      #endregion

      #region Fields

      private readonly DiceService _diceService;
      private readonly ICommand _rollCharacterInitiativeCommand;
      private readonly ICommand _rollAllMonstersInitiativeCommand;
      private readonly ICommand _rollMonsterInitiativeCommand;
      private readonly ICommand _acceptCommand;
      private readonly ICommand _rejectCommand;
      private List<EncounterCreatureViewModel> _creatures;
      private ObservableCollection<EncounterCreatureViewModel> _characters = new ObservableCollection<EncounterCreatureViewModel>();
      private ObservableCollection<EncounterCreatureViewModel> _monsters = new ObservableCollection<EncounterCreatureViewModel>();

      #endregion

      #region Constructor

      public InitiativeViewModel(DiceService diceService)
      {
         _diceService = diceService;

         _rollCharacterInitiativeCommand = new RelayCommand(obj => true, obj => RollCharacterInitiative((EncounterCreatureViewModel)obj));
         _rollAllMonstersInitiativeCommand = new RelayCommand(obj => true, obj => RollAllMonstersInitiative());
         _rollMonsterInitiativeCommand = new RelayCommand(obj => true, obj => RollMonsterInitiative((EncounterCreatureViewModel)obj));
         _acceptCommand = new RelayCommand(obj => true, obj => Accept());
         _rejectCommand = new RelayCommand(obj => true, obj => Reject());
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets characters
      /// </summary>
      public IEnumerable<EncounterCreatureViewModel> Characters
      {
         get { return _characters; }
      }

      /// <summary>
      /// Gets monsters
      /// </summary>
      public IEnumerable<EncounterCreatureViewModel> Monsters
      {
         get { return _monsters; }
      }

      /// <summary>
      /// Gets roll character initiative command
      /// </summary>
      public ICommand RollCharacterInitiativeCommand
      {
         get { return _rollCharacterInitiativeCommand; }
      }

      /// <summary>
      /// Gets roll all monsters initiative command
      /// </summary>
      public ICommand RollAllMonstersInitiativeCommand
      {
         get { return _rollAllMonstersInitiativeCommand; }
      }

      /// <summary>
      /// Gets roll monster initiative command
      /// </summary>
      public ICommand RollMonsterInitiativeCommand
      {
         get { return _rollMonsterInitiativeCommand; }
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

      #region Public Methods

      /// <summary>
      /// Sets the creatures to roll initiative for
      /// </summary>
      /// <param name="creatures"></param>
      public void SetCreatures(List<EncounterCreatureViewModel> creatures)
      {
         _creatures = creatures;

         _characters.Clear();
         _monsters.Clear();

         foreach (EncounterCreatureViewModel encounterCreatureViewModel in _creatures)
         {
            if (encounterCreatureViewModel is EncounterCharacterViewModel encounterCharacterViewModel)
            {
               EncounterCharacterModel characterModelCopy = new EncounterCharacterModel(encounterCharacterViewModel.EncounterCharacterModel);
               characterModelCopy.Initiative = 0;
               _characters.Add(new EncounterCharacterViewModel(characterModelCopy));
            }
            else if (encounterCreatureViewModel is EncounterMonsterViewModel encounterMonsterViewModel)
            {
               EncounterMonsterModel monsterModelCopy = new EncounterMonsterModel(encounterMonsterViewModel.EncounterMonsterModel);
               monsterModelCopy.Initiative = 0;
               _monsters.Add(new EncounterMonsterViewModel(monsterModelCopy));
            }
         }
      }

      #endregion

      #region Private Methods

      private void RollCharacterInitiative(EncounterCreatureViewModel character)
      {
         character.Initiative = (int)_diceService.EvaluateExpression($"1d20+{character.InitiativeBonus}").Item1;
      }

      private void RollAllMonstersInitiative()
      {
         foreach (EncounterCreatureViewModel monster in _monsters)
         {
            monster.Initiative = (int)_diceService.EvaluateExpression($"1d20+{monster.InitiativeBonus}").Item1;
         }
      }

      private void RollMonsterInitiative(EncounterCreatureViewModel monster)
      {
         monster.Initiative = (int)_diceService.EvaluateExpression($"1d20+{monster.InitiativeBonus}").Item1;
      }

      private void Accept()
      {
         foreach (EncounterCreatureViewModel character in _characters)
         {
            EncounterCreatureViewModel viewModel = _creatures.FirstOrDefault(x => x.EncounterCreatureModel.ID == character.EncounterCreatureModel.ID);
            if (viewModel != null)
            {
               viewModel.Initiative = character.Initiative;
            }
         }

         foreach (EncounterCreatureViewModel monster in _monsters)
         {
            EncounterCreatureViewModel viewModel = _creatures.FirstOrDefault(x => x.EncounterCreatureModel.ID == monster.EncounterCreatureModel.ID);
            if (viewModel != null)
            {
               viewModel.Initiative = monster.Initiative;
            }
         }

         AcceptSelected?.Invoke(this, EventArgs.Empty);
      }

      private void Reject()
      {
         RejectSelected?.Invoke(this, EventArgs.Empty);
      }

      #endregion
   }
}
