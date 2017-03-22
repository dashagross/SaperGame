using System;
using System.Windows;

namespace Saper
{
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
