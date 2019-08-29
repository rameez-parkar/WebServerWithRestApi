using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;

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
            
            if(_parser.MethodType == "GET" && _config.WebApps.ContainsKey(splitPath[1]))
            {
                _type = new StaticApp(senderSocket, _config.WebApps[splitPath[1]], splitPath[2]);
                _type.Response();
            }
            else if(_parser.MethodType == "POST" && splitPath[1] == "api")
            {
                string requestBody = _parser.Body;
                _type = new RestApi(senderSocket, splitPath[2], requestBody);
                _type.Response();
            }
            else
            {
                Console.WriteLine("404...File Not Found...");
            }
        }
    }

    public class RestApi : ITypeHandler
    {
        private Socket _senderSocket;
        private string _apiTitle;
        private string _requestBody;
        ApiOperationIdentifier _apiOperationIdentifier = new ApiOperationIdentifier();
        ApiOperation _apiOperation = new ApiOperation();
        public RestApi(Socket senderSocket, string apiTitle, string requestBody)
        {
            _senderSocket = senderSocket;
            _apiTitle = apiTitle;
            _requestBody = requestBody;
        }
        public void Response()
        {
            string operation = _apiOperationIdentifier.GetOperation(_apiTitle);
            if(operation == "Operation not found...")
            {
                
            }
            else
            {
                JObject data = JObject.Parse(_requestBody);
                JObject responseData = (JObject)_apiOperation.GetType().GetMethod(operation).Invoke(_apiOperation, new object[] { data });
                byte[] byteResponseData = Encoding.ASCII.GetBytes(responseData.ToString());
                StringBuilder responseHeader = new StringBuilder();
                responseHeader.AppendLine("HTTP/1.1 200 OK");
                responseHeader.AppendLine("Content-Type: application/json;charset=UTF-8");
                responseHeader.AppendLine();
                List<byte> response = new List<byte>();
                response.AddRange(Encoding.ASCII.GetBytes(responseHeader.ToString()));
                response.AddRange(byteResponseData);
                byte[] responseByte = response.ToArray();
                _senderSocket.Send(responseByte);
                _senderSocket.Shutdown(SocketShutdown.Both);
                _senderSocket.Close();
            }
        }
    }

    public class ApiOperationIdentifier
    {
        public string GetOperation(string apiTitle)
        {
            foreach (var property in typeof(ApiOperation).GetMethods())
            {
                var attributes = (OperationAttribute[])property.GetCustomAttributes (typeof(OperationAttribute), false);
                foreach (var attribute in attributes)
                {

                    if (apiTitle == attribute.Operation)
                        return property.Name;
                }
            }
            return "Operation not found...";
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class OperationAttribute : Attribute
    {
        public string Operation { get; private set; }

        public OperationAttribute(string operation)
        {
            Operation = operation;
        }
    }

    public class ApiOperation
    {
        [Operation("year")]
        public JObject CheckIfLeapYear(JObject data)
        {
            string result = "";
            int year = (int)data.SelectToken("year");
            if (year % 400 != 0)
            {
                if (year % 4 == 0 && year % 100 != 0)
                {
                    result += $"{year} is a leap year.";
                }
                else
                {
                    result += $"{year} is not a leap year.";
                }
            }
            else
            {
                result += $"{year} is a leap year.";
            }
            return new JObject(new JProperty("result", result));
        }
    }
}
