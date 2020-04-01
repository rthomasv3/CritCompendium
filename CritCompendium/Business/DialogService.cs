using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CritCompendium.ViewModels;
using CritCompendium.ViewModels.DialogViewModels;
using CritCompendium.ViewModels.ObjectViewModels;
using CritCompendium.Views;
using CritCompendium.Views.SearchViews;
using CritCompendiumInfrastructure;
using CritCompendiumInfrastructure.Models;

namespace CritCompendium.Business
{
   public sealed class DialogService
   {
      #region Events

      public event EventHandler DialogOpened;
      public event EventHandler DialogClosed;

      #endregion

      #region Fields

      private Window _parentWindow;

      #endregion

      #region Constructors

      /// <summary>
      /// Creates and instance of <see cref="DialogService"/>
      /// </summary>
      public DialogService()
      {
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Sets the owner of the dialogs
      /// </summary>
      public void SetOwner(Window owner)
      {
         _parentWindow = owner;
      }

      /// <summary>
      /// Shows confirmation dialog
      /// </summary>
      /// <returns></returns>
      public bool? ShowConfirmationDialog(string title, string body, string accept = "Yes", string reject = "No", string cancel = "Cancel")
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         ConfirmationView confirmationView = new ConfirmationView();
         ConfirmationViewModel viewModel = new ConfirmationViewModel(body, accept, reject, cancel);
         confirmationView.DataContext = viewModel;

         modalDialog.WindowTitle = title;
         modalDialog.Body = confirmationView;
         modalDialog.Confirmation = viewModel;

         return ShowDialog(modalDialog);
      }

      /// <summary>
      /// Shows the dice roll dialog
      /// </summary>
      public void ShowDiceRollDialog(string title, string toRoll)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         DiceRollView diceRollView = new DiceRollView();
         diceRollView.ViewModel.RollString = toRoll;

         modalDialog.WindowTitle = title;
         modalDialog.Body = diceRollView;

         ShowDialog(modalDialog);
      }

      /// <summary>
      /// Shows details dialog
      /// </summary>
      public void ShowDetailsDialog(BackgroundViewModel backgroundViewModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         modalDialog.WindowTitle = backgroundViewModel.Name;
         modalDialog.Body = new DetailsView(backgroundViewModel);

         ShowDialog(modalDialog);
      }

      /// <summary>
      /// Shows details dialog
      /// </summary>
      public void ShowDetailsDialog(ClassViewModel classViewModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         modalDialog.WindowTitle = classViewModel.Name;
         modalDialog.Body = new DetailsView(classViewModel);

         ShowDialog(modalDialog);
      }

      /// <summary>
      /// Shows details dialog
      /// </summary>
      public void ShowDetailsDialog(ConditionViewModel conditionViewModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         modalDialog.WindowTitle = conditionViewModel.Name;
         modalDialog.Body = new DetailsView(conditionViewModel);

         ShowDialog(modalDialog);
      }

      /// <summary>
      /// Shows details dialog
      /// </summary>
      public void ShowDetailsDialog(RaceViewModel raceViewModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         modalDialog.WindowTitle = raceViewModel.Name;
         modalDialog.Body = new DetailsView(raceViewModel);

         ShowDialog(modalDialog);
      }

      /// <summary>
      /// Shows details dialog
      /// </summary>
      public void ShowDetailsDialog(SpellViewModel spellViewModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         modalDialog.WindowTitle = spellViewModel.Name;
         modalDialog.Body = new DetailsView(spellViewModel);

         ShowDialog(modalDialog);
      }

      /// <summary>
      /// Shows details dialog
      /// </summary>
      public void ShowDetailsDialog(FeatureViewModel featureViewModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         modalDialog.WindowTitle = featureViewModel.Name;
         modalDialog.Body = new DetailsView(featureViewModel);

         ShowDialog(modalDialog);
      }

      /// <summary>
      /// Shows details dialog
      /// </summary>
      public void ShowDetailsDialog(FeatViewModel featViewModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         modalDialog.WindowTitle = featViewModel.Name;
         modalDialog.Body = new DetailsView(featViewModel);

         ShowDialog(modalDialog);
      }

      /// <summary>
      /// Shows details dialog
      /// </summary>
      public void ShowDetailsDialog(MonsterViewModel monsterViewModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         modalDialog.WindowTitle = monsterViewModel.Name;
         modalDialog.Body = new DetailsView(monsterViewModel);

         ShowDialog(modalDialog);
      }

