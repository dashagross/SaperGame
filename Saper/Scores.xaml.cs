using System;
using System.Collections.Generic;
using System.Windows;

namespace Saper
{
    public partial class Scores : Window
    {
        public TimeSpan Elapsed { get; }

        public List<ScoreItem> BestScores { get; }

        public Scores(TimeSpan elapsed, List<ScoreItem> bestScores)
        {
            Elapsed = elapsed;
            BestScores = bestScores;
            InitializeComponent();
        }

        private void OK_Button(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
