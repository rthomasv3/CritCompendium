using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendium.Services;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendium.ViewModels.ObjectViewModels;
using CriticalCompendiumInfrastructure.Enums;
using CriticalCompendiumInfrastructure.Models;
using CriticalCompendiumInfrastructure.Persistence;
using CriticalCompendiumInfrastructure.Services;
using CriticalCompendiumInfrastructure.Services.Search;
using CriticalCompendiumInfrastructure.Services.Search.Input;

namespace CritCompendium.ViewModels
{
	public sealed class BackgroundsViewModel :NotifyPropertyChanged
    {
		#region Fields

		private readonly Compendium _compendium;
		private readonly BackgroundSearchService _backgroundSearchService;
		private readonly StringService _stringService;
		private readonly DialogService _dialogService;
		private readonly XMLImporter _xmlImporter;
		private readonly XMLExporter _xmlExporter;
        private readonly ObservableCollection<BackgroundListItemViewModel> _backgrounds = new ObservableCollection<BackgroundListItemViewModel>();
		private readonly ICommand _selectBackgroundCommand;
		private readonly ICommand _editBackgroundCommand;
		private readonly ICommand _exportBackgroundCommand;
        private readonly ICommand _cancelEditBackgroundCommand;
		private readonly ICommand _saveEditBackgroundCommand;
		private readonly ICommand _resetFiltersCommand;
		private readonly ICommand _addCommand;
		private readonly ICommand _copyCommand;
		private readonly ICommand _deleteCommand;
        private readonly ICommand _importCommand;
        private readonly ICommand _selectNextCommand;
        private readonly ICommand _selectPreviousCommand;
        private BackgroundSearchInput _backgroundSearchInput;
		private BackgroundViewModel _selectedBackground;
		private string _editBackgroundXML;
		private bool _editHasUnsavedChanges;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of <see cref="BackgroundsViewModel"/>
		/// </summary>
		public BackgroundsViewModel(Compendium compendium, BackgroundSearchService backgroundSearchService,  BackgroundSearchInput backgroundSearchInput,
			StringService stringService, DialogService dialogService, XMLImporter xmlImporter, XMLExporter xmlExporter)
		{
			_compendium = compendium;
			_backgroundSearchService = backgroundSearchService;
            _backgroundSearchInput = backgroundSearchInput;
			_stringService = stringService;
			_dialogService = dialogService;
			_xmlImporter = xmlImporter;
            _xmlExporter = xmlExporter;

			_selectBackgroundCommand = new RelayCommand(obj => true, obj => SelectBackground(obj as BackgroundListItemViewModel));
			_editBackgroundCommand = new RelayCommand(obj => true, obj => EditBackground(obj as BackgroundViewModel));
			_exportBackgroundCommand = new RelayCommand(obj => true, obj => ExportBackground(obj as BackgroundViewModel));
            _cancelEditBackgroundCommand = new RelayCommand(obj => true, obj => CancelEditBackground());
			_saveEditBackgroundCommand = new RelayCommand(obj => HasUnsavedChanges, obj => SaveEditBackground());
			_resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
			_addCommand = new RelayCommand(obj => true, obj => Add());
			_copyCommand = new RelayCommand(obj => _selectedBackground != null, obj => Copy());
			_deleteCommand = new RelayCommand(obj => _selectedBackground != null, obj => Delete());
            _importCommand = new RelayCommand(obj => true, obj => Import());
            _selectNextCommand = new RelayCommand(obj => true, obj => SelectNext());
            _selectPreviousCommand = new RelayCommand(obj => true, obj => SelectPrevious());

            Search();
		}

        #endregion

        #region Properties

        /// <summary>
        /// List of backgrounds
        /// </summary>
        public ObservableCollection<BackgroundListItemViewModel> Backgrounds
		{
			get { return _backgrounds; }
		}

		/// <summary>
		/// Gets or sets the search text
		/// </summary>
		public string SearchText
		{
			get { return _backgroundSearchInput.SearchText; }
			set
			{
				_backgroundSearchInput.SearchText = value;
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
                return _backgroundSearchInput.AppliedFilterCount > 0 ? $"Sort and Filter ({_backgroundSearchInput.AppliedFilterCount})" : "Sort and Filter";
            }
        }

