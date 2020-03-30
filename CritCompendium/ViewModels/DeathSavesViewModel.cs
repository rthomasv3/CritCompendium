using System.Windows.Input;
using CritCompendium.ViewModels.ObjectViewModels;

namespace CritCompendium.ViewModels
{
    public sealed class DeathSavesViewModel : ObjectViewModel
    {
        #region Fields

        private int _successes;
        private int _failures;
        
        private readonly ICommand _updateDeathSaveSuccessesCommand;
        private readonly ICommand _updateDeathSaveFailuresCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="DeathSavesViewModel"/>
        /// </summary>
        public DeathSavesViewModel(int successes, int failures)
        {
            _successes = successes;
            _failures = failures;

            _updateDeathSaveSuccessesCommand = new RelayCommand(obj => true, obj => UpdateDeathSaveSuccesses((bool)obj));
            _updateDeathSaveFailuresCommand = new RelayCommand(obj => true, obj => UpdateDeathSaveFailures((bool)obj));
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets or sets successes
        /// </summary>
        public int Successes
        {
            get { return _successes; }
            set { _successes = value; }
        }

        /// <summary>
        /// Gets or sets failures
        /// </summary>
        public int Failures
        {
            get { return _failures; }
            set { _failures = value; }
        }

        /// <summary>
        /// Gets first death save success
        /// </summary>
        public bool FirstDeathSaveSuccess
        {
            get { return _successes > 0; }
        }

        /// <summary>
        /// Gets second death save success
        /// </summary>
        public bool SecondDeathSaveSuccess
        {
            get { return _successes > 1; }
        }

        /// <summary>
        /// Gets third death save success
        /// </summary>
        public bool ThirdDeathSaveSuccess
        {
            get { return _successes > 2; }
        }

        /// <summary>
        /// Gets first death save failure
        /// </summary>
        public bool FirstDeathSaveFailure
        {
            get { return _failures > 0; }
        }

        /// <summary>
        /// Gets second death save failure
        /// </summary>
        public bool SecondDeathSaveFailure
        {
            get { return _failures > 1; }
        }

        /// <summary>
        /// Gets third death save failure
        /// </summary>
        public bool ThirdDeathSaveFailure
        {
            get { return _failures > 2; }
        }

        /// <summary>
        /// Gets update death save successes command
        /// </summary>
        public ICommand UpdateDeathSaveSuccessesCommand
        {
            get { return _updateDeathSaveSuccessesCommand; }
        }

        /// <summary>
        /// Gets update death save failures command
        /// </summary>
        public ICommand UpdateDeathSaveFailuresCommand
        {
            get { return _updateDeathSaveFailuresCommand; }
        }

        #endregion

        #region Private Methods
        
        private void UpdateDeathSaveSuccesses(bool check)
        {
            if (check)
            {
                _successes++;
            }
            else
            {
                _successes--;
            }

            OnPropertyChanged(nameof(FirstDeathSaveSuccess));
            OnPropertyChanged(nameof(SecondDeathSaveSuccess));
            OnPropertyChanged(nameof(ThirdDeathSaveSuccess));
        }

        private void UpdateDeathSaveFailures(bool check)
        {
            if (check)
            {
                _failures++;
            }
            else
            {
                _failures--;
            }

            OnPropertyChanged(nameof(FirstDeathSaveFailure));
            OnPropertyChanged(nameof(SecondDeathSaveFailure));
            OnPropertyChanged(nameof(ThirdDeathSaveFailure));
        }

        #endregion
    }
}
