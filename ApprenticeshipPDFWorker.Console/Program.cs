using System.Collections.Generic;
using ApprenticeshipPDFWorker.Core;
using ApprenticeshipPDFWorker.Core.Models;

namespace ApprenticeshipPDFWorker.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            new PdfWorker().Run();
        }
    }
}

