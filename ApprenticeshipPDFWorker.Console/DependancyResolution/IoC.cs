using ApprenticeshipPDFWorker.Console.DependancyResolution;
using ApprenticeshipPDFWorker.Core;
using ApprenticeshipPDFWorker.Core.Services;
using ApprenticeshipPDFWorker.Core.Settings;
using StructureMap;

namespace ApprenticeshipPDFWorker.Console
{
    public static class IoC
    {
        public static IContainer Initialise()
        {
            return new Container(c =>
            {
                c.AddRegistry<CoreRegistry>();
                c.AddRegistry<WebRegistry>();
                c.AddRegistry<ComparerRegistry>();
            });
        }
    }
}
