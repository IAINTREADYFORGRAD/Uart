using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Runtime.InteropServices;

namespace UartApp
{

    class Program1
    {
        
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: UartApp <COM port name>");
                return;
            }

            string comPortName = args[0];

            Uart comPort = new Uart(comPortName, Handshake.None);
        }

    }
}