using System;
using System.Runtime.Serialization;

namespace Exceptions.Exceptions
{
    public class UnknownDepencyException : Exception
    {
        public UnknownDepencyException()
        {
        }

        public UnknownDepencyException(string message) : base(message)
        {
        }

        public UnknownDepencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownDepencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}