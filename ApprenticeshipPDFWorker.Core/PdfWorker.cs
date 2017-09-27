using ApprenticeshipPDFWorker.Core.Services;

namespace ApprenticeshipPDFWorker.Core
{
    public class PdfWorker
    {
        private readonly IUrlsRepository _repository = new DatabaseRepository();

        public void Run()
        {
            // Take the data from the CSV file and store it in variable csvData
            var csvData = new StandardCsvRepository().Read("StandardPages.csv");

            // Grabs the urls from the stored csv data
            var htmlData = new WebDownloader().GetAll(csvData);

            //scrapes the urls for the pdf urls
            var linkUris = new ScreenScraper().GetLinkUris(htmlData);

            //saves the pdf urls
            _repository.Save(linkUris);
        }
    }
}