using System.Windows;

using SaperLibrary;
using System.Windows.Input;

namespace Saper
{
    public partial class MainWindow : Window
    {
        public ViewModel ViewModel { get; }

        public MainWindow()
        {
            ViewModel = new ViewModel(30, 16, 99); //(cols, rows, bombs)

            InitializeComponent();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var clickPoint = e.GetPosition((IInputElement)sender);
            int x = (int)(clickPoint.X / ViewModel.CellSize.Width);
            int y = (int)(clickPoint.Y / ViewModel.CellSize.Height);
            ViewModel.OpenCell(x, y);
        }
    }
}
