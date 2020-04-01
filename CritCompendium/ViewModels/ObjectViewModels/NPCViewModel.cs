using System;
using System.Linq;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public sealed class NPCViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();

      private readonly NPCModel _npcModel;

      private string _name;
      private string _tags;
      private string _occupation;
      private string _backstory;
      private string _ideal;
      private string _bond;
      private string _flaw;
      private string _appearance;
      private string _abilities;
      private string _mannerism;
      private string _interactions;
      private string _usefulKnowledge;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of <see cref="NPCViewModel"/>
      /// </summary>
      public NPCViewModel(NPCModel npcModel)
      {
         _npcModel = npcModel;

         if (!String.IsNullOrWhiteSpace(npcModel.Name))
         {
            _name = npcModel.Name;
         }
         else
         {
            _name = "Unknown Name";
         }

         if (_npcModel.Tags.Any())
         {
            _tags = String.Join(", ", _npcModel.Tags);
         }
         else
         {
            _tags = "None";
         }

         _occupation = _stringService.UnknownIfNullOrEmpty(_npcModel.Occupation);
         _backstory = _stringService.UnknownIfNullOrEmpty(_npcModel.Backstory);
         _ideal = _stringService.UnknownIfNullOrEmpty(_npcModel.Ideal);
         _bond = _stringService.UnknownIfNullOrEmpty(_npcModel.Bond);
         _flaw = _stringService.UnknownIfNullOrEmpty(_npcModel.Flaw);
         _appearance = _stringService.UnknownIfNullOrEmpty(_npcModel.Appearance);
         _abilities = _stringService.UnknownIfNullOrEmpty(_npcModel.Abilities);
         _mannerism = _stringService.UnknownIfNullOrEmpty(_npcModel.Mannerism);
         _interactions = _stringService.UnknownIfNullOrEmpty(_npcModel.Interactions);
         _usefulKnowledge = _stringService.UnknownIfNullOrEmpty(_npcModel.UsefulKnowledge);
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets npc model
      /// </summary>
      public NPCModel NPCModel
      {
         get { return _npcModel; }
      }

      /// <summary>
      /// Gets name
      /// </summary>
      public string Name
      {
         get { return _name; }
      }

      /// <summary>
      /// Gets tags
      /// </summary>
      public string Tags
      {
         get { return _tags; }
      }

      /// <summary>
      /// Gets occupation
      /// </summary>
      public string Occupation
      {
         get { return _occupation; }
      }

      /// <summary>
      /// Gets backstory
      /// </summary>
      public string Backstory
      {
         get { return _backstory; }
      }

      /// <summary>
      /// Gets ideal
      /// </summary>
      public string Ideal
      {
         get { return _ideal; }
      }

      /// <summary>
      /// Gets bond
      /// </summary>
      public string Bond
      {
         get { return _bond; }
      }

      /// <summary>
      /// Gets flaw
      /// </summary>
      public string Flaw
      {
         get { return _flaw; }
      }

      /// <summary>
      /// Gets appearance
      /// </summary>
      public string Appearance
      {
         get { return _appearance; }
      }

      /// <summary>
      /// Gets abilities
      /// </summary>
      public string Abilities
      {
         get { return _abilities; }
      }

      /// <summary>
      /// Gets mannerism
      /// </summary>
      public string Mannerism
      {
         get { return _mannerism; }
      }

      /// <summary>
      /// Gets interactions
      /// </summary>
      public string Interactions
      {
         get { return _interactions; }
      }

      /// <summary>
      /// Gets useful knowledge
      /// </summary>
      public string UsefulKnowledge
      {
         get { return _usefulKnowledge; }
      }

      #endregion
   }
}
