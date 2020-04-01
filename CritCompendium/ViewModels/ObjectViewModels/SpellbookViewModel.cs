using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class SpellbookViewModel : ObjectViewModel
   {
      #region Fields

      private readonly Compendium _compendium = DependencyResolver.Resolve<Compendium>();
      private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();
      private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
      private readonly StatService _statService = DependencyResolver.Resolve<StatService>();
      private readonly DocumentService _documentService = DependencyResolver.Resolve<DocumentService>();

      private readonly SpellbookModel _spellbookModel;
      private readonly List<KeyValuePair<ClassModel, string>> _classOptions = new List<KeyValuePair<ClassModel, string>>();
      private KeyValuePair<ClassModel, string> _selectedClassOption;
      private readonly List<KeyValuePair<RaceModel, string>> _raceOptions = new List<KeyValuePair<RaceModel, string>>();
      private KeyValuePair<RaceModel, string> _selectedRaceOption;
      private readonly List<KeyValuePair<Ability, string>> _abilityOptions = new List<KeyValuePair<Ability, string>>();
      private KeyValuePair<Ability, string> _selectedAbilityOption;
      private ObservableCollection<SpellbookEntryViewModel> _spells = new ObservableCollection<SpellbookEntryViewModel>();
      private int _baseSaveDC;
      private int _baseToHitBonus;
      private ObservableCollection<int> _spellSlots = new ObservableCollection<int>();
      private ObservableCollection<string> _spellSlotsDisplay = new ObservableCollection<string>();

      private ObservableCollection<SpellsByLevelViewModel> _spellsByLevel = new ObservableCollection<SpellsByLevelViewModel>();

      private readonly ICommand _rollToHitCommand;
      private readonly ICommand _decreaseSpellSlotCommand;
      private readonly ICommand _increaseSpellSlotCommand;
      private readonly ICommand _showAddSpellDialogCommand;
      private readonly ICommand _removeSpellCommand;
      private readonly ICommand _exportCommand;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="SpellbookViewModel"/>
      /// </summary>
      public SpellbookViewModel(SpellbookModel spellbookModel) : base()
      {
         _spellbookModel = spellbookModel;

         foreach (SpellbookEntryModel spellbookEntryModel in _spellbookModel.Spells.OrderBy(x => x.Spell.Name))
         {
            _spells.Add(new SpellbookEntryViewModel(spellbookEntryModel));
         }

         _rollToHitCommand = new RelayCommand(obj => true, obj => RollToHit());
         _decreaseSpellSlotCommand = new RelayCommand(obj => true, obj => DecreaseSpellSlots((int)obj));
         _increaseSpellSlotCommand = new RelayCommand(obj => true, obj => IncreaseSpellSlots((int)obj));
         _showAddSpellDialogCommand = new RelayCommand(obj => true, obj => ShowAddSpellDialog());
         _removeSpellCommand = new RelayCommand(obj => true, obj => RemoveSpell((SpellbookEntryViewModel)obj));
         _exportCommand = new RelayCommand(obj => true, obj => Export());
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets id
      /// </summary>
      public Guid ID
      {
         get { return _spellbookModel.ID; }
      }

      /// <summary>
      /// Gets spellbook model
      /// </summary>
      public SpellbookModel SpellbookModel
      {
         get { return _spellbookModel; }
      }

      /// <summary>
      /// Gets class options
      /// </summary>
      public List<KeyValuePair<ClassModel, string>> ClassOptions
      {
         get { return _classOptions; }
      }

      /// <summary>
      /// Gets race options
      /// </summary>
      public List<KeyValuePair<RaceModel, string>> RaceOptions
      {
         get { return _raceOptions; }
      }

      /// <summary>
      /// Gets ability options
      /// </summary>
      public List<KeyValuePair<Ability, string>> AbilityOptions
      {
         get { return _abilityOptions; }
      }

      /// <summary>
      /// Gets or sets selected class option
      /// </summary>
      public KeyValuePair<ClassModel, string> SelectedClassOption
      {
         get { return _selectedClassOption; }
         set
         {
            _selectedClassOption = value;
            _spellbookModel.Class = value.Key;
            _spellbookModel.Race = null;

            if (_spellbookModel.Class != null)
            {
               KeyValuePair<Ability, string> pair = _abilityOptions.FirstOrDefault(x => x.Key == _spellbookModel.Class.SpellAbility);
               if (!pair.Equals(default(KeyValuePair<Ability, string>)))
               {
                  _selectedAbilityOption = pair;
                  _spellbookModel.Ability = pair.Key;
                  OnPropertyChanged(nameof(Ability));
                  OnPropertyChanged(nameof(SelectedAbilityOption));
               }
            }

            OnPropertyChanged(nameof(Class));
            OnPropertyChanged(nameof(Race));
         }
      }

      /// <summary>
      /// Gets or sets selected race option
      /// </summary>
      public KeyValuePair<RaceModel, string> SelectedRaceOption
      {
         get { return _selectedRaceOption; }
         set
         {
            _selectedRaceOption = value;
            _spellbookModel.Race = value.Key;
            _spellbookModel.Class = null;
            OnPropertyChanged(nameof(Race));
            OnPropertyChanged(nameof(Class));
         }
      }

      /// <summary>
      /// Gets or sets selected ability option
      /// </summary>
      public KeyValuePair<Ability, string> SelectedAbilityOption
      {
         get { return _selectedAbilityOption; }
         set
         {
            _selectedAbilityOption = value;
            _spellbookModel.Ability = value.Key;
            OnPropertyChanged(nameof(Ability));
         }
      }

      /// <summary>
      /// Gets or sets name
      /// </summary>
      public string Name
      {
         get { return _spellbookModel.Name; }
         set { _spellbookModel.Name = value; }
      }

      /// <summary>
      /// Gets or sets based on class
      /// </summary>
      public bool BasedOnClass
      {
         get { return _spellbookModel.BasedOnClass; }
         set
         {
            _spellbookModel.BasedOnClass = value;
            OnPropertyChanged(nameof(BasedOnClass));
         }
      }

      /// <summary>
      /// Gets or sets based on race
      /// </summary>
      public bool BasedOnRace
      {
         get { return _spellbookModel.BasedOnRace; }
         set
         {
            _spellbookModel.BasedOnRace = value;
            OnPropertyChanged(nameof(BasedOnRace));
         }
      }

      /// <summary>
      /// Gets class
      /// </summary>
      public ClassModel Class
      {
         get { return _spellbookModel.Class; }
      }

      /// <summary>
      /// Gets race
      /// </summary>
      public RaceModel Race
      {
         get { return _spellbookModel.Race; }
      }

      /// <summary>
      /// Gets or sets ability
      /// </summary>
      public Ability Ability
      {
         get { return _spellbookModel.Ability; }
         set { _spellbookModel.Ability = value; }
      }

      /// <summary>
      /// Gets ability display
      /// </summary>
      public string AbilityDisplay
      {
         get { return _spellbookModel.Ability == Ability.None ? "None" : _stringService.GetString(_spellbookModel.Ability); }
      }

      /// <summary>
      /// Gets or sets additional dc bonus
      /// </summary>
      public int AdditionalDCBonus
      {
         get { return _spellbookModel.AdditionalDCBonus; }
         set { _spellbookModel.AdditionalDCBonus = value; }
      }

      /// <summary>
      /// Gets or sets additional to hit bonus
      /// </summary>
      public int AdditionalToHitBonus
      {
         get { return _spellbookModel.AdditionalToHitBonus; }
         set { _spellbookModel.AdditionalToHitBonus = value; }
      }

      /// <summary>
      /// Gets or sets reset on short rest
      /// </summary>
      public bool ResetOnShortRest
      {
         get { return _spellbookModel.ResetOnShortRest; }
         set { _spellbookModel.ResetOnShortRest = value; }
      }

      /// <summary>
      /// Gets or sets reset on long rest
      /// </summary>
      public bool ResetOnLongRest
      {
         get { return _spellbookModel.ResetOnLongRest; }
         set { _spellbookModel.ResetOnLongRest = value; }
      }

      /// <summary>
      /// Gets or sets base save dc
      /// </summary>
      public int BaseSaveDC
      {
         get { return _baseSaveDC; }
         set { _baseSaveDC = value; }
      }

      /// <summary>
      /// Gets save dc
      /// </summary>
      public int SaveDC
      {
         get { return _baseSaveDC + _spellbookModel.AdditionalDCBonus; }
      }

      /// <summary>
      /// Gets or sets base to hit bonus
      /// </summary>
      public int BaseToHitBonus
      {
         get { return _baseToHitBonus; }
         set { _baseToHitBonus = value; }
      }

      /// <summary>
      /// Gets to hit bonus
      /// </summary>
      public int ToHitBonus
      {
         get { return _baseToHitBonus + _spellbookModel.AdditionalToHitBonus; }
      }

      /// <summary>
      /// Gets to hit bonus display
      /// </summary>
      public string ToHitBonusDisplay
      {
         get { return _statService.AddPlusOrMinus(_baseToHitBonus + _spellbookModel.AdditionalToHitBonus); }
      }

      /// <summary>
      /// Gets or sets spell slots
      /// </summary>
      public IEnumerable<int> SpellSlots
      {
         get { return _spellSlots; }
         set
         {
            _spellSlots = new ObservableCollection<int>(value);
            UpdateSpellSlotsDisplay();
         }
      }

      /// <summary>
      /// Gets or sets spell slots display
      /// </summary>
      public IEnumerable<string> SpellSlotsDisplay
      {
         get { return _spellSlotsDisplay; }
      }

      /// <summary>
      /// Gets max cantrips
      /// </summary>
      public int MaxCantrips
      {
         get { return _spellSlots.ElementAtOrDefault(0); }
      }

      /// <summary>
      /// Gets current first level spell slots
      /// </summary>
      public int CurrentFirstLevelSpellSlots
      {
         get { return _spellbookModel.CurrentFirstLevelSpellSlots; }
      }

      /// <summary>
      /// Gets max first level spell slots
      /// </summary>
      public int MaxFirstLevelSpellSlots
      {
         get { return _spellSlots.ElementAtOrDefault(1); }
      }

      /// <summary>
      /// Gets current second level spell slots
      /// </summary>
      public int CurrentSecondLevelSpellSlots
      {
         get { return _spellbookModel.CurrentSecondLevelSpellSlots; }
      }

      /// <summary>
      /// Gets max second level spell slots
      /// </summary>
      public int MaxSecondLevelSpellSlots
      {
         get { return _spellSlots.ElementAtOrDefault(2); }
      }

      /// <summary>
      /// Gets current thrid level spell slots
      /// </summary>
      public int CurrentThirdLevelSpellSlots
      {
         get { return _spellbookModel.CurrentThirdLevelSpellSlots; }
      }

      /// <summary>
      /// Gets max third level spell slots
      /// </summary>
      public int MaxThirdLevelSpellSlots
      {
         get { return _spellSlots.ElementAtOrDefault(3); }
      }

      /// <summary>
      /// Gets current fourth level spell slots
      /// </summary>
      public int CurrentFourthLevelSpellSlots
      {
         get { return _spellbookModel.CurrentFourthLevelSpellSlots; }
      }

      /// <summary>
      /// Gets max fourth level spell slots
      /// </summary>
      public int MaxFourthLevelSpellSlots
      {
         get { return _spellSlots.ElementAtOrDefault(4); }
      }

      /// <summary>
      /// Gets current fifth level spell slots
      /// </summary>
      public int CurrentFifthLevelSpellSlots
      {
         get { return _spellbookModel.CurrentFifthLevelSpellSlots; }
      }

      /// <summary>
      /// Gets max fifth level spell slots
      /// </summary>
      public int MaxFifthLevelSpellSlots
      {
         get { return _spellSlots.ElementAtOrDefault(5); }
      }

      /// <summary>
      /// Gets current sixth level spell slots
      /// </summary>
      public int CurrentSixthLevelSpellSlots
      {
         get { return _spellbookModel.CurrentSixthLevelSpellSlots; }
      }

      /// <summary>
      /// Gets max sixth level spell slots
      /// </summary>
      public int MaxSixthLevelSpellSlots
      {
         get { return _spellSlots.ElementAtOrDefault(6); }
      }

      /// <summary>
      /// Gets current seventh level spell slots
      /// </summary>
      public int CurrentSeventhLevelSpellSlots
      {
         get { return _spellbookModel.CurrentSeventhLevelSpellSlots; }
      }

      /// <summary>
      /// Gets max seventh level spell slots
      /// </summary>
      public int MaxSeventhLevelSpellSlots
      {
         get { return _spellSlots.ElementAtOrDefault(7); }
      }

      /// <summary>
      /// Gets current eighth level spell slots
      /// </summary>
      public int CurrentEighthLevelSpellSlots
      {
         get { return _spellbookModel.CurrentEighthLevelSpellSlots; }
      }

      /// <summary>
      /// Gets max eighth level spell slots
      /// </summary>
      public int MaxEighthLevelSpellSlots
      {
         get { return _spellSlots.ElementAtOrDefault(8); }
      }

      /// <summary>
      /// Gets current ninth level spell slots
      /// </summary>
      public int CurrentNinthLevelSpellSlots
      {
         get { return _spellbookModel.CurrentNinthLevelSpellSlots; }
      }

      /// <summary>
      /// Gets max ninth level spell slots
      /// </summary>
      public int MaxNinthLevelSpellSlots
      {
         get { return _spellSlots.ElementAtOrDefault(9); }
      }

      /// <summary>
      /// Gets spells
      /// </summary>
      public IEnumerable<SpellbookEntryViewModel> Spells
      {
         get { return _spells; }
      }

      /// <summary>
      /// Gets spells by level
      /// </summary>
      public IEnumerable<SpellsByLevelViewModel> SpellsByLevel
      {
         get { return _spellsByLevel; }
      }

      /// <summary>
      /// Gets cantrips
      /// </summary>
      public IEnumerable<SpellbookEntryViewModel> Cantrips
      {
         get { return _spells.Where(x => x.Spell != null && x.Spell.Level == 0).OrderBy(x => x.SpellName); }
      }

      /// <summary>
      /// Gets first level spells
      /// </summary>
      public IEnumerable<SpellbookEntryViewModel> FirstLevelSpells
      {
         get { return _spells.Where(x => x.Spell != null && x.Spell.Level == 1).OrderBy(x => x.SpellName); }
      }

      /// <summary>
      /// Gets second level spells
      /// </summary>
      public IEnumerable<SpellbookEntryViewModel> SecondLevelSpells
      {
         get { return _spells.Where(x => x.Spell != null && x.Spell.Level == 2).OrderBy(x => x.SpellName); }
      }

      /// <summary>
      /// Gets thrid level spells
      /// </summary>
      public IEnumerable<SpellbookEntryViewModel> ThirdLevelSpells
      {
         get { return _spells.Where(x => x.Spell != null && x.Spell.Level == 3).OrderBy(x => x.SpellName); }
      }

      /// <summary>
      /// Gets fourth level spells
      /// </summary>
      public IEnumerable<SpellbookEntryViewModel> FourthLevelSpells
      {
         get { return _spells.Where(x => x.Spell != null && x.Spell.Level == 4).OrderBy(x => x.SpellName); }
      }

      /// <summary>
      /// Gets fifth level spells
      /// </summary>
      public IEnumerable<SpellbookEntryViewModel> FifthLevelSpells
      {
         get { return _spells.Where(x => x.Spell != null && x.Spell.Level == 5).OrderBy(x => x.SpellName); }
      }

      /// <summary>
      /// Gets sixth level spells
      /// </summary>
      public IEnumerable<SpellbookEntryViewModel> SixthLevelSpells
      {
         get { return _spells.Where(x => x.Spell != null && x.Spell.Level == 6).OrderBy(x => x.SpellName); }
      }

      /// <summary>
      /// Gets seventh level spells
      /// </summary>
      public IEnumerable<SpellbookEntryViewModel> SeventhLevelSpells
      {
         get { return _spells.Where(x => x.Spell != null && x.Spell.Level == 7).OrderBy(x => x.SpellName); }
      }

      /// <summary>
      /// Gets eighth level spells
      /// </summary>
      public IEnumerable<SpellbookEntryViewModel> EighthLevelSpells
      {
         get { return _spells.Where(x => x.Spell != null && x.Spell.Level == 8).OrderBy(x => x.SpellName); }
      }

      /// <summary>
      /// Gets ninth level spells
      /// </summary>
      public IEnumerable<SpellbookEntryViewModel> NinthLevelSpells
      {
         get { return _spells.Where(x => x.Spell != null && x.Spell.Level == 9).OrderBy(x => x.SpellName); }
      }

      /// <summary>
      /// Gets roll to hit command
      /// </summary>
      public ICommand RollToHitCommand
      {
         get { return _rollToHitCommand; }
      }

      /// <summary>
      /// Gets decrease spell slot command
      /// </summary>
      public ICommand DecreaseSpellSlotCommand
      {
         get { return _decreaseSpellSlotCommand; }
      }

      /// <summary>
      /// Gets increase spell slot command
      /// </summary>
      public ICommand IncreaseSpellSlotCommand
      {
         get { return _increaseSpellSlotCommand; }
      }

      /// <summary>
      /// Gets show add spell dialog command
      /// </summary>
      public ICommand ShowAddSpellDialogCommand
      {
         get { return _showAddSpellDialogCommand; }
      }

      /// <summary>
      /// Gets remove spell command
      /// </summary>
      public ICommand RemoveSpellCommand
      {
         get { return _removeSpellCommand; }
      }

      /// <summary>
      /// Gets export command
      /// </summary>
      public ICommand ExportCommand
      {
         get { return _exportCommand; }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Initializes creation options
      /// </summary>
      public void InitializeOptions()
      {
         foreach (ClassModel classModel in _compendium.Classes)
         {
            _classOptions.Add(new KeyValuePair<ClassModel, string>(classModel, classModel.Name));
         }

         if (_spellbookModel.Class != null)
         {
            _selectedClassOption = _classOptions.FirstOrDefault(x => x.Key != null && x.Key.ID == _spellbookModel.Class.ID);
         }

         if (_selectedClassOption.Equals(default(KeyValuePair<ClassModel, string>)) && _classOptions.Any())
         {
            _selectedClassOption = _classOptions[0];
            _spellbookModel.Class = _selectedClassOption.Key;

            KeyValuePair<Ability, string> pair = _abilityOptions.FirstOrDefault(x => x.Key == _spellbookModel.Class.SpellAbility);
            if (!pair.Equals(default(KeyValuePair<Ability, string>)))
            {
               _selectedAbilityOption = pair;
               _spellbookModel.Ability = pair.Key;
               OnPropertyChanged(nameof(Ability));
               OnPropertyChanged(nameof(SelectedAbilityOption));
            }
         }

         foreach (RaceModel raceModel in _compendium.Races)
         {
            _raceOptions.Add(new KeyValuePair<RaceModel, string>(raceModel, raceModel.Name));
         }

         if (_spellbookModel.Race != null)
         {
            _selectedRaceOption = _raceOptions.FirstOrDefault(x => x.Key != null && x.Key.ID == _spellbookModel.Race.ID);
         }

         if (_selectedRaceOption.Equals(default(KeyValuePair<RaceModel, string>)) && _raceOptions.Any())
         {
            _selectedRaceOption = _raceOptions[0];
         }

         _abilityOptions.Add(new KeyValuePair<Ability, string>(Ability.None, "None"));
         foreach (Ability ability in Enum.GetValues(typeof(Ability)))
         {
            if (ability != Ability.None)
            {
               _abilityOptions.Add(new KeyValuePair<Ability, string>(ability, _stringService.GetString(ability)));
            }
         }

         _selectedAbilityOption = _abilityOptions.FirstOrDefault(x => x.Key == _spellbookModel.Ability);
      }

      /// <summary>
      /// Initializes spells by level
      /// </summary>
      public void InitializeSpellsByLevel()
      {
         if (_spellSlots.Count > 0 && _spellSlots.Sum() > 0)
         {
            int maxLevel = 0;

            for (int i = _spellSlots.Count - 1; i > -1; --i)
            {
               if (_spellSlots[i] > 0)
               {
                  maxLevel = i;
                  break;
               }
            }

            for (int i = 0; i <= maxLevel; ++i)
            {
               if (_spellSlots.Count > i)
               {
                  _spellsByLevel.Add(new SpellsByLevelViewModel(i, _spellSlots[i], _spellbookModel.BasedOnClass, _spellbookModel.BasedOnRace));
               }
               else
               {
                  _spellsByLevel.Add(new SpellsByLevelViewModel(i, 0, _spellbookModel.BasedOnClass, _spellbookModel.BasedOnRace));
               }
            }
         }

         foreach (SpellbookEntryModel spellbookEntryModel in _spellbookModel.Spells.OrderBy(x => x.Spell.Level))
         {
            int level = Math.Max(spellbookEntryModel.Spell.Level, 0);

            SpellsByLevelViewModel spellsByLevelViewModel = _spellsByLevel.FirstOrDefault(x => x.Level == level);

            if (spellsByLevelViewModel != null)
            {
               spellsByLevelViewModel.AddSpell(spellbookEntryModel);
            }
            else
            {
               if (_spellSlots.Count > level)
               {
                  spellsByLevelViewModel = new SpellsByLevelViewModel(level, _spellSlots[level], _spellbookModel.BasedOnClass, _spellbookModel.BasedOnRace);
                  spellsByLevelViewModel.AddSpell(spellbookEntryModel);
                  _spellsByLevel.Add(spellsByLevelViewModel);
               }
               else
               {
                  spellsByLevelViewModel = new SpellsByLevelViewModel(level, 0, _spellbookModel.BasedOnClass, _spellbookModel.BasedOnRace);
                  spellsByLevelViewModel.AddSpell(spellbookEntryModel);
                  _spellsByLevel.Add(spellsByLevelViewModel);
               }
            }
         }
      }

      /// <summary>
      /// Resets spell slots
      /// </summary>
      public void ResetSpellSlotsAndUses()
      {
         if (_spellbookModel.BasedOnClass)
         {
            _spellbookModel.CurrentFirstLevelSpellSlots = _spellSlots[1];
            _spellbookModel.CurrentSecondLevelSpellSlots = _spellSlots[2];
            _spellbookModel.CurrentThirdLevelSpellSlots = _spellSlots[3];
            _spellbookModel.CurrentFourthLevelSpellSlots = _spellSlots[4];
            _spellbookModel.CurrentFifthLevelSpellSlots = _spellSlots[5];
            _spellbookModel.CurrentSixthLevelSpellSlots = _spellSlots[6];
            _spellbookModel.CurrentSeventhLevelSpellSlots = _spellSlots[7];
            _spellbookModel.CurrentEighthLevelSpellSlots = _spellSlots[8];
            _spellbookModel.CurrentNinthLevelSpellSlots = _spellSlots[9];

            UpdateSpellSlotsDisplay();
         }
         else if (_spellbookModel.BasedOnRace)
         {
            foreach (SpellsByLevelViewModel spellsByLevelViewModel in _spellsByLevel)
            {
               foreach (SpellbookEntryViewModel spellbookEntryViewModel in spellsByLevelViewModel.Spells)
               {
                  spellbookEntryViewModel.Used = false;
               }
            }
         }
      }

      #endregion

      #region Private Methods

      private void RollToHit()
      {
         _dialogService.ShowDiceRollDialog("Spell Attack", "1d20" + _statService.AddPlusOrMinus(_baseToHitBonus + _spellbookModel.AdditionalToHitBonus));
      }

      private void DecreaseSpellSlots(int level)
      {
         switch (level)
         {
            case 0:
               _spellbookModel.CurrentFirstLevelSpellSlots = Math.Max(_spellbookModel.CurrentFirstLevelSpellSlots - 1, 0);
               _spellSlotsDisplay[0] = $"{_spellbookModel.CurrentFirstLevelSpellSlots}/{_spellSlots[1]}";
               break;
            case 1:
               _spellbookModel.CurrentSecondLevelSpellSlots = Math.Max(_spellbookModel.CurrentSecondLevelSpellSlots - 1, 0);
               _spellSlotsDisplay[1] = $"{_spellbookModel.CurrentSecondLevelSpellSlots}/{_spellSlots[2]}";
               break;
            case 2:
               _spellbookModel.CurrentThirdLevelSpellSlots = Math.Max(_spellbookModel.CurrentThirdLevelSpellSlots - 1, 0);
               _spellSlotsDisplay[2] = $"{_spellbookModel.CurrentThirdLevelSpellSlots}/{_spellSlots[3]}";
               break;
            case 3:
               _spellbookModel.CurrentFourthLevelSpellSlots = Math.Max(_spellbookModel.CurrentFourthLevelSpellSlots - 1, 0);
               _spellSlotsDisplay[3] = $"{_spellbookModel.CurrentFourthLevelSpellSlots}/{_spellSlots[4]}";
               break;
            case 4:
               _spellbookModel.CurrentFifthLevelSpellSlots = Math.Max(_spellbookModel.CurrentFifthLevelSpellSlots - 1, 0);
               _spellSlotsDisplay[4] = $"{_spellbookModel.CurrentFifthLevelSpellSlots}/{_spellSlots[5]}";
               break;
            case 5:
               _spellbookModel.CurrentSixthLevelSpellSlots = Math.Max(_spellbookModel.CurrentSixthLevelSpellSlots - 1, 0);
               _spellSlotsDisplay[5] = $"{_spellbookModel.CurrentSixthLevelSpellSlots}/{_spellSlots[6]}";
               break;
            case 6:
               _spellbookModel.CurrentSeventhLevelSpellSlots = Math.Max(_spellbookModel.CurrentSeventhLevelSpellSlots - 1, 0);
               _spellSlotsDisplay[6] = $"{_spellbookModel.CurrentSeventhLevelSpellSlots}/{_spellSlots[7]}";
               break;
            case 7:
               _spellbookModel.CurrentEighthLevelSpellSlots = Math.Max(_spellbookModel.CurrentEighthLevelSpellSlots - 1, 0);
               _spellSlotsDisplay[7] = $"{_spellbookModel.CurrentEighthLevelSpellSlots}/{_spellSlots[8]}";
               break;
            case 8:
               _spellbookModel.CurrentNinthLevelSpellSlots = Math.Max(_spellbookModel.CurrentNinthLevelSpellSlots - 1, 0);
               _spellSlotsDisplay[8] = $"{_spellbookModel.CurrentNinthLevelSpellSlots}/{_spellSlots[9]}";
               break;
         }
      }

      private void IncreaseSpellSlots(int level)
      {
         switch (level)
         {
            case 0:
               _spellbookModel.CurrentFirstLevelSpellSlots = Math.Min(_spellbookModel.CurrentFirstLevelSpellSlots + 1, _spellSlots[1]);
               _spellSlotsDisplay[0] = $"{_spellbookModel.CurrentFirstLevelSpellSlots}/{_spellSlots[1]}";
               break;
            case 1:
               _spellbookModel.CurrentSecondLevelSpellSlots = Math.Min(_spellbookModel.CurrentSecondLevelSpellSlots + 1, _spellSlots[2]);
               _spellSlotsDisplay[1] = $"{_spellbookModel.CurrentSecondLevelSpellSlots}/{_spellSlots[2]}";
               break;
            case 2:
               _spellbookModel.CurrentThirdLevelSpellSlots = Math.Min(_spellbookModel.CurrentThirdLevelSpellSlots + 1, _spellSlots[3]);
               _spellSlotsDisplay[2] = $"{_spellbookModel.CurrentThirdLevelSpellSlots}/{_spellSlots[3]}";
               break;
            case 3:
               _spellbookModel.CurrentFourthLevelSpellSlots = Math.Min(_spellbookModel.CurrentFourthLevelSpellSlots + 1, _spellSlots[4]);
               _spellSlotsDisplay[3] = $"{_spellbookModel.CurrentFourthLevelSpellSlots}/{_spellSlots[4]}";
               break;
            case 4:
               _spellbookModel.CurrentFifthLevelSpellSlots = Math.Min(_spellbookModel.CurrentFifthLevelSpellSlots + 1, _spellSlots[5]);
               _spellSlotsDisplay[4] = $"{_spellbookModel.CurrentFifthLevelSpellSlots}/{_spellSlots[5]}";
               break;
            case 5:
               _spellbookModel.CurrentSixthLevelSpellSlots = Math.Min(_spellbookModel.CurrentSixthLevelSpellSlots + 1, _spellSlots[6]);
               _spellSlotsDisplay[5] = $"{_spellbookModel.CurrentSixthLevelSpellSlots}/{_spellSlots[6]}";
               break;
            case 6:
               _spellbookModel.CurrentSeventhLevelSpellSlots = Math.Min(_spellbookModel.CurrentSeventhLevelSpellSlots + 1, _spellSlots[7]);
               _spellSlotsDisplay[6] = $"{_spellbookModel.CurrentSeventhLevelSpellSlots}/{_spellSlots[7]}";
               break;
            case 7:
               _spellbookModel.CurrentEighthLevelSpellSlots = Math.Min(_spellbookModel.CurrentEighthLevelSpellSlots + 1, _spellSlots[8]);
               _spellSlotsDisplay[7] = $"{_spellbookModel.CurrentEighthLevelSpellSlots}/{_spellSlots[8]}";
               break;
            case 8:
               _spellbookModel.CurrentNinthLevelSpellSlots = Math.Min(_spellbookModel.CurrentNinthLevelSpellSlots + 1, _spellSlots[9]);
               _spellSlotsDisplay[8] = $"{_spellbookModel.CurrentNinthLevelSpellSlots}/{_spellSlots[9]}";
               break;
         }
      }

      private void ShowAddSpellDialog()
      {
         IEnumerable<SpellModel> spells = _dialogService.ShowSpellSearchDialog(true);

         if (spells != null && spells.Any())
         {
            foreach (SpellModel spell in spells)
            {
               AddSpell(spell);
            }

            OnPropertyChanged(nameof(Spells));
            OnPropertyChanged(nameof(SpellsByLevel));
            OnPropertyChanged(nameof(Cantrips));
            OnPropertyChanged(nameof(FirstLevelSpells));
            OnPropertyChanged(nameof(SecondLevelSpells));
            OnPropertyChanged(nameof(ThirdLevelSpells));
            OnPropertyChanged(nameof(FourthLevelSpells));
            OnPropertyChanged(nameof(FifthLevelSpells));
            OnPropertyChanged(nameof(SixthLevelSpells));
            OnPropertyChanged(nameof(SeventhLevelSpells));
            OnPropertyChanged(nameof(EighthLevelSpells));
            OnPropertyChanged(nameof(NinthLevelSpells));
         }
      }

      private void AddSpell(SpellModel spellModel)
      {
         if (!_spells.Any(x => x.Spell.ID == spellModel.ID))
         {
            SpellbookEntryModel spellbookEntryModel = new SpellbookEntryModel();
            spellbookEntryModel.Spell = spellModel;

            int level = Math.Max(spellModel.Level, 0);

            SpellsByLevelViewModel spellsByLevelViewModel = _spellsByLevel.FirstOrDefault(x => x.Level == level);

            if (spellsByLevelViewModel != null)
            {
               spellsByLevelViewModel.AddSpell(spellbookEntryModel);
            }
            else
            {
               spellsByLevelViewModel = new SpellsByLevelViewModel(level, _spellSlots.Count > level ? _spellSlots[level] : 0, _spellbookModel.BasedOnClass, _spellbookModel.BasedOnRace);
               spellsByLevelViewModel.AddSpell(spellbookEntryModel);

               bool added = false;
               if (_spellsByLevel.Count > 0)
               {
                  for (int i = 0; i < _spellsByLevel.Count; ++i)
                  {
                     if (_spellsByLevel[i].Level > level)
                     {
                        _spellsByLevel.Insert(i, spellsByLevelViewModel);
                        added = true;
                        break;
                     }
                  }
               }

               if (!added)
               {
                  _spellsByLevel.Add(spellsByLevelViewModel);
               }
            }

            _spells.Add(new SpellbookEntryViewModel(spellbookEntryModel));
            _spellbookModel.Spells.Add(spellbookEntryModel);
         }
      }

      private void RemoveSpell(SpellbookEntryViewModel spellbookEntryViewModel)
      {
         bool? result = _dialogService.ShowConfirmationDialog("Remove Spell", "Are you sure you want to remove " + spellbookEntryViewModel.SpellName + "?", "Yes", "No", null);
         if (result == true)
         {
            int level = Math.Max(spellbookEntryViewModel.Spell.Level, 0);

            SpellsByLevelViewModel spellsByLevelViewModel = _spellsByLevel.FirstOrDefault(x => x.Level == level);
            if (spellsByLevelViewModel != null)
            {
               spellsByLevelViewModel.RemoveSpell(spellbookEntryViewModel.SpellbookEntryModel);
               if (!spellsByLevelViewModel.Spells.Any() && _spellSlots[level] == 0)
               {
                  _spellsByLevel.Remove(spellsByLevelViewModel);
               }
            }

            SpellbookEntryViewModel spell = _spells.FirstOrDefault(x => x.Spell.ID == spellbookEntryViewModel.Spell.ID);
            if (spell != null)
            {
               _spells.Remove(spell);
               _spellbookModel.Spells.Remove(spell.SpellbookEntryModel);
            }
         }
      }

      private void UpdateSpellSlotsDisplay()
      {
         _spellSlotsDisplay.Clear();

         _spellSlotsDisplay.Add($"{_spellbookModel.CurrentFirstLevelSpellSlots}/{_spellSlots[1]}");
         _spellSlotsDisplay.Add($"{_spellbookModel.CurrentSecondLevelSpellSlots}/{_spellSlots[2]}");
         _spellSlotsDisplay.Add($"{_spellbookModel.CurrentThirdLevelSpellSlots}/{_spellSlots[3]}");
         _spellSlotsDisplay.Add($"{_spellbookModel.CurrentFourthLevelSpellSlots}/{_spellSlots[4]}");
         _spellSlotsDisplay.Add($"{_spellbookModel.CurrentFifthLevelSpellSlots}/{_spellSlots[5]}");
         _spellSlotsDisplay.Add($"{_spellbookModel.CurrentSixthLevelSpellSlots}/{_spellSlots[6]}");
         _spellSlotsDisplay.Add($"{_spellbookModel.CurrentSeventhLevelSpellSlots}/{_spellSlots[7]}");
         _spellSlotsDisplay.Add($"{_spellbookModel.CurrentEighthLevelSpellSlots}/{_spellSlots[8]}");
         _spellSlotsDisplay.Add($"{_spellbookModel.CurrentNinthLevelSpellSlots}/{_spellSlots[9]}");
      }

      private void Export()
      {
         Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
         saveFileDialog.Filter = "Word Document|*.docx";
         saveFileDialog.Title = "Save Spellbook";
         saveFileDialog.FileName = _spellbookModel.Name;

         if (saveFileDialog.ShowDialog() == true)
         {
            try
            {
               string ext = System.IO.Path.GetExtension(saveFileDialog.FileName);

               if (ext == ".docx")
               {
                  _documentService.CreateWordDoc(saveFileDialog.FileName, this);
               }
               else
               {
                  _dialogService.ShowConfirmationDialog("Unable To Export", "Invalid file extension.", "OK", null, null);
               }
            }
            catch (Exception)
            {
               _dialogService.ShowConfirmationDialog("Unable To Export", "An error occurred when attempting to export the spellbook.", "OK", null, null);
            }
         }
      }

      #endregion

      #region Protected Methods

      protected override void OnAccept()
      {
         if (String.IsNullOrWhiteSpace(_spellbookModel.Name))
         {
            _dialogService.ShowConfirmationDialog("Required Field", "Name is required.", "OK", null, null);
         }
         else
         {
            Accept();
         }
      }

      #endregion
   }
}
