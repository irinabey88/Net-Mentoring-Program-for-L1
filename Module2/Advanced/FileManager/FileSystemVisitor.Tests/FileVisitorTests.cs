using System;
using System.IO;
using FileManagerSubscriber;
using NUnit.Framework;

namespace FileSystemVisitor.Tests
{
    [TestFixture]
    public class FileVisitorTests
    {
        [Test]
        public void FileSystemVisitor_Search_ArgumentException()
        {
            var fileManager = new FileManager.Services.FileManager(path => true);
            var visitor = new FileManager.Services.FileSystemVisitor(fileManager);

            Assert.Throws<ArgumentException>(() => visitor.Search(string.Empty));
        }

        [Test]
        public void FileSystemVisitor_Search_Null_ArgumentException()
        {
            var fileManager = new FileManager.Services.FileManager(path => true);
            var visitor = new FileManager.Services.FileSystemVisitor(fileManager);

            Assert.Throws<ArgumentException>(() => visitor.Search(null));
        }

        [Test]
        public void FileSystemVisitor_Create_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new FileManager.Services.FileSystemVisitor(null));
        }

        [Test]
        public void FileManager_Create_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new FileManager.Services.FileManager(null));
        }

        [Test]
        public void FileSystemVisitor_Search_Execution()
        {
            var fileManager = new FileManager.Services.FileManager(x => true);
            var visitor = new FileManager.Services.FileSystemVisitor(fileManager);

            var path = $@"D:\English";

            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                var directories = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
                visitor.Search(path);

                if (files.Length > 0 || directories.Length > 0)
                {
                    Assert.Greater(fileManager.FileSystemObjectsList.Count, 0);
                }
                else
                {
                    Assert.Equals(fileManager.FileSystemObjectsList.Count, 0);
                }
            }
            else
            {
                Assert.Throws<DirectoryNotFoundException>(() => visitor.Search(path));
            }
        }

        [Test]
        public void FileSystemVisitor_Search_Only_Files()
        {
            var path = $@"D:\English";

            var fileManager = new FileManager.Services.FileManager(x => x.ToLower().Contains("eng"));
            var visitor = new FileManager.Services.FileSystemVisitor(fileManager);
            var fileService = new ManagerSubscriber(visitor, x => true, x => true, x => false);
            visitor.Search(path);

            foreach (var fileSystemObject in fileManager.FileSystemObjectsList)
            {
                Assert.Equals(fileSystemObject.GetFileSystemType(), "-F-");
            }
        }

        [Test]
        public void FileSystemVisitor_Search_Only_Directories()
        {
            var path = $@"D:\English";

            var fileManager = new FileManager.Services.FileManager(x => x.ToLower().Contains("eng"));
            var visitor = new FileManager.Services.FileSystemVisitor(fileManager);
            var fileService = new ManagerSubscriber(visitor, x => true, x => false, x => true);
            visitor.Search(path);

            foreach (var fileSystemObject in fileManager.FileSystemObjectsList)
            {
                Assert.Equals(fileSystemObject.GetFileSystemType(), "-D-");
            }
        }
    }
}
