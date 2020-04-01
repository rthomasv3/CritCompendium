using System;
using System.Windows.Input;
using CritCompendium.ViewModels.DialogViewModels;

namespace CritCompendium.ViewModels.ObjectViewModels
{
   public class ObjectViewModel : NotifyPropertyChanged, IConfirmation, ICopyInformation
   {
      #region Events

      public event EventHandler AcceptSelected;
      public event EventHandler RejectSelected;
      public event EventHandler CancelSelected;

      #endregion

      #region Fields

      protected readonly ICommand _acceptCommand;
      protected readonly ICommand _rejectCommand;
      protected readonly ICommand _copyInformationCommand;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates an instance of <see cref="ObjectViewModel"/>
      /// </summary>
      public ObjectViewModel()
      {
         _acceptCommand = new RelayCommand(obj => true, obj => OnAccept());
         _rejectCommand = new RelayCommand(obj => true, obj => OnReject());
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets accept command
      /// </summary>
      public ICommand AcceptCommand
      {
         get { return _acceptCommand; }
      }

      /// <summary>
      /// Gets reject command
      /// </summary>
      public ICommand RejectCommand
      {
         get { return _rejectCommand; }
      }

      /// <summary>
      /// Gets copy information command
      /// </summary>
      public ICommand CopyInformationCommand
      {
         get { return _copyInformationCommand; }
      }

      #endregion

      #region Protected Methods

      protected void Accept()
      {
         AcceptSelected?.Invoke(this, EventArgs.Empty);
      }

      protected virtual void OnAccept()
      {
         AcceptSelected?.Invoke(this, EventArgs.Empty);
      }

      protected virtual void OnReject()
      {
         RejectSelected?.Invoke(this, EventArgs.Empty);
      }

      #endregion
   }
}
