using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using ApprenticeshipPDFWorker.Core.Models;
using ApprenticeshipPDFWorker.Core.Settings;
using Dapper;

namespace ApprenticeshipPDFWorker.Core.Services
{
    public class UrlRecordService : IUrlRecordService
    {
        private readonly ILog _log;
        private readonly IDatabaseSettings _settings;

        public UrlRecordService(ILog log, IDatabaseSettings settings)
        {
            _log = log;
            _settings = settings;
        }

        public ICollection<StoredUrls> GetRecordsFromDatabase()
        {
            using (var connection = new DbConnection(_settings.ConnectionString))
            {
                return connection.Query<StoredUrls>("SELECT * FROM PdfTable").ToList();
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

                    _log.Info($"X-X-X  Saved update for {change.StandardCode}  X-X-X");
                }
            }
        }
    }
}