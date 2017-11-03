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
            var csvData = _csvRepository.Read("StandardPages.csv");

            var htmlDataFromCsv = _webDownloader.GetAll(csvData);

            var govUkLinks = _screenScraper.GetLinkUris(htmlDataFromCsv);

            _dbRepository.ProcessPdfUrlsFromGovUk(govUkLinks);
        }
    }
}