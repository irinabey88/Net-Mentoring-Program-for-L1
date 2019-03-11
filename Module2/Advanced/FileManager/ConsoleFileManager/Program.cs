using System;
using FileManager.Services;
using FileManagerSubscriber;

namespace ConsoleFileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"ENTER PATH");
            var enteredPath = Console.ReadLine();

            var visitor = new FileSystemVisitor(new FileManager.Services.FileManager(path => path.ToLower().Contains("eng")));
            var fileService = new ManagerSubscriber(visitor, path => path.Contains("12"), path => true, path => false);
            visitor.Search($@"{enteredPath}");
            Console.ReadKey();
        }
    }
}
