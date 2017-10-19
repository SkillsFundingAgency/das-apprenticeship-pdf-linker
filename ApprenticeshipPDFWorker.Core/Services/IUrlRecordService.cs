using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core
{
    public interface IUrlRecordService
    {
        ICollection<StoredUrls> GetRecordsFromDatabase();
        void InsertChanges(IEnumerable<StoredUrls> linkUris);
    }
}