using System;

namespace FileManager.Models.EventArgs
{
    /// <summary>
    /// Represents a <see cref="FilteredObjectEventArgs"/>.
    /// </summary>
    public class FilteredObjectEventArgs : System.EventArgs
    {
        /// <summary>
        /// Create an instance of <see cref="FilteredObjectEventArgs"/>.
        /// </summary>
        /// <param name="filteredObject">The <see cref="FileSystemObject"/></param>
        public FilteredObjectEventArgs(FileSystemObject filteredObject)
        {
            FilteredObject = filteredObject ?? throw new ArgumentNullException(nameof(filteredObject));
        }

        /// <summary>
        /// Flag for canceling search.
        /// </summary>
        public bool CancelSearch { get; set; }

        /// <summary>
        /// Flag for excepting an objecth from result search.
        /// </summary>
        public bool ExeptObject { get; set; }

        /// <summary>
        /// Gets a FilteredObject.
        /// </summary>
        public FileSystemObject FilteredObject { get; }
    }
}