using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using CritCompendiumInfrastructure.Business;

namespace CritCompendium.ViewModels.DialogViewModels
{
   public sealed class DiceRollViewModel : NotifyPropertyChanged
   {
      #region Fields

      private readonly DiceService _diceService;
      private readonly BackgroundWorker _diceWorker;
      private readonly RelayCommand _rollCommand;
      private string _rollString;
      private string _rollResult;
      private string _rollResultExpression;
      private string _rollAdvantageResult;
      private string _rollAdvantageResultExpression;
      private string _rollDisadvantageResult;
      private string _rollDisadvantageResultExpression;
      private bool _advantageDisadvantageVisible;

      #endregion

      #region Constructor

      /// <summary>
      /// Creats an instance of <see cref="DiceRollViewModel"/>
      /// </summary>
      public DiceRollViewModel(DiceService diceService)
      {
         _diceService = diceService;

         _diceWorker = new BackgroundWorker();
         _diceWorker.DoWork += _diceWorker_DoWork;

         _rollCommand = new RelayCommand(obj => true, obj => Roll());
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets the roll string
      /// </summary>
      public string RollString
      {
         get { return _rollString; }
         set
         {
            if (Set(ref _rollString, value))
            {
               AdvantageDisadvantageVisible = _rollString.Contains("d20");
            }
         }
      }

      /// <summary>
      /// Gets the roll result
      /// </summary>
      public string RollResult
      {
         get { return _rollResult; }
      }

      /// <summary>
      /// Gets the roll result expression
      /// </summary>
      public string RollResultExpression
      {
         get { return _rollResultExpression; }
      }

      /// <summary>
      /// Gets the roll advantage result
      /// </summary>
      public string RollAdvantageResult
      {
         get { return _rollAdvantageResult; }
      }

      /// <summary>
      /// Gets the roll advantage result expression
      /// </summary>
      public string RollAdvantageResultExpression
      {
         get { return _rollAdvantageResultExpression; }
      }

      /// <summary>
      /// Gets the roll disadvantage result
      /// </summary>
      public string RollDisadvantageResult
      {
         get { return _rollDisadvantageResult; }
      }

      /// <summary>
      /// Gets the roll disadvantage result expression
      /// </summary>
      public string RollDisadvantageResultExpression
      {
         get { return _rollDisadvantageResultExpression; }
      }

      public bool AdvantageDisadvantageVisible
      {
         get { return _advantageDisadvantageVisible; }
         set { Set(ref _advantageDisadvantageVisible, value); }
      }

      /// <summary>
      /// Gets the roll command
      /// </summary>
      public ICommand RollCommand
      {
         get { return _rollCommand; }
      }

      #endregion

      #region Private Methods

      private void Roll()
      {
         if (!_diceWorker.IsBusy)
         {
            _diceWorker.RunWorkerAsync();
         }
      }

      private void _diceWorker_DoWork(object sender, DoWorkEventArgs e)
      {
         int rolls = 0;
         while (rolls++ < 10)
         {
            (double, string) mainDiceResult = _diceService.EvaluateExpression(_rollString);
            _rollResult = mainDiceResult.Item1.ToString();
            _rollResultExpression = mainDiceResult.Item2;

            if (_advantageDisadvantageVisible)
            {
               (double, string) secondDiceResult = _diceService.EvaluateExpression(_rollString);

               if (secondDiceResult.Item1 > mainDiceResult.Item1)
               {
                  _rollAdvantageResult = secondDiceResult.Item1.ToString();
                  _rollAdvantageResultExpression = secondDiceResult.Item2;
                  _rollDisadvantageResult = mainDiceResult.Item1.ToString();
                  _rollDisadvantageResultExpression = mainDiceResult.Item2;
               }
               else
               {
                  _rollAdvantageResult = mainDiceResult.Item1.ToString();
                  _rollAdvantageResultExpression = mainDiceResult.Item2;
                  _rollDisadvantageResult = secondDiceResult.Item1.ToString();
                  _rollDisadvantageResultExpression = secondDiceResult.Item2;
               }

               OnPropertyChanged(nameof(RollAdvantageResult));
               OnPropertyChanged(nameof(RollAdvantageResultExpression));
               OnPropertyChanged(nameof(RollDisadvantageResult));
               OnPropertyChanged(nameof(RollDisadvantageResultExpression));
            }

            OnPropertyChanged(nameof(RollResult));
            OnPropertyChanged(nameof(RollResultExpression));

            Thread.Sleep(100);
         }
      }

      #endregion
   }
}
