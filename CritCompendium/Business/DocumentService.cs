using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendiumInfrastructure.Models;
using CritCompendiumInfrastructure.Business;
using Spire.Pdf;
using Spire.Pdf.Actions;
using Spire.Pdf.Fields;
using Spire.Pdf.Graphics;
using Spire.Pdf.Widget;
using Xceed.Words.NET;

namespace CritCompendium.Business
{
   public sealed class DocumentService
   {
      #region Fields

      private readonly StringService _stringService;
      private readonly StatService _statService;
      private readonly Formatting _nameFormat;
      private readonly Formatting _headerFormat;
      private readonly Formatting _labelFormat;
      private readonly Formatting _normalFormat;

      private Dictionary<string, string> _characterTextBoxMap = new Dictionary<string, string>();
      private Dictionary<string, bool> _characterCheckBoxMap = new Dictionary<string, bool>();

      private readonly float _letterWidth = 613.0f;
      private readonly float _letterHeight = 792.0f;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="DocumentService"/>
      /// </summary>
      public DocumentService(StringService stringService, StatService statService)
      {
         _stringService = stringService;
         _statService = statService;

         _nameFormat = new Formatting();
         _nameFormat.FontFamily = new Font("Bookman Old Style");
         _nameFormat.Bold = true;
         _nameFormat.Size = 14;
         _nameFormat.FontColor = System.Drawing.Color.FromArgb(64, 64, 64);

         _headerFormat = new Formatting();
         _headerFormat.FontFamily = new Font("Bookman Old Style");
         _headerFormat.Bold = true;
         _headerFormat.Size = 11;
         _headerFormat.FontColor = System.Drawing.Color.FromArgb(64, 64, 64);

         _labelFormat = new Formatting();
         _labelFormat.FontFamily = new Font("Bookman Old Style");
         _labelFormat.Size = 11;
         _labelFormat.FontColor = System.Drawing.Color.FromArgb(0, 0, 0);

         _normalFormat = new Formatting();
         _normalFormat.FontFamily = new Font("Bookman Old Style");
         _normalFormat.Size = 11;
         _normalFormat.FontColor = System.Drawing.Color.FromArgb(64, 64, 64);
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Creates a character sheet pdf in the specified location
      /// </summary>
      public void CreateCharacterPDF(string fileLocation, CharacterModel character)
      {
         using (Stream stream = GetType().Assembly.GetManifestResourceStream("CritCompendium.Resources.Documents.CC_Character_Sheet_5e.pdf"))
         {
            BuildCharacterMaps(character);

            PdfDocument pdf = CopyDocument(stream);
            PdfFormWidget pdfForm = pdf.Form as PdfFormWidget;

            if (character.Spellbooks.Count > 1)
            {
               PdfPageBase spellsPage = pdf.Pages[2];
               PdfTemplate template = spellsPage.CreateTemplate();

               List<PdfTextBoxField> textFields = new List<PdfTextBoxField>();
               List<PdfCheckBoxField> checkFields = new List<PdfCheckBoxField>();
               for (int i = 0; i < pdfForm.FieldsWidget.List.Count; ++i)
               {
                  PdfField field = pdfForm.FieldsWidget.List[i] as PdfField;

                  if (field.Page == spellsPage)
                  {
                     if (field is PdfTextBoxField textField)
                     {
                        textFields.Add(textField);
                     }
                     if (field is PdfCheckBoxField checkBoxField)
                     {
                        checkFields.Add(checkBoxField);
                     }
                  }
               }

               for (int i = 1; i < character.Spellbooks.Count; ++i)
               {
                  PdfPageBase spellPageCopy = pdf.Pages.Add(spellsPage.Size, new PdfMargins(0));
                  spellPageCopy.Canvas.DrawTemplate(template, new System.Drawing.PointF(0, 0));

                  foreach (PdfTextBoxField textField in textFields)
                  {
                     PdfTextBoxField newTextField = new PdfTextBoxField(spellPageCopy, textField.Name + $" ({i})");
                     newTextField.Bounds = textField.Bounds;
                     newTextField.BackColor = PdfRGBColor.Empty;
                     newTextField.BorderWidth = 0.0f;
                     newTextField.Multiline = textField.Multiline;
                     newTextField.TextAlignment = textField.TextAlignment;
                     pdfForm.FieldsWidget.Add(newTextField);
                  }

                  foreach (PdfCheckBoxField checkField in checkFields)
                  {
                     PdfCheckBoxField newCheckField = new PdfCheckBoxField(spellPageCopy, checkField.Name + $" ({i})");
                     newCheckField.Bounds = checkField.Bounds;
                     newCheckField.BackColor = PdfRGBColor.Empty;
                     newCheckField.BorderWidth = 0.0f;
                     newCheckField.Style = PdfCheckBoxStyle.Circle;
                     pdfForm.FieldsWidget.Add(newCheckField);
                  }
               }
            }

            for (int i = 0; i < pdfForm.FieldsWidget.List.Count; ++i)
            {
               PdfField field = pdfForm.FieldsWidget.List[i] as PdfField;

               if (field is PdfTextBoxField textField)
               {
                  if (_characterTextBoxMap.ContainsKey(textField.Name))
                  {
                     textField.Text = _characterTextBoxMap[textField.Name];
                  }
               }
               else if (field is PdfCheckBoxField checkField)
               {
                  if (_characterCheckBoxMap.ContainsKey(checkField.Name))
                  {
                     checkField.Checked = _characterCheckBoxMap[checkField.Name];
                  }
               }
            }

            pdf.DocumentInformation.Author = "Crit Compendium";
            pdf.DocumentInformation.CreationDate = DateTime.Now;
            pdf.DocumentInformation.Creator = "Crit Compendium";
            pdf.DocumentInformation.Title = "Crit Compendium - Character Sheet";
            pdf.DocumentInformation.Producer = String.Empty;

            pdf.SaveToFile(fileLocation);
         }
      }

      /// <summary>
      /// Creates a word document for the item
      /// </summary>
      public void CreateWordDoc(string fileLocaiton, ItemModel item)
      {
         DocX doc = DocX.Create(fileLocaiton);

         doc.PageHeight = _letterHeight;
         doc.PageWidth = _letterWidth;

         doc.InsertParagraph(item.Name, false, _nameFormat);
         doc.InsertParagraph(_stringService.CapitalizeWords(String.Format("{0}, {1}", _stringService.GetString(item.Type), item.Magic ? "Magic" : "Non-Magic")), false, _normalFormat);
         doc.InsertParagraph("", false, _normalFormat);

         Paragraph value = doc.InsertParagraph("Value: ", false, _labelFormat);
         value.Append(UnknownIfNullOrEmpty(item.Value), _normalFormat);

         Paragraph weight = doc.InsertParagraph("Weight: ", false, _labelFormat);
         weight.Append(UnknownIfNullOrEmpty(item.Weight), _normalFormat);

         if (!String.IsNullOrWhiteSpace(item.AC))
         {
            Paragraph ac = doc.InsertParagraph("AC: ", false, _labelFormat);
            ac.Append(item.AC, _normalFormat);
         }

         if (item.Type == CritCompendiumInfrastructure.Enums.ItemType.Heavy_Armor ||
             item.Type == CritCompendiumInfrastructure.Enums.ItemType.Light_Armor ||
             item.Type == CritCompendiumInfrastructure.Enums.ItemType.Medium_Armor)
         {
            Paragraph stealth = doc.InsertParagraph("Stealth Disadvantage: ", false, _labelFormat);
            stealth.Append(item.StealthDisadvantage ? "Yes" : "No", _normalFormat);

            Paragraph str = doc.InsertParagraph("Strength Requirement: ", false, _labelFormat);
            str.Append(NoneIfNullOrEmpty(item.StrengthRequirement), _normalFormat);
         }

         if (!String.IsNullOrWhiteSpace(item.Dmg1) &&
             !String.IsNullOrWhiteSpace(item.Dmg2))
         {
            Paragraph dmg = doc.InsertParagraph("Damage: ", false, _labelFormat);
            dmg.Append(item.Dmg1 + " / " + item.Dmg2, _normalFormat);
         }
         else if (!String.IsNullOrWhiteSpace(item.Dmg1))
         {
            Paragraph dmg = doc.InsertParagraph("Damage: ", false, _labelFormat);
            dmg.Append(item.Dmg1, _normalFormat);
         }
         else if (!String.IsNullOrWhiteSpace(item.Dmg2))
         {
            Paragraph dmg = doc.InsertParagraph("Damage: ", false, _labelFormat);
            dmg.Append(item.Dmg2, _normalFormat);
         }

         if (!String.IsNullOrWhiteSpace(item.DmgType))
         {
            Paragraph dmgType = doc.InsertParagraph("Damage Type: ", false, _labelFormat);
            dmgType.Append(item.DmgType, _normalFormat);
         }

         if (!String.IsNullOrWhiteSpace(item.Range))
         {
            Paragraph range = doc.InsertParagraph("Range: ", false, _labelFormat);
            range.Append(item.Range, _normalFormat);
         }

         if (!String.IsNullOrWhiteSpace(item.Properties))
         {
            Paragraph props = doc.InsertParagraph("Properties: ", false, _labelFormat);
            props.Append(item.Properties, _normalFormat);
         }


         Paragraph rarity = doc.InsertParagraph("Rarity: ", false, _labelFormat);
         rarity.Append(UnknownIfNullOrEmpty(_stringService.GetString(item.Rarity)), _normalFormat);

         Paragraph attune = doc.InsertParagraph("Requires Attunement: ", false, _labelFormat);
         attune.Append(item.RequiresAttunement ? "Yes" : "No", _normalFormat);

         doc.InsertParagraph("", false, _normalFormat);

         string description = String.Join(Environment.NewLine + Environment.NewLine, item.TextCollection.Where(x => !String.IsNullOrWhiteSpace(x)));
         if (!String.IsNullOrWhiteSpace(description))
         {
            doc.InsertParagraph("Description", false, _labelFormat);
            doc.InsertParagraph(description, false, _normalFormat);
         }

         doc.Save();
      }

      /// <summary>
      /// Creates a word document for the monster
      /// </summary>
      public void CreateWordDoc(string fileLocaiton, MonsterModel monster)
      {
         DocX doc = DocX.Create(fileLocaiton);

         doc.PageHeight = _letterHeight;
         doc.PageWidth = _letterWidth;

         doc.InsertParagraph(monster.Name, false, _nameFormat);
         doc.InsertParagraph(_stringService.CapitalizeWords(String.Format("{0} {1}, {2}", monster.Size, monster.Type, monster.Alignment)), false, _normalFormat);
         doc.InsertParagraph("", false, _normalFormat);

         Paragraph ac = doc.InsertParagraph("Armor Class: ", false, _labelFormat);
         ac.Append(monster.AC, _normalFormat);
         Paragraph hp = doc.InsertParagraph("Hit Points: ", false, _labelFormat);
         hp.Append(monster.HP, _normalFormat);
         Paragraph speed = doc.InsertParagraph("Speed: ", false, _labelFormat);
         speed.Append(monster.Speed, _normalFormat);
         doc.InsertParagraph("", false, _normalFormat);

         Table abilityTable = doc.InsertTable(2, 6);
         abilityTable.Rows[0].Cells[0].Paragraphs[0].InsertText("STR", false, _labelFormat);
         abilityTable.Rows[0].Cells[1].Paragraphs[0].InsertText("DEX", false, _labelFormat);
         abilityTable.Rows[0].Cells[2].Paragraphs[0].InsertText("CON", false, _labelFormat);
         abilityTable.Rows[0].Cells[3].Paragraphs[0].InsertText("INT", false, _labelFormat);
         abilityTable.Rows[0].Cells[4].Paragraphs[0].InsertText("WIS", false, _labelFormat);
         abilityTable.Rows[0].Cells[5].Paragraphs[0].InsertText("CHA", false, _labelFormat);
         abilityTable.Rows[1].Cells[0].Paragraphs[0].InsertText(String.Format("{0} ({1})", monster.Strength, _statService.GetStatBonusString(monster.Strength)), false, _normalFormat);
         abilityTable.Rows[1].Cells[1].Paragraphs[0].InsertText(String.Format("{0} ({1})", monster.Dexterity, _statService.GetStatBonusString(monster.Dexterity)), false, _normalFormat);
         abilityTable.Rows[1].Cells[2].Paragraphs[0].InsertText(String.Format("{0} ({1})", monster.Constitution, _statService.GetStatBonusString(monster.Constitution)), false, _normalFormat);
         abilityTable.Rows[1].Cells[3].Paragraphs[0].InsertText(String.Format("{0} ({1})", monster.Intelligence, _statService.GetStatBonusString(monster.Intelligence)), false, _normalFormat);
         abilityTable.Rows[1].Cells[4].Paragraphs[0].InsertText(String.Format("{0} ({1})", monster.Wisdom, _statService.GetStatBonusString(monster.Wisdom)), false, _normalFormat);
         abilityTable.Rows[1].Cells[5].Paragraphs[0].InsertText(String.Format("{0} ({1})", monster.Charisma, _statService.GetStatBonusString(monster.Charisma)), false, _normalFormat);
         doc.InsertParagraph("", false, _normalFormat);

         if (monster.Saves.Any())
         {
            Paragraph saves = doc.InsertParagraph("Saving Throws: ", false, _labelFormat);
            string savesString = String.Join(", ", monster.Saves.Select(x => String.Format("{0} {1}", _stringService.GetString(x.Key), _statService.AddPlusOrMinus(x.Value))));
            saves.Append(savesString, _normalFormat);
         }

         if (monster.Skills.Any())
         {
            Paragraph skills = doc.InsertParagraph("Skills: ", false, _labelFormat);
            string skillsString = String.Join(", ", monster.Skills.Select(x => String.Format("{0} {1}", _stringService.GetString(x.Key), _statService.AddPlusOrMinus(x.Value))));
            skills.Append(skillsString, _normalFormat);
         }

         if (!String.IsNullOrWhiteSpace(monster.Vulnerabilities))
         {
            Paragraph vulnerabilities = doc.InsertParagraph("Vulnerabilities: ", false, _labelFormat);
            vulnerabilities.Append(monster.Vulnerabilities, _normalFormat);
         }

         if (!String.IsNullOrWhiteSpace(monster.Resistances))
         {
            Paragraph resistances = doc.InsertParagraph("Resistances: ", false, _labelFormat);
            resistances.Append(monster.Resistances, _normalFormat);
         }

         if (!String.IsNullOrWhiteSpace(monster.Immunities))
         {
            Paragraph immunities = doc.InsertParagraph("Immunities: ", false, _labelFormat);
            immunities.Append(monster.Immunities, _normalFormat);
         }

         if (!String.IsNullOrWhiteSpace(monster.ConditionImmunities))
         {
            Paragraph conditionImmunities = doc.InsertParagraph("Condition Immunities: ", false, _labelFormat);
            conditionImmunities.Append(monster.ConditionImmunities, _normalFormat);
         }

         if (!String.IsNullOrWhiteSpace(monster.Senses))
         {
            Paragraph senses = doc.InsertParagraph("Senses: ", false, _labelFormat);
            senses.Append(monster.Senses, _normalFormat);
         }

         if (monster.Languages.Any())
         {
            Paragraph languages = doc.InsertParagraph("Languages: ", false, _labelFormat);
            string languagesString = String.Join(", ", monster.Languages);
            languages.Append(languagesString, _normalFormat);
         }

         if (!String.IsNullOrWhiteSpace(monster.CR))
         {
            Paragraph cr = doc.InsertParagraph("CR: ", false, _labelFormat);
            string crString = String.Format("{0} ({1} XP)", monster.CR, _stringService.CRXPString(monster.CR));
            cr.Append(crString, _normalFormat);
         }

         doc.InsertParagraph("", false, _normalFormat);

         if (monster.Traits.Any())
         {
            doc.InsertParagraph("Traits", false, _headerFormat);
            doc.InsertParagraph("", false, _normalFormat);

            foreach (TraitModel trait in monster.Traits)
            {
               doc.InsertParagraph(trait.Name, false, _labelFormat);
               doc.InsertParagraph(String.Join(Environment.NewLine + Environment.NewLine, trait.TextCollection), false, _normalFormat);
               doc.InsertParagraph("", false, _normalFormat);
            }
         }

         if (monster.Actions.Any())
         {
            doc.InsertParagraph("Actions", false, _headerFormat);
            doc.InsertParagraph("", false, _normalFormat);

            foreach (MonsterActionModel action in monster.Actions)
            {
               doc.InsertParagraph(action.Name, false, _labelFormat);
               doc.InsertParagraph(String.Join(Environment.NewLine + Environment.NewLine, action.TextCollection), false, _normalFormat);
               doc.InsertParagraph("", false, _normalFormat);
            }
         }

         if (monster.LegendaryActions.Any())
         {
            doc.InsertParagraph("Legendary Actions", false, _headerFormat);
            doc.InsertParagraph("", false, _normalFormat);

            foreach (MonsterActionModel action in monster.LegendaryActions)
            {
               doc.InsertParagraph(action.Name, false, _labelFormat);
               doc.InsertParagraph(String.Join(Environment.NewLine + Environment.NewLine, action.TextCollection), false, _normalFormat);
               doc.InsertParagraph("", false, _normalFormat);
            }
         }

         if (monster.Reactions.Any())
         {
            doc.InsertParagraph("Reactions", false, _headerFormat);
            doc.InsertParagraph("", false, _normalFormat);

            foreach (MonsterActionModel action in monster.Reactions)
            {
               doc.InsertParagraph(action.Name, false, _labelFormat);
               doc.InsertParagraph(String.Join(Environment.NewLine + Environment.NewLine, action.TextCollection), false, _normalFormat);
               doc.InsertParagraph("", false, _normalFormat);
            }
         }

         if (monster.SpellSlots.Any() || monster.Spells.Any())
         {
            doc.InsertParagraph("Spells", false, _headerFormat);
            doc.InsertParagraph("", false, _normalFormat);
         }

         if (monster.SpellSlots.Any())
         {
            doc.InsertParagraph("1st\t2nd\t3rd\t4th\t5th\t6th\t7th\t8th\t9th", false, _labelFormat);
            Paragraph slotParagraph = doc.InsertParagraph();
            for (int i = 0; i < 9; ++i)
            {
               slotParagraph.Append(monster.SpellSlots.Count > i ? monster.SpellSlots[i] + "\t" : "0\t", _normalFormat);
            }
            doc.InsertParagraph("", false, _normalFormat);
         }

         if (monster.Spells.Any())
         {
            doc.InsertParagraph(String.Join(", ", monster.Spells), false, _normalFormat);
         }

         doc.Save();
      }

      /// <summary>
      /// Creates a word document for the spell
      /// </summary>
      public void CreateWordDoc(string fileLocaiton, SpellModel spell)
      {
         DocX doc = DocX.Create(fileLocaiton);

         doc.PageHeight = _letterHeight;
         doc.PageWidth = _letterWidth;

         string level = "Unknown Level";

         if (spell.Level == 0)
         {
            level = "Cantrip";
         }
         else if (spell.Level > 0)
         {
            level = "Level " + spell.Level.ToString();
         }

         doc.InsertParagraph(spell.Name, false, _nameFormat);
         doc.InsertParagraph(_stringService.CapitalizeWords(String.Format("{0}, {1}", level, UnknownIfNullOrEmpty(_stringService.GetString(spell.SpellSchool)))), false, _normalFormat);
         doc.InsertParagraph("", false, _normalFormat);

         Paragraph range = doc.InsertParagraph("Range: ", false, _labelFormat);
         range.Append(UnknownIfNullOrEmpty(spell.Range), _normalFormat);

         Paragraph ritual = doc.InsertParagraph("Ritual: ", false, _labelFormat);
         ritual.Append(spell.IsRitual ? "Yes" : "No", _normalFormat);

         Paragraph time = doc.InsertParagraph("Casting Time: ", false, _labelFormat);
         time.Append(UnknownIfNullOrEmpty(spell.CastingTime), _normalFormat);

         Paragraph duration = doc.InsertParagraph("Duration: ", false, _labelFormat);
         duration.Append(UnknownIfNullOrEmpty(spell.Duration), _normalFormat);

         doc.InsertParagraph("", false, _normalFormat);

         doc.InsertParagraph("Components", false, _labelFormat);
         doc.InsertParagraph(NoneIfNullOrEmpty(spell.Components), false, _normalFormat);
         doc.InsertParagraph("", false, _normalFormat);

         doc.InsertParagraph("Classes", false, _labelFormat);
         doc.InsertParagraph(UnknownIfNullOrEmpty(spell.Classes), false, _normalFormat);
         doc.InsertParagraph("", false, _normalFormat);

         List<string> text = new List<string>();
         foreach (string s in spell.TextCollection)
         {
            text.Add(s.Replace("\t", "").Trim());
         }
         doc.InsertParagraph("Description", false, _labelFormat);
         doc.InsertParagraph(String.Join(Environment.NewLine + Environment.NewLine, text), false, _normalFormat);

         doc.Save();
      }

      /// <summary>
      /// Creates a word document for the spell
      /// </summary>
      public void CreateWordDoc(string fileLocaiton, SpellbookViewModel spellbookViewModel)
      {
         DocX doc = DocX.Create(fileLocaiton);

         doc.PageHeight = _letterHeight;
         doc.PageWidth = _letterWidth;
         doc.MarginBottom = 36;
         doc.MarginLeft = 36;
         doc.MarginRight = 36;
         doc.MarginTop = 36;

         Formatting titleFormat = new Formatting();
         titleFormat.FontFamily = new Font("Bookman Old Style");
         titleFormat.Size = 14;
         titleFormat.Bold = true;
         titleFormat.FontColor = System.Drawing.Color.FromArgb(64, 64, 64);

         Formatting levelFormat = new Formatting();
         levelFormat.FontFamily = new Font("Bookman Old Style");
         levelFormat.Size = 13;
         levelFormat.FontColor = System.Drawing.Color.FromArgb(64, 64, 64);

         Formatting levelFormatBold = new Formatting();
         levelFormatBold.FontFamily = new Font("Bookman Old Style");
         levelFormatBold.Size = 13;
         levelFormatBold.Bold = true;
         levelFormatBold.FontColor = System.Drawing.Color.FromArgb(64, 64, 64);

         Formatting labelFormat = new Formatting();
         labelFormat = new Formatting();
         labelFormat.FontFamily = new Font("Bookman Old Style");
         labelFormat.Bold = true;
         labelFormat.Size = 7;
         labelFormat.FontColor = System.Drawing.Color.FromArgb(148, 148, 148);

         Formatting normalFormat = new Formatting();
         normalFormat.FontFamily = new Font("Bookman Old Style");
         normalFormat.Size = 10;
         normalFormat.FontColor = System.Drawing.Color.FromArgb(64, 64, 64);

         Formatting smallFormat = new Formatting();
         smallFormat.FontFamily = new Font("Bookman Old Style");
         smallFormat.Size = 8;
         smallFormat.FontColor = System.Drawing.Color.FromArgb(64, 64, 64);

         Image checkboxImage;
         Image checkboxCheckedImage;
         using (Stream imageStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/CritCompendium;component/Resources/Images/checkbox.jpg")).Stream)
         {
            checkboxImage = doc.AddImage(imageStream);
         }
         using (Stream imageStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/CritCompendium;component/Resources/Images/checkbox_checked.jpg")).Stream)
         {
            checkboxCheckedImage = doc.AddImage(imageStream);
         }
         Picture checkboxPicture = checkboxImage.CreatePicture(16, 16);
         Picture checkboxCheckedPicture = checkboxCheckedImage.CreatePicture(16, 16);

         Table boolDetailsTable = doc.InsertTable(2, 4);

         boolDetailsTable.SetBorder(TableBorderType.Bottom, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, System.Drawing.Color.FromArgb(196, 196, 196)));
         boolDetailsTable.SetWidthsPercentage(new float[] { 50f, 16.6f, 16.6f, 16.6f }, null);

         boolDetailsTable.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.left;
         boolDetailsTable.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
         boolDetailsTable.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
         boolDetailsTable.Rows[0].Cells[3].Paragraphs[0].Alignment = Alignment.center;
         boolDetailsTable.Rows[1].Cells[0].Paragraphs[0].Alignment = Alignment.left;
         boolDetailsTable.Rows[1].Cells[1].Paragraphs[0].Alignment = Alignment.center;
         boolDetailsTable.Rows[1].Cells[2].Paragraphs[0].Alignment = Alignment.center;
         boolDetailsTable.Rows[1].Cells[3].Paragraphs[0].Alignment = Alignment.center;

         boolDetailsTable.Rows[1].Cells[1].Paragraphs[0].LineSpacingAfter = 6f;
         boolDetailsTable.Rows[1].Cells[2].Paragraphs[0].LineSpacingAfter = 6f;
         boolDetailsTable.Rows[1].Cells[3].Paragraphs[0].LineSpacingAfter = 6f;

         boolDetailsTable.MergeCellsInColumn(0, 0, 1);

         boolDetailsTable.Rows[0].Cells[0].Paragraphs[0].InsertText(spellbookViewModel.Name, false, titleFormat);

         boolDetailsTable.Rows[0].Cells[1].Paragraphs[0].InsertText(_stringService.GetString(spellbookViewModel.Ability), false, normalFormat);
         boolDetailsTable.Rows[0].Cells[2].Paragraphs[0].InsertText(_statService.AddPlusOrMinus(spellbookViewModel.SaveDC), false, normalFormat);
         boolDetailsTable.Rows[0].Cells[3].Paragraphs[0].InsertText(_statService.AddPlusOrMinus(spellbookViewModel.ToHitBonus), false, normalFormat);
         boolDetailsTable.Rows[1].Cells[1].Paragraphs[0].InsertText("Spellcasting Ability", false, labelFormat);
         boolDetailsTable.Rows[1].Cells[2].Paragraphs[0].InsertText("Spell Save DC", false, labelFormat);
         boolDetailsTable.Rows[1].Cells[3].Paragraphs[0].InsertText("Spell Attack Bonus", false, labelFormat);

         int minLevel = spellbookViewModel.Spells.Where(x => x.Spell != null).Min(x => x.Spell.Level);
         int maxLevel = spellbookViewModel.Spells.Where(x => x.Spell != null).Max(x => x.Spell.Level);
         for (int i = minLevel; i <= maxLevel; ++i)
         {
            IEnumerable<SpellbookEntryViewModel> spellsOfLevel = spellbookViewModel.Spells.Where(x => x.Spell != null && x.Spell.Level == i).OrderBy(x => x.Spell.Name);

            if (spellsOfLevel.Any())
            {
               doc.InsertParagraph("", false, normalFormat);

               string level = "Unknown Level";

               if (i == 0)
               {
                  level = "Cantrips";
               }
               else if (i > 0)
               {
                  level = $"Level {i}";
               }

               Paragraph levelParagraph = doc.InsertParagraph(level, false, levelFormatBold);
               if (i > 0)
               {
                  levelParagraph.Append($": ___/{spellbookViewModel.SpellSlots.ElementAt(i)}", levelFormat);
               }

               foreach (SpellbookEntryViewModel spellbookEntryViewModel in spellsOfLevel)
               {
                  doc.InsertParagraph("", false, smallFormat);

                  Table spellTable = doc.InsertTable(3, 7);

                  spellTable.Rows[0].BreakAcrossPages = false;
                  spellTable.Rows[1].BreakAcrossPages = false;
                  spellTable.Rows[2].BreakAcrossPages = false;

                  spellTable.SetBorder(TableBorderType.Bottom, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, System.Drawing.Color.FromArgb(196, 196, 196)));

                  spellTable.SetWidthsPercentage(new float[] { 4f, 22f, 8f, 8f, 8f, 16f, 34f }, null);

                  spellTable.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                  spellTable.Rows[1].Cells[0].Paragraphs[0].Alignment = Alignment.center;

                  spellTable.Rows[0].Cells[0].Paragraphs[0].LineSpacingBefore = 6f;
                  spellTable.Rows[0].Cells[1].Paragraphs[0].LineSpacingBefore = 6f;
                  spellTable.Rows[0].Cells[2].Paragraphs[0].LineSpacingBefore = 6f;
                  spellTable.Rows[0].Cells[3].Paragraphs[0].LineSpacingBefore = 6f;
                  spellTable.Rows[0].Cells[4].Paragraphs[0].LineSpacingBefore = 6f;
                  spellTable.Rows[0].Cells[5].Paragraphs[0].LineSpacingBefore = 6f;
                  spellTable.Rows[0].Cells[6].Paragraphs[0].LineSpacingBefore = 6f;

                  spellTable.Rows[1].Cells[1].Paragraphs[0].LineSpacingBefore = 1.6f;
                  spellTable.Rows[1].Cells[2].Paragraphs[0].LineSpacingBefore = 1.6f;
                  spellTable.Rows[1].Cells[3].Paragraphs[0].LineSpacingBefore = 1.6f;
                  spellTable.Rows[1].Cells[4].Paragraphs[0].LineSpacingBefore = 1.6f;
                  spellTable.Rows[1].Cells[5].Paragraphs[0].LineSpacingBefore = 1.6f;
                  spellTable.Rows[1].Cells[6].Paragraphs[0].LineSpacingBefore = 1.6f;

                  spellTable.Rows[0].Cells[0].Paragraphs[0].InsertText(spellbookViewModel.BasedOnClass ? "P" : "U", false, labelFormat);
                  spellTable.Rows[0].Cells[1].Paragraphs[0].InsertText("Name", false, labelFormat);
                  spellTable.Rows[0].Cells[2].Paragraphs[0].InsertText("School", false, labelFormat);
                  spellTable.Rows[0].Cells[3].Paragraphs[0].InsertText("Time", false, labelFormat);
                  spellTable.Rows[0].Cells[4].Paragraphs[0].InsertText("Range", false, labelFormat);
                  spellTable.Rows[0].Cells[5].Paragraphs[0].InsertText("Duration", false, labelFormat);
                  spellTable.Rows[0].Cells[6].Paragraphs[0].InsertText("Components", false, labelFormat);

                  spellTable.Rows[1].Cells[0].Paragraphs[0].InsertPicture((spellbookViewModel.BasedOnRace || !spellbookEntryViewModel.Prepared) ? checkboxPicture : checkboxCheckedPicture);
                  spellTable.Rows[1].Cells[1].Paragraphs[0].InsertText(spellbookEntryViewModel.Spell.Name, false, normalFormat);
                  spellTable.Rows[1].Cells[2].Paragraphs[0].InsertText(_stringService.GetAbbreviationString(spellbookEntryViewModel.Spell.SpellSchool).Trim(), false, normalFormat);
                  spellTable.Rows[1].Cells[3].Paragraphs[0].InsertText(AbbreviateCastingTime(spellbookEntryViewModel.Spell.CastingTime), false, normalFormat);
                  spellTable.Rows[1].Cells[4].Paragraphs[0].InsertText(AbbreviateRange(spellbookEntryViewModel.Spell.Range), false, normalFormat);
                  spellTable.Rows[1].Cells[5].Paragraphs[0].InsertText(AbbreviateDuration(spellbookEntryViewModel.Spell.Duration), false, normalFormat);
                  spellTable.Rows[1].Cells[6].Paragraphs[0].InsertText(spellbookEntryViewModel.Spell.Components.Trim(), false, normalFormat);

                  spellTable.Rows[2].MergeCells(0, 6);

                  spellTable.Rows[2].Cells[0].Paragraphs[0].LineSpacingAfter = 12f;
                  spellTable.Rows[2].Cells[0].Paragraphs[0].LineSpacingBefore = 6f;
                  spellTable.Rows[2].Cells[0].Paragraphs[0].InsertText(String.Join(" ", spellbookEntryViewModel.Spell.TextCollection.Select(x => x.Trim())), false, smallFormat);
               }
            }
         }

         doc.Save();
      }

      #endregion

      #region Private Methods

      private string UnknownIfNullOrEmpty(object obj)
      {
         return ReplaceIfNullOrEmpty(obj, "Unknown");
      }

      private string NoneIfNullOrEmpty(object obj)
      {
         return ReplaceIfNullOrEmpty(obj, "None");
      }

      private string ReplaceIfNullOrEmpty(object obj, string replace)
      {
         string value = replace;

         if (obj != null && !String.IsNullOrWhiteSpace(obj.ToString()))
         {
            value = obj.ToString();
         }

         return value;
      }

      private void BuildCharacterMaps(CharacterModel characterModel)
      {
         CharacterViewModel characterView = new CharacterViewModel(characterModel);

         _characterTextBoxMap.Clear();
         _characterCheckBoxMap.Clear();

         _characterTextBoxMap["CharacterName"] = characterView.Name;
         _characterTextBoxMap["CharacterName 2"] = characterView.Name;
         _characterTextBoxMap["ClassLevel"] = characterView.ClassDisplay;
         _characterTextBoxMap["Background"] = characterView.Background.Name;
         _characterTextBoxMap["Race"] = characterView.Race.Name;
         _characterTextBoxMap["Alignment"] = characterView.Alignment;
         _characterTextBoxMap["ProfBonus"] = characterView.ProficiencyDisplay;
         _characterTextBoxMap["STRmod"] = _statService.GetStatBonusString(characterView.TotalStrength);
         _characterTextBoxMap["DEXmod"] = _statService.GetStatBonusString(characterView.TotalDexterity);
         _characterTextBoxMap["CONmod"] = _statService.GetStatBonusString(characterView.TotalConstitution);
         _characterTextBoxMap["INTmod"] = _statService.GetStatBonusString(characterView.TotalIntelligence);
         _characterTextBoxMap["WISmod"] = _statService.GetStatBonusString(characterView.TotalWisdom);
         _characterTextBoxMap["CHAmod"] = _statService.GetStatBonusString(characterView.TotalCharisma);
         _characterTextBoxMap["STR"] = characterView.TotalStrength.ToString();
         _characterTextBoxMap["DEX"] = characterView.TotalDexterity.ToString();
         _characterTextBoxMap["CON"] = characterView.TotalConstitution.ToString();
         _characterTextBoxMap["INT"] = characterView.TotalIntelligence.ToString();
         _characterTextBoxMap["WIS"] = characterView.TotalWisdom.ToString();
         _characterTextBoxMap["CHA"] = characterView.TotalCharisma.ToString();
         _characterTextBoxMap["Passive"] = characterView.PassivePerception.ToString();
         _characterTextBoxMap["AC"] = characterView.AC.ToString();
         _characterTextBoxMap["Initiative"] = characterView.InitiativeDisplay;
         _characterTextBoxMap["Speed"] = characterView.WalkSpeed.ToString();
         _characterTextBoxMap["Character_HP"] = characterView.MaxHP.ToString();

         Dictionary<int, int> hitDice = new Dictionary<int, int>();
         foreach (LevelModel level in characterModel.Levels)
         {
            if (level.Class != null)
            {
               if (hitDice.ContainsKey(level.Class.HitDie))
               {
                  hitDice[level.Class.HitDie]++;
               }
               else
               {
                  hitDice[level.Class.HitDie] = 1;
               }
            }
         }
         _characterTextBoxMap["HDTotal"] = String.Join(", ", hitDice.Select(x => $"{x.Value}d{x.Key}"));

         List<string> attacks = new List<string>();
         for (int i = 0; i < characterView.Attacks.Count(); ++i)
         {
            AttackViewModel attackViewModel = characterView.Attacks.ElementAt(i);
            _characterTextBoxMap[$"Wpn Name {i}"] = attackViewModel.Name;

            if (attackViewModel.ShowToHit)
            {
               _characterTextBoxMap[$"Wpn{i} AtkBonus"] = attackViewModel.ToHit;
            }

            string damage = attackViewModel.Damage;

            if (attackViewModel.ShowDamage)
            {
               _characterTextBoxMap[$"Wpn{i} Damage"] = damage;
            }

            if (i > 2)
            {
               attacks.Add($"{attackViewModel.Name}\t{attackViewModel.ToHit}\t{damage}");
            }
         }

         foreach (CounterViewModel counterViewModel in characterView.Counters)
         {
            attacks.Add($"{counterViewModel.Name}\t ____ / {counterViewModel.MaxValue}");
         }

         _characterTextBoxMap["AttacksSpellcasting"] = String.Join("\n", attacks);

         _characterTextBoxMap["PersonalityTraits"] = characterView.PersonalityTraits;
         _characterTextBoxMap["Ideals"] = characterView.Ideals;
         _characterTextBoxMap["Bonds"] = characterView.Bonds;
         _characterTextBoxMap["Flaws"] = characterView.Flaws;

         List<string> features = new List<string>();
         foreach (FeatureViewModel featureView in characterView.Features)
         {
            features.Add(featureView.Name);
         }
         _characterTextBoxMap["Features and Traits"] = String.Join("\n", features);

         List<string> proficiencies = new List<string>();
         if (!String.IsNullOrWhiteSpace(characterView.SavingThrowNotes))
         {
            proficiencies.Add(characterView.SavingThrowNotes);
         }
         if (characterView.ArmorProficiencies != "None")
         {
            proficiencies.Add(characterView.ArmorProficiencies);
         }
         if (characterView.WeaponProficiencies != "None")
         {
            proficiencies.Add(characterView.WeaponProficiencies);
         }
         if (characterView.ToolProficiencies != "None")
         {
            proficiencies.Add(characterView.ToolProficiencies);
         }
         if (characterView.Languages != "None")
         {
            proficiencies.Add(characterView.Languages);
         }
         _characterTextBoxMap["ProficienciesLang"] = String.Join("\n\n", proficiencies);

         List<string> equipment = new List<string>();
         foreach (EquipmentViewModel equipmentView in characterView.EquippedItems)
         {
            if (equipmentView.Quantity > 1)
            {
               equipment.Add($"{equipmentView.Quantity}x {equipmentView.Name}");
            }
            else
            {
               equipment.Add(equipmentView.Name);
            }
         }
         _characterTextBoxMap["Equipment"] = String.Join(", ", equipment);

         List<string> treasure = new List<string>();
         foreach (BagViewModel bagView in characterView.Bags)
         {
            List<string> unequippedItems = new List<string>();
            foreach (EquipmentViewModel equipmentView in bagView.Equipment.Where(x => !x.Equipped))
            {
               if (equipmentView.Quantity > 1)
               {
                  unequippedItems.Add($"{equipmentView.Quantity}x {equipmentView.Name}");
               }
               else
               {
                  unequippedItems.Add(equipmentView.Name);
               }
            }
            treasure.Add(String.Join(", ", unequippedItems));
         }
         _characterTextBoxMap["Treasure"] = String.Join("\n\n", treasure);

         _characterTextBoxMap["Age"] = characterView.Age.ToString();
         _characterTextBoxMap["Height"] = characterView.Height;
         _characterTextBoxMap["Weight"] = characterView.Weight.ToString();
         _characterTextBoxMap["Eyes"] = characterView.Eyes;
         _characterTextBoxMap["Skin"] = characterView.Skin;
         _characterTextBoxMap["Hair"] = characterView.Hair;
         _characterTextBoxMap["Backstory"] = characterView.Backstory;

         List<string> featsAndTraits = new List<string>();
         foreach (FeatViewModel featView in characterView.Feats)
         {
            featsAndTraits.Add(featView.Name + "\n" + featView.Description);
         }
         if (characterModel.Race != null)
         {
            foreach (TraitModel trait in characterModel.Race.Traits)
            {
               featsAndTraits.Add(trait.Name + "\n" + trait.Text);
            }
         }
         _characterTextBoxMap["Feat+Traits"] = String.Join("\n\n", featsAndTraits);

         for (int i = 0; i < characterView.Spellbooks.Count(); ++i)
         {
            SpellbookViewModel spellbookView = characterView.Spellbooks.ElementAt(i);

            if (spellbookView.BasedOnClass)
            {
               string bookIndex = i > 0 ? $" ({i.ToString()})" : String.Empty;

               if (spellbookView.Class != null)
               {
                  _characterTextBoxMap[$"Spellcasting Class" + bookIndex] = spellbookView.Class.Name;
               }
               _characterTextBoxMap["SpellcastingAbility" + bookIndex] = spellbookView.AbilityDisplay;
               _characterTextBoxMap["SpellSaveDC" + bookIndex] = spellbookView.SaveDC.ToString();
               _characterTextBoxMap["SpellAtkBonus" + bookIndex] = spellbookView.ToHitBonusDisplay;


               _characterTextBoxMap["SlotsTotal 1" + bookIndex] = spellbookView.MaxFirstLevelSpellSlots.ToString();
               _characterTextBoxMap["SlotsTotal 2" + bookIndex] = spellbookView.MaxSecondLevelSpellSlots.ToString();
               _characterTextBoxMap["SlotsTotal 3" + bookIndex] = spellbookView.MaxThirdLevelSpellSlots.ToString();
               _characterTextBoxMap["SlotsTotal 4" + bookIndex] = spellbookView.MaxFourthLevelSpellSlots.ToString();
               _characterTextBoxMap["SlotsTotal 5" + bookIndex] = spellbookView.MaxFifthLevelSpellSlots.ToString();
               _characterTextBoxMap["SlotsTotal 6" + bookIndex] = spellbookView.MaxSixthLevelSpellSlots.ToString();
               _characterTextBoxMap["SlotsTotal 7" + bookIndex] = spellbookView.MaxSeventhLevelSpellSlots.ToString();
               _characterTextBoxMap["SlotsTotal 8" + bookIndex] = spellbookView.MaxEighthLevelSpellSlots.ToString();
               _characterTextBoxMap["SlotsTotal 9" + bookIndex] = spellbookView.MaxNinthLevelSpellSlots.ToString();

               for (int j = 0; j < spellbookView.Cantrips.Count(); ++j)
               {
                  _characterTextBoxMap[$"Cantrip{j}" + bookIndex] = spellbookView.Cantrips.ElementAt(j).SpellName;
               }

               for (int j = 0; j < spellbookView.FirstLevelSpells.Count(); ++j)
               {
                  _characterTextBoxMap[$"FirstLevelSpell{j}" + bookIndex] = spellbookView.FirstLevelSpells.ElementAt(j).SpellName;
               }

               for (int j = 0; j < spellbookView.SecondLevelSpells.Count(); ++j)
               {
                  _characterTextBoxMap[$"SecondLevelSpell{j}" + bookIndex] = spellbookView.SecondLevelSpells.ElementAt(j).SpellName;
               }

               for (int j = 0; j < spellbookView.ThirdLevelSpells.Count(); ++j)
               {
                  _characterTextBoxMap[$"ThirdLevelSpell{j}" + bookIndex] = spellbookView.ThirdLevelSpells.ElementAt(j).SpellName;
               }

               for (int j = 0; j < spellbookView.FourthLevelSpells.Count(); ++j)
               {
                  _characterTextBoxMap[$"FourthLevelSpell{j}" + bookIndex] = spellbookView.FourthLevelSpells.ElementAt(j).SpellName;
               }

               for (int j = 0; j < spellbookView.FifthLevelSpells.Count(); ++j)
               {
                  _characterTextBoxMap[$"FifthLevelSpell{j}" + bookIndex] = spellbookView.FifthLevelSpells.ElementAt(j).SpellName;
               }

               for (int j = 0; j < spellbookView.SixthLevelSpells.Count(); ++j)
               {
                  _characterTextBoxMap[$"SixthLevelSpell{j}" + bookIndex] = spellbookView.SixthLevelSpells.ElementAt(j).SpellName;
               }

               for (int j = 0; j < spellbookView.SeventhLevelSpells.Count(); ++j)
               {
                  _characterTextBoxMap[$"SeventhLevelSpell{j}" + bookIndex] = spellbookView.SeventhLevelSpells.ElementAt(j).SpellName;
               }

               for (int j = 0; j < spellbookView.EighthLevelSpells.Count(); ++j)
               {
                  _characterTextBoxMap[$"EighthLevelSpell{j}" + bookIndex] = spellbookView.EighthLevelSpells.ElementAt(j).SpellName;
               }

               for (int j = 0; j < spellbookView.NinthLevelSpells.Count(); ++j)
               {
                  _characterTextBoxMap[$"NinthLevelSpell{j}" + bookIndex] = spellbookView.NinthLevelSpells.ElementAt(j).SpellName;
               }
            }
         }

         foreach (AbilityViewModel abilityView in characterView.AbilityProficiencies)
         {
            _characterCheckBoxMap[$"ST {abilityView.AbilityString}"] = abilityView.Proficient;
            _characterTextBoxMap[$"{abilityView.AbilityString}SavingThrow"] = abilityView.SaveBonusString;
         }

         foreach (SkillViewModel skillView in characterView.SkillProficiencies)
         {
            _characterCheckBoxMap[$"ChBx {skillView.SkillString}"] = skillView.Proficient;
            _characterCheckBoxMap[$"ChBx {skillView.SkillString} E"] = skillView.Expertise;
            _characterTextBoxMap[skillView.SkillString] = skillView.BonusString;
         }
      }

      private PdfDocument CopyDocument(Stream pdfStream)
      {
         string uri = @"pack://application:,,,/" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ";component/Resources/Images/cc_title_dark.png";
         System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage(new Uri(uri, UriKind.Absolute));
         PdfImage streamLogo = null;
         using (MemoryStream outStream = new MemoryStream())
         {
            System.Windows.Media.Imaging.PngBitmapEncoder enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmapImage));
            enc.Save(outStream);
            streamLogo = PdfImage.FromStream(outStream);
         }

