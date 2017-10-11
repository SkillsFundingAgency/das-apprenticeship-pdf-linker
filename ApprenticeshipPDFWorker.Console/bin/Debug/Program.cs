using ApprenticeshipPDFWorker.Core;
using StructureMap;

namespace ApprenticeshipPDFWorker.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(_ =>
            {
                _.For<IUrlRecordService>().Use<UrlRecordService>();
                _.For<IUrlsRepository>().Use<DatabaseRepository>();
                _.For<IUrlRecordComparer>().Use<UrlRecordComparer>();
                _.For<IPdfWorker>().Use<PdfWorker>();
                _.For<ILog>().Use<ConsoleLogger>();
            });

            container.GetInstance<IPdfWorker>().Run();
        }
    }
}

