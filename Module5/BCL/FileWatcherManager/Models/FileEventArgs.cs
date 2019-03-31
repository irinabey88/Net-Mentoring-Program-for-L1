using System;

namespace FileWatcherManager.Models
{
    public class FileEventArgs
    {
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