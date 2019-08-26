using System;
using System.Collections.Generic;
using System.IO;
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
        private ITypeHandler _type;

        public Dispatcher()
        {
            _parser = new HttpRequestParser();
            _config = new WebAppConfiguration();
        }
        
        public void Dispatch(Socket senderSocket)
        {
            _parser.Parse(senderSocket);
            string[] splitPath = _parser.UrlPath.Split("/");
            foreach (var a in splitPath)
                Console.WriteLine(a);
            if(_parser.MethodType == "GET" && _config.WebApps.ContainsKey(splitPath[1]))
            {
                _type = new StaticApp(senderSocket, _config.WebApps[splitPath[1]], splitPath[2]);
                _type.Response();
            }
            else if(_parser.MethodType == "POST" && splitPath[1] == "api")
            {
                _type = new RestApi(senderSocket, splitPath[2]);
                _type.Response();
            }
            else
            {
                Console.WriteLine("404...File Not Found...");
            }
            //Console.WriteLine("usdhus");
            //Console.ReadKey();
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
            string[] streamFactors = stringStreamData.Split('\n');
            string[] urlData = streamFactors[0].Split(' ');
            MethodType = urlData[0];
            UrlPath = urlData[1];
            if (MethodType == "POST")
            {
                Body = stringStreamData.Substring(stringStreamData.IndexOf("{"), (stringStreamData.IndexOf("}") - stringStreamData.IndexOf("{") + 1));
            }
        }
    }

    public interface ITypeHandler
    {
        void Response();
    }

    public class StaticApp : ITypeHandler
    {
        private Socket _senderSocket;
        private string _rootDirectory;
        private string _fileName;
        public StaticApp(Socket senderSocket, string rootDirectory, string fileName)
        {
            _senderSocket = senderSocket;
            _rootDirectory = rootDirectory;
            _fileName = fileName;
        }
        public void Response()
        {
            string filePath = _rootDirectory + @"\" + _fileName;

            if(File.Exists(filePath))
            {
                byte[] byteFileData = FileHandling.ConvertToBytes(filePath);
                StringBuilder responseHeader = new StringBuilder();
                responseHeader.AppendLine("HTTP/1.1 200 OK");
                responseHeader.AppendLine("Content-Type: text/html;charset=UTF-8");
                responseHeader.AppendLine();
                List<byte> response = new List<byte>();
                response.AddRange(Encoding.ASCII.GetBytes(responseHeader.ToString()));
                response.AddRange(byteFileData);
                byte[] responseByte = response.ToArray();
                _senderSocket.Send(responseByte);
                Console.ReadKey();
            }
        }
    }

    public class FileHandling
    {
        public static byte[] ConvertToBytes(string filePath)
        {
            string data = File.ReadAllText(filePath);
            byte[] byteFileData = new byte[data.Length];
            byteFileData = Encoding.ASCII.GetBytes(data);
            return byteFileData;
        }
    }

    public class RestApi : ITypeHandler
    {
        private Socket _senderSocket;
        private string _apiTitle;
        public RestApi(Socket senderSocket, string apiTitle)
        {
            _senderSocket = senderSocket;
            _apiTitle = apiTitle;
        }
        public void Response()
        {
            throw new NotImplementedException();
        }
    }
}
