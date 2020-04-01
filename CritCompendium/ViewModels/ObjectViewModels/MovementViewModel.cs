using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.ViewModels.ObjectViewModels
{
    public sealed class MovementViewModel : ObjectViewModel
    {
        #region Fields

        private readonly MovementModel _movementModel;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="MovementViewModel"/>
        /// </summary>
        public MovementViewModel(MovementModel movementModel)
        {
            _movementModel = movementModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets id
        /// </summary>
        public Guid ID
        {
            get { return _movementModel.ID; }
        }

        /// <summary>
        /// Gets movement model
        /// </summary>
        public MovementModel MovementModel
        {
            get { return _movementModel; }
        }

        /// <summary>
        /// Gets or sets walk speed
        /// </summary>
        public int WalkSpeed
        {
            get { return _movementModel.WalkSpeed; }
            set { _movementModel.WalkSpeed = value; }
        }

        /// <summary>
        /// Gets or sets swim speed
        /// </summary>
        public int SwimSpeed
        {
            get { return _movementModel.SwimSpeed; }
            set { _movementModel.SwimSpeed = value; }
        }

        /// <summary>
        /// Gets or sets climb speed
        /// </summary>
        public int ClimbSpeed
        {
            get { return _movementModel.ClimbSpeed; }
            set { _movementModel.ClimbSpeed = value; }
        }

        /// <summary>
        /// Gets or sets crawl speed
        /// </summary>
        public int CrawlSpeed
        {
            get { return _movementModel.CrawlSpeed; }
            set { _movementModel.CrawlSpeed = value; }
        }

        /// <summary>
        /// Gets or sets fly speed
        /// </summary>
        public int FlySpeed
        {
            get { return _movementModel.FlySpeed; }
            set { _movementModel.FlySpeed = value; }
        }

        /// <summary>
        /// Gets or sets apply encumbrance
        /// </summary>
        public bool ApplyEncumbrance
        {
            get { return _movementModel.ApplyEncumbrance; }
            set { _movementModel.ApplyEncumbrance = value; }
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion
    }
}
