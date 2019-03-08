using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FileManager.Interfaces;
using FileManager.Models;
using FileManager.Models.EventArgs;

namespace FileManager.Services
{
    /// <summary>
    /// Represents a <see cref="FileSystemVisitor"/>
    /// </summary>
    public class FileSystemVisitor : IEnumerable<FileSystemObject>, ISystemVisitor
    {
        private readonly IFileManager _fileManager;

        private bool _exceptFiles;

        private bool _exceptDirectories;

        private bool _cancelSearch;

        /// <summary>
        /// Create an instance of the object <see cref="FileSystemVisitor"/>.
        /// </summary>
        /// <param name="fileManager">The <see cref="IFileManager"/></param>
        public FileSystemVisitor(IFileManager fileManager)
        {
            _fileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));
        }

        #region Enumerator

        public IEnumerator<FileSystemObject> GetEnumerator()
        {
            return GetFileSystemObjectsList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<FileSystemObject> GetFileSystemObjectsList()
        {
            foreach (var fileObject in _fileManager.FileSystemObjectsList)
            {
                yield return fileObject;
            }
        }

        #endregion

        #region Events

        public event EventHandler<EventArgs> Start;

        public event EventHandler<EventArgs> Finish;

        public event EventHandler<FoundObjectEventArgs> ObjectFound;

        public event EventHandler<FilteredObjectEventArgs> ObjectFiltered;

        #region Methods on events

        protected void OnStart(EventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            Start?.Invoke(this, arg);

            Console.WriteLine($"START SEARCH! \r\n");
        }

        protected void OnFinish(EventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            Finish?.Invoke(this, arg);

            Console.WriteLine($"FINISH SEARCH \r\n");
            Console.WriteLine($"----- File system objects found: {_fileManager.FileSystemObjectsList.Count} ------- \r\n");

            var i = 0;
            foreach (var file in _fileManager.FileSystemObjectsList)
            {


                Console.WriteLine($"{i++}. {file.Name} TYPE : {file.GetFileSystemType()}");
            }
        }

        protected void OnObjectFound(FoundObjectEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            Console.WriteLine($"Found type {arg.FoundObject.GetFileSystemType()} {arg.FoundObject.FullName}");

            this.ObjectFound?.Invoke(this, arg);

            _cancelSearch = arg.CancelSearch;

            if (_cancelSearch)
            {
                Console.WriteLine($"SEARCH IS CANCELED! \r\n");
            }
        }

        protected void OnObjectFiltered(FilteredObjectEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            Console.WriteLine($"Filtered {arg.FilteredObject.GetFileSystemType()} {arg.FilteredObject.FullName}");

            this.ObjectFiltered?.Invoke(this, arg);

            _cancelSearch = arg.CancelSearch;
            switch (arg.FilteredObject.GetFileSystemType())
            {
                case "-F-":
                    _exceptFiles = arg.ExeptObject;
                    break;
                case "-D-":
                    _exceptDirectories = arg.ExeptObject;
                    break;
                default:
                    throw new ArgumentException($"Unkown object type recived: {arg.FilteredObject.GetFileSystemType()}");
            }

            if (_cancelSearch)
            {
                Console.WriteLine($"Search is canceled!");
            }
            if (_exceptDirectories)
            {
                Console.WriteLine($"Directories are excepted from search!");
            }
            if (_exceptFiles)
            {
                Console.WriteLine($"Files are excepted from search!");
            }
        }

        #endregion

        #endregion

        #region Public methods

        /// <summary>
        /// Search files and directories by given rules.
        /// </summary>
        /// <param name="path">The name that is searched.</param>
        public void Search(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"Invalid {nameof(path)} parameter value.");                
            }

            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"path");
            }

            OnStart(EventArgs.Empty);

            ProcessDirectory(path);

            OnFinish(EventArgs.Empty);
        }

        #endregion

        #region Private methods

        private void ProcessDirectory(string path)
        {
            if (_cancelSearch) return;

            var foundDirectory = new DirectoryObject
            {
                FullName = path
            };

            OnObjectFound(new FoundObjectEventArgs(foundDirectory));
            if(_cancelSearch) return;

            if (_fileManager.IsFiltered(foundDirectory.Name))
            {
                OnObjectFiltered(new FilteredObjectEventArgs(foundDirectory));

                if(_cancelSearch) return;

                if (!_exceptDirectories)
                {
                    _fileManager.Save(foundDirectory);
                }
            }

            var filesInDirectory = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var file in filesInDirectory)
            {
                ProcessFile(file);
            }

            var childDirectories = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            foreach (var directory in childDirectories)
            {
                ProcessDirectory(directory);
            }
        }

        private void ProcessFile(string path)
        {
            if (_cancelSearch) return;

            var foundFile = new FileObject()
            {
                FullName = path
            };

            OnObjectFound(new FoundObjectEventArgs(foundFile));
            if(_cancelSearch) return;

            if (_fileManager.IsFiltered(foundFile.Name))
            {
                OnObjectFiltered(new FilteredObjectEventArgs(foundFile));
                if (!_exceptFiles)
                {
                    _fileManager.Save(foundFile);
                }
            }
        }

        #endregion
    }
}