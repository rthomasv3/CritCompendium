using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CriticalCompendiumInfrastructure.Enums;
using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Persistence;
using CriticalCompendiumInfrastructure.Services;
using CriticalCompendiumInfrastructure.Services.Search;
using CriticalCompendiumInfrastructure.Services.Search.Input;
using CritCompendium.Services;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendium.ViewModels.ObjectViewModels;

namespace CritCompendium.ViewModels
{
	public sealed class SpellsViewModel : NotifyPropertyChanged
	{
		#region Fields

		private readonly Compendium _compendium;
		private readonly SpellSearchService _spellSearchService;
        private readonly SpellSearchInput _spellSearchInput;
        private readonly StringService _stringService;
		private readonly DialogService _dialogService;
		private readonly XMLImporter _xmlImporter;
        private readonly XMLExporter _xmlExporter;
		private readonly DocumentService _documentService;
        private readonly ObservableCollection<SpellListItemViewModel> _spells = new ObservableCollection<SpellListItemViewModel>();
		private readonly ICommand _selectSpellCommand;
		private readonly ICommand _editSpellCommand;
		private readonly ICommand _exportSpellCommand;
        private readonly ICommand _cancelEditSpellCommand;
		private readonly ICommand _saveEditSpellCommand;
		private readonly ICommand _resetFiltersCommand;
		private readonly ICommand _addCommand;
		private readonly ICommand _copyCommand;
		private readonly ICommand _deleteCommand;
        private readonly ICommand _importCommand;
        private readonly ICommand _selectNextCommand;
        private readonly ICommand _selectPreviousCommand;
        private SpellViewModel _selectedSpell;
		private string _editSpellXML;
		private bool _editHasUnsavedChanges;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of <see cref="SpellsViewModel"/>
		/// </summary>
		public SpellsViewModel(Compendium compendium, SpellSearchService spellSearchService, SpellSearchInput spellSearchInput,
			StringService stringService, DialogService dialogService, XMLImporter xmlImporter, XMLExporter xmlExporter, DocumentService documentService)
		{
			_compendium = compendium;
			_spellSearchService = spellSearchService;
            _spellSearchInput = spellSearchInput;
			_stringService = stringService;
			_dialogService = dialogService;
			_xmlImporter = xmlImporter;
            _xmlExporter = xmlExporter;
            _documentService = documentService;

			_selectSpellCommand = new RelayCommand(obj => true, obj => SelectSpell(obj as SpellListItemViewModel));
			_editSpellCommand = new RelayCommand(obj => true, obj => EditSpell(obj as SpellViewModel));
			_exportSpellCommand = new RelayCommand(obj => true, obj => ExportSpell(obj as SpellViewModel));
            _cancelEditSpellCommand = new RelayCommand(obj => true, obj => CancelEditSpell());
			_saveEditSpellCommand = new RelayCommand(obj => HasUnsavedChanges, obj => SaveEditSpell());
			_resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
			_addCommand = new RelayCommand(obj => true, obj => Add());
			_copyCommand = new RelayCommand(obj => _selectedSpell != null, obj => Copy());
			_deleteCommand = new RelayCommand(obj => _selectedSpell != null, obj => Delete());
            _importCommand = new RelayCommand(obj => true, obj => Import());
            _selectNextCommand = new RelayCommand(obj => true, obj => SelectNext());
            _selectPreviousCommand = new RelayCommand(obj => true, obj => SelectPrevious());

            InitializeSearch();
		}

        #endregion

        #region Properties

        /// <summary>
        /// List of spells
        /// </summary>
        public ObservableCollection<SpellListItemViewModel> Spells
		{
			get { return _spells; }
		}

		/// <summary>
		/// Gets or sets the search text
		/// </summary>
		public string SearchText
		{
			get { return _spellSearchInput.SearchText; }
			set
			{
				_spellSearchInput.SearchText = value;
				Search();
			}
        }

        /// <summary>
        /// Gets sort and filter header
        /// </summary>
        public string SortAndFilterHeader
        {
            get
            {
                return _spellSearchInput.AppliedFilterCount > 0 ? $"Sort and Filter ({_spellSearchInput.AppliedFilterCount})" : "Sort and Filter";
            }
        }

