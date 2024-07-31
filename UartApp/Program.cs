using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace UartApp
{



    /*class Program
    {
        private void ExistSerialPortPrint()
        {
            string[] ports = SerialPort.GetPortNames();

            Console.WriteLine("Available COM Ports:");
            foreach (string port in ports)
            {
                Console.WriteLine(port);
            }
        }

        static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            string comPortName = args[0];

            Uart comPort = new Uart(comPortName, Handshake.None);
            comPort.UartStart();
            comPort.CtsMonitor();
            comPort.DsrMonitor();


            while (true)
            {

                var key = Console.ReadKey(true);
                var scanCode = key.Key;

                switch (scanCode)
                {
                    case ConsoleKey.Escape:

                        comPort.UartDestroy();

                        return;

                    case ConsoleKey.F1:

                        comPort.RtsEnable();

                        break;

                    case ConsoleKey.F2:

                        comPort.RtsDisable();

                        break;

                    case ConsoleKey.F3:

                        comPort.DtrEnable();

                        break;

                    case ConsoleKey.F4:

                        comPort.DtrDisable();

                        break;
                }

                string msg = Console.ReadLine();
                if (!string.IsNullOrEmpty (msg)) {
                    await comPort.UartWrite(msg);
                }
            }

        }
    }*/

    /*class Program1
    {

        private void ExistSerialPortPrint()
        {
            string[] ports = SerialPort.GetPortNames();

            Console.WriteLine("Available COM Ports:");
            foreach (string port in ports)
            {
                Console.WriteLine(port);
            }
        }

        static void Main(string[] args)
        {

            ExistSerialPortPrint();

        }
    }

    class Program2
    {
        private void ExistSerialPortPrint()
        {
            string[] ports = SerialPort.GetPortNames();

            Console.WriteLine("Available COM Ports:");
            foreach (string port in ports)
            {
                Console.WriteLine(port);
            }
        }

        static void Main(string[] args)
        {
            Program2 program = new Program();
            program.ExistSerialPortPrint();
        }
    }*/
}