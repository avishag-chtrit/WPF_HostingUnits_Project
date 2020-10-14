using System;
using System.Runtime.Serialization;

namespace DAL
{
    [Serializable]
    public class CanNotLoadFileException : Exception
    {
        public CanNotLoadFileException()
        {
        }

        public CanNotLoadFileException(string message) : base(message)
        {
        }

        public CanNotLoadFileException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CanNotLoadFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}