namespace ApprenticeshipPDFWorker.Core.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveQuotationMark(this string str)
        {
            return str?.Replace("\"", string.Empty) ?? string.Empty;
        }
    }
}
