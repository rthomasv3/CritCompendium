using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class AdventureEditViewModel : NotifyPropertyChanged
    {
        #region Fields

        private readonly AdventureModel _adventureModel;

        private string _name;
        private string _tags;
        private string _introduction;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="AdventureEditViewModel"/>
        /// </summary>
        public AdventureEditViewModel(AdventureModel adventureModel)
        {
            _adventureModel = new AdventureModel(adventureModel);

            _name = _adventureModel.Name;
            if (_adventureModel.Tags.Any())
            {
                _tags = String.Join(", ", _adventureModel.Tags);
            }
            _introduction = _adventureModel.Introduction;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets adventure model
        /// </summary>
        public AdventureModel AdventureModel
        {
            get { return _adventureModel; }
        }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (Set(ref _name, value))
                {
                    _adventureModel.Name = _name;
                }
            }
        }

        /// <summary>
        /// Gets or sets tags
        /// </summary>
        public string Tags
        {
            get { return _tags; }
            set
            {
                if (Set(ref _tags, value))
                {
                    if (!String.IsNullOrWhiteSpace(_tags))
                    {
                        _adventureModel.Tags.Clear();
                        foreach (string tag in _tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            _adventureModel.Tags.Add(tag.Trim());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets introduction
        /// </summary>
        public string Introduction
        {
            get { return _introduction; }
            set
            {
                if (Set(ref _introduction, value))
                {
                    _adventureModel.Introduction = _introduction;
                }
            }
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion
    }
}
