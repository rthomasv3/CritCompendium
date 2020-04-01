using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendium.ViewModels.DialogViewModels;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class CompanionViewModel : NotifyPropertyChanged, IConfirmation
    {
        #region Events
        
        public event EventHandler AcceptSelected;
        public event EventHandler RejectSelected;
        public event EventHandler CancelSelected;

        #endregion

        #region Fields

        private readonly Compendium _compendium = DependencyResolver.Resolve<Compendium>();
        private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();
        private readonly DiceService _diceService = DependencyResolver.Resolve<DiceService>();
        private readonly CompanionModel _companionModel;
        private readonly List<KeyValuePair<MonsterModel, string>> _monsterOptions = new List<KeyValuePair<MonsterModel, string>>();
        private KeyValuePair<MonsterModel, string> _selectedMonsterOption;

        private readonly ICommand _showHPDialogCommand;
        private readonly ICommand _searchMonstersCommand;
        private readonly ICommand _viewMonsterCommand;
        private readonly ICommand _rollCompanionHPCommand;
        private readonly ICommand _acceptCommand;
        private readonly ICommand _rejectCommand;

        #endregion

        #region Constructor

        public CompanionViewModel(CompanionModel companionModel)
        {
            _companionModel = companionModel;

            _showHPDialogCommand = new RelayCommand(obj => true, obj => ShowHPDialog());
            _searchMonstersCommand = new RelayCommand(obj => true, obj => SearchMonsters());
            _viewMonsterCommand = new RelayCommand(obj => true, obj => ViewMonster());
            _rollCompanionHPCommand = new RelayCommand(obj => true, obj => RollCompanionHP());
            _acceptCommand = new RelayCommand(obj => true, obj => OnAccept());
            _rejectCommand = new RelayCommand(obj => true, obj => OnReject());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets id
        /// </summary>
        public Guid ID
        {
            get { return _companionModel.ID; }
        }

        /// <summary>
        /// Gets companion model
        /// </summary>
        public CompanionModel CompanionModel
        {
            get { return _companionModel; }
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
                _companionModel.MonsterModel = value.Key;
                _companionModel.Name = _companionModel.MonsterModel.Name;
                UpdateCompanionHP();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(MonsterHP));
            }
        }

        /// <summary>
        /// Gets or sets monster model
        /// </summary>
        public MonsterModel MonsterModel
        {
            get { return _companionModel.MonsterModel; }
            set { _companionModel.MonsterModel = value; }
        }

        /// <summary>
        /// Gets is monster set
        /// </summary>
        public bool IsMonsterSet
        {
            get { return _companionModel.MonsterModel != null; }
        }

        /// <summary>
        /// Gets monster hp
        /// </summary>
        public string MonsterHP
        {
            get { return _companionModel.MonsterModel != null ? _companionModel.MonsterModel.HP : "0"; }
        }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name
        {
            get { return _companionModel.Name; }
            set { _companionModel.Name = value; }
        }

        /// <summary>
        /// Gets or sets current hp
        /// </summary>
        public int CurrentHP
        {
            get { return _companionModel.CurrentHP; }
            set
            {
                _companionModel.CurrentHP = value;
                OnPropertyChanged(nameof(CurrentHP));
                OnPropertyChanged(nameof(LessHP));
                OnPropertyChanged(nameof(HPDisplay));
            }
        }

        /// <summary>
        /// Gets or sets max hp
        /// </summary>
        public int MaxHP
        {
            get { return _companionModel.MaxHP; }
            set { _companionModel.MaxHP = value; }
        }

        /// <summary>
        /// Gets less hp
        /// </summary>
        public int LessHP
        {
            get { return _companionModel.MaxHP - _companionModel.CurrentHP; }
        }

        /// <summary>
        /// Gets hp display
        /// </summary>
        public string HPDisplay
        {
            get { return $"{_companionModel.CurrentHP}/{_companionModel.MaxHP}"; }
        }

        /// <summary>
        /// Gets or sets notes
        /// </summary>
        public string Notes
        {
            get { return _companionModel.Notes; }
            set { _companionModel.Notes = value; }
        }

        /// <summary>
        /// Gets show hp dialog command
        /// </summary>
        public ICommand ShowHPDialogCommand
        {
            get { return _showHPDialogCommand; }
        }

        /// <summary>
        /// Gets view monster command
        /// </summary>
        public ICommand ViewMonsterCommand
        {
            get { return _viewMonsterCommand; }
        }

        /// <summary>
        /// Gets search monsters command
        /// </summary>
        public ICommand SearchMonstersCommand
        {
            get { return _searchMonstersCommand; }
        }

        /// <summary>
        /// Gets roll companion hp command
        /// </summary>
        public ICommand RollCompanionHPCommand
        {
            get { return _rollCompanionHPCommand; }
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
        /// Initializes monster options
        /// </summary>
        public void InitializeMonsterOptions()
        {
            _monsterOptions.Add(new KeyValuePair<MonsterModel, string>(null, "None"));
            foreach (MonsterModel monsterModel in _compendium.Monsters)
            {
                _monsterOptions.Add(new KeyValuePair<MonsterModel, string>(monsterModel, monsterModel.Name));
            }

            if (_companionModel != null && _companionModel.MonsterModel != null)
            {
                _selectedMonsterOption = _monsterOptions.FirstOrDefault(x => x.Key != null && x.Key.ID == _companionModel.MonsterModel.ID);
            }

            if (_selectedMonsterOption.Equals(default(KeyValuePair<MonsterModel, string>)))
            {
                _selectedMonsterOption = _monsterOptions[0];
            }
        }

        #endregion

        #region Private Methods

        private void UpdateCompanionHP()
        {
            int maxHP = 0;

            if (_companionModel.MonsterModel != null)
            {
                string hpString = _companionModel.MonsterModel.HP;
                if (!String.IsNullOrWhiteSpace(hpString))
                {
                    if (int.TryParse(hpString, out int hpParsed1))
                    {
                        maxHP = hpParsed1;
                    }
                    else
                    {
                        string[] hpStrings = hpString.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        if (hpStrings.Length > 0)
                        {
                            if (int.TryParse(hpStrings[0], out int hpParsed2))
                            {
                                maxHP = hpParsed2;
                            }
                        }
                    }
                }
            }

            _companionModel.MaxHP = maxHP;
            OnPropertyChanged(nameof(MaxHP));
        }

        private void ShowHPDialog()
        {
            int? result = _dialogService.ShowAddSubtractDialog(_companionModel.CurrentHP);
            if (result.HasValue)
            {
                _companionModel.CurrentHP = Math.Max(Math.Min(_companionModel.MaxHP, result.Value), 0);
                OnPropertyChanged(nameof(CurrentHP));
                OnPropertyChanged(nameof(LessHP));
                OnPropertyChanged(nameof(HPDisplay));
            }
        }

        private void SearchMonsters()
        {
            IEnumerable<MonsterModel> monsters = _dialogService.ShowMonsterSearchDialog(false);
            if (monsters.Any())
            {
                _companionModel.MonsterModel = monsters.First();

                KeyValuePair<MonsterModel, string> monsterPair = _monsterOptions.FirstOrDefault(x => x.Key != null && x.Key.ID == _companionModel.MonsterModel.ID);
                if (!monsterPair.Equals(default(KeyValuePair<MonsterModel, string>)))
                {
                    _selectedMonsterOption = monsterPair;
                    _companionModel.Name = _companionModel.MonsterModel.Name;
                    OnPropertyChanged(nameof(SelectedMonsterOption));
                    OnPropertyChanged(nameof(MonsterHP));
                    OnPropertyChanged(nameof(Name));
                    UpdateCompanionHP();
                }
            }
        }

        private void ViewMonster()
        {
            if (_companionModel.MonsterModel != null)
            {
                _dialogService.ShowDetailsDialog(new MonsterViewModel(_companionModel.MonsterModel));
            }
        }

        private void RollCompanionHP()
        {
            if (_companionModel.MonsterModel != null)
            {
                string hpString = _companionModel.MonsterModel.HP;
                if (!String.IsNullOrWhiteSpace(hpString))
                {
                    string[] hpStrings = hpString.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (hpStrings.Length > 1)
                    {
                        (double, string) rollResult = _diceService.EvaluateExpression(hpStrings[1]);
                        if (rollResult.Item1 != 0)
                        {
                            _companionModel.MaxHP = (int)rollResult.Item1;
                            OnPropertyChanged(nameof(MaxHP));
                        }
                    }
                }
            }
        }

        private void OnAccept()
        {
            if (String.IsNullOrWhiteSpace(_companionModel.Name))
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
