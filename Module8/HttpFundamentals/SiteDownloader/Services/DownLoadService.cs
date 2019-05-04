using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SiteDownloader.Enum;
using SiteDownloader.Interfaces;
using SiteDownloader.Models;

namespace SiteDownloader.Services
{
    /// <summary>
    /// Represents a DownLoadService.
    /// </summary>
    public class DownloadService : IDownloadService
    {
        private readonly string _adress;
        private readonly string _folder;
        private readonly Options _options;
        private readonly Uri _uri;
        private readonly HashSet<string> _visitedAdress;
        private readonly ILog _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadService"/> class.
        /// </summary>
        /// <param name="adress">An address.</param>
        /// <param name="folder">A folder for saving.</param>
        /// <param name="options">An optin <see cref="Options"/>.</param>
        public DownloadService(string adress, string folder, Options options)
        {
            if (string.IsNullOrWhiteSpace(adress))
            {
                throw new ArgumentNullException(nameof(adress));
            }
            if (string.IsNullOrWhiteSpace(folder))
            {
                throw new ArgumentNullException(nameof(folder));
            }

            _visitedAdress = new HashSet<string>();
            _adress = adress;
            _uri = new Uri(adress);
            _folder = folder;
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = new LogService();
        }

        /// <summary>
        /// Downloads a site.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task DownloadSite()
        {
            await DownloadAsync(new List<string>{ _adress}, 0).ConfigureAwait(false);
            _logger.Info($"Count downloaded links = {_visitedAdress.Count}");
        }

        /// <summary>
        /// Downloads all links from given sites.
        /// </summary>
        /// <param name="urls">A list of sites.</param>
        /// <param name="level">A download level.</param>
        /// <returns>A <see cref="Task"/></returns>
        private async Task DownloadAsync(IEnumerable<string> urls, int level)
        {
            if (urls == null)
            {
                throw new ArgumentNullException(nameof(_adress));
            }          

            var checkedUrls = CheckUrls(urls);

            if (level > _options.DownloadLevel)
            {
                return;
            }

            using (var client = new HttpClient())
            {
                try
                {                 
                    var listHttpResposes = await GetResponseMsg(checkedUrls, client);
                    var downLoadList = ApplyFilterForDonloadType(listHttpResposes);
                    foreach (var url in downLoadList)
                    {
                        _visitedAdress.Add(url.Key);
                    }

                    var listHtml = await GetFiles(downLoadList);
                    LoadOnDisk(listHtml);

                    if (level + 1 <= _options.DownloadLevel)
                    {
                        var innerUrl = new List<string>();
                        foreach (var html in listHtml)
                        {
                            innerUrl.AddRange(ProcessHtml(html.Value, new Uri(html.Key)));
                        }

                        if (innerUrl?.Count > 0)
                        {
                            await DownloadAsync(innerUrl, level + 1);
                        }
                    }                   
                }
                catch (Exception e)
                {
                    _logger.Error(e.ToString());
                    throw;
                }           
            }
        }

        /// <summary>
        /// Gets <see cref="HttpResponseMessage"/> for given urls.
        /// </summary>
        /// <param name="urls">A list of urls.</param>
        /// <param name="client">A <see cref="HttpResponseMessage"/>.</param>
        /// <returns>A <see cref="Task{Dictionary{string, HttpResponseMessage}"/>.</returns>
        private async Task<Dictionary<string, HttpResponseMessage>> GetResponseMsg(IEnumerable<string> urls, HttpClient client)
        {
            List<Task<HttpResponseMessage>> listTasks = new List<Task<HttpResponseMessage>>();
            foreach (var url in urls)
            {
                listTasks.Add(client.GetAsync(url));
            }

            return await Task.WhenAll(listTasks)
                .ContinueWith(responses =>
                {
                    var listResponse = responses.Result.ToList();
                    Dictionary<string, HttpResponseMessage> dictionResult = new Dictionary<string, HttpResponseMessage>();

                    foreach (var response in listResponse)
                    {
                        if (!dictionResult.ContainsKey(response.RequestMessage.RequestUri.AbsoluteUri))
                        {
                            dictionResult.Add(response.RequestMessage.RequestUri.AbsoluteUri, response);
                        }
                    }
                    return dictionResult;
                });
        }

        /// <summary>
        /// Loads files by url.
        /// </summary>
        /// <param name="urls">A dictionary of <see cref="Dictionary{string, HttpResponseMessage}"/>.</param>
        /// <returns>A <see cref="Task{Dictionary{string, byte[]}"/>.</returns>
        private async Task<Dictionary<string, byte[]>> GetFiles(Dictionary<string, HttpResponseMessage> urls)
        {
            List<Task<UrlContent>> tHtmlAll = new List<Task<UrlContent>>();
            foreach (var url in urls)
            {
                tHtmlAll.Add(url.Value.Content.ReadAsByteArrayAsync()
                    .ContinueWith(response => new UrlContent
                    {
                        Url = url.Key,
                        Content = response.Result
                    }));
            }

            return await Task.WhenAll(tHtmlAll)
                .ContinueWith(responses =>
                {
                    var listResponse = responses.Result.ToList();
                    Dictionary<string, byte[]> dictionResult = new Dictionary<string, byte[]>();
                    foreach (var response in listResponse)
                    {
                        if (!dictionResult.ContainsKey(response.Url))
                        {
                            dictionResult.Add(response.Url, response.Content);
                        }
                    }
                    return dictionResult;
                });
        }

