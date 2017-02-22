using System.Windows;

using SaperLibrary;
using System.Windows.Input;
using System.Windows.Controls;

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

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var clickPoint = e.GetPosition((IInputElement)sender);
            int x = (int)(clickPoint.X / ViewModel.CellSize.Width);
            int y = (int)(clickPoint.Y / ViewModel.CellSize.Height);

            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    if (!ViewModel.Field[x, y].IsFlagged)
                    {
                        ViewModel.OpenCell(x, y);
                        ViewModel.OpenCellsNearEmpty(x, y);
                    }
                    break;

                case MouseButton.Right:
                    ViewModel.FlagCell(x, y);
                    break;

                case MouseButton.Middle:

                default:
                    break;
            }

        }
    }
}
