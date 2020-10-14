using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class DoubleKeyException : Exception
    {
        public DoubleKeyException()
        {
        }

        public DoubleKeyException(string message) : base(message)
        {
        }

        public DoubleKeyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DoubleKeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}