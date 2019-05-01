using System;
using SiteDownloader.Interfaces;

namespace SiteDownloader.Services
{
    public class LogService : ILog
    {
        public void Error(string error)
        {
            Console.WriteLine($"ERROR {error}");
        }

        public void Info(string info)
        {
            Console.WriteLine($"INFO {info}");
        }
    }
}