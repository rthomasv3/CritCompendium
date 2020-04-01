using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using CritCompendium.Business;
using CritCompendiumInfrastructure.Business;

namespace CritCompendium.ViewModels.DialogViewModels
{
   public sealed class CalculatorViewModel : NotifyPropertyChanged
   {
      #region Events

      public event EventHandler Close;

      #endregion

      #region Fields

      private readonly DiceService _diceService;
      private readonly DialogService _dialogService;
      private readonly BackgroundWorker _rollWorker;
      private readonly RelayCommand _addTextCommand;
      private readonly RelayCommand _calculateCommand;
      private readonly RelayCommand _rollCommand;
      private readonly RelayCommand _deleteCommand;
      private readonly RelayCommand _clearCommand;
      private bool _closeOnCalculate;
      private bool _accepted;
      private string _expressionString;
      private int _result = 0;
      private string _resultExpressionString = "0";
      private string _averageString = "0";
      private string _halfString = "0";
      private string _minString = "0";
      private string _maxString = "0";

      #endregion

      #region Constructor

      /// <summary>
      /// Creats an instance of <see cref="CalculatorViewModel"/>
      /// </summary>
      public CalculatorViewModel(DiceService diceService, DialogService dialogService)
      {
         _diceService = diceService;
         _dialogService = dialogService;

         _rollWorker = new BackgroundWorker();
         _rollWorker.DoWork += _rollWorker_DoWork;
         _rollWorker.RunWorkerCompleted += _rollWorker_RunWorkerCompleted;

         _addTextCommand = new RelayCommand(obj => true, obj => AddText(obj as string));
         _calculateCommand = new RelayCommand(obj => true, obj => Calculate());
         _rollCommand = new RelayCommand(obj => true, obj => Roll());
         _deleteCommand = new RelayCommand(obj => true, obj => Delete());
         _clearCommand = new RelayCommand(obj => true, obj => Clear());
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets close on calculate
      /// </summary>
      public bool CloseOnCalculate
      {
         get { return _closeOnCalculate; }
         set { _closeOnCalculate = value; }
      }

      /// <summary>
      /// Gets accepted
      /// </summary>
      public bool Accepted
      {
         get { return _accepted; }
      }

      /// <summary>
      /// Gets or sets expression string
      /// </summary>
      public string ExpressionString
      {
         get { return _expressionString; }
         set { _expressionString = value; }
      }

      /// <summary>
      /// Gets the result
      /// </summary>
      public int Result
      {
         get { return _result; }
      }

      /// <summary>
      /// Gets the result expression
      /// </summary>
      public string ResultExpression
      {
         get { return _resultExpressionString; }
      }

      /// <summary>
      /// Gets half
      /// </summary>
      public string Half
      {
         get { return _halfString; }
      }

      /// <summary>
      /// Gets average
      /// </summary>
      public string Average
      {
         get { return _averageString; }
      }

      /// <summary>
      /// Gets min
      /// </summary>
      public string Min
      {
         get { return _minString; }
      }

      /// <summary>
      /// Gets max
      /// </summary>
      public string Max
      {
         get { return _maxString; }
      }

      /// <summary>
      /// Gets add text command
      /// </summary>
      public ICommand AddTextCommand
      {
         get { return _addTextCommand; }
      }

      /// <summary>
      /// Gets calculate command
      /// </summary>
      public ICommand CalculateCommand
      {
         get { return _calculateCommand; }
      }

      /// <summary>
      /// Gets roll command
      /// </summary>
      public ICommand RollCommand
      {
         get { return _rollCommand; }
      }

      /// <summary>
      /// Gets delete command
      /// </summary>
      public ICommand DeleteCommand
      {
         get { return _deleteCommand; }
      }

      /// <summary>
      /// Gets clear command
      /// </summary>
      public ICommand ClearCommand
      {
         get { return _clearCommand; }
      }

      #endregion

      #region Private Methods

      private void AddText(string text)
      {
         _expressionString += text;

         OnPropertyChanged(nameof(ExpressionString));
      }

      private void Calculate()
      {
         (double, string) mainDiceResult = _diceService.EvaluateExpression(_expressionString);
         _result = (int)mainDiceResult.Item1;

         if (_closeOnCalculate)
         {
            _accepted = true;
            Close?.Invoke(this, EventArgs.Empty);
         }
      }

      private void Roll()
      {
         if (!_rollWorker.IsBusy)
         {
            _rollWorker.RunWorkerAsync();
         }
      }

      private void Delete()
      {
         if (!String.IsNullOrWhiteSpace(_expressionString))
         {
            _expressionString = _expressionString.Remove(_expressionString.Length - 1);
            OnPropertyChanged(nameof(ExpressionString));
         }
      }

      private void Clear()
      {
         _expressionString = String.Empty;
         _result = 0;
         _resultExpressionString = "0";
         _halfString = "0";
         _averageString = "0";
         _minString = "0";
         _maxString = "0";

         OnPropertyChanged(nameof(ExpressionString));
         OnPropertyChanged(nameof(Result));
         OnPropertyChanged(nameof(ResultExpression));
         OnPropertyChanged(nameof(Half));
         OnPropertyChanged(nameof(Average));
         OnPropertyChanged(nameof(Min));
         OnPropertyChanged(nameof(Max));
      }

      private void _rollWorker_DoWork(object sender, DoWorkEventArgs e)
      {
         if (!String.IsNullOrWhiteSpace(_expressionString))
         {
            (double, double, double) avgMinMax = _diceService.EvaluateExpressionAvgMinMax(_expressionString);
            _averageString = ((int)avgMinMax.Item1).ToString();
            _minString = ((int)avgMinMax.Item2).ToString();
            _maxString = ((int)avgMinMax.Item3).ToString();

            OnPropertyChanged(nameof(Average));
            OnPropertyChanged(nameof(Min));
            OnPropertyChanged(nameof(Max));

            int rolls = 0;
            int maxRolls = _expressionString.ToLower().Contains("d") ? 10 : 1;
            while (rolls++ < maxRolls)
            {
               (double, string) mainDiceResult = _diceService.EvaluateExpression(_expressionString);
               _result = (int)mainDiceResult.Item1;
               _resultExpressionString = mainDiceResult.Item2;

               if (_result > 0)
               {
                  _halfString = ((int)Math.Max(1, ((int)(_result / 2)))).ToString();
               }
               else
               {
                  _halfString = "0";
               }

               OnPropertyChanged(nameof(Result));
               OnPropertyChanged(nameof(ResultExpression));
               OnPropertyChanged(nameof(Half));
               Thread.Sleep(100);
            }
         }
      }

      private void _rollWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         if (e.Error != null)
         {
            _dialogService.ShowConfirmationDialog("Dice Roll Error", $"{e.Error.Message}\n\n{e.Error.StackTrace}", "OK", null, null);
         }
      }

      #endregion
   }
}
