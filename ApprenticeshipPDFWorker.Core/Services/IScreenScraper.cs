using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core.Services
{
    public interface IScreenScraper
    {
        IList<string> GetLinks(string html, string selector, string textInTitle);
        string GetLinkUri(string html, string linkTitle);
        IEnumerable<Urls> GetLinkUris(IEnumerable<HtmlStandardPage> htmlData);
    }
}