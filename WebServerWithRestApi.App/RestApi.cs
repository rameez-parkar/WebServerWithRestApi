using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;

namespace WebServerWithRestApi.App
{
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
            byte[] responseByte = null;
            if(operation == "Operation not found...")
            {
                JObject responseData = new JObject(new JProperty("Error", "Operation not found..."));
                byte[] byteResponseData = Encoding.ASCII.GetBytes(responseData.ToString());
                StringBuilder responseHeader = new StringBuilder();
                responseHeader.AppendLine("HTTP/1.1 404 NOT_FOUND");
                responseHeader.AppendLine("Content-Type: application/json;charset=UTF-8");
                List<byte> response = new List<byte>();
                response.AddRange(Encoding.ASCII.GetBytes(responseHeader.ToString()));
                response.AddRange(byteResponseData);
                responseByte = response.ToArray();
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
                responseByte = response.ToArray();
            }
            _senderSocket.Send(responseByte);
            _senderSocket.Shutdown(SocketShutdown.Both);
            _senderSocket.Close();
        }
    }
}
