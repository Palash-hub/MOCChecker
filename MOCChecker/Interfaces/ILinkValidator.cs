namespace MOCChecker.Interfaces
{
    public interface ILinkValidator
    {
        Task ValidateLinkAsync(IEnumerable<DocumentLink> links, IEnumerable<string> fileIndex);
    }
}
