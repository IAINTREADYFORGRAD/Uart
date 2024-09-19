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
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
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

        private void RtsToggleButton_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void RtsToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
        }

        private void DtrToggleButton_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void DtrToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
        }

        private Brush ColorOff()
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7092BE"));
        }

        private Brush ColorOn()
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99D9EA"));
        }
    }


}

