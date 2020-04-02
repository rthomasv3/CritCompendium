using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class AdventureModel : CompendiumEntryModel
   {
      #region Fields

      private List<string> _tags = new List<string>();
      private string _introduction;
      private List<string> _goals = new List<string>();
      private List<HookModel> _hooks = new List<HookModel>();
      private List<EventModel> _events = new List<EventModel>();
      private List<Guid> _sideQuests = new List<Guid>();

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="AdventureModel"/>
      /// </summary>
      public AdventureModel()
      {
      }

      /// <summary>
      /// Creates a copy of <see cref="AdventureModel"/>
      /// </summary>
      public AdventureModel(AdventureModel adventureModel) : base(adventureModel)
      {
         _tags = new List<string>(adventureModel.Tags);
         _introduction = adventureModel.Introduction;
         _goals = new List<string>(adventureModel.Goals);

         _hooks = new List<HookModel>();
         foreach (HookModel hookModel in adventureModel.Hooks)
         {
            _hooks.Add(new HookModel(hookModel));
         }

         _events = new List<EventModel>();
         foreach (EventModel eventModel in adventureModel.Events)
         {
            _events.Add(new EventModel(eventModel));
         }

         _sideQuests = new List<Guid>(adventureModel.SideQuests);
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets introduction
      /// </summary>
      public string Introduction
      {
         get { return _introduction; }
         set { _introduction = value; }
      }

      /// <summary>
      /// Gets or sets goals
      /// </summary>
      public List<string> Goals
      {
         get { return _goals; }
         set { _goals = value; }
      }

      /// <summary>
      /// Gets or sets hooks
      /// </summary>
      public List<HookModel> Hooks
      {
         get { return _hooks; }
         set { _hooks = value; }
      }

      /// <summary>
      /// Gets or sets events
      /// </summary>
      public List<EventModel> Events
      {
         get { return _events; }
         set { _events = value; }
      }

      /// <summary>
      /// Gets or sets side quests
      /// </summary>
      public List<Guid> SideQuests
      {
         get { return _sideQuests; }
         set { _sideQuests = value; }
      }

      #endregion
   }
}
