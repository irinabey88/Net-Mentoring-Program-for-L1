using System;
using System.Runtime.Serialization;

namespace SymbolParser.CustomExceptions
{
    /// <summary>
    /// Represents a <see cref="InvalidLengthInputStringException"/>.
    /// </summary>
    public class InvalidLengthInputStringException : Exception
    {
        /// <summary>
        /// Creates an instance of <see cref="InvalidLengthInputStringException"/>.
        /// </summary>
        public InvalidLengthInputStringException()
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="InvalidLengthInputStringException"/>.
        /// </summary>
        public InvalidLengthInputStringException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="InvalidLengthInputStringException"/>.
        /// </summary>
        public InvalidLengthInputStringException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="InvalidLengthInputStringException"/>.
        /// </summary>
        public InvalidLengthInputStringException(int stringLength, int indexSymbol) : base($"Length of input string value {stringLength} is less than index of symbol value: {indexSymbol}")
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="InvalidLengthInputStringException"/>.
        /// </summary>
        protected InvalidLengthInputStringException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}