using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WebServerWithRestApi.App
{
    public class HttpRequestListener
    {
        private Socket _socket;
        private IPEndPoint _endPoint;
        private int port = 9090;
        public void ListenForConnections()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _endPoint = new IPEndPoint(IPAddress.Any, port);
            _socket.Bind(_endPoint);
            _socket.Listen(10);
            Console.WriteLine("Listening for connection...");
        }

        public Socket AcceptConnection()
        {
            try
            {
                Task<Socket> socket = Task.Run(() =>
                {
                    while (true)
                    {
                        Socket senderSocket = _socket.Accept();
                        Console.WriteLine("Connection established...");
                        return senderSocket;
                    }
                });
                return socket.GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                throw new ConnectionFailedException();
            }
        }
    }
}
