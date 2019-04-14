using System;
using System.Globalization;
using System.IO;
using System.Threading;
using ConfigurationManager.Services;
using FileWatcherManager.Models;
using FileWatcherManager.Services;

namespace BCL
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurationService = new ConfigurationService(Directory.GetCurrentDirectory());

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(configurationService.GetCulture().CultureName);

            var watcherService = new FileWatcherService(configurationService);
            watcherService.Find += OnFind;
            watcherService.Filter += OnFilter;
            watcherService.NotFilter += OnNotFilter;
            watcherService.Move += OnMove;
            watcherService.Start();

            Console.ReadLine();
        }

        private static void OnFilter(object sender, FileEventArgs arg)
        {
            Console.WriteLine(Resources.Messages.Filter, arg.Name, arg.Path);
        }

        private static void OnNotFilter(object sender, FileEventArgs arg)
        {
            Console.WriteLine(Resources.Messages.NotFilter, arg.Name, arg.Path);
        }

        private static void OnMove(object sender, FileEventArgs arg)
        {
            Console.WriteLine(Resources.Messages.Move, arg.Name, arg.Path);
        }

        private static void OnFind(object sender, FileEventArgs arg)
        {
            Console.WriteLine(Resources.Messages.Find, arg.Name, arg.Path);
        }
    }
}
