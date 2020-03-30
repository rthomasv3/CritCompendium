using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CritCompendium
{
    /// <summary>
    /// Interaction logic for ProficiencyControl.xaml
    /// </summary>
    public partial class ProficiencyControl : UserControl
    {
        private int _value = 0;
        private Dictionary<int, string> _values = new Dictionary<int, string>() { { 0, "" }, { 1, "P" } };

        public ProficiencyControl()
        {
            DataContext = this;
            InitializeComponent();
        }

        public static readonly DependencyProperty ProficiencyProperty =
             DependencyProperty.RegisterAttached(
             "Proficiency",
             typeof(bool),
             typeof(ProficiencyControl),
             new UIPropertyMetadata(false, new PropertyChangedCallback(OnProficiencyPropertyChanged)));

        private static void OnProficiencyPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((ProficiencyControl)o).Proficiency = (bool)e.NewValue;
        }

        public bool Proficiency
        {
            get { return _value == 1; }
            set
            {
                _value = value ? 1 : _value;
                UpdateDisplayText();
            }
        }

        public static readonly DependencyProperty ExpertiseProperty =
             DependencyProperty.RegisterAttached(
             "Expertise",
             typeof(bool),
             typeof(ProficiencyControl),
             new UIPropertyMetadata(false, new PropertyChangedCallback(OnExpertisePropertyChanged)));

        private static void OnExpertisePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((ProficiencyControl)o).Expertise = (bool)e.NewValue;
        }

        public bool Expertise
        {
            get { return _value == 2; }
            set
            {
                _value = value ? 2 : _value;
                UpdateDisplayText();
            }
        }

        public static readonly DependencyProperty SupportsExpertiseProperty =
             DependencyProperty.RegisterAttached(
             "SupportsExpertise",
             typeof(bool),
             typeof(ProficiencyControl),
             new UIPropertyMetadata(false, new PropertyChangedCallback(OnSupportsExpertiseChanged)));
        
        private static void OnSupportsExpertiseChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((ProficiencyControl)o).SupportsExpertise = (bool)e.NewValue;
        }

        /// <summary>
        /// Gets or sets text
        /// </summary>
        public bool SupportsExpertise
        {
            get { return (bool)GetValue(SupportsExpertiseProperty); }
            set
            {
                SetValue(SupportsExpertiseProperty, value);

                if (SupportsExpertise)
                {
                    _values = new Dictionary<int, string>() { { 0, "" }, { 1, "P" }, { 2, "E" } };
                }
                else
                {
                    _values = new Dictionary<int, string>() { { 0, "" }, { 1, "P" } };
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _value = ++_value % _values.Count;
            UpdateDisplayText();
        }

        private void UpdateDisplayText()
        {
            _displayText.Text = _values[_value];
        }
    }
}
