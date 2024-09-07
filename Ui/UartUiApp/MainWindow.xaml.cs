
using System.Windows;

namespace UartUiApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Page1 page1 = new Page1();
            this.Content = page1;  
        }
    }
}

