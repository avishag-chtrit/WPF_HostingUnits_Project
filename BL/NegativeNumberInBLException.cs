using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class NegativeNumberInBLException : Exception
    {
        public NegativeNumberInBLException()
        {
        }

        public NegativeNumberInBLException(string message) : base(message)
        {
        }

        public NegativeNumberInBLException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NegativeNumberInBLException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}