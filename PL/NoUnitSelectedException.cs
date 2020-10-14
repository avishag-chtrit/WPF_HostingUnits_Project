using System;
using System.Runtime.Serialization;

namespace PL
{
    [Serializable]
    internal class NoUnitSelectedException : Exception
    {
        public NoUnitSelectedException()
        {
        }

        public NoUnitSelectedException(string message) : base(message)
        {
        }

        public NoUnitSelectedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoUnitSelectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}