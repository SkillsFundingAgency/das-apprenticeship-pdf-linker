﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using ApprenticeshipPDFWorker.Core.Models;
using Dapper;

namespace ApprenticeshipPDFWorker.Core
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
            var urlList = connection.Query<StoredUrls>("SELECT * FROM PdfTable").ToList();
            connection.Close();
            return urlList;
        }
        public void InsertChanges(IEnumerable<StoredUrls> linkUris)
        {

            // updateList is populated in the comparison methods, and so only the entries 
            // which are different between the csv file and the database are updated.
            foreach (var change in linkUris)
            {
                using (var connection = new DbConnection(ConfigurationManager.ConnectionStrings["GovUk"].ConnectionString))
                {
                    connection.Execute(@"
                INSERT INTO PdfTable 
                  (
                    StandardCode, 
                    StandardUrl, 
                    AssessmentUrl , 
                    DateSeen) 
                VALUES 
                  (@StandardCode, 
                   @StandardUrl, 
                   @AssessmentUrl , 
                   @DateSeen)", change);

                    _log.Info($"Saved update for {change.StandardCode}");
                }
            }
        }
    }
}