using System.Data;
using System.IO.Ports;
using System.Security.Cryptography;
using System.Windows;

namespace WpfApp {
    public class Uart
    {
        static SerialPort serialPort = null;
        public string Name = null;
        public event EventHandler ctsHigh;
        public event EventHandler ctsLow;
        public event EventHandler dsrHigh;
        public event EventHandler dsrLow;
        public event EventHandler cdHigh;
        public event EventHandler cdLow;

        public Uart(string name, Handshake handshake)
        {
            if (!name.StartsWith("COM", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            serialPort = new SerialPort();
            try
            {
                UartInit(name, handshake);
            }
            catch (UnauthorizedAccessException)
            {

            }
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

            Name = name;
        }

        public void UartStart()
        {

            if (serialPort == null)
            {
                Console.WriteLine("Uart has not been created");
                return;
            }

            serialPort.Open();
            serialPort.PinChanged += new SerialPinChangedEventHandler(CtsMonitorHandler);
            serialPort.PinChanged += new SerialPinChangedEventHandler(DsrMonitorHandler);
            serialPort.PinChanged += new SerialPinChangedEventHandler(CdMonitorHandler);

            Console.WriteLine(serialPort.PortName + " is open");
        }

        public void UartDestroy()
        {
            if (serialPort == null)
            {
                return;
            }

            serialPort.Close();
            serialPort.PinChanged -= CtsMonitorHandler;
            serialPort.PinChanged -= DsrMonitorHandler;
            serialPort.PinChanged -= CdMonitorHandler;


            Console.WriteLine(serialPort.PortName + " is closed");

            serialPort = null;
        }


        public void RtsEnable()
        {
            if (serialPort == null || !serialPort.IsOpen)
            {
                return;
            }

            serialPort.RtsEnable = true;
        }

        public void RtsDisable()
        {
            if (serialPort == null || !serialPort.IsOpen)
            {
                return;
            }

            serialPort.RtsEnable = false;
        }

        public void DtrEnable()
        {
            if (serialPort == null || !serialPort.IsOpen)
            {
                return;
            }

            serialPort.DtrEnable = true;
        }

        public void DtrDisable()
        {
            if (serialPort == null || !serialPort.IsOpen)
            {
                return;
            }

            serialPort.DtrEnable = false;
        }

        protected virtual void OnCtsHigh()
        {
           ctsHigh?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnCtsLow()
        {
            ctsLow?.Invoke(this, EventArgs.Empty);
        }

        private void CtsMonitorHandler(object sender, SerialPinChangedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            if (e.EventType == SerialPinChange.CtsChanged)
            {
                bool cts = sp.CtsHolding;
                if (cts)
                {
                    OnCtsHigh();
                }
                else
                {
                    OnCtsLow();
                }
            }
        }

        protected virtual void OnDsrHigh()
        {
            dsrHigh?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDsrLow()
        {
            dsrLow?.Invoke(this, EventArgs.Empty);
        }

        private void DsrMonitorHandler(object sender, SerialPinChangedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            if (e.EventType == SerialPinChange.DsrChanged)
            {
                bool dsr = sp.DsrHolding;
                if (dsr)
                {
                    OnDsrHigh();
                }
                else
                {
                    OnDsrLow();
                }
            }
        }

        protected virtual void OnCdHigh()
        {
            cdHigh?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnCdLow()
        {
            cdLow?.Invoke(this, EventArgs.Empty);
        }
        private void CdMonitorHandler(object sender, SerialPinChangedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            if (e.EventType == SerialPinChange.CDChanged)
            {
                bool dcd = sp.CDHolding;
                if (dcd)
                {
                    OnCdHigh();
                } else
                {
                    OnCdLow();
                }
            }
        }

        private void UartRead(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            string data = sp.ReadExisting();

            if (string.IsNullOrEmpty(data))
            {
                return;
            }

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

        private async Task<bool> CtsWait()
        {
            if (serialPort.CtsHolding)
            {
                return false;
            }

            Console.Write(serialPort.PortName + " CTS is dropped");

            return await CtsTimeOut();
        }

        private async Task<bool> CtsTimeOut()
        {
            TimeSpan timeout = TimeSpan.FromSeconds(5);

            using (var cts = new CancellationTokenSource(timeout))
            {
                try
                {
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

