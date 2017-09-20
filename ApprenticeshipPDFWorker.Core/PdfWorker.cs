using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core.Models;
using ApprenticeshipPDFWorker.Core.Services;

namespace ApprenticeshipPDFWorker.Core
{
    public class PdfWorker
    {
        private IUrlsRepository repository = new DatabaseRepository();

        public void Run()
        {
            var csvData = new StandardCsvRepository().Read("StandardPages.csv");
            var htmlData = new WebDownloader().GetAll(csvData);
            var linkUris = new ScreenScraper().GetLinkUris(htmlData);
            repository.Save(linkUris);
        }
    }
}