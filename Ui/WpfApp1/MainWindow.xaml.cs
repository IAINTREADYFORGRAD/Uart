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

// INS button

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private Uart uart1 = null;
        private Uart uart2 = null;
        private bool rts1ButtClick = false;
        private bool dtr1ButtClick = false;
        private bool rts2ButtClick = false;
        private bool dtr2ButtClick = false;

        private bool ComboBoxFiredOnce = false;
        public MainWindow()
        {
            InitializeComponent();
            PortComboBoxInit();
            comboBoxPorts1.SelectionChanged += ComboBoxPorts_SelectionChanged1;
            comboBoxPorts2.SelectionChanged += ComboBoxPorts_SelectionChanged2;
        }

        private void PortComboBoxInit()
        {
            string[] ports = SerialPort.GetPortNames();
            string selectedPort1 = null;
            string selectedPort2 = null;

            if (ports.Length > 0)
            {
                comboBoxPorts1.ItemsSource = ports;

                selectedPort1 = ports[0];

                comboBoxPorts1.Text = selectedPort1;
            }

            if (ports.Length > 1)
            {
                //
                // port: parameter used within the lambda expression representing 'ports'
                //
                string[] filteredPorts = ports.Where(port => port != selectedPort1).ToArray();
                comboBoxPorts2.ItemsSource = filteredPorts;

                selectedPort2 = ports[0];

                comboBoxPorts2.Text = selectedPort2;
            }
        }

        private void Uart1CtsHigh(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                cts1Switch.Text = "HIGH";

                cts1Switch.Foreground   = ColorOn();
                cts1Pin1.Fill           = ColorOn();
                lineCts1Pin1Pin2.Stroke = ColorOn();
                crossCtsRts.Stroke      = ColorOn();
            });
        }

        private void Uart1CtsLow(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                cts1Switch.Text = "LOW";

                cts1Switch.Foreground   = ColorOff();
                cts1Pin1.Fill           = ColorOff();
                lineCts1Pin1Pin2.Stroke = ColorOff();
                crossCtsRts.Stroke      = ColorOff();
            });
        }

        private void Uart1DsrHigh(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                dsr1Switch.Text = "HIGH";

                dsr1Switch.Foreground   = ColorOn();
                dsr1Pin1.Fill           = ColorOn();
                lineDsr1Pin1Pin2.Stroke = ColorOn();
                crossDsrDtr.Stroke      = ColorOn();
            });
        }

        private void Uart1DsrLow(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                dsr1Switch.Text = "LOW";

                dsr1Switch.Foreground   = ColorOff();
                dsr1Pin1.Fill           = ColorOff();
                lineDsr1Pin1Pin2.Stroke = ColorOff();
                crossDsrDtr.Stroke      = ColorOff();

            });
        }

        private void Uart1DcdHigh(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                dcd1Switch.Text = "HIGH";

                dcd1Switch.Foreground = ColorOn();
                dcd1Pin.Fill          = ColorOn();
                lineDcd1ToDcd2.Stroke = ColorOn();

            });
        }

        private void Uart1DcdLow(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                dcd1Switch.Text = "LOW";

                dcd1Switch.Foreground = ColorOff();
                dcd1Pin.Fill          = ColorOff();
                lineDcd1ToDcd2.Stroke = ColorOff();
            });
        }

        private void ComboBoxPorts_SelectionChanged1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string   selectedPort;
            string[] filteredPorts;

            if (ComboBoxToList())
            {

                if (uart2 == null)
                {
                    filteredPorts = SerialPort.GetPortNames().ToArray();
                }
                else
                {
                    filteredPorts = SerialPort.GetPortNames().Where(port => (port != uart2.Name)).ToArray();
                }

                comboBoxPorts1.ItemsSource = filteredPorts;
            }
            else if (ComboBoxToSelect())
            {

                if (e.AddedItems.Count == 0)
                {
                    return;
                }

                if (uart1 != null)
                {
                    uart1.UartDestroy();
                }

                selectedPort = e.AddedItems[0] as string;
                
                if (selectedPort != null)
                {
                    comboBoxPorts1.Text = selectedPort;

                    uart1 = new Uart(selectedPort, Handshake.None);
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
        }

        private void Uart2CtsHigh(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                cts2Switch.Text = "HIGH";

                cts2Switch.Foreground   = ColorOn();
                cts2Pin1.Fill           = ColorOn();
                lineCts2Pin1Pin2.Stroke = ColorOn();
                crossRtsCts.Stroke      = ColorOn();
            });
        }

        private void Uart2CtsLow(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                cts2Switch.Text = "LOW";

                cts2Switch.Foreground   = ColorOff();
                cts2Pin1.Fill           = ColorOff();
                lineCts2Pin1Pin2.Stroke = ColorOff();
                crossRtsCts.Stroke      = ColorOff();
            });
        }

        private void Uart2DsrHigh(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                dsr2Switch.Text = "HIGH";

                dsr2Switch.Foreground   = ColorOn();
                dsr2Pin1.Fill           = ColorOn();
                lineDsr2Pin1Pin2.Stroke = ColorOn();
                crossDtrDsr.Stroke      = ColorOn();

            });
        }

        private void Uart2DsrLow(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                // sleep 0.5s
                dsr2Switch.Text = "LOW";

                dsr2Switch.Foreground   = ColorOff();
                dsr2Pin1.Fill           = ColorOff();
                lineDsr2Pin1Pin2.Stroke = ColorOff();
                crossDtrDsr.Stroke      = ColorOff();
            });
        }

        private void Uart2DcdHigh(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                dcd2Switch.Text = "HIGH";

                dcd2Switch.Foreground = ColorOn();
                dcd2Pin.Fill          = ColorOn();
                lineDcd1ToDcd2.Stroke = ColorOn();
            });
        }

        private void Uart2DcdLow(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                dcd2Switch.Text = "LOW";

                dcd2Switch.Foreground = ColorOff();
                dcd2Pin.Fill          = ColorOff();
                lineDcd1ToDcd2.Stroke = ColorOff();

            });
        }
        private void ComboBoxPorts_SelectionChanged2(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string   selectedPort;
            string[] filteredPorts;

            if (ComboBoxToList())
            {
                if (uart1 == null)
                {
                    filteredPorts = SerialPort.GetPortNames().ToArray();
                }
                else
                {
                    filteredPorts = SerialPort.GetPortNames().Where(port => (port != uart1.Name)).ToArray();
                }

                comboBoxPorts2.ItemsSource = filteredPorts;

            }   else if (ComboBoxToSelect())
            {
                if (e.AddedItems.Count == 0)
                {
                    return;
                }

                if (uart2 != null)
                {
                    uart2.UartDestroy();
                }

                selectedPort = e.AddedItems[0] as string;

                if (selectedPort != null)
                {
                    comboBoxPorts2.Text = selectedPort;

                    uart2 = new Uart(selectedPort, Handshake.None);
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
        }

        private void ResetCable ()
        {

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
                rts1Pin1.Fill           = ColorOn();
                lineRts1Pin1Pin2.Stroke = ColorOn();

                rts1ButtClick = true;

            } else
            {
                uart1.RtsDisable();
                rts1ButtText.Text = "OFF";

                rts1ButtText.Foreground = ColorOff();
                rts1Pin1.Fill           = ColorOff();
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
                dtr1Pin1.Fill           = ColorOn();
                lineDtr1Pin1Pin2.Stroke = ColorOn();

                dtr1ButtClick = true;
            }
            else
            {
                uart1.DtrDisable();
                dtr1ButtText.Text = "OFF";

                dtr1ButtText.Foreground = ColorOff();
                dtr1Pin1.Fill           = ColorOff();
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
                rts2Pin1.Fill           = ColorOn();
                lineRts2Pin1Pin2.Stroke = ColorOn();

                rts2ButtClick = true;

            } else
            {
                uart2.RtsDisable();
                rts2ButtText.Text = "OFF";

                rts2ButtText.Foreground = ColorOff();
                rts2Pin1.Fill           = ColorOff();
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
                dtr2Pin1.Fill           = ColorOn();
                lineDtr2Pin1Pin2.Stroke = ColorOn();

                dtr2ButtClick = true;

            } else
            {
                uart2.DtrDisable();
                dtr2ButtText.Text = "OFF";

                dtr2ButtText.Foreground = ColorOff();
                dtr2Pin1.Fill           = ColorOff();
                lineDtr2Pin1Pin2.Stroke = ColorOff();

                dtr2ButtClick = false;
            }
        }

        private bool ComboBoxToList ()
        {
            ComboBoxFiredOnce = true;

            return !ComboBoxFiredOnce;
        }

        private bool ComboBoxToSelect()
        {
            ComboBoxFiredOnce = false;

            return !ComboBoxFiredOnce;
        }

        private Brush ColorOff()
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7092BE"));
        }

        private Brush ColorOn()
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99D9EA"));
        }

        private void Port1Init ()
        {
            cts1Switch.Text         = "LOW";
            cts1Switch.Foreground   = ColorOff();
            cts1Pin1.Fill           = ColorOff();
            lineCts1Pin1Pin2.Stroke = ColorOff();
            crossCtsRts.Stroke      = ColorOff();

            dsr1Switch.Text         = "LOW";
            dsr1Switch.Foreground   = ColorOff();
            dsr1Pin1.Fill           = ColorOff();
            lineDsr1Pin1Pin2.Stroke = ColorOff();
            crossDsrDtr.Stroke      = ColorOff();

            dcd1Switch.Text         = "LOW";
            dcd1Switch.Foreground   = ColorOff();
            dcd1Pin.Fill            = ColorOff();
            lineDcd1ToDcd2.Stroke   = ColorOff();

            rts1ButtText.Text       = "OFF";
            rts1ButtText.Foreground = ColorOff();
            rts1Pin1.Fill           = ColorOff();
            lineRts1Pin1Pin2.Stroke = ColorOff();

            dtr1ButtText.Text       = "OFF";
            dtr1ButtText.Foreground = ColorOff();
            dtr1Pin1.Fill           = ColorOff();
            lineDtr1Pin1Pin2.Stroke = ColorOff();
        }

        private void Port2Init()
        {
            cts2Switch.Text         = "LOW";
            cts2Switch.Foreground   = ColorOff();
            cts2Pin1.Fill           = ColorOff();
            lineCts2Pin1Pin2.Stroke = ColorOff();
            crossRtsCts.Stroke      = ColorOff();

            dsr2Switch.Text         = "LOW";
            dsr2Switch.Foreground   = ColorOff();
            dsr2Pin1.Fill           = ColorOff();
            lineDsr2Pin1Pin2.Stroke = ColorOff();
            crossDtrDsr.Stroke      = ColorOff();

            dcd2Switch.Text         = "LOW";
            dcd2Switch.Foreground   = ColorOff();
            dcd2Pin.Fill            = ColorOff();
            lineDcd1ToDcd2.Stroke   = ColorOff();

            rts2ButtText.Text       = "OFF";
            rts2ButtText.Foreground = ColorOff();
            rts2Pin1.Fill           = ColorOff();
            lineRts2Pin1Pin2.Stroke = ColorOff();

            dtr2ButtText.Text       = "OFF";
            dtr2ButtText.Foreground = ColorOff();
            dtr2Pin1.Fill           = ColorOff();
            lineDtr2Pin1Pin2.Stroke = ColorOff();
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