      /// <summary>
      /// Shows details dialog
      /// </summary>
      public void ShowDetailsDialog(ItemViewModel itemViewModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         modalDialog.WindowTitle = itemViewModel.Name;
         modalDialog.Body = new DetailsView(itemViewModel);

         ShowDialog(modalDialog);
      }

      /// <summary>
      /// Shows encounter character dialog
      /// </summary>
      public EncounterCharacterModel ShowEncounterCharacterDialog(string title, EncounterCharacterModel encounterCharacterModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         CreateEncounterCharacterView createEncounterCharacterView = new CreateEncounterCharacterView();
         EncounterCharacterModel modelCopy = new EncounterCharacterModel(encounterCharacterModel);
         EncounterCharacterDialogViewModel viewModel = new EncounterCharacterDialogViewModel(modelCopy);
         createEncounterCharacterView.DataContext = viewModel;

         modalDialog.WindowTitle = title;
         modalDialog.Body = createEncounterCharacterView;
         modalDialog.Confirmation = viewModel;

         ShowDialog(modalDialog);

         return modalDialog.Result == true ? viewModel.EncounterCharacterModel : null;
      }

      /// <summary>
      /// Shows encounter monster dialog
      /// </summary>
      public EncounterMonsterModel ShowEncounterMonsterDialog(string title, EncounterMonsterModel encounterMonsterModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         CreateEncounterMonsterView createEncounterMonsterView = new CreateEncounterMonsterView();
         EncounterMonsterModel modelCopy = new EncounterMonsterModel(encounterMonsterModel);
         EncounterMonsterDialogViewModel viewModel = new EncounterMonsterDialogViewModel(modelCopy);
         createEncounterMonsterView.DataContext = viewModel;

         modalDialog.WindowTitle = title;
         modalDialog.Body = createEncounterMonsterView;
         modalDialog.Confirmation = viewModel;

         ShowDialog(modalDialog);

         return modalDialog.Result == true ? viewModel.EncounterMonsterModel : null;
      }

      /// <summary>
      /// Shows the create random encounter dialog
      /// </summary>
      public IEnumerable<EncounterMonsterModel> ShowCreateRandomEncounterDialog(List<EncounterCharacterModel> encounterCharacters)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         CreateRandomEncounterView createRandomEncounterView = new CreateRandomEncounterView();
         RandomEncounterDialogViewModel viewModel = new RandomEncounterDialogViewModel(encounterCharacters);
         createRandomEncounterView.DataContext = viewModel;

         modalDialog.WindowTitle = "Create Random Encounter";
         modalDialog.Body = createRandomEncounterView;
         modalDialog.Confirmation = viewModel;

         ShowDialog(modalDialog);

