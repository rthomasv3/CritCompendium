using System;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class MovementModel
   {
      #region Fields

      private Guid _id;
      private int _walkSpeed;
      private int _swimSpeed;
      private int _climbSpeed;
      private int _crawlSpeed;
      private int _flySpeed;
      private bool _applyEncumbrance;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="MovementModel"/>
      /// </summary>
      public MovementModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates an instance of <see cref="MovementModel"/>
      /// </summary>
      public MovementModel(MovementModel movementModel)
      {
         _id = movementModel.ID;
         _walkSpeed = movementModel.WalkSpeed;
         _swimSpeed = movementModel.SwimSpeed;
         _climbSpeed = movementModel.ClimbSpeed;
         _crawlSpeed = movementModel.CrawlSpeed;
         _flySpeed = movementModel.FlySpeed;
         _applyEncumbrance = movementModel.ApplyEncumbrance;
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
      /// Gets or sets walk speed
      /// </summary>
      public int WalkSpeed
      {
         get { return _walkSpeed; }
         set { _walkSpeed = value; }
      }

      /// <summary>
      /// Gets or sets swim speed
      /// </summary>
      public int SwimSpeed
      {
         get { return _swimSpeed; }
         set { _swimSpeed = value; }
      }

      /// <summary>
      /// Gets or sets climb speed
      /// </summary>
      public int ClimbSpeed
      {
         get { return _climbSpeed; }
         set { _climbSpeed = value; }
      }

      /// <summary>
      /// Gets or sets crawl speed
      /// </summary>
      public int CrawlSpeed
      {
         get { return _crawlSpeed; }
         set { _crawlSpeed = value; }
      }

      /// <summary>
      /// Gets or sets fly speed
      /// </summary>
      public int FlySpeed
      {
         get { return _flySpeed; }
         set { _flySpeed = value; }
      }

      /// <summary>
      /// Gets or sets apply encumbrance
      /// </summary>
      public bool ApplyEncumbrance
      {
         get { return _applyEncumbrance; }
         set { _applyEncumbrance = value; }
      }

      #endregion
   }
}
