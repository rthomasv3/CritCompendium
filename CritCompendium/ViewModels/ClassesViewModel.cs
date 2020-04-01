using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Persistence;
using CritCompendiumInfrastructure.Services;
using CritCompendiumInfrastructure.Services.Search;
using CritCompendiumInfrastructure.Services.Search.Input;
using CritCompendium.Services;
using CritCompendium.ViewModels.ListItemViewModels;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendiumInfrastructure;

namespace CritCompendium.ViewModels
{
	public sealed class ClassesViewModel : NotifyPropertyChanged
	{
		#region Fields

		private readonly Compendium _compendium;
		private readonly ClassSearchService _classSearchService;
        private readonly ClassSearchInput _classSearchInput;
        private readonly StringService _stringService;
		private readonly DialogService _dialogService;
		private readonly XMLImporter _xmlImporter;
		private readonly XMLExporter _xmlExporter;
		private readonly DataManager _dataManager;
        private readonly ObservableCollection<ClassListItemViewModel> _classes = new ObservableCollection<ClassListItemViewModel>();
        
		private readonly ICommand _selectClassCommand;
		private readonly ICommand _editClassCommand;
		private readonly ICommand _exportClassCommand;
        private readonly ICommand _cancelEditClassCommand;
		private readonly ICommand _saveEditClassCommand;
		private readonly ICommand _resetFiltersCommand;
		private readonly ICommand _addCommand;
		private readonly ICommand _copyCommand;
		private readonly ICommand _deleteCommand;
        private readonly ICommand _importCommand;
        private readonly ICommand _selectNextCommand;
        private readonly ICommand _selectPreviousCommand;
        private ClassViewModel _selectedClass;
		private string _editClassXML;
		private bool _editHasUnsavedChanges;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of <see cref="ClassesViewModel"/>
		/// </summary>
		public ClassesViewModel(Compendium compendium, ClassSearchService classSearchService, ClassSearchInput classSearchInput,
            StringService stringService, DialogService dialogService, XMLImporter xmlImporter, XMLExporter xmlExporter, DataManager dataManager)
		{
			_compendium = compendium;
			_classSearchService = classSearchService;
            _classSearchInput = classSearchInput;
			_stringService = stringService;
			_dialogService = dialogService;
			_xmlImporter = xmlImporter;
            _xmlExporter = xmlExporter;
            _dataManager = dataManager;

			_selectClassCommand = new RelayCommand(obj => true, obj => SelectClass(obj as ClassListItemViewModel));
			_editClassCommand = new RelayCommand(obj => true, obj => EditClass(obj as ClassViewModel));
			_exportClassCommand = new RelayCommand(obj => true, obj => ExportClass(obj as ClassViewModel));
            _cancelEditClassCommand = new RelayCommand(obj => true, obj => CancelEditClass());
			_saveEditClassCommand = new RelayCommand(obj => HasUnsavedChanges, obj => SaveEditClass());
			_resetFiltersCommand = new RelayCommand(obj => true, obj => InitializeSearch());
			_addCommand = new RelayCommand(obj => true, obj => Add());
			_copyCommand = new RelayCommand(obj => _selectedClass != null, obj => Copy());
			_deleteCommand = new RelayCommand(obj => _selectedClass != null, obj => Delete());
            _importCommand = new RelayCommand(obj => true, obj => Import());
            _selectNextCommand = new RelayCommand(obj => true, obj => SelectNext());
            _selectPreviousCommand = new RelayCommand(obj => true, obj => SelectPrevious());

            Search();
		}

        #endregion

        #region Properties

        /// <summary>
        /// List of classes
        /// </summary>
        public ObservableCollection<ClassListItemViewModel> Classes
		{
			get { return _classes; }
		}

		/// <summary>
		/// Gets or sets the search text
		/// </summary>
		public string SearchText
		{
			get { return _classSearchInput.SearchText; }
			set
			{
				_classSearchInput.SearchText = value;
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
                return _classSearchInput.AppliedFilterCount > 0 ? $"Sort and Filter ({_classSearchInput.AppliedFilterCount})" : "Sort and Filter";
            }
        }

