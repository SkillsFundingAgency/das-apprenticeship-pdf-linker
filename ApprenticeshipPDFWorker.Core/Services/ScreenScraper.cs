using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Parser.Html;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core.Services
{
    // Class to scrape screens for urls

    public class ScreenScraper : IScreenScraper
    {
        public string GetLinkUri(string html, string linkTitle)
        {
            var uri = GetLinks(html, ".attachment-details h2 a", linkTitle)?.FirstOrDefault();

            return uri != null ? new Uri(new Uri("https://www.gov.uk"), uri).ToString() : string.Empty;
        }


        public IList<string> GetLinks(string html, string selector, string textInTitle)
        {
            if (string.IsNullOrEmpty(html))
            {
                return new List<string>();
            }
                var parser = new HtmlParser();
                var result = parser.Parse(html);
                var all = result.QuerySelectorAll(selector);

            // Return Where value pulls based on a link (attribute "href") contanining the correct keyword
                return all.Where(x => x.InnerHtml.Contains(textInTitle)||x.InnerHtml.Contains("Assesment")).Select(x => x.GetAttribute("href")).ToList();
            }

        public IEnumerable<Urls> GetLinkUris(IEnumerable<HtmlStandardPage> htmlData)
        {
            foreach (var standardEntry in htmlData)
            {
                yield return new Urls
                {
                    AssessmentUrl = GetLinkUri(standardEntry.Html, "Assessment"),
                    StandardCode = standardEntry.StandardCode,
                    StandardUrl = GetLinkUri(standardEntry.Html, "Apprenticeship"),
                };

            }
        }
    }
}
