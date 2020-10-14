using System;
using System.Runtime.Serialization;

namespace DAL
{
    [Serializable]
    public class KeyDoesNotExistException : Exception
    {
        public KeyDoesNotExistException()
        {
        }

        public KeyDoesNotExistException(string message) : base(message)
        {
        }

        public KeyDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected KeyDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}