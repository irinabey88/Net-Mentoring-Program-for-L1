namespace SiteDownloader.Interfaces
{
    /// <summary>
    /// Represents ILog interface.
    /// </summary>
    public interface ILog
    {
        void Error(string error);

        void Info(string info);
    }
}