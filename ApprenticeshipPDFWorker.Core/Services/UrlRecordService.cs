using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using ApprenticeshipPDFWorker.Core.Models;
using Dapper;

namespace ApprenticeshipPDFWorker.Core.Services
{
    public class UrlRecordService : IUrlRecordService
    {
        private readonly ILog _log;

        public UrlRecordService(ILog log)
        {
            _log = log;
        }

        public ICollection<StoredUrls> GetRecordsFromDatabase()
        {
            var connection =
                new SqlConnection(
                    "Server=.\\SQLEXPRESS;Database=GovUkApprenticeships;Trusted_Connection=True;MultipleActiveResultSets=True;");
            connection.Open();
            var dbUrlRecords = connection.Query<StoredUrls>("SELECT * FROM PdfTable").ToList();
            connection.Close();
            return dbUrlRecords;
        }
        public void InsertChanges(IEnumerable<Urls> linkUris)
        {
            foreach (var change in linkUris)
            {
                using (var connection = new DbConnection(ConfigurationManager.ConnectionStrings["GovUk"].ConnectionString))
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