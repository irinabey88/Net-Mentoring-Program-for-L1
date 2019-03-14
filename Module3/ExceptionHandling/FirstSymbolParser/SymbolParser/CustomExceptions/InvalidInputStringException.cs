using System;
using System.Runtime.Serialization;

namespace SymbolParser.CustomExceptions
{
    /// <summary>
    /// Represents a <see cref="InvalidInputStringException"/>.
    /// </summary>
    public class InvalidInputStringException : Exception
    {
        /// <summary>
        /// Creates an instance of <see cref="InvalidInputStringException"/>.
        /// </summary>
        public InvalidInputStringException(): base($"Entered string is null or empty!")
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="InvalidInputStringException"/>.
        /// </summary>
        public InvalidInputStringException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="InvalidInputStringException"/>.
        /// </summary>
        public InvalidInputStringException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="InvalidInputStringException"/>.
        /// </summary>
        protected InvalidInputStringException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}