using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core.Models;
using ApprenticeshipPDFWorker.Core.Services;
using SFA.DAS.NLog.Logger;

namespace ApprenticeshipPDFWorker.Core
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly IUrlRecordService _recordService;
        private readonly IUrlRecordComparer _comparer;
        private readonly ILog _log;

        public DatabaseRepository(IUrlRecordService recordService, IUrlRecordComparer comparer, ILog log)
        {
            _recordService = recordService;
            _comparer = comparer;
            _log = log;
        }

        public void ProcessPdfUrlsFromGovUk(IEnumerable<Urls> govUkUrls)
        {
            var dbUrlRecords = _recordService.GetRecordsFromDatabase();
            var mappedChanges = _comparer.GetChanges(govUkUrls, dbUrlRecords);
            _recordService.InsertChanges(mappedChanges);
            _log.Info(_recordService.ChangeCountMessageBuilder());
        }
    }
}