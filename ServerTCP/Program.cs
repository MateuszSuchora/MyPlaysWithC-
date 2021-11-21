using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
class Server
{
    TcpListener server = null;
    int port = 8080;
    public Server(string ip, int port)
    {
        IPAddress localAddr = IPAddress.Parse(ip);
        server = new TcpListener(localAddr, port);
        server.Start();
        StartListener();
    }
    public int GetPort(){ return port;}
    public void SetPort() { port = port + 1; }
    public void StartListener()
    {
        try
        {
            
            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Moveing client on port {0}", port+1);
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
    public void HandleDeivce(Object obj)
    {
        TcpClient client = (TcpClient)obj;
        var stream = client.GetStream();
        string imei = String.Empty;
        Byte[] bytes = new Byte[256];
        try
        {
            Console.WriteLine("{1}: Set connection on port: {0}", port + 1, Thread.CurrentThread.ManagedThreadId);
            SetPort();
            string str = port.ToString();
            Byte[] reply = System.Text.Encoding.ASCII.GetBytes(str);
            stream.Write(reply, 0, reply.Length);
            client.Close();
            ServerClientPort newport = new ServerClientPort("192.168.168.15", port);
            
            
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.ToString());
            client.Close();
        }

    }
   
}
