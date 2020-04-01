using System;
using System.Collections.Generic;
using System.Linq;

namespace CritCompendiumInfrastructure.Services
{
   public class CompendiumChangeEventArgs : EventArgs
   {
      #region Fields

      private readonly List<Guid> _ids;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="CompendiumChangeEventArgs"/>
      /// </summary>
      public CompendiumChangeEventArgs(IEnumerable<Guid> ids)
      {
         _ids = ids.ToList();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets the ids of the changed objects
      /// </summary>
      public List<Guid> IDs
      {
         get { return _ids; }
      }

      #endregion
   }
}
