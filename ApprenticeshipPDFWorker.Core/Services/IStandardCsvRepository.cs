using System.Collections.Generic;
using System.IO;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core.Services
{
    public interface IStandardCsvRepository
    {
        IEnumerable<CsvStandardRow> Convert(StreamReader reader);
        IEnumerable<CsvStandardRow> Read(string fileName);
    }
}