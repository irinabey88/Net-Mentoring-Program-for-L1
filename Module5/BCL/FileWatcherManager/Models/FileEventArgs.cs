using System;

namespace FileWatcherManager.Models
{
    /// <summary>
    /// Represent an object <see cref="FileEventArgs"/>
    /// </summary>
    public class FileEventArgs
    {
        /// <summary>
        /// Initialise an object of the <see cref="FileEventArgs"/>
        /// </summary>
        /// <param name="name">The file name.</param>
        /// <param name="path">The full path to the file.</param>
        public FileEventArgs(string name, string path)
        {
            Name = string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentException($"Invalid {nameof(name)} value")
                : name;
            Path = string.IsNullOrWhiteSpace(path)
                ? throw new ArgumentException($"Invalid {nameof(path)} value")
                : path;
        }

        public string Name { get; }

        public string Path { get; }
    }
}