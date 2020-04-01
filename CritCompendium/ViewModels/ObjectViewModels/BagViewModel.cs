using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class BagViewModel : ObjectViewModel
    {
        #region Events
        
        public event EventHandler EquippedChanged;

        #endregion

        #region Fields
        
        private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();
        private readonly Compendium _compendium = DependencyResolver.Resolve<Compendium>();

        private readonly BagModel _bagModel;
        private ObservableCollection<EquipmentViewModel> _equipment = new ObservableCollection<EquipmentViewModel>();
        private float _totalWeight;

        private readonly ICommand _showAddEquipmentDialogCommand;
        private readonly ICommand _showAddPackDialogCommand;
        private readonly ICommand _showEditEquipmentDialogCommand;
        private readonly ICommand _removeEquipmentCommand;
        private readonly ICommand _showUpdateMoneyDialogCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="BagViewModel"/>
        /// </summary>
        public BagViewModel(BagModel bagModel) : base()
        {
            _bagModel = bagModel;
            
            foreach (EquipmentModel equipmentModel in bagModel.Equipment.OrderBy(x => x.Name))
            {
                EquipmentViewModel equipmentViewModel = new EquipmentViewModel(equipmentModel);
                equipmentViewModel.PropertyChanged += EquipmentViewModel_PropertyChanged;
                _equipment.Add(equipmentViewModel);
            }

            UpdateTotalWeight();

            _showAddEquipmentDialogCommand = new RelayCommand(obj => true, obj => ShowAddEquipmentDialog());
            _showAddPackDialogCommand = new RelayCommand(obj => true, obj => ShowAddPackDialog());
            _showEditEquipmentDialogCommand = new RelayCommand(obj => true, obj => ShowEditEquipmentDialog((EquipmentViewModel)obj));
            _removeEquipmentCommand = new RelayCommand(obj => true, obj => RemoveEquipment((EquipmentViewModel)obj));
            _showUpdateMoneyDialogCommand = new RelayCommand(obj => true, obj => ShowUpdateMoneyDialog((Currency)obj));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets id
        /// </summary>
        public Guid ID
        {
            get { return _bagModel.ID; }
            set { _bagModel.ID = value; }
        }

        /// <summary>
        /// Gets bad model
        /// </summary>
        public BagModel BagModel
        {
            get { return _bagModel; }
        }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name
        {
            get { return _bagModel.Name; }
            set { _bagModel.Name = value; }
        }

        /// <summary>
        /// Gets or sets fixed weight
        /// </summary>
        public bool UseFixedWeight
        {
            get { return _bagModel.FixedWeight; }
            set { _bagModel.FixedWeight = value; }
        }

        /// <summary>
        /// Gets or sets fixed weight value
        /// </summary>
        public int FixedWeight
        {
            get { return _bagModel.FixedWeightValue; }
            set { _bagModel.FixedWeightValue = value; }
        }

        /// <summary>
        /// Gets total weight
        /// </summary>
        public float TotalWeight
        {
            get { return _totalWeight; }
        }

        /// <summary>
        /// Gets or sets copper
        /// </summary>
        public int Copper
        {
            get { return _bagModel.Copper; }
            set
            {
                _bagModel.Copper = value;
                OnPropertyChanged(nameof(Copper));
            }
        }

        /// <summary>
        /// Gets or sets silver
        /// </summary>
        public int Silver
        {
            get { return _bagModel.Silver; }
            set
            {
                _bagModel.Silver = value;
                OnPropertyChanged(nameof(Silver));
            }
        }

        /// <summary>
        /// Gets or sets electrum
        /// </summary>
        public int Electrum
        {
            get { return _bagModel.Electrum; }
            set
            {
                _bagModel.Electrum = value;
                OnPropertyChanged(nameof(Electrum));
            }
        }

        /// <summary>
        /// Gets or sets gold
        /// </summary>
        public int Gold
        {
            get { return _bagModel.Gold; }
            set
            {
               _bagModel.Gold = value;
                OnPropertyChanged(nameof(Gold));
            }
        }

        /// <summary>
        /// Gets or sets platinum
        /// </summary>
        public int Platinum
        {
            get { return _bagModel.Platinum; }
            set
            {
                _bagModel.Platinum  = value;
                OnPropertyChanged(nameof(Platinum));
            }
        }

        /// <summary>
        /// Gets equipment
        /// </summary>
        public IEnumerable<EquipmentViewModel> Equipment
        {
            get { return _equipment; }
        }

        /// <summary>
        /// Gets show equipment header
        /// </summary>
        public bool ShowEquipmentHeader
        {
            get { return _equipment.Any(); }
        }

        /// <summary>
        /// Gets show add equipment dialog command
        /// </summary>
        public ICommand ShowAddEquipmentDialogCommand
        {
            get { return _showAddEquipmentDialogCommand; }
        }

        /// <summary>
        /// Gets show add pack dialog command
        /// </summary>
        public ICommand ShowAddPackDialogCommand
        {
            get { return _showAddPackDialogCommand; }
        }

        /// <summary>
        /// Gets show edit equipment dialog command
        /// </summary>
        public ICommand ShowEditEquipmentDialogCommand
        {
            get { return _showEditEquipmentDialogCommand; }
        }

        /// <summary>
        /// Gets remove equipment
        /// </summary>
        public ICommand RemoveEquipmentCommand
        {
            get { return _removeEquipmentCommand; }
        }

        /// <summary>
        /// Gets show update money dialog command
        /// </summary>
        public ICommand ShowUpdateMoneyDialogCommand
        {
            get { return _showUpdateMoneyDialogCommand; }
        }

        #endregion

        #region Private Methods
        
        private void ShowAddEquipmentDialog()
        {
            EquipmentModel equipmentModel = _dialogService.ShowCreateEquipmentDialog("Add Item", new EquipmentModel());
            if (equipmentModel != null)
            {
                EquipmentViewModel equipmentViewModel = new EquipmentViewModel(equipmentModel);
                equipmentViewModel.PropertyChanged += EquipmentViewModel_PropertyChanged;

                _equipment.Add(equipmentViewModel);
                _bagModel.Equipment.Add(equipmentModel);

                _equipment = new ObservableCollection<EquipmentViewModel>(_equipment.OrderBy(x => x.Name));

                OnPropertyChanged(nameof(Equipment));
                OnPropertyChanged(nameof(ShowEquipmentHeader));
                UpdateTotalWeight();
            }
        }

        private void ShowEditEquipmentDialog(EquipmentViewModel equipmentViewModel)
        {
            EquipmentModel equipmentModel = _dialogService.ShowCreateEquipmentDialog($"Edit {equipmentViewModel.Name}", equipmentViewModel.EquipmentModel);
            if (equipmentModel != null)
            {
                _equipment.Remove(equipmentViewModel);
                _bagModel.Equipment.Remove(equipmentViewModel.EquipmentModel);

                EquipmentViewModel newEquipmentViewModel = new EquipmentViewModel(equipmentModel);
                newEquipmentViewModel.PropertyChanged += EquipmentViewModel_PropertyChanged;

                _equipment.Add(newEquipmentViewModel);
                _bagModel.Equipment.Add(equipmentModel);

                _equipment = new ObservableCollection<EquipmentViewModel>(_equipment.OrderBy(x => x.Name));

                OnPropertyChanged(nameof(Equipment));
                OnPropertyChanged(nameof(ShowEquipmentHeader));
                UpdateTotalWeight();
            }
        }

        private void RemoveEquipment(EquipmentViewModel equipmentViewModel)
        {
            bool? result = _dialogService.ShowConfirmationDialog("Remove Item", "Are you sure you want to remove " + equipmentViewModel.Name + "?", "Yes", "No", null);
            if (result == true)
            {
                _bagModel.Equipment.Remove(equipmentViewModel.EquipmentModel);
                _equipment.Remove(equipmentViewModel);
                OnPropertyChanged(nameof(Equipment));
                OnPropertyChanged(nameof(ShowEquipmentHeader));
                UpdateTotalWeight();
            }
        }
        
        private void ShowAddPackDialog()
        {
            PackModel packModel = _dialogService.ShowAddPackDialog();
            if (packModel != null)
            {
                foreach (string item in packModel.Items)
                {
                    string itemName = String.Empty;
                    int quantity = 0;
                    string[] parts = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        itemName = parts[0];
                        if (Int32.TryParse(parts[1], out int number))
                        {
                            quantity = number;
                        }
                    }
                    else
                    {
                        itemName = item;
                        quantity = 1;
                    }

                    if (!String.IsNullOrWhiteSpace(itemName))
                    {
                        EquipmentModel equipmentModel = new EquipmentModel();
                        ItemModel itemModel = _compendium.Items.FirstOrDefault(x => x.Name.Equals(itemName, StringComparison.CurrentCultureIgnoreCase));

                        if (itemModel != null)
                        {
                            equipmentModel.Item = itemModel;
                            equipmentModel.Name = itemModel.Name;
                        }
                        else
                        {
                            equipmentModel.Name = itemName;
                        }

                        equipmentModel.Quantity = quantity;

                        EquipmentViewModel equipmentViewModel = new EquipmentViewModel(equipmentModel);
                        equipmentViewModel.PropertyChanged += EquipmentViewModel_PropertyChanged;

                        _equipment.Add(equipmentViewModel);
                        _bagModel.Equipment.Add(equipmentModel);
                    }
                }

                _equipment = new ObservableCollection<EquipmentViewModel>(_equipment.OrderBy(x => x.Name));

                OnPropertyChanged(nameof(Equipment));
                OnPropertyChanged(nameof(ShowEquipmentHeader));
                UpdateTotalWeight();
            }
        }

        private void EquipmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EquipmentViewModel.Equipped))
            {
                EquippedChanged?.Invoke(sender, EventArgs.Empty);
            }
            else
            {
                UpdateTotalWeight();
            }
        }

        private void ShowUpdateMoneyDialog(Currency currency)
        {
            int currentAmount = 0;

            switch (currency)
            {
                case Currency.Copper:
                    currentAmount = _bagModel.Copper;
                    break;

                case Currency.Silver:
                    currentAmount = _bagModel.Silver;
                    break;

                case Currency.Electrum:
                    currentAmount = _bagModel.Electrum;
                    break;

                case Currency.Gold:
                    currentAmount = _bagModel.Gold;
                    break;

                case Currency.Platinum:
                    currentAmount = _bagModel.Platinum;
                    break;
            }
            
            int? result = _dialogService.ShowAddSubtractDialog(currentAmount);
            if (result.HasValue)
            {
                int newAmount = Math.Max(result.Value, 0);

                switch (currency)
                {
                    case Currency.Copper:
                        _bagModel.Copper = newAmount;
                        OnPropertyChanged(nameof(Copper));
                        break;

                    case Currency.Silver:
                        _bagModel.Silver = newAmount;
                        OnPropertyChanged(nameof(Silver));
                        break;

                    case Currency.Electrum:
                        _bagModel.Electrum = newAmount;
                        OnPropertyChanged(nameof(Electrum));
                        break;

                    case Currency.Gold:
                        _bagModel.Gold = newAmount;
                        OnPropertyChanged(nameof(Gold));
                        break;

                    case Currency.Platinum:
                        _bagModel.Platinum = newAmount;
                        OnPropertyChanged(nameof(Platinum));
                        break;
                }

                UpdateTotalWeight();
            }
        }

        private void UpdateTotalWeight()
        {
            _totalWeight = 0.0f;

            if (_bagModel.FixedWeight)
            {
                _totalWeight = _bagModel.FixedWeightValue;
            }
            else
            {
                _totalWeight += _bagModel.Copper * 0.02f;
                _totalWeight += _bagModel.Silver * 0.02f;
                _totalWeight += _bagModel.Electrum * 0.02f;
                _totalWeight += _bagModel.Gold * 0.02f;
                _totalWeight += _bagModel.Platinum * 0.02f;

                foreach (EquipmentModel equipment in _bagModel.Equipment)
                {
                    if (equipment.Item != null)
                    {
                        if (float.TryParse(equipment.Item.Weight, out float weight))
                        {
                            _totalWeight += weight * equipment.Quantity;
                        }
                    }
                }
            }

            OnPropertyChanged(nameof(TotalWeight));
        }

        protected override void OnAccept()
        {
            if (String.IsNullOrWhiteSpace(_bagModel.Name))
            {
                _dialogService.ShowConfirmationDialog("Required Field", "Name is required.", "OK", null, null);
            }
            else
            {
                Accept();
            }
        }

        #endregion
    }
}