         PdfDocument originalDocument = new PdfDocument(pdfStream);
         PdfFormWidget originalForm = originalDocument.Form as PdfFormWidget;

         PdfDocument newDocument = new PdfDocument(pdfStream);
         PdfFormWidget newForm = newDocument.Form as PdfFormWidget;
         newForm.FieldsWidget.Clear();
         newForm.Fields.Clear();
         newDocument.Pages.RemoveAt(0);
         newDocument.Pages.RemoveAt(0);
         newDocument.Pages.RemoveAt(0);

         for (int i = 0; i < originalDocument.Pages.Count; ++i)
         {
            PdfPageBase page = originalDocument.Pages[i];
            PdfTemplate template = page.CreateTemplate();

            PdfPageBase newPage = newDocument.Pages.Add(page.Size, new PdfMargins(0));
            newPage.Canvas.DrawTemplate(template, new System.Drawing.PointF(0, 0));
            newPage.Canvas.DrawImage(streamLogo, 50, 18, 196, 30);

            for (int j = 0; j < originalForm.FieldsWidget.List.Count; ++j)
            {
               PdfField field = originalForm.FieldsWidget.List[j] as PdfField;

               if (field.Page == page)
               {
                  if (field is PdfTextBoxFieldWidget textField)
                  {
                     PdfTextBoxField newTextField = new PdfTextBoxField(newPage, textField.Name);
                     newTextField.Bounds = textField.Bounds;
                     newTextField.BackColor = PdfRGBColor.Empty;
                     newTextField.BorderWidth = 0.0f;
                     newTextField.Multiline = textField.Multiline;
                     newTextField.TextAlignment = textField.TextAlignment;

                     if (i == 0 && IsSaveOrSkill(textField.Name))
                     {
                        newTextField.Font = new PdfFont((PdfFont)textField.Font, 6.0f);
                     }
                     else
                     {
                        newTextField.Font = textField.Font;
                     }

                     newForm.FieldsWidget.Add(newTextField);
                  }

                  if (field is PdfCheckBoxWidgetFieldWidget checkBoxField)
                  {
                     PdfCheckBoxField newCheckField = new PdfCheckBoxField(newPage, checkBoxField.Name);
                     newCheckField.Bounds = checkBoxField.Bounds;
                     newCheckField.BackColor = PdfRGBColor.Empty;
                     newCheckField.BorderWidth = 0.0f;
                     newCheckField.Style = PdfCheckBoxStyle.Circle;
                     newForm.FieldsWidget.Add(newCheckField);
                  }

                  if (field is PdfButtonWidgetFieldWidget buttonField)
                  {
                     PdfButtonField newButtonField = new PdfButtonField(newPage, buttonField.Name);
                     newButtonField.Bounds = buttonField.Bounds;
                     newButtonField.BackColor = PdfRGBColor.Empty;
                     newButtonField.BorderWidth = 0.0f;
                     newButtonField.LayoutMode = PdfButtonLayoutMode.IconOnly;
                     newButtonField.Actions.MouseUp = new PdfJavaScriptAction("event.target.buttonImportIcon();");
                     newForm.FieldsWidget.Add(newButtonField);
                  }
               }
            }
         }

