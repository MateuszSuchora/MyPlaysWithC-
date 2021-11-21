using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
class ServerClientPort
{
    TcpListener server = null;
    public ServerClientPort(string ip, int port)
    {
        IPAddress localAddr = IPAddress.Parse(ip);
        server = new TcpListener(localAddr, port);
        server.Start();
        StartListener(port);
    }
    public void StartListener(int port)
    {
        try
        {

            while (true)
            {
                Console.WriteLine("Waiting for a connection on port {0}", port);
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");
                Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
                t.Start(client);
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
            server.Stop();
        }
    }
    public static void HandleDeivce(Object obj)
    {
        TcpClient client = (TcpClient)obj;
        var stream = client.GetStream();
        string imei = String.Empty;
        string data = null;
        Byte[] bytes = new Byte[256];
        int i;
        try
        {
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                
                string hex = BitConverter.ToString(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("{1}: Received: {0}", data, Thread.CurrentThread.ManagedThreadId);
                if (data != "{quit}")
                {
                    //string str = "Hey Device!";
                    string str = Console.ReadLine();
                    Byte[] reply = System.Text.Encoding.ASCII.GetBytes(str);
                    stream.Write(reply, 0, reply.Length);
                    Console.WriteLine("{1}: Sent: {0}", str, Thread.CurrentThread.ManagedThreadId);
                }
                else { 
                    Console.WriteLine("Client quit");
                    client.Close();
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.ToString());
            client.Close();
        }
    }

}