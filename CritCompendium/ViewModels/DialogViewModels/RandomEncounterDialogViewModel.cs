using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Business;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business;

namespace CritCompendium.ViewModels.DialogViewModels
{
   public sealed class RandomEncounterDialogViewModel : NotifyPropertyChanged, IConfirmation
   {
      #region Events

      public event EventHandler AcceptSelected;
      public event EventHandler RejectSelected;
      public event EventHandler CancelSelected;

      #endregion

      #region Fields

      private readonly Compendium _compendium = DependencyResolver.Resolve<Compendium>();
      private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
      private readonly StatService _statService = DependencyResolver.Resolve<StatService>();
      private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();

      private readonly List<EncounterCharacterModel> _encounterCharacters;
      private readonly List<KeyValuePair<EncounterChallenge, string>> _difficultyOptions = new List<KeyValuePair<EncounterChallenge, string>>();
      private readonly List<KeyValuePair<string, string>> _environmentOptions = new List<KeyValuePair<string, string>>();
      private KeyValuePair<EncounterChallenge, string> _selectedDifficultyOption;
      private KeyValuePair<string, string> _selectedEnvironmentOption;
      private int _minMonsters;
      private int _maxMonsters;
      private ObservableCollection<EncounterMonsterViewModel> _monsters = new ObservableCollection<EncounterMonsterViewModel>();