        /// <summary>
        /// Gets or sets sort and filters expanded
        /// </summary>
        public bool SortAndFilterExpanded
        {
            get { return _classSearchInput.SortAndFiltersExpanded; }
            set { _classSearchInput.SortAndFiltersExpanded = value; }
        }

        /// <summary>
        /// Gets sort options
        /// </summary>
        public List<KeyValuePair<ClassSortOption, string>> SortOptions
        {
            get { return _classSearchInput.SortOptions; }
        }

        /// <summary>
        /// Gets selected sort option
        /// </summary>
        public KeyValuePair<ClassSortOption, string> SelectedSortOption
        {
            get { return _classSearchInput.SortOption; }
            set
            {
                _classSearchInput.SortOption = value;
                Search();
            }
        }

        /// <summary>
        /// List of abilities
        /// </summary>
        public List<KeyValuePair<Ability, string>> Abilities
		{
			get { return _classSearchInput.Abilities; }
		}

		/// <summary>
		/// Selected ability
		/// </summary>
		public KeyValuePair<Ability, string> SelectedAbility
		{
			get { return _classSearchInput.Ability; }
			set
			{
				_classSearchInput.Ability = value;
				Search();
			}
		}

		/// <summary>
		/// List of armors
		/// </summary>
		public List<KeyValuePair<ArmorType, string>> Armors
		{
			get { return _classSearchInput.Armors; }
		}

		/// <summary>
		/// Selected armor
		/// </summary>
		public KeyValuePair<ArmorType, string> SelectedArmor
		{
			get { return _classSearchInput.Armor; }
			set
			{
				_classSearchInput.Armor = value;
				Search();
			}
		}

		/// <summary>
		/// List of weapons
		/// </summary>
		public List<KeyValuePair<WeaponType, string>> Weapons
		{
			get { return _classSearchInput.Weapons; }
		}

		/// <summary>
		/// Selected weapon
		/// </summary>
		public KeyValuePair<WeaponType, string> SelectedWeapon
		{
			get { return _classSearchInput.Weapon; }
			set
			{
				_classSearchInput.Weapon = value;
				Search();
			}
		}

		/// <summary>
		/// List of tools
		/// </summary>
		public List<KeyValuePair<Enum, string>> Tools
		{
			get { return _classSearchInput.Tools; }
		}

		/// <summary>
		/// Selected tool
		/// </summary>
		public KeyValuePair<Enum, string> SelectedTool
		{
			get { return _classSearchInput.Tool; }
			set
			{
				_classSearchInput.Tool = value;
				Search();
			}
		}

		/// <summary>
		/// List of skills
		/// </summary>
		public List<KeyValuePair<Skill, string>> Skills
		{
			get { return _classSearchInput.Skills; }
		}

		/// <summary>
		/// Selected skill
		/// </summary>
		public KeyValuePair<Skill, string> SelectedSkill
		{
			get { return _classSearchInput.Skill; }
			set
			{
				_classSearchInput.Skill = value;
				Search();
			}
		}

		/// <summary>
		/// Command to select a class
		/// </summary>
		public ICommand SelectClassCommand
		{
			get { return _selectClassCommand; }
		}

		/// <summary>
		/// Selected class
		/// </summary>
		public ClassViewModel SelectedClass
		{
			get { return _selectedClass; }
		}

