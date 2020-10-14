using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class CanNotRemoveCollectionClearanceException : Exception
    {
        public CanNotRemoveCollectionClearanceException()
        {
        }

        public CanNotRemoveCollectionClearanceException(string message) : base(message)
        {
        }

        public CanNotRemoveCollectionClearanceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CanNotRemoveCollectionClearanceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}