        /// <summary>
        /// Gets or sets sort and filters expanded
        /// </summary>
        public bool SortAndFilterExpanded
        {
            get { return _backgroundSearchInput.SortAndFiltersExpanded; }
            set { _backgroundSearchInput.SortAndFiltersExpanded = value; }
        }

        /// <summary>
        /// Gets sort options
        /// </summary>
        public List<KeyValuePair<BackgroundSortOption, string>> SortOptions
        {
            get { return _backgroundSearchInput.SortOptions; }
        }

        /// <summary>
        /// Gets selected sort option
        /// </summary>
        public KeyValuePair<BackgroundSortOption, string> SelectedSortOption
        {
            get { return _backgroundSearchInput.SortOption; }
            set
            {
                _backgroundSearchInput.SortOption = value;
                Search();
            }
        }

        /// <summary>
        /// List of skills
        /// </summary>
        public List<KeyValuePair<Skill, string>> Skills
		{
			get { return _backgroundSearchInput.Skills; }
		}

		/// <summary>
		/// Selected skill
		/// </summary>
		public KeyValuePair<Skill, string> SelectedSkill
		{
			get { return _backgroundSearchInput.Skill; }
			set
			{
				_backgroundSearchInput.Skill = value;
				Search();
			}
		}

		/// <summary>
		/// List of tools
		/// </summary>
		public List<KeyValuePair<Enum, string>> Tools
		{
			get { return _backgroundSearchInput.Tools; }
		}

		/// <summary>
		/// Selected tool
		/// </summary>
		public KeyValuePair<Enum, string> SelectedTool
		{
			get { return _backgroundSearchInput.Tool; }
			set
			{
				_backgroundSearchInput.Tool = value;
				Search();
			}
		}

		/// <summary>
		/// List of languages
		/// </summary>
		public List<KeyValuePair<LanguageModel, string>> Languages
		{
			get { return _backgroundSearchInput.Languages; }
		}

		/// <summary>
		/// Selected language
		/// </summary>
		public KeyValuePair<LanguageModel, string> SelectedLanguage
		{
			get { return _backgroundSearchInput.Language; }
			set
			{
				_backgroundSearchInput.Language = value;
				Search();
			}
		}

		/// <summary>
		/// Command to select a background
		/// </summary>
		public ICommand SelectBackgroundCommand
		{
			get { return _selectBackgroundCommand; }
		}

		/// <summary>
		/// Selected background
		/// </summary>
		public BackgroundViewModel SelectedBackground
		{
			get { return _selectedBackground; }
		}

