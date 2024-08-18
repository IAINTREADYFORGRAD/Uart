using System.IO.Ports;

public class Uart
{
    static SerialPort serialPort;

    public Uart(string name, Handshake handshake)
    {
        serialPort = new SerialPort();
        try
        {
            UartInit(name, handshake);
        }
        catch (UnauthorizedAccessException)
        {

        }
    }


    public void RtsEnable()
    {
        if (serialPort == null || !serialPort.IsOpen)
        {
            return;
        }

        serialPort.RtsEnable = true;
    }

    public void RtsEnable()
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
}