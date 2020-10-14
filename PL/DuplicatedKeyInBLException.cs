using System;
using System.Runtime.Serialization;

namespace PL
{
    [Serializable]
    public class DuplicatedKeyInBLException : Exception
    {
        public DuplicatedKeyInBLException()
        {
        }

        public DuplicatedKeyInBLException(string message) : base(message)
        {
        }

        public DuplicatedKeyInBLException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicatedKeyInBLException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}