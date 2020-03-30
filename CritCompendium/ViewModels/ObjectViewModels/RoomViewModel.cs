using System;
using System.IO;
using System.Windows.Input;
using CriticalCompendiumInfrastructure.Models;
using Microsoft.Win32;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class RoomViewModel : NotifyPropertyChanged
    {
        #region Fields

        private readonly RoomModel _roomModel;

        private string _name;
        private string _description;
        private string _entry;
        private string _map;
        private string _floor;

        public readonly ICommand _browseMapLocationCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="RoomViewModel"/>
        /// </summary>
        public RoomViewModel(RoomModel roomModel)
        {
            _roomModel = roomModel;

            _name = _roomModel.Name;
            _description = _roomModel.Description;
            _entry = _roomModel.Entry;
            _map = _roomModel.Map;
            _floor = _roomModel.Floor;

            _browseMapLocationCommand = new RelayCommand(obj => true, obj => BrowseMapLocation());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets room model
        /// </summary>
        public RoomModel RoomModel
        {
            get { return _roomModel; }
        }

        /// <summary>
        /// Gets room title
        /// </summary>
        public string Title
        {
            get { return !String.IsNullOrWhiteSpace(_name) ? _name : "Unknown Room"; }
        }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (Set(ref _name, value))
                {
                    _roomModel.Name = _name;

                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        /// <summary>
        /// Gets or sets description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                if (Set(ref _description, value))
                {
                    _roomModel.Description = _description;
                }
            }
        }

        /// <summary>
        /// Gets or sets entry
        /// </summary>
        public string Entry
        {
            get { return _entry; }
            set
            {
                if (Set(ref _entry, value))
                {
                    _roomModel.Entry = _entry;
                }
            }
        }

        /// <summary>
        /// Gets or sets map
        /// </summary>
        public string Map
        {
            get { return _map; }
            set
            {
                if (Set(ref _map, value))
                {
                    _roomModel.Map = _map;
                }
            }
        }

        /// <summary>
        /// Gets or sets floor
        /// </summary>
        public string Floor
        {
            get { return _floor; }
            set
            {
                if (Set(ref _floor, value))
                {
                    _roomModel.Floor = _floor;
                }
            }
        }

        /// <summary>
        /// Gets browse map location
        /// </summary>
        public ICommand BrowseMapLocationCommand
        {
            get { return _browseMapLocationCommand; }
        }

        #endregion

        #region Private Methods

        private void BrowseMapLocation()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                _map = Path.GetFileName(openFileDialog.FileName);
                _roomModel.Map = _map;

                OnPropertyChanged(nameof(Map));
            }
        }

        #endregion
    }
}
