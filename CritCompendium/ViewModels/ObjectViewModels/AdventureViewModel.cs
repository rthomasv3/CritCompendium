using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class AdventureViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly AdventureModel _adventureModel;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of <see cref="AdventureViewModel"/>
      /// </summary>
      public AdventureViewModel(AdventureModel adventureModel)
      {
         _adventureModel = adventureModel;
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
      /// Gets name
      /// </summary>
      public string Name
      {
         get { return _adventureModel.Name; }
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
