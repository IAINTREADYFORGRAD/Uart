using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Runtime.InteropServices;


namespace UartApp
{


    public class Uart
    {

        static SerialPort serialPort;


        public Uart(string name, Handshake handshake)
        {
            serialPort = new SerialPort();

            UartInit(name, handshake);
            UartStart();
            UartWrite();
            UartDestroy();
        }

        public void UartInit(string name, Handshake handshake)
        {

            serialPort.PortName = name;
            serialPort.BaudRate = 9600;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = (Handshake)handshake;
            serialPort.ReadTimeout = 2000; // unit: milliseconds
            serialPort.WriteTimeout = 2000;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(UartRead);
        }

        private void UartStart()
        {

            if (serialPort == null)
            {
                Console.WriteLine("Uart has not been created");
                return;
            }

            serialPort.Open();

            Console.WriteLine(serialPort.PortName + " is open");
        }

        public void UartDestroy()
        {
            if (serialPort == null)
            {
                return;
            }

            serialPort.Close();

            Console.WriteLine(serialPort.PortName + " is closed");
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
        private void UartRead(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            //
            // ReadExisting(): reads all the immediately available bytes in the serial port input buffer
            //
            string data = sp.ReadExisting();

            Console.WriteLine(serialPort.PortName + " received: " + data);
        }

        private void UartWrite()
        {
            string msg = "";
            

            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }

                msg = Console.ReadLine();
                if (!string.IsNullOrEmpty(msg))
                {
                    serialPort.WriteLine(msg);
                }
            }
        }
    }
}


