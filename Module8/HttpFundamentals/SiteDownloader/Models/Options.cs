using System.Collections.Generic;
using SiteDownloader.Enum;

namespace SiteDownloader.Models
{
    /// <summary>
    /// Represents an Options for downloading site.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// 
        /// </summary>
        public int DownloadLevel { get; set; } = 0;
        
        public RedirectDistriction Redirect { get; set; } = RedirectDistriction.NotHigherCurrent;

        public IEnumerable<string> ResourcesDistriction { get; set; } = new List<string> {"gif", "jpeg", "jpg", "pdf"};

        public bool Trace { get; set; } = false;
    }
}