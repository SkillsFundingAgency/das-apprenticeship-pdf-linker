using ApprenticeshipPDFWorker.Core.Services;

namespace ApprenticeshipPDFWorker.Core
{
    public class PdfWorker : IPdfWorker
    {
        private readonly IDatabaseRepository _dbRepository;
        private readonly IStandardCsvRepository _csvRepository;
        private readonly IWebDownloader _webDownloader;
        private readonly IScreenScraper _screenScraper;

        public PdfWorker(IDatabaseRepository dbRepository, IStandardCsvRepository csvRepository,
            IWebDownloader webDownloader, IScreenScraper screenScraper)
        {
            _dbRepository = dbRepository;
            _csvRepository = csvRepository;
            _webDownloader = webDownloader;
            _screenScraper = screenScraper;
        }

        public void Run()
        {
            // Take the data from the CSV file and store it in variable csvData
            var csvData = _csvRepository.Read("StandardPages.csv");

            // Grabs the urls from the stored csv data
            var htmlDataFromCsv = _webDownloader.GetAll(csvData);

            //scrapes the urls for the pdf urls
            var govUkLinks = _screenScraper.GetLinkUris(htmlDataFromCsv);

            //saves the pdf urls
            _dbRepository.ProcessPdfUrlsFromGovUk(govUkLinks);
        }
    }
}