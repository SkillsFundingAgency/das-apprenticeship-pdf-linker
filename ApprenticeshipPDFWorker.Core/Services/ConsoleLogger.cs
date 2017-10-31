using System;

namespace ApprenticeshipPDFWorker.Core.Services
{
    public class ConsoleLogger : ILog
    {
        public void Info(string message)
        {
            Console.WriteLine(message);
        }
    }
}