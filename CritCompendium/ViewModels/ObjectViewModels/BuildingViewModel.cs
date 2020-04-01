using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using Microsoft.Win32;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class BuildingViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();

      private readonly BuildingModel _buildingModel;

      private string _name;
      private string _description;
      private string _map;
      private readonly Dictionary<BuildingType, string> _buildingTypeOptions = new Dictionary<BuildingType, string>();
      private KeyValuePair<BuildingType, string> _selectedBuildingType;
      private string _customBuildingType;
      private ObservableCollection<RoomViewModel> _rooms = new ObservableCollection<RoomViewModel>();

      public readonly ICommand _browseMapLocationCommand;
      public readonly ICommand _addRoomCommand;
      public readonly ICommand _deleteRoomCommand;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of <see cref="BuildingViewModel"/>
      /// </summary>
      public BuildingViewModel(BuildingModel buildingModel)
      {
         _buildingModel = buildingModel;

         _name = _buildingModel.Name;
         _description = _buildingModel.Description;
         _map = _buildingModel.Map;

         foreach (BuildingType buildingType in Enum.GetValues(typeof(BuildingType)).Cast<BuildingType>().OrderBy(x => x.ToString()))
         {
            if (buildingType != BuildingType.Other)
            {
               _buildingTypeOptions.Add(buildingType, buildingType.ToString().Replace("_", " "));
            }
         }
         _buildingTypeOptions.Add(BuildingType.Other, BuildingType.Other.ToString());
         _selectedBuildingType = _buildingTypeOptions.FirstOrDefault(x => x.Key == _buildingModel.BuildingType);

         _customBuildingType = _buildingModel.CustomBuildingType;

         foreach (RoomModel roomModel in _buildingModel.Rooms)
         {
            RoomViewModel roomViewModel = new RoomViewModel(roomModel);
            roomViewModel.PropertyChanged += RoomViewModel_PropertyChanged;
            _rooms.Add(roomViewModel);
         }

         _browseMapLocationCommand = new RelayCommand(obj => true, obj => BrowseMapLocation());
         _addRoomCommand = new RelayCommand(obj => true, obj => AddRoom());
         _deleteRoomCommand = new RelayCommand(obj => true, obj => DeleteRoom(obj as RoomViewModel));
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets building model
      /// </summary>
      public BuildingModel BuildingModel
      {
         get { return _buildingModel; }
      }

      /// <summary>
      /// Gets room title
      /// </summary>
      public string Title
      {
         get { return !String.IsNullOrWhiteSpace(_name) ? _name : "Unknown Building"; }
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
               _buildingModel.Name = _name;

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
               _buildingModel.Description = _description;
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
               _buildingModel.Map = _map;
            }
         }
      }

      /// <summary>
      /// Gets building type options
      /// </summary>
      public Dictionary<BuildingType, string> BuildingTypeOptions
      {
         get { return _buildingTypeOptions; }
      }

      /// <summary>
      /// Gets or sets building type
      /// </summary>
      public KeyValuePair<BuildingType, string> SelectedBuildingType
      {
         get { return _selectedBuildingType; }
         set
         {
            if (Set(ref _selectedBuildingType, value))
            {
               _buildingModel.BuildingType = _selectedBuildingType.Key;

               OnPropertyChanged(nameof(TypeOptionIsCustom));
            }
         }
      }

      /// <summary>
      /// Returns true if the building type is custom (other)
      /// </summary>
      public bool TypeOptionIsCustom
      {
         get { return _buildingModel.BuildingType == BuildingType.Other; }
      }

      /// <summary>
      /// Gets or sets custom building type
      /// </summary>
      public string CustomBuildingType
      {
         get { return _customBuildingType; }
         set
         {
            if (Set(ref _customBuildingType, value))
            {
               _buildingModel.CustomBuildingType = _customBuildingType;
            }
         }
      }

      /// <summary>
      /// Gets rooms
      /// </summary>
      public IEnumerable<RoomViewModel> Rooms
      {
         get { return _rooms; }
      }

      /// <summary>
      /// Gets browse map location
      /// </summary>
      public ICommand BrowseMapLocationCommand
      {
         get { return _browseMapLocationCommand; }
      }

      /// <summary>
      /// Gets add room command
      /// </summary>
      public ICommand AddRoomCommand
      {
         get { return _addRoomCommand; }
      }

      /// <summary>
      /// Gets delete room command
      /// </summary>
      public ICommand DeleteRoomCommand
      {
         get { return _deleteRoomCommand; }
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
            _buildingModel.Map = _map;

            OnPropertyChanged(nameof(Map));
         }
      }

      private void AddRoom()
      {
         RoomModel roomModel = new RoomModel();
         RoomViewModel roomViewModel = new RoomViewModel(roomModel);

         roomViewModel.PropertyChanged += RoomViewModel_PropertyChanged;

         _buildingModel.Rooms.Add(roomModel);
         _rooms.Add(roomViewModel);

         OnPropertyChanged(nameof(Rooms));
      }

      private void DeleteRoom(RoomViewModel roomViewModel)
      {
         if (roomViewModel != null)
         {
            string name = !String.IsNullOrWhiteSpace(roomViewModel.Name) ? roomViewModel.Name : "Unknown Name";
            if (_dialogService.ShowConfirmationDialog("Delete Room", $"Are you sure you want to delete {name}?", "Yes", "No", null) == true)
            {
               roomViewModel.PropertyChanged -= RoomViewModel_PropertyChanged;

               _buildingModel.Rooms.Remove(roomViewModel.RoomModel);
               _rooms.Remove(roomViewModel);
            }
         }
      }

      private void RoomViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         OnPropertyChanged();
      }

      #endregion
   }
}
