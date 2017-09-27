using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApprenticeshipPDFWorker.Core.Models;
using ApprenticeshipPDFWorker.Core.Services;
using NUnit.Framework;

namespace ApprenticeshipPDFWorker.UnitTest.Services
{
    [TestFixture]
    public class StandardCsvRepositoryTests
    {

        private static readonly string FileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"StandardPages.csv");

        private const string CsvData = @"StandardCode,StandardName,UrlLink
1,Network Engineer,https://network-engineer
2,Software Developer,https://apprenticeship-standard-software-developer";

        [Test]
        public void ShouldFindStandardsInACsv()
        {
            // Arrange
            var expected = CsvStandardRows();
            var input = GenerateReaderFromString(CsvData);
            var sut = new StandardCsvRepository();

            // Act
            var actual = sut.Convert(input).ToList();

            // Assert
            Assert.AreEqual(expected.First().StandardCode, actual.First().StandardCode);
            Assert.AreEqual(expected.Count, actual.Count);
        }


        [Test]
        public void CsvReadAndCheckDataLength()
        {
            // Arrange
            var first = new CsvStandardRow {
                    StandardCode = "1",
                    StandardName = "Network Engineer",
                    UrlLink = "https://www.gov.uk/government/publications/apprenticeship-standard-network-engineer"};
            var last = new CsvStandardRow {
                        StandardCode = "169",
                        StandardName = "Chef De Partie",
                        UrlLink = "https://www.gov.uk/government/publications/apprenticeship-standard-chef-de-partie"
                    };
            var sut = new StandardCsvRepository();

            // Act
            var csvData = sut.Read(FileName);

            //Assert
            Assert.AreEqual(162, csvData.Count());

            Assert.AreEqual(first.StandardCode, csvData.First().StandardCode);
            Assert.AreEqual(first.StandardName, csvData.First().StandardName);
            Assert.AreEqual(first.UrlLink, csvData.First().UrlLink);

            Assert.AreEqual(last.StandardCode, csvData.Last().StandardCode);
            Assert.AreEqual(last.StandardName, csvData.Last().StandardName);
            Assert.AreEqual(last.UrlLink, csvData.Last().UrlLink);
        }

        [Test]
        [Ignore("")]
        public void CompareCsvUrlDataToUrlListData()
        {
            // Arrange
            var csvData = new StandardCsvRepository().Read(FileName).ToList();
            // Act
            var urlList = UrlListWorker(csvData).ToList();

            // Assert
            for (int i = 0; i < csvData.Count; i++)
            {
                Assert.AreEqual(csvData[i].UrlLink, urlList[i]);
            }
        }

        public IEnumerable<CsvStandardRow> ConvertCsv(string csvData)
        {
            var formattedData = csvData.Split('\r');
            var returnList = new List<CsvStandardRow>();
            for (var i = 0; i < formattedData.Length - 1; i++)
            {
                var lineData = formattedData[i].Substring(1).Split(','); //substring(1) because of \n on each line.
                returnList.Add(new CsvStandardRow());
                returnList.ElementAt(i).StandardCode = lineData[0];
                returnList.ElementAt(i).StandardName = lineData[1];
                returnList.ElementAt(i).UrlLink = lineData[2];
            }
            //removing the header, used to have this in the conditional but i=1 is sloppy.
            returnList.RemoveAt(0);
            return returnList;
        }

        public static IEnumerable<string> UrlListWorker(IEnumerable<CsvStandardRow> data)
        {
            return data.Select(pumpkin => pumpkin.UrlLink);
        }

        private static List<CsvStandardRow> CsvStandardRows()
        {
            var expected = new List<CsvStandardRow>
            {
                new CsvStandardRow()
                {
                    StandardCode = "1",
                    StandardName = "Network Engineer",
                    UrlLink = "https://network-engineer"
                },
                new CsvStandardRow()
                {
                    StandardCode = "2",
                    StandardName = "Software Developer",
                    UrlLink = "https://apprenticeship-standard-software-developer"
                }
            };
            return expected;
        }

        private static StreamReader GenerateReaderFromString(string input)
        {
            var stream = GenerateStreamFromString(input);
            return new StreamReader(stream);
        }

        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

    }
}
