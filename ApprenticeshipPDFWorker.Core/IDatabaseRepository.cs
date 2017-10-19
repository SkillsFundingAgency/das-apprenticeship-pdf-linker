using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core
{
    public interface IDatabaseRepository
    {
        void ProcessPdfUrlsFromGovUk(IEnumerable<Urls> govUkUrls);
    }
}