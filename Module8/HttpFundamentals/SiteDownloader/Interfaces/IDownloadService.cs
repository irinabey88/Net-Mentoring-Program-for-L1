using System.Threading.Tasks;

namespace SiteDownloader.Interfaces
{
    public interface IDownloadService
    {
        Task DownloadSite();
    }
}