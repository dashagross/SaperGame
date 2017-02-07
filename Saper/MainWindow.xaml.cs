using System.Windows;

using SaperLibrary;


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
    }
}
