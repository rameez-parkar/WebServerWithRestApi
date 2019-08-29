using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace WebServerWithRestApi.App
{
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
            string mimetype = MimeTypes.GetMIMEType(filePath);
            if (File.Exists(filePath))
            {
                byte[] byteFileData = FileHandling.ConvertToBytes(filePath);
                StringBuilder responseHeader = new StringBuilder();
                responseHeader.AppendLine("HTTP/1.1 200 OK");
                responseHeader.AppendLine("Content-Type: " + mimetype + ";charset=UTF-8");
                responseHeader.AppendLine();
                List<byte> response = new List<byte>();
                response.AddRange(Encoding.ASCII.GetBytes(responseHeader.ToString()));
                response.AddRange(byteFileData);
                byte[] responseByte = response.ToArray();
                _senderSocket.Send(responseByte);
                _senderSocket.Shutdown(SocketShutdown.Both);
                _senderSocket.Close();
            }
        }
    }
}
