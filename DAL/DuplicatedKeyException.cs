using System;
using System.Runtime.Serialization;

namespace DAL
{
    [Serializable]
    public class DuplicatedKeyException : Exception
    {
        public DuplicatedKeyException()
        {
        }

        public DuplicatedKeyException(string message) : base(message)
        {
        }

        public DuplicatedKeyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicatedKeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}