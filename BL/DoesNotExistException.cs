using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class DoesNotExistException : Exception
    {
        public DoesNotExistException()
        {
        }

        public DoesNotExistException(string message) : base(message)
        {
        }

        public DoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}