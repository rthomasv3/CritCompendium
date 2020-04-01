using System;
using System.Collections.Generic;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class NPCModel
   {
      #region Fields

      private Guid _id;
      private string _name;
      private List<string> _tags = new List<string>();
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
      /// Creates an instance of <see cref="NPCModel"/>
      /// </summary>
      public NPCModel()
      {
         _id = Guid.NewGuid();
      }

      /// <summary>
      /// Creates a copy of <see cref="NPCModel"/>
      /// </summary>
      public NPCModel(NPCModel npcModel)
      {
         _id = npcModel.ID;
         _name = npcModel.Name;
         _tags = new List<string>(npcModel.Tags);
         _occupation = npcModel.Occupation;
         _backstory = npcModel.Backstory;
         _ideal = npcModel.Ideal;
         _bond = npcModel.Bond;
         _flaw = npcModel.Flaw;
         _appearance = npcModel.Appearance;
         _abilities = npcModel.Abilities;
         _mannerism = npcModel.Mannerism;
         _interactions = npcModel.Interactions;
         _usefulKnowledge = npcModel.UsefulKnowledge;
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
      /// Gets or sets tags
      /// </summary>
      public List<string> Tags
      {
         get { return _tags; }
         set { _tags = value; }
      }

      /// <summary>
      /// Gets or sets occupation
      /// </summary>
      public string Occupation
      {
         get { return _occupation; }
         set { _occupation = value; }
      }

      /// <summary>
      /// Gets or sets backstory
      /// </summary>
      public string Backstory
      {
         get { return _backstory; }
         set { _backstory = value; }
      }

      /// <summary>
      /// Gets or sets ideal
      /// </summary>
      public string Ideal
      {
         get { return _ideal; }
         set { _ideal = value; }
      }

      /// <summary>
      /// Gets or sets bond
      /// </summary>
      public string Bond
      {
         get { return _bond; }
         set { _bond = value; }
      }

      /// <summary>
      /// Gets or sets flaw
      /// </summary>
      public string Flaw
      {
         get { return _flaw; }
         set { _flaw = value; }
      }

      /// <summary>
      /// Gets or sets appearance
      /// </summary>
      public string Appearance
      {
         get { return _appearance; }
         set { _appearance = value; }
      }

      /// <summary>
      /// Gets or sets abilities
      /// </summary>
      public string Abilities
      {
         get { return _abilities; }
         set { _abilities = value; }
      }

      /// <summary>
      /// Gets or sets mannerism
      /// </summary>
      public string Mannerism
      {
         get { return _mannerism; }
         set { _mannerism = value; }
      }

      /// <summary>
      /// Gets or sets interactions
      /// </summary>
      public string Interactions
      {
         get { return _interactions; }
         set { _interactions = value; }
      }

      /// <summary>
      /// Gets or sets useful knowledge
      /// </summary>
      public string UsefulKnowledge
      {
         get { return _usefulKnowledge; }
         set { _usefulKnowledge = value; }
      }

      #endregion
   }
}
