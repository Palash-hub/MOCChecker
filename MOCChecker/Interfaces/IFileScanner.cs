namespace MOCChecker.Interfaces
{
    public interface IFileScanner
    {
        IEnumerable<string> GetAllFiles(string directoryPath);
    }
}
