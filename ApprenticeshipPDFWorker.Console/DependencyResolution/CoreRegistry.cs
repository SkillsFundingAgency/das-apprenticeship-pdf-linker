using StructureMap;
using ApprenticeshipPDFWorker.Core;
using ApprenticeshipPDFWorker.Core.Settings;

namespace ApprenticeshipPDFWorker.Console.DependencyResolution
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
