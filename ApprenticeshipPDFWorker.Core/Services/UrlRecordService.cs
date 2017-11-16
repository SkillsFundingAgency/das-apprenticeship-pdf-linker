using System;
using System.Collections.Generic;
using System.Linq;
using ApprenticeshipPDFWorker.Core.Models;
using ApprenticeshipPDFWorker.Core.Settings;
using Dapper;
using SFA.DAS.NLog.Logger;

namespace ApprenticeshipPDFWorker.Core.Services
{
    public class UrlRecordService : IUrlRecordService
    {
        private readonly ILog _log;
        private readonly IDatabaseSettings _settings;
        private int updateCount;

        public UrlRecordService(ILog log, IDatabaseSettings settings)
        {
            _log = log;
            _settings = settings;
        }

        public ICollection<StoredUrls> GetRecordsFromDatabase()
        {
            using (var connection = new DbConnection(_settings.ConnectionString))
            {
                return connection.Query<StoredUrls>("SELECT StandardCode, StandardUrl, AssessmentUrl, DateSeen FROM PdfTable").ToList();
            }
        }
        public void InsertChanges(IEnumerable<Urls> linkUris)
        {
            foreach (var change in linkUris)
            {
                using (var connection = new DbConnection(_settings.ConnectionString))
                {
                    connection.Execute(@"
                INSERT INTO PdfTable 
                  (
                    StandardCode, 
                    StandardUrl, 
                    AssessmentUrl
                    ) 
                VALUES 
                  (@StandardCode, 
                   @StandardUrl, 
                   @AssessmentUrl
                    )", change);

                    _log.Debug($"Updated Urls in Database for Standard Code: {change.StandardCode}");
                    updateCount += 1;
                }
            }
        }

        public string ChangeCountMessageBuilder()
        {
            if (updateCount == 1)
            {
                return $"{updateCount} record updated in the database.";
            }

            return $"{updateCount} records updated in the database.";
        }
    }
}