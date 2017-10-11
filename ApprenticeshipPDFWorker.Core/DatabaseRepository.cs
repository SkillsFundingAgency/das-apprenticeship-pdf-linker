using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly IUrlRecordService _recordService;
        private readonly IUrlRecordComparer _compararer;

        public DatabaseRepository(IUrlRecordService recordService, IUrlRecordComparer compararer)
        {
            _recordService = recordService;
            _compararer = compararer;
        }

        public void Save(IEnumerable<Urls> govUkUrls)
        {
            var existingRecords = _recordService.GetRecordsFromDatabase();
            var mappedChanges = _compararer.GetChanges(govUkUrls, existingRecords);
            _recordService.InsertChanges(mappedChanges);
        }
    }
}


//REFACTORING = making sure everything makes sense from a readability perspective

// TEST stuff