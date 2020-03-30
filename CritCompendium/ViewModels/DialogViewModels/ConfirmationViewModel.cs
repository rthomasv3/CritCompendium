using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CritCompendium.ViewModels.DialogViewModels
{
	public sealed class ConfirmationViewModel : NotifyPropertyChanged, IConfirmation
	{
		#region Events
		
		public event EventHandler AcceptSelected;
		public event EventHandler RejectSelected;
		public event EventHandler CancelSelected;

		#endregion

		#region Fields
		
		private readonly string _body;
		private readonly string _accept;
		private readonly string _reject;
		private readonly string _cancel;

		private readonly ICommand _acceptCommand;
		private readonly ICommand _rejectCommand;
		private readonly ICommand _cancelCommand;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance of <see cref="ConfirmationViewModel"/>
		/// </summary>
		public ConfirmationViewModel(string body, string accept, string reject, string cancel)
		{
			_body = body;
			_accept = accept;
			_reject = reject;
			_cancel = cancel;

			_acceptCommand = new RelayCommand(obj => true, obj => OnAccept());
			_rejectCommand = new RelayCommand(obj => true, obj => OnReject());
			_cancelCommand = new RelayCommand(obj => true, obj => OnCancel());
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets body
		/// </summary>
		public string Body
		{
			get { return _body; }
		}

		/// <summary>
		/// Gets accept
		/// </summary>
		public string Accept
		{
			get { return _accept; }
		}

		/// <summary>
		/// Gets reject
		/// </summary>
		public string Reject
		{
			get { return _reject; }
		}

		/// <summary>
		/// Gets cancel
		/// </summary>
		public string Cancel
		{
			get { return _cancel; }
		}

		/// <summary>
		/// True if accept should be visible
		/// </summary>
		public bool AcceptVisible
		{
			get { return _accept != null; }
		}

		/// <summary>
		/// True if reject should be visible
		/// </summary>
		public bool RejectVisible
		{
			get { return _reject != null; }
		}

		/// <summary>
		/// True if cancel should be visible
		/// </summary>
		public bool CancelVisible
		{
			get { return _cancel != null; }
		}

		/// <summary>
		/// Gets acceptCommand
		/// </summary>
		public ICommand AcceptCommand
		{
			get { return _acceptCommand; }
		}

		/// <summary>
		/// Gets rejectCommand
		/// </summary>
		public ICommand RejectCommand
		{
			get { return _rejectCommand; }
		}

		/// <summary>
		/// Gets cancelCommand
		/// </summary>
		public ICommand CancelCommand
		{
			get { return _cancelCommand; }
		}

		#endregion

		#region Non-Public Methods

		private void OnAccept()
		{
			AcceptSelected?.Invoke(this, EventArgs.Empty);
		}

		private void OnReject()
		{
			RejectSelected?.Invoke(this, EventArgs.Empty);
		}

		private void OnCancel()
		{
			CancelSelected?.Invoke(this, EventArgs.Empty);
		}

		#endregion
	}
}
