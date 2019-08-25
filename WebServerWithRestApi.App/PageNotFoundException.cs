using System;
using System.Runtime.Serialization;

namespace WebServerWithRestApi.App
{
    [Serializable]
    internal class PageNotFoundException : Exception
    {
        public PageNotFoundException()
        {
            Console.WriteLine("404...Page Not Found...");
        }

        public PageNotFoundException(string message) : base(message)
        {
        }

        public PageNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PageNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}