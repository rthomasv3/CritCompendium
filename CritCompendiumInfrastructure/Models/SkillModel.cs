using CritCompendiumInfrastructure.Enums;
using CritCompendiumInfrastructure.Services;

namespace CritCompendiumInfrastructure.Models
{
   public sealed class SkillModel
   {
      #region Fields

      private readonly StatService _statService = DependencyResolver.Resolve<StatService>();
      private readonly StringService _stringService = DependencyResolver.Resolve<StringService>();
      private readonly Skill _skill;
      private readonly string _skillString;
      private readonly string _skillAbilityString;
      private int _bonus;
      private bool _proficient;
      private bool _expertise;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of <see cref="SkillModel"/>
      /// </summary>
      public SkillModel(Skill skill, int bonus, bool proficient, bool expertise)
      {
         _skill = skill;
         _bonus = bonus;
         _proficient = proficient;
         _expertise = expertise;

         _skillString = _stringService.GetString(skill);

         _skillAbilityString = _skillString;
         if (skill != Skill.None)
         {
            Ability ability = _statService.GetSkillAbility(skill);
            if (ability != Ability.None)
            {
               string abrev = _stringService.GetAbbreviationString(ability).ToUpper();
               _skillAbilityString = $"{_skillString} ({abrev})";
            }
         }
      }

      /// <summary>
      /// Creates a copy of <see cref="SkillModel"/>
      /// </summary>
      public SkillModel(SkillModel skillModel)
      {
         _skill = skillModel.Skill;
         _skillString = skillModel.SkillString;
         _bonus = skillModel.Bonus;
         _proficient = skillModel.Proficient;
         _expertise = skillModel.Expertise;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets skill
      /// </summary>
      public Skill Skill
      {
         get { return _skill; }
      }

      /// <summary>
      /// Gets skill string
      /// </summary>
      public string SkillString
      {
         get { return _skillString; }
      }

      /// <summary>
      /// Gets skill string with ability
      /// </summary>
      public string SkillAbilityString
      {
         get { return _skillAbilityString; }
      }

      /// <summary>
      /// Gets or sets value
      /// </summary>
      public int Bonus
      {
         get { return _bonus; }
         set { _bonus = value; }
      }

      /// <summary>
      /// Gets bonus string
      /// </summary>
      public string BonusString
      {
         get { return _statService.AddPlusOrMinus(_bonus); }
      }

      /// <summary>
      /// Gets or set proficient
      /// </summary>
      public bool Proficient
      {
         get { return _proficient; }
         set { _proficient = value; }
      }

      /// <summary>
      /// Gets or set expertise
      /// </summary>
      public bool Expertise
      {
         get { return _expertise; }
         set { _expertise = value; }
      }

      #endregion
   }
}
