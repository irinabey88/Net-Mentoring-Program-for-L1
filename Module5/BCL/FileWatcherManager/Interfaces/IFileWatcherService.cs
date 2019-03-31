using System;
using FileWatcherManager.Models;

namespace FileWatcherManager.Interfaces
{
    /// <summary>
    /// Representes an interface <see cref="IFileWatcherService"/>
    /// </summary>
    public interface IFileWatcherService
    {
        /// <summary>
        /// Start watching given directories.
        /// </summary>
        void Start();

        event EventHandler<FileEventArgs> Find;

        event EventHandler<FileEventArgs> Filter;

        event EventHandler<FileEventArgs> NotFilter;

        event EventHandler<FileEventArgs> Move;
    }
}