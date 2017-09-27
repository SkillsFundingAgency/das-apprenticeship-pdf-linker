using ApprenticeshipPDFWorker.Core;

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

