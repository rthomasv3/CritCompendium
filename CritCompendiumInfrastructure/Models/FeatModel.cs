using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class FeatModel : CompendiumEntryModel
   {
      #region Fields

      private string _prerequisite;
      private List<string> _textCollection = new List<string>();
      private List<ModifierModel> _modifiers = new List<ModifierModel>();
      private string _xml;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a default instance of <see cref="FeatModel"/>
      /// </summary>
      public FeatModel()
      {
      }

      /// <summary>
      /// Creates a copy of <see cref="FeatModel"/>
      /// </summary>
      public FeatModel(FeatModel featModel) : base(featModel)
      {
         _prerequisite = featModel.Prerequisite;
         _textCollection = new List<string>(featModel.TextCollection);

         _modifiers = new List<ModifierModel>();
         foreach (ModifierModel modifierModel in featModel.Modifiers)
         {
            _modifiers.Add(new ModifierModel(modifierModel));
         }

         _xml = featModel.XML;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets prerequisite
      /// </summary>
      public string Prerequisite
      {
         get { return _prerequisite; }
         set { _prerequisite = value; }
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
