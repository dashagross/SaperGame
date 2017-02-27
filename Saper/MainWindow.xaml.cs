using System.Windows;

using System.Windows.Input;

namespace Saper
{
    public partial class MainWindow : Window
    {
        public ViewModel ViewModel { get; }

        public MainWindow()
        {
            ViewModel = new ViewModel(30, 16, 99); // (cols, rows, bombs)

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
                    ViewModel.Rules.OpenCell(x, y);                    
                    break;

                case MouseButton.Right:
                    ViewModel.Rules.ToggleFlag(x, y);
                    break;

                case MouseButton.Middle:
                    ViewModel.Rules.OpenCellArea(x, y);
                    break;

                default:
                    break;
            }

        }
    }
}
