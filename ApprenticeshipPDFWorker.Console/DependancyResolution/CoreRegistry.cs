using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprenticeshipPDFWorker.Core;
using ApprenticeshipPDFWorker.Core.Settings;

namespace ApprenticeshipPDFWorker.Console.DependancyResolution
{
    class CoreRegistry : Registry

    {
        public CoreRegistry()
        {
            For<IPdfWorker>().Use<PdfWorker>();
            For<IDatabaseRepository>().Use<DatabaseRepository>();
            For<IDatabaseSettings>().Use<DatabaseSettings>();
        }
    }
}
