﻿using System;
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
using CritCompendium.ViewModels.ObjectViewModels;

namespace CritCompendium.Views
{
    /// <summary>
    /// Interaction logic for ArmorClassView.xaml
    /// </summary>
    public partial class ArmorClassView : UserControl
    {
        /// <summary>
        /// Creates an instance of <see cref="ArmorClassView"/>
        /// </summary>
        public ArmorClassView(ArmorClassViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        /// <summary>
        /// Gets view model
        /// </summary>
        public ArmorClassViewModel ViewModel
        {
            get { return DataContext as ArmorClassViewModel; }
        }
    }
}