        /// <summary>
        /// Gets or sets sort and filters expanded
        /// </summary>
        public bool SortAndFiltersExpanded
        {
            get { return _spellSearchInput.SortAndFiltersExpanded; }
            set { _spellSearchInput.SortAndFiltersExpanded = value; }
        }

        /// <summary>
        /// Gets sort options
        /// </summary>
        public List<KeyValuePair<SpellSortOption, string>> SortOptions
        {
            get { return _spellSearchInput.SortOptions; }
        }

        /// <summary>
        /// Gets selected sort option
        /// </summary>
        public KeyValuePair<SpellSortOption, string> SelectedSortOption
        {
            get { return _spellSearchInput.SortOption; }
            set
            {
                _spellSearchInput.SortOption = value;
                Search();
            }
        }

        /// <summary>
        /// List of levels
        /// </summary>
        public List<KeyValuePair<int?, string>> Levels
		{
			get { return _spellSearchInput.Levels; }
		}

		/// <summary>
		/// Selected level
		/// </summary>
		public KeyValuePair<int?, string> SelectedLevel
		{
			get { return _spellSearchInput.Level; }
			set
			{
				_spellSearchInput.Level = value;
				Search();
			}
		}

		/// <summary>
		/// List of schools
		/// </summary>
		public List<KeyValuePair<SpellSchool, string>> Schools
		{
			get { return _spellSearchInput.Schools; }
		}

		/// <summary>
		/// Selected school
		/// </summary>
		public KeyValuePair<SpellSchool, string> SelectedSchool
		{
			get { return _spellSearchInput.School; }
			set
			{
				_spellSearchInput.School = value;
				Search();
			}
		}

		/// <summary>
		/// List of classes
		/// </summary>
		public List<KeyValuePair<string, string>> Classes
		{
			get { return _spellSearchInput.Classes; }
		}

		/// <summary>
		/// Selected class
		/// </summary>
		public KeyValuePair<string, string> SelectedClass
		{
			get { return _spellSearchInput.Class; }
			set
			{
				_spellSearchInput.Class = value;
				Search();
			}
		}

		/// <summary>
		/// List of concentration options
		/// </summary>
		public List<KeyValuePair<bool?, string>> ConcentrationOptions
		{
			get { return _spellSearchInput.ConcentrationOptions; }
		}

		/// <summary>
		/// Selected concentration option
		/// </summary>
		public KeyValuePair<bool?, string> SelectedConcentrationOption
		{
			get { return _spellSearchInput.Concentration; }
			set
			{
				_spellSearchInput.Concentration = value;
				Search();
			}
		}

		/// <summary>
		/// List of ritual options
		/// </summary>
		public List<KeyValuePair<bool?, string>> RitualOptions
		{
			get { return _spellSearchInput.RitualOptions; }
		}

		/// <summary>
		/// Selected ritual option
		/// </summary>
		public KeyValuePair<bool?, string> SelectedRitualOption
		{
			get { return _spellSearchInput.Ritual; }
			set
			{
				_spellSearchInput.Ritual = value;
				Search();
			}
		}

		/// <summary>
		/// Command to select a spell
		/// </summary>
		public ICommand SelectSpellCommand
		{
			get { return _selectSpellCommand; }
		}

		/// <summary>
		/// Selected spell
		/// </summary>
		public SpellViewModel SelectedSpell
		{
			get { return _selectedSpell; }
		}

