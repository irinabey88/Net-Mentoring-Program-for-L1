using System;
using System.Runtime.Serialization;

namespace FileWatcherManager.Exceptions
{
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