		/// <summary>
		/// Editing class xml
		/// </summary>
		public string EditingClassXML
		{
			get { return _editClassXML; }
			set
			{
				if (Set(ref _editClassXML, value) && !_editHasUnsavedChanges)
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
		/// Command to edit class
		/// </summary>
		public ICommand EditClassCommand
		{
			get { return _editClassCommand; }
		}

        /// <summary>
		/// Command to export class
		/// </summary>
		public ICommand ExportClassCommand
        {
            get { return _exportClassCommand; }
        }

        /// <summary>
        /// Command to cancel edit class
        /// </summary>
        public ICommand CancelEditClassCommand
		{
			get { return _cancelEditClassCommand; }
		}

		/// <summary>
		/// Command to save edit class
		/// </summary>
		public ICommand SaveEditClassCommand
		{
			get { return _saveEditClassCommand; }
		}

		/// <summary>
		/// Command to add class
		/// </summary>
		public ICommand AddClassCommand
		{
			get { return _addCommand; }
		}

		/// <summary>
		/// Command to copy class
		/// </summary>
		public ICommand CopyClassCommand
		{
			get { return _copyCommand; }
		}

		/// <summary>
		/// Command to delete selected class
		/// </summary>
		public ICommand DeleteClassCommand
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
        /// True if currently editing a class
        /// </summary>
        public bool IsEditingClass
		{
			get { return _editClassXML != null; }
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
            _classes.Clear();
            foreach (ClassModel classModel in _classSearchService.Search(_classSearchInput))
            {
                _classes.Add(new ClassListItemViewModel(classModel, _stringService));
            }

            if (_selectedClass != null)
            {
                ClassListItemViewModel classItem = _classes.FirstOrDefault(x => x.ClassModel.ID == _selectedClass.ClassModel.ID);
                if (classItem != null)
                {
                    classItem.IsSelected = true;
                }
            }

            OnPropertyChanged(nameof(SortAndFilterHeader));
        }

        #endregion

        #region Private Methods

        private void InitializeSearch()
		{
            _classSearchInput.Reset();

			OnPropertyChanged(nameof(SearchText));
			OnPropertyChanged(nameof(SelectedSortOption));
            OnPropertyChanged(nameof(SelectedAbility));
			OnPropertyChanged(nameof(SelectedArmor));
			OnPropertyChanged(nameof(SelectedWeapon));
			OnPropertyChanged(nameof(SelectedTool));
			OnPropertyChanged(nameof(SelectedSkill));

			Search();
		}

		private void SelectClass(ClassListItemViewModel classItem)
		{
			bool selectClass = true;

			if (_editClassXML != null)
			{
				if (_editHasUnsavedChanges)
				{
					string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
														 _selectedClass.Name, Environment.NewLine + Environment.NewLine);
                    string accept = "Save and Continue";
                    string reject = "Discard Changes";
                    string cancel = "Cancel Navigation";
                    bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                    if (result == true)
                    {
                        if (!SaveEditClass())
                        {
                            selectClass = false;
                        }
                    }
                    else if (result == false)
                    {
                        CancelEditClass();
                    }
                    else
                    {
                        selectClass = false;
                    }
                }
				else
				{
					CancelEditClass();
				}
			}

			if (selectClass)
			{
				foreach (ClassListItemViewModel item in _classes)
				{
					item.IsSelected = false;
				}
				classItem.IsSelected = true;

				_selectedClass = new ClassViewModel(classItem.ClassModel);
				OnPropertyChanged(nameof(SelectedClass));
			}
		}

		private void EditClass(ClassViewModel classModel)
		{
			_editClassXML = classModel.XML;

			OnPropertyChanged(nameof(EditingClassXML));
			OnPropertyChanged(nameof(IsEditingClass));
		}

        private void CancelEditClass()
		{
			_editHasUnsavedChanges = false;
			_editClassXML = null;

			OnPropertyChanged(nameof(EditingClassXML));
			OnPropertyChanged(nameof(IsEditingClass));
			OnPropertyChanged(nameof(HasUnsavedChanges));
		}

		private bool SaveEditClass()
		{
			bool saved = false;

			try
			{
				ClassModel model = _xmlImporter.GetClass(_editClassXML);

				if (model != null)
				{
					model.ID = _selectedClass.ClassModel.ID;
                    _compendium.UpdateClass(model);
					_selectedClass = new ClassViewModel(model);

					ClassListItemViewModel oldListItem = _classes.FirstOrDefault(x => x.ClassModel.ID == model.ID);
					if (oldListItem != null)
					{
                        if (_classSearchService.SearchInputApplies(_classSearchInput, model))
                        {
                            oldListItem.UpdateModel(model);
                        }
                        else
                        {
                            _classes.Remove(oldListItem);
                        }
					}

					_editClassXML = null;
					_editHasUnsavedChanges = false;

					SortClasses();

                    _compendium.SaveClasses();

					OnPropertyChanged(nameof(SelectedClass));
					OnPropertyChanged(nameof(EditingClassXML));
					OnPropertyChanged(nameof(IsEditingClass));
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
			bool addClass = true;

			if (_editClassXML != null)
			{
				if (_editHasUnsavedChanges)
				{
					string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
														 _selectedClass.Name, Environment.NewLine + Environment.NewLine);
                    string accept = "Save and Continue";
                    string reject = "Discard Changes";
                    string cancel = "Cancel Navigation";
                    bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                    if (result == true)
                    {
                        if (!SaveEditClass())
                        {
                            addClass = false;
                        }
                    }
                    else if (result == false)
                    {
                        CancelEditClass();
                    }
                    else
                    {
                        addClass = false;
                    }
                }
				else
				{
					CancelEditClass();
				}
			}

			if (addClass)
			{
				string xml = "<name>New Class</name><hd>0</hd><proficiency></proficiency><spellAbility></spellAbility><autolevel level=\"1\"><feature optional=\"YES\"><name>Starting</name><text></text></feature></autolevel>";

				ClassModel classModel = _xmlImporter.GetClass(xml);

				_compendium.AddClass(classModel);

                if (_classSearchService.SearchInputApplies(_classSearchInput, classModel))
                {
                    ClassListItemViewModel listItem = new ClassListItemViewModel(classModel, _stringService);
                    _classes.Add(listItem);
                    foreach (ClassListItemViewModel item in _classes)
                    {
                        item.IsSelected = false;
                    }
                    listItem.IsSelected = true;
                }

				_selectedClass = new ClassViewModel(classModel);

				_editClassXML = classModel.XML;

				SortClasses();

                _compendium.SaveClasses();

                OnPropertyChanged(nameof(EditingClassXML));
				OnPropertyChanged(nameof(IsEditingClass));
				OnPropertyChanged(nameof(SelectedClass));
            }
		}

		private void Copy()
		{
			if (_selectedClass != null)
			{
				bool copyClass = true;

				if (_editClassXML != null)
				{
					if (_editHasUnsavedChanges)
					{
						string body = String.Format("{0} has unsaved changes.{1}What would you like to do?",
															 _selectedClass.Name, Environment.NewLine + Environment.NewLine);
                        string accept = "Save and Continue";
                        string reject = "Discard Changes";
                        string cancel = "Cancel Navigation";
                        bool? result = _dialogService.ShowConfirmationDialog("Unsaved Changes", body, accept, reject, cancel);

                        if (result == true)
                        {
                            if (!SaveEditClass())
                            {
                                copyClass = false;
                            }
                        }
                        else if (result == false)
                        {
                            CancelEditClass();
                        }
                        else
                        {
                            copyClass = false;
                        }
                    }
					else
					{
						CancelEditClass();
					}
				}

				if (copyClass)
				{
					ClassModel newClass = new ClassModel(_selectedClass.ClassModel);
					newClass.Name += " (copy)";
					newClass.ID = Guid.NewGuid();
					newClass.XML = newClass.XML.Replace("<name>" + _selectedClass.ClassModel.Name + "</name>",
																	"<name>" + newClass.Name + "</name>");

					_compendium.AddClass(newClass);

                    if (_classSearchService.SearchInputApplies(_classSearchInput, newClass))
                    {
                        ClassListItemViewModel listItem = new ClassListItemViewModel(newClass, _stringService);
                        _classes.Add(listItem);
                        foreach (ClassListItemViewModel item in _classes)
                        {
                            item.IsSelected = false;
                        }
                        listItem.IsSelected = true;
                    }

					_selectedClass = new ClassViewModel(newClass);

					SortClasses();

                    _compendium.SaveClasses();

                    OnPropertyChanged(nameof(SelectedClass));
                }
			}
		}

		private void Delete()
		{
			if (_selectedClass != null)
			{
                bool canDelete = true;

                foreach (CharacterModel character in _compendium.Characters)
                {
                    foreach(LevelModel level in character.Levels)
                    {
                        if (level.Class != null && level.Class.ID == _selectedClass.ClassModel.ID)
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
                                                                 _selectedClass.Name);

                    bool? result = _dialogService.ShowConfirmationDialog("Delete Class", message, "Yes", "No", null);

                    if (result == true)
                    {
                        _compendium.DeleteClass(_selectedClass.ClassModel.ID);

                        ClassListItemViewModel listItem = _classes.FirstOrDefault(x => x.ClassModel.ID == _selectedClass.ClassModel.ID);
                        if (listItem != null)
                        {
                            _classes.Remove(listItem);
                        }

                        _selectedClass = null;

                        _compendium.SaveClasses();

                        OnPropertyChanged(nameof(SelectedClass));

                        if (_editClassXML != null)
                        {
                            CancelEditClass();
                        }
                    }
                }
                else
                {
                    _dialogService.ShowConfirmationDialog("Unable Delete Class", "Class is in use by a character.", "OK", null, null);
                }
			}
		}

		private void SortClasses()
		{
			if (_classes != null && _classes.Count > 0)
			{
                List<ClassModel> classes = _classSearchService.Sort(_classes.Select(x => x.ClassModel), _classSearchInput.SortOption.Key);
                for (int i = 0; i < classes.Count; ++i)
                {
                    if (classes[i].ID != _classes[i].ClassModel.ID)
                    {
                        ClassListItemViewModel classListItem = _classes.FirstOrDefault(x => x.ClassModel.ID == classes[i].ID);
                        if (classListItem != null)
                        {
                            _classes.Move(_classes.IndexOf(classListItem), i);
                        }
                    }
                }
			}
        }

        private void ExportClass(ClassViewModel classViewModel)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "XML Document|*.xml";
            saveFileDialog.Title = "Save Class";
            saveFileDialog.FileName = classViewModel.Name;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string ext = System.IO.Path.GetExtension(saveFileDialog.FileName);

                    if (ext == ".xml")
                    {
                        string xml = _xmlExporter.FormatXMLWithHeader(_xmlExporter.GetXML(classViewModel.ClassModel));
                        System.IO.File.WriteAllText(saveFileDialog.FileName, xml, System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        _dialogService.ShowConfirmationDialog("Unable To Export", "Invalid file extension.", "OK", null, null);
                    }
                }
                catch (Exception)
                {
                    _dialogService.ShowConfirmationDialog("Unable To Export", "An error occurred when attempting to export the class.", "OK", null, null);
                }
            }
        }
        
