using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Persistence;
using CritCompendiumInfrastructure.Services;
using CritCompendiumInfrastructure.Services.Search;
using CritCompendiumInfrastructure.Services.Search.Input;
using CritCompendium.Services;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendium.ViewModels.ObjectViewModels;

namespace CritCompendium.ViewModels
{
    public sealed class ConditionsViewModel : NotifyPropertyChanged
    {
		#region Fields

		private readonly Compendium _compendium;
		private readonly ConditionSearchService _conditionSearchService;
        private readonly ConditionSearchInput _conditionSearchInput;
        private readonly StringService _stringService;
		private readonly DialogService _dialogService;
		private readonly XMLImporter _xmlImporter;
		private readonly XMLExporter _xmlExporter;
        private readonly ObservableCollection<ConditionListItemViewModel> _conditions = new ObservableCollection<ConditionListItemViewModel>();
		private readonly ICommand _selectConditionCommand;
		private readonly ICommand _editConditionCommand;
		private readonly ICommand _exportConditionCommand;
        private readonly ICommand _cancelEditConditionCommand;
		private readonly ICommand _saveEditConditionCommand;
		private readonly ICommand _resetFiltersCommand;
		private readonly ICommand _addCommand;
		private readonly ICommand _copyCommand;
		private readonly ICommand _deleteCommand;
        private readonly ICommand _importCommand;
        private readonly ICommand _selectNextCommand;
        private readonly ICommand _selectPreviousCommand;
        private ConditionViewModel _selectedCondition;
		private string _editConditionXML;
		private bool _editHasUnsavedChanges;

		#endregion

		#region Constructors

		public ConditionsViewModel(Compendium compendium, ConditionSearchService conditionSearchService, ConditionSearchInput conditionSearchInput,
			StringService stringService, DialogService dialogService, XMLImporter xmlImporter, XMLExporter xmlExporter)
		{
			_compendium = compendium;
			_conditionSearchService = conditionSearchService;
            _conditionSearchInput = conditionSearchInput;
			_stringService = stringService;
			_dialogService = dialogService;
			_xmlImporter = xmlImporter;
            _xmlExporter = xmlExporter;

			_selectConditionCommand = new RelayCommand(obj => true, obj => SelectCondition(obj as ConditionListItemViewModel));
			_editConditionCommand = new RelayCommand(obj => true, obj => EditCondition(obj as ConditionViewModel));
			_exportConditionCommand = new RelayCommand(obj => true, obj => ExportCondition(obj as ConditionViewModel));
            _cancelEditConditionCommand = new RelayCommand(obj => true, obj => CancelEditCondition());
			_saveEditConditionCommand = new RelayCommand(obj => HasUnsavedChanges, obj => SaveEditCondition());
			_resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
			_addCommand = new RelayCommand(obj => true, obj => Add());
			_copyCommand = new RelayCommand(obj => _selectedCondition != null, obj => Copy());
			_deleteCommand = new RelayCommand(obj => _selectedCondition != null, obj => Delete());
            _importCommand = new RelayCommand(obj => true, obj => Import());
            _selectNextCommand = new RelayCommand(obj => true, obj => SelectNext());
            _selectPreviousCommand = new RelayCommand(obj => true, obj => SelectPrevious());

            Search();
		}

		#endregion

		#region Properties

		/// <summary>
		/// List of conditions
		/// </summary>
		public ObservableCollection<ConditionListItemViewModel> Conditions
		{
			get { return _conditions; }
		}

		/// <summary>
		/// Gets or sets the search text
		/// </summary>
		public string SearchText
		{
			get { return _conditionSearchInput.SearchText; }
			set
			{
				_conditionSearchInput.SearchText = value;
				Search();
			}
		}

		/// <summary>
		/// Command to select a condition
		/// </summary>
		public ICommand SelectConditionCommand
		{
			get { return _selectConditionCommand; }
		}

		/// <summary>
		/// Selected condition
		/// </summary>
		public ConditionViewModel SelectedCondition
		{
			get { return _selectedCondition; }
		}

		/// <summary>
		/// Editing condition xml
		/// </summary>
		public string EditingConditionXML
		{
			get { return _editConditionXML; }
			set
			{
				if (Set(ref _editConditionXML, value) && !_editHasUnsavedChanges)
				{
					_editHasUnsavedChanges = true;
					OnPropertyChanged(nameof(HasUnsavedChanges));
				}
			}
		}

		/// <summary>
		/// Command to reset filters
		/// </summary>
		public ICommand ResetFiltersCommand
		{
			get { return _resetFiltersCommand; }
		}

		/// <summary>
		/// Command to edit condition
		/// </summary>
		public ICommand EditConditionCommand
		{
			get { return _editConditionCommand; }
		}

