using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Services;
using CriticalCompendiumInfrastructure;
using CriticalCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public class EncounterCreatureViewModel : NotifyPropertyChanged
    {
        #region Fields

        private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();

        private readonly EncounterCreatureModel _encounterCreatureModel;
        private ObservableCollection<AppliedConditionViewModel> _conditions = new ObservableCollection<AppliedConditionViewModel>();
        private readonly ICommand _showHPDialogCommand;
        private readonly ICommand _showAddConditionDialogCommand;
        private readonly ICommand _showEditConditionDialogCommand;
        private readonly ICommand _viewConditionCommand;
        private readonly ICommand _removeConditionCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="EncounterCreatureModel"/>
        /// </summary>
        public EncounterCreatureViewModel(EncounterCreatureModel encounterCreatureModel)
        {
            _encounterCreatureModel = encounterCreatureModel;

            _conditions.Clear();
            foreach (AppliedConditionModel appliedConditionModel in _encounterCreatureModel.Conditions)
            {
                _conditions.Add(new AppliedConditionViewModel(appliedConditionModel));
            }

            _showHPDialogCommand = new RelayCommand(obj => true, obj => ShowHPDialog());
            _showAddConditionDialogCommand = new RelayCommand(obj => true, obj => ShowAddConditionDialog());
            _showEditConditionDialogCommand = new RelayCommand(obj => true, obj => ShowEditConditionDialog(obj as AppliedConditionViewModel));
            _viewConditionCommand = new RelayCommand(obj => true, obj => ViewConditon(obj as AppliedConditionViewModel));
            _removeConditionCommand = new RelayCommand(obj => true, obj => RemoveCondition(obj as AppliedConditionViewModel));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets encounter creature model
        /// </summary>
        public EncounterCreatureModel EncounterCreatureModel
        {
            get { return _encounterCreatureModel; }
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
        /// Gets or sets max hp
        /// </summary>
        public int MaxHP
        {
            get { return _encounterCreatureModel.MaxHP; }
            set { _encounterCreatureModel.MaxHP = value; }
        }

        /// <summary>
        /// Gets less hp
        /// </summary>
        public int LessHP
        {
            get { return _encounterCreatureModel.LessHP; }
        }

        /// <summary>
        /// Gets hp display
        /// </summary>
        public string HPDisplay
        {
            get { return $"{_encounterCreatureModel.CurrentHP}/{_encounterCreatureModel.MaxHP}"; }
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
        /// Gets or sets passive perception
        /// </summary>
        public int PassivePerception
        {
            get { return _encounterCreatureModel.PassivePerception; }
            set { _encounterCreatureModel.PassivePerception = value; }
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
            set
            {
                _encounterCreatureModel.Initiative = value;
                OnPropertyChanged(nameof(Initiative));
            }
        }

        /// <summary>
        /// Gets or sets selected
        /// </summary>
        public bool Selected
        {
            get { return _encounterCreatureModel.Selected; }
            set
            {
                _encounterCreatureModel.Selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        /// <summary>
        /// Gets conditions
        /// </summary>
        public IEnumerable<AppliedConditionViewModel> Conditions
        {
            get { return _conditions; }
        }

        /// <summary>
        /// Gets show hp dialog command
        /// </summary>
        public ICommand ShowHPDialogCommand
        {
            get { return _showHPDialogCommand; }
        }

        /// <summary>
        /// Gets show add condition dialog command
        /// </summary>
        public ICommand ShowAddConditionDialogCommand
        {
            get { return _showAddConditionDialogCommand; }
        }

        /// <summary>
        /// Gets show edit condition dialog command
        /// </summary>
        public ICommand ShowEditConditionDialogCommand
        {
            get { return _showEditConditionDialogCommand; }
        }

        /// <summary>
        /// Gets view condition command
        /// </summary>
        public ICommand ViewConditionCommand
        {
            get { return _viewConditionCommand; }
        }

        /// <summary>
        /// Gets remove condition command
        /// </summary>
        public ICommand RemoveConditionCommand
        {
            get { return _removeConditionCommand; }
        }

        #endregion

        #region Private Methods

        private void ShowHPDialog()
        {
            int? result = _dialogService.ShowAddSubtractDialog(_encounterCreatureModel.CurrentHP);
            if (result.HasValue)
            {
                _encounterCreatureModel.CurrentHP = Math.Max(Math.Min(_encounterCreatureModel.MaxHP, result.Value), 0);
                OnPropertyChanged(nameof(CurrentHP));
                OnPropertyChanged(nameof(LessHP));
                OnPropertyChanged(nameof(HPDisplay));
            }
        }

        private void ShowAddConditionDialog()
        {
            AppliedConditionModel appliedConditionModel = _dialogService.ShowCreateAppliedConditionDialog("Add Condition", new AppliedConditionModel());
            if (appliedConditionModel != null)
            {
                _conditions.Add(new AppliedConditionViewModel(appliedConditionModel));
                _encounterCreatureModel.Conditions.Add(appliedConditionModel);

                _conditions = new ObservableCollection<AppliedConditionViewModel>(_conditions.OrderBy(x => x.Name));

                OnPropertyChanged(nameof(Conditions));
            }
        }

        private void ShowEditConditionDialog(AppliedConditionViewModel appliedConditionView)
        {
            AppliedConditionModel appliedConditionModel = _dialogService.ShowCreateAppliedConditionDialog("Edit Condition", appliedConditionView.AppliedConditionModel);
            if (appliedConditionModel != null)
            {
                _conditions.Remove(appliedConditionView);
                _encounterCreatureModel.Conditions.Remove(appliedConditionView.AppliedConditionModel);

                _conditions.Add(new AppliedConditionViewModel(appliedConditionModel));
                _encounterCreatureModel.Conditions.Add(appliedConditionModel);

                _conditions = new ObservableCollection<AppliedConditionViewModel>(_conditions.OrderBy(x => x.Name));

                OnPropertyChanged(nameof(Conditions));
            }
        }

        private void ViewConditon(AppliedConditionViewModel appliedConditionView)
        {
            if (appliedConditionView != null)
            {
                _dialogService.ShowDetailsDialog(new ConditionViewModel(appliedConditionView.Condition));
            }
        }


        private void RemoveCondition(AppliedConditionViewModel appliedConditionView)
        {
            if (appliedConditionView != null)
            {
                _conditions.Remove(appliedConditionView);
                _encounterCreatureModel.Conditions.Remove(appliedConditionView.AppliedConditionModel);

                _conditions = new ObservableCollection<AppliedConditionViewModel>(_conditions.OrderBy(x => x.Name));

                OnPropertyChanged(nameof(Conditions));
            }
        }

        #endregion
    }
}
