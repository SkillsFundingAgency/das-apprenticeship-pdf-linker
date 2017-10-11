using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core
{
    public class DatabaseRepository : IUrlsRepository
    {
        private readonly IUrlRecordService _recordService;

        public DatabaseRepository(IUrlRecordService recordService)
        {
            _recordService = recordService;
        }

        public void Save(IEnumerable<Urls> govUkUrls)
        {
            var existingRecords = _recordService.GetRecordsFromDatabase();
            var unamppedChanges = new UrlRecordCompararer().GetChanges(govUkUrls, existingRecords);
            var mappedChanges = new UrlRecordMapper().MapToDatabase(unamppedChanges);
            _recordService.InsertChanges(mappedChanges);
        }
    }
}


//REFACTORING = making sure everything makes sense from a readability perspective

// TEST stuff

// REPEATED CODE IEnumerableUrlsExtension