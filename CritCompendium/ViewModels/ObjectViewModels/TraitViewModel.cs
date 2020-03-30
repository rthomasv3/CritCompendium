using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CriticalCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
	public sealed class TraitViewModel
	{
		#region Fields

		private readonly TraitModel _traitModel;
		private string _text;

		#endregion

		#region Constructors

		public TraitViewModel(TraitModel traitModel)
		{
			_traitModel = traitModel;

			for (int i = 0; i < _traitModel.TextCollection.Count; ++i)
			{
				_text += _traitModel.TextCollection[i].Replace("\t", Environment.NewLine);

				if (i + 1 < _traitModel.TextCollection.Count)
				{
					_text += Environment.NewLine;
				}
			}
		}

		#endregion

		#region Properties

		public string Name
		{
			get { return _traitModel.Name; }
			set { _traitModel.Name = value; }
		}

		public string Text
		{
			get { return _text; }
			set { _text = value; }
		}

		#endregion
	}
}
