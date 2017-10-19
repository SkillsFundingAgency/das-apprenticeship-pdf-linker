using System.Collections.Generic;
using NUnit.Framework;
using ApprenticeshipPDFWorker.Core;
using ApprenticeshipPDFWorker.Core.Models;
using Moq;

namespace ApprenticeshipPDFWorker.UnitTest.Services
{
    [TestFixture]
    public class DatabaseRepositoryTests
    {
        [Test]
        public void ShouldMakeExpectedCallsToServices()
        {
            // Arrange
            var mockRecordService = new Mock<IUrlRecordService>();
            var mockComparer = new Mock<IUrlRecordComparer>();

            mockRecordService.Setup(x => x.GetRecordsFromDatabase()).Returns(new List<StoredUrls>());
            mockRecordService.Setup(x => x.InsertChanges(It.IsAny<IEnumerable<StoredUrls>>()));

            mockComparer.Setup(x => x.GetChanges(It.IsAny<IEnumerable<Urls>>(), It.IsAny<ICollection<StoredUrls>>()))
                .Returns(new List<StoredUrls>());


            var sut = new DatabaseRepository(mockRecordService.Object, mockComparer.Object);

            // Act
            sut.ProcessPdfUrlsFromGovUk(new List<Urls>());

            // Assert
            mockRecordService.VerifyAll();
            mockComparer.VerifyAll();
        }
    }
}
