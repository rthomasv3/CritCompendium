using System.Collections.Generic;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class ItemModel : CompendiumEntryModel
   {
      #region Fields

      private ItemType _type;
      private bool _magic;
      private bool _requiresAttunement;
      private string _value;
      private string _weight;
      private string _dmg1;
      private string _dmg2;
      private string _dmgType;
      private string _properties;
      private Rarity _rarity;
      private string _ac;
      private string _strengthRequirement;
      private bool _stealthDisadvantage;
      private string _range;
      private List<string> _textCollection = new List<string>();
      private List<ModifierModel> _modifiers = new List<ModifierModel>();
      private List<string> _rolls = new List<string>();
      private string _xml;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a default instance of <see cref="ItemModel"/>
      /// </summary>
      public ItemModel()
      {
      }

      /// <summary>
      /// Creates a copy of <see cref="ItemModel"/>
      /// </summary>
      public ItemModel(ItemModel itemModel) : base(itemModel)
      {
         _type = itemModel.Type;
         _magic = itemModel.Magic;
         _requiresAttunement = itemModel.RequiresAttunement;
         _value = itemModel.Value;
         _weight = itemModel.Weight;
         _dmg1 = itemModel.Dmg1;
         _dmg2 = itemModel.Dmg2;
         _dmgType = itemModel.DmgType;
         _properties = itemModel.Properties;
         _rarity = itemModel.Rarity;
         _ac = itemModel.AC;
         _strengthRequirement = itemModel.StrengthRequirement;
         _stealthDisadvantage = itemModel.StealthDisadvantage;
         _range = itemModel.Range;
         _textCollection = new List<string>(itemModel.TextCollection);
         _rolls = new List<string>(itemModel.Rolls);

         _modifiers = new List<ModifierModel>();
         foreach (ModifierModel modifierModel in itemModel.Modifiers)
         {
            _modifiers.Add(new ModifierModel(modifierModel));
         }

         _xml = itemModel.XML;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets type
      /// </summary>
      public ItemType Type
      {
         get { return _type; }
         set { _type = value; }
      }

      /// <summary>
      /// Gets or sets magic
      /// </summary>
      public bool Magic
      {
         get { return _magic; }
         set { _magic = value; }
      }

      /// <summary>
      /// Gets or sets requresAttunement
      /// </summary>
      public bool RequiresAttunement
      {
         get { return _requiresAttunement; }
         set { _requiresAttunement = value; }
      }

      /// <summary>
      /// Gets or sets value
      /// </summary>
      public string Value
      {
         get { return _value; }
         set { _value = value; }
      }

      /// <summary>
      /// Gets or sets weight
      /// </summary>
      public string Weight
      {
         get { return _weight; }
         set { _weight = value; }
      }

      /// <summary>
      /// Gets or sets dmg1
      /// </summary>
      public string Dmg1
      {
         get { return _dmg1; }
         set { _dmg1 = value; }
      }

      /// <summary>
      /// Gets or sets dmg2
      /// </summary>
      public string Dmg2
      {
         get { return _dmg2; }
         set { _dmg2 = value; }
      }

      /// <summary>
      /// Gets or sets dmgType
      /// </summary>
      public string DmgType
      {
         get { return _dmgType; }
         set { _dmgType = value; }
      }

      /// <summary>
      /// Gets or sets properties
      /// </summary>
      public string Properties
      {
         get { return _properties; }
         set { _properties = value; }
      }

      /// <summary>
      /// Gets or sets rarity
      /// </summary>
      public Rarity Rarity
      {
         get { return _rarity; }
         set { _rarity = value; }
      }

      /// <summary>
      /// Gets or sets ac
      /// </summary>
      public string AC
      {
         get { return _ac; }
         set { _ac = value; }
      }

      /// <summary>
      /// Gets or sets strengthRequirement
      /// </summary>
      public string StrengthRequirement
      {
         get { return _strengthRequirement; }
         set { _strengthRequirement = value; }
      }

      /// <summary>
      /// Gets or sets stealthDisadvantage
      /// </summary>
      public bool StealthDisadvantage
      {
         get { return _stealthDisadvantage; }
         set { _stealthDisadvantage = value; }
      }

      /// <summary>
      /// Gets or sets range
      /// </summary>
      public string Range
      {
         get { return _range; }
         set { _range = value; }
      }

      /// <summary>
      /// Gets or sets text
      /// </summary>
      public List<string> TextCollection
      {
         get { return _textCollection; }
         set { _textCollection = value; }
      }

      /// <summary>
      /// Gets or sets modifiers
      /// </summary>
      public List<ModifierModel> Modifiers
      {
         get { return _modifiers; }
         set { _modifiers = value; }
      }

      /// <summary>
      /// Gets or sets rolls
      /// </summary>
      public List<string> Rolls
      {
         get { return _rolls; }
         set { _rolls = value; }
      }

      /// <summary>
      /// Gets or sets xml
      /// </summary>
      public string XML
      {
         get { return _xml; }
         set { _xml = value; }
      }

      #endregion
   }
}
