using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprenticeshipPDFWorker.Core.Services;
using StructureMap;

namespace ApprenticeshipPDFWorker.Console.DependancyResolution
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
