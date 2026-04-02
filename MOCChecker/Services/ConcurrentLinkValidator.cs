using MOCChecker.Interfaces;
using MOCChecker.Models;

namespace MOCChecker.Services
{
    public class ConcurrentLinkValidator : ILinkValidator, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly SemaphoreSlim _semaphore;

        public ConcurrentLinkValidator(int maxConcurrentRequests = 10)
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
            _semaphore= new SemaphoreSlim(maxConcurrentRequests);
        }

        public async Task ValidateLinkAsync(IEnumerable<DocumentLink> links, string rootDirectory)
        {
            var tasks = new List<Task>();

            foreach (var link in links)
            {
                if (link.Type.Equals(LinkType.Internal))
                {
                    tasks.Add(ValidateInternalAsync(link, rootDirectory));
                }
                else
                {
                    tasks.Add(ValidateExternalAsync(link));
                }
            }

            await Task.WhenAll(tasks);
        }

        private Task ValidateInternalAsync(DocumentLink link, string rootDirectory)
        {
            if (string.IsNullOrEmpty(rootDirectory))
            {
                throw new ArgumentException(rootDirectory);
            }
            if (link == null)
            {
                throw new ArgumentException("link is null");
            }

            var fullPath = Path.Combine(rootDirectory,link.TargetPath+".md");

            if (!File.Exists(fullPath))
            {
                link.Status = LinkStatus.Broken;
            }
            else
            {
                link.Status = LinkStatus.Valid;
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
                await _httpClient.SendAsync(message);
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
