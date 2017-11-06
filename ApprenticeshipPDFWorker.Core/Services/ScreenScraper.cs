using System;
using System.Collections.Generic;
using System.Configuration;
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
            var uri = GetLinks(html, ConfigurationManager.AppSettings["AttatchmentDetailsString"], linkTitle)?.FirstOrDefault();

           return uri != null ? new Uri(new Uri(ConfigurationManager.AppSettings["GovUkBaseUrl"]), uri).ToString() : string.Empty;
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

                return all.Where(x => x.InnerHtml.Contains(textInTitle)).Select(x => x.GetAttribute("href")).ToList();
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
