using System;
using System.IO;
using System.Reflection;
using CodeCompendium.BinarySerialization;
using CritCompendiumInfrastructure.Persistence;

namespace CritCompendiumInfrastructure.Business
{
   /// <summary>
   /// Class used to save and load data.
   /// </summary>
   public sealed class DataManager
   {
      #region Fields

      private static readonly string _launchSaveFileName = "compendium.data";
      private static readonly string _compendiumSaveFileName = "compendium.data";

      private readonly BinarySerializer _binarySerializer;
      private readonly string _saveDataFolder;
      private int _runCount;
      private DateTime _lastLaunchDate;
      private string _lastVersionLaunched;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of the <see cref="DataManager"/> class.
      /// </summary>
      public DataManager(BinarySerializer binarySerializer)
      {
         _binarySerializer = binarySerializer ?? throw new ArgumentNullException(nameof(binarySerializer));

         _saveDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CritCompendium");

         LoadLaunchData();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Returns true if it's the first time the program was launched (no app data found).
      /// </summary>
      public bool FirstLaunch
      {
         get { return _runCount == 0; }
      }

      /// <summary>
      /// Gets the run count.
      /// </summary>
      public int RunCount
      {
         get { return _runCount; }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Saves the compendium to the specified location (or default location if not provided).
      /// </summary>
      public bool SaveCompendium(Compendium compendium, string fileLocation = null)
      {
         if (compendium == null)
         {
            throw new ArgumentNullException(nameof(compendium));
         }

         if (String.IsNullOrWhiteSpace(fileLocation))
         {
            fileLocation = Path.Combine(_saveDataFolder, _compendiumSaveFileName);
         }

         CompendiumRecord compendiumRecord = AssembleCompendiumRecord(compendium);
         byte[] bytes = _binarySerializer.Serialize(compendiumRecord);
         File.WriteAllBytes(fileLocation, bytes);

         return bytes.Length > 0 && File.Exists(fileLocation);
      }

      /// <summary>
      /// Loads the compendium at the specified location (or default location if not provided).
      /// </summary>
      public Compendium LoadCompendium(string fileLocation = null)
      {
         Compendium compendium = null;

         if (String.IsNullOrWhiteSpace(fileLocation))
         {
            fileLocation = Path.Combine(_saveDataFolder, _compendiumSaveFileName);
         }

         if (File.Exists(fileLocation))
         {
            byte[] bytes = File.ReadAllBytes(fileLocation);
            CompendiumRecord compendiumRecord = _binarySerializer.Deserialize<CompendiumRecord>(bytes);
            compendium = AssembleCompendium(compendiumRecord);
         }

         return compendium;
      }

      /// <summary>
      /// Saves the application launch data.
      /// </summary>
      public void SaveLaunchData()
      {
         LaunchRecord launchRecord = new LaunchRecord()
         {
            RunCount = _runCount,
            LastLaunchDate = _lastLaunchDate,
            LastVersionLaunched = _lastVersionLaunched
         };

         byte[] bytes = _binarySerializer.Serialize(launchRecord);
         File.WriteAllBytes(Path.Combine(_saveDataFolder, _launchSaveFileName), bytes);
      }

      #endregion

      #region Private Methods

      private void LoadLaunchData()
      {
         string fileLocation = Path.Combine(_saveDataFolder, _launchSaveFileName);

         if (File.Exists(fileLocation))
         {
            byte[] bytes = File.ReadAllBytes(fileLocation);
            LaunchRecord launchRecord = _binarySerializer.Deserialize<LaunchRecord>(bytes);

            _runCount = launchRecord.RunCount;
            _lastLaunchDate = launchRecord.LastLaunchDate;
            _lastVersionLaunched = launchRecord.LastVersionLaunched;
         }
         else
         {
            _runCount = 0;
            _lastLaunchDate = DateTime.Now;
            _lastVersionLaunched = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            SaveLaunchData();
         }
      }

      private CompendiumRecord AssembleCompendiumRecord(Compendium compendium)
      {
         CompendiumRecord compendiumRecord = new CompendiumRecord();



         return compendiumRecord;
      }

      private Compendium AssembleCompendium(CompendiumRecord compendiumRecord)
      {
         return null;
      }

      #endregion
   }
}
