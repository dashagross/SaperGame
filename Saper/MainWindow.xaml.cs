using System;
using System.Windows;

using System.Windows.Input;

namespace Saper
{
    public partial class MainWindow : Window
    {
        public ViewModel ViewModel { get; }

        public MainWindow()
        {
            ViewModel = new ViewModel();

            InitializeComponent();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var clickPoint = e.GetPosition((IInputElement)sender);
            int x = (int)(clickPoint.X / ViewModel.CellSize.Width);
            int y = (int)(clickPoint.Y / ViewModel.CellSize.Height);

            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    ViewModel.OpenCell(x, y);                    
                    break;

                case MouseButton.Right:
                    ViewModel.ToggleFlag(x, y);
                    break;

                case MouseButton.Middle:
                    ViewModel.OpenCellArea(x, y);
                    break;

                default:
                    break;
            }

        }

        private void New_Game(object sender, RoutedEventArgs e)
        {
            ViewModel.Start();
        }

        private void Settings_Button(object sender, RoutedEventArgs e)
        {
            var settings = new Settings(ViewModel.Difficulty);
            bool? result = settings.ShowDialog();
            if (result.HasValue && result.Value)
            {
                ViewModel.Difficulty = settings.Difficulty;
            }               
        }
    }
}
