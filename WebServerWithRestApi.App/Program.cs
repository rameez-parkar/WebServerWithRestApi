using System;
using System.Net.Sockets;
using System.Text;
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

    public class Dispatcher
    {
        private HttpRequestParser _parser;
        private WebAppConfiguration _config;

        public Dispatcher()
        {
            _parser = new HttpRequestParser();
            _config = new WebAppConfiguration();
        }
        
        public void Dispatch(Socket senderSocket)
        {
            _parser.Parse(senderSocket);
            string[] splitPath = _parser.UrlPath.Split("/");
            if(_parser.MethodType == "GET" && _config.WebApps.ContainsKey(splitPath[0]))
            {

            }
            else if(_parser.MethodType == "POST" && splitPath[0] == "api")
            {

            }
            else
            {
                throw new PageNotFoundException();
            }
        }
    }

    public class HttpRequestParser
    {
        public string MethodType { get; private set; }
        public string UrlPath { get; private set; }
        public string Body { get; private set; }
        public void Parse(Socket senderSocket)
        {
            NetworkStream networkStream = new NetworkStream(senderSocket);
            byte[] byteStreamData = new byte[1024];
            int streamDataCount = networkStream.Read(byteStreamData, 0, byteStreamData.Length);
            string stringStreamData = Encoding.ASCII.GetString(byteStreamData, 0, streamDataCount);
            Console.WriteLine(stringStreamData);
            string[] streamFactors = stringStreamData.Split('\n');
            string[] urlData = streamFactors[0].Split(' ');
            MethodType = urlData[0];
            UrlPath = urlData[1];
            if (MethodType == "POST")
            {
                Body = stringStreamData.Substring(stringStreamData.IndexOf("{"), (stringStreamData.IndexOf("}") - stringStreamData.IndexOf("{") + 1));
                Console.WriteLine(Body);
            }
            Console.ReadKey();
        }
    }
}
