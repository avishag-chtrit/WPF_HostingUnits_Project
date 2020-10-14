using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class DuplicatedKeyInBlException : Exception
    {
        public DuplicatedKeyInBlException()
        {
        }

        public DuplicatedKeyInBlException(string message) : base(message)
        {
        }

        public DuplicatedKeyInBlException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicatedKeyInBlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}