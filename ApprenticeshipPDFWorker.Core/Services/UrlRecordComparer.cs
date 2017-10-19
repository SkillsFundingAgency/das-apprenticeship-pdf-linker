using System;
using System.Collections.Generic;
using System.Linq;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Core
{
    public class UrlRecordComparer : IUrlRecordComparer
    {
        public IEnumerable<StoredUrls> 
            GetChanges(IEnumerable<Urls> govUkUris, ICollection<StoredUrls> dbUris)
        {
            foreach (var uri in govUkUris)
            {
                var latest =
                    dbUris.OrderByDescending(x => x.DateSeen).FirstOrDefault(x => x.StandardCode == uri.StandardCode);
                if (latest == null || uri.StandardUrl != latest.StandardUrl || uri.AssessmentUrl != latest.AssessmentUrl)
                {
                    yield return new StoredUrls
                    {
                        AssessmentUrl = uri.AssessmentUrl,
                        DateSeen = DateTime.UtcNow,
                        StandardCode = uri.StandardCode,
                        StandardUrl = uri.StandardUrl
                    };
                }
            }
        }
    }
}