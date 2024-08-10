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
    }
    public enum Direction
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop
    }

    public  class LinePositionConverter : IValueConverter
    {
        // When you define a property with get and set accessors,
        // the backing field is what holds the value that the property retrieves or modifies
        // backing field: a private variable that stores the actual data for a property in a class
        public Direction Direction { get; set; }

        // value: represented by 'path'
        // targetType: type of the binding target property
        //             it indicates what type of data the converter should return
        // parameter: corresponds to the ConverterParameter which in this case is 'Line' element
        // object: the base type for all types in C#
        // Boxing: When a value type like double is returned from a method with an object return type,
        //         it undergoes boxing, which means it is wrapped in an object box.
        //         This allows it to be stored in the object type.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double position = 0;
            double offset;

            // Handle cases where the value is a double (e.g., ActualWidth or ActualHeight)
            if (value is double actualValue)
            {
                switch (Direction)
                {
                    case Direction.LeftToRight:
                        position = 0; // Start at the left (X1)
                        break;
                    case Direction.RightToLeft:
                        position = actualValue; // End at the right (X2)
                        break;
                    case Direction.TopToBottom:
                        position = 0; // Start at the top (Y1)
                        break;
                    case Direction.BottomToTop:
                        position = actualValue; // End at the bottom (Y2)
                        break;
                }

                // If a parameter is passed, it is used to adjust the position, typically for Y-axis offsets
                if (parameter != null && double.TryParse(parameter.ToString(), out offset))
                {
                    position += offset;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}