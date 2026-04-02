namespace MOCChecker.Interfaces
{
    public interface IFileScanner
    {
        IEnumerable<string> GetMarkdownFiles(string directoryPath);
    }
}
