using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using CritCompendium.ViewModels.DialogViewModels;

namespace CritCompendium.ViewModels
{
    public sealed class SettingsViewModel : NotifyPropertyChanged, IConfirmation
    {
        #region Events
        
        public event EventHandler AcceptSelected;
        public event EventHandler RejectSelected;
        public event EventHandler CancelSelected;

        #endregion

        #region Fields

        private readonly ThemeManager _themeManager;

        private readonly List<string> _fontOptions = new List<string>();
        private string _selectedFontOption;
        private readonly List<KeyValuePair<AccentColor, string>> _accentColorOptions = new List<KeyValuePair<AccentColor, string>>();
        private KeyValuePair<AccentColor, string> _selectedAccentColor = new KeyValuePair<AccentColor, string>();
        private readonly List<KeyValuePair<Theme, string>> _themeOptions = new List<KeyValuePair<Theme, string>>();
        private KeyValuePair<Theme, string> _selectedTheme = new KeyValuePair<Theme, string>();

        private readonly ICommand _acceptCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="SettingsViewModel"/>
        /// </summary>
        public SettingsViewModel(ThemeManager themeManager)
        {
            _themeManager = themeManager;

            using (InstalledFontCollection fontsCollection = new InstalledFontCollection())
            {
                foreach (System.Drawing.FontFamily fontFamily in fontsCollection.Families)
                {
                    _fontOptions.Add(fontFamily.Name);
                }
            }

            if (_fontOptions.Contains(_themeManager.Font))
            {
                _selectedFontOption = _themeManager.Font;
            }
            else
            {
                _selectedFontOption = _fontOptions[0];
            }

            foreach (AccentColor color in Enum.GetValues(typeof(AccentColor)))
            {
                _accentColorOptions.Add(new KeyValuePair<AccentColor, string>(color, color.ToString()));
            }

            if (_accentColorOptions.Any(x => x.Key == _themeManager.AccentColor))
            {
                _selectedAccentColor = _accentColorOptions.First(x => x.Key == _themeManager.AccentColor);
            }
            else
            {
                _selectedAccentColor = _accentColorOptions[0];
            }

            foreach (Theme theme in Enum.GetValues(typeof(Theme)))
            {
                _themeOptions.Add(new KeyValuePair<Theme, string>(theme, theme.ToString()));
            }

            if (_themeOptions.Any(x => x.Key == _themeManager.Theme))
            {
                _selectedTheme = _themeOptions.First(x => x.Key == _themeManager.Theme);
            }
            else
            {
                _selectedTheme = _themeOptions[0];
            }

            _acceptCommand = new RelayCommand(obj => true, obj => Accept());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets font options
        /// </summary>
        public IEnumerable<string> FontOptions
        {
            get { return _fontOptions; }
        }

        /// <summary>
        /// Gets or sets the selected font option
        /// </summary>
        public string SelectedFontOption
        {
            get { return _selectedFontOption; }
            set
            {
                if (Set(ref _selectedFontOption, value))
                {
                    _themeManager.SetFont(_selectedFontOption);
                    _themeManager.SaveTheme();
                }
            }
        }

        /// <summary>
        /// Gets accent color options
        /// </summary>
        public IEnumerable<KeyValuePair<AccentColor, string>> AccentColorOptions
        {
            get { return _accentColorOptions; }
        }

        /// <summary>
        /// Gets or sets accent color options
        /// </summary>
        public KeyValuePair<AccentColor, string> SelectedAccentColorOption
        {
            get { return _selectedAccentColor; }
            set
            {
                _selectedAccentColor = value;
                _themeManager.SetAccentColor(_selectedAccentColor.Key);
                _themeManager.SetTheme(_selectedTheme.Key);
                _themeManager.SaveTheme();
                OnPropertyChanged(nameof(AccentColor));
            }
        }
        
        /// <summary>
        /// Gets accent color
        /// </summary>
        public Color AccentColor
        {
            get { return _themeManager.GetAccentColor(_selectedAccentColor.Key); }
        }

        /// <summary>
        /// Gets theme options
        /// </summary>
        public IEnumerable<KeyValuePair<Theme, string>> ThemeOptions
        {
            get { return _themeOptions; }
        }

        /// <summary>
        /// Gets or sets selected theme option
        /// </summary>
        public KeyValuePair<Theme, string> SelectedThemeOption
        {
            get { return _selectedTheme; }
            set
            {
                _selectedTheme = value;
                _themeManager.SetTheme(value.Key);
                _themeManager.SaveTheme();
            }
        }

        /// <summary>
        /// Gets accept command
        /// </summary>
        public ICommand AcceptCommand
        {
            get { return _acceptCommand; }
        }

        #endregion

        #region Private Methods
        
        private void Accept()
        {
            AcceptSelected?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
