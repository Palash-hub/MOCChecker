namespace MOCChecker.Interfaces
{
    public interface ILinkValidator
    {
        Task ValidateLinkAsync(IEnumerable<DocumentLink> links, string rootDirectory);
    }
}
