using System;
using System.Runtime.Serialization;

namespace Exceptions.Exceptions
{
    public class AddNotInterfaceDependencyException : Exception
    {
        public AddNotInterfaceDependencyException()
        {
        }

        public AddNotInterfaceDependencyException(string message) : base(message)
        {
        }

        public AddNotInterfaceDependencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AddNotInterfaceDependencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}