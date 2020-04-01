using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using CritCompendium.Services;

namespace CritCompendium.ViewModels
{
   public sealed class AboutViewModel
   {
      #region Fields

      private readonly DialogService _dialogService;
      private readonly string _version;

      private readonly ICommand _viewSRDLicenseCommand;
      private readonly ICommand _viewDocXLicenseCommand;
      private readonly ICommand _viewFacepunchLicenseCommand;
      private readonly ICommand _viewSpireLicenseCommand;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="AboutViewModel"/>
      /// </summary>
      public AboutViewModel(DialogService dialogService)
      {
         _dialogService = dialogService;

         _viewSRDLicenseCommand = new RelayCommand(obj => true, obj => ViewSRDLicense());
         _viewDocXLicenseCommand = new RelayCommand(obj => true, obj => ViewDocXLicense());
         _viewFacepunchLicenseCommand = new RelayCommand(obj => true, obj => ViewFacepunchLicense());
         _viewSpireLicenseCommand = new RelayCommand(obj => true, obj => ViewSpireLicense());

         _version = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets version string
      /// </summary>
      public string Version
      {
         get { return $"v {_version} (5e)"; }
      }

      /// <summary>
      /// Gets view srd license command
      /// </summary>
      public ICommand ViewSRDLicenseCommand
      {
         get { return _viewSRDLicenseCommand; }
      }

      /// <summary>
      /// Gets view docx license command
      /// </summary>
      public ICommand ViewDocXLicenseCommand
      {
         get { return _viewDocXLicenseCommand; }
      }

      /// <summary>
      /// Gets view facepunch license command
      /// </summary>
      public ICommand ViewFacepunchLicenseCommand
      {
         get { return _viewFacepunchLicenseCommand; }
      }

      /// <summary>
      /// Gets view spire license command
      /// </summary>
      public ICommand ViewSpireLicenseCommand
      {
         get { return _viewSpireLicenseCommand; }
      }

      #endregion

      #region Private Methods

      private void ViewSRDLicense()
      {
         using (Stream stream = GetType().Assembly.GetManifestResourceStream("CritCompendium.Resources.Licenses.v5.1_SRD.txt"))
         {
            using (StreamReader sr = new StreamReader(stream))
            {
               string licenseText = sr.ReadToEnd();
               _dialogService.ShowConfirmationDialog("v5.1 SRD License", licenseText, "OK", null, null);
            }
         }
      }

      private void ViewDocXLicense()
      {
         using (Stream stream = GetType().Assembly.GetManifestResourceStream("CritCompendium.Resources.Licenses.docx.txt"))
         {
            using (StreamReader sr = new StreamReader(stream))
            {
               string licenseText = sr.ReadToEnd();
               _dialogService.ShowConfirmationDialog("DocX License", licenseText, "OK", null, null);
            }
         }
      }

      private void ViewFacepunchLicense()
      {
         using (Stream stream = GetType().Assembly.GetManifestResourceStream("CritCompendium.Resources.Licenses.facepunch.steamworks.txt"))
         {
            using (StreamReader sr = new StreamReader(stream))
            {
               string licenseText = sr.ReadToEnd();
               _dialogService.ShowConfirmationDialog("Facepunch.Steamworks License", licenseText, "OK", null, null);
            }
         }
      }

      private void ViewSpireLicense()
      {
         using (Stream stream = GetType().Assembly.GetManifestResourceStream("CritCompendium.Resources.Licenses.freespire.pdf.txt"))
         {
            using (StreamReader sr = new StreamReader(stream))
            {
               string licenseText = sr.ReadToEnd();
               _dialogService.ShowConfirmationDialog("FreeSpire.PDF License", licenseText, "OK", null, null);
            }
         }
      }

      #endregion
   }
}
