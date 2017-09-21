using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core.Models;
using ApprenticeshipPDFWorker.Core.Services;

namespace ApprenticeshipPDFWorker.Core
{
    public class PdfWorker
    {
        private readonly IUrlsRepository _repository = new DatabaseRepository();

        public void Run()
        {
            var csvData = new StandardCsvRepository().Read("StandardPages.csv");
            var htmlData = new WebDownloader().GetAll(csvData);
            var linkUris = new ScreenScraper().GetLinkUris(htmlData);
            _repository.Save(linkUris);
        }
    }
}