        /// <summary>
		/// Command to export condition
		/// </summary>
		public ICommand ExportConditionCommand
        {
            get { return _exportConditionCommand; }
        }

        /// <summary>
        /// Command to cancel edit condition
        /// </summary>
        public ICommand CancelEditConditionCommand
		{
			get { return _cancelEditConditionCommand; }
		}

		/// <summary>
		/// Command to save edit condition
		/// </summary>
		public ICommand SaveEditConditionCommand
		{
			get { return _saveEditConditionCommand; }
		}

		/// <summary>
		/// Command to add condition
		/// </summary>
		public ICommand AddConditionCommand
		{
			get { return _addCommand; }
		}

		/// <summary>
		/// Command to copy condition
		/// </summary>
		public ICommand CopyConditionCommand
		{
			get { return _copyCommand; }
		}

		/// <summary>
		/// Command to delete selected condition
		/// </summary>
		public ICommand DeleteConditionCommand
		{
			get { return _deleteCommand; }
        }

        /// <summary>
        /// Command to import
        /// </summary>
        public ICommand ImportCommand
        {
            get { return _importCommand; }
        }

        /// <summary>
        /// Command to select next
        /// </summary>
        public ICommand SelectNextCommand
        {
            get { return _selectNextCommand; }
        }

        /// <summary>
        /// Command to select previous
        /// </summary>
        public ICommand SelectPreviousCommand
        {
            get { return _selectPreviousCommand; }
        }

        /// <summary>
        /// True if currently editing a condition
        /// </summary>
        public bool IsEditingCondition
		{
			get { return _editConditionXML != null; }
		}

		/// <summary>
		/// True if edit has unsaved changes
		/// </summary>
		public bool HasUnsavedChanges
		{
			get { return _editHasUnsavedChanges; }
		}

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches, applying current sorting and filtering
        /// </summary>
        public void Search()
        {
            _conditions.Clear();
            foreach (ConditionModel conditionModel in _conditionSearchService.Search(_conditionSearchInput))
            {
                _conditions.Add(new ConditionListItemViewModel(conditionModel));
            }

            if (_selectedCondition != null)
            {
                ConditionListItemViewModel condition = _conditions.FirstOrDefault(x => x.ConditionModel.ID == _selectedCondition.ConditionModel.ID);
                if (condition != null)
                {
                    condition.IsSelected = true;
                }
            }
        }

        #endregion

        #region Private Methods

        private void InitializeSearch()
		{
            _conditionSearchInput.Reset();

			OnPropertyChanged(nameof(SearchText));

			Search();
		}

		private void SelectCondition(ConditionListItemViewModel conditionItem)
		{
			bool selectCondition = true;

			if (_editConditionXML != null)
			{
				if (_editHasUnsavedChanges)
				{
					string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
														 _selectedCondition.Name, Environment.NewLine + Environment.NewLine);
                    string accept = "Save and Continue";
                    string reject = "Discard Changes";
                    string cancel = "Cancel Navigation";
                    bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                    if (result == true)
                    {
                        if (!SaveEditCondition())
                        {
                            selectCondition = false;
                        }
                    }
                    else if (result == false)
                    {
                        CancelEditCondition();
                    }
                    else
                    {
                        selectCondition = false;
                    }
                }
				else
				{
					CancelEditCondition();
				}
			}

			if (selectCondition)
			{
				foreach (ConditionListItemViewModel item in _conditions)
				{
					item.IsSelected = false;
				}
				conditionItem.IsSelected = true;

				_selectedCondition = new ConditionViewModel(conditionItem.ConditionModel);
				OnPropertyChanged(nameof(SelectedCondition));
			}
		}

		private void EditCondition(ConditionViewModel conditionModel)
		{
			_editConditionXML = conditionModel.XML;

			OnPropertyChanged(nameof(EditingConditionXML));
			OnPropertyChanged(nameof(IsEditingCondition));
		}

		private void CancelEditCondition()
		{
			_editHasUnsavedChanges = false;
			_editConditionXML = null;

			OnPropertyChanged(nameof(EditingConditionXML));
			OnPropertyChanged(nameof(IsEditingCondition));
			OnPropertyChanged(nameof(HasUnsavedChanges));
		}

