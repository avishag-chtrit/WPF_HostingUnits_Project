using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class CanNotDeleteException : Exception
    {
        public CanNotDeleteException()
        {
        }

        public CanNotDeleteException(string message) : base(message)
        {
        }

        public CanNotDeleteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CanNotDeleteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}