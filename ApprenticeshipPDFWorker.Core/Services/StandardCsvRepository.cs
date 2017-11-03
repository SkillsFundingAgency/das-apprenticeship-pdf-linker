﻿using System.Collections.Generic;
using System.IO;
using ApprenticeshipPDFWorker.Core.Models;
using CsvHelper;

namespace ApprenticeshipPDFWorker.Core.Services
{
    public class StandardCsvRepository : IStandardCsvRepository
    {
        public IEnumerable<CsvStandardRow> Read(string fileName)
        {
            var streamReader = File.OpenText(fileName);
            return Convert(streamReader);
        }

        public IEnumerable<CsvStandardRow> Convert(StreamReader reader)
        {
            var csvReader = new CsvReader(reader);
            return csvReader.GetRecords<CsvStandardRow>();
        }
    }
}
