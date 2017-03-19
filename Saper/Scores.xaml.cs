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
using System.Windows.Shapes;

namespace Saper
{
    /// <summary>
    /// Interaction logic for Scores.xaml
    /// </summary>
    public partial class Scores : Window
    {
        public TimeSpan Elapsed { get; }

        public Scores(TimeSpan ts)
        {
            Elapsed = ts;
            InitializeComponent();
        }

        private void OK_Button(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
