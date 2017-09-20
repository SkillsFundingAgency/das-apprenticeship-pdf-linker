using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //var badData = new List<Urls>();
            //badData.Add(new Urls() { StandardCode = "');DROP TABLE Test2;--"});
            //new DatabaseRepository().Save(badData);
            //new DatabaseRepository().Save(null);
            new PdfWorker().Run();
        }
    }
}

