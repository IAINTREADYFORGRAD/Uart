using System;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UartUiApp
{
    public partial class Page2 : Page
    {
        private Uart uart = null;
        private string portName = null;
        private int baudRate = 115200;
        private string logName = null;

        public Page2()
        {
            InitializeComponent();
            PortComboBox.SelectionChanged += PortSelectionChanged;
            PortComboBox.DropDownOpened += PortDropDown;

            PortComboBox.SelectionChanged += BaudRateSelectionChanged;
            PortComboBox.DropDownOpened += BaudRateDropDown;
        }

        private void PortSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                string selectedPort;

                if (uart != null)
                {
                    uart.UartDestroy();
                    Debug.WriteLine("uart " + uart.Name + " is closed");
                }

                if (e.AddedItems.Count == 0)
                {
                    return;
                }

                selectedPort = e.AddedItems[0] as string;

                if (selectedPort != null)
                {
                    PortComboBox.Text = selectedPort;
                    portName = selectedPort;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in ComboBoxSelectionChanged: " + ex.Message);
            }
        }

        private void PortDropDown(object sender, EventArgs e)
        {
            try
            {
                string[] filteredPorts;

                filteredPorts = SerialPort.GetPortNames().ToArray();
           
                PortComboBox.ItemsSource = filteredPorts;


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in ComboBoxDropDown: " + ex.Message);
            }
        }

        private void BaudRateSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                string BaudRateStr;
                int    BaudRate;

                if (e.AddedItems.Count == 0)
                {
                    return;
                }

                BaudRateStr = e.AddedItems[0] as string;
                if (BaudRateStr != null && int.TryParse(BaudRateStr, out BaudRate))
                {
                    BaudRateComboBox.Text = BaudRateStr;
                    baudRate = BaudRate;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in BaudRateSelectionChanged: " + ex.Message);
            }
        }

        private void BaudRateDropDown(object sender, EventArgs e)
        {
            try
            {
                string[] BaudRate = new string[5];

                BaudRate[0] = "115200";
                BaudRate[1] = "57600";
                BaudRate[2] = "38400";
                BaudRate[3] = "19200";
                BaudRate[4] = "9600";

                BaudRateComboBox.ItemsSource = BaudRate;


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in BaudRateDropDown: " + ex.Message);
            }
        }
        private void PortInit()
        {
            CtsState.Text = "LOW";
            CtsState.Foreground = ColorOff();
            CtsPin.Fill = ColorOff();

            DsrState.Text = "LOW";
            DsrState.Foreground = ColorOff();
            DsrPin.Fill = ColorOff();

            DcdState.Text = "LOW";
            DcdState.Foreground = ColorOff();
            DcdPin.Fill = ColorOff();
        }

        private void UartCtsHigh(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    CtsState.Text = "HIGH";
                    CtsState.Foreground = ColorOn();
                    CtsPin.Fill = ColorOn();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in UartCtsHigh: " + ex.Message);
            }
        }

        private void UartCtsLow(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    CtsState.Text = "LOW";
                    CtsState.Foreground = ColorOff();
                    CtsPin.Fill = ColorOff();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in UartCtsLow: " + ex.Message);
            }
        }

        private void UartDsrHigh(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    DsrState.Text = "HIGH";
                    DsrState.Foreground = ColorOn();
                    DsrPin.Fill = ColorOn();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in UartDsrHigh: " + ex.Message);
            }
        }

        private void UartDsrLow(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    DsrState.Text = "LOW";
                    DsrState.Foreground = ColorOff();
                    DsrPin.Fill = ColorOff();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in UartDsrLow: " + ex.Message);
            }
        }

        private void UartDcdHigh(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    DcdState.Text = "HIGH";
                    DcdState.Foreground = ColorOn();
                    DcdPin.Fill = ColorOn();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in UartDcdHigh: " + ex.Message);
            }
        }

        private void UartDcdLow(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    DcdState.Text = "LOW";
                    DcdState.Foreground = ColorOff();
                    DcdPin.Fill = ColorOff();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in UartDcdLow: " + ex.Message);
            }
        }

        private void RtsToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (uart == null)
            {
                return;
            }

            uart.RtsEnable();

        }

        private void RtsToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (uart == null)
            {
                return;
            }

            uart.RtsDisable();
        }

        private void DtrToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (uart == null)
            {
                return;
            }

            uart.DtrEnable();

        }

        private void DtrToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (uart == null)
            {
                return;
            }

            uart.DtrDisable();
        }

        private async void SendButtonClick(object sender, RoutedEventArgs e)
        {
            string message = InputBox.Text;

            if (uart == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(message))
            {
                await uart.UartWrite(message);
            }
            else
            {
                Debug.WriteLine(uart.Name + "No Message To Send");
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

        private void LogNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                logName = textBox.Text;
                textBox.Text = logName;

            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (uart != null)
            {
                uart.UartDestroy();
            }


            uart = new Uart(portName, baudRate, DataDisplayBlock, logName);
            uart.UartStart();
            uart.ctsHigh += UartCtsHigh;
            uart.ctsLow += UartCtsLow;
            uart.dsrHigh += UartDsrHigh;
            uart.dsrLow += UartDsrLow;
            uart.cdHigh += UartDcdHigh;
            uart.cdLow += UartDcdLow;

            PortInit();
        }



        // start, global var for baudrate, logname, and port#
    }


}

