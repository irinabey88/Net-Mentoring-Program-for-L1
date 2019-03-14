using System;
using System.Runtime.Serialization;

namespace SymbolParser.CustomExceptions
{
    /// <summary>
    /// Represents a <see cref="IndexOutOfRangeException"/>.
    /// </summary>
    public class IndexSymbolOutOfRangeException : Exception
    {
        /// <summary>
        /// Create an instance of <see cref="IndexSymbolOutOfRangeException"/>.
        /// </summary>
        public IndexSymbolOutOfRangeException()
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="IndexSymbolOutOfRangeException"/>.
        /// </summary>
        public IndexSymbolOutOfRangeException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="IndexSymbolOutOfRangeException"/>.
        /// </summary>
        public IndexSymbolOutOfRangeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="IndexSymbolOutOfRangeException"/>.
        /// </summary>
        protected IndexSymbolOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}