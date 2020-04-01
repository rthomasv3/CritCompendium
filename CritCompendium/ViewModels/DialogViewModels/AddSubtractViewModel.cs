using System;
using System.Windows.Input;

namespace CritCompendium.ViewModels.DialogViewModels
{
   public sealed class AddSubtractViewModel : IConfirmation
   {
      #region Events

      public event EventHandler AcceptSelected;
      public event EventHandler RejectSelected;
      public event EventHandler CancelSelected;

      #endregion

      #region Fields

      private readonly int _initialValue;
      private string _amount;
      private int? _result;

      private readonly ICommand _rejectCommand;
      private readonly ICommand _addCommand;
      private readonly ICommand _subtractCommand;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of <see cref="AddSubtractViewModel"/>
      /// </summary>
      public AddSubtractViewModel(int initialValue)
      {
         _initialValue = initialValue;

         _rejectCommand = new RelayCommand(obj => true, obj => Reject());
         _addCommand = new RelayCommand(obj => true, obj => Add());
         _subtractCommand = new RelayCommand(obj => true, obj => Subtract());
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets initial value
      /// </summary>
      public int InitialValue
      {
         get { return _initialValue; }
      }

      /// <summary>
      /// Gets or sets amount
      /// </summary>
      public string Amount
      {
         get { return _amount; }
         set { _amount = value; }
      }

      /// <summary>
      /// Gets result
      /// </summary>
      public int? Result
      {
         get { return _result; }
      }

      /// <summary>
      /// Gets reject command
      /// </summary>
      public ICommand RejectCommand
      {
         get { return _rejectCommand; }
      }

      /// <summary>
      /// Gets add command
      /// </summary>
      public ICommand AddCommand
      {
         get { return _addCommand; }
      }

      /// <summary>
      /// Gets subtract command
      /// </summary>
      public ICommand SubtractCommand
      {
         get { return _subtractCommand; }
      }

      #endregion

      #region Private Methods

      private void Reject()
      {
         CancelSelected?.Invoke(this, EventArgs.Empty);
      }

      private void Add()
      {
         if (Int32.TryParse(_amount, out int amount))
         {
            _result = _initialValue + amount;
         }

         AcceptSelected?.Invoke(this, EventArgs.Empty);
      }

      private void Subtract()
      {
         if (Int32.TryParse(_amount, out int amount))
         {
            _result = _initialValue - amount;
         }

         AcceptSelected?.Invoke(this, EventArgs.Empty);
      }

      #endregion
   }
}
