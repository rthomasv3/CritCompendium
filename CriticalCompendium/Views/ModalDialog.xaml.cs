using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CritCompendium.ViewModels.DialogViewModels;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for ModalDialog.xaml
    /// </summary>
    public partial class ModalDialog : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private string _title;
		private FrameworkElement _body;
		private IConfirmation _confirmation;
		private bool? _result = null;
        private bool _tabPressed;

        public ModalDialog()
        {
            InitializeComponent();

            DataContext = this;

            PreviewKeyDown += View_PreviewKeyDown;
            PreviewGotKeyboardFocus += View_PreviewGotKeyboardFocus;
        }

		public string WindowTitle
		{
			get { return _title; }
			set
			{
				_title = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowTitle)));
			}
		}

		public FrameworkElement Body
		{
			get { return _body; }
			set
			{
				_body = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Body)));
			}
		}

		public bool? Result
		{
			get { return _result; }
		}

		public IConfirmation Confirmation
		{
			get { return _confirmation; }
			set
			{
				SetConfirmation(value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Confirmation)));
			}
		}

		private void SetConfirmation(IConfirmation confirmation)
		{
			if (_confirmation != null)
			{
				_confirmation.AcceptSelected -= Confirmation_AcceptSelected;
				_confirmation.RejectSelected -= Confirmation_RejectSelected;
				_confirmation.CancelSelected -= Confirmation_CancelSelected;
			}

			_confirmation = confirmation;
			_confirmation.AcceptSelected += Confirmation_AcceptSelected;
			_confirmation.RejectSelected += Confirmation_RejectSelected;
			_confirmation.CancelSelected += Confirmation_CancelSelected;
		}

		private void Confirmation_AcceptSelected(object sender, EventArgs e)
		{
			_result = true;
			Close();
		}

		private void Confirmation_RejectSelected(object sender, EventArgs e)
		{
			_result = false;
			Close();
		}

		private void Confirmation_CancelSelected(object sender, EventArgs e)
		{
			_result = null;
			Close();
		}

		private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}

		private void _closeButton_Click(object sender, RoutedEventArgs e)
		{
			_result = null;
			Close();
		}

        private void View_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            _tabPressed = e.Key == Key.Tab;
        }

        private void View_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (_tabPressed)
            {
                if (e.NewFocus is TextBox textBox)
                {
                    textBox.SelectAll();
                }
                _tabPressed = false;
            }
        }
    }
}
