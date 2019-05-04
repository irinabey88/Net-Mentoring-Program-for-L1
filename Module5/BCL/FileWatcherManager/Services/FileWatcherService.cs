using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ConfigurationManager.Interfaces;
using ConfigurationManager.Models;
using FileWatcherManager.Exceptions;
using FileWatcherManager.Interfaces;
using FileWatcherManager.Models;

namespace FileWatcherManager.Services
{
    /// <summary>
    /// Represents an object of <see cref="FileWatcherService"/>
    /// </summary>
    public class FileWatcherService : IFileWatcherService
    {
        private readonly IConfigurationService _configurationService;

        private readonly List<FileSystemWatcher> _fileSystemWatchers = new List<FileSystemWatcher>();

        private int _fileCounter;

        private int _copyCounter;

        /// <summary>
        /// Initialize an inctance of the <see cref="FileWatcherService"/>
        /// </summary>
        /// <param name="configurationService">The service with configuration.</param>
        public FileWatcherService(IConfigurationService configurationService)
        {
            _configurationService =
                configurationService ?? throw new ArgumentNullException(nameof(configurationService));

            CheckAndCreateDirectory(_configurationService.GetDefaultConfiguration().DirectoryName);
            CreateWatchersList();
        }

        #region Events

        public event EventHandler<FileEventArgs> Find; 

        public event EventHandler<FileEventArgs> Filter;

        public event EventHandler<FileEventArgs> NotFilter;

        public event EventHandler<FileEventArgs> Move;

        #endregion

        #region Public methods

        /// <summary>
        /// Start watching given directories.
        /// </summary>
        public void Start()
        {
            foreach (var watcher in _fileSystemWatchers)
            {
                watcher.EnableRaisingEvents = true;
            }
        }

        #endregion

        #region OnEvent methods

        private void OnCreate(object sender, FileSystemEventArgs arg)
        {
            ProcessFile(arg);
        }

        private void OnDelete(object sender, FileSystemEventArgs arg)
        {
            Console.WriteLine(Resources.Messages.DELETE, arg.Name, Path.GetDirectoryName(arg.FullPath));
        }

        protected void OnFind(FileEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            Find?.Invoke(this, arg);
        }

        protected void OnFilter(FileEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            Filter?.Invoke(this, arg);
        }

        protected void OnNotFilter(FileEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            NotFilter?.Invoke(this, arg);
        }

        protected void OnMove(FileEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            Move?.Invoke(this, arg);
        }

        #endregion

        #region Private methods
        
        /// <summary>
        /// Create whatchers for listened directories
        /// </summary>
        private void CreateWatchersList()
        {
            foreach (var directory in _configurationService.GetDirectoriesToListen())
            {
                CheckAndCreateDirectory(directory.DirectoryName);

                var watcher = new FileSystemWatcher
                {
                    Path = directory.DirectoryName,
                    NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName,
                    Filter = directory.Filter
                };

                watcher.Created += OnCreate;
                watcher.Deleted += OnDelete;
                _fileSystemWatchers.Add(watcher);
            }
        }

        #endregion

        #region Process file methods
        /// <summary>
        /// Process files by setuped configuration.
        /// </summary>
        /// <param name="arg">The created in dirrectory file.</param>
        private void ProcessFile(FileSystemEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            ProcessRulesOnFile(arg);
        }

        /// <summary>
        /// Applies transformation rules for detected file.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="rule">The transformation rules.</param>
        /// <returns>The transformed name.</returns>
        private string TransformFileName(string fileName, RuleConfiguration rule)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(Resources.Messages.EmptyFileName);
            }

            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            string transformedName = GetFileName(fileName);

            if (rule.IsAddNumber)
            {
                transformedName = $"{transformedName}_{++_fileCounter}";
            }

            if (rule.IsAddDate)
            {
                transformedName = $"{transformedName}_{DateTime.Now.ToShortDateString()}";
            }

            return $"{transformedName}.{GetFileExtension(fileName)}";
        }

        /// <summary>
        /// Processes the created in directory file. 
        /// </summary>
        /// <param name="arg">The created file.</param>
        private void ProcessRulesOnFile(FileSystemEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            if (File.Exists(arg.FullPath) && !IsLocked(arg.FullPath))
            {
                OnFind(new FileEventArgs(arg.Name, arg.FullPath));
                var rules = _configurationService.GetFileRules();
                foreach (var rule in rules)
                {
                    var nameWithoutExt = GetFileName(arg.Name);

                    if (Regex.IsMatch(nameWithoutExt, rule.Pattern))
                    {
                        OnFilter(new FileEventArgs(arg.Name, arg.FullPath));

                        var destinationDirectory = GetDestinationDirectory(rule.DestinationDirectory,
                            _configurationService.GetDefaultConfiguration().DirectoryName);

                        try
                        {
                            if (!File.Exists(GetDestinationPath(destinationDirectory, TransformFileName(arg.Name, rule))))
                            {
                                File.Move(arg.FullPath,
                                    GetDestinationPath(destinationDirectory, TransformFileName(arg.Name, rule)));
                            }
                            else
                            {
                                File.Move(arg.FullPath,
                                    GetDestinationPath(destinationDirectory,
                                        $"{TransformFileName(arg.Name, rule)}_Copy_{_copyCounter++}"));
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new FileMoveException(ex.ToString());
                        }

                        OnMove(new FileEventArgs(arg.Name, arg.FullPath));
                        break;
                    }

                    OnNotFilter(new FileEventArgs(arg.Name, arg.FullPath));
                }
            }
        }

        /// <summary>
        /// Getes the directory for moving.
        /// </summary>
        /// <param name="ruleDirectory">The directory is setted in the config.</param>
        /// <param name="defaultDirectory">The default directory.</param>
        /// <returns>The directory for moving.</returns>
        private string GetDestinationDirectory(string ruleDirectory, string defaultDirectory)
        {
            if (string.IsNullOrWhiteSpace(ruleDirectory))
            {
                throw new ArgumentNullException(nameof(ruleDirectory));
            }

            if (string.IsNullOrWhiteSpace(defaultDirectory))
            {
                throw new ArgumentNullException(nameof(defaultDirectory));
            }

            return Directory.Exists(ruleDirectory)
                ? ruleDirectory
                : _configurationService.GetDefaultConfiguration().DirectoryName;
        }

        /// <summary>
        /// Creates directory if not exist.
        /// </summary>
        /// <param name="path">The path to directory.</param>
        private void CheckAndCreateDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Gets the file name without extension.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The file name without extension.</returns>
        private string GetFileName(string fileName) => fileName.Substring(0, fileName.LastIndexOf('.'));

        /// <summary>
        /// Gets files extension.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The file extension.</returns>
        private string GetFileExtension(string fileName) => fileName.Substring(fileName.LastIndexOf('.') + 1);

        /// <summary>
        /// Build the full destination path.
        /// </summary>
        /// <param name="folderName">The folder name.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns></returns>
        private string GetDestinationPath(string folderName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
            {
                throw new ArgumentException(Resources.Messages.EmptyFolderName);
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(Resources.Messages.EmptyFileName);
            }

            return Path.Combine(folderName, fileName);
        }

        /// <summary>
        /// Detectes is file locked.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>Is file locked.</returns>
        public bool IsLocked(string fileName)
        {
            try
            {
                using (File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return true;
            }
        }

        #endregion
    }
}