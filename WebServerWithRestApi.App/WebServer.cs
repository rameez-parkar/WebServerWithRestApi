using System.Net.Sockets;

namespace WebServerWithRestApi.App
{
    public class WebServer
    {
        private HttpRequestListener _httpRequestListener;
        public WebServer()
        {
            _httpRequestListener = new HttpRequestListener();
        }
        public void Start()
        {
            _httpRequestListener.ListenForConnections();
            _httpRequestListener.AcceptConnection();
        }
    }
}