		/// <summary>
		/// Editing background xml
		/// </summary>
		public string EditingBackgroundXML
		{
			get { return _editBackgroundXML; }
			set
			{
				if (Set(ref _editBackgroundXML, value) && !_editHasUnsavedChanges)
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
		/// Command to edit background
		/// </summary>
		public ICommand EditBackgroundCommand
		{
			get { return _editBackgroundCommand; }
		}

        /// <summary>
		/// Command to export background
		/// </summary>
		public ICommand ExportBackgroundCommand
        {
            get { return _exportBackgroundCommand; }
        }


        /// <summary>
        /// Command to cancel edit background
        /// </summary>
        public ICommand CancelEditBackgroundCommand
		{
			get { return _cancelEditBackgroundCommand; }
		}

		/// <summary>
		/// Command to save edit background
		/// </summary>
		public ICommand SaveEditBackgroundCommand
		{
			get { return _saveEditBackgroundCommand; }
		}

		/// <summary>
		/// Command to add background
		/// </summary>
		public ICommand AddBackgroundCommand
		{
			get { return _addCommand; }
		}

		/// <summary>
		/// Command to copy background
		/// </summary>
		public ICommand CopyBackgroundCommand
		{
			get { return _copyCommand; }
		}

		/// <summary>
		/// Command to delete selected background
		/// </summary>
		public ICommand DeleteBackgroundCommand
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
        /// True if currently editing a background
        /// </summary>
        public bool IsEditingBackground
		{
			get { return _editBackgroundXML != null; }
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
            _backgrounds.Clear();
            foreach (BackgroundModel backgroundModel in _backgroundSearchService.Search(_backgroundSearchInput))
            {
                _backgrounds.Add(new BackgroundListItemViewModel(backgroundModel, _stringService));
            }

            if (_selectedBackground != null)
            {
                BackgroundListItemViewModel background = _backgrounds.FirstOrDefault(x => x.BackgroundModel.ID == _selectedBackground.BackgroundModel.ID);
                if (background != null)
                {
                    background.IsSelected = true;
                }
            }

            OnPropertyChanged(nameof(SortAndFilterHeader));
        }

        #endregion

        #region Non-Public Methods

        private void InitializeSearch()
		{
            _backgroundSearchInput.Reset();

			OnPropertyChanged(nameof(SearchText));
			OnPropertyChanged(nameof(SelectedSortOption));
            OnPropertyChanged(nameof(SelectedSkill));
			OnPropertyChanged(nameof(SelectedTool));
			OnPropertyChanged(nameof(SelectedLanguage));

			Search();
		}

        private void SelectBackground(BackgroundListItemViewModel background)
		{
			bool selectBackground = true;

			if (_editBackgroundXML != null)
			{
				if (_editHasUnsavedChanges)
				{
					string body = String.Format("{0} has unsaved changes.{1}What would you like to do?", 
														 _selectedBackground.Name, Environment.NewLine + Environment.NewLine);
					string accept = "Save and Continue";
					string reject = "Discard Changes";
					string cancel = "Cancel Navigation";
					bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

					if (result == true)
					{
                        if (!SaveEditBackground())
                        {
                            selectBackground = false;
                        }
					}
					else if (result == false)
					{
                        CancelEditBackground();
                    }
					else
					{
                        selectBackground = false;
                    }
				}
				else
				{
					CancelEditBackground();
				}
			}

			if (selectBackground)
			{
				foreach (BackgroundListItemViewModel item in _backgrounds)
				{
					item.IsSelected = false;
				}
				background.IsSelected = true;

				_selectedBackground = new BackgroundViewModel(background.BackgroundModel);
				OnPropertyChanged(nameof(SelectedBackground));
			}
		}

		private void EditBackground(BackgroundViewModel background)
		{
			_editBackgroundXML = background.XML;

			OnPropertyChanged(nameof(EditingBackgroundXML));
			OnPropertyChanged(nameof(IsEditingBackground));
		}

		private void CancelEditBackground()
		{
			_editHasUnsavedChanges = false;
			_editBackgroundXML = null;

			OnPropertyChanged(nameof(EditingBackgroundXML));
			OnPropertyChanged(nameof(IsEditingBackground));
			OnPropertyChanged(nameof(HasUnsavedChanges));
		}

		private bool SaveEditBackground()
		{
			bool saved = false;

			try
			{
				BackgroundModel model = _xmlImporter.GetBackground(_editBackgroundXML);

				if (model != null)
                {
                    model.ID = _selectedBackground.BackgroundModel.ID;
                    _compendium.UpdateBackground(model);
                    _selectedBackground = new BackgroundViewModel(model);

                    BackgroundListItemViewModel oldListItem = _backgrounds.FirstOrDefault(x => x.BackgroundModel.ID == model.ID);
					if (oldListItem != null)
					{
                        if (_backgroundSearchService.SearchInputApplies(_backgroundSearchInput, model))
                        {
                            oldListItem.UpdateModel(model);
                        }
                        else
                        {
                            _backgrounds.Remove(oldListItem);
                        }
					}

					_editBackgroundXML = null;
					_editHasUnsavedChanges = false;
					
					SortBackgrounds();

                    _compendium.SaveBackgrounds();

                    OnPropertyChanged(nameof(SelectedBackground));
					OnPropertyChanged(nameof(EditingBackgroundXML));
					OnPropertyChanged(nameof(IsEditingBackground));
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
			bool addBackground = true;

			if (_editBackgroundXML != null)
			{
				if (_editHasUnsavedChanges)
				{
					string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
														 _selectedBackground.Name, Environment.NewLine + Environment.NewLine);
                    string accept = "Save and Continue";
                    string reject = "Discard Changes";
                    string cancel = "Cancel Navigation";
                    bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                    if (result == true)
                    {
                        if (!SaveEditBackground())
                        {
                            addBackground = false;
                        }
                    }
                    else if (result == false)
                    {
                        CancelEditBackground();
                    }
                    else
                    {
                        addBackground = false;
                    }
                }
				else
				{
					CancelEditBackground();
				}
			}

			if (addBackground)
			{
				string xml = "<name>New Background</name><proficiency></proficiency><trait><name></name><text></text></trait>";

				BackgroundModel backgroundModel = _xmlImporter.GetBackground(xml);

				_compendium.AddBackground(backgroundModel);

                if (_backgroundSearchService.SearchInputApplies(_backgroundSearchInput, backgroundModel))
                {
                    BackgroundListItemViewModel listItem = new BackgroundListItemViewModel(backgroundModel, _stringService);
                    _backgrounds.Add(listItem);
                    foreach (BackgroundListItemViewModel item in _backgrounds)
                    {
                        item.IsSelected = false;
                    }
                    listItem.IsSelected = true;
                }

				_selectedBackground = new BackgroundViewModel(backgroundModel);

				_editBackgroundXML = backgroundModel.XML;

				SortBackgrounds();

                _compendium.SaveBackgrounds();

                OnPropertyChanged(nameof(EditingBackgroundXML));
				OnPropertyChanged(nameof(IsEditingBackground));
				OnPropertyChanged(nameof(SelectedBackground));
			}
		}

		private void Copy()
		{
			if (_selectedBackground != null)
			{
				bool copyBackground = true;

				if (_editBackgroundXML != null)
				{
					if (_editHasUnsavedChanges)
					{
						string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
															 _selectedBackground.Name, Environment.NewLine + Environment.NewLine);
                        string accept = "Save and Continue";
                        string reject = "Discard Changes";
                        string cancel = "Cancel Navigation";
                        bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                        if (result == true)
                        {
                            if (!SaveEditBackground())
                            {
                                copyBackground = false;
                            }
                        }
                        else if (result == false)
                        {
                            CancelEditBackground();
                        }
                        else
                        {
                            copyBackground = false;
                        }
                    }
					else
					{
						CancelEditBackground();
					}
				}

				if (copyBackground)
				{
					BackgroundModel newBackground = new BackgroundModel(_selectedBackground.BackgroundModel);
					newBackground.Name += " (copy)";
					newBackground.ID = Guid.NewGuid();
					newBackground.XML = newBackground.XML.Replace("<name>" + _selectedBackground.BackgroundModel.Name + "</name>",
																				 "<name>" + newBackground.Name + "</name>");

					_compendium.AddBackground(newBackground);

                    if (_backgroundSearchService.SearchInputApplies(_backgroundSearchInput, newBackground))
                    {
                        BackgroundListItemViewModel listItem = new BackgroundListItemViewModel(newBackground, _stringService);
                        _backgrounds.Add(listItem);
                        foreach (BackgroundListItemViewModel item in _backgrounds)
                        {
                            item.IsSelected = false;
                        }
                        listItem.IsSelected = true;
                    }

					_selectedBackground = new BackgroundViewModel(newBackground);

					SortBackgrounds();

                    _compendium.SaveBackgrounds();

                    OnPropertyChanged(nameof(SelectedBackground));
				}
			}
		}

		private void Delete()
		{
			if (_selectedBackground != null)
			{
                bool canDelete = true;

                foreach (CharacterModel character in _compendium.Characters)
                {
                    if (character.Background != null && character.Background.ID == _selectedBackground.BackgroundModel.ID)
                    {
                        canDelete = false;
                        break;
                    }
                }

                if (canDelete)
                {
                    string message = String.Format("Are you sure you want to delete {0}?",
                                                                 _selectedBackground.Name);

                    bool? result = _dialogService.ShowConfirmationDialog("Delete Background", message, "Yes", "No", null);

                    if (result == true)
                    {
                        _compendium.DeleteBackground(_selectedBackground.BackgroundModel.ID);

                        BackgroundListItemViewModel listItem = _backgrounds.FirstOrDefault(x => x.BackgroundModel.ID == _selectedBackground.BackgroundModel.ID);
                        if (listItem != null)
                        {
                            _backgrounds.Remove(listItem);
                        }

                        _selectedBackground = null;

                        _compendium.SaveBackgrounds();

                        OnPropertyChanged(nameof(SelectedBackground));

                        if (_editBackgroundXML != null)
                        {
                            CancelEditBackground();
                        }
                    }
                }
                else
                {
                    _dialogService.ShowConfirmationDialog("Unable Delete Background", "Background is in use by a character.", "OK", null, null);
                }
			}
		}

		private void SortBackgrounds()
		{
			if (_backgrounds != null && _backgrounds.Count > 0)
			{
                List<BackgroundModel> backgrounds = _backgroundSearchService.Sort(_backgrounds.Select(x => x.BackgroundModel), _backgroundSearchInput.SortOption.Key);
                for (int i = 0; i < backgrounds.Count; ++i)
                {
                    if (backgrounds[i].ID != _backgrounds[i].BackgroundModel.ID)
                    {
                        BackgroundListItemViewModel background = _backgrounds.FirstOrDefault(x => x.BackgroundModel.ID == backgrounds[i].ID);
                        if (background != null)
                        {
                            _backgrounds.Move(_backgrounds.IndexOf(background), i);
                        }
                    }
                }
			}
		}

        private void ExportBackground(BackgroundViewModel backgroundViewModel)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "XML Document|*.xml";
            saveFileDialog.Title = "Save Background";
            saveFileDialog.FileName = backgroundViewModel.Name;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string ext = System.IO.Path.GetExtension(saveFileDialog.FileName);

                    if (ext == ".xml")
                    {
                        string xml = _xmlExporter.FormatXMLWithHeader(_xmlExporter.GetXML(backgroundViewModel.BackgroundModel));
                        System.IO.File.WriteAllText(saveFileDialog.FileName, xml, System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        _dialogService.ShowConfirmationDialog("Unable To Export", "Invalid file extension.", "OK", null, null);
                    }
                }
                catch (Exception)
                {
                    _dialogService.ShowConfirmationDialog("Unable To Export", "An error occurred when attempting to export the background.", "OK", null, null);
                }
            }
        }

