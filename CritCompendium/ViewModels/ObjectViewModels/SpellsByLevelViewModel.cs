using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class SpellsByLevelViewModel : NotifyPropertyChanged
    {
        #region Fields

        private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();

        private int _level;
        private bool _showSpellSlots;
        private bool _showClassSpellHeader;
        private bool _showRaceSpellHeader;
        private int _currentSpellSlots;
        private int _maxSpellSlots;
        private ObservableCollection<SpellbookEntryViewModel> _spells = new ObservableCollection<SpellbookEntryViewModel>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="SpellsByLevelViewModel"/>
        /// </summary>
        public SpellsByLevelViewModel(int level, int maxSpellSlots, bool basedOnClass, bool basedOnRace)
        {
            _level = level;
            _maxSpellSlots = maxSpellSlots;
            _showClassSpellHeader = basedOnClass;
            _showRaceSpellHeader = basedOnRace;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets level
        /// </summary>
        public int Level
        {
            get { return _level; }
        }

        /// <summary>
        /// Gets level display
        /// </summary>
        public string LevelDisplay
        {
            get { return _level > 0 ? $"{_stringService.NumberToOrdinal(_level)} Level" : "Cantrips"; }
        }

        /// <summary>
        /// Gets show class spell header
        /// </summary>
        public bool ShowClassSpellHeader
        {
            get { return _showClassSpellHeader && _spells.Any(); }
        }

        /// <summary>
        /// Gets show race spell header
        /// </summary>
        public bool ShowRaceSpellHeader
        {
            get { return _showRaceSpellHeader && _spells.Any(); }
        }

        /// <summary>
        /// Gets spells
        /// </summary>
        public IEnumerable<SpellbookEntryViewModel> Spells
        {
            get { return _spells; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the spell
        /// </summary>
        public void AddSpell(SpellbookEntryModel spellbookEntryModel)
        {
            _spells.Add(new SpellbookEntryViewModel(spellbookEntryModel));
            _spells = new ObservableCollection<SpellbookEntryViewModel>(_spells.OrderBy(x => x.SpellName));
            OnPropertyChanged(nameof(Spells));
            OnPropertyChanged(nameof(ShowClassSpellHeader));
            OnPropertyChanged(nameof(ShowRaceSpellHeader));
        }

        /// <summary>
        /// Removes the spell
        /// </summary>
        public void RemoveSpell(SpellbookEntryModel spellbookEntryModel)
        {
            SpellbookEntryViewModel spellbookEntryViewModel = _spells.FirstOrDefault(x => x.SpellbookEntryModel.ID == spellbookEntryModel.ID);
            if (spellbookEntryViewModel != null)
            {
                _spells.Remove(spellbookEntryViewModel);
                OnPropertyChanged(nameof(Spells));
                OnPropertyChanged(nameof(ShowClassSpellHeader));
                OnPropertyChanged(nameof(ShowRaceSpellHeader));
            }
        }

        #endregion
    }
}
