using System.Net.Sockets;
using System.Text;

namespace WebServerWithRestApi.App
{
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
}
