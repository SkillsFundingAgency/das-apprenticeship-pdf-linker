using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApprenticeshipPDFWorker.Core.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveQuotationMark(this string str)
        {
            if (str == null)
            {
                return string.Empty;
            }

            return str.Replace("\"", string.Empty);
        }
    }
}
