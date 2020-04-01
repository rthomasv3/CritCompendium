using CritCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class WeaponProficiencyViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly WeaponProficiencyModel _weaponProficiencyModel;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="WeaponProficiencyViewModel"/>
      /// </summary>
      public WeaponProficiencyViewModel(WeaponProficiencyModel weaponProficiencyModel)
      {
         _weaponProficiencyModel = weaponProficiencyModel;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets simple weapons checked
      /// </summary>
      public bool SimpleWeaponsChecked
      {
         get { return _weaponProficiencyModel.SimpleWeaponsProficiency; }
         set
         {
            _weaponProficiencyModel.SimpleWeaponsProficiency = value;

            _weaponProficiencyModel.ClubProficiency = value;
            _weaponProficiencyModel.DaggerProficiency = value;
            _weaponProficiencyModel.GreatclubProficiency = value;
            _weaponProficiencyModel.HandaxeProficiency = value;
            _weaponProficiencyModel.JavelinProficiency = value;
            _weaponProficiencyModel.LightHammerProficiency = value;
            _weaponProficiencyModel.MaceProficiency = value;
            _weaponProficiencyModel.QuarterstaffProficiency = value;
            _weaponProficiencyModel.SickleProficiency = value;
            _weaponProficiencyModel.SpearProficiency = value;
            _weaponProficiencyModel.CrossbowLightProficiency = value;
            _weaponProficiencyModel.DartProficiency = value;
            _weaponProficiencyModel.ShortbowProficiency = value;
            _weaponProficiencyModel.SlingProficiency = value;

            OnPropertyChanged(nameof(SimpleWeaponsChecked));
            OnPropertyChanged(nameof(ClubChecked));
            OnPropertyChanged(nameof(DaggerChecked));
            OnPropertyChanged(nameof(GreatclubChecked));
            OnPropertyChanged(nameof(HandaxeChecked));
            OnPropertyChanged(nameof(JavelinChecked));
            OnPropertyChanged(nameof(LightHammerChecked));
            OnPropertyChanged(nameof(MaceChecked));
            OnPropertyChanged(nameof(QuarterstaffChecked));
            OnPropertyChanged(nameof(SickleChecked));
            OnPropertyChanged(nameof(SpearChecked));
            OnPropertyChanged(nameof(CrossbowLightChecked));
            OnPropertyChanged(nameof(DartChecked));
            OnPropertyChanged(nameof(ShortbowChecked));
            OnPropertyChanged(nameof(SlingChecked));
         }
      }

      /// <summary>
      /// Gets or sets martial weapons checked
      /// </summary>
      public bool MartialWeaponsChecked
      {
         get { return _weaponProficiencyModel.MartialWeaponsProficiency; }
         set
         {
            _weaponProficiencyModel.MartialWeaponsProficiency = value;

            _weaponProficiencyModel.BattleaxeProficiency = value;
            _weaponProficiencyModel.FlailProficiency = value;
            _weaponProficiencyModel.GlaiveProficiency = value;
            _weaponProficiencyModel.GreataxeProficiency = value;
            _weaponProficiencyModel.GreatswordProficiency = value;
            _weaponProficiencyModel.HalberdProficiency = value;
            _weaponProficiencyModel.LanceProficiency = value;
            _weaponProficiencyModel.LongswordProficiency = value;
            _weaponProficiencyModel.MaulProficiency = value;
            _weaponProficiencyModel.MorningstarProficiency = value;
            _weaponProficiencyModel.PikeProficiency = value;
            _weaponProficiencyModel.RapierProficiency = value;
            _weaponProficiencyModel.ScimitarProficiency = value;
            _weaponProficiencyModel.ShortswordProficiency = value;
            _weaponProficiencyModel.TridentProficiency = value;
            _weaponProficiencyModel.WarPickProficiency = value;
            _weaponProficiencyModel.WarhammerProficiency = value;
            _weaponProficiencyModel.WhipProficiency = value;
            _weaponProficiencyModel.BlowgunProficiency = value;
            _weaponProficiencyModel.CrossbowHandProficiency = value;
            _weaponProficiencyModel.CrossbowHeavyProficiency = value;
            _weaponProficiencyModel.LongbowProficiency = value;
            _weaponProficiencyModel.NetProficiency = value;

            OnPropertyChanged(nameof(MartialWeaponsChecked));
            OnPropertyChanged(nameof(BattleaxeChecked));
            OnPropertyChanged(nameof(FlailChecked));
            OnPropertyChanged(nameof(GlaiveChecked));
            OnPropertyChanged(nameof(GreataxeChecked));
            OnPropertyChanged(nameof(GreatswordChecked));
            OnPropertyChanged(nameof(HalberdChecked));
            OnPropertyChanged(nameof(LanceChecked));
            OnPropertyChanged(nameof(LongswordChecked));
            OnPropertyChanged(nameof(MaulChecked));
            OnPropertyChanged(nameof(MorningstarChecked));
            OnPropertyChanged(nameof(PikeChecked));
            OnPropertyChanged(nameof(RapierChecked));
            OnPropertyChanged(nameof(ScimitarChecked));
            OnPropertyChanged(nameof(ShortswordChecked));
            OnPropertyChanged(nameof(TridentChecked));
            OnPropertyChanged(nameof(WarPickChecked));
            OnPropertyChanged(nameof(WarhammerChecked));
            OnPropertyChanged(nameof(WhipChecked));
            OnPropertyChanged(nameof(BlowgunChecked));
            OnPropertyChanged(nameof(CrossbowHandChecked));
            OnPropertyChanged(nameof(CrossbowHeavyChecked));
            OnPropertyChanged(nameof(LongbowChecked));
            OnPropertyChanged(nameof(NetChecked));
         }
      }


      /// <summary>
      /// Gets or sets club checked
      /// </summary>
      public bool ClubChecked
      {
         get { return _weaponProficiencyModel.ClubProficiency; }
         set
         {
            _weaponProficiencyModel.ClubProficiency = value;

            if (AllSimpleWeaponsChecked())
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = true;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }
            else if (_weaponProficiencyModel.SimpleWeaponsProficiency)
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = false;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets dagger checked
      /// </summary>
      public bool DaggerChecked
      {
         get { return _weaponProficiencyModel.DaggerProficiency; }
         set
         {
            _weaponProficiencyModel.DaggerProficiency = value;

            if (AllSimpleWeaponsChecked())
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = true;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }
            else if (_weaponProficiencyModel.SimpleWeaponsProficiency)
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = false;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets greatclub checked
      /// </summary>
      public bool GreatclubChecked
      {
         get { return _weaponProficiencyModel.GreatclubProficiency; }
         set
         {
            _weaponProficiencyModel.GreatclubProficiency = value;

            if (AllSimpleWeaponsChecked())
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = true;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }
            else if (_weaponProficiencyModel.SimpleWeaponsProficiency)
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = false;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets handaxe checked
      /// </summary>
      public bool HandaxeChecked
      {
         get { return _weaponProficiencyModel.HandaxeProficiency; }
         set
         {
            _weaponProficiencyModel.HandaxeProficiency = value;

            if (AllSimpleWeaponsChecked())
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = true;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }
            else if (_weaponProficiencyModel.SimpleWeaponsProficiency)
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = false;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets javelin checked
      /// </summary>
      public bool JavelinChecked
      {
         get { return _weaponProficiencyModel.JavelinProficiency; }
         set
         {
            _weaponProficiencyModel.JavelinProficiency = value;

            if (AllSimpleWeaponsChecked())
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = true;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }
            else if (_weaponProficiencyModel.SimpleWeaponsProficiency)
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = false;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets light hammer checked
      /// </summary>
      public bool LightHammerChecked
      {
         get { return _weaponProficiencyModel.LightHammerProficiency; }
         set
         {
            _weaponProficiencyModel.LightHammerProficiency = value;

            if (AllSimpleWeaponsChecked())
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = true;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }
            else if (_weaponProficiencyModel.SimpleWeaponsProficiency)
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = false;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets mace checked
      /// </summary>
      public bool MaceChecked
      {
         get { return _weaponProficiencyModel.MaceProficiency; }
         set
         {
            _weaponProficiencyModel.MaceProficiency = value;

            if (AllSimpleWeaponsChecked())
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = true;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }
            else if (_weaponProficiencyModel.SimpleWeaponsProficiency)
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = false;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets quarterstaff checked
      /// </summary>
      public bool QuarterstaffChecked
      {
         get { return _weaponProficiencyModel.QuarterstaffProficiency; }
         set
         {
            _weaponProficiencyModel.QuarterstaffProficiency = value;

            if (AllSimpleWeaponsChecked())
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = true;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }
            else if (_weaponProficiencyModel.SimpleWeaponsProficiency)
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = false;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets sickle checked
      /// </summary>
      public bool SickleChecked
      {
         get { return _weaponProficiencyModel.SickleProficiency; }
         set
         {
            _weaponProficiencyModel.SickleProficiency = value;

            if (AllSimpleWeaponsChecked())
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = true;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }
            else if (_weaponProficiencyModel.SimpleWeaponsProficiency)
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = false;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets spear checked
      /// </summary>
      public bool SpearChecked
      {
         get { return _weaponProficiencyModel.SpearProficiency; }
         set
         {
            _weaponProficiencyModel.SpearProficiency = value;

            if (AllSimpleWeaponsChecked())
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = true;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }
            else if (_weaponProficiencyModel.SimpleWeaponsProficiency)
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = false;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets crossbow light checked
      /// </summary>
      public bool CrossbowLightChecked
      {
         get { return _weaponProficiencyModel.CrossbowLightProficiency; }
         set
         {
            _weaponProficiencyModel.CrossbowLightProficiency = value;

            if (AllSimpleWeaponsChecked())
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = true;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }
            else if (_weaponProficiencyModel.SimpleWeaponsProficiency)
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = false;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets dart checked
      /// </summary>
      public bool DartChecked
      {
         get { return _weaponProficiencyModel.DartProficiency; }
         set
         {
            _weaponProficiencyModel.DartProficiency = value;

            if (AllSimpleWeaponsChecked())
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = true;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }
            else if (_weaponProficiencyModel.SimpleWeaponsProficiency)
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = false;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets shortbow checked
      /// </summary>
      public bool ShortbowChecked
      {
         get { return _weaponProficiencyModel.ShortbowProficiency; }
         set
         {
            _weaponProficiencyModel.ShortbowProficiency = value;

            if (AllSimpleWeaponsChecked())
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = true;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }
            else if (_weaponProficiencyModel.SimpleWeaponsProficiency)
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = false;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets sling checked
      /// </summary>
      public bool SlingChecked
      {
         get { return _weaponProficiencyModel.SlingProficiency; }
         set
         {
            _weaponProficiencyModel.SlingProficiency = value;

            if (AllSimpleWeaponsChecked())
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = true;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }
            else if (_weaponProficiencyModel.SimpleWeaponsProficiency)
            {
               _weaponProficiencyModel.SimpleWeaponsProficiency = false;
               OnPropertyChanged(nameof(SimpleWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }


      /// <summary>
      /// Gets or sets battleaxe  checked
      /// </summary>
      public bool BattleaxeChecked
      {
         get { return _weaponProficiencyModel.BattleaxeProficiency; }
         set
         {
            _weaponProficiencyModel.BattleaxeProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets flail checked
      /// </summary>
      public bool FlailChecked
      {
         get { return _weaponProficiencyModel.FlailProficiency; }
         set
         {
            _weaponProficiencyModel.FlailProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets glaive checked
      /// </summary>
      public bool GlaiveChecked
      {
         get { return _weaponProficiencyModel.GlaiveProficiency; }
         set
         {
            _weaponProficiencyModel.GlaiveProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets greataxe checked
      /// </summary>
      public bool GreataxeChecked
      {
         get { return _weaponProficiencyModel.GreataxeProficiency; }
         set
         {
            _weaponProficiencyModel.GreataxeProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets greatsword checked
      /// </summary>
      public bool GreatswordChecked
      {
         get { return _weaponProficiencyModel.GreatswordProficiency; }
         set
         {
            _weaponProficiencyModel.GreatswordProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets halberd checked
      /// </summary>
      public bool HalberdChecked
      {
         get { return _weaponProficiencyModel.HalberdProficiency; }
         set
         {
            _weaponProficiencyModel.HalberdProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets lance checked
      /// </summary>
      public bool LanceChecked
      {
         get { return _weaponProficiencyModel.LanceProficiency; }
         set
         {
            _weaponProficiencyModel.LanceProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets longsword checked
      /// </summary>
      public bool LongswordChecked
      {
         get { return _weaponProficiencyModel.LongswordProficiency; }
         set
         {
            _weaponProficiencyModel.LongswordProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets maul checked
      /// </summary>
      public bool MaulChecked
      {
         get { return _weaponProficiencyModel.MaulProficiency; }
         set
         {
            _weaponProficiencyModel.MaulProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets morningstar checked
      /// </summary>
      public bool MorningstarChecked
      {
         get { return _weaponProficiencyModel.MorningstarProficiency; }
         set
         {
            _weaponProficiencyModel.MorningstarProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets pike checked
      /// </summary>
      public bool PikeChecked
      {
         get { return _weaponProficiencyModel.PikeProficiency; }
         set
         {
            _weaponProficiencyModel.PikeProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets rapier checked
      /// </summary>
      public bool RapierChecked
      {
         get { return _weaponProficiencyModel.RapierProficiency; }
         set
         {
            _weaponProficiencyModel.RapierProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets scimitar checked
      /// </summary>
      public bool ScimitarChecked
      {
         get { return _weaponProficiencyModel.ScimitarProficiency; }
         set
         {
            _weaponProficiencyModel.ScimitarProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets shortsword checked
      /// </summary>
      public bool ShortswordChecked
      {
         get { return _weaponProficiencyModel.ShortswordProficiency; }
         set
         {
            _weaponProficiencyModel.ShortswordProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets trident checked
      /// </summary>
      public bool TridentChecked
      {
         get { return _weaponProficiencyModel.TridentProficiency; }
         set
         {
            _weaponProficiencyModel.TridentProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets war pick checked
      /// </summary>
      public bool WarPickChecked
      {
         get { return _weaponProficiencyModel.WarPickProficiency; }
         set
         {
            _weaponProficiencyModel.WarPickProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets warhammer checked
      /// </summary>
      public bool WarhammerChecked
      {
         get { return _weaponProficiencyModel.WarhammerProficiency; }
         set
         {
            _weaponProficiencyModel.WarhammerProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets whip checked
      /// </summary>
      public bool WhipChecked
      {
         get { return _weaponProficiencyModel.WhipProficiency; }
         set
         {
            _weaponProficiencyModel.WhipProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets blowgun checked
      /// </summary>
      public bool BlowgunChecked
      {
         get { return _weaponProficiencyModel.BlowgunProficiency; }
         set
         {
            _weaponProficiencyModel.BlowgunProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets crossbow hand checked
      /// </summary>
      public bool CrossbowHandChecked
      {
         get { return _weaponProficiencyModel.CrossbowHandProficiency; }
         set
         {
            _weaponProficiencyModel.CrossbowHandProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets crossbow heavy checked
      /// </summary>
      public bool CrossbowHeavyChecked
      {
         get { return _weaponProficiencyModel.CrossbowHeavyProficiency; }
         set
         {
            _weaponProficiencyModel.CrossbowHeavyProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets longbow checked
      /// </summary>
      public bool LongbowChecked
      {
         get { return _weaponProficiencyModel.LongbowProficiency; }
         set
         {
            _weaponProficiencyModel.LongbowProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      /// <summary>
      /// Gets or sets net checked
      /// </summary>
      public bool NetChecked
      {
         get { return _weaponProficiencyModel.NetProficiency; }
         set
         {
            _weaponProficiencyModel.NetProficiency = value;

            if (AllMartialWeaponsChecked())
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = true;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }
            else if (_weaponProficiencyModel.MartialWeaponsProficiency)
            {
               _weaponProficiencyModel.MartialWeaponsProficiency = false;
               OnPropertyChanged(nameof(MartialWeaponsChecked));
            }

            OnPropertyChanged();
         }
      }

      #endregion

      #region Private Methods

      private bool AllSimpleWeaponsChecked()
      {
         return _weaponProficiencyModel.ClubProficiency &&
                _weaponProficiencyModel.DaggerProficiency &&
                _weaponProficiencyModel.GreatclubProficiency &&
                _weaponProficiencyModel.HandaxeProficiency &&
                _weaponProficiencyModel.JavelinProficiency &&
                _weaponProficiencyModel.LightHammerProficiency &&
                _weaponProficiencyModel.MaceProficiency &&
                _weaponProficiencyModel.QuarterstaffProficiency &&
                _weaponProficiencyModel.SickleProficiency &&
                _weaponProficiencyModel.SpearProficiency &&
                _weaponProficiencyModel.CrossbowLightProficiency &&
                _weaponProficiencyModel.DartProficiency &&
                _weaponProficiencyModel.ShortbowProficiency &&
                _weaponProficiencyModel.SlingProficiency;
      }

      private bool AllMartialWeaponsChecked()
      {
         return _weaponProficiencyModel.BattleaxeProficiency &&
                _weaponProficiencyModel.FlailProficiency &&
                _weaponProficiencyModel.GlaiveProficiency &&
                _weaponProficiencyModel.GreataxeProficiency &&
                _weaponProficiencyModel.GreatswordProficiency &&
                _weaponProficiencyModel.HalberdProficiency &&
                _weaponProficiencyModel.LanceProficiency &&
                _weaponProficiencyModel.LongswordProficiency &&
                _weaponProficiencyModel.MaulProficiency &&
                _weaponProficiencyModel.MorningstarProficiency &&
                _weaponProficiencyModel.PikeProficiency &&
                _weaponProficiencyModel.RapierProficiency &&
                _weaponProficiencyModel.ScimitarProficiency &&
                _weaponProficiencyModel.ShortswordProficiency &&
                _weaponProficiencyModel.TridentProficiency &&
                _weaponProficiencyModel.WarPickProficiency &&
                _weaponProficiencyModel.WarhammerProficiency &&
                _weaponProficiencyModel.WhipProficiency &&
                _weaponProficiencyModel.BlowgunProficiency &&
                _weaponProficiencyModel.CrossbowHandProficiency &&
                _weaponProficiencyModel.CrossbowHeavyProficiency &&
                _weaponProficiencyModel.LongbowProficiency &&
                _weaponProficiencyModel.NetProficiency;
      }

      #endregion
   }
}
