using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
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

            serialPort.RtsEnable = true;
        }

        public void UartStart()
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

        public async Task UartWrite(string msg)
        {
            if (!await CtsWait())
            {
                serialPort.WriteLine(msg);
            }
            else
            {
                Console.WriteLine(serialPort.PortName + " CTS timeout and failed to send msg");
            }

        }

        private async Task<bool> CtsWait ()
        {
            if (serialPort.CtsHolding)
            {
                return false;
            }

            Console.Write(serialPort.PortName + " CTS is dropped");

            return await CtsTimeOut();
        }

        public void RtsEnable ()
        {
            serialPort.RtsEnable = true;
            Console.WriteLine(serialPort.PortName + " rts is enabled");
        }

        public void RtsDisable ()
        {
            serialPort.RtsEnable = false;
            Console.WriteLine(serialPort.PortName + " rts is disabled");
        }

        public void DtrEnable() // Data Terminal Ready
        {
            serialPort.DtrEnable = true;
            Console.WriteLine(serialPort.PortName + " dtr is enabled");
        }

        public void DtrDisable() // Data Set Ready
        {
            serialPort.DtrEnable = false;
            Console.WriteLine(serialPort.PortName + " dtr is disabled");
        }

        //
        // Task<bool>: produce a boolean result
        // async: could be await
        //
        private async Task<bool> CtsTimeOut ()
        {
            //
            // CancellationTokenSource
            //   1. used to create a CancellationToken and to signal that the operation should be canceled
            //   2. signal cancellation by calling its Cancel method.
            //      set the IsCancellationRequested property of the associated CancellationToken to true.
            //   3. token will automatically cancel after the specified timeout duration
            //   4. implements the IDisposable interface,
            //      which means it has a Dispose method that needs to be called to clean up resources properly
            // using
            //   1. ensure that the resources are properly disposed of when they are no longer needed
            //   2. using statement ensures that the Dispose method is called on the CancellationTokenSource (cts)
            //      when the code block is exited, whether it exits normally or due to an exception.
            //   3. here is to ensure 'cts.Dispose()' is called as soon as the code block is exited
            //

            TimeSpan timeout = TimeSpan.FromSeconds(5); 

            using (var cts = new CancellationTokenSource(timeout))
            {
                try
                {
                    //
                    // starts a new task to run the specified code asynchronously
                    // 
                    await Task.Run(() =>
                    {
                        while (!serialPort.CtsHolding)
                        {
                            Console.Write(".");
                            if (cts.Token.IsCancellationRequested)
                            {
                                cts.Token.ThrowIfCancellationRequested();
                            }
                            Thread.Sleep(1000); // Check every 100 ms
                        }
                    }, cts.Token);

                    return false;
                }
                catch (OperationCanceledException)
                {
                    return true; // Timeout occurred
                }
            }
        }
    }
}


