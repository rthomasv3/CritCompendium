using System;
using System.Xml.Linq;
using CriticalCompendiumInfrastructure.Models;

namespace CriticalCompendiumInfrastructure.Persistence
{
    public sealed class XMLExporter
    {
        #region Fields

        private readonly int _version = 1;
        private readonly string _header = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        private readonly string _compendiumStart = "<compendium version=\"{0}\" date=\"{1}\">";
        private readonly string _compendiumEnd = "</compendium>";

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="XMLExporter"/>
        /// </summary>
        public XMLExporter()
        {
            
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds xml header and wraps with compendium tags
        /// </summary>
        public string WrapAndFormatXMLWithHeader(string xml)
        {
            return FormatXMLWithHeader(xml.Insert(0, String.Format(_compendiumStart, _version, DateTime.Now)) + _compendiumEnd);
        }

        /// <summary>
        /// Adds xml header and wraps with compendium tags
        /// </summary>
        public string FormatXMLWithHeader(string xml)
        {
            return FormatXMLString(xml).Insert(0, _header + Environment.NewLine);
        }

        /// <summary>
        /// Gets formatted xml of object
        /// </summary>
        public string GetXML(BackgroundModel model)
        {
            string xml = String.Empty;

            if (model != null)
            {
                xml = $"<background><id>{model.ID}</id>{model.XML}</background>";
            }

            return xml;
        }

        /// <summary>
        /// Gets formatted xml of object
        /// </summary>
        public string GetXML(ClassModel model)
        {
            string xml = String.Empty;

            if (model != null)
            {
                xml = $"<class><id>{model.ID}</id>{model.XML}</class>";
            }

            return xml;
        }

        /// <summary>
        /// Gets formatted xml of object
        /// </summary>
        public string GetXML(ConditionModel model)
        {
            string xml = String.Empty;

            if (model != null)
            {
                xml = $"<condition><id>{model.ID}</id>{model.XML}</condition>";
            }

            return xml;
        }

        /// <summary>
        /// Gets formatted xml of object
        /// </summary>
        public string GetXML(FeatModel model)
        {
            string xml = String.Empty;

            if (model != null)
            {
                xml = $"<feat><id>{model.ID}</id>{model.XML}</feat>";
            }

            return xml;
        }

        /// <summary>
        /// Gets formatted xml of object
        /// </summary>
        public string GetXML(ItemModel model)
        {
            string xml = String.Empty;

            if (model != null)
            {
                xml = $"<item><id>{model.ID}</id>{model.XML}</item>";
            }

            return xml;
        }

        /// <summary>
        /// Gets formatted xml of object
        /// </summary>
        public string GetXML(MonsterModel model)
        {
            string xml = String.Empty;

            if (model != null)
            {
                xml = $"<monster><id>{model.ID}</id>{model.XML}</monster>";
            }

            return xml;
        }

        /// <summary>
        /// Gets formatted xml of object
        /// </summary>
        public string GetXML(RaceModel model)
        {
            string xml = String.Empty;

            if (model != null)
            {
                xml = $"<race><id>{model.ID}</id>{model.XML}</race>";
            }

            return xml;
        }

        /// <summary>
        /// Gets formatted xml of object
        /// </summary>
        public string GetXML(SpellModel model)
        {
            string xml = String.Empty;

            if (model != null)
            {
                xml = $"<spell><id>{model.ID}</id>{model.XML}</spell>";
            }

            return xml;
        }

        /// <summary>
        /// Formats the xml string
        /// </summary>
        public string FormatXMLString(string xml)
        {
            return XElement.Parse(xml).ToString();
        }

        #endregion
    }
}
