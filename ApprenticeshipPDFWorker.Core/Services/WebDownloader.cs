using System;
using System.Collections.Generic;
using System.Net;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core.Services
{
    public class WebDownloader
    {
        /// <summary>
        /// Downloads a Page
        /// </summary>
        /// <param name="url"></param>
        /// <exception cref="WebException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public string Get(string url)
        {
            using (var client = new WebClient())
            {
                client.CachePolicy =
                    new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

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