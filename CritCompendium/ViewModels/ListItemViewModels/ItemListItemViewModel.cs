using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ListItemViewModels
{
	public sealed class ItemListItemViewModel : NotifyPropertyChanged
	{
		#region Fields

		private ItemModel _itemModel;
		private readonly StringService _stringService;
		private string _details;
		private bool _isSelected = false;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of <see cref="ItemListItemViewModel"/>
		/// </summary>
		public ItemListItemViewModel(ItemModel itemModel, StringService stringService)
		{
			_itemModel = itemModel;
			_stringService = stringService;

			Initialize();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Item model
		/// </summary>
		public ItemModel ItemModel
		{
			get { return _itemModel; }
		}

		/// <summary>
		/// Item name
		/// </summary>
		public string Name
		{
			get { return _itemModel.Name; }
		}

		/// <summary>
		/// Item details
		/// </summary>
		public string Details
		{
			get { return _details; }
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
		public void UpdateModel(ItemModel itemModel)
		{
			_itemModel = itemModel;
			OnPropertyChanged("");
		}

		#endregion

		#region Non-Public Methods

		private void Initialize()
		{
			_details = _stringService.GetString(_itemModel.Type) + (_itemModel.Magic ? ", Magic" : ", Non-Magic");
		}

		#endregion
	}
}
