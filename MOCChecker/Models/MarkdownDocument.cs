namespace MOCChecker
{
    public record MarkdownDocument(string FilePath)
    {
        public List<DocumentLink> Links { get; set; } = [];
    }
}