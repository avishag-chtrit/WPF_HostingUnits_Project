using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    public class UnitBookedException : Exception
    {
        public UnitBookedException()
        {
        }

        public UnitBookedException(string message) : base(message)
        {
        }

        public UnitBookedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnitBookedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}