using System;
using System.Threading;

namespace Uart_Console_App
{
    class Program
    {
        static void Main(string[] args)
        {
            Uart Com3 = new Uart("COM3", 9600);
            Uart Com4 = new Uart("COM4", 9600);

            Com3.OpenSerial();
            Com4.OpenSerial();

            //for (int i = 0; i < 10; i++)
            //{
            //    string line = u.ReadLines();
            //    if (line != "")
            //    {
            //        Console.Write(line);
            //    }
            //    Thread.Sleep(1000);
            //}

            Thread SendThread = new Thread (() => Com3.Send("Msg From Com3"));
            Thread ReceiveThread = new Thread(() => Com4.Receive());


            Com3.CloseSerial();
            Com4.CloseSerial();
        }
    }
}