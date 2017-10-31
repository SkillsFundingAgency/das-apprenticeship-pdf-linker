using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core.Services
{
    public interface IUrlRecordComparer
    {
        IEnumerable<Urls> GetChanges(IEnumerable<Urls> govUkUris, ICollection<StoredUrls> dbUris);
    }
}