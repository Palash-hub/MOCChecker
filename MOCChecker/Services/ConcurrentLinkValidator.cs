using MOCChecker.Interfaces;
using MOCChecker.Models;

namespace MOCChecker.Services
{
    public class ConcurrentLinkValidator : ILinkValidator, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly SemaphoreSlim _semaphore;

        public ConcurrentLinkValidator(HttpClient httpClient, int maxConcurrentRequests = 10)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            //_httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            _semaphore= new SemaphoreSlim(maxConcurrentRequests);
        }

        public async Task ValidateLinkAsync(IEnumerable<DocumentLink> links, Dictionary<string, string> fileIndex)
        {
            //var allFiles = Directory.GetFiles(rootDirectory, "*.*", SearchOption.AllDirectories);
            //var fileIndex = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            //foreach (var file in allFiles)
            //{
            //    var fileName = Path.GetFileName(file);
            //    if (fileName.EndsWith(".md") || fileName.EndsWith(".png"))
            //    {
            //        fileIndex.TryAdd(fileName, file);
            //    }
            //}

            var tasks = new List<Task>();

            foreach (var link in links)
            {
                if (link.Type == LinkType.Internal)
                {
                    tasks.Add(ValidateInternalAsync(link, fileIndex));
                }
                else
                {
                    tasks.Add(ValidateExternalAsync(link));
                }
            }

            await Task.WhenAll(tasks);
        }

        private Task ValidateInternalAsync(DocumentLink link, Dictionary<string, string> fileIndex)
        {
            if (link == null)
            {
                throw new ArgumentException("link is null");
            }

            var cleanTarget = Path.GetFileName(link.TargetPath);

            var fileName = Path.HasExtension(cleanTarget) 
                ? link.TargetPath 
                : link.TargetPath + ".md";

            if (fileIndex.ContainsKey(fileName))
            {
                link.Status = LinkStatus.Valid;
            }
            else
            {
                link.Status = LinkStatus.Broken;
            }

            return Task.CompletedTask;
        }

        private async Task ValidateExternalAsync(DocumentLink link)
        {
            if (link == null)
            {
                throw new ArgumentException("link is null");
            }

            await _semaphore.WaitAsync();

            try
            {
                var message = new HttpRequestMessage()
                {
                    RequestUri = new Uri(link.TargetPath),
                    Method = HttpMethod.Head
                };

                var response = await _httpClient.SendAsync(message);

                if (response.IsSuccessStatusCode)
                {
                    link.Status = LinkStatus.Valid;
                }
                else
                {
                    link.Status = LinkStatus.Broken;
                }
            }
            catch(Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                link.Status= LinkStatus.Timeout;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _semaphore.Dispose();
        }
    }
}
