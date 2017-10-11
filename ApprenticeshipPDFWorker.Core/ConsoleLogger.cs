using System;

namespace ApprenticeshipPDFWorker.Core
{
    public class ConsoleLogger : ILog
    {
        public void Info(string message)
        {
            Console.WriteLine(message);
        }
    }
}