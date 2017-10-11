using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core
{
    public interface IDatabaseRepository
    {
        void Save(IEnumerable<Urls> govUkUrls);
    }
}