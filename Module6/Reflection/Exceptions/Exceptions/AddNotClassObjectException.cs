using System;
using System.Runtime.Serialization;

namespace Exceptions.Exceptions
{
    public class AddNotClassObjectException : Exception
    {
        public AddNotClassObjectException()
        {
        }

        public AddNotClassObjectException(string message) : base(message)
        {
        }

        public AddNotClassObjectException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AddNotClassObjectException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}