using System.Collections.Generic;
using FileManager.Models;

namespace FileManager.Interfaces
{
    /// <summary>
    /// Represents an  <see cref="IFileManager"/> interface.
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// Gets <see cref="FileSystemObject"/> storage.
        /// </summary>
        List<FileSystemObject> FileSystemObjectsList { get; }

        /// <summary>
        /// Cheks is <see cref="FileSystemObject"/> filtered.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool IsFiltered(string path);

        /// <summary>
        /// Save <see cref="FileSystemObject"/> to storage.
        /// </summary>
        /// <param name="fileSystemObject">The <see cref="FileSystemObject"/>.</param>
        void Save(FileSystemObject fileSystemObject);
    }
}