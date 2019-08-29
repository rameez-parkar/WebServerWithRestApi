using System.Threading;

namespace WebServerWithRestApi.App
{
    class Program
    {
        static void Main(string[] args)
        {
            WebServer _webServer = new WebServer();
            _webServer.Start();
        }
    }
}
