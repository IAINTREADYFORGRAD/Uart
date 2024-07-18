using System;
using System.Threading;

namespace Uart_Console_App
{
    class Program
    {
        static void Main__(string[] args)
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
            try
            {
                // () => Com3.Send("Msg From Com3"): create a lambda expression
                // this lambda expression is passed to the Thread constructor
                // (): lambda expression takes no parameters
                // =>: separates the parameter list from the body of the lambda expression
                // When SendThread.Start() is called later in the program, the thread starts executing the lambda expression
                //Thread SendThread = new Thread (() => Com3.Send("Msg From Com3"));
                //Thread ReceiveThread = new Thread(() => Com4.Receive());

                //SendThread.Start();
                //ReceiveThread.Start();
                Com4.RtsEnable();

                Com3.Send("Msg From Com3");
                Com4.Receive();


            } catch (Exception ex) {
                Console.WriteLine($"error: {ex.Message}");
            } finally {
                Com4.RtsDisable();
                Com3.CloseSerial();
                Com4.CloseSerial();
            }

        }
    }
}