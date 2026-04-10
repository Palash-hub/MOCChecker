namespace MOCChecker.Interfaces
{
    public interface ILinkValidator
    {
        Task ValidateLinkAsync(IEnumerable<DocumentLink> links, Dictionary<string, string> fileIndex);
    }
}
