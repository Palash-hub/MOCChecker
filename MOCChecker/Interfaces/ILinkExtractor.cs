namespace MOCChecker.Interfaces
{
    public interface ILinkExtractor
    {
        Task<List<DocumentLink>> ExtractLinksAsync(string filePath);
    }
}
