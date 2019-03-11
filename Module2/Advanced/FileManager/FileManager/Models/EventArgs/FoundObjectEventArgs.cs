using System;

namespace FileManager.Models.EventArgs
{
    /// <summary>
    /// Represents a <see cref="FoundObjectEventArgs"/>.
    /// </summary>
    public class FoundObjectEventArgs : System.EventArgs
    {
        /// <summary>
        /// Create an instance of <see cref="FoundObjectEventArgs"/>.
        /// </summary>
        /// <param name="foundObject">The <see cref="FoundObjectEventArgs"/></param>
        public FoundObjectEventArgs(FileSystemObject foundObject)
        {
            FoundObject = foundObject ?? throw new ArgumentNullException(nameof(foundObject));
        }

        /// <summary>
        /// Flag for canceling search.
        /// </summary>
        public bool CancelSearch { get; set; }

        /// <summary>
        /// Gets a FoundObject.
        /// </summary>
        public FileSystemObject FoundObject { get; }
    }
}