         return modalDialog.Result == true && viewModel.Monsters.Any() ? viewModel.Monsters.Select(x => x.EncounterMonsterModel) : null;
      }

      /// <summary>
      /// Shows item search dialog
      /// </summary>
      public IEnumerable<ItemModel> ShowItemSearchDialog(bool multiselect)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         ItemSearchView itemSearchView = new ItemSearchView();

         modalDialog.WindowTitle = "Search Items";
         modalDialog.Body = itemSearchView;
         modalDialog.Confirmation = itemSearchView.ViewModel;

         ShowDialog(modalDialog);

         return modalDialog.Result == true ? itemSearchView.ViewModel.SelectedItems : Enumerable.Empty<ItemModel>();
      }

      /// <summary>
      /// Shows monster search dialog
      /// </summary>
      public IEnumerable<MonsterModel> ShowMonsterSearchDialog(bool multiselect)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         MonsterSearchView monsterSelectionView = new MonsterSearchView();

         modalDialog.WindowTitle = "Search Monsters";
         modalDialog.Body = monsterSelectionView;
         modalDialog.Confirmation = monsterSelectionView.ViewModel;

         ShowDialog(modalDialog);

         return modalDialog.Result == true ? monsterSelectionView.ViewModel.SelectedMonsters : Enumerable.Empty<MonsterModel>();
      }

      /// <summary>
      /// Shows spell search dialog
      /// </summary>
      public IEnumerable<SpellModel> ShowSpellSearchDialog(bool multiselect)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         SpellSearchView spellSearchView = new SpellSearchView();

         spellSearchView.ViewModel.MultiSelect = multiselect;

         modalDialog.WindowTitle = "Search Spells";
         modalDialog.Body = spellSearchView;
         modalDialog.Confirmation = spellSearchView.ViewModel;

         ShowDialog(modalDialog);

         return modalDialog.Result == true ? spellSearchView.ViewModel.SelectedSpells : Enumerable.Empty<SpellModel>();
      }

      /// <summary>
      /// Shows the calculator dialog
      /// </summary>
      public int? ShowCalculatorDialog(string expression, bool closeOnCalculate = false)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         CalculatorView calculatorView = new CalculatorView(expression, closeOnCalculate);

         modalDialog.WindowTitle = "Calculator";
         modalDialog.Body = calculatorView;

         Show(modalDialog);
         modalDialog.Owner = null;

         return calculatorView.ViewModel.Accepted ? (int?)calculatorView.ViewModel.Result : null;
      }

      /// <summary>
      /// Shows add subtract dialog
      /// </summary>
      public int? ShowAddSubtractDialog(int initialValue)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         AddSubtractViewModel addSubtractViewModel = new AddSubtractViewModel(initialValue);
         AddSubtractView addSubtractView = new AddSubtractView(addSubtractViewModel);

         modalDialog.WindowTitle = "Add/Subtract";
         modalDialog.Body = addSubtractView;
         modalDialog.Confirmation = addSubtractView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? addSubtractView.ViewModel.Result : null;
      }

      /// <summary>
      /// Shows the roll initiative dialog
      /// </summary>
      public bool? ShowInitiativeDialog(List<EncounterCreatureViewModel> creatures)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         InitiativeView initiativeView = new InitiativeView();
         initiativeView.ViewModel.SetCreatures(creatures);

         modalDialog.WindowTitle = "Roll Initiative";
         modalDialog.Body = initiativeView;
         modalDialog.Confirmation = initiativeView.ViewModel;

         return ShowDialog(modalDialog);
      }

      /// <summary>
      /// Shows create attack dialog
      /// </summary>
      public AttackModel ShowCreateAttackDialog(string title, AttackModel attackModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         AttackModel attackModelCopy = new AttackModel(attackModel);
         CreateAttackView createAttackView = new CreateAttackView(new AttackViewModel(attackModelCopy));

         modalDialog.WindowTitle = title;
         modalDialog.Body = createAttackView;
         modalDialog.Confirmation = createAttackView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? attackModelCopy : null;
      }

      /// <summary>
      /// Shows create applied condition dialog
      /// </summary>
      public AppliedConditionModel ShowCreateAppliedConditionDialog(string title, AppliedConditionModel appliedConditionModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         AppliedConditionModel appliedConditionModelCopy = new AppliedConditionModel(appliedConditionModel);
         ApplyConditionView applyConditionView = new ApplyConditionView(new AppliedConditionViewModel(appliedConditionModelCopy));

         modalDialog.WindowTitle = title;
         modalDialog.Body = applyConditionView;
         modalDialog.Confirmation = applyConditionView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? appliedConditionModelCopy : null;
      }

      /// <summary>
      /// Shows create counter dialog
      /// </summary>
      public CounterModel ShowCreateCounterDialog(string title, CounterModel counterModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         CounterModel counterModelCopy = new CounterModel(counterModel);
         CreateCounterView createCounterView = new CreateCounterView(new CounterViewModel(counterModelCopy));

         modalDialog.WindowTitle = title;
         modalDialog.Body = createCounterView;
         modalDialog.Confirmation = createCounterView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? counterModelCopy : null;
      }

      /// <summary>
      /// Shows create companion dialog
      /// </summary>
      public CompanionModel ShowCreateCompanionDialog(string title, CompanionModel companionModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         CompanionModel companionModelCopy = new CompanionModel(companionModel);
         CompanionViewModel companionViewModel = new CompanionViewModel(companionModelCopy);
         companionViewModel.InitializeMonsterOptions();
         CreateCompanionView createCompanionView = new CreateCompanionView(companionViewModel);

         modalDialog.WindowTitle = title;
         modalDialog.Body = createCompanionView;
         modalDialog.Confirmation = createCompanionView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? companionModelCopy : null;
      }

      /// <summary>
      /// Shows create bag dialog
      /// </summary>
      public BagModel ShowCreateBagDialog(string title, BagModel bagModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         BagModel bagModelCopy = new BagModel(bagModel);
         CreateBagView createBagView = new CreateBagView(new BagViewModel(bagModelCopy));

         modalDialog.WindowTitle = title;
         modalDialog.Body = createBagView;
         modalDialog.Confirmation = createBagView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? bagModelCopy : null;
      }

      /// <summary>
      /// Shows create equipment dialog
      /// </summary>
      public EquipmentModel ShowCreateEquipmentDialog(string title, EquipmentModel equipmentModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         EquipmentModel equipmentModelCopy = new EquipmentModel(equipmentModel);
         EquipmentViewModel equipmentViewModel = new EquipmentViewModel(equipmentModelCopy);
         equipmentViewModel.InitializeItemOptions();
         CreateEquipmentView createEquipmentView = new CreateEquipmentView(equipmentViewModel);

         modalDialog.WindowTitle = title;
         modalDialog.Body = createEquipmentView;
         modalDialog.Confirmation = createEquipmentView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? equipmentModelCopy : null;
      }

      /// <summary>
      /// Shows add pack dialog
      /// </summary>
      public PackModel ShowAddPackDialog()
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         PackViewModel packViewModel = new PackViewModel(new PackModel());
         packViewModel.InitializePackOptions();
         AddPackView addPackView = new AddPackView(packViewModel);

         modalDialog.WindowTitle = "Add Pack";
         modalDialog.Body = addPackView;
         modalDialog.Confirmation = addPackView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? addPackView.ViewModel.PackModel : null;
      }

      /// <summary>
      /// Shows create spellbook dialog
      /// </summary>
      public SpellbookModel ShowCreateSpellbookDialog(string title, SpellbookModel spellbookModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         SpellbookModel spellbookModelCopy = new SpellbookModel(spellbookModel);
         SpellbookViewModel spellbookViewModel = new SpellbookViewModel(spellbookModelCopy);
         spellbookViewModel.InitializeOptions();
         CreateSpellbookView createSpellbookView = new CreateSpellbookView(spellbookViewModel);

         modalDialog.WindowTitle = title;
         modalDialog.Body = createSpellbookView;
         modalDialog.Confirmation = createSpellbookView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? spellbookModelCopy : null;
      }

      /// <summary>
      /// Shows create stat modification model dialog
      /// </summary>
      public StatModificationModel ShowCreatetatModificationDialog(string title, StatModificationModel statModificationModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         StatModificationModel statModificationModelCopy = new StatModificationModel(statModificationModel);
         StatModificationViewModel statModificationViewModel = new StatModificationViewModel(statModificationModelCopy);
         statModificationViewModel.InitializeOptions();
         CreateStatModificationView createStatModificationView = new CreateStatModificationView(statModificationViewModel);

         modalDialog.WindowTitle = title;
         modalDialog.Body = createStatModificationView;
         modalDialog.Confirmation = createStatModificationView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? statModificationModelCopy : null;
      }

      /// <summary>
      /// Shows armor class dialog
      /// </summary>
      public ArmorClassModel ShowArmorClassDialog(ArmorClassModel armorClassModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         ArmorClassModel armorClassModelCopy = new ArmorClassModel(armorClassModel);
         ArmorClassViewModel armorClassViewModel = new ArmorClassViewModel(armorClassModelCopy);
         armorClassViewModel.InitializeOptions();
         ArmorClassView armorClassView = new ArmorClassView(armorClassViewModel);

         modalDialog.WindowTitle = "Armor Class";
         modalDialog.Body = armorClassView;
         modalDialog.Confirmation = armorClassView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? armorClassModelCopy : null;
      }

      /// <summary>
      /// Shows movement dialog
      /// </summary>
      public MovementModel ShowMovementDialog(MovementModel movementModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         MovementModel movementModelCopy = new MovementModel(movementModel);
         MovementViewModel movementViewModel = new MovementViewModel(movementModelCopy);
         MovementView movementView = new MovementView(movementViewModel);

         modalDialog.WindowTitle = "Movement";
         modalDialog.Body = movementView;
         modalDialog.Confirmation = movementView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? movementModelCopy : null;
      }

      /// <summary>
      /// Shows short rest dialog
      /// </summary>
      public List<LevelModel> ShowShortRestDialog(List<LevelModel> levels, int conMod)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         List<LevelModel> levelsCopy = new List<LevelModel>();
         foreach (LevelModel level in levels)
         {
            levelsCopy.Add(new LevelModel(level));
         }

         ShortRestViewModel shortRestViewModel = new ShortRestViewModel(levelsCopy, conMod);
         ShortRestView shortRestView = new ShortRestView(shortRestViewModel);

         modalDialog.WindowTitle = "Short Rest";
         modalDialog.Body = shortRestView;
         modalDialog.Confirmation = shortRestView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? levelsCopy : null;
      }

      /// <summary>
      /// Shows convert money dialog
      /// </summary>
      public BagModel ShowConvertMoneyDialog(BagModel bagModel)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         BagModel bagModelCopy = new BagModel(bagModel);
         ConvertMoneyViewModel convertMoneyViewModel = new ConvertMoneyViewModel(bagModelCopy);
         ConvertMoneyView convertMoneyView = new ConvertMoneyView(convertMoneyViewModel);

         modalDialog.WindowTitle = "Convert Money";
         modalDialog.Body = convertMoneyView;
         modalDialog.Confirmation = convertMoneyView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? bagModelCopy : null;
      }

      /// <summary>
      /// Shows level up dialog
      /// </summary>
      public LevelModel ShowLevelUpDialog(Dictionary<KeyValuePair<Guid, string>, int> classesMap, int level)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         LevelUpViewModel levelUpViewModel = new LevelUpViewModel(classesMap, level);
         LevelUpView levelUpView = new LevelUpView(levelUpViewModel);

         modalDialog.WindowTitle = "Level Up";
         modalDialog.Body = levelUpView;
         modalDialog.Confirmation = levelUpView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? levelUpViewModel.Level.LevelModel : null;
      }

      /// <summary>
      /// Shows death saves dialog
      /// </summary>
      public (int?, int?) ShowDeathSavesDialog(int successes, int failures)
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         DeathSavesViewModel deathSavesViewModel = new DeathSavesViewModel(successes, failures);
         DeathSavesView deathSavesView = new DeathSavesView(deathSavesViewModel);

         modalDialog.WindowTitle = "Death Saving Throws";
         modalDialog.Body = deathSavesView;
         modalDialog.Confirmation = deathSavesView.ViewModel;

         bool? result = ShowDialog(modalDialog);

         return result == true ? ((int?)deathSavesViewModel.Successes, (int?)deathSavesViewModel.Failures) : (null, null);
      }

      public void ShowImportView()
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         ImportView importView = DependencyResolver.Resolve<ImportView>();

         modalDialog.WindowTitle = "Import";
         modalDialog.Body = importView;
         modalDialog.Confirmation = importView.ViewModel;

         ShowDialog(modalDialog);
      }

      /// <summary>
      /// Shows patreon dialog
      /// </summary>
      public void ShowPatreonDialog()
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         PatreonView patreonView = DependencyResolver.Resolve<PatreonView>();

         modalDialog.WindowTitle = "Patreon";
         modalDialog.Body = patreonView;

         ShowDialog(modalDialog);
      }

      /// <summary>
      /// Shows about dialog
      /// </summary>
      public void ShowAboutDialog()
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         AboutView aboutView = DependencyResolver.Resolve<AboutView>();

         modalDialog.WindowTitle = "About Crit Compendium";
         modalDialog.Body = aboutView;

         ShowDialog(modalDialog);
      }

      /// <summary>
      /// Shows settings window
      /// </summary>
      public void ShowSettingsWindow()
      {
         ModalDialog modalDialog = new ModalDialog();

         if (_parentWindow != null)
         {
            modalDialog.Owner = _parentWindow;
         }

         SettingsView settingsView = DependencyResolver.Resolve<SettingsView>();

         modalDialog.WindowTitle = "Appearance Settings";
         modalDialog.Body = settingsView;
         modalDialog.Confirmation = settingsView.ViewModel;

         ShowDialog(modalDialog);
      }

      #endregion

      #region Non-Public Methods

      private bool? ShowDialog(ModalDialog dialog)
      {
         DialogOpened?.Invoke(this, EventArgs.Empty);
         dialog.ShowDialog();
         DialogClosed?.Invoke(this, EventArgs.Empty);

         return dialog.Result;
      }

      private bool? Show(ModalDialog dialog)
      {
         DialogOpened?.Invoke(this, EventArgs.Empty);
         dialog.Show();
         DialogClosed?.Invoke(this, EventArgs.Empty);

         return dialog.Result;
      }

      #endregion
   }
}