      private readonly ICommand _acceptCommand;
      private readonly ICommand _rejectCommand;
      private readonly ICommand _generateMonstersCommand;
      private readonly ICommand _viewMonsterDetailsCommand;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="RandomEncounterDialogViewModel"/>
      /// </summary>
      public RandomEncounterDialogViewModel(List<EncounterCharacterModel> encounterCharacters)
      {
         _encounterCharacters = encounterCharacters;

         foreach (EncounterChallenge encounterChallenge in Enum.GetValues(typeof(EncounterChallenge)))
         {
            if (encounterChallenge != EncounterChallenge.Unknown)
            {
               _difficultyOptions.Add(new KeyValuePair<EncounterChallenge, string>(encounterChallenge, _stringService.GetString(encounterChallenge)));
            }
         }
         _selectedDifficultyOption = _difficultyOptions.FirstOrDefault();

         _environmentOptions.Add(new KeyValuePair<string, string>(null, "Any Environment"));
         foreach (MonsterModel monster in _compendium.Monsters)
         {
            if (!String.IsNullOrWhiteSpace(monster.Environment))
            {
               foreach (string environmentSplit in monster.Environment.Split(new char[] { ',' }))
               {
                  string environment = environmentSplit.Trim();
                  if (!_environmentOptions.Any(x => x.Value.ToLower() == environment.ToLower()))
                  {
                     _environmentOptions.Add(new KeyValuePair<string, string>(environment, _stringService.CapitalizeWords(environment)));
                  }
               }
            }
         }
         _selectedEnvironmentOption = _environmentOptions.FirstOrDefault();

         _minMonsters = 1;
         _maxMonsters = 10;

         _acceptCommand = new RelayCommand(obj => true, obj => OnAccept());
         _rejectCommand = new RelayCommand(obj => true, obj => OnReject());
         _generateMonstersCommand = new RelayCommand(obj => true, obj => GenerateMonsters());
         _viewMonsterDetailsCommand = new RelayCommand(obj => true, obj => ViewMonsterDetails((EncounterMonsterViewModel)obj));
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets difficulty options
      /// </summary>
      public List<KeyValuePair<EncounterChallenge, string>> DifficultyOptions
      {
         get { return _difficultyOptions; }
      }

      /// <summary>
      /// Gets environment options
      /// </summary>
      public List<KeyValuePair<string, string>> EnvironmentOptions
      {
         get { return _environmentOptions; }
      }

      /// <summary>
      /// Gets or sets selected difficulty option
      /// </summary>
      public KeyValuePair<EncounterChallenge, string> SelectedDifficultyOption
      {
         get { return _selectedDifficultyOption; }
         set { _selectedDifficultyOption = value; }
      }

      /// <summary>
      /// Gets or sets selected environment option
      /// </summary>
      public KeyValuePair<string, string> SelectedEnvironmentOption
      {
         get { return _selectedEnvironmentOption; }
         set { _selectedEnvironmentOption = value; }
      }

      /// <summary>
      /// Gets or sets min monsters
      /// </summary>
      public int MinMonsters
      {
         get { return _minMonsters; }
         set
         {
            if (value > 0)
            {
               _minMonsters = value;
            }
            else
            {
               _minMonsters = 1;
               OnPropertyChanged(nameof(MinMonsters));
            }
         }
      }

      /// <summary>
      /// Gets or sets maxMmonsters
      /// </summary>
      public int MaxMonsters
      {
         get { return _maxMonsters; }
         set
         {
            if (value > _minMonsters)
            {
               _maxMonsters = value;
            }
            else
            {
               _maxMonsters = _minMonsters;
               OnPropertyChanged(nameof(MaxMonsters));
            }
         }
      }

      /// <summary>
      /// Gets monsters
      /// </summary>
      public IEnumerable<EncounterMonsterViewModel> Monsters
      {
         get { return _monsters; }
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

      /// <summary>
      /// Gets generate monsters command
      /// </summary>
      public ICommand GenerateMonstersCommand
      {
         get { return _generateMonstersCommand; }
      }

      /// <summary>
      /// Gets view monster details command
      /// </summary>
      public ICommand ViewMonsterDetailsCommand
      {
         get { return _viewMonsterDetailsCommand; }
      }

      #endregion

      #region Private Methods

      private void OnAccept()
      {
         if (_monsters.Any())
         {
            if (_dialogService.ShowConfirmationDialog("Replace Monsters", "Replace all monsters in the encounter with the randomly generated monsters?", "Yes", "No", null) == true)
            {
               AcceptSelected?.Invoke(this, EventArgs.Empty);
            }
         }
         else
         {
            RejectSelected?.Invoke(this, EventArgs.Empty);
         }
      }

      private void OnReject()
      {
         RejectSelected?.Invoke(this, EventArgs.Empty);
      }

      private void GenerateMonsters()
      {
         Mouse.OverrideCursor = Cursors.Wait;

         int minTotalMonsterXP = 0;
         int maxTotalMonsterXP = 0;

         foreach (EncounterCharacterModel character in _encounterCharacters)
         {
            int baseThreshold = _statService.BaseXPThresholds[1];
            if (_statService.BaseXPThresholds.ContainsKey(character.Level))
            {
               baseThreshold = _statService.BaseXPThresholds[character.Level];
            }

            if (_selectedDifficultyOption.Key == EncounterChallenge.Easy)
            {
               maxTotalMonsterXP += (2 * baseThreshold);
            }
            else if (_selectedDifficultyOption.Key == EncounterChallenge.Medium)
            {
               minTotalMonsterXP += (2 * baseThreshold);
               maxTotalMonsterXP += (3 * baseThreshold);
            }
            else if (_selectedDifficultyOption.Key == EncounterChallenge.Hard)
            {
               minTotalMonsterXP += (3 * baseThreshold);
               maxTotalMonsterXP += (4 * baseThreshold);
            }
            else if (_selectedDifficultyOption.Key == EncounterChallenge.Deadly)
            {
               minTotalMonsterXP += (4 * baseThreshold);
               maxTotalMonsterXP += (5 * baseThreshold);
            }
            else if (_selectedDifficultyOption.Key == EncounterChallenge.TPK)
            {
               minTotalMonsterXP += (5 * baseThreshold);
               maxTotalMonsterXP += (20 * baseThreshold);
            }
         }

         int attempts = 0;
         List<EncounterMonsterModel> monsters = new List<EncounterMonsterModel>();
         while (attempts++ < 50)
         {
            int totalMonsterXP = 0;
            int totalMonsters = 0;
            int adjustedTotalMonsterXP = 0;
            string selectedEnvironment = _selectedEnvironmentOption.Key != null ? _selectedEnvironmentOption.Key.ToLower() : null;
            foreach (MonsterModel monsterModel in _compendium.Monsters.OrderBy(x => Guid.NewGuid()))
            {
               if (selectedEnvironment == null || (monsterModel.Environment != null && monsterModel.Environment.ToLower().Contains(selectedEnvironment)))
               {
                  int maxMonsterXP = Math.Min(maxTotalMonsterXP / _minMonsters, maxTotalMonsterXP - adjustedTotalMonsterXP);

                  if (maxMonsterXP < 25 ||
                      totalMonsters >= _maxMonsters ||
                      adjustedTotalMonsterXP > minTotalMonsterXP)
                  {
                     break;
                  }

                  int monsterXP = _statService.GetMonsterXP(monsterModel);

                  if (monsterXP > 0 && monsterXP <= maxMonsterXP)
                  {
                     EncounterMonsterModel encounterMonsterModel = new EncounterMonsterModel();
                     encounterMonsterModel.MonsterModel = monsterModel;

                     int maxQuantity = 0;
                     int potentialAdjust = (int)(_statService.GetXPMultiplier(totalMonsters + 1) * (float)(totalMonsterXP + monsterXP));
                     while (potentialAdjust < maxTotalMonsterXP)
                     {
                        potentialAdjust = (int)(_statService.GetXPMultiplier(totalMonsters + ++maxQuantity + 1) * (float)(totalMonsterXP + (monsterXP * (maxQuantity + 1))));
                     }

                     maxQuantity = Math.Min(maxQuantity, _maxMonsters - monsters.Count);

                     if (maxQuantity > 0)
                     {
                        encounterMonsterModel.Quantity = new Random((int)DateTime.Now.Ticks).Next(1, maxQuantity);
                        monsters.Add(encounterMonsterModel);

                        totalMonsters += encounterMonsterModel.Quantity;
                        totalMonsterXP += (monsterXP * encounterMonsterModel.Quantity);

                        adjustedTotalMonsterXP = (int)(_statService.GetXPMultiplier(totalMonsters) * (float)totalMonsterXP);
                     }
                  }
               }
            }

            if (_statService.EstimateEncounterChallenge(_encounterCharacters, monsters) == _selectedDifficultyOption.Key)
            {
               break;
            }
            else
            {
               monsters.Clear();
            }
         }

         Mouse.OverrideCursor = null;

         if (monsters.Count > 0)
         {
            _monsters.Clear();
            foreach (EncounterMonsterModel encounterMonster in monsters.OrderBy(x => x.Name))
            {
               _monsters.Add(new EncounterMonsterViewModel(encounterMonster));
            }
         }
         else
         {
            _dialogService.ShowConfirmationDialog("Encounter Creation Failed", "Unable to generate an encounter using the selected criteria.", "OK", null, null);
         }
      }

      private void ViewMonsterDetails(EncounterMonsterViewModel encounterMonsterViewModel)
      {
         if (encounterMonsterViewModel != null)
         {
            _dialogService.ShowDetailsDialog(encounterMonsterViewModel.MonsterViewModel);
         }
      }

      #endregion
   }
}
