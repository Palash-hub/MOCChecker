using MOCChecker;
using MOCChecker.Models;
using MOCChecker.Services;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class ConcurrentLinkValidatorTests
    {
        [Fact]
        public async Task ValidateLinksAsync_ShouldSetStatusValid_WhenExternalLinkReturns200OK()
        {
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
            var validator = new ConcurrentLinkValidator(magicHttpClient);
            var link = new DocumentLink("D:\\test.md", "https://example.com/some-page", LinkType.External)
            {
                Status = LinkStatus.Pending
            };

            await validator.ValidateLinkAsync([link], []);

            Assert.Equal(LinkStatus.Valid, link.Status);
        }


    }
}
