using MOCChecker.Models;
using MOCChecker.Services;

namespace MOCChecker.Tests
{
    public class RegexLinkExtractorTests
    {
        private async Task<string> CreateTempMarkdownFileAsync(string content)
        {
            var tempFilePath = Path.GetTempFileName();
            await File.WriteAllTextAsync(tempFilePath, content);
            return tempFilePath;
        }

        [Fact]
        public async Task ExtractLinksAsync_ShouldFind_ObsidianInternalLinks()
        {
            // 1. ARRANGE
            var content = "Здесь текст и ссылка на [[Моя заметка]]. И еще [[Другая заметка|с алиасом]].";
            var filePath = await CreateTempMarkdownFileAsync(content);
            var extractor = new RegexLinkExtractor();

            try
            {
                // 2. ACT
                var result = (await extractor.ExtractLinksAsync(filePath)).ToList();

                // 3. ASSERT
                Assert.Multiple(
                    () => Assert.Equal(2, result.Count),
                    () => Assert.Equal(LinkType.Internal, result[0].Type),
                    () => Assert.Equal("Моя заметка", result[0].TargetPath),
                    () => Assert.Equal(LinkType.Internal, result[1].Type),
                    () => Assert.Equal("Другая заметка", result[1].TargetPath)  
                );
            }
            finally
            {
                if (File.Exists(filePath)) File.Delete(filePath);
            }
        }

        [Fact]
        public async Task ExtractLinksAsync_ShouldFind_ExternalMarkdownLinks()
        {
            // ARRANGE
            var content = "Почитай про C# [здесь](https://learn.microsoft.com) и [тут](http://test.com).";
            var filePath = await CreateTempMarkdownFileAsync(content);
            var extractor = new RegexLinkExtractor();

            try
            {
                // ACT
                var result = (await extractor.ExtractLinksAsync(filePath)).ToList();

                // ASSERT
                Assert.Multiple(
                    () => Assert.Equal(2, result.Count),
                    () => Assert.All(result, link => Assert.Equal(LinkType.External, link.Type)),

                    () => Assert.Equal("https://learn.microsoft.com", result[0].TargetPath),
                    () => Assert.Equal("http://test.com", result[1].TargetPath)
                );
            }
            finally
            {
                if (File.Exists(filePath)) File.Delete(filePath);
            }
        }

        [Fact]
        public async Task ExtractLinksAsync_ShouldIgnore_ImageLinks_IfConfigured()
        {
            // ARRANGE
            var content = "Картинка ![[image.png]] и ![alt](pic.jpg).";
            var filePath = await CreateTempMarkdownFileAsync(content);
            var extractor = new RegexLinkExtractor();

            try
            {
                // ACT
                var result = (await extractor.ExtractLinksAsync(filePath)).ToList();

                // ASSERT
                Assert.Multiple(
                    () => Assert.Equal(2, result.Count),
                    () => Assert.Equal("image.png", result[0].TargetPath),
                    () => Assert.Equal("pic.jpg", result[1].TargetPath)
                );
            }
            finally
            {
                if (File.Exists(filePath)) File.Delete(filePath);
            }
        }
    }
}
