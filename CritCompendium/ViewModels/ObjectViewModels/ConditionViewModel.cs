using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
	public sealed class ConditionViewModel
	{
		#region Fields

		private readonly ConditionModel _conditionModel;
		private readonly string _description;
		private readonly string _text;
		private readonly List<List<TableElementViewModel>> _elements = new List<List<TableElementViewModel>>();

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance of <see cref="ConditionViewModel"/>
		/// </summary>
		public ConditionViewModel(ConditionModel conditionModel)
		{
			_conditionModel = conditionModel;
			if (_conditionModel.Description == null)
			{
				_description = String.Join(Environment.NewLine + Environment.NewLine, _conditionModel.TextCollection);
			}
			else
			{
				_description = _conditionModel.Description;
				_text = String.Join(Environment.NewLine + Environment.NewLine, _conditionModel.TextCollection);
			}
			
			int columns = _conditionModel.Headers.Count;
			List<TableElementViewModel> headers = new List<TableElementViewModel>();
			for (int i = 0; i < columns; ++i)
			{
				headers.Add(new TableElementViewModel(_conditionModel.Headers[i], true, i));
			}
			_elements.Add(headers);
			
			int index = 0;
			while (index < _conditionModel.Elements.Count)
			{
				if (index + columns <= _conditionModel.Elements.Count)
				{
					List<string> elements = _conditionModel.Elements.Skip(index).Take(columns).ToList();
					List<TableElementViewModel> elementViewModels = new List<TableElementViewModel>();
					for (int i = 0; i < elements.Count; ++i)
					{
						elementViewModels.Add(new TableElementViewModel(elements[i], false, i));
					}
					_elements.Add(elementViewModels);

					index += columns;
				}
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets condition model
		/// </summary>
		public ConditionModel ConditionModel
		{
			get { return _conditionModel; }
		}

		/// <summary>
		/// Gets name
		/// </summary>
		public string Name
		{
			get { return _conditionModel.Name; }
		}

		/// <summary>
		/// Gets description
		/// </summary>
		public string Description
		{
			get { return _description; }
		}

		/// <summary>
		/// Gets elements
		/// </summary>
		public List<List<TableElementViewModel>> Elements
		{
			get { return _elements; }
		}

		/// <summary>
		/// Gets text
		/// </summary>
		public string Text
		{
			get { return _text; }
		}

		/// <summary>
		/// Gets XML
		/// </summary>
		public string XML
		{
			get { return _conditionModel.XML; }
		}

		#endregion
	}
}
