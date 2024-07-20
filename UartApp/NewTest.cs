using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Runtime.InteropServices;


namespace UartApp {


    public class Uart {

        static SerialPort serialPort;
        
        public void UartCreate(string name, int handshake) {

            serialPort = new SerialPort();

            serialPort.PortName = name;
            serialPort.BaudRate = 9600;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = handshake;

        }

        public void UartStart() {

            if (serialPort == null) {
                Console.WriteLine("Uart has not been created");
                return;
            }

            serialPort.Open();

            Console.WriteLine(SerialPort.PortName + " is open");
        }

        public void UartClose()
        {
            if (serialPort == null)
            {
                Console.WriteLine("Uart has not been created");
                return;
            }


            serialPort.Close();

            Console.WriteLine(SerialPort.PortName + " is closed");
        }

        private void ReceiveEventCreate() {
            serialPort.DataReceived += new SerialDataReceivedEventHandler(ReceivedEvent);
        }

        private void PortPollingStart() {
            Thread readThread = new Thread(Polling);
        }

        //
        // interrupt mothed
        //
        // 1. Data Arrival: When data arrives at the serial port, it triggers the DataReceived event.
        // 2. Event Handling: The DataReceived event handler (DataReceivedHandler1) is executed.
        //                    This execution is handled by a thread from the system's thread pool.
        // 3. Thread Pool: a collection of threads managed by the .NET runtime.
        //                 It provides threads to handle various tasks, including IO operations, timer callbacks, and event handling.
        //                 Using the thread pool avoids the overhead of creating and destroying threads manually.
        //
        private void ReceivedEvent(object sender, SerialDataReceivedEventArgs e) {
            SerialPort sp = (SerialPort)sender;

            //
            // ReadExisting(): reads all the immediately available bytes in the serial port input buffer
            //
            string data = sp.ReadExisting();

            Console.WriteLine(serialPort.PortName + "　event received: " + data);
        }



        //
        // polling method
        //
        //   1. disadvantage: introduce some latency, as it depends on the frequency of the polling
        //
        private void Polling() {

            while (true) {
                try {
                    string message = serialPort.ReadLine();
                    Console.WriteLine(serialPort.PortName + "　polling received: " + message);
                } catch (TimeoutException) { }

                if (IsPollingStop()) {
                    break;
                }
            }

        }
        
        private bool IsPollingStop() {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape) {
                return true;
            }

        }

        class Program {




        }

    }
}
    /*class Program
{
    static SerialPort serialPort1;
    static SerialPort serialPort2;
    static bool continue;

    static void Main (string[] args)
    {
        serialPort1 = new SerialPort();
        serialPort2 = new SerialPort();

        // Set the properties for COM3
        serialPort1.PortName = "COM3";
        serialPort1.BaudRate = 9600;
        serialPort1.Parity = Parity.None;
        serialPort1.DataBits = 8;
        serialPort1.StopBits = StopBits.One;
        serialPort1.Handshake = Handshake.None;
        serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler1);

        // Set the properties for COM4
        serialPort2.PortName = "COM4";
        serialPort2.BaudRate = 9600;
        serialPort2.Parity = Parity.None;
        serialPort2.DataBits = 8;
        serialPort2.StopBits = StopBits.One;
        serialPort2.Handshake = Handshake.None;
        serialPort2.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler2);

        // Open the ports
        serialPort1.Open();
        serialPort2.Open();

        continue = true;

        Thread readThread1 = new Thread(Read1);
        Thread readThread2 = new Thread(Read2);

        readThread1.Start();
        readThread2.Start();

        Console.WriteLine("Type 'exit' to quit the program...");
        while (continue)
        {
            string message = Console.ReadLine();
            if (message.ToLower() == "exit")
            {
               continue = false;
            }
            else
            {
                serialPort1.WriteLine(message);
                serialPort2.WriteLine(message);
            }
        }

        serialPort1.Close();
        serialPort2.Close();
    }

    private static void Read1()
    {
        while (continue)
        {
            try
            {
                string message = serialPort1.ReadLine();
                Console.WriteLine("COM3 polling received: " + message);
            }
            catch (TimeoutException) { }
        }
    }

    private static void Read2()
    {
        while (continue)
        {
            try
            {
                string message = serialPort2.ReadLine();
                Console.WriteLine("COM4 polling received: " + message);
            }
            catch (TimeoutException) { }
        }
    }

    private static void DataReceivedHandler1(object sender, SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        //
        // ReadExisting(): reads all the immediately available bytes, based on the encoding, in the serial port input buffer
        //
        string indata = sp.ReadExisting();
        Console.WriteLine("COM3 event received data: " + indata);
    }

    private static void DataReceivedHandler2(object sender, SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        string indata = sp.ReadExisting();
        Console.WriteLine("COM4 event received data: " + indata);
    }
}*/
