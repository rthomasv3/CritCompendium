using System;
using System.Windows;
using System.Windows.Input;
using CritCompendium.Business;
using CritCompendium.Views;
using CritCompendiumInfrastructure.Persistence;
using CritCompendiumInfrastructure.Business;

namespace CritCompendium.ViewModels
{
   public sealed class MainWindowViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly Compendium _compendium;
      private readonly DataManager _dataManager;
      private readonly DialogService _dialogService;

      private readonly string _patreonURL = "https://patreon.com/whimsyandwizardry";
      private readonly string _twitterURL = "https://twitter.com/whimsy_wizardry";
      private readonly string _redditURL = "https://reddit.com/u/whimsyandwizardry";

      private readonly ICommand _minimizeCommand;
      private readonly ICommand _restoreCommand;
      private readonly ICommand _maximizeCommand;
      private readonly ICommand _closeCommand;
      private readonly ICommand _showSettingsCommand;

      private readonly ICommand _charactersCommand;
      private readonly ICommand _adventuresCommand;
      private readonly ICommand _encountersCommand;
      private readonly ICommand _locationsCommand;
      private readonly ICommand _npcsCommand;
      private readonly ICommand _tablesCommand;
      private readonly ICommand _backgroundsCommand;
      private readonly ICommand _classesCommand;
      private readonly ICommand _conditionsCommand;
      private readonly ICommand _featsCommand;
      private readonly ICommand _itemsCommand;
      private readonly ICommand _monstersCommand;
      private readonly ICommand _racesCommand;
      private readonly ICommand _spellsCommand;
      private readonly ICommand _referenceCommand;
      private readonly ICommand _importCommand;
      private readonly ICommand _showCalculatorCommand;
      private readonly ICommand _showPatreonCommand;
      private readonly ICommand _showOnboardingCommand;
      private readonly ICommand _nextOnboardingCommand;
      private readonly ICommand _exitOnboardingCommand;
      private readonly ICommand _showAboutCommand;
      private readonly ICommand _navigateToPatreonCommand;
      private readonly ICommand _navigateToTwitterCommand;
      private readonly ICommand _navigateToRedditCommand;

      private FrameworkElement _mainView;
      private bool _onboardingVisible;
      private bool _onboardingAVisible;
      private bool _onboardingBVisible;
      private bool _onboardingCVisible;
      private bool _onboardingDVisible;
      private bool _onboardingEVisible;

      #endregion

      #region Constructor

