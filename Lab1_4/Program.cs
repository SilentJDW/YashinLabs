using System.Net;
using System.Net.Sockets;

namespace Lab1_4
{
    internal class Program

    {
        public static void Recieve(int sreq, ref Socket sclient)
        {
            Console.WriteLine($"Remote client: {sclient.RemoteEndPoint}");
            using var stream = new NetworkStream(sclient);

            using var r = new StreamReader(stream);
            using var w = new StreamWriter(stream);

            string result = r.ReadLine();
            Console.WriteLine($"Received: {result}, Requests: {sreq}");
            //Thread.Sleep(100); //

            w.WriteLine(result.ToUpper());
            w.Flush();
            sclient.Dispose();

        }
        static void Main(string[] args)
        {
            const int PORT = 1111;
            const int MAX_QUEUE = 10;
            Console.WriteLine("Hello, World!");
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, PORT);
            using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(MAX_QUEUE);
            int req = 0;
            object sync = new object();
            while (true)
            {
                Socket client = socket.Accept();
                Task.Run(() =>
                {
                    try
                    {
                        Console.WriteLine($"Remote client: {client.RemoteEndPoint}");
                        using var stream = new NetworkStream(client);

                        using var r = new StreamReader(stream);
                        using var w = new StreamWriter(stream);
                        lock (sync) req++;
                        string result = r.ReadLine();
                        Console.WriteLine($"Received: {result}, Requests: {req}");
                        //Thread.Sleep(100); //

                        w.WriteLine(result.ToUpper());
                        w.Flush();
                    }
                    finally
                    {
                        client.Dispose();
                    } 
                });
            }
        }
    }
}
