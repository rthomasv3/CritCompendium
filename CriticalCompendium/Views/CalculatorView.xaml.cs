using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CritCompendium.ViewModels.DialogViewModels;
using CriticalCompendiumInfrastructure;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for CalculatorView.xaml
    /// </summary>
    public partial class CalculatorView : UserControl
    {
        private readonly CalculatorViewModel _viewModel = DependencyResolver.Resolve<CalculatorViewModel>();

        public CalculatorView(string expression, bool closeOnCalculate)
        {
            InitializeComponent();

            DataContext = _viewModel;

            _viewModel.ExpressionString = expression;
            _viewModel.CloseOnCalculate = closeOnCalculate;
            
            _viewModel.Close += _viewModel_Close;

            if (!String.IsNullOrWhiteSpace(expression) && !closeOnCalculate)
            {
                Loaded += CalculatorView_LoadedRoll;
            }
            else
            {
                Loaded += CalculatorView_LoadedFocus;
            }
        }

        private void CalculatorView_LoadedRoll(object sender, RoutedEventArgs e)
        {
            Roll();
        }

        private void CalculatorView_LoadedFocus(object sender, RoutedEventArgs e)
        {
            _expressionTextBox.Focus();
            _expressionTextBox.CaretIndex = _expressionTextBox.Text.Length;
        }

        public CalculatorViewModel ViewModel
        {
            get { return _viewModel; }
        }

        private void _viewModel_Close(object sender, EventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                Roll();
            }
        }

        private void Roll()
        {
            _viewModel.RollCommand.Execute(null);

            RoutedEventArgs routedEventArgs = new RoutedEventArgs(Button.ClickEvent);
            _rollButton.RaiseEvent(routedEventArgs);
        }
    }
}
