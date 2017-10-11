using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core.Services
{
    public interface IWebDownloader
    {
        string Get(string url);
        IEnumerable<HtmlStandardPage> GetAll(IEnumerable<CsvStandardRow> csvData);
    }
}