        /// <summary>
        /// Checkes and caches urls.
        /// </summary>
        /// <param name="urls">A list of urls.</param>
        /// <returns>A <see cref="IEnumerable{string}"/>.</returns>
        private IEnumerable<string> CheckUrls(IEnumerable<string> urls)
        {
            if (urls == null)
            {
                throw new ArgumentNullException(nameof(urls));
            }

            var loaddedUrls = new List<string>();
            foreach (var url in urls)
            {
                if (!_visitedAdress.Contains(url) && url.StartsWith($@"http"))
                {
                    if (_options.Trace)
                    {

                        _logger.Info(url);
                    }

                    loaddedUrls.Add(url);                  
                }
            }

            return loaddedUrls;
        }

        /// <summary>
        /// Checks content type and apply filter for type.
        /// </summary>
        /// <param name="responses">A dictionary of content.</param>
        /// <returns>A <see cref="Dictionary{string, HttpResponseMessage}"/></returns>
        private Dictionary<string, HttpResponseMessage> ApplyFilterForDonloadType(Dictionary<string, HttpResponseMessage> responses)
        {
            var toDownLoadList = new Dictionary<string, HttpResponseMessage>();
            foreach (var response in responses)
            {
                if (string.IsNullOrEmpty(response.Value.Content.Headers.ContentType.CharSet))
                {
                    //Check content type and apply filter (jpg,png,pdf)
                    if (_options.ResourcesDistriction.Any(response.Value.Content.Headers.ContentType.MediaType
                        .Contains))
                    {

                    }
                }
                else
                {
                    toDownLoadList.Add(response.Key, response.Value);
                }
            }

            return toDownLoadList;
        }

        /// <summary>
        /// Creates path to folder and file name on disk.
        /// </summary>
        /// <param name="adressUri">A uri.</param>
        /// <returns>A <see cref="Tuple"/> of folder and file name.</returns>
        private Tuple<string, string> CreateFolderPath(Uri adressUri)
        {
            if (adressUri == null)
            {
                throw new ArgumentNullException(nameof(adressUri));
            }

            string folderPath;

            var segments = adressUri.Segments;
            var lastSegment = segments[segments.Length - 1];

            string fileName;
            if (lastSegment.Contains("."))
            {
                fileName = lastSegment;

                string tmpPath = "";
                foreach (var segment in segments)
                {
                    if (segment.Equals(fileName))
                    {
                        if (!fileName.Contains("."))
                        {
                            fileName = $@"{fileName}.html";
                        }
                        continue;
                    }

                    tmpPath = $@"{tmpPath}{segment.Replace("/", "\\")}";
                }
                folderPath = $@"{_folder}\\{adressUri.Authority}{tmpPath}";
            }
            else
            {
                fileName = "Index.html";

                string tmpPath = "";

                foreach (var segment in segments)
                {
                    tmpPath = $@"{tmpPath}{segment.Replace("/", "\\")}";
                }
                folderPath = $@"{_folder}\\{adressUri.Authority}{tmpPath}";
            }
            fileName = fileName.Replace("/", string.Empty).Replace("=", string.Empty).Replace("?", string.Empty).Replace("&", string.Empty);

            return new Tuple<string, string>(folderPath, fileName);
        }

        /// <summary>
        /// Checks input level of download.
        /// </summary>
        /// <param name="nodeAddresOrig">A node adress.</param>
        /// <param name="hostsName">A host name.</param>
        /// <returns></returns>
        private bool CheckRedirect(string nodeAddresOrig, string hostsName)
        {
            if (string.IsNullOrEmpty(nodeAddresOrig))
            {
                return false;
            }

            // Check if go out from site
            if (!nodeAddresOrig.StartsWith("http"))
            {
                return false;
            }
            if (_options.Redirect == RedirectDistriction.NotHigherCurrent &&
                !nodeAddresOrig.Contains(_adress))
            {
                return false;
            }
            if (_options.Redirect == RedirectDistriction.CurrentDomain &&
                !nodeAddresOrig.Contains(_uri.Authority))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Process html.
        /// </summary>
        /// <param name="content">A content.</param>
        /// <param name="adressUri">A uri.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        private IEnumerable<string> ProcessHtml(byte[] content, Uri adressUri)
        {
            var urls = new List<string>();
            var html = Encoding.Default.GetString(content);

            HtmlDocument hap = new HtmlDocument();
            hap.LoadHtml(html);
            HtmlNodeCollection nodes = hap.DocumentNode.SelectNodes("//a");

            if (nodes != null)
                foreach (HtmlNode node in nodes)
                {
                    var nodeAddresOrig = node.GetAttributeValue("href", null);
                    if (string.IsNullOrEmpty(nodeAddresOrig))
                    {
                        continue;
                    }

                    // Check if go out from site
                    if (!CheckRedirect(nodeAddresOrig, adressUri.Authority))
                    {
                        continue;
                    }

                    urls.Add(nodeAddresOrig);                    
                }

            return urls;
        }

        /// <summary>
        /// Saves page to file.
        /// </summary>
        /// <param name="path">A path to file.</param>
        /// <param name="contentByteArray">A content of file.</param>
        private void SaveToFile(string path, byte[] contentByteArray)
        {
            using (FileStream fs = File.Create(path))
            {
                fs.Write(contentByteArray, 0, contentByteArray.Length);
            }
        }

        /// <summary>
        /// Loads received data on disk.
        /// </summary>
        /// <param name="listHtml">A <see cref="Dictionary{TKey,TValue}"/></param>
        private void LoadOnDisk(Dictionary<string, byte[]> listHtml)
        {
            if (listHtml == null)
            {
                throw new ArgumentNullException(nameof(listHtml));
            }

            foreach (var html in listHtml)
            {
                var path = CreateFolderPath(new Uri(html.Key));

                if (!Directory.Exists(path.Item1))
                {
                    Directory.CreateDirectory(path.Item1);
                }

                var fileSavePath = $@"{path.Item1}\{path.Item2}";
                SaveToFile(fileSavePath, html.Value);
            }
        }
    }
}