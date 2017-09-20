using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core
{
    public interface IUrlsRepository
    {
        void Save(IEnumerable<Urls> linkUris);
    }
}