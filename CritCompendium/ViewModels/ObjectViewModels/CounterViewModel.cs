using System;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendium.ViewModels.DialogViewModels;
using CriticalCompendiumInfrastructure;
using CriticalCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public class CounterViewModel : NotifyPropertyChanged, IConfirmation
    {
        #region Events
        
        public event EventHandler AcceptSelected;
        public event EventHandler RejectSelected;
        public event EventHandler CancelSelected;

        #endregion

        #region Fields

        private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();
        private readonly CounterModel _counterModel;
        private readonly ICommand _increaseCurrentValueCommand;
        private readonly ICommand _decreaseCurrentValueCommand;

        private readonly ICommand _acceptCommand;
        private readonly ICommand _rejectCommand;

        #endregion

        #region Constructor

        public CounterViewModel(CounterModel counterModel)
        {
            _counterModel = counterModel;

            _increaseCurrentValueCommand = new RelayCommand(obj => true, obj => IncreaseCurrentValue());
            _decreaseCurrentValueCommand = new RelayCommand(obj => true, obj => DecreaseCurrentValue());

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
            get { return _counterModel.ID; }
        }

        /// <summary>
        /// Gets counter model
        /// </summary>
        public CounterModel CounterModel
        {
            get { return _counterModel; }
        }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name
        {
            get { return _counterModel.Name; }
            set { _counterModel.Name = value; }
        }

        /// <summary>
        /// Gets or sets current value
        /// </summary>
        public int CurrentValue
        {
            get { return _counterModel.CurrentValue; }
            set
            {
                _counterModel.CurrentValue = value;
                OnPropertyChanged(nameof(CurrentValue));
                OnPropertyChanged(nameof(ValueDisplay));
            }
        }

        /// <summary>
        /// Gets or sets max value
        /// </summary>
        public int MaxValue
        {
            get { return _counterModel.MaxValue; }
            set
            {
                _counterModel.MaxValue = value;

                if (_counterModel.CurrentValue > _counterModel.MaxValue)
                {
                    _counterModel.CurrentValue = _counterModel.MaxValue;
                    OnPropertyChanged(nameof(CurrentValue));
                }
            }
        }

        /// <summary>
        /// Gets value display
        /// </summary>
        public string ValueDisplay
        {
            get { return $"{_counterModel.CurrentValue}/{_counterModel.MaxValue}"; }
        }

        /// <summary>
        /// Gets or sets reset on short rest
        /// </summary>
        public bool ResetOnShortRest
        {
            get { return _counterModel.ResetOnShortRest; }
            set { _counterModel.ResetOnShortRest = value; }
        }

        /// <summary>
        /// Gets or sets reset on long rest
        /// </summary>
        public bool ResetOnLongRest
        {
            get { return _counterModel.ResetOnLongRest; }
            set { _counterModel.ResetOnLongRest = value; }
        }

        /// <summary>
        /// Gets or sets notes
        /// </summary>
        public string Notes
        {
            get { return _counterModel.Notes; }
            set { _counterModel.Notes = value; }
        }

        /// <summary>
        /// Gets increase current value command
        /// </summary>
        public ICommand IncreaseCurrentValueCommand
        {
            get { return _increaseCurrentValueCommand; }
        }

        /// <summary>
        /// Gets decrease current value command
        /// </summary>
        public ICommand DecreaseCurrentValueCommand
        {
            get { return _decreaseCurrentValueCommand; }
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

        #region Private Methods

        private void IncreaseCurrentValue()
        {
            _counterModel.CurrentValue = Math.Min(_counterModel.MaxValue, _counterModel.CurrentValue + 1);
            OnPropertyChanged(nameof(CurrentValue));
            OnPropertyChanged(nameof(ValueDisplay));
        }

        private void DecreaseCurrentValue()
        {
            _counterModel.CurrentValue = Math.Max(0, _counterModel.CurrentValue - 1);
            OnPropertyChanged(nameof(CurrentValue));
            OnPropertyChanged(nameof(ValueDisplay));
        }

        private void OnAccept()
        {
            if (String.IsNullOrWhiteSpace(_counterModel.Name))
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
