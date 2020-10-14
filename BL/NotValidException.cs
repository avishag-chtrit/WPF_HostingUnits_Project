using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class NotValidException : Exception
    {
        public NotValidException()
        {
        }

        public NotValidException(string message) : base(message)
        {
        }

        public NotValidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotValidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}