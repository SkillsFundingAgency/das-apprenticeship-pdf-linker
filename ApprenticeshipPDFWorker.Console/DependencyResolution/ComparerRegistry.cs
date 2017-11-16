using ApprenticeshipPDFWorker.Core.Services;
using StructureMap;

namespace ApprenticeshipPDFWorker.Console.DependencyResolution
{
    class ComparerRegistry : Registry
    {
        public ComparerRegistry()
        {
            For<IUrlRecordService>().Use<UrlRecordService>();
            For<IUrlRecordComparer>().Use<UrlRecordComparer>();
        }
    }
}
