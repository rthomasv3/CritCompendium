using System;
using System.Collections.Generic;
using System.Linq;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ListItemViewModels
{
    public sealed class CharacterListItemViewModel : NotifyPropertyChanged
    {
		#region Fields

		private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
		private CharacterModel _characterModel;
		private string _description;
		private bool _isSelected = false;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance of <see cref="CharacterListItemViewModel"/>
		/// </summary>
		/// <param name="characterModel"></param>
		public CharacterListItemViewModel(CharacterModel characterModel)
		{
			_characterModel = characterModel;

			Initialize();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Character model
		/// </summary>
		public CharacterModel CharacterModel
		{
			get { return _characterModel; }
		}

		/// <summary>
		/// Character name
		/// </summary>
		public string Name
		{
			get { return String.IsNullOrWhiteSpace(_characterModel.Name) ? "Unknown Name" : _characterModel.Name; }
		}

		/// <summary>
		/// Character description
		/// </summary>
		public string Description
		{
			get { return _description; }
		}

		/// <summary>
		/// True if selected
		/// </summary>
		public bool IsSelected
		{
			get { return _isSelected; }
			set { Set(ref _isSelected, value); }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Updates the model
		/// </summary>
		public void UpdateModel(CharacterModel characterModel)
		{
			_characterModel = characterModel;
            Initialize();
			OnPropertyChanged(String.Empty);
		}

		#endregion

		#region Non-Public Methods

		private void Initialize()
		{
            _description = String.Empty;

            _description += _characterModel.Race != null ? $"{_characterModel.Race.Name} " : "Unknown Race ";

            if (_characterModel.Levels.Count > 0)
            {
                Dictionary<KeyValuePair<Guid, string>, int> _classes = new Dictionary<KeyValuePair<Guid, string>, int>();
                foreach (LevelModel level in _characterModel.Levels)
                {
                    KeyValuePair<Guid, string> pair = new KeyValuePair<Guid, string>(level.Class.ID, level.Class.Name);

                    if (_classes.ContainsKey(pair))
                    {
                        _classes[pair]++;
                    }
                    else
                    {
                        _classes[pair] = 1;
                    }
                }
                _description += String.Join(", ", _classes.Select(x => $"{x.Key.Value} {x.Value}"));
            }
            else
            {
                _description += "Unknown Class";
            }
        }

		#endregion
	}
}
