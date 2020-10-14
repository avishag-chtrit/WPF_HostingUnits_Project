using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class ChangeCollectionClearanceException : Exception
    {
        public ChangeCollectionClearanceException()
        {
        }

        public ChangeCollectionClearanceException(string message) : base(message)
        {
        }

        public ChangeCollectionClearanceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ChangeCollectionClearanceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}