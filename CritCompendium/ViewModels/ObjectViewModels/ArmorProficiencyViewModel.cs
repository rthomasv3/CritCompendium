using CritCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class ArmorProficiencyViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly ArmorProficiencyModel _armorProficiencyModel;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="ArmorProficiencyViewModel"/>
      /// </summary>
      public ArmorProficiencyViewModel(ArmorProficiencyModel armorProficiencyModel)
      {
         _armorProficiencyModel = armorProficiencyModel;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets light checked
      /// </summary>
      public bool LightChecked
      {
         get { return _armorProficiencyModel.LightArmorProficiency; }
         set
         {
            _armorProficiencyModel.LightArmorProficiency = value;

            _armorProficiencyModel.PaddedProficiency = value;
            _armorProficiencyModel.LeatherProficiency = value;
            _armorProficiencyModel.StuddedLeatherProficiency = value;

            OnPropertyChanged(nameof(LightChecked));
            OnPropertyChanged(nameof(PaddedChecked));
            OnPropertyChanged(nameof(LeatherChecked));
            OnPropertyChanged(nameof(StuddedLeatherChecked));
         }
      }

      /// <summary>
      /// Gets or sets medium checked
      /// </summary>
      public bool MediumChecked
      {
         get { return _armorProficiencyModel.MediumArmorProficiency; }
         set
         {
            _armorProficiencyModel.MediumArmorProficiency = value;

            _armorProficiencyModel.HideProficiency = value;
            _armorProficiencyModel.ChainShirtProficiency = value;
            _armorProficiencyModel.ScaleMailProficiency = value;
            _armorProficiencyModel.BreastplateProficiency = value;
            _armorProficiencyModel.HalfPlateProficiency = value;

            OnPropertyChanged(nameof(MediumChecked));
            OnPropertyChanged(nameof(HideChecked));
            OnPropertyChanged(nameof(ChainShirtChecked));
            OnPropertyChanged(nameof(ScaleMailChecked));
            OnPropertyChanged(nameof(BreastplateChecked));
            OnPropertyChanged(nameof(HalfPlateChecked));
         }
      }

      /// <summary>
      /// Gets or sets heavy checked
      /// </summary>
      public bool HeavyChecked
      {
         get { return _armorProficiencyModel.HeavyArmorProficiency; }
         set
         {
            _armorProficiencyModel.HeavyArmorProficiency = value;

            _armorProficiencyModel.RingMailProficiency = value;
            _armorProficiencyModel.ChainMailProficiency = value;
            _armorProficiencyModel.SplintProficiency = value;
            _armorProficiencyModel.PlateProficiency = value;

            OnPropertyChanged(nameof(HeavyChecked));
            OnPropertyChanged(nameof(RingMailChecked));
            OnPropertyChanged(nameof(ChainMailChecked));
            OnPropertyChanged(nameof(SplintChecked));
            OnPropertyChanged(nameof(PlateChecked));
         }
      }

      /// <summary>
      /// Gets or sets shields checked
      /// </summary>
      public bool ShieldsChecked
      {
         get { return _armorProficiencyModel.ShieldsProficiency; }
         set
         {
            _armorProficiencyModel.ShieldsProficiency = value;

            _armorProficiencyModel.ShieldProficiency = value;

            OnPropertyChanged(nameof(ShieldsChecked));
            OnPropertyChanged(nameof(ShieldChecked));
         }
      }


      /// <summary>
      /// Gets or sets padded checked
      /// </summary>
      public bool PaddedChecked
      {
         get { return _armorProficiencyModel.PaddedProficiency; }
         set
         {
            _armorProficiencyModel.PaddedProficiency = value;

            if (AllLightChecked())
            {
               _armorProficiencyModel.LightArmorProficiency = true;
               OnPropertyChanged(nameof(LightChecked));
            }
            else if (_armorProficiencyModel.LightArmorProficiency)
            {
               _armorProficiencyModel.LightArmorProficiency = false;
               OnPropertyChanged(nameof(LightChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets leather checked
      /// </summary>
      public bool LeatherChecked
      {
         get { return _armorProficiencyModel.LeatherProficiency; }
         set
         {
            _armorProficiencyModel.LeatherProficiency = value;

            if (AllLightChecked())
            {
               _armorProficiencyModel.LightArmorProficiency = true;
               OnPropertyChanged(nameof(LightChecked));
            }
            else if (_armorProficiencyModel.LightArmorProficiency)
            {
               _armorProficiencyModel.LightArmorProficiency = false;
               OnPropertyChanged(nameof(LightChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets studded leather checked
      /// </summary>
      public bool StuddedLeatherChecked
      {
         get { return _armorProficiencyModel.StuddedLeatherProficiency; }
         set
         {
            _armorProficiencyModel.StuddedLeatherProficiency = value;

            if (AllLightChecked())
            {
               _armorProficiencyModel.LightArmorProficiency = true;
               OnPropertyChanged(nameof(LightChecked));
            }
            else if (_armorProficiencyModel.LightArmorProficiency)
            {
               _armorProficiencyModel.LightArmorProficiency = false;
               OnPropertyChanged(nameof(LightChecked));
            }

            OnPropertyChanged();
         }
      }


      /// <summary>
      /// Gets or sets hide checked
      /// </summary>
      public bool HideChecked
      {
         get { return _armorProficiencyModel.HideProficiency; }
         set
         {
            _armorProficiencyModel.HideProficiency = value;

            if (AllMediumChecked())
            {
               _armorProficiencyModel.MediumArmorProficiency = true;
               OnPropertyChanged(nameof(MediumChecked));
            }
            else if (_armorProficiencyModel.MediumArmorProficiency)
            {
               _armorProficiencyModel.MediumArmorProficiency = false;
               OnPropertyChanged(nameof(MediumChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets chain shirt checked
      /// </summary>
      public bool ChainShirtChecked
      {
         get { return _armorProficiencyModel.ChainShirtProficiency; }
         set
         {
            _armorProficiencyModel.ChainShirtProficiency = value;

            if (AllMediumChecked())
            {
               _armorProficiencyModel.MediumArmorProficiency = true;
               OnPropertyChanged(nameof(MediumChecked));
            }
            else if (_armorProficiencyModel.MediumArmorProficiency)
            {
               _armorProficiencyModel.MediumArmorProficiency = false;
               OnPropertyChanged(nameof(MediumChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets scale mail checked
      /// </summary>
      public bool ScaleMailChecked
      {
         get { return _armorProficiencyModel.ScaleMailProficiency; }
         set
         {
            _armorProficiencyModel.ScaleMailProficiency = value;

            if (AllMediumChecked())
            {
               _armorProficiencyModel.MediumArmorProficiency = true;
               OnPropertyChanged(nameof(MediumChecked));
            }
            else if (_armorProficiencyModel.MediumArmorProficiency)
            {
               _armorProficiencyModel.MediumArmorProficiency = false;
               OnPropertyChanged(nameof(MediumChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets breastplate checked
      /// </summary>
      public bool BreastplateChecked
      {
         get { return _armorProficiencyModel.BreastplateProficiency; }
         set
         {
            _armorProficiencyModel.BreastplateProficiency = value;

            if (AllMediumChecked())
            {
               _armorProficiencyModel.MediumArmorProficiency = true;
               OnPropertyChanged(nameof(MediumChecked));
            }
            else if (_armorProficiencyModel.MediumArmorProficiency)
            {
               _armorProficiencyModel.MediumArmorProficiency = false;
               OnPropertyChanged(nameof(MediumChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets half plate checked
      /// </summary>
      public bool HalfPlateChecked
      {
         get { return _armorProficiencyModel.HalfPlateProficiency; }
         set
         {
            _armorProficiencyModel.HalfPlateProficiency = value;

            if (AllMediumChecked())
            {
               _armorProficiencyModel.MediumArmorProficiency = true;
               OnPropertyChanged(nameof(MediumChecked));
            }
            else if (_armorProficiencyModel.MediumArmorProficiency)
            {
               _armorProficiencyModel.MediumArmorProficiency = false;
               OnPropertyChanged(nameof(MediumChecked));
            }

            OnPropertyChanged();
         }
      }


      /// <summary>
      /// Gets or sets ring mail checked
      /// </summary>
      public bool RingMailChecked
      {
         get { return _armorProficiencyModel.RingMailProficiency; }
         set
         {
            _armorProficiencyModel.RingMailProficiency = value;

            if (AllHeavyChecked())
            {
               _armorProficiencyModel.HeavyArmorProficiency = true;
               OnPropertyChanged(nameof(HeavyChecked));
            }
            else if (_armorProficiencyModel.HeavyArmorProficiency)
            {
               _armorProficiencyModel.HeavyArmorProficiency = false;
               OnPropertyChanged(nameof(HeavyChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets chain mail checked
      /// </summary>
      public bool ChainMailChecked
      {
         get { return _armorProficiencyModel.ChainMailProficiency; }
         set
         {
            _armorProficiencyModel.ChainMailProficiency = value;

            if (AllHeavyChecked())
            {
               _armorProficiencyModel.HeavyArmorProficiency = true;
               OnPropertyChanged(nameof(HeavyChecked));
            }
            else if (_armorProficiencyModel.HeavyArmorProficiency)
            {
               _armorProficiencyModel.HeavyArmorProficiency = false;
               OnPropertyChanged(nameof(HeavyChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets splint checked
      /// </summary>
      public bool SplintChecked
      {
         get { return _armorProficiencyModel.SplintProficiency; }
         set
         {
            _armorProficiencyModel.SplintProficiency = value;

            if (AllHeavyChecked())
            {
               _armorProficiencyModel.HeavyArmorProficiency = true;
               OnPropertyChanged(nameof(HeavyChecked));
            }
            else if (_armorProficiencyModel.HeavyArmorProficiency)
            {
               _armorProficiencyModel.HeavyArmorProficiency = false;
               OnPropertyChanged(nameof(HeavyChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets plate checked
      /// </summary>
      public bool PlateChecked
      {
         get { return _armorProficiencyModel.PlateProficiency; }
         set
         {
            _armorProficiencyModel.PlateProficiency = value;

            if (AllHeavyChecked())
            {
               _armorProficiencyModel.HeavyArmorProficiency = true;
               OnPropertyChanged(nameof(HeavyChecked));
            }
            else if (_armorProficiencyModel.HeavyArmorProficiency)
            {
               _armorProficiencyModel.HeavyArmorProficiency = false;
               OnPropertyChanged(nameof(HeavyChecked));
            }

            OnPropertyChanged();
         }
      }


      /// <summary>
      /// Gets or sets shield checked
      /// </summary>
      public bool ShieldChecked
      {
         get { return _armorProficiencyModel.ShieldProficiency; }
         set
         {
            _armorProficiencyModel.ShieldProficiency = value;

            if (AllShieldsChecked())
            {
               _armorProficiencyModel.ShieldsProficiency = true;
               OnPropertyChanged(nameof(ShieldsChecked));
            }
            else if (_armorProficiencyModel.ShieldsProficiency)
            {
               _armorProficiencyModel.ShieldsProficiency = false;
               OnPropertyChanged(nameof(ShieldsChecked));
            }

            OnPropertyChanged();
         }
      }

      #endregion

      #region Private Methods

      private bool AllLightChecked()
      {
         return _armorProficiencyModel.PaddedProficiency && _armorProficiencyModel.LeatherProficiency &&
                _armorProficiencyModel.StuddedLeatherProficiency;
      }

      private bool AllMediumChecked()
      {
         return _armorProficiencyModel.HideProficiency && _armorProficiencyModel.ChainShirtProficiency &&
                _armorProficiencyModel.ScaleMailProficiency && _armorProficiencyModel.BreastplateProficiency &&
                _armorProficiencyModel.HalfPlateProficiency;
      }

      private bool AllHeavyChecked()
      {
         return _armorProficiencyModel.RingMailProficiency && _armorProficiencyModel.ChainMailProficiency &&
               _armorProficiencyModel.SplintProficiency && _armorProficiencyModel.PlateProficiency;
      }

      private bool AllShieldsChecked()
      {
         return _armorProficiencyModel.ShieldProficiency;
      }

      #endregion
   }
}
