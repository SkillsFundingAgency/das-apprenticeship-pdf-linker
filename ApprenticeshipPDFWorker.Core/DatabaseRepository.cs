using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly IUrlRecordService _recordService;
        private readonly IUrlRecordComparer _comparer;

        public DatabaseRepository(IUrlRecordService recordService, IUrlRecordComparer comparer)
        {
            _recordService = recordService;
            _comparer = comparer;
        }

        public void ProcessPdfUrlsFromGovUk(IEnumerable<Urls> govUkUrls)
        {
            var dbUrlRecords = _recordService.GetRecordsFromDatabase();
            var mappedChanges = _comparer.GetChanges(govUkUrls, dbUrlRecords);
            _recordService.InsertChanges(mappedChanges);
        }
    }
}