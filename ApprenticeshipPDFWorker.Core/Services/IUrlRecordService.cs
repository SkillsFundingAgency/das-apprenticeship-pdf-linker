using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core.Services
{
    public interface IUrlRecordService
    {
        ICollection<StoredUrls> GetRecordsFromDatabase();
        void InsertChanges(IEnumerable<Urls> linkUris);
    }
}