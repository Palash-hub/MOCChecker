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

            var files = new List<string>();
            var directoriesToProcess = new Queue<string>();
            directoriesToProcess.Enqueue(directoryPath);

            while (directoriesToProcess.Count > 0)
            {
                var currentDir = directoriesToProcess.Dequeue();
                var dirName = Path.GetFileName(currentDir);

                if (dirName[0] == '.')
                {
                    continue;
                }

                try
                {
                    files.AddRange(Directory.GetFiles(currentDir));

                    foreach(var subDir in Directory.GetDirectories(currentDir))
                    {
                        directoriesToProcess.Enqueue(subDir);
                    }
                }
                catch(UnauthorizedAccessException)
                {
                    // Нет доступа к папке
                }
            }

            return files;
        }
    }
}
