using System.Net.Sockets;

namespace WebServerWithRestApi.App
{
    public class WebServer
    {
        private HttpRequestListener _httpRequestListener;
        private Dispatcher _dispatcher;
        public WebServer()
        {
            _httpRequestListener = new HttpRequestListener();
            _dispatcher = new Dispatcher();
        }
        public void Start()
        {
            _httpRequestListener.ListenForConnections();
            Socket senderSocket = _httpRequestListener.AcceptConnection();
            _dispatcher.Dispatch(senderSocket);
        }
    }
}
