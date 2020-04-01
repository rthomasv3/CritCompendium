using System;
using System.Collections.Generic;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Services;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class PackViewModel : ObjectViewModel
   {
      #region Fields

      private readonly Compendium _compendium = DependencyResolver.Resolve<Compendium>();

      private PackModel _packModel;
      private readonly List<KeyValuePair<PackModel, string>> _packOptions = new List<KeyValuePair<PackModel, string>>();
      private KeyValuePair<PackModel, string> _selectedPackOption;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="PackViewModel"/>
      /// </summary>
      public PackViewModel(PackModel packModel)
      {
         _packModel = packModel;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets id
      /// </summary>
      public Guid ID
      {
         get { return _packModel.ID; }
      }

      /// <summary>
      /// Gets name
      /// </summary>
      public string Name
      {
         get { return _packModel.Name; }
      }

      /// <summary>
      /// Gets items
      /// </summary>
      public List<string> Items
      {
         get { return _packModel.Items; }
      }

      /// <summary>
      /// Gets pack model
      /// </summary>
      public PackModel PackModel
      {
         get { return _packModel; }
      }

      /// <summary>
      /// Gets pack options
      /// </summary>
      public List<KeyValuePair<PackModel, string>> PackOptions
      {
         get { return _packOptions; }
      }

      /// <summary>
      /// Gets or sets selected pack option
      /// </summary>
      public KeyValuePair<PackModel, string> SelectedPackOption
      {
         get { return _selectedPackOption; }
         set
         {
            _selectedPackOption = value;
            _packModel = value.Key;
         }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Initializes pack options
      /// </summary>
      public void InitializePackOptions()
      {
         _packOptions.Clear();
         foreach (PackModel packModel in _compendium.Packs)
         {
            _packOptions.Add(new KeyValuePair<PackModel, string>(packModel, packModel.Name));
         }

         _selectedPackOption = _packOptions[0];
         _packModel = _selectedPackOption.Key;
      }

      #endregion
   }
}
