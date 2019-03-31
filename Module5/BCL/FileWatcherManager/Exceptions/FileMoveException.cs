using System;
using System.Runtime.Serialization;

namespace FileWatcherManager.Exceptions
{
    /// <summary>
    /// Represences an object of <see cref="FileMoveException"/>
    /// </summary>
    public class FileMoveException : Exception
    {
        public FileMoveException(): base(Resources.Messages.FileMoveEx)
        {
        }

        public FileMoveException(string message) : base(message)
        {
        }

        public FileMoveException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FileMoveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}