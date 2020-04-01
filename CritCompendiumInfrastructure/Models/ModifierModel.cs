using System;
using CritCompendiumInfrastructure.Enums;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class ModifierModel
   {
      #region Fields

      private Guid _id;
      private string _text;
      private ModifierCategory _modifierCategory;
      private int _value;
      private Ability _ability;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a default instance of <see cref="ModifierModel"/>
      /// </summary>
      public ModifierModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates a copy of <see cref="ModifierModel"/>
      /// </summary>
      public ModifierModel(ModifierModel modifierModel)
      {
         _id = modifierModel.ID;
         _text = modifierModel.Text;
         _modifierCategory = modifierModel.ModifierCategory;
         _value = modifierModel.Value;
         _ability = modifierModel.Ability;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets id
      /// </summary>
      public Guid ID
      {
         get { return _id; }
         set { _id = value; }
      }

      /// <summary>
      /// Gets or sets text
      /// </summary>
      public string Text
      {
         get { return _text; }
         set { _text = value; }
      }

      /// <summary>
      /// Gets or sets modifier category
      /// </summary>
      public ModifierCategory ModifierCategory
      {
         get { return _modifierCategory; }
         set { _modifierCategory = value; }
      }

      /// <summary>
      /// Gets or sets value
      /// </summary>
      public int Value
      {
         get { return _value; }
         set { _value = value; }
      }

      /// <summary>
      /// Gets or sets ability
      /// </summary>
      public Ability Ability
      {
         get { return _ability; }
         set { _ability = value; }
      }

      #endregion
   }
}
