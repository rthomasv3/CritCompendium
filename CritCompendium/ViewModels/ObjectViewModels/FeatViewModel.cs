using System;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class FeatViewModel
   {
      #region Fields

      private readonly FeatModel _featModel;
      private readonly string _prerequisite;
      private readonly string _description;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of <see cref="FeatViewModel"/>
      /// </summary>
      public FeatViewModel(FeatModel featModel)
      {
         _featModel = featModel;
         _prerequisite = String.IsNullOrWhiteSpace(_featModel.Prerequisite) ? "None" : _featModel.Prerequisite;
         _description = String.Join(Environment.NewLine, _featModel.TextCollection);
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets feat model
      /// </summary>
      public FeatModel FeatModel
      {
         get { return _featModel; }
      }

      /// <summary>
      /// Gets name
      /// </summary>
      public string Name
      {
         get { return _featModel.Name; }
      }

      /// <summary>
      /// Gets prerequisite
      /// </summary>
      public string Prerequisite
      {
         get { return _prerequisite; }
      }

      /// <summary>
      /// Gets description
      /// </summary>
      public string Description
      {
         get { return _description; }
      }

      /// <summary>
      /// Gets xml
      /// </summary>
      public string XML
      {
         get { return _featModel.XML; }
      }

      #endregion
   }
}
