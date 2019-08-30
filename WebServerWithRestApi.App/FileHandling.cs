using System.IO;
using System.Text;

namespace WebServerWithRestApi.App
{
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
}
