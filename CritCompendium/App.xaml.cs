using System;
using System.Windows;
using System.Windows.Threading;
using CritCompendium.Services;
using CritCompendium.ViewModels;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Persistence;
using CritCompendiumInfrastructure.Services;
using CritCompendiumInfrastructure.Services.Search.Input;

namespace CritCompendium
{
   /// <summary>
   /// Interaction logic for App.xaml
   /// </summary>
   public partial class App : Application
   {
      private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();

      private void Application_Startup(object sender, StartupEventArgs e)
      {
         SetupDependencies();

         StartupUri = new Uri("/CritCompendium;component/Views/MainWindow.xaml", UriKind.Relative);

         DispatcherUnhandledException += App_DispatcherUnhandledException;
         SessionEnding += App_SessionEnding;
         Exit += App_Exit;
      }

      private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
      {
         _dialogService.ShowConfirmationDialog("Unhandled Exception", e.Exception.Message + "\n\n" + e.Exception.StackTrace, "OK", null, null);
      }

      private void App_SessionEnding(object sender, SessionEndingCancelEventArgs e)
      {
         Compendium compendium = DependencyResolver.Resolve<Compendium>();
         DataManager dataManager = DependencyResolver.Resolve<DataManager>();

         if (compendium != null)
         {
            compendium.SaveCharacters();
            compendium.SaveEncounters();
         }

         if (dataManager != null)
         {
            dataManager.SaveLaunchData();
         }
      }

      private void App_Exit(object sender, ExitEventArgs e)
      {
         Compendium compendium = DependencyResolver.Resolve<Compendium>();
         DataManager dataManager = DependencyResolver.Resolve<DataManager>();

         if (compendium != null)
         {
            compendium.SaveCharacters();
            compendium.SaveEncounters();
         }

         if (dataManager != null)
         {
            dataManager.SaveLaunchData();
         }
      }

      private void SetupDependencies()
      {
         DependencyResolver.RegisterInstance(_dialogService);
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<StringService>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<XMLImporter>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<XMLExporter>());
         DependencyResolver.RegisterInterface(typeof(IDataManager), typeof(DataManager));
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<DataManager>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<ThemeManager>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<SettingsViewModel>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<StatService>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<DiceService>());

         Compendium compendium = DependencyResolver.Resolve<Compendium>();
         compendium.LoadCompendium();
         DependencyResolver.RegisterInstance(compendium);

         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<CharacterSearchInput>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<AdventureSearchInput>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<EncounterSearchInput>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<LocationSearchInput>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<NPCSearchInput>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<TableSearchInput>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<BackgroundSearchInput>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<ClassSearchInput>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<ConditionSearchInput>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<FeatSearchInput>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<ItemSearchInput>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<MonsterSearchInput>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<RaceSearchInput>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<SpellSearchInput>());

         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<CharactersViewModel>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<AdventuresViewModel>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<EncountersViewModel>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<LocationsViewModel>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<NPCsViewModel>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<TablesViewModel>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<BackgroundsViewModel>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<ClassesViewModel>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<ConditionsViewModel>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<FeatsViewModel>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<ItemsViewModel>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<MonstersViewModel>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<RacesViewModel>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<SpellsViewModel>());
         DependencyResolver.RegisterInstance(DependencyResolver.Resolve<MainWindowViewModel>());
      }
   }
}