         return newDocument;
      }

      private bool IsSaveOrSkill(string name)
      {
         return name.Contains("SavingThrow") ||
                name == "Athletics" ||
                name == "Acrobatics" ||
                name == "Sleight of Hand" ||
                name == "Stealth" ||
                name == "Arcana" ||
                name == "History" ||
                name == "Investigation" ||
                name == "Nature" ||
                name == "Religion" ||
                name == "Animal Handling" ||
                name == "Insight" ||
                name == "Medicine" ||
                name == "Perception" ||
                name == "Survival" ||
                name == "Deception" ||
                name == "Intimidation" ||
                name == "Performance" ||
                name == "Persuasion";
      }

      private string AbbreviateCastingTime(string castingTime)
      {
         string abbreviated = castingTime.Trim().ToLower().Replace("reaction", "R").Replace("bonus action", "BA").Replace("action", "A").
             Replace("hour", "h").Replace("minute", "m");
         return _stringService.CapitalizeWords(abbreviated);
      }

      private string AbbreviateRange(string range)
      {
         string abbreviated = range.Trim().ToLower().Replace("feet", "ft").Replace("-foot", " ft").Replace("foot", "ft");
         return _stringService.CapitalizeWords(abbreviated);
      }

      private string AbbreviateDuration(string duration)
      {
         string abbreviated = duration.Trim().ToLower().Replace("concentration,", "(C)").Replace("instantaneous", "Inst").
             Replace("hours", "h").Replace("minutes", "m").Replace("hour", "h").Replace("minute", "m");
         return _stringService.CapitalizeWords(abbreviated);
      }

      #endregion
   }
}
