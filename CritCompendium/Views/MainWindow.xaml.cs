using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using CritCompendium.Services;
using CritCompendium.ViewModels;
using CritCompendiumInfrastructure;

namespace CritCompendium.Views
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      private readonly DialogService _dialogService = DependencyResolver.Resolve<DialogService>();
      private readonly MainWindowViewModel _viewModel = DependencyResolver.Resolve<MainWindowViewModel>();
      private bool _resizeInProcess = false;
      private bool _tabPressed;

      public MainWindow()
      {
         InitializeComponent();

         DataContext = _viewModel;

         MaxHeight = SystemParameters.WorkArea.Height + 8;
         MaxWidth = SystemParameters.WorkArea.Width + 8;

         StateChanged += MainWindow_StateChanged;

         Loaded += MainWindow_Loaded;

         PreviewKeyDown += View_PreviewKeyDown;
         PreviewGotKeyboardFocus += View_PreviewGotKeyboardFocus;
      }

      private void MainWindow_Loaded(object sender, RoutedEventArgs e)
      {
         _dialogService.SetOwner(this);

         DataManager dataManager = DependencyResolver.Resolve<DataManager>();

         if (dataManager.FirstLaunch)
         {
            _viewModel.ShowOnboardingCommand.Execute(null);
         }
      }

      private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
      {
         if (e.ClickCount == 2)
         {
            if (WindowState == WindowState.Maximized)
            {
               _viewModel.RestoreMainWindow();
            }
            else
            {
               _viewModel.MaximizeMainWindow();
            }
         }
         else if (e.LeftButton == MouseButtonState.Pressed)
         {
            Application.Current.MainWindow.DragMove();
         }
      }

      private void Grid_MouseMove(object sender, MouseEventArgs e)
      {
         if (e.LeftButton == MouseButtonState.Pressed &&
             WindowState == WindowState.Maximized)
         {
            double startingLeft = PointToScreen(new Point(0, 0)).X;

            Point mousePosition = e.GetPosition(this);

            Top = mousePosition.Y - PointToScreen(mousePosition).Y - 8;

            double w = mousePosition.X / Width;

            _viewModel.RestoreMainWindow();

            Left = startingLeft + mousePosition.X - (Width * w);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
               Application.Current.MainWindow.DragMove();
            }
         }
      }

      private void Resize_Init(object sender, MouseButtonEventArgs e)
      {
         Rectangle senderRect = sender as Rectangle;
         if (senderRect != null)
         {
            _resizeInProcess = true;
            senderRect.CaptureMouse();
         }
      }

      private void Resize_End(object sender, MouseButtonEventArgs e)
      {
         Rectangle senderRect = sender as Rectangle;
         if (senderRect != null)
         {
            _resizeInProcess = false; ;
            senderRect.ReleaseMouseCapture();
         }
      }

      private void Resizing_Window(object sender, MouseEventArgs e)
      {
         if (_resizeInProcess)
         {
            Rectangle senderRect = sender as Rectangle;
            Window mainWindow = this;
            if (senderRect != null)
            {
               double width = e.GetPosition(mainWindow).X;
               double height = e.GetPosition(mainWindow).Y;
               senderRect.CaptureMouse();
               if (senderRect.Name.ToLower().Contains("right"))
               {
                  width += 1;
                  if (width > 0)
                     mainWindow.Width = width;
               }
               if (senderRect.Name.ToLower().Contains("left"))
               {
                  width -= 1;
                  mainWindow.Left += width;
                  width = mainWindow.Width - width;
                  if (width > 0)
                  {
                     mainWindow.Width = width;
                  }
               }
               if (senderRect.Name.ToLower().Contains("bottom"))
               {
                  height += 1;
                  if (height > 0)
                     mainWindow.Height = height;
               }
               if (senderRect.Name.ToLower().Contains("top"))
               {
                  height -= 1;
                  mainWindow.Top += height;
                  height = mainWindow.Height - height;
                  if (height > 0)
                  {
                     mainWindow.Height = height;
                  }
               }
            }
         }
      }

      private void MainWindow_StateChanged(object sender, System.EventArgs e)
      {
         if (WindowState == WindowState.Maximized)
         {
            _viewModel.MaximizeMainWindow();
         }
         else if (WindowState == WindowState.Normal)
         {
            _viewModel.RestoreMainWindow();
         }
      }

      private void Grid_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
      {
         e.Handled = true;
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
