using System;
using System.Collections.Generic;
using System.Net;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core.Services
{
    public class WebDownloader : IWebDownloader
    {
        /// <summary>
        /// Downloads a single page which then is populated into a list of all pages
        /// </summary>
        /// <param name="url"></param>
        /// <exception cref="WebException"> Thrown when page attempted cannot be accessed </exception>
        /// <exception cref="NotSupportedException"> Thrown when passed or used for an unsupported purpose </exception>
        /// <exception cref="ArgumentNullException"> Thrown when passed a null </exception>
        /// <returns></returns>
        public string Get(string url)
        {
            using (var client = new WebClient())
            {
                client.CachePolicy =
                    new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                new ConsoleLogger().Info($"Downloading {url}");
                return client.DownloadString(url);
            }
        }

        public IEnumerable<HtmlStandardPage> GetAll(IEnumerable<CsvStandardRow> csvData)
        {
            foreach (var row in csvData)
            {
                yield return new HtmlStandardPage()
                {
                    StandardCode = row.StandardCode,
                    Html = Get(row.UrlLink)
                };
            }
        }
    }
}