		/// <summary>
		/// Editing spell xml
		/// </summary>
		public string EditingSpellXML
		{
			get { return _editSpellXML; }
			set
			{
				if (Set(ref _editSpellXML, value) && !_editHasUnsavedChanges)
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
		/// Command to edit spell
		/// </summary>
		public ICommand EditSpellCommand
		{
			get { return _editSpellCommand; }
		}

        /// <summary>
		/// Command to export spell
		/// </summary>
		public ICommand ExportSpellCommand
        {
            get { return _exportSpellCommand; }
        }

        /// <summary>
        /// Command to cancel edit spell
        /// </summary>
        public ICommand CancelEditSpellCommand
		{
			get { return _cancelEditSpellCommand; }
		}

		/// <summary>
		/// Command to save edit spell
		/// </summary>
		public ICommand SaveEditSpellCommand
		{
			get { return _saveEditSpellCommand; }
		}

		/// <summary>
		/// Command to add spell
		/// </summary>
		public ICommand AddSpellCommand
		{
			get { return _addCommand; }
		}

		/// <summary>
		/// Command to copy spell
		/// </summary>
		public ICommand CopySpellCommand
		{
			get { return _copyCommand; }
		}

		/// <summary>
		/// Command to delete selected spell
		/// </summary>
		public ICommand DeleteSpellCommand
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
        /// True if currently editing a spell
        /// </summary>
        public bool IsEditingSpell
		{
			get { return _editSpellXML != null; }
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
            _spells.Clear();
            foreach (SpellModel spellModel in _spellSearchService.Search(_spellSearchInput))
            {
                _spells.Add(new SpellListItemViewModel(spellModel, _stringService));
            }

            if (_selectedSpell != null)
            {
                SpellListItemViewModel spell = _spells.FirstOrDefault(x => x.SpellModel.ID == _selectedSpell.SpellModel.ID);
                if (spell != null)
                {
                    spell.IsSelected = true;
                }
            }

            OnPropertyChanged(nameof(SortAndFilterHeader));
        }

        #endregion

        #region Private Methods

        private void InitializeSearch()
		{
            _spellSearchInput.Reset();

			OnPropertyChanged(nameof(SearchText));
			OnPropertyChanged(nameof(SelectedSortOption));
            OnPropertyChanged(nameof(SelectedLevel));
			OnPropertyChanged(nameof(SelectedSchool));
			OnPropertyChanged(nameof(SelectedClass));
			OnPropertyChanged(nameof(SelectedConcentrationOption));
			OnPropertyChanged(nameof(SelectedRitualOption));

			Search();
		}

        private void SelectSpell(SpellListItemViewModel spellItem)
		{
			bool selectSpell = true;

			if (_editSpellXML != null)
			{
				if (_editHasUnsavedChanges)
				{
					string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
														 _selectedSpell.Name, Environment.NewLine + Environment.NewLine);
                    string accept = "Save and Continue";
                    string reject = "Discard Changes";
                    string cancel = "Cancel Navigation";
                    bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                    if (result == true)
                    {
                        if (!SaveEditSpell())
                        {
                            selectSpell = false;
                        }
                    }
                    else if (result == false)
                    {
                        CancelEditSpell();
                    }
                    else
                    {
                        selectSpell = false;
                    }
                }
				else
				{
					CancelEditSpell();
				}
			}

			if (selectSpell)
			{
				foreach (SpellListItemViewModel item in _spells)
				{
					item.IsSelected = false;
				}
				spellItem.IsSelected = true;

				_selectedSpell = new SpellViewModel(spellItem.SpellModel);
				OnPropertyChanged(nameof(SelectedSpell));
			}
		}

		private void EditSpell(SpellViewModel spellModel)
		{
			_editSpellXML = spellModel.XML;

			OnPropertyChanged(nameof(EditingSpellXML));
			OnPropertyChanged(nameof(IsEditingSpell));
		}

		private void CancelEditSpell()
		{
			_editHasUnsavedChanges = false;
			_editSpellXML = null;

			OnPropertyChanged(nameof(EditingSpellXML));
			OnPropertyChanged(nameof(IsEditingSpell));
			OnPropertyChanged(nameof(HasUnsavedChanges));
		}

		private bool SaveEditSpell()
		{
			bool saved = false;

			try
			{
				SpellModel model = _xmlImporter.GetSpell(_editSpellXML);

				if (model != null)
				{
					model.ID = _selectedSpell.SpellModel.ID;
                    _compendium.UpdateSpell(model);
					_selectedSpell = new SpellViewModel(model);

					SpellListItemViewModel oldListItem = _spells.FirstOrDefault(x => x.SpellModel.ID == model.ID);
					if (oldListItem != null)
					{
                        if (_spellSearchService.SearchInputApplies(_spellSearchInput, model))
                        {
                            oldListItem.UpdateModel(model);
                        }
                        else
                        {
                            _spells.Remove(oldListItem);
                        }
					}

					_editSpellXML = null;
					_editHasUnsavedChanges = false;

					SortSpells();

                    _compendium.SaveSpells();

					OnPropertyChanged(nameof(SelectedSpell));
					OnPropertyChanged(nameof(EditingSpellXML));
					OnPropertyChanged(nameof(IsEditingSpell));
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
			bool addSpell = true;

			if (_editSpellXML != null)
			{
				if (_editHasUnsavedChanges)
				{
					string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
														 _selectedSpell.Name, Environment.NewLine + Environment.NewLine);
                    string accept = "Save and Continue";
                    string reject = "Discard Changes";
                    string cancel = "Cancel Navigation";
                    bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                    if (result == true)
                    {
                        if (!SaveEditSpell())
                        {
                            addSpell = false;
                        }
                    }
                    else if (result == false)
                    {
                        CancelEditSpell();
                    }
                    else
                    {
                        addSpell = false;
                    }
                }
				else
				{
					CancelEditSpell();
				}
			}

			if (addSpell)
			{
				string xml = @"<name>New Spell</name>
									<level></level>
									<school></school>
									<ritual></ritual>
									<time></time>
									<range></range>
									<components></components>
									<duration></duration>
									<classes></classes>
									<text></text>
									<roll></roll>";

				SpellModel spellModel = _xmlImporter.GetSpell(xml);

				_compendium.AddSpell(spellModel);

                if (_spellSearchService.SearchInputApplies(_spellSearchInput, spellModel))
                {
                    SpellListItemViewModel listItem = new SpellListItemViewModel(spellModel, _stringService);
                    _spells.Add(listItem);
                    foreach (SpellListItemViewModel item in _spells)
                    {
                        item.IsSelected = false;
                    }
                    listItem.IsSelected = true;
                }

				_selectedSpell = new SpellViewModel(spellModel);

				_editSpellXML = spellModel.XML;

				SortSpells();

                _compendium.SaveSpells();

                OnPropertyChanged(nameof(EditingSpellXML));
				OnPropertyChanged(nameof(IsEditingSpell));
				OnPropertyChanged(nameof(SelectedSpell));
			}
		}

		private void Copy()
		{
			if (_selectedSpell != null)
			{
				bool copySpell = true;

				if (_editSpellXML != null)
				{
					if (_editHasUnsavedChanges)
					{
						string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
															 _selectedSpell.Name, Environment.NewLine + Environment.NewLine);
                        string accept = "Save and Continue";
                        string reject = "Discard Changes";
                        string cancel = "Cancel Navigation";
                        bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                        if (result == true)
                        {
                            if (!SaveEditSpell())
                            {
                                copySpell = false;
                            }
                        }
                        else if (result == false)
                        {
                            CancelEditSpell();
                        }
                        else
                        {
                            copySpell = false;
                        }
                    }
					else
					{
						CancelEditSpell();
					}
				}

				if (copySpell)
				{
					SpellModel newSpell = new SpellModel(_selectedSpell.SpellModel);
					newSpell.Name += " (copy)";
					newSpell.ID = Guid.NewGuid();
					newSpell.XML = newSpell.XML.Replace("<name>" + _selectedSpell.SpellModel.Name + "</name>",
																   "<name>" + newSpell.Name + "</name>");

					_compendium.AddSpell(newSpell);

                    if (_spellSearchService.SearchInputApplies(_spellSearchInput, newSpell))
                    {
                        SpellListItemViewModel listItem = new SpellListItemViewModel(newSpell, _stringService);
                        _spells.Add(listItem);
                        foreach (SpellListItemViewModel item in _spells)
                        {
                            item.IsSelected = false;
                        }
                        listItem.IsSelected = true;
                    }

					_selectedSpell = new SpellViewModel(newSpell);

					SortSpells();

                    _compendium.SaveSpells();

                    OnPropertyChanged(nameof(SelectedSpell));
				}
			}
		}

		private void Delete()
		{
			if (_selectedSpell != null)
			{
                bool canDelete = true;

                foreach (CharacterModel character in _compendium.Characters)
                {
                    foreach (SpellbookModel spellbook in character.Spellbooks)
                    {
                        foreach (SpellbookEntryModel spellbookEntry in spellbook.Spells)
                        {
                            if (spellbookEntry.Spell != null && spellbookEntry.Spell.ID == _selectedSpell.SpellModel.ID)
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

                    if (!canDelete)
                    {
                        break;
                    }
                }

                if (canDelete)
                {
                    string message = String.Format("Are you sure you want to delete {0}?",
                                                             _selectedSpell.Name);

                    bool? result = _dialogService.ShowConfirmationDialog("Delete Spell", message, "Yes", "No", null);

                    if (result == true)
                    {
                        _compendium.DeleteSpell(_selectedSpell.SpellModel.ID);

                        SpellListItemViewModel listItem = _spells.FirstOrDefault(x => x.SpellModel.ID == _selectedSpell.SpellModel.ID);
                        if (listItem != null)
                        {
                            _spells.Remove(listItem);
                        }

                        _selectedSpell = null;

                        _compendium.SaveSpells();

                        OnPropertyChanged(nameof(SelectedSpell));

                        if (_editSpellXML != null)
                        {
                            CancelEditSpell();
                        }
                    }
                }
                else
                {
                    _dialogService.ShowConfirmationDialog("Unable Delete Spell", "Spell is in use by a character.", "OK", null, null);
                }
			}
		}

		private void SortSpells()
		{
			if (_spells != null && _spells.Count > 0)
			{
                List<SpellModel> spells = _spellSearchService.Sort(_spells.Select(x => x.SpellModel), _spellSearchInput.SortOption.Key);
                for (int i = 0; i < spells.Count; ++i)
                {
                    if (spells[i].ID != _spells[i].SpellModel.ID)
                    {
                        SpellListItemViewModel spell = _spells.FirstOrDefault(x => x.SpellModel.ID == spells[i].ID);
                        if (spell != null)
                        {
                            _spells.Move(_spells.IndexOf(spell), i);
                        }
                    }
                }
			}
        }

        private void ExportSpell(SpellViewModel spellViewModel)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Word Document|*.docx|XML Document|*.xml";
            saveFileDialog.Title = "Save Spell";
            saveFileDialog.FileName = spellViewModel.Name;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string ext = System.IO.Path.GetExtension(saveFileDialog.FileName);

                    if (ext == ".docx")
                    {
                        _documentService.CreateWordDoc(saveFileDialog.FileName, spellViewModel.SpellModel);
                    }
                    else if (ext == ".xml")
                    {
                        string xml = _xmlExporter.FormatXMLWithHeader(_xmlExporter.GetXML(spellViewModel.SpellModel));
                        System.IO.File.WriteAllText(saveFileDialog.FileName, xml, System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        _dialogService.ShowConfirmationDialog("Unable To Export", "Invalid file extension.", "OK", null, null);
                    }
                }
                catch (Exception)
                {
                    _dialogService.ShowConfirmationDialog("Unable To Export", "An error occurred when attempting to export the spell.", "OK", null, null);
                }
            }
        }

        private void Import()
        {
            _dialogService.ShowImportView();
        }

        private void SelectNext()
        {
            if (_spells.Any())
            {
                SpellListItemViewModel selected = _spells.FirstOrDefault(x => x.IsSelected);

                foreach (SpellListItemViewModel spell in _spells)
                {
                    spell.IsSelected = false;
                }

                if (selected == null)
                {
                    _spells[0].IsSelected = true;
                    _selectedSpell = new SpellViewModel(_spells[0].SpellModel);
                }
                else
                {
                    int index = Math.Min(_spells.IndexOf(selected) + 1, _spells.Count - 1);
                    _spells[index].IsSelected = true;
                    _selectedSpell = new SpellViewModel(_spells[index].SpellModel);
                }

                OnPropertyChanged(nameof(SelectedSpell));
            }
        }

        private void SelectPrevious()
        {
            if (_spells.Any())
            {
                SpellListItemViewModel selected = _spells.FirstOrDefault(x => x.IsSelected);

                foreach (SpellListItemViewModel spell in _spells)
                {
                    spell.IsSelected = false;
                }

                if (selected == null)
                {
                    _spells[_spells.Count - 1].IsSelected = true;
                    _selectedSpell = new SpellViewModel(_spells[_spells.Count - 1].SpellModel);
                }
                else
                {
                    int index = Math.Max(_spells.IndexOf(selected) - 1, 0);
                    _spells[index].IsSelected = true;
                    _selectedSpell = new SpellViewModel(_spells[index].SpellModel);
                }

                OnPropertyChanged(nameof(SelectedSpell));
            }
        }

        #endregion
    }
}
