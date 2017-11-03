using System.Collections.Generic;
using NUnit.Framework;
using ApprenticeshipPDFWorker.Core;
using ApprenticeshipPDFWorker.Core.Models;
using ApprenticeshipPDFWorker.Core.Services;
using Moq;
using System;
using System.Linq;

namespace ApprenticeshipPDFWorker.UnitTest.Services
{
    [TestFixture]
    public class DatabaseRepositoryTests
    {
        [Test]
        public void ShouldMakeCallsToWorkersAndServices()
        {
            //Arrange
            var mockStandardCvsRepository = new Mock<IStandardCsvRepository>();
            var mockWebDownloader = new Mock<IWebDownloader>();
            var mockScreenScraper = new Mock<IScreenScraper>();

            mockStandardCvsRepository.Setup(x => x.Read(It.IsAny<string>())).Returns(new List<CsvStandardRow>());
            mockWebDownloader.Setup(x => x.GetAll(It.IsAny<IEnumerable<CsvStandardRow>>()))
                .Returns(new List<HtmlStandardPage>());
            mockScreenScraper.Setup(x => x.GetLinkUris(It.IsAny<IEnumerable<HtmlStandardPage>>()))
                .Returns(new List<Urls>());

            var mockRecordService = new Mock<IUrlRecordService>();
            var mockComparer = new Mock<IUrlRecordComparer>();

            mockRecordService.Setup(x => x.GetRecordsFromDatabase()).Returns(new List<StoredUrls>());
            mockRecordService.Setup(x => x.InsertChanges(It.IsAny<IEnumerable<Urls>>()));

            mockComparer.Setup(x => x.GetChanges(It.IsAny<IEnumerable<Urls>>(), It.IsAny<ICollection<StoredUrls>>()))
                .Returns(new List<Urls>());

            var sut = new PdfWorker(new DatabaseRepository(mockRecordService.Object, mockComparer.Object),
                mockStandardCvsRepository.Object, mockWebDownloader.Object, mockScreenScraper.Object);
            //Act

            sut.Run();

            //Assert
            mockStandardCvsRepository.VerifyAll();
            mockWebDownloader.VerifyAll();
            mockScreenScraper.VerifyAll();
            mockComparer.VerifyAll();
            mockRecordService.VerifyAll();
        }
        [Test]
        public void ShouldCheckDifferencesAndReturnMappedData()
        {
            //Arrange
            var testUrlList = new List<Urls>
            {
                new Urls
                {
                    AssessmentUrl = "https://fakeaddress.com",
                    StandardCode =  "1",
                    StandardUrl = "https://otherfakeaddress.com"
                }
            };
            var expected = new List<StoredUrls>
            {
                new StoredUrls
                {
                    AssessmentUrl = "https://fakeaddress.com",
                    DateSeen =  DateTime.UtcNow,
                    StandardCode = "1",
                    StandardUrl = "https://otherfakeaddress.com"
                }
            };

            var sut = new UrlRecordComparer();

            // Act
            var actual = sut.GetChanges(testUrlList, new List<StoredUrls>()).ToList();

            // Assert
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(expected.FirstOrDefault().StandardCode, actual.First().StandardCode);
            Assert.AreEqual(expected.FirstOrDefault().AssessmentUrl, actual.First().AssessmentUrl);
            Assert.AreEqual(expected.FirstOrDefault().StandardUrl, actual.First().StandardUrl);
        }
    }
}