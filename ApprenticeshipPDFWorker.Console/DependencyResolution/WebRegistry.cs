using ApprenticeshipPDFWorker.Core.Services;
using StructureMap;

namespace ApprenticeshipPDFWorker.Console.DependencyResolution
{
    class WebRegistry : Registry
    {
        public WebRegistry()
        {
            For<IScreenScraper>().Use<ScreenScraper>();
            For<IWebDownloader>().Use<WebDownloader>();
            For<IStandardCsvRepository>().Use<StandardCsvRepository>();
        }
}
}
