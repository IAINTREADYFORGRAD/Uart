using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
class Program
{
    static SerialPort _serialPort1;
    static SerialPort _serialPort2;
    static bool _continue;

    static void Main(string[] args)
    {
        _serialPort1 = new SerialPort();
        _serialPort2 = new SerialPort();

        // Set the properties for COM3
        _serialPort1.PortName = "COM3";
        _serialPort1.BaudRate = 9600;
        _serialPort1.Parity = Parity.None;
        _serialPort1.DataBits = 8;
        _serialPort1.StopBits = StopBits.One;
        _serialPort1.Handshake = Handshake.None;
        _serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler1);

        // Set the properties for COM4
        _serialPort2.PortName = "COM4";
        _serialPort2.BaudRate = 9600;
        _serialPort2.Parity = Parity.None;
        _serialPort2.DataBits = 8;
        _serialPort2.StopBits = StopBits.One;
        _serialPort2.Handshake = Handshake.None;
        _serialPort2.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler2);

        // Open the ports
        _serialPort1.Open();
        _serialPort2.Open();

        _continue = true;

        Thread readThread1 = new Thread(Read1);
        Thread readThread2 = new Thread(Read2);

        readThread1.Start();
        readThread2.Start();

        Console.WriteLine("Type 'exit' to quit the program...");
        while (_continue)
        {
            string message = Console.ReadLine();
            if (message.ToLower() == "exit")
            {
                _continue = false;
            }
            else
            {
                _serialPort1.WriteLine(message);
                _serialPort2.WriteLine(message);
            }
        }

        // Close the ports
        _serialPort1.Close();
        _serialPort2.Close();
    }

    private static void Read1()
    {
        while (_continue)
        {
            try
            {
                string message = _serialPort1.ReadLine();
                Console.WriteLine("COM3 received: " + message);
            }
            catch (TimeoutException) { }
        }
    }

    private static void Read2()
    {
        while (_continue)
        {
            try
            {
                string message = _serialPort2.ReadLine();
                Console.WriteLine("COM4 received: " + message);
            }
            catch (TimeoutException) { }
        }
    }

    private static void DataReceivedHandler1(object sender, SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        string indata = sp.ReadExisting();
        Console.WriteLine("COM3 received data: " + indata);
    }

    private static void DataReceivedHandler2(object sender, SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        string indata = sp.ReadExisting();
        Console.WriteLine("COM4 received data: " + indata);
    }
}
