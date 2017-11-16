using ApprenticeshipPDFWorker.Core.Services;
using SFA.DAS.NLog.Logger;
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
            For<ILog>().Use(x => new NLogLogger(x.ParentType, null, null)).AlwaysUnique();
        }
}
}
