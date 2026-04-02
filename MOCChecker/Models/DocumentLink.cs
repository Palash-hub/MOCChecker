using MOCChecker.Models;

namespace MOCChecker
{
    public record DocumentLink(string SourceFilePath, string TargetPath, LinkType Type)
    {
        public LinkStatus Status { get; set; } = LinkStatus.Pending;
    }
}