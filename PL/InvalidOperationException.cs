using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace PL
{
     [Serializable]
        internal class InvalidOperationException : Exception
        {
            public InvalidOperationException()
            {
            }

            public InvalidOperationException(string message) : base(message)
            {
            }

            public InvalidOperationException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected InvalidOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    
}
