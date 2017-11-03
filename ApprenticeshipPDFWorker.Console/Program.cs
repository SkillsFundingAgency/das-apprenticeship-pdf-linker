using ApprenticeshipPDFWorker.Core;
using ApprenticeshipPDFWorker.Console.DependencyResolution;

namespace ApprenticeshipPDFWorker.Console
{    
    class Program
    {
        static void Main(string[] args)
        {
            // container sets up dependancy injection
            var container = IoC.Initialise();
            // calls the run method
            container.GetInstance<IPdfWorker>().Run();
        }
    }
}

