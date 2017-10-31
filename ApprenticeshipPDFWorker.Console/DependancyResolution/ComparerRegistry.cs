using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprenticeshipPDFWorker.Core.Services;
using StructureMap;

namespace ApprenticeshipPDFWorker.Console.DependancyResolution
{
    class ComparerRegistry : Registry
    {
        public ComparerRegistry()
        {
            For<IUrlRecordService>().Use<UrlRecordService>();
            For<IUrlRecordComparer>().Use<UrlRecordComparer>();
            For<ILog>().Use<ConsoleLogger>();
        }
    }
}
