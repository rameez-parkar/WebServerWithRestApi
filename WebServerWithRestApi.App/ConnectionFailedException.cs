using System;
using System.Runtime.Serialization;

namespace WebServerWithRestApi.App
{
    [Serializable]
    internal class ConnectionFailedException : Exception
    {
        public ConnectionFailedException()
        {
            Console.WriteLine("Connection Failed...");
        }

        public ConnectionFailedException(string message) : base(message)
        {
        }

        public ConnectionFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConnectionFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}