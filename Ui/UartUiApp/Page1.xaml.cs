using System;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UartUiApp
{
    public partial class Page1 : Page
    {
        private Uart uart1 = null;
        private Uart uart2 = null;
        private bool rts1ButtClick = false;
        private bool dtr1ButtClick = false;
        private bool rts2ButtClick = false;
        private bool dtr2ButtClick = false;

        public Page1()
        {
            InitializeComponent();
            this.Loaded += Page1_Loaded;
        }
        private void Page1_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Page1_Loaded");
            comboBox1.SelectionChanged += ComboBoxSelectionChanged1;
            comboBox2.SelectionChanged += ComboBoxSelectionChanged2;
            comboBox1.DropDownOpened += ComboBoxDropDown1;
            comboBox2.DropDownOpened += ComboBoxDropDown2;
        }

        private void Uart1CtsHigh(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    cts1Switch.Text = "HIGH";
                    cts1Switch.Foreground = ColorOn();
                    cts1Pin1.Fill = ColorOn();
                    lineCts1Pin1Pin2.Stroke = ColorOn();
                    crossCtsRts.Stroke = ColorOn();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Uart1CtsHigh: " + ex.Message);
            }
        }

        private void Uart1CtsLow(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    cts1Switch.Text = "LOW";
                    cts1Switch.Foreground = ColorOff();
                    cts1Pin1.Fill = ColorOff();
                    lineCts1Pin1Pin2.Stroke = ColorOff();
                    crossCtsRts.Stroke = ColorOff();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Uart1CtsLow: " + ex.Message);
            }
        }

        private void Uart1DsrHigh(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    dsr1Switch.Text = "HIGH";
                    dsr1Switch.Foreground = ColorOn();
                    dsr1Pin1.Fill = ColorOn();
                    lineDsr1Pin1Pin2.Stroke = ColorOn();
                    crossDsrDtr.Stroke = ColorOn();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Uart1DsrHigh: " + ex.Message);
            }
        }

        private void Uart1DsrLow(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    dsr1Switch.Text = "LOW";
                    dsr1Switch.Foreground = ColorOff();
                    dsr1Pin1.Fill = ColorOff();
                    lineDsr1Pin1Pin2.Stroke = ColorOff();
                    crossDsrDtr.Stroke = ColorOff();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Uart1DsrLow: " + ex.Message);
            }
        }

        private void Uart1DcdHigh(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    dcd1Switch.Text = "HIGH";
                    dcd1Switch.Foreground = ColorOn();
                    dcd1Pin.Fill = ColorOn();
                    lineDcd1ToDcd2.Stroke = ColorOn();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Uart1DcdHigh: " + ex.Message);
            }
        }

        private void Uart1DcdLow(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    dcd1Switch.Text = "LOW";
                    dcd1Switch.Foreground = ColorOff();
                    dcd1Pin.Fill = ColorOff();
                    lineDcd1ToDcd2.Stroke = ColorOff();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Uart1DcdLow: " + ex.Message);
            }
        }

        private void ComboBoxDropDown1(object sender, EventArgs e)
        {
            try
            {
                string[] filteredPorts;

                {
                    if (uart2 == null)
                    {
                        filteredPorts = SerialPort.GetPortNames().ToArray();
                    }
                    else
                    {
                        filteredPorts = SerialPort.GetPortNames().Where(port => (port != uart2.Name)).ToArray();
                    }

                    comboBox1.ItemsSource = filteredPorts;
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in ComboBoxDropDown1: " + ex.Message);
            }
        }

        private void ComboBoxSelectionChanged1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                string selectedPort;

                if (uart1 != null)
                {
                    uart1.UartDestroy();
                    Debug.WriteLine("uart1 " + uart1.Name + " is closed");
                }

                if (e.AddedItems.Count == 0)
                {
                    return;
                }

                selectedPort = e.AddedItems[0] as string;

                if (selectedPort != null)
                {
                    comboBox1.Text = selectedPort;

                    uart1 = new Uart(selectedPort, 115200, DataDisplayBlock1);
                    uart1.UartStart();
                    uart1.ctsHigh += Uart1CtsHigh;
                    uart1.ctsLow += Uart1CtsLow;
                    uart1.dsrHigh += Uart1DsrHigh;
                    uart1.dsrLow += Uart1DsrLow;
                    uart1.cdHigh += Uart1DcdHigh;
                    uart1.cdLow += Uart1DcdLow;

                    Port1Init();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in ComboBoxSelectionChanged1: " + ex.Message);
            }
        }

        private void Uart2CtsHigh(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    cts2Switch.Text = "HIGH";
                    cts2Switch.Foreground = ColorOn();
                    cts2Pin1.Fill = ColorOn();
                    lineCts2Pin1Pin2.Stroke = ColorOn();
                    crossRtsCts.Stroke = ColorOn();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Uart2CtsHigh: " + ex.Message);
            }
        }

        private void Uart2CtsLow(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    cts2Switch.Text = "LOW";
                    cts2Switch.Foreground = ColorOff();
                    cts2Pin1.Fill = ColorOff();
                    lineCts2Pin1Pin2.Stroke = ColorOff();
                    crossRtsCts.Stroke = ColorOff();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Uart2CtsLow: " + ex.Message);
            }
        }

        private void Uart2DsrHigh(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    dsr2Switch.Text = "HIGH";
                    dsr2Switch.Foreground = ColorOn();
                    dsr2Pin1.Fill = ColorOn();
                    lineDsr2Pin1Pin2.Stroke = ColorOn();
                    crossDtrDsr.Stroke = ColorOn();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Uart2DsrHigh: " + ex.Message);
            }
        }

        private void Uart2DsrLow(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    dsr2Switch.Text = "LOW";
                    dsr2Switch.Foreground = ColorOff();
                    dsr2Pin1.Fill = ColorOff();
                    lineDsr2Pin1Pin2.Stroke = ColorOff();
                    crossDtrDsr.Stroke = ColorOff();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Uart2DsrLow: " + ex.Message);
            }
        }

        private void Uart2DcdHigh(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    dcd2Switch.Text = "HIGH";
                    dcd2Switch.Foreground = ColorOn();
                    dcd2Pin.Fill = ColorOn();
                    lineDcd1ToDcd2.Stroke = ColorOn();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Uart2DcdHigh: " + ex.Message);
            }
        }

        private void Uart2DcdLow(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    dcd2Switch.Text = "LOW";
                    dcd2Switch.Foreground = ColorOff();
                    dcd2Pin.Fill = ColorOff();
                    lineDcd1ToDcd2.Stroke = ColorOff();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Uart2DcdLow: " + ex.Message);
            }
        }

        private void ComboBoxDropDown2(object sender, EventArgs e)
        {
            try
            {
                string[] filteredPorts;

                Debug.WriteLine("ComboBoxDropDown2 enter");

                if (uart1 == null)
                {
                    Debug.WriteLine("uart1 == null");
                    filteredPorts = SerialPort.GetPortNames().ToArray();
                }
                else
                {
                    Debug.WriteLine("uart1 != null");
                    filteredPorts = SerialPort.GetPortNames().Where(port => (port != uart1.Name)).ToArray();
                }

                comboBox2.ItemsSource = filteredPorts;

                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in ComboBoxPorts_SelectionChanged2: " + ex.Message);
            }
        }

        private void ComboBoxSelectionChanged2(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                string selectedPort;

                if (uart2 != null)
                {
                    uart2.UartDestroy();
                    Debug.WriteLine("uart2 " + uart2.Name + " is closed");
                }

                if (e.AddedItems.Count == 0)
                {
                    return;
                }

                selectedPort = e.AddedItems[0] as string;

                if (selectedPort != null)
                {
                    comboBox2.Text = selectedPort;

                    uart2 = new Uart(selectedPort, 115200, DataDisplayBlock2);
                    uart2.UartStart();
                    uart2.ctsHigh += Uart2CtsHigh;
                    uart2.ctsLow += Uart2CtsLow;
                    uart2.dsrHigh += Uart2DsrHigh;
                    uart2.dsrLow += Uart2DsrLow;
                    uart2.cdHigh += Uart2DcdHigh;
                    uart2.cdLow += Uart2DcdLow;

                    Port2Init();
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in ComboBoxSelectionChanged2: " + ex.Message);
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
            if (uart1 == null)
            {
                return;
            }

            if (!rts1ButtClick)
            {
                uart1.RtsEnable();
                rts1ButtText.Text = "ON";

                rts1ButtText.Foreground = ColorOn();
                rts1Pin1.Fill = ColorOn();
                lineRts1Pin1Pin2.Stroke = ColorOn();

                rts1ButtClick = true;

            }
            else
            {
                uart1.RtsDisable();
                rts1ButtText.Text = "OFF";

                rts1ButtText.Foreground = ColorOff();
                rts1Pin1.Fill = ColorOff();
                lineRts1Pin1Pin2.Stroke = ColorOff();

                rts1ButtClick = false;
            }
        }

        private void Dtr1Butt_Click(object sender, RoutedEventArgs e)
        {
            if (uart1 == null)
            {
                return;
            }

            if (!dtr1ButtClick)
            {
                uart1.DtrEnable();
                dtr1ButtText.Text = "ON";

                dtr1ButtText.Foreground = ColorOn();
                dtr1Pin1.Fill = ColorOn();
                lineDtr1Pin1Pin2.Stroke = ColorOn();

                dtr1ButtClick = true;
            }
            else
            {
                uart1.DtrDisable();
                dtr1ButtText.Text = "OFF";

                dtr1ButtText.Foreground = ColorOff();
                dtr1Pin1.Fill = ColorOff();
                lineDtr1Pin1Pin2.Stroke = ColorOff();

                dtr1ButtClick = false;
            }

        }

        private void Rts2Butt_Click(object sender, RoutedEventArgs e)
        {
            if (uart2 == null)
            {
                return;
            }

            if (!rts2ButtClick)
            {
                uart2.RtsEnable();
                rts2ButtText.Text = "ON";

                rts2ButtText.Foreground = ColorOn();
                rts2Pin1.Fill = ColorOn();
                lineRts2Pin1Pin2.Stroke = ColorOn();

                rts2ButtClick = true;

            }
            else
            {
                uart2.RtsDisable();
                rts2ButtText.Text = "OFF";

                rts2ButtText.Foreground = ColorOff();
                rts2Pin1.Fill = ColorOff();
                lineRts2Pin1Pin2.Stroke = ColorOff();

                rts2ButtClick = false;
            }
        }

        private void Dtr2Butt_Click(object sender, RoutedEventArgs e)
        {
            if (uart2 == null)
            {
                return;
            }

            if (!dtr2ButtClick)
            {
                uart2.DtrEnable();
                dtr2ButtText.Text = "ON";

                dtr2ButtText.Foreground = ColorOn();
                dtr2Pin1.Fill = ColorOn();
                lineDtr2Pin1Pin2.Stroke = ColorOn();

                dtr2ButtClick = true;

            }
            else
            {
                uart2.DtrDisable();
                dtr2ButtText.Text = "OFF";

                dtr2ButtText.Foreground = ColorOff();
                dtr2Pin1.Fill = ColorOff();
                lineDtr2Pin1Pin2.Stroke = ColorOff();

                dtr2ButtClick = false;
            }
        }

        private Brush ColorOff()
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7092BE"));
        }

        private Brush ColorOn()
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99D9EA"));
        }

        private void Port1Init()
        {
            cts1Switch.Text = "LOW";
            cts1Switch.Foreground = ColorOff();
            cts1Pin1.Fill = ColorOff();
            lineCts1Pin1Pin2.Stroke = ColorOff();
            crossCtsRts.Stroke = ColorOff();

            dsr1Switch.Text = "LOW";
            dsr1Switch.Foreground = ColorOff();
            dsr1Pin1.Fill = ColorOff();
            lineDsr1Pin1Pin2.Stroke = ColorOff();
            crossDsrDtr.Stroke = ColorOff();

            dcd1Switch.Text = "LOW";
            dcd1Switch.Foreground = ColorOff();
            dcd1Pin.Fill = ColorOff();
            lineDcd1ToDcd2.Stroke = ColorOff();

            rts1ButtText.Text = "OFF";
            rts1ButtText.Foreground = ColorOff();
            rts1Pin1.Fill = ColorOff();
            lineRts1Pin1Pin2.Stroke = ColorOff();

            dtr1ButtText.Text = "OFF";
            dtr1ButtText.Foreground = ColorOff();
            dtr1Pin1.Fill = ColorOff();
            lineDtr1Pin1Pin2.Stroke = ColorOff();
        }

        private void Port2Init()
        {
            cts2Switch.Text = "LOW";
            cts2Switch.Foreground = ColorOff();
            cts2Pin1.Fill = ColorOff();
            lineCts2Pin1Pin2.Stroke = ColorOff();
            crossRtsCts.Stroke = ColorOff();

            dsr2Switch.Text = "LOW";
            dsr2Switch.Foreground = ColorOff();
            dsr2Pin1.Fill = ColorOff();
            lineDsr2Pin1Pin2.Stroke = ColorOff();
            crossDtrDsr.Stroke = ColorOff();

            dcd2Switch.Text = "LOW";
            dcd2Switch.Foreground = ColorOff();
            dcd2Pin.Fill = ColorOff();
            lineDcd1ToDcd2.Stroke = ColorOff();

            rts2ButtText.Text = "OFF";
            rts2ButtText.Foreground = ColorOff();
            rts2Pin1.Fill = ColorOff();
            lineRts2Pin1Pin2.Stroke = ColorOff();

            dtr2ButtText.Text = "OFF";
            dtr2ButtText.Foreground = ColorOff();
            dtr2Pin1.Fill = ColorOff();
            lineDtr2Pin1Pin2.Stroke = ColorOff();
        }

        private async void SendButtonClick1(object sender, RoutedEventArgs e)
        {
            string message = UartInputBox1.Text;

            if (uart1 == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(message))
            {
                await uart1.UartWrite(message);
            }
            else
            {
                Debug.WriteLine(uart1.Name + "No Message To Send");
            }
        }

        private async void SendButtonClick2(object sender, RoutedEventArgs e)
        {
            string message = UartInputBox2.Text;

            if (uart2 == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(message))
            {
                await uart2.UartWrite(message);
            }
            else
            {
                Debug.WriteLine(uart2.Name + "No Message To Send");
            }
        }
    }
    public class CoordinateGet : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UIElement element && parameter is string axis)
            {
                var parentContainer = VisualTreeHelper.GetParent(element);

                while (parentContainer != null && !(parentContainer is Page))
                {
                    parentContainer = VisualTreeHelper.GetParent(parentContainer);
                }

                // If parentContainer is of type Visual, it casts it to Visual and assigns it to visualParentContainer
                // when you retrieve it using VisualTreeHelper.GetParent(). This method returns a DependencyObject
                // Both Visual (2D elements like UI controls) and Visual3D (3D elements) inherit from DependencyObject
                if (parentContainer is Visual visualParentContainer)
                {
                    try
                    {
                        var position = element.TransformToAncestor(visualParentContainer).Transform(new Point(0, 0));

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

