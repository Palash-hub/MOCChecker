using MOCChecker.Interfaces;

namespace MOCChecker.Services
{
    public class LocalFileScanner : IFileScanner
    {
        public IEnumerable<string> GetAllFiles(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                throw new ArgumentException("Path to file is empty.", nameof(directoryPath));
            }

            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"Directory is't found: {directoryPath}");
            }

            return Directory.EnumerateFiles(directoryPath, "*.*", searchOption: SearchOption.AllDirectories);
        }
    }
}
