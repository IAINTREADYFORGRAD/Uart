using System;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Data;

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

        private void comboBoxPorts_SelectionChanged1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
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

        private void comboBoxPorts_SelectionChanged2(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            
            string selectedPort2 = e.AddedItems[0] as string;

            string[] filteredPorts = SerialPort.GetPortNames().Where(port => port != selectedPort2).ToArray();
            comboBoxPorts1.ItemsSource = filteredPorts;
        }
    }
    public class X1Converter : IValueConverter
    {
        public string Direction { get; set; } = "LeftToRight";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double actualSize)
            {
                // For X coordinates, we may adjust based on element's side (left or right)
                if (Direction == "LeftToRight")
                {
                    return 0; // Start from the left side of the element
                }
                else if (Direction == "RightToLeft")
                {
                    return actualSize; // Start from the right side of the element
                }

                // For Y coordinates, we align based on the item index (optional, if parameter is used)
                if (parameter is string indexStr && int.TryParse(indexStr, out int index))
                {
                    return (actualSize / 5) * (index + 1);
                }

                return actualSize / 2; // Default to center for Y coordinates
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}