		private bool SaveEditCondition()
		{
			bool saved = false;

			try
			{
				ConditionModel model = _xmlImporter.GetCondition(_editConditionXML);

				if (model != null)
				{
					model.ID = _selectedCondition.ConditionModel.ID;
                    _compendium.UpdateCondition(model);
					_selectedCondition = new ConditionViewModel(model);

					ConditionListItemViewModel oldListItem = _conditions.FirstOrDefault(x => x.ConditionModel.ID == model.ID);
					if (oldListItem != null)
					{
                        if (_conditionSearchService.SearchInputApplies(_conditionSearchInput, model))
                        {
                            oldListItem.UpdateModel(model);
                        }
                        else
                        {
                            _conditions.Remove(oldListItem);
                        }
					}

					_editConditionXML = null;
					_editHasUnsavedChanges = false;

					SortConditions();

                    _compendium.SaveConditions();

					OnPropertyChanged(nameof(SelectedCondition));
					OnPropertyChanged(nameof(EditingConditionXML));
					OnPropertyChanged(nameof(IsEditingCondition));
					OnPropertyChanged(nameof(HasUnsavedChanges));

					saved = true;
				}
				else
				{
					string message = String.Format("Something went wrong...{0}{1}{2}{3}",
														 Environment.NewLine + Environment.NewLine,
														 "The following are required:",
														 Environment.NewLine,
														 "-name");
					_dialogService.ShowConfirmationDialog("Unable To Save", message, "OK", null, null);
				}
			}
			catch (Exception ex)
			{
				string message = String.Format("Something went wrong...{0}{1}",
														 Environment.NewLine + Environment.NewLine,
														 ex.Message);
				_dialogService.ShowConfirmationDialog("Unable To Save", message, "OK", null, null);
			}

			return saved;
		}

		private void Add()
		{
			bool addCondition = true;

			if (_editConditionXML != null)
			{
				if (_editHasUnsavedChanges)
				{
					string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
														 _selectedCondition.Name, Environment.NewLine + Environment.NewLine);
                    string accept = "Save and Continue";
                    string reject = "Discard Changes";
                    string cancel = "Cancel Navigation";
                    bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                    if (result == true)
                    {
                        if (!SaveEditCondition())
                        {
                            addCondition = false;
                        }
                    }
                    else if (result == false)
                    {
                        CancelEditCondition();
                    }
                    else
                    {
                        addCondition = false;
                    }
                }
				else
				{
					CancelEditCondition();
				}
			}

