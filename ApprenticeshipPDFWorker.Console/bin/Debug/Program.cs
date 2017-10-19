using ApprenticeshipPDFWorker.Core;
using ApprenticeshipPDFWorker.Core.Services;
using StructureMap;

namespace ApprenticeshipPDFWorker.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // container sets up dependancy injection
            var container = new Container(_ =>
            {
                _.For<IUrlRecordService>().Use<UrlRecordService>();
                _.For<IDatabaseRepository>().Use<DatabaseRepository>();
                _.For<IUrlRecordComparer>().Use<UrlRecordComparer>();
                _.For<IPdfWorker>().Use<PdfWorker>();
                _.For<ILog>().Use<ConsoleLogger>();
                _.For<IScreenScraper>().Use<ScreenScraper>();
                _.For<IWebDownloader>().Use<WebDownloader>();
                _.For<IStandardCsvRepository>().Use<StandardCsvRepository>();
            });
            // calls the run method
            container.GetInstance<IPdfWorker>().Run();
        }
    }
}

