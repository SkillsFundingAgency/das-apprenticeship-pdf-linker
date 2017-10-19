using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core
{
    public interface IUrlRecordComparer
    {
        IEnumerable<StoredUrls> GetChanges(IEnumerable<Urls> govUkUris, ICollection<StoredUrls> dbUris);
    }
}