			if (addCondition)
			{
				string xml = "<name>New Condition</name><text></text>";

				ConditionModel conditionModel = _xmlImporter.GetCondition(xml);

				_compendium.AddCondition(conditionModel);

                if (_conditionSearchService.SearchInputApplies(_conditionSearchInput, conditionModel))
                {
                    ConditionListItemViewModel listItem = new ConditionListItemViewModel(conditionModel);
                    _conditions.Add(listItem);
                    foreach (ConditionListItemViewModel item in _conditions)
                    {
                        item.IsSelected = false;
                    }
                    listItem.IsSelected = true;
                }

				_selectedCondition = new ConditionViewModel(conditionModel);

				_editConditionXML = conditionModel.XML;

				SortConditions();

                _compendium.SaveConditions();

                OnPropertyChanged(nameof(EditingConditionXML));
				OnPropertyChanged(nameof(IsEditingCondition));
				OnPropertyChanged(nameof(SelectedCondition));
			}
		}

		private void Copy()
		{
			if (_selectedCondition != null)
			{
				bool copyCondition = true;

				if (_editConditionXML != null)
				{
					if (_editHasUnsavedChanges)
					{
						string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
															 _selectedCondition.Name, Environment.NewLine + Environment.NewLine);
                        string accept = "Save and Continue";
                        string reject = "Discard Changes";
                        string cancel = "Cancel Navigation";
                        bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                        if (result == true)
                        {
                            if (!SaveEditCondition())
                            {
                                copyCondition = false;
                            }
                        }
                        else if (result == false)
                        {
                            CancelEditCondition();
                        }
                        else
                        {
                            copyCondition = false;
                        }
                    }
					else
					{
						CancelEditCondition();
					}
				}

                if (copyCondition)
                {
                    ConditionModel newCondition = new ConditionModel(_selectedCondition.ConditionModel);
                    newCondition.Name += " (copy)";
                    newCondition.ID = Guid.NewGuid();
                    newCondition.XML = newCondition.XML.Replace("<name>" + _selectedCondition.ConditionModel.Name + "</name>",
                                                                              "<name>" + newCondition.Name + "</name>");

                    _compendium.AddCondition(newCondition);

                    if (_conditionSearchService.SearchInputApplies(_conditionSearchInput, newCondition))
                    {
                        ConditionListItemViewModel listItem = new ConditionListItemViewModel(newCondition);
                        _conditions.Add(listItem);
                        foreach (ConditionListItemViewModel item in _conditions)
                        {
                            item.IsSelected = false;
                        }
                        listItem.IsSelected = true;
                    }

					_selectedCondition = new ConditionViewModel(newCondition);

					SortConditions();

                    _compendium.SaveConditions();

                    OnPropertyChanged(nameof(SelectedCondition));
				}
			}
		}

		private void Delete()
		{
			if (_selectedCondition != null)
			{
                bool canDelete = true;

                foreach (CharacterModel character in _compendium.Characters)
                {
                    foreach (AppliedConditionModel condition in character.Conditions)
                    {
                        if (condition.ConditionModel != null && condition.ConditionModel.ID == _selectedCondition.ConditionModel.ID)
                        {
                            canDelete = false;
                            break;
                        }
                    }

                    if (!canDelete)
                    {
                        break;
                    }
                }

                if (canDelete)
                {
                    string message = String.Format("Are you sure you want to delete {0}?",
                                                                 _selectedCondition.Name);

                    bool? result = _dialogService.ShowConfirmationDialog("Delete Condition", message, "Yes", "No", null);

                    if (result == true)
                    {
                        _compendium.DeleteCondition(_selectedCondition.ConditionModel.ID);

                        ConditionListItemViewModel listItem = _conditions.FirstOrDefault(x => x.ConditionModel.ID == _selectedCondition.ConditionModel.ID);
                        if (listItem != null)
                        {
                            _conditions.Remove(listItem);
                        }

                        _selectedCondition = null;

                        _compendium.SaveConditions();

                        OnPropertyChanged(nameof(SelectedCondition));

                        if (_editConditionCommand != null)
                        {
                            CancelEditCondition();
                        }
                    }
                }
                else
                {
                    _dialogService.ShowConfirmationDialog("Unable Delete Condition", "Condition is in use by a character.", "OK", null, null);
                }
			}
		}

		private void SortConditions()
		{
			if (_conditions != null && _conditions.Count > 0)
			{
                List<ConditionModel> conditions = _conditionSearchService.Sort(_conditions.Select(x => x.ConditionModel), _conditionSearchInput.SortOption);
                for (int i = 0; i < conditions.Count; ++i)
                {
                    if (conditions[i].ID != _conditions[i].ConditionModel.ID)
                    {
                        ConditionListItemViewModel conditionListItem = _conditions.FirstOrDefault(x => x.ConditionModel.ID == conditions[i].ID);
                        if (conditionListItem != null)
                        {
                            _conditions.Move(_conditions.IndexOf(conditionListItem), i);
                        }
                    }
                }
			}
		}

        private void ExportCondition(ConditionViewModel conditionViewModel)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "XML Document|*.xml";
            saveFileDialog.Title = "Save Condition";
            saveFileDialog.FileName = conditionViewModel.Name;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string ext = System.IO.Path.GetExtension(saveFileDialog.FileName);

                    if (ext == ".xml")
                    {
                        string xml = _xmlExporter.FormatXMLWithHeader(_xmlExporter.GetXML(conditionViewModel.ConditionModel));
                        System.IO.File.WriteAllText(saveFileDialog.FileName, xml, System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        _dialogService.ShowConfirmationDialog("Unable To Export", "Invalid file extension.", "OK", null, null);
                    }
                }
                catch (Exception)
                {
                    _dialogService.ShowConfirmationDialog("Unable To Export", "An error occurred when attempting to export the condition.", "OK", null, null);
                }
            }
        }

        private void Import()
        {
            _dialogService.ShowImportView();
        }

        private void SelectNext()
        {
            if (_conditions.Any())
            {
                ConditionListItemViewModel selected = _conditions.FirstOrDefault(x => x.IsSelected);

                foreach (ConditionListItemViewModel condition in _conditions)
                {
                    condition.IsSelected = false;
                }

                if (selected == null)
                {
                    _conditions[0].IsSelected = true;
                    _selectedCondition = new ConditionViewModel(_conditions[0].ConditionModel);
                }
                else
                {
                    int index = Math.Min(_conditions.IndexOf(selected) + 1, _conditions.Count - 1);
                    _conditions[index].IsSelected = true;
                    _selectedCondition = new ConditionViewModel(_conditions[index].ConditionModel);
                }

                OnPropertyChanged(nameof(SelectedCondition));
            }
        }

        private void SelectPrevious()
        {
            if (_conditions.Any())
            {
                ConditionListItemViewModel selected = _conditions.FirstOrDefault(x => x.IsSelected);

                foreach (ConditionListItemViewModel condition in _conditions)
                {
                    condition.IsSelected = false;
                }

                if (selected == null)
                {
                    _conditions[_conditions.Count - 1].IsSelected = true;
                    _selectedCondition = new ConditionViewModel(_conditions[_conditions.Count - 1].ConditionModel);
                }
                else
                {
                    int index = Math.Max(_conditions.IndexOf(selected) - 1, 0);
                    _conditions[index].IsSelected = true;
                    _selectedCondition = new ConditionViewModel(_conditions[index].ConditionModel);
                }

                OnPropertyChanged(nameof(SelectedCondition));
            }
        }

        #endregion
    }
}