        private void Import()
        {
            _dialogService.ShowImportView();
        }

        private void SelectNext()
        {
            if (_backgrounds.Any())
            {
                BackgroundListItemViewModel selected = _backgrounds.FirstOrDefault(x => x.IsSelected);

                foreach (BackgroundListItemViewModel background in _backgrounds)
                {
                    background.IsSelected = false;
                }

                if (selected == null)
                {
                    _backgrounds[0].IsSelected = true;
                    _selectedBackground = new BackgroundViewModel(_backgrounds[0].BackgroundModel);
                }
                else
                {
                    int index = Math.Min(_backgrounds.IndexOf(selected) + 1, _backgrounds.Count - 1);
                    _backgrounds[index].IsSelected = true;
                    _selectedBackground = new BackgroundViewModel(_backgrounds[index].BackgroundModel);
                }

                OnPropertyChanged(nameof(SelectedBackground));
            }
        }

        private void SelectPrevious()
        {
            if (_backgrounds.Any())
            {
                BackgroundListItemViewModel selected = _backgrounds.FirstOrDefault(x => x.IsSelected);

                foreach (BackgroundListItemViewModel background in _backgrounds)
                {
                    background.IsSelected = false;
                }

                if (selected == null)
                {
                    _backgrounds[_backgrounds.Count - 1].IsSelected = true;
                    _selectedBackground = new BackgroundViewModel(_backgrounds[_backgrounds.Count - 1].BackgroundModel);
                }
                else
                {
                    int index = Math.Max(_backgrounds.IndexOf(selected) - 1, 0);
                    _backgrounds[index].IsSelected = true;
                    _selectedBackground = new BackgroundViewModel(_backgrounds[index].BackgroundModel);
                }

                OnPropertyChanged(nameof(SelectedBackground));
            }
        }

        #endregion
    }
}
