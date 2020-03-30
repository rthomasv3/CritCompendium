using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CritCompendium.ViewModels.ObjectViewModels;
using CriticalCompendiumInfrastructure;
using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels
{
    public sealed class ShortRestViewModel : ObjectViewModel
    {
        #region Fields

        private readonly DiceService _diceService = DependencyResolver.Resolve<DiceService>();
        private readonly List<LevelViewModel> _levelViewModels = new List<LevelViewModel>();
        private int _conModifier;
        private int _totalToHeal;

        private readonly ICommand _rollHitDieCommand;

        #endregion

        #region Constructor

        public ShortRestViewModel(List<LevelModel> levelModels, int conModifier)
        {
            foreach (LevelModel level in levelModels)
            {
                LevelViewModel levelView = new LevelViewModel(level);
                levelView.PropertyChanged += LevelView_PropertyChanged;
                _levelViewModels.Add(levelView);
            }
            _conModifier = conModifier;

            _rollHitDieCommand = new RelayCommand(obj => true, obj => RollHitDie((LevelViewModel)obj));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets unused hit die levels
        /// </summary>
        public IEnumerable<LevelViewModel> UnusedHitDieLevels
        {
            get { return _levelViewModels.Where(x => !x.HitDieUsed); }
        }

        /// <summary>
        /// Gets hit dice display
        /// </summary>
        public string HitDiceDisplay
        {
            get { return $"{UnusedHitDieLevels.Count()}/{_levelViewModels.Count}"; }
        }

        /// <summary>
        /// Gets show hit dice header
        /// </summary>
        public bool ShowHitDiceHeader
        {
            get { return UnusedHitDieLevels.Any(); }
        }

        /// <summary>
        /// Gets total to heal
        /// </summary>
        public int TotalToHeal
        {
            get { return _totalToHeal; }
            set { _totalToHeal = value; }
        }

        /// <summary>
        /// Gets roll hit die command
        /// </summary>
        public ICommand RollHitDieCommand
        {
            get { return _rollHitDieCommand; }
        }

        #endregion

        #region Private Methods
        
        private void RollHitDie(LevelViewModel levelView)
        {
            if (Int32.TryParse(levelView.HitDie.Replace("d", ""), out int hitDie))
            {
                levelView.HitDieRestRoll = _diceService.RandomNumber(1, hitDie);
                UpdateTotal();
            }
        }

        private void LevelView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LevelViewModel.HitDieRestRoll))
            {
                UpdateTotal();
            }
        }

        private void UpdateTotal()
        {
            _totalToHeal = 0;
            foreach (LevelViewModel levelView in _levelViewModels.Where(x => !x.HitDieUsed))
            {
                if (levelView.HitDieRestRoll > 0)
                {
                    _totalToHeal += Math.Max(levelView.HitDieRestRoll + _conModifier, 0);
                }
            }
            OnPropertyChanged(nameof(TotalToHeal));
        }

        #endregion
    }
}
