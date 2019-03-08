using System;
using System.Collections.Generic;
using FileManager.Interfaces;
using FileManager.Models;

namespace FileManager.Services
{
    /// <summary>
    /// Represents a <see cref="FileManager"/>.
    /// </summary>
    public class FileManager : IFileManager
    {
        private readonly Predicate<string> _filteredFunction;

        private readonly List<FileSystemObject> _fileSystemObjects = new List<FileSystemObject>();

        /// <summary>
        /// Create an instance of <see cref="FileManager"/>.
        /// </summary>
        /// <param name="filteredFunction">Filtration rules.</param>
        public FileManager(Predicate<string> filteredFunction)
        {
            _filteredFunction = filteredFunction ?? throw new ArgumentNullException(nameof(filteredFunction));
        }

        public List<FileSystemObject> FileSystemObjectsList => _fileSystemObjects;

        /// <summary>
        /// Check is <see cref="FileSystemObject"/> filtered.
        /// </summary>
        /// <param name="path">Full file name.</param>
        /// <returns></returns>
        public bool IsFiltered(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"File system object name can't be null or empty");
            }

            return _filteredFunction?.Invoke(path) ?? true;
        }

        /// <summary>
        /// Save <see cref="FileSystemObject"/> to storage.
        /// </summary>
        /// <param name="fileSystemObject">The <see cref="FileSystemObject"/>.</param>
        public void Save(FileSystemObject fileSystemObject)
        {
            if (fileSystemObject == null)
            {
                throw new ArgumentNullException(nameof(fileSystemObject));
            }

            _fileSystemObjects.Add(fileSystemObject);
        }
    }
}