using System;
using System.Threading.Tasks;
using SiteDownloader.Enum;
using SiteDownloader.Interfaces;
using SiteDownloader.Models;
using SiteDownloader.Services;

namespace SiteDownloaderConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var pd = new SiteDownloader("https://lady.tut.by/", $@"{AppDomain.CurrentDomain.BaseDirectory}\tut",
                new Options()
                {
                    DownloadLevel = 1,
                    Trace = false,
                    Redirect = RedirectDistriction.WithoutRestriction
                });

            pd.Start();
            Console.ReadLine();
        }
    }

    public class SiteDownloader
    {
        private readonly IDownloadService _downloadService;

        public SiteDownloader(string adres, string folder, Options options)
        {
            _downloadService = new DownloadService(adres, folder, options);
        }

        public void Start()
        {
            try
            {
                Task.WhenAll(_downloadService.DownloadSite())
                    .ContinueWith(task => Console.WriteLine("All work done!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
