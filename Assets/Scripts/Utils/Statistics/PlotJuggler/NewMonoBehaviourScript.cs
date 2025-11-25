using System;
using System.Net.Sockets;
using System.Text;

public class PlotJugglerSender
{
    private TcpClient client;
    private NetworkStream stream;
    private string host = "127.0.0.1";
    private int port = 9999;

    public void Connect()
    {
        client = new TcpClient(host, port);
        stream = client.GetStream();
    }

    public void SendData(string seriesName, float value)
    {
        if (stream != null && stream.CanWrite)
        {
            string data = $"{DateTime.UtcNow:O},{value},{seriesName}\n";
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            stream.Write(bytes, 0, bytes.Length);
        }
    }

    public void Disconnect()
    {
        stream?.Close();
        client?.Close();
    }
}
