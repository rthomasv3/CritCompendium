using System;
using System.Linq;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class NPCEditViewModel : NotifyPropertyChanged
    {
        #region Fields
        
        private readonly NPCModel _npcModel;

        private string _name;
        private string _tags;
        private string _occupation;
        private string _backstory;
        private string _ideal;
        private string _bond;
        private string _flaw;
        private string _appearance;
        private string _abilities;
        private string _mannerism;
        private string _interactions;
        private string _usefulKnowledge;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="NPCEditViewModel"/>
        /// </summary>
        public NPCEditViewModel(NPCModel npcModel)
        {
            _npcModel = new NPCModel(npcModel);

            _name = npcModel.Name;

            if (_npcModel.Tags.Any())
            {
                _tags = String.Join(", ", _npcModel.Tags);
            }

            _occupation = _npcModel.Occupation;
            _backstory = _npcModel.Backstory;
            _ideal = _npcModel.Ideal;
            _bond = _npcModel.Bond;
            _flaw = _npcModel.Flaw;
            _appearance = _npcModel.Appearance;
            _abilities = _npcModel.Abilities;
            _mannerism = _npcModel.Mannerism;
            _interactions = _npcModel.Interactions;
            _usefulKnowledge = _npcModel.UsefulKnowledge;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets npc model
        /// </summary>
        public NPCModel NPCModel
        {
            get { return _npcModel; }
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
                    _npcModel.Name = _name;
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
                        _npcModel.Tags.Clear();
                        foreach (string tag in _tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            _npcModel.Tags.Add(tag.Trim());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets occupation
        /// </summary>
        public string Occupation
        {
            get { return _occupation; }
            set
            {
                if (Set(ref _occupation, value))
                {
                    _npcModel.Occupation = _occupation;
                }
            }
        }

        /// <summary>
        /// Gets or sets backstory
        /// </summary>
        public string Backstory
        {
            get { return _backstory; }
            set
            {
                if (Set(ref _backstory, value))
                {
                    _npcModel.Backstory = _backstory;
                }
            }
        }

        /// <summary>
        /// Gets or sets ideal
        /// </summary>
        public string Ideal
        {
            get { return _ideal; }
            set
            {
                if (Set(ref _ideal, value))
                {
                    _npcModel.Ideal = _ideal;
                }
            }
        }

        /// <summary>
        /// Gets or sets bond
        /// </summary>
        public string Bond
        {
            get { return _bond; }
            set
            {
                if (Set(ref _bond, value))
                {
                    _npcModel.Bond = _bond;
                }
            }
        }

        /// <summary>
        /// Gets or sets flaw
        /// </summary>
        public string Flaw
        {
            get { return _flaw; }
            set
            {
                if (Set(ref _flaw, value))
                {
                    _npcModel.Flaw = _flaw;
                }
            }
        }

        /// <summary>
        /// Gets or sets appearance
        /// </summary>
        public string Appearance
        {
            get { return _appearance; }
            set
            {
                if (Set(ref _appearance, value))
                {
                    _npcModel.Appearance = _appearance;
                }
            }
        }

        /// <summary>
        /// Gets or sets abilities
        /// </summary>
        public string Abilities
        {
            get { return _abilities; }
            set
            {
                if (Set(ref _abilities, value))
                {
                    _npcModel.Abilities = _abilities;
                }
            }
        }

        /// <summary>
        /// Gets or sets mannerism
        /// </summary>
        public string Mannerism
        {
            get { return _mannerism; }
            set
            {
                if (Set(ref _mannerism, value))
                {
                    _npcModel.Mannerism = _mannerism;
                }
            }
        }

        /// <summary>
        /// Gets or sets interactions
        /// </summary>
        public string Interactions
        {
            get { return _interactions; }
            set
            {
                if (Set(ref _interactions, value))
                {
                    _npcModel.Interactions = _interactions;
                }
            }
        }

        /// <summary>
        /// Gets or sets useful knowledge
        /// </summary>
        public string UsefulKnowledge
        {
            get { return _usefulKnowledge; }
            set
            {
                if (Set(ref _usefulKnowledge, value))
                {
                    _npcModel.UsefulKnowledge = _usefulKnowledge;
                }
            }
        }

        #endregion
    }
}
