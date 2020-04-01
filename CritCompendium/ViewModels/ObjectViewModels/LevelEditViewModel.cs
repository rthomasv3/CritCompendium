using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;
using CritCompendium.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
	public sealed class LevelEditViewModel : NotifyPropertyChanged
	{
		#region Events

		public event EventHandler ClassChanged;

		#endregion

		#region Fields

		private readonly Compendium _compendium = DependencyResolver.Resolve<Compendium>();
		private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();
		private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();

		private readonly LevelModel _levelModel;
		private int _level;
		private int _levelOfClass;
		private Guid _class;
		private List<Tuple<Guid, string>> _classes = new List<Tuple<Guid, string>>();
		private ObservableCollection<FeatureViewModel> _features = new ObservableCollection<FeatureViewModel>();
		private List<Tuple<Guid, string>> _availableFeats = new List<Tuple<Guid, string>>();
		private ObservableCollection<FeatEditViewModel> _feats = new ObservableCollection<FeatEditViewModel>();
		private int _hitDieResult;
		private bool _hitDieUsed;
		private int _additionalHP;
		private bool _canDelete;
		private ObservableCollection<AbilityScoreEditViewModel> _abilityScoreImprovements = new ObservableCollection<AbilityScoreEditViewModel>();
		private readonly List<Tuple<Ability, string>> _abilities = new List<Tuple<Ability, string>>();
		private readonly List<int> _abilityValues = new List<int> { 1, 2 };

		private readonly RelayCommand _viewClassCommand;
		private readonly RelayCommand _viewFeatureCommand;
		private readonly RelayCommand _viewFeatCommand;
		private readonly RelayCommand _addAbilityScoreCommand;
		private readonly RelayCommand _deleteAbilityScoreCommand;
		private readonly RelayCommand _addFeatCommand;
		private readonly RelayCommand _deleteFeatCommand;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates and instance of <see cref="LevelEditViewModel"/>
		/// </summary>
		public LevelEditViewModel(LevelModel levelModel)
		{
			_levelModel = levelModel;

			_level = _levelModel.Level;
			_levelOfClass = _levelModel.LevelOfClass;

			foreach (ClassModel classModel in _compendium.Classes)
			{
				_classes.Add(new Tuple<Guid, string>(classModel.ID, classModel.Name));
			}

			_class = _levelModel.Class.ID;

			foreach (FeatModel featModel in _levelModel.Feats)
			{
				_feats.Add(new FeatEditViewModel(featModel));
			}

			_hitDieResult = _levelModel.HitDieResult;
			_hitDieUsed = _levelModel.HitDieUsed;
			_additionalHP = _levelModel.AdditionalHP;

			foreach (KeyValuePair<Ability, int> abilityScore in _levelModel.AbilityScoreImprovements)
			{
				_abilityScoreImprovements.Add(new AbilityScoreEditViewModel(abilityScore.Key, abilityScore.Value));
			}

			foreach (Ability a in Enum.GetValues(typeof(Ability)))
			{
				if (a != Ability.None)
				{
					_abilities.Add(new Tuple<Ability, string>(a, _stringService.GetString(a)));
				}
			}

			_viewClassCommand = new RelayCommand(obj => true, obj => ViewClass());
			_viewFeatureCommand = new RelayCommand(obj => true, obj => ViewFeature((FeatureViewModel)obj));
			_viewFeatCommand = new RelayCommand(obj => true, obj => ViewFeat((FeatEditViewModel)obj));
			_addAbilityScoreCommand = new RelayCommand(obj => true, obj => AddAbilityScore());
			_deleteAbilityScoreCommand = new RelayCommand(obj => true, obj => DeleteAbilityScore((AbilityScoreEditViewModel)obj));
			_addFeatCommand = new RelayCommand(obj => true, obj => AddFeat());
			_deleteFeatCommand = new RelayCommand(obj => true, obj => DeleteFeat((FeatEditViewModel)obj));

			foreach (FeatModel featModel in _compendium.Feats)
			{
				_availableFeats.Add(new Tuple<Guid, string>(featModel.ID, featModel.Name));
			}

            UpdateFeatures();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets level model
		/// </summary>
		public LevelModel LevelModel
		{
			get { return _levelModel; }
		}

		/// <summary>
		/// Gets or sets level
		/// </summary>
		public int Level
		{
			get { return _level; }
			set
			{
				if (Set(ref _level, value))
				{
					_levelModel.Level = _level;
				}
			}
		}

		/// <summary>
		/// Gets or sets level or current class
		/// </summary>
		public int LevelOfClass
		{
			get { return _levelOfClass; }
			set
			{
				Set(ref _levelOfClass, value);

				UpdateFeatures();

				OnPropertyChanged(nameof(Features));
			}
		}

		/// <summary>
		/// Gets or sets class
		/// </summary>
		public Tuple<Guid, string> Class
		{
			get { return _classes.FirstOrDefault(x => x.Item1 == _class); }
			set
			{
				if (Set(ref _class, value.Item1))
				{
					_levelModel.Class = _compendium.Classes.FirstOrDefault(x => x.ID == _class);
					if (_levelModel.Class != null)
                    {
                        _levelModel.Features.Clear();
                        _hitDieResult = _level == 1 ? _levelModel.Class.HitDie : (_levelModel.Class.HitDie / 2) + 1;
                        _levelModel.HitDieResult = _hitDieResult;

                        OnPropertyChanged(nameof(HitDieResult));
						OnPropertyChanged(nameof(HitDie));
					}
					ClassChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Gets classes
		/// </summary>
		public IEnumerable<Tuple<Guid, string>> Classes
		{
			get { return _classes; }
		}

		/// <summary>
		/// Gets or sets features
		/// </summary>
		public IEnumerable<FeatureViewModel> Features
		{
			get { return _features; }
		}

		/// <summary>
		/// Gets or sets feats
		/// </summary>
		public IEnumerable<FeatEditViewModel> Feats
		{
			get { return _feats; }
		}

		/// <summary>
		/// Gets available feats
		/// </summary>
		public IEnumerable<Tuple<Guid, string>> AvailableFeats
		{
			get { return _availableFeats; }
		}

		/// <summary>
		/// Gets hit die
		/// </summary>
		public string HitDie
		{
			get { return _levelModel.Class != null ? "d" + _levelModel.Class.HitDie.ToString() : "Unknown"; }
		}

		/// <summary>
		/// Gets or sets hit die result
		/// </summary>
		public int HitDieResult
		{
			get { return _hitDieResult; }
			set
			{
				if (Set(ref _hitDieResult, value))
				{
					_levelModel.HitDieResult = _hitDieResult;
				}
			}
		}

		/// <summary>
		/// Gets or sets hit die used
		/// </summary>
		public bool HitDieUsed
		{
			get { return _hitDieUsed; }
			set
			{
				if (Set(ref _hitDieUsed, value))
				{
					_levelModel.HitDieUsed = _hitDieUsed;
				}
			}
		}

		/// <summary>
		/// Gets or sets additional HP
		/// </summary>
		public int AdditionalHP
		{
			get { return _additionalHP; }
			set
			{
				if (Set(ref _additionalHP, value))
				{
					_levelModel.AdditionalHP = _additionalHP;
				}
			}
		}

		/// <summary>
		/// Gets or sets ability score improvements
		/// </summary>
		public IEnumerable<AbilityScoreEditViewModel> AbilityScoreImprovements
		{
			get { return _abilityScoreImprovements; }
		}

		/// <summary>
		/// Gets abilities
		/// </summary>
		public IEnumerable<Tuple<Ability, string>> Abilities
		{
			get { return _abilities; }
		}

		/// <summary>
		/// Gets ability values
		/// </summary>
		public IEnumerable<int> AbilityValues
		{
			get { return _abilityValues; }
		}

		/// <summary>
		/// Gets or sets can delete
		/// </summary>
		public bool CanDelete
		{
			get { return _canDelete; }
			set { Set(ref _canDelete, value); }
		}

		/// <summary>
		/// Gets view class command
		/// </summary>
		public ICommand ViewClassCommand
		{
			get { return _viewClassCommand; }
		}

		/// <summary>
		/// Gets view feature command
		/// </summary>
		public ICommand ViewFeatureCommand
		{
			get { return _viewFeatureCommand; }
		}

		/// <summary>
		/// Gets view feat command
		/// </summary>
		public ICommand ViewFeatCommand
		{
			get { return _viewFeatCommand; }
		}

		/// <summary>
		/// Gets add ability score command
		/// </summary>
		public ICommand AddAbilityScoreCommand
		{
			get { return _addAbilityScoreCommand; }
		}

		/// <summary>
		/// Gets delete ability score command
		/// </summary>
		public ICommand DeleteAbilityScoreCommand
		{
			get { return _deleteAbilityScoreCommand; }
		}

		/// <summary>
		/// Gets add feat command
		/// </summary>
		public ICommand AddFeatCommand
		{
			get { return _addFeatCommand; }
		}

		/// <summary>
		/// Gets delete feat command
		/// </summary>
		public ICommand DeleteFeatCommand
		{
			get { return _deleteFeatCommand; }
		}

		#endregion

		#region Private Methods

		private void ViewClass()
		{
			_dialogService.ShowDetailsDialog(new ClassViewModel(_levelModel.Class));
		}

		private void ViewFeature(FeatureViewModel featureView)
		{
			_dialogService.ShowDetailsDialog(featureView);
		}

		private void ViewFeat(FeatEditViewModel featEditView)
		{
			_dialogService.ShowDetailsDialog(new FeatViewModel(_compendium.Feats.First(x => x.ID == featEditView.SelectedFeat.Item1)));
		}

        private void UpdateFeatures()
        {
            _features.Clear();
            foreach (AutoLevelModel autoLevelModel in _levelModel.Class.AutoLevels.Where(x => x.Level == _levelOfClass))
            {
                foreach (FeatureModel featureModel in autoLevelModel.Features)
                {
                    FeatureViewModel featureView = new FeatureViewModel(featureModel);

                    if (_levelModel.Features.Any(x => x.ID == featureModel.ID))
                    {
                        featureView.IsSelected = true;
                    }
                    else if (featureView.IsSelected)
                    {
                        _levelModel.Features.Add(featureModel);
                    }

                    featureView.IsSelectedChanged += FeatureView_IsSelectedChanged;
                    _features.Add(featureView);
                }
            }
        }

		private void AddAbilityScore()
		{
			AbilityScoreEditViewModel abilityScoreEditView = new AbilityScoreEditViewModel(Ability.Strength, 1);
			abilityScoreEditView.SelectionChanged += AbilityScoreEditView_SelectionChanged;
			_abilityScoreImprovements.Add(abilityScoreEditView);

			UpdateModelAbilityScores();

			OnPropertyChanged(nameof(AbilityScoreImprovements));
		}

		private void DeleteAbilityScore(AbilityScoreEditViewModel abilityScore)
		{
			_abilityScoreImprovements.Remove(abilityScore);

			UpdateModelAbilityScores();

			OnPropertyChanged(nameof(AbilityScoreImprovements));
		}

		private void AbilityScoreEditView_SelectionChanged(object sender, EventArgs e)
		{
			UpdateModelAbilityScores();

			OnPropertyChanged(nameof(AbilityScoreImprovements));
		}

		private void UpdateModelAbilityScores()
		{
			_levelModel.AbilityScoreImprovements.Clear();
			foreach (AbilityScoreEditViewModel ability in _abilityScoreImprovements)
			{
				_levelModel.AbilityScoreImprovements.Add(new KeyValuePair<Ability, int>(ability.SelectedAbility.Item1, ability.Value));
			}
		}

		private void AddFeat()
		{
			FeatModel featModel = _compendium.Feats.First();
			FeatEditViewModel featEditView = new FeatEditViewModel(featModel);
			featEditView.SelectionChanged += FeatEditView_SelectionChanged;
			_feats.Add(featEditView);

			UpdateModelFeats();

			OnPropertyChanged(nameof(Feats));
		}

		private void DeleteFeat(FeatEditViewModel featEditView)
		{
			_feats.Remove(featEditView);

			UpdateModelFeats();

			OnPropertyChanged(nameof(Feats));
		}

		private void FeatEditView_SelectionChanged(object sender, EventArgs e)
		{
			UpdateModelFeats();

			OnPropertyChanged(nameof(Feats));
		}

		private void UpdateModelFeats()
		{
			_levelModel.Feats.Clear();
			foreach (FeatEditViewModel feat in _feats)
			{
				FeatModel model = _compendium.Feats.FirstOrDefault(x => x.ID == feat.SelectedFeat.Item1);
				if (model != null)
				{
					_levelModel.Feats.Add(model);
				}
			}
		}

		private void FeatureView_IsSelectedChanged(object sender, EventArgs e)
		{
			FeatureViewModel featureView = (FeatureViewModel)sender;
			if (featureView.IsSelected && !_levelModel.Features.Contains(featureView.FeatureModel))
			{
				_levelModel.Features.Add(featureView.FeatureModel);
			}
			else if (!featureView.IsSelected && _levelModel.Features.Contains(featureView.FeatureModel))
			{
				_levelModel.Features.Remove(featureView.FeatureModel);
			}

			OnPropertyChanged(nameof(Features));
		}

		#endregion
	}
}
