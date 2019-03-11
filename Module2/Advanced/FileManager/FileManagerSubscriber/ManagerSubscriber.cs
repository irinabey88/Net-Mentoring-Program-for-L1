using System;
using FileManager.Models;
using FileManager.Models.EventArgs;
using FileManager.Services;

namespace FileManagerSubscriber
{
    public class ManagerSubscriber
    {
        private readonly Predicate<string> _cancelSearch;

        private readonly Predicate<string> _exceptDirectory;

        private readonly Predicate<string> _exceptFile;

        public ManagerSubscriber(FileSystemVisitor fileSystemVisitor, Predicate<string> cancelSearch,
            Predicate<string> exceptDirectory, Predicate<string> exceptFile)
        {
            if(fileSystemVisitor == null) throw new ArgumentNullException(nameof(fileSystemVisitor));

            _cancelSearch = cancelSearch ?? throw new ArgumentNullException(nameof(cancelSearch));
            _exceptDirectory = exceptDirectory ?? throw new ArgumentNullException(nameof(exceptDirectory));
            _exceptFile = exceptFile ?? throw new ArgumentNullException(nameof(exceptFile));

            fileSystemVisitor.Start += ProcessStart;
            fileSystemVisitor.Finish += ProcessFinish;
            fileSystemVisitor.ObjectFound += ProcessFound;
            fileSystemVisitor.ObjectFiltered += ProcessFilter;
        }

        #region Subscribing methods

        private void ProcessStart(object sender, EventArgs arg) { }


        private void ProcessFinish(object sender, EventArgs arg) { }

        private void ProcessFound(object sender, FoundObjectEventArgs arg)
        {
            if (_cancelSearch.Invoke(arg.FoundObject.FullName))
            {
                arg.CancelSearch = true;
            }
        }

        private void ProcessFilter(object sender, FilteredObjectEventArgs arg)
        {
            if (_cancelSearch.Invoke(arg.FilteredObject.FullName))
            {
                arg.CancelSearch = true;
            }

            if (arg.FilteredObject is DirectoryObject)
            {
                if (_exceptDirectory.Invoke(arg.FilteredObject.FullName))
                {
                    arg.ExeptObject = true;
                }
            }
            else
            {
                if (_exceptFile.Invoke(arg.FilteredObject.FullName))
                {
                    arg.ExeptObject = true;
                }
            }
        }
        #endregion
    }
}
