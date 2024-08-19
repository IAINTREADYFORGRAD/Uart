using System;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfApp;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private Uart uart1;
        private Uart uart2;
        public MainWindow()
        {
            InitializeComponent();
            PopulateComboBoxWithPorts();
        }

        private void PopulateComboBoxWithPorts()
        {
            string[] ports = SerialPort.GetPortNames();
            string selectedPort1 = null;

            if (ports.Length > 0)
            {
                comboBoxPorts1.ItemsSource = ports;
                comboBoxPorts1.SelectedIndex = 0;

                selectedPort1 = ports[0];
            }

            if (ports.Length > 1)
            {
                //
                // port: parameter used within the lambda expression representing 'ports'
                //
                string[] filteredPorts = ports.Where(port => port != selectedPort1).ToArray();
                comboBoxPorts2.ItemsSource = filteredPorts;
                comboBoxPorts2.SelectedIndex = 0;
            }
        }

        private void ComboBoxPorts_SelectionChanged1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;

            }

            //
            // cast the selected item from the ComboBox to a string
            //
            string selectedPort1 = e.AddedItems[0] as string;

            string[] filteredPorts = SerialPort.GetPortNames().Where(port => port != selectedPort1).ToArray();
            comboBoxPorts1.ItemsSource = filteredPorts;

            if (selectedPort1 != null)
            {
                if (uart1 != null)
                {
                    uart1.UartDestroy();
                }

                uart1 = new Uart(selectedPort1, Handshake.None);
                uart1.UartStart();
            }
        }
        private void ComboBoxPorts_SelectionChanged2(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            string selectedPort2 = e.AddedItems[0] as string;

            string[] filteredPorts = SerialPort.GetPortNames().Where(port => port != selectedPort2).ToArray();
            comboBoxPorts2.ItemsSource = filteredPorts;

            if (selectedPort2 != null)
            {
                if (uart2 != null)
                {
                    uart2.UartDestroy();
                }

                uart2 = new Uart(selectedPort2, Handshake.None);
                uart2.UartStart();
            }
        }

        private void OnLineLoaded(object sender, RoutedEventArgs e)
        {
            var line = sender as Line;
            BindingOperations.GetBindingExpression(line, Line.X1Property)?.UpdateTarget();
            BindingOperations.GetBindingExpression(line, Line.Y1Property)?.UpdateTarget();
            BindingOperations.GetBindingExpression(line, Line.X2Property)?.UpdateTarget();
            BindingOperations.GetBindingExpression(line, Line.Y2Property)?.UpdateTarget();
        }

        private void Rts1Butt_Click(object sender, RoutedEventArgs e)
        {
            uart1.RtsEnable();
            rts1Butt.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99D9EA"));
            lineRts1Pin1Pin2.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99D9EA"));
        }

        private void Dtr1Butt_Click(object sender, RoutedEventArgs e)
        {
            uart1.DtrEnable();
            dtr1Butt.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99D9EA"));
            lineDtr1Pin1Pin2.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99D9EA"));
        }

        private void Rts2Butt_Click(object sender, RoutedEventArgs e)
        {
            uart2.RtsEnable();
            rts1Butt.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99D9EA"));
            lineRts2Pin1Pin2.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99D9EA"));
        }

        private void Dtr2Butt_Click(object sender, RoutedEventArgs e)
        {
            uart1.DtrEnable();
            dtr1Butt.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99D9EA"));
            lineDtr2Pin1Pin2.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99D9EA"));
        }

    }

    
    public class CoordinateGet : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UIElement element && parameter is string axis)
            {
                var window = Application.Current.MainWindow;
                if (VisualTreeHelper.GetParent(element) != null)
                {
                    try
                    {
                        var position = element.TransformToAncestor(window).Transform(new Point(0, 0));

                        if (axis == "X") return position.X + element.RenderSize.Width / 2;
                        else if (axis == "Y") return position.Y + element.RenderSize.Height / 2;

                    }
                    catch (InvalidOperationException)
                    {
                        return DependencyProperty.UnsetValue;
                    }
                }
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}