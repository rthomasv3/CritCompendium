using System;

namespace CritCompendium.ViewModels.ListItemViewModels
{
    public sealed class ListItemViewModel<T> : NotifyPropertyChanged
    {
        #region Fields
        
        private T _model;
        private string _name;
        private string _description;
        private bool _isSelected = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="ListItemViewModel"/>
        /// </summary>
        public ListItemViewModel(T model)
        {
            _model = model;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Model
        /// </summary>
        public T Model
        {
            get { return _model; }
            set { Set(ref _model, value); }
        }

        /// <summary>
        /// Background name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        /// <summary>
        /// Background details
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
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
    }
}
