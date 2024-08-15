using System;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
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
                // port: parameter used within the lambda expression trepresenting 'ports'
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
            comboBoxPorts2.ItemsSource = filteredPorts;
        }

        /*private void Line_Loaded(object sender, RoutedEventArgs e)
         {
             Point positionOfRts1Pin = rts1Pin.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
             Point positionOfRts2Pin = rts2Pin.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));

             connectingLine.X1 = positionOfRts1Pin.X + rts1Pin.ActualWidth / 2;
             connectingLine.Y1 = positionOfRts1Pin.Y + rts1Pin.ActualHeight / 2;

             connectingLine.X2 = positionOfRts2Pin.X + rts2Pin.ActualWidth / 2;
             connectingLine.Y2 = positionOfRts2Pin.Y + rts2Pin.ActualHeight / 2;
         }*/
        private void ComboBoxPorts_SelectionChanged2(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            string selectedPort2 = e.AddedItems[0] as string;

            string[] filteredPorts = SerialPort.GetPortNames().Where(port => port != selectedPort2).ToArray();
            comboBoxPorts1.ItemsSource = filteredPorts;
        }

        private void OnLineLoaded(object sender, RoutedEventArgs e)
        {
            var line = sender as Line;
            BindingOperations.GetBindingExpression(line, Line.X1Property)?.UpdateTarget();
            BindingOperations.GetBindingExpression(line, Line.Y1Property)?.UpdateTarget();
            BindingOperations.GetBindingExpression(line, Line.X2Property)?.UpdateTarget();
            BindingOperations.GetBindingExpression(line, Line.Y2Property)?.UpdateTarget();
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