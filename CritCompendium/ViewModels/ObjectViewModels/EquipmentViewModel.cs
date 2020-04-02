using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Business;
using CritCompendium.ViewModels.DialogViewModels;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class EquipmentViewModel : NotifyPropertyChanged, IConfirmation
   {
      #region Events

      public event EventHandler AcceptSelected;
      public event EventHandler RejectSelected;
      public event EventHandler CancelSelected;

      #endregion

      #region Fields

      private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();
      private readonly Compendium _compendium = DependencyResolver.Resolve<Compendium>();

      private readonly EquipmentModel _equipmentModel;
      private readonly List<KeyValuePair<ItemModel, string>> _itemOptions = new List<KeyValuePair<ItemModel, string>>();
      private KeyValuePair<ItemModel, string> _selectedItemOption;

      private readonly ICommand _searchItemsCommand;
      private readonly ICommand _viewDetailsCommand;
      private readonly ICommand _decreaseQuantityCommand;
      private readonly ICommand _increaseQuantityCommand;
      private readonly ICommand _acceptCommand;
      private readonly ICommand _rejectCommand;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="EquipmentViewModel"/>
      /// </summary>
      public EquipmentViewModel(EquipmentModel equipmentModel)
      {
         _equipmentModel = equipmentModel;

         _searchItemsCommand = new RelayCommand(obj => true, obj => SearchItems());
         _viewDetailsCommand = new RelayCommand(obj => true, obj => ViewDetails());
         _decreaseQuantityCommand = new RelayCommand(obj => true, obj => DecreaseQuantity());
         _increaseQuantityCommand = new RelayCommand(obj => true, obj => IncreaseQuantity());
         _acceptCommand = new RelayCommand(obj => true, obj => OnAccept());
         _rejectCommand = new RelayCommand(obj => true, obj => OnReject());
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets id
      /// </summary>
      public Guid ID
      {
         get { return _equipmentModel.ID; }
      }

      /// <summary>
      /// Gets equipment model
      /// </summary>
      public EquipmentModel EquipmentModel
      {
         get { return _equipmentModel; }
      }

      /// <summary>
      /// Gets item options
      /// </summary>
      public List<KeyValuePair<ItemModel, string>> ItemOptions
      {
         get { return _itemOptions; }
      }

      /// <summary>
      /// Gets or sets selected item option
      /// </summary>
      public KeyValuePair<ItemModel, string> SelectedItemOption
      {
         get { return _selectedItemOption; }
         set
         {
            _selectedItemOption = value;
            _equipmentModel.Item = value.Key;
            UpdateItemProperties();
         }
      }

      /// <summary>
      /// Gets or sets item
      /// </summary>
      public ItemModel Item
      {
         get { return _equipmentModel.Item; }
         set { _equipmentModel.Item = value; }
      }

      /// <summary>
      /// Gets is item set
      /// </summary>
      public bool IsItemSet
      {
         get { return _equipmentModel.Item != null; }
      }

      /// <summary>
      /// Gets or sets name
      /// </summary>
      public string Name
      {
         get { return _equipmentModel.Name; }
         set { _equipmentModel.Name = value; }
      }

      /// <summary>
      /// Gets or sets equipped
      /// </summary>
      public bool Equipped
      {
         get { return _equipmentModel.Equipped; }
         set
         {
            _equipmentModel.Equipped = value;
            OnPropertyChanged(nameof(Equipped));
         }
      }

      /// <summary>
      /// Gets or sets quantity
      /// </summary>
      public int Quantity
      {
         get { return _equipmentModel.Quantity; }
         set { _equipmentModel.Quantity = value; }
      }

      /// <summary>
      /// Gets or sets notes
      /// </summary>
      public string Notes
      {
         get { return _equipmentModel.Notes; }
         set { _equipmentModel.Notes = value; }
      }

      /// <summary>
      /// Gets search items command
      /// </summary>
      public ICommand SearchItemsCommand
      {
         get { return _searchItemsCommand; }
      }

      /// <summary>
      /// Gets view details command
      /// </summary>
      public ICommand ViewDetailsCommand
      {
         get { return _viewDetailsCommand; }
      }

      /// <summary>
      /// Gets decrease quantity command
      /// </summary>
      public ICommand DecreaseQuantityCommand
      {
         get { return _decreaseQuantityCommand; }
      }

      /// <summary>
      /// Gets increase quantity command
      /// </summary>
      public ICommand IncreaseQuantityCommand
      {
         get { return _increaseQuantityCommand; }
      }

      /// <summary>
      /// Gets accept command
      /// </summary>
      public ICommand AcceptCommand
      {
         get { return _acceptCommand; }
      }

      /// <summary>
      /// Gets reject command
      /// </summary>
      public ICommand RejectCommand
      {
         get { return _rejectCommand; }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Initializes item options
      /// </summary>
      public void InitializeItemOptions()
      {
         _itemOptions.Clear();
         _itemOptions.Add(new KeyValuePair<ItemModel, string>(null, "None"));
         foreach (ItemModel itemModel in _compendium.Items)
         {
            _itemOptions.Add(new KeyValuePair<ItemModel, string>(itemModel, itemModel.Name));
         }

         if (_equipmentModel.Item != null)
         {
            _selectedItemOption = _itemOptions.FirstOrDefault(x => x.Key != null && x.Key.Id == _equipmentModel.Item.Id);
         }

         if (_selectedItemOption.Equals(default(KeyValuePair<ItemModel, string>)))
         {
            _selectedItemOption = _itemOptions[0];
         }
      }

      #endregion

      #region Private Methods

      private void UpdateItemProperties()
      {
         if (_equipmentModel.Item != null)
         {
            _equipmentModel.Name = _equipmentModel.Item.Name;
            OnPropertyChanged(nameof(Name));
         }
         else
         {
            _equipmentModel.Name = String.Empty;
            OnPropertyChanged(nameof(Name));
         }
      }

      private void SearchItems()
      {
         IEnumerable<ItemModel> items = _dialogService.ShowItemSearchDialog(false);
         if (items != null && items.Any())
         {
            _equipmentModel.Item = items.First();

            KeyValuePair<ItemModel, string> itemPair = _itemOptions.FirstOrDefault(x => x.Key != null && x.Key.Id == _equipmentModel.Item.Id);
            if (!itemPair.Equals(default(KeyValuePair<ItemModel, string>)))
            {
               _selectedItemOption = itemPair;
               OnPropertyChanged(nameof(SelectedItemOption));
            }

            UpdateItemProperties();
         }
      }

      private void ViewDetails()
      {
         if (_equipmentModel.Item != null)
         {
            _dialogService.ShowDetailsDialog(new ItemViewModel(_equipmentModel.Item));
         }
      }

      private void DecreaseQuantity()
      {
         _equipmentModel.Quantity = Math.Max(_equipmentModel.Quantity - 1, 1);
         OnPropertyChanged(nameof(Quantity));
      }

      private void IncreaseQuantity()
      {
         _equipmentModel.Quantity = _equipmentModel.Quantity + 1;
         OnPropertyChanged(nameof(Quantity));
      }

      private void OnAccept()
      {
         if (String.IsNullOrWhiteSpace(_equipmentModel.Name))
         {
            _dialogService.ShowConfirmationDialog("Required Field", "Name is required.", "OK", null, null);
         }
         else
         {
            AcceptSelected?.Invoke(this, EventArgs.Empty);
         }
      }

      private void OnReject()
      {
         RejectSelected?.Invoke(this, EventArgs.Empty);
      }

      #endregion
   }
}
