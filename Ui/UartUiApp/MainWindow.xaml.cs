
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.Windows.Media;

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
            MainFrame.Navigate(new Page1());
            Button1ShadowOn();
            Button2ShadowOff();
        }

        private void GoToPage1_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page1());

            Button1ShadowOn();
            Button2ShadowOff();
        }

        private void GoToPage2_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page2());
            Button2ShadowOn();
            Button1ShadowOff();
        }

        private void Button1ShadowOn()
        {
            Button1.Effect = new DropShadowEffect
            {
                Color = Colors.Black,
                BlurRadius = 10,
                ShadowDepth = 5,
                Opacity = 1
            };
        }

        private void Button2ShadowOn()
        {
            Button2.Effect = new DropShadowEffect
            {
                Color = Colors.Black,
                BlurRadius = 10,
                ShadowDepth = 5,
                Opacity = 0.5
            };
        }
        private void Button1ShadowOff ()
        {
            Button1.Effect = null;
        }

        private void Button2ShadowOff()
        {
            Button2.Effect = null;
        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}

