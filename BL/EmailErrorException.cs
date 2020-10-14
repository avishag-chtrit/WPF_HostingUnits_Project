using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class EmailErrorException : Exception
    {
        public EmailErrorException()
        {
        }

        public EmailErrorException(string message) : base(message)
        {
        }

        public EmailErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmailErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}