using MOCChecker;
using MOCChecker.Interfaces;
using MOCChecker.Models;
using Moq;

namespace Test
{
    public class ApplicationTest
    {
        [Fact]
        public async Task RunAsync_ShouldFilterImages_WhenIgnoreImagesIsTrue()
        {
            // 1. ARRANGE
            var scannerMock = new Mock<IFileScanner>();
            var extractorMock = new Mock<ILinkExtractor>();
            var validatorMock = new Mock<ILinkValidator>();
            var reporterMock = new Mock<IReportGenerator>();

            scannerMock.Setup(s => s.GetAllFiles(It.IsAny<string>()))
                       .Returns(["note.md"]);

            var mixedLinks = new List<DocumentLink>
            {
                new("note.md", "target.md", LinkType.Internal),
                new("note.md", "image.png", LinkType.Internal)
            };

            extractorMock.Setup(e => e.ExtractLinksAsync(It.IsAny<string>()))
                         .ReturnsAsync(mixedLinks);

            var app = new Application(scannerMock.Object, extractorMock.Object, validatorMock.Object, reporterMock.Object);

            // 2. ACT
            await app.RunAsync("C:\\fake", ignoreImages: true);

            // 3. ASSERT
            validatorMock.Verify(v => v.ValidateLinkAsync(
                It.Is<IEnumerable<DocumentLink>>(links => links.Count() == 1),
                It.IsAny<IEnumerable<string>>()
            ), Times.Once);
        }
    }
}