      public MainWindowViewModel(Compendium compendium, DataManager dataManager, DialogService dialogService)
      {
         _compendium = compendium;
         _dataManager = dataManager;
         _dialogService = dialogService;

         _minimizeCommand = new RelayCommand(obj => true, obj => MinimizeMainWindow());
         _restoreCommand = new RelayCommand(obj => true, obj => RestoreMainWindow());
         _maximizeCommand = new RelayCommand(obj => true, obj => MaximizeMainWindow());
         _closeCommand = new RelayCommand(obj => true, obj => CloseMainWindow());
         _showSettingsCommand = new RelayCommand(obj => true, obj => ShowSettingsWindow());

         _charactersCommand = new RelayCommand(obj => true, obj => NavigateToCharacters());
         _adventuresCommand = new RelayCommand(obj => true, obj => NavigateToAdventures());
         _encountersCommand = new RelayCommand(obj => true, obj => NavigateToEncounters());
         _locationsCommand = new RelayCommand(obj => true, obj => NavigateToLocations());
         _npcsCommand = new RelayCommand(obj => true, obj => NavigateToNPCs());
         _tablesCommand = new RelayCommand(obj => true, obj => NavigateToTables());
         _backgroundsCommand = new RelayCommand(obj => true, obj => NavigateToBackgrounds());
         _classesCommand = new RelayCommand(obj => true, obj => NavigateToClasses());
         _conditionsCommand = new RelayCommand(obj => true, obj => NavigateToConditions());
         _featsCommand = new RelayCommand(obj => true, obj => NavigateToFeats());
         _itemsCommand = new RelayCommand(obj => true, obj => NavigateToItems());
         _monstersCommand = new RelayCommand(obj => true, obj => NavigateToMonsters());
         _racesCommand = new RelayCommand(obj => true, obj => NavigateToRaces());
         _spellsCommand = new RelayCommand(obj => true, obj => NavigateToSpells());
         _referenceCommand = new RelayCommand(obj => true, obj => NavigateToReference());
         _importCommand = new RelayCommand(obj => true, obj => NavigateToImport());
         _showCalculatorCommand = new RelayCommand(obj => true, obj => ShowCalculator(obj as string));
         _showPatreonCommand = new RelayCommand(obj => true, obj => ShowPatreon());
         _showOnboardingCommand = new RelayCommand(obj => true, obj => ShowOnboarding());
         _nextOnboardingCommand = new RelayCommand(obj => true, obj => NextOnboarding());
         _exitOnboardingCommand = new RelayCommand(obj => true, obj => ExitOnboarding());
         _showAboutCommand = new RelayCommand(obj => true, obj => ShowAbout());
         _navigateToPatreonCommand = new RelayCommand(obj => true, obj => NavigateToPatreon());
         _navigateToTwitterCommand = new RelayCommand(obj => true, obj => NavigateToTwitter());
         _navigateToRedditCommand = new RelayCommand(obj => true, obj => NavigateToReddit());

         _mainView = new CharactersView();

         _compendium.ImportComplete += _compendium_ImportComplete;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Main view
      /// </summary>
      public FrameworkElement MainView
      {
         get { return _mainView; }
         set
         {
            if (Set(ref _mainView, value))
            {
               OnPropertyChanged(nameof(CharactersSelected));
               OnPropertyChanged(nameof(AdventuresSelected));
               OnPropertyChanged(nameof(EncountersSelected));
               OnPropertyChanged(nameof(LocationsSelected));
               OnPropertyChanged(nameof(NPCsSelected));
               OnPropertyChanged(nameof(TablesSelected));
               OnPropertyChanged(nameof(BackgroundsSelected));
               OnPropertyChanged(nameof(ClassesSelected));
               OnPropertyChanged(nameof(ConditionsSelected));
               OnPropertyChanged(nameof(FeatsSelected));
               OnPropertyChanged(nameof(ItemsSelected));
               OnPropertyChanged(nameof(MonstersSelected));
               OnPropertyChanged(nameof(RacesSelected));
               OnPropertyChanged(nameof(SpellsSelected));
               OnPropertyChanged(nameof(ReferenceSelected));
               OnPropertyChanged(nameof(ImportSelected));
            }
         }
      }

      /// <summary>
      /// True if the restore button should be visible
      /// </summary>
      public bool RestoreVisible
      {
         get { return Application.Current.MainWindow.WindowState == WindowState.Maximized; }
      }

      /// <summary>
      /// True if the maximize button should be visible
      /// </summary>
      public bool MaximizeVisible
      {
         get { return Application.Current.MainWindow.WindowState != WindowState.Maximized; }
      }

      /// <summary>
      /// Gets characters selected
      /// </summary>
      public bool CharactersSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(CharactersView); }
      }

      /// <summary>
      /// Gets adventures selected
      /// </summary>
      public bool AdventuresSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(AdventuresView); }
      }

      /// <summary>
      /// Gets encounters selected
      /// </summary>
      public bool EncountersSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(EncountersView); }
      }

      /// <summary>
      /// Gets locations selected
      /// </summary>
      public bool LocationsSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(LocationsView); }
      }

      /// <summary>
      /// Gets npcs selected
      /// </summary>
      public bool NPCsSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(NPCsView); }
      }

      /// <summary>
      /// Gets tables selected
      /// </summary>
      public bool TablesSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(TablesView); }
      }

      /// <summary>
      /// Gets backgrounds selected
      /// </summary>
      public bool BackgroundsSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(BackgroundsView); }
      }

      /// <summary>
      /// Gets classes selected
      /// </summary>
      public bool ClassesSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(ClassesView); }
      }

      /// <summary>
      /// Gets conditions selected
      /// </summary>
      public bool ConditionsSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(ConditionsView); }
      }

      /// <summary>
      /// Gets feats selected
      /// </summary>
      public bool FeatsSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(FeatsView); }
      }

      /// <summary>
      /// Gets items selected
      /// </summary>
      public bool ItemsSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(ItemsView); }
      }

      /// <summary>
      /// Gets monsters selected
      /// </summary>
      public bool MonstersSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(MonstersView); }
      }

      /// <summary>
      /// Gets races selected
      /// </summary>
      public bool RacesSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(RacesView); }
      }

      /// <summary>
      /// Gets spells selected
      /// </summary>
      public bool SpellsSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(SpellsView); }
      }

      /// <summary>
      /// Gets reference selected
      /// </summary>
      public bool ReferenceSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(ReferenceView); }
      }

      /// <summary>
      /// Gets import selected
      /// </summary>
      public bool ImportSelected
      {
         get { return _mainView != null && _mainView.GetType() == typeof(ImportView); }
      }

      /// <summary>
      /// Gets onboarding
      /// </summary>
      public bool OnboardingVisible
      {
         get { return _onboardingVisible; }
      }

      /// <summary>
      /// Gets onboarding a visible
      /// </summary>
      public bool OnboardingAVisible
      {
         get { return _onboardingAVisible; }
      }

      /// <summary>
      /// Gets onboarding b visible
      /// </summary>
      public bool OnboardingBVisible
      {
         get { return _onboardingBVisible; }
      }

      /// <summary>
      /// Gets onboarding c visible
      /// </summary>
      public bool OnboardingCVisible
      {
         get { return _onboardingCVisible; }
      }

      /// <summary>
      /// Gets onboarding d visible
      /// </summary>
      public bool OnboardingDVisible
      {
         get { return _onboardingDVisible; }
      }

      /// <summary>
      /// Gets onboarding e visible
      /// </summary>
      public bool OnboardingEVisible
      {
         get { return _onboardingEVisible; }
      }

      /// <summary>
      /// Command to minimize main window
      /// </summary>
      public ICommand MinimizeCommand
      {
         get { return _minimizeCommand; }
      }

      /// <summary>
      /// Command to restore main window
      /// </summary>
      public ICommand RestoreCommand
      {
         get { return _restoreCommand; }
      }

      /// <summary>
      /// Command to maximize main window
      /// </summary>
      public ICommand MaximizeCommand
      {
         get { return _maximizeCommand; }
      }

      /// <summary>
      /// Command to close main window
      /// </summary>
      public ICommand CloseCommand
      {
         get { return _closeCommand; }
      }

      /// <summary>
      /// Command to show settings
      /// </summary>
      public ICommand ShowSettingsCommand
      {
         get { return _showSettingsCommand; }
      }

      /// <summary>
      /// Gets characters command
      /// </summary>
      public ICommand CharactersCommand
      {
         get { return _charactersCommand; }
      }

      /// <summary>
      /// Gets adventures command
      /// </summary>
      public ICommand AdventuresCommand
      {
         get { return _adventuresCommand; }
      }

      /// <summary>
      /// Gets encounters command
      /// </summary>
      public ICommand EncountersCommand
      {
         get { return _encountersCommand; }
      }

      /// <summary>
      /// Gets locations command
      /// </summary>
      public ICommand LocationsCommand
      {
         get { return _locationsCommand; }
      }

      /// <summary>
      /// Gets npcs command
      /// </summary>
      public ICommand NPCsCommand
      {
         get { return _npcsCommand; }
      }

      /// <summary>
      /// Gets tables command
      /// </summary>
      public ICommand TablesCommand
      {
         get { return _tablesCommand; }
      }

      /// <summary>
      /// Gets backgrounds command
      /// </summary>
      public ICommand BackgroundsCommand
      {
         get { return _backgroundsCommand; }
      }

      /// <summary>
      /// Gets classes command
      /// </summary>
      public ICommand ClassesCommand
      {
         get { return _classesCommand; }
      }

      /// <summary>
      /// Gets conditions command
      /// </summary>
      public ICommand ConditionsCommand
      {
         get { return _conditionsCommand; }
      }

      /// <summary>
      /// Gets feats command
      /// </summary>
      public ICommand FeatsCommand
      {
         get { return _featsCommand; }
      }

      /// <summary>
      /// Gets items command
      /// </summary>
      public ICommand ItemsCommand
      {
         get { return _itemsCommand; }
      }

      /// <summary>
      /// Gets monsters command
      /// </summary>
      public ICommand MonstersCommand
      {
         get { return _monstersCommand; }
      }

      /// <summary>
      /// Gets races command
      /// </summary>
      public ICommand RacesCommand
      {
         get { return _racesCommand; }
      }

      /// <summary>
      /// Gets spells command
      /// </summary>
      public ICommand SpellsCommand
      {
         get { return _spellsCommand; }
      }

      /// <summary>
      /// Gets reference command
      /// </summary>
      public ICommand ReferenceCommand
      {
         get { return _referenceCommand; }
      }

      /// <summary>
      /// Gets import command
      /// </summary>
      public ICommand ImportCommand
      {
         get { return _importCommand; }
      }

      /// <summary>
      /// Gets show calculator command
      /// </summary>
      public ICommand ShowCalculatorCommand
      {
         get { return _showCalculatorCommand; }
      }

      /// <summary>
      /// Gets show patreon command
      /// </summary>
      public ICommand ShowPatreonCommand
      {
         get { return _showPatreonCommand; }
      }

      /// <summary>
      /// Gets show onboarding command
      /// </summary>
      public ICommand ShowOnboardingCommand
      {
         get { return _showOnboardingCommand; }
      }

      /// <summary>
      /// Gets next onboarding command
      /// </summary>
      public ICommand NextOnboardingCommand
      {
         get { return _nextOnboardingCommand; }
      }

      /// <summary>
      /// Gets exit onboarding command
      /// </summary>
      public ICommand ExitOnboardingCommand
      {
         get { return _exitOnboardingCommand; }
      }

      /// <summary>
      /// Gets show about command
      /// </summary>
      public ICommand ShowAboutCommand
      {
         get { return _showAboutCommand; }
      }

      /// <summary>
      /// Gets navigate to patreon Command
      /// </summary>
      public ICommand NavigateToPatreonCommand
      {
         get { return _navigateToPatreonCommand; }
      }

      /// <summary>
      /// Gets navigate to twitter command
      /// </summary>
      public ICommand NavigateToTwitterCommand
      {
         get { return _navigateToTwitterCommand; }
      }

      /// <summary>
      /// Gets navigate to reddit command
      /// </summary>
      public ICommand NavigateToRedditCommand
      {
         get { return _navigateToRedditCommand; }
      }

      #endregion

      #region Public Methods

      public void RestoreMainWindow()
      {
         Application.Current.MainWindow.WindowState = WindowState.Normal;
         OnPropertyChanged(nameof(RestoreVisible));
         OnPropertyChanged(nameof(MaximizeVisible));
      }

      public void MaximizeMainWindow()
      {
         Application.Current.MainWindow.WindowState = WindowState.Maximized;
         OnPropertyChanged(nameof(RestoreVisible));
         OnPropertyChanged(nameof(MaximizeVisible));
      }

      #endregion

      #region Non-Public Methods

      private void MinimizeMainWindow()
      {
         Application.Current.MainWindow.WindowState = WindowState.Minimized;
      }

      private void CloseMainWindow()
      {
         Application.Current.MainWindow.Close();
      }

      private void ShowSettingsWindow()
      {
         _dialogService.ShowSettingsWindow();
      }

      private void NavigateToCharacters()
      {
         MainView = new CharactersView();
      }

      private void NavigateToAdventures()
      {
         MainView = new AdventuresView();
      }

      private void NavigateToEncounters()
      {
         MainView = new EncountersView();
      }

      private void NavigateToLocations()
      {
         MainView = new LocationsView();
      }

      private void NavigateToNPCs()
      {
         MainView = new NPCsView();
      }

      private void NavigateToTables()
      {
         MainView = new TablesView();
      }

      private void NavigateToBackgrounds()
      {
         MainView = new BackgroundsView();
      }

      private void NavigateToClasses()
      {
         MainView = new ClassesView();
      }

      private void NavigateToConditions()
      {
         MainView = new ConditionsView();
      }

      private void NavigateToFeats()
      {
         MainView = new FeatsView();
      }

      private void NavigateToItems()
      {
         MainView = new ItemsView();
      }

      private void NavigateToMonsters()
      {
         MainView = new MonstersView();
      }

      private void NavigateToRaces()
      {
         MainView = new RacesView();
      }

      private void NavigateToSpells()
      {
         MainView = new SpellsView();
      }

      private void NavigateToReference()
      {
         MainView = new ReferenceView();
      }

      private void NavigateToImport()
      {
         _dialogService.ShowImportView();
      }

      private void ShowCalculator(string expression)
      {
         _dialogService.ShowCalculatorDialog(expression);
      }

      private void _compendium_ImportComplete(object sender, System.EventArgs e)
      {
         if (_mainView != null)
         {
            if (_mainView.DataContext is CharactersViewModel charactersViewModel)
            {
               charactersViewModel.Search();
            }
            else if (_mainView.DataContext is EncountersViewModel encountersViewModel)
            {
               encountersViewModel.Search();
            }
            else if (_mainView.DataContext is BackgroundsViewModel backgroundsViewModel)
            {
               backgroundsViewModel.Search();
            }
            else if (_mainView.DataContext is ClassesViewModel classesViewModel)
            {
               classesViewModel.Search();
            }
            else if (_mainView.DataContext is ConditionsViewModel conditionsViewModel)
            {
               conditionsViewModel.Search();
            }
            else if (_mainView.DataContext is FeatsViewModel featsViewModel)
            {
               featsViewModel.Search();
            }
            else if (_mainView.DataContext is ItemsViewModel itemsViewModel)
            {
               itemsViewModel.Search();
            }
            else if (_mainView.DataContext is MonstersViewModel monstersViewModel)
            {
               monstersViewModel.Search();
            }
            else if (_mainView.DataContext is RacesViewModel racesViewModel)
            {
               racesViewModel.Search();
            }
            else if (_mainView.DataContext is SpellsViewModel spellsViewModel)
            {
               spellsViewModel.Search();
            }
         }
      }

      private void ShowPatreon()
      {
         _dialogService.ShowPatreonDialog();
      }

      private void ShowOnboarding()
      {
         _onboardingVisible = true;
         _onboardingAVisible = true;

         OnPropertyChanged(nameof(OnboardingVisible));
         OnPropertyChanged(nameof(OnboardingAVisible));
      }

      private void NextOnboarding()
      {
         if (_onboardingAVisible)
         {
            _onboardingAVisible = false;
            _onboardingBVisible = true;
            OnPropertyChanged(nameof(OnboardingAVisible));
            OnPropertyChanged(nameof(OnboardingBVisible));
         }
         else if (_onboardingBVisible)
         {
            _onboardingBVisible = false;
            _onboardingCVisible = true;
            OnPropertyChanged(nameof(OnboardingBVisible));
            OnPropertyChanged(nameof(OnboardingCVisible));
         }
         else if (_onboardingCVisible)
         {
            _onboardingCVisible = false;
            _onboardingDVisible = true;
            OnPropertyChanged(nameof(OnboardingCVisible));
            OnPropertyChanged(nameof(OnboardingDVisible));
         }
         else if (_onboardingDVisible)
         {
            _onboardingDVisible = false;
            _onboardingEVisible = true;
            OnPropertyChanged(nameof(OnboardingDVisible));
            OnPropertyChanged(nameof(OnboardingEVisible));
         }
      }

      private void ExitOnboarding()
      {
         _onboardingVisible = false;
         _onboardingAVisible = false;
         _onboardingBVisible = false;
         _onboardingCVisible = false;
         _onboardingDVisible = false;
         _onboardingEVisible = false;

         OnPropertyChanged(nameof(OnboardingVisible));
         OnPropertyChanged(nameof(OnboardingAVisible));
         OnPropertyChanged(nameof(OnboardingBVisible));
         OnPropertyChanged(nameof(OnboardingCVisible));
         OnPropertyChanged(nameof(OnboardingDVisible));
         OnPropertyChanged(nameof(OnboardingEVisible));
      }

      private void ShowAbout()
      {
         _dialogService.ShowAboutDialog();
      }

      private void NavigateToPatreon()
      {
         _dialogService.ShowPatreonDialog();
      }

      private void NavigateToTwitter()
      {
         System.Diagnostics.Process.Start(_twitterURL);
      }

      private void NavigateToReddit()
      {
         System.Diagnostics.Process.Start(_redditURL);
      }

      #endregion
   }
}
