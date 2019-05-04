using System;
using System.Runtime.Serialization;

namespace Exceptions.Exceptions
{
    public class MultiplyConstructorsExceptions : Exception
    {
        public MultiplyConstructorsExceptions()
        {
        }

        public MultiplyConstructorsExceptions(string message) : base(message)
        {
        }

        public MultiplyConstructorsExceptions(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MultiplyConstructorsExceptions(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}