using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class CanNotLoadFileInBlException : Exception
    {
        public CanNotLoadFileInBlException()
        {
        }

        public CanNotLoadFileInBlException(string message) : base(message)
        {
        }

        public CanNotLoadFileInBlException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CanNotLoadFileInBlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}