        private void Import()
        {
            _dialogService.ShowImportView();
        }

        private void SelectNext()
        {
            if (_classes.Any())
            {
                ClassListItemViewModel selected = _classes.FirstOrDefault(x => x.IsSelected);

                foreach (ClassListItemViewModel c in _classes)
                {
                    c.IsSelected = false;
                }

                if (selected == null)
                {
                    _classes[0].IsSelected = true;
                    _selectedClass = new ClassViewModel(_classes[0].ClassModel);
                }
                else
                {
                    int index = Math.Min(_classes.IndexOf(selected) + 1, _classes.Count - 1);
                    _classes[index].IsSelected = true;
                    _selectedClass = new ClassViewModel(_classes[index].ClassModel);
                }

                OnPropertyChanged(nameof(SelectedClass));
            }
        }

        private void SelectPrevious()
        {
            if (_classes.Any())
            {
                ClassListItemViewModel selected = _classes.FirstOrDefault(x => x.IsSelected);

                foreach (ClassListItemViewModel c in _classes)
                {
                    c.IsSelected = false;
                }

                if (selected == null)
                {
                    _classes[_classes.Count - 1].IsSelected = true;
                    _selectedClass = new ClassViewModel(_classes[_classes.Count - 1].ClassModel);
                }
                else
                {
                    int index = Math.Max(_classes.IndexOf(selected) - 1, 0);
                    _classes[index].IsSelected = true;
                    _selectedClass = new ClassViewModel(_classes[index].ClassModel);
                }

                OnPropertyChanged(nameof(SelectedClass));
            }
        }

        #endregion
    }
}
