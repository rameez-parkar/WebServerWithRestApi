using System;
using System.Net.Sockets;

namespace WebServerWithRestApi.App
{
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
                _type = new StaticApp(senderSocket, _config.WebApps["Error"], "404.html");
                _type.Response();
            }
        }
    }
}
