using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Business;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class EncounterViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();
      private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
      private readonly StatService _statService = DependencyResolver.Resolve<StatService>();

      private readonly EncounterModel _encounterModel;
      private string _name;
      private string _notes;
      private List<EncounterCreatureViewModel> _creatures = new List<EncounterCreatureViewModel>();
      private List<EncounterCreatureViewModel> _creaturesExpanded = new List<EncounterCreatureViewModel>();


      private readonly ICommand _playEncounterCommand;
      private readonly ICommand _pauseEncounterCommand;
      private readonly ICommand _stopEncounterCommand;
      private readonly ICommand _nextInitiativeCommand;
      private readonly ICommand _addCharacterCommand;
      private readonly ICommand _editCharacterCommand;
      private readonly ICommand _copyCharacterCommand;
      private readonly ICommand _removeCharacterCommand;
      private readonly ICommand _addMonsterCommand;
      private readonly ICommand _editMonsterCommand;
      private readonly ICommand _copyMonsterCommand;
      private readonly ICommand _removeMonsterCommand;
      private readonly ICommand _randomEncounterCommand;
      private readonly ICommand _viewNotesCommand;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="EncounterViewModel"/>
      /// </summary>
      public EncounterViewModel(EncounterModel encounterModel)
      {
         _encounterModel = new EncounterModel(encounterModel);

         _name = _encounterModel.Name;
         _notes = _encounterModel.Notes;

         CreateCreatureViewModels();
         EstimateEncounterChallenge();

         _playEncounterCommand = new RelayCommand(obj => true, obj => PlayEncounter());
         _pauseEncounterCommand = new RelayCommand(obj => true, obj => PauseEncounter());
         _stopEncounterCommand = new RelayCommand(obj => StateIsPlaying || StateIsPaused, obj => StopEncounter());
         _nextInitiativeCommand = new RelayCommand(obj => StateIsPlaying, obj => NextInitiative());
         _addCharacterCommand = new RelayCommand(obj => true, obj => ShowAddCharacterWindow());
         _editCharacterCommand = new RelayCommand(obj => true, obj => ShowEditCharacterWindow((EncounterCharacterViewModel)obj));
         _copyCharacterCommand = new RelayCommand(obj => true, obj => CopyCharacter((EncounterCharacterViewModel)obj));
         _removeCharacterCommand = new RelayCommand(obj => true, obj => RemoveCharacter((EncounterCharacterViewModel)obj));
         _addMonsterCommand = new RelayCommand(obj => true, obj => ShowAddMonsterWindow());
         _editMonsterCommand = new RelayCommand(obj => true, obj => ShowEditMonsterWindow((EncounterMonsterViewModel)obj));
         _copyMonsterCommand = new RelayCommand(obj => true, obj => CopyMonster((EncounterMonsterViewModel)obj));
         _removeMonsterCommand = new RelayCommand(obj => true, obj => RemoveMonster((EncounterMonsterViewModel)obj));
         _randomEncounterCommand = new RelayCommand(obj => true, obj => ShowRandomEncounterWindow());
         _viewNotesCommand = new RelayCommand(obj => true, obj => ViewNotes());
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets encounter model
      /// </summary>
      public EncounterModel EncounterModel
      {
         get { return _encounterModel; }
      }

      /// <summary>
      /// Gets or sets name
      /// </summary>
      public string Name
      {
         get { return _name; }
         set
         {
            if (Set(ref _name, value))
            {
               _encounterModel.Name = _name;
            }
         }
      }

      /// <summary>
      /// Gets estimated encounter challenge
      /// </summary>
      public string EstimatedChallenge
      {
         get { return _stringService.GetString(_encounterModel.EncounterChallenge); }
      }

      /// <summary>
      /// Gets or sets notes
      /// </summary>
      public string Notes
      {
         get { return _notes; }
         set
         {
            if (Set(ref _notes, value))
            {
               _encounterModel.Notes = _notes;
            }
         }
      }

      /// <summary>
      /// True if state is playing
      /// </summary>
      public bool StateIsPlaying
      {
         get { return _encounterModel.EncounterState == EncounterState.Playing; }
      }

      /// <summary>
      /// True if state is paused
      /// </summary>
      public bool StateIsPaused
      {
         get { return _encounterModel.EncounterState == EncounterState.Paused; }
      }

      /// <summary>
      /// True if state is stopped
      /// </summary>
      public bool StateIsStopped
      {
         get { return _encounterModel.EncounterState == EncounterState.Stopped; }
      }

      /// <summary>
      /// True if state is stopped or paused
      /// </summary>
      public bool StateIsStoppedOrPaused
      {
         get { return _encounterModel.EncounterState == EncounterState.Stopped || _encounterModel.EncounterState == EncounterState.Paused; }
      }

      /// <summary>
      /// Gets current initiative
      /// </summary>
      public string CurrentInitiative
      {
         get
         {
            string current = "None";

            EncounterCreatureViewModel selected = _creaturesExpanded.FirstOrDefault(x => x.Selected);
            if (selected != null)
            {
               int initiative = selected.Initiative.HasValue ? selected.Initiative.Value : 0;
               current = $"{initiative} - {selected.Name}";
            }

            return current;
         }
      }

      /// <summary>
      /// Gets round
      /// </summary>
      public int Round
      {
         get { return _encounterModel.Round; }
      }

      /// <summary>
      /// Gets time elapsed
      /// </summary>
      public string TimeElapsed
      {
         get { return TimeSpan.FromSeconds(Math.Max(0, _encounterModel.Round - 1) * 6).ToString("m'm 'ss's'"); }
      }

      /// <summary>
      /// Gets creatures
      /// </summary>
      public IEnumerable<EncounterCreatureViewModel> Creatures
      {
         get { return _creatures; }
      }

      /// <summary>
      /// Gets creatures expanded by quantity
      /// </summary>
      public IEnumerable<EncounterCreatureViewModel> CreaturesExpanded
      {
         get { return _creaturesExpanded; }
      }

      /// <summary>
      /// Gets characters
      /// </summary>
      public IEnumerable<EncounterCreatureViewModel> Characters
      {
         get { return _creatures.Where(x => x is EncounterCharacterViewModel); }
      }

      /// <summary>
      /// Gets monsters
      /// </summary>
      public IEnumerable<EncounterCreatureViewModel> Monsters
      {
         get { return _creatures.Where(x => x is EncounterMonsterViewModel); }
      }

      /// <summary>
      /// Gets monsters expanded by quantity
      /// </summary>
      public IEnumerable<EncounterCreatureViewModel> MonstersExpanded
      {
         get { return _creaturesExpanded.Where(x => x is EncounterMonsterViewModel); }
      }

      /// <summary>
      /// Gets average level
      /// </summary>
      public int AverageCharacterLevel
      {
         get { return Characters.Any() ? (int)Characters.Average(x => ((EncounterCharacterViewModel)x).Level) : 0; }
      }

      /// <summary>
      /// Gets average max hp
      /// </summary>
      public int AverageCharacterMaxHP
      {
         get { return Characters.Any() ? (int)Characters.Average(x => x.MaxHP) : 0; }
      }

      /// <summary>
      /// Gets average ac
      /// </summary>
      public int AverageCharacterAC
      {
         get { return Characters.Any() ? (int)Characters.Average(x => x.AC) : 0; }
      }

      /// <summary>
      /// Gets average save dc
      /// </summary>
      public int AverageCharacterSaveDC
      {
         get { return Characters.Any() ? (int)Characters.Average(x => x.SpellSaveDC) : 0; }
      }

      /// <summary>
      /// Gets average passive perception
      /// </summary>
      public int AverageCharacterPassivePerception
      {
         get { return Characters.Any() ? (int)Characters.Average(x => x.PassivePerception) : 0; }
      }

      /// <summary>
      /// Gets average passive investigation
      /// </summary>
      public int AverageCharacterPassiveInvestigation
      {
         get { return Characters.Any() ? (int)Characters.Average(x => ((EncounterCharacterViewModel)x).PassiveInvestigation) : 0; }
      }

      /// <summary>
      /// Gets total monster xp
      /// </summary>
      public int TotalMonsterXP
      {
         get
         {
            int totalXP = 0;

            foreach (EncounterMonsterViewModel monster in MonstersExpanded)
            {
               string xpString = _stringService.CRXPString(monster.CR);
               if (!String.IsNullOrWhiteSpace(xpString))
               {
                  if (Int32.TryParse(xpString.Replace(",", ""), out int xp))
                  {
                     totalXP += xp;
                  }
               }
            }

            return totalXP;
         }
      }

      /// <summary>
      /// Gets average monster cr
      /// </summary>
      public int AverageMonsterCR
      {
         get { return Monsters.Any() ? (int)MonstersExpanded.Average(x => _statService.CRToFloat(((EncounterMonsterViewModel)x).CR)) : 0; }
      }

      /// <summary>
      /// Gets average monster max hp
      /// </summary>
      public int AverageMonsterMaxHP
      {
         get { return Monsters.Any() ? (int)MonstersExpanded.Average(x => ((EncounterMonsterViewModel)x).MaxHP) : 0; }
      }

      /// <summary>
      /// Gets average monster ac
      /// </summary>
      public int AverageMonsterAC
      {
         get { return Monsters.Any() ? (int)MonstersExpanded.Average(x => ((EncounterMonsterViewModel)x).AC) : 0; }
      }

      /// <summary>
      /// Gets average monster save dc
      /// </summary>
      public int AverageMonsterSaveDC
      {
         get { return Monsters.Any() ? (int)MonstersExpanded.Average(x => ((EncounterMonsterViewModel)x).SpellSaveDC) : 0; }
      }

      /// <summary>
      /// Gets average monster passive perception
      /// </summary>
      public int AverageMonsterPassivePerception
      {
         get { return Monsters.Any() ? (int)MonstersExpanded.Average(x => ((EncounterMonsterViewModel)x).PassivePerception) : 0; }
      }

      /// <summary>
      /// Gets average monster damage/turn
      /// </summary>
      public int AverageMonsterDamageTurn
      {
         get { return Monsters.Any() ? (int)MonstersExpanded.Average(x => ((EncounterMonsterViewModel)x).AverageDamageTurn) : 0; }
      }

      /// <summary>
      /// Gets overall monster damage vulnerabilities
      /// </summary>
      public string OverallMonsterDamageVulnerabilities
      {
         get
         {
            HashSet<string> vulnerabilities = new HashSet<string>();
            foreach (EncounterMonsterViewModel monster in Monsters)
            {
               if (!String.IsNullOrWhiteSpace(monster.EncounterMonsterModel.DamageVulnerabilities))
               {
                  foreach (string s in _stringService.CapitalizeWords(monster.EncounterMonsterModel.DamageVulnerabilities).Split(new char[] { ',' }))
                  {
                     vulnerabilities.Add(s.Trim());
                  }
               }
            }
            return vulnerabilities.Any() ? String.Join(", ", vulnerabilities) : "None";
         }
      }

      /// <summary>
      /// Gets overall monster damage resistances
      /// </summary>
      public string OverallMonsterDamageResistances
      {
         get
         {
            HashSet<string> resistances = new HashSet<string>();
            foreach (EncounterMonsterViewModel monster in Monsters)
            {
               if (!String.IsNullOrWhiteSpace(monster.EncounterMonsterModel.DamageResistances))
               {
                  foreach (string s in _stringService.CapitalizeWords(monster.EncounterMonsterModel.DamageResistances).Split(new char[] { ',' }))
                  {
                     resistances.Add(s.Trim());
                  }
               }
            }
            return resistances.Any() ? String.Join(", ", resistances) : "None";
         }
      }

      /// <summary>
      /// Gets overall monster damage immunities
      /// </summary>
      public string OverallMonsterDamageImmunities
      {
         get
         {
            HashSet<string> immunities = new HashSet<string>();
            foreach (EncounterMonsterViewModel monster in Monsters)
            {
               if (!String.IsNullOrWhiteSpace(monster.EncounterMonsterModel.DamageImmunities))
               {
                  foreach (string s in _stringService.CapitalizeWords(monster.EncounterMonsterModel.DamageImmunities).Split(new char[] { ',' }))
                  {
                     immunities.Add(s.Trim());
                  }
               }
            }
            return immunities.Any() ? String.Join(", ", immunities) : "None";
         }
      }

      /// <summary>
      /// Gets overall monster condition immunities
      /// </summary>
      public string OverallMonsterConditionImmunities
      {
         get
         {
            HashSet<string> immunities = new HashSet<string>();
            foreach (EncounterMonsterViewModel monster in Monsters)
            {
               if (!String.IsNullOrWhiteSpace(monster.EncounterMonsterModel.ConditionImmunities))
               {
                  foreach (string s in _stringService.CapitalizeWords(monster.EncounterMonsterModel.ConditionImmunities).Split(new char[] { ',' }))
                  {
                     immunities.Add(s.Trim());
                  }
               }
            }
            return immunities.Any() ? String.Join(", ", immunities) : "None";
         }
      }

      /// <summary>
      /// Gets play encounter command
      /// </summary>
      public ICommand PlayEncounterCommand
      {
         get { return _playEncounterCommand; }
      }

      /// <summary>
      /// Gets pause encounter command
      /// </summary>
      public ICommand PauseEncounterCommand
      {
         get { return _pauseEncounterCommand; }
      }

      /// <summary>
      /// Gets stop encounter command
      /// </summary>
      public ICommand StopEncounterCommand
      {
         get { return _stopEncounterCommand; }
      }

      /// <summary>
      /// Gets next initiative command
      /// </summary>
      public ICommand NextInitiativeCommand
      {
         get { return _nextInitiativeCommand; }
      }

      /// <summary>
      /// Gets add character command
      /// </summary>
      public ICommand AddCharacterCommand
      {
         get { return _addCharacterCommand; }
      }

      /// <summary>
      /// Gets edit character command
      /// </summary>
      public ICommand EditCharacterCommand
      {
         get { return _editCharacterCommand; }
      }

      /// <summary>
      /// Gets copy character command
      /// </summary>
      public ICommand CopyCharacterCommand
      {
         get { return _copyCharacterCommand; }
      }

      /// <summary>
      /// Gets remove character command
      /// </summary>
      public ICommand RemoveCharacterCommand
      {
         get { return _removeCharacterCommand; }
      }

      /// <summary>
      /// Gets add monster command
      /// </summary>
      public ICommand AddMonsterCommand
      {
         get { return _addMonsterCommand; }
      }

      /// <summary>
      /// Gets edit monster command
      /// </summary>
      public ICommand EditMonsterCommand
      {
         get { return _editMonsterCommand; }
      }

      /// <summary>
      /// Gets copy monster command
      /// </summary>
      public ICommand CopyMonsterCommand
      {
         get { return _copyMonsterCommand; }
      }

      /// <summary>
      /// Gets remove monster command
      /// </summary>
      public ICommand RemoveMonsterCommand
      {
         get { return _removeMonsterCommand; }
      }

      /// <summary>
      /// Gets random encounter command
      /// </summary>
      public ICommand RandomEncounterCommand
      {
         get { return _randomEncounterCommand; }
      }

      /// <summary>
      /// Gets view notes command
      /// </summary>
      public ICommand ViewNotesCommand
      {
         get { return _viewNotesCommand; }
      }

      #endregion

      #region Private Methods

      private void CreateCreatureViewModels()
      {
         _creatures.Clear();
         _creaturesExpanded.Clear();
         foreach (EncounterCreatureModel encounterCreatureModel in _encounterModel.Creatures)
         {
            if (encounterCreatureModel is EncounterCharacterModel)
            {
               EncounterCharacterViewModel encounterCharacterViewModel = new EncounterCharacterViewModel(encounterCreatureModel as EncounterCharacterModel);
               _creatures.Add(encounterCharacterViewModel);
               _creaturesExpanded.Add(encounterCharacterViewModel);
            }
            else if (encounterCreatureModel is EncounterMonsterModel)
            {
               EncounterMonsterViewModel encounterMonsterViewModel = new EncounterMonsterViewModel(encounterCreatureModel as EncounterMonsterModel);
               _creatures.Add(encounterMonsterViewModel);

               if (encounterMonsterViewModel.Quantity > 1)
               {
                  for (int i = 0; i < encounterMonsterViewModel.Quantity; ++i)
                  {
                     EncounterMonsterModel encounterMonsterModelCopy = new EncounterMonsterModel(encounterCreatureModel as EncounterMonsterModel);
                     EncounterMonsterViewModel encounterMonsterViewModelCopy = new EncounterMonsterViewModel(encounterMonsterModelCopy);
                     encounterMonsterViewModelCopy.Name += $" ({i + 1})";
                     _creaturesExpanded.Add(encounterMonsterViewModelCopy);
                  }
               }
               else
               {
                  _creaturesExpanded.Add(encounterMonsterViewModel);
               }
            }
         }

         _creatures = _creatures.OrderByDescending(x => x.Initiative).ToList();
         _creaturesExpanded = _creaturesExpanded.OrderByDescending(x => x.Initiative).ToList();
      }

      private void PlayEncounter()
      {
         List<EncounterCreatureViewModel> creatures = _creatures.Where(x => x.Initiative == null).ToList();

         if (creatures.Any())
         {
            bool? result = _dialogService.ShowInitiativeDialog(creatures);

            if (result == true)
            {
               CreateCreatureViewModels();

               if (_encounterModel.Round == 0)
               {
                  _encounterModel.Round = 1;
                  OnPropertyChanged(nameof(Round));
                  OnPropertyChanged(nameof(TimeElapsed));

                  NextInitiative();
               }

               _encounterModel.EncounterState = EncounterState.Playing;
               OnPropertyChanged(nameof(StateIsPaused));
               OnPropertyChanged(nameof(StateIsPlaying));
               OnPropertyChanged(nameof(StateIsStopped));
               OnPropertyChanged(nameof(StateIsStoppedOrPaused));
               OnPropertyChanged(nameof(CreaturesExpanded));
            }
         }
         else
         {
            _encounterModel.EncounterState = EncounterState.Playing;
            OnPropertyChanged(nameof(StateIsPaused));
            OnPropertyChanged(nameof(StateIsPlaying));
            OnPropertyChanged(nameof(StateIsStopped));
            OnPropertyChanged(nameof(StateIsStoppedOrPaused));
         }
      }

      private void PauseEncounter()
      {
         _encounterModel.EncounterState = EncounterState.Paused;
         OnPropertyChanged(nameof(StateIsPaused));
         OnPropertyChanged(nameof(StateIsPlaying));
         OnPropertyChanged(nameof(StateIsStopped));
         OnPropertyChanged(nameof(StateIsStoppedOrPaused));
      }

      private void StopEncounter()
      {
         bool? result = _dialogService.ShowConfirmationDialog("Stop Encounter",
             $"Are you sure you want to stop the encounter?{Environment.NewLine + Environment.NewLine}This will reset current round, elapsed time, initiative, hp, and conditions.", "Yes", "No", null);

         if (result == true)
         {
            _encounterModel.Round = 0;

            foreach (EncounterCreatureModel encounterCreatureModel in _encounterModel.Creatures)
            {
               encounterCreatureModel.Initiative = null;
               encounterCreatureModel.Selected = false;
               encounterCreatureModel.CurrentHP = encounterCreatureModel.MaxHP;
               encounterCreatureModel.Conditions.Clear();
            }

            CreateCreatureViewModels();

            _encounterModel.EncounterState = EncounterState.Stopped;
            OnPropertyChanged(nameof(Round));
            OnPropertyChanged(nameof(TimeElapsed));
            OnPropertyChanged(nameof(CurrentInitiative));
            OnPropertyChanged(nameof(StateIsPaused));
            OnPropertyChanged(nameof(StateIsPlaying));
            OnPropertyChanged(nameof(StateIsStopped));
            OnPropertyChanged(nameof(StateIsStoppedOrPaused));
            OnPropertyChanged(nameof(Creatures));
            OnPropertyChanged(nameof(CreaturesExpanded));
         }
      }

      private void NextInitiative()
      {
         if (_creatures.Any())
         {
            EncounterCreatureViewModel selected = _creaturesExpanded.FirstOrDefault(x => x.Selected);
            if (selected == null)
            {
               _creaturesExpanded[0].Selected = true;
            }
            else if (selected == _creaturesExpanded.Last())
            {
               selected.Selected = false;
               _creaturesExpanded[0].Selected = true;
               _encounterModel.Round++;

               OnPropertyChanged(nameof(Round));
               OnPropertyChanged(nameof(TimeElapsed));
            }
            else
            {
               selected.Selected = false;
               _creaturesExpanded[_creaturesExpanded.IndexOf(selected) + 1].Selected = true;
            }

            OnPropertyChanged(nameof(CurrentInitiative));
         }
      }

      private void ShowAddCharacterWindow()
      {
         EncounterCharacterModel encounterCharacterModel = _dialogService.ShowEncounterCharacterDialog("Add Character", new EncounterCharacterModel());

         if (encounterCharacterModel != null)
         {
            _encounterModel.Creatures.Add(encounterCharacterModel);

            CreateCreatureViewModels();
            EstimateEncounterChallenge();

            OnPropertyChanged(nameof(Characters));
            OnPropertyChanged(nameof(AverageCharacterLevel));
            OnPropertyChanged(nameof(AverageCharacterMaxHP));
            OnPropertyChanged(nameof(AverageCharacterAC));
            OnPropertyChanged(nameof(AverageCharacterSaveDC));
            OnPropertyChanged(nameof(AverageCharacterPassivePerception));
            OnPropertyChanged(nameof(AverageCharacterPassiveInvestigation));
         }
      }

      private void ShowEditCharacterWindow(EncounterCharacterViewModel encounterCharacterModel)
      {
         EncounterCharacterModel editedCharacterModel = _dialogService.ShowEncounterCharacterDialog("Edit Character", encounterCharacterModel.EncounterCharacterModel);

         if (editedCharacterModel != null)
         {
            EncounterCreatureModel originalCharacter = _encounterModel.Creatures.FirstOrDefault(x => x.ID == encounterCharacterModel.EncounterCharacterModel.ID);
            if (originalCharacter != null)
            {
               _encounterModel.Creatures[_encounterModel.Creatures.IndexOf(originalCharacter)] = editedCharacterModel;

               CreateCreatureViewModels();
               EstimateEncounterChallenge();

               OnPropertyChanged(nameof(Characters));
               OnPropertyChanged(nameof(AverageCharacterLevel));
               OnPropertyChanged(nameof(AverageCharacterMaxHP));
               OnPropertyChanged(nameof(AverageCharacterAC));
               OnPropertyChanged(nameof(AverageCharacterSaveDC));
               OnPropertyChanged(nameof(AverageCharacterPassivePerception));
               OnPropertyChanged(nameof(AverageCharacterPassiveInvestigation));
            }
         }
      }

      private void CopyCharacter(EncounterCharacterViewModel encounterCharacterModel)
      {
         EncounterCharacterModel copy = new EncounterCharacterModel(encounterCharacterModel.EncounterCharacterModel);
         copy.Name += " (copy)";
         copy.ID = Guid.NewGuid();
         _encounterModel.Creatures.Add(copy);

         CreateCreatureViewModels();
         EstimateEncounterChallenge();

         OnPropertyChanged(nameof(Characters));
         OnPropertyChanged(nameof(AverageCharacterLevel));
         OnPropertyChanged(nameof(AverageCharacterMaxHP));
         OnPropertyChanged(nameof(AverageCharacterAC));
         OnPropertyChanged(nameof(AverageCharacterSaveDC));
         OnPropertyChanged(nameof(AverageCharacterPassivePerception));
         OnPropertyChanged(nameof(AverageCharacterPassiveInvestigation));
      }

      private void RemoveCharacter(EncounterCharacterViewModel encounterCharacterModel)
      {
         bool? result = _dialogService.ShowConfirmationDialog("Remove Character", "Are you sure you want to remove " + encounterCharacterModel.Name + "?", "Yes", "No", null);
         if (result == true)
         {
            _encounterModel.Creatures.RemoveAll(x => x.ID == encounterCharacterModel.EncounterCharacterModel.ID);

            CreateCreatureViewModels();
            EstimateEncounterChallenge();

            OnPropertyChanged(nameof(Characters));
            OnPropertyChanged(nameof(AverageCharacterLevel));
            OnPropertyChanged(nameof(AverageCharacterMaxHP));
            OnPropertyChanged(nameof(AverageCharacterAC));
            OnPropertyChanged(nameof(AverageCharacterSaveDC));
            OnPropertyChanged(nameof(AverageCharacterPassivePerception));
            OnPropertyChanged(nameof(AverageCharacterPassiveInvestigation));
         }
      }

      private void ShowAddMonsterWindow()
      {
         EncounterMonsterModel encounterMonsterModel = _dialogService.ShowEncounterMonsterDialog("Add Monster", new EncounterMonsterModel());

         if (encounterMonsterModel != null)
         {
            encounterMonsterModel.CurrentHP = encounterMonsterModel.MaxHP;
            _encounterModel.Creatures.Add(encounterMonsterModel);

            CreateCreatureViewModels();
            EstimateEncounterChallenge();

            OnPropertyChanged(nameof(Monsters));
            OnPropertyChanged(nameof(TotalMonsterXP));
            OnPropertyChanged(nameof(AverageMonsterCR));
            OnPropertyChanged(nameof(AverageMonsterMaxHP));
            OnPropertyChanged(nameof(AverageMonsterAC));
            OnPropertyChanged(nameof(AverageMonsterSaveDC));
            OnPropertyChanged(nameof(AverageMonsterPassivePerception));
            OnPropertyChanged(nameof(AverageMonsterDamageTurn));
            OnPropertyChanged(nameof(OverallMonsterDamageVulnerabilities));
            OnPropertyChanged(nameof(OverallMonsterDamageResistances));
            OnPropertyChanged(nameof(OverallMonsterDamageImmunities));
            OnPropertyChanged(nameof(OverallMonsterConditionImmunities));
         }
      }

      private void ShowEditMonsterWindow(EncounterMonsterViewModel encounterMonsterViewModel)
      {
         EncounterMonsterModel editedMonsterModel = _dialogService.ShowEncounterMonsterDialog("Edit Monster", encounterMonsterViewModel.EncounterMonsterModel);

         if (editedMonsterModel != null)
         {
            EncounterCreatureModel originalMonster = _encounterModel.Creatures.FirstOrDefault(x => x.ID == encounterMonsterViewModel.EncounterMonsterModel.ID);
            if (originalMonster != null)
            {
               _encounterModel.Creatures[_encounterModel.Creatures.IndexOf(originalMonster)] = editedMonsterModel;

               CreateCreatureViewModels();
               EstimateEncounterChallenge();

               OnPropertyChanged(nameof(Monsters));
               OnPropertyChanged(nameof(TotalMonsterXP));
               OnPropertyChanged(nameof(AverageMonsterCR));
               OnPropertyChanged(nameof(AverageMonsterMaxHP));
               OnPropertyChanged(nameof(AverageMonsterAC));
               OnPropertyChanged(nameof(AverageMonsterSaveDC));
               OnPropertyChanged(nameof(AverageMonsterPassivePerception));
               OnPropertyChanged(nameof(AverageMonsterDamageTurn));
               OnPropertyChanged(nameof(OverallMonsterDamageVulnerabilities));
               OnPropertyChanged(nameof(OverallMonsterDamageResistances));
               OnPropertyChanged(nameof(OverallMonsterDamageImmunities));
               OnPropertyChanged(nameof(OverallMonsterConditionImmunities));
            }
         }
      }

      private void CopyMonster(EncounterMonsterViewModel encounterMonsterModel)
      {
         EncounterMonsterModel copy = new EncounterMonsterModel(encounterMonsterModel.EncounterMonsterModel);
         copy.Name += " (copy)";
         copy.ID = Guid.NewGuid();
         _encounterModel.Creatures.Add(copy);

         CreateCreatureViewModels();
         EstimateEncounterChallenge();

         OnPropertyChanged(nameof(Monsters));
         OnPropertyChanged(nameof(TotalMonsterXP));
         OnPropertyChanged(nameof(AverageMonsterCR));
         OnPropertyChanged(nameof(AverageMonsterMaxHP));
         OnPropertyChanged(nameof(AverageMonsterAC));
         OnPropertyChanged(nameof(AverageMonsterSaveDC));
         OnPropertyChanged(nameof(AverageMonsterPassivePerception));
         OnPropertyChanged(nameof(AverageMonsterDamageTurn));
         OnPropertyChanged(nameof(OverallMonsterDamageVulnerabilities));
         OnPropertyChanged(nameof(OverallMonsterDamageResistances));
         OnPropertyChanged(nameof(OverallMonsterDamageImmunities));
         OnPropertyChanged(nameof(OverallMonsterConditionImmunities));
      }

      private void RemoveMonster(EncounterMonsterViewModel encounterMonsterModel)
      {
         bool? result = _dialogService.ShowConfirmationDialog("Remove Monster", "Are you sure you want to remove " + encounterMonsterModel.Name + "?", "Yes", "No", null);
         if (result == true)
         {
            _encounterModel.Creatures.RemoveAll(x => x.ID == encounterMonsterModel.EncounterMonsterModel.ID);

            CreateCreatureViewModels();
            EstimateEncounterChallenge();

            OnPropertyChanged(nameof(Monsters));
            OnPropertyChanged(nameof(TotalMonsterXP));
            OnPropertyChanged(nameof(AverageMonsterCR));
            OnPropertyChanged(nameof(AverageMonsterMaxHP));
            OnPropertyChanged(nameof(AverageMonsterAC));
            OnPropertyChanged(nameof(AverageMonsterSaveDC));
            OnPropertyChanged(nameof(AverageMonsterPassivePerception));
            OnPropertyChanged(nameof(AverageMonsterDamageTurn));
            OnPropertyChanged(nameof(OverallMonsterDamageVulnerabilities));
            OnPropertyChanged(nameof(OverallMonsterDamageResistances));
            OnPropertyChanged(nameof(OverallMonsterDamageImmunities));
            OnPropertyChanged(nameof(OverallMonsterConditionImmunities));
         }
      }

      private void ShowRandomEncounterWindow()
      {
         List<EncounterCharacterModel> characters = new List<EncounterCharacterModel>();
         foreach (EncounterCharacterModel encounterCharacterModel in _encounterModel.Creatures.Where(x => x is EncounterCharacterModel))
         {
            characters.Add(encounterCharacterModel);
         }

         IEnumerable<EncounterMonsterModel> monsters = _dialogService.ShowCreateRandomEncounterDialog(characters);
         if (monsters != null)
         {
            _encounterModel.Creatures.RemoveAll(x => !(x is EncounterCharacterModel));

            foreach (EncounterMonsterModel encounterCreatureModel in monsters)
            {
               EncounterMonsterModel encounterMonsterModel = new EncounterMonsterModel(encounterCreatureModel);
               encounterMonsterModel.CurrentHP = encounterMonsterModel.MaxHP;
               _encounterModel.Creatures.Add(encounterMonsterModel);
            }

            CreateCreatureViewModels();
            EstimateEncounterChallenge();

            OnPropertyChanged(nameof(Monsters));
            OnPropertyChanged(nameof(TotalMonsterXP));
            OnPropertyChanged(nameof(AverageMonsterCR));
            OnPropertyChanged(nameof(AverageMonsterMaxHP));
            OnPropertyChanged(nameof(AverageMonsterAC));
            OnPropertyChanged(nameof(AverageMonsterSaveDC));
            OnPropertyChanged(nameof(AverageMonsterPassivePerception));
            OnPropertyChanged(nameof(AverageMonsterDamageTurn));
            OnPropertyChanged(nameof(OverallMonsterDamageVulnerabilities));
            OnPropertyChanged(nameof(OverallMonsterDamageResistances));
            OnPropertyChanged(nameof(OverallMonsterDamageImmunities));
            OnPropertyChanged(nameof(OverallMonsterConditionImmunities));
         }
      }

      private void ViewNotes()
      {
         _dialogService.ShowConfirmationDialog($"{_encounterModel.Name} - Notes",
             String.IsNullOrWhiteSpace(_encounterModel.Notes) ? "No Notes Found" : _encounterModel.Notes, "OK", null, null);
      }

      private void EstimateEncounterChallenge()
      {
         _encounterModel.EncounterChallenge = _statService.EstimateEncounterChallenge(_encounterModel.Creatures.Where(x => x is EncounterCharacterModel),
                                                                                      _encounterModel.Creatures.Where(x => x is EncounterMonsterModel));
         OnPropertyChanged(nameof(EstimatedChallenge));
      }

      #endregion
   }
}
