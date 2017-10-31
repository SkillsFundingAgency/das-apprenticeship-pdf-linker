using System.Configuration;

namespace ApprenticeshipPDFWorker.Core.Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString => ConfigurationManager.ConnectionStrings["GovUk"].ConnectionString;
    }
}