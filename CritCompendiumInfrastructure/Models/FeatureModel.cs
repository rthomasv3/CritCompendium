using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class FeatureModel
   {
      #region Fields

      private Guid _id;
      private string _name;
      private bool _optional;
      private List<string> _textCollection = new List<string>();

      #endregion

      #region Constructors

      /// <summary>
      /// Creates a default instance of <see cref="FeatureModel"/>
      /// </summary>
      public FeatureModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates a copy of <see cref="FeatureModel"/>
      /// </summary>
      public FeatureModel(FeatureModel featureModel)
      {
         _id = featureModel.ID;
         _name = featureModel.Name;
         _optional = featureModel.Optional;
         _textCollection = new List<string>(featureModel.TextCollection);
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
      /// Gets or sets name
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// Gets or sets optional
      /// </summary>
      public bool Optional
      {
         get { return _optional; }
         set { _optional = value; }
      }

      /// <summary>
      /// Gets or sets textCollection
      /// </summary>
      public List<string> TextCollection
      {
         get { return _textCollection; }
         set { _textCollection = value; }
      }

      /// <summary>
      /// Gets text as single string
      /// </summary>
      public string Text
      {
         get { return String.Join(Environment.NewLine, _textCollection).Trim(); }
      }

      #endregion
   }
}
