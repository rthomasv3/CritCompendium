using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using CritCompendiumInfrastructure.Business;

namespace CritCompendium
{
   public sealed class ThemeManager
   {
      #region Fields

      private static readonly int _version = 1;
      private readonly ResourceDictionary _blueAccent;
      private readonly ResourceDictionary _greenAccent;
      private readonly ResourceDictionary _orangeAccent;
      private readonly ResourceDictionary _purpleAccent;
      private readonly ResourceDictionary _redAccent;
      private readonly ResourceDictionary _steelAccent;
      private readonly IDataManager _dataManager;

      private string _selectedFont;
      private AccentColor _selectedAccentColor;
      private Theme _selectedTheme;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of <see cref="ThemeManager"/>
      /// </summary>
      public ThemeManager(IDataManager dataManager)
      {
         _dataManager = dataManager;

         Uri location = new Uri(@"/CritCompendium;component/Resources/Themes/Accents/Blue.xaml", UriKind.Relative);
         _blueAccent = (ResourceDictionary)Application.LoadComponent(location);

         location = new Uri(@"/CritCompendium;component/Resources/Themes/Accents/Green.xaml", UriKind.Relative);
         _greenAccent = (ResourceDictionary)Application.LoadComponent(location);

         location = new Uri(@"/CritCompendium;component/Resources/Themes/Accents/Orange.xaml", UriKind.Relative);
         _orangeAccent = (ResourceDictionary)Application.LoadComponent(location);

         location = new Uri(@"/CritCompendium;component/Resources/Themes/Accents/Purple.xaml", UriKind.Relative);
         _purpleAccent = (ResourceDictionary)Application.LoadComponent(location);

         location = new Uri(@"/CritCompendium;component/Resources/Themes/Accents/Red.xaml", UriKind.Relative);
         _redAccent = (ResourceDictionary)Application.LoadComponent(location);

         location = new Uri(@"/CritCompendium;component/Resources/Themes/Accents/Steel.xaml", UriKind.Relative);
         _steelAccent = (ResourceDictionary)Application.LoadComponent(location);

         byte[] themeBytes = _dataManager.LoadTheme();

         if (themeBytes == null)
         {
            _selectedFont = "Segoe UI";
            _selectedAccentColor = AccentColor.Blue;
            _selectedTheme = Theme.Dark;
         }
         else
         {
            using (MemoryStream stream = new MemoryStream(themeBytes))
            {
               using (BinaryReader reader = new BinaryReader(stream))
               {
                  int version = reader.ReadInt32();

                  if (version == 1)
                  {
                     _selectedFont = reader.ReadString();
                     _selectedAccentColor = (AccentColor)reader.ReadInt32();
                     _selectedTheme = (Theme)reader.ReadInt32();
                  }
               }
            }
         }

         SetFont(_selectedFont);
         SetAccentColor(_selectedAccentColor);
         SetTheme(_selectedTheme);
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets the selected font
      /// </summary>
      public string Font
      {
         get { return _selectedFont; }
      }

      /// <summary>
      /// Gets the selected accent color
      /// </summary>
      public AccentColor AccentColor
      {
         get { return _selectedAccentColor; }
      }

      /// <summary>
      /// Gets the selected theme
      /// </summary>
      public Theme Theme
      {
         get { return _selectedTheme; }
      }

      #endregion

      #region Public Methods

      public void SetFont(string font)
      {
         Application.Current.Resources["_primaryFont"] = new FontFamily(font);
         _selectedFont = font;
      }

      public Color GetAccentColor(AccentColor accentColor)
      {
         string value = String.Empty;

         if (accentColor == AccentColor.Blue)
         {
            value = _blueAccent["_accentColor"].ToString();
         }
         else if (accentColor == AccentColor.Green)
         {
            value = _greenAccent["_accentColor"].ToString();
         }
         else if (accentColor == AccentColor.Orange)
         {
            value = _orangeAccent["_accentColor"].ToString();
         }
         else if (accentColor == AccentColor.Purple)
         {
            value = _purpleAccent["_accentColor"].ToString();
         }
         else if (accentColor == AccentColor.Red)
         {
            value = _redAccent["_accentColor"].ToString();
         }
         else if (accentColor == AccentColor.Steel)
         {
            value = _steelAccent["_accentColor"].ToString();
         }

         return (Color)ColorConverter.ConvertFromString(value);
      }

      public void SetAccentColor(AccentColor accentColor)
      {
         if (accentColor == AccentColor.Blue)
         {
            Application.Current.Resources.MergedDictionaries.RemoveAt(0);
            Application.Current.Resources.MergedDictionaries.Insert(0, _blueAccent);
         }
         else if (accentColor == AccentColor.Green)
         {
            Application.Current.Resources.MergedDictionaries.RemoveAt(0);
            Application.Current.Resources.MergedDictionaries.Insert(0, _greenAccent);
         }
         else if (accentColor == AccentColor.Orange)
         {
            Application.Current.Resources.MergedDictionaries.RemoveAt(0);
            Application.Current.Resources.MergedDictionaries.Insert(0, _orangeAccent);
         }
         else if (accentColor == AccentColor.Purple)
         {
            Application.Current.Resources.MergedDictionaries.RemoveAt(0);
            Application.Current.Resources.MergedDictionaries.Insert(0, _purpleAccent);
         }
         else if (accentColor == AccentColor.Red)
         {
            Application.Current.Resources.MergedDictionaries.RemoveAt(0);
            Application.Current.Resources.MergedDictionaries.Insert(0, _redAccent);
         }
         else if (accentColor == AccentColor.Steel)
         {
            Application.Current.Resources.MergedDictionaries.RemoveAt(0);
            Application.Current.Resources.MergedDictionaries.Insert(0, _steelAccent);
         }

         _selectedAccentColor = accentColor;
      }

      public void SetTheme(Theme theme)
      {
         Uri location = null;

         if (theme == Theme.Dark)
         {
            location = new Uri(@"/CritCompendium;component/Resources/Themes/DarkTheme.xaml", UriKind.Relative);
         }
         else if (theme == Theme.Light)
         {
            location = new Uri(@"/CritCompendium;component/Resources/Themes/LightTheme.xaml", UriKind.Relative);
         }
         else if (theme == Theme.Parchment)
         {
            location = new Uri(@"/CritCompendium;component/Resources/Themes/ParchmentTheme.xaml", UriKind.Relative);
         }

         if (location != null)
         {
            ResourceDictionary themeResource = (ResourceDictionary)Application.LoadComponent(location);
            Application.Current.Resources.MergedDictionaries.RemoveAt(1);
            Application.Current.Resources.MergedDictionaries.Insert(1, themeResource);
         }

         _selectedTheme = theme;
      }

      /// <summary>
      /// Saves the theme
      /// </summary>
      public void SaveTheme()
      {
         byte[] themeBytes;

         using (MemoryStream stream = new MemoryStream())
         {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
               writer.Write(_version);
               writer.Write(_selectedFont);
               writer.Write((int)_selectedAccentColor);
               writer.Write((int)_selectedTheme);
            }
            themeBytes = stream.ToArray();
         }

         _dataManager.SaveTheme(themeBytes);
      }

      #endregion
   }
}
