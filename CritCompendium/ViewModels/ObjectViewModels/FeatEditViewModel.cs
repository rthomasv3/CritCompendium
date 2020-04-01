using System;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
	public sealed class FeatEditViewModel
	{
		#region Events

		public event EventHandler SelectionChanged;

		#endregion

		#region Fields

		private Tuple<Guid, string> _selectedFeat;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates and instance of <see cref="FeatEditViewModel"/>
		/// </summary>
		public FeatEditViewModel(FeatModel feat)
		{
			_selectedFeat = new Tuple<Guid, string>(feat.ID, feat.Name);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets of sets selected feat
		/// </summary>
		public Tuple<Guid, string> SelectedFeat
		{
			get { return _selectedFeat; }
			set
			{
				_selectedFeat = value;
				SelectionChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		#endregion
	}
}
