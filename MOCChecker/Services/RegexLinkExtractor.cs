using MOCChecker.Interfaces;
using MOCChecker.Models;
using System.Text.RegularExpressions;

namespace MOCChecker.Services
{
    public partial class RegexLinkExtractor : ILinkExtractor
    {
        [GeneratedRegex(@"\[\[(.*?)(?:\|.*?)?\]\]")]
        private static partial Regex ObsidianLinkRegex();

        [GeneratedRegex(@"\[.*?\]\((.*?)\)")]
        private static partial Regex MarkdownLinkRegex();

        public async Task<List<DocumentLink>> ExtractLinksAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Path to file is empty.", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"File {filePath} is't exists");
            }

            var documentList = new List<DocumentLink>();
            var content = await File.ReadAllTextAsync(filePath);

            var obsidianMatches = ObsidianLinkRegex().Matches(content);
            var markdownMatches = MarkdownLinkRegex().Matches(content);

            foreach ( var match in obsidianMatches.Concat(markdownMatches))
            {
                var target = match.Groups[1].Value.Trim();

                if (target.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    documentList.Add(new DocumentLink(filePath, target, LinkType.External));
                }
                else
                {
                    documentList.Add(new DocumentLink(filePath, target, LinkType.Internal));
                }
            }

            return documentList;
        }
    }
}
