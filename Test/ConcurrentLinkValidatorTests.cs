using MOCChecker;
using MOCChecker.Models;
using MOCChecker.Services;
using Moq;
using Moq.Protected;
using System.Net;

namespace Test
{
    public class ConcurrentLinkValidatorTests
    {
        [Fact]
        public async Task ValidateLinksAsync_ShouldSetStatusValid_WhenExternalLinkReturns200OK()
        {
            // 1. ARRANGE
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                });

            var magicHttpClient = new HttpClient(handlerMock.Object);
            var validator = new ConcurrentLinkValidator(magicHttpClient, 10);
            var link = new DocumentLink("D:\\test.md", "https://example.com/some-page", LinkType.External)
            {
                Status = LinkStatus.Pending
            };

            // 2. ACT
            await validator.ValidateLinkAsync([link], []);

            // 3. ASSERT
            Assert.Equal(LinkStatus.Valid, link.Status);
        }

        [Fact]
        public async Task ValidateLinksAsync_ShouldSetStatusBroken_WhenExternalLinkReturns404()
        {
            // 1. ARRANGE
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.NotFound
               });

            var magicHttpClient = new HttpClient(handlerMock.Object);
            var validator = new ConcurrentLinkValidator(magicHttpClient, 10);

            var link = new DocumentLink("D:\\test.md", "https://example.com/some-page", LinkType.External)
            {
                //Status = LinkStatus.Pending
            };

            // 2. ACT
            await validator.ValidateLinkAsync([link], []);

            // 3. ASSERT
            Assert.Equal(LinkStatus.Broken, link.Status);
        }
    }
}
