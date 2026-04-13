using MOCChecker.Interfaces;
using MOCChecker.Models;

namespace MOCChecker.Services
{
    public class ConcurrentLinkValidator(HttpClient httpClient, int maxConcurrentRequests) : ILinkValidator, IDisposable
    {
        private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        private readonly SemaphoreSlim _semaphore = new(maxConcurrentRequests);

        public async Task ValidateLinkAsync(IEnumerable<DocumentLink> links, IEnumerable<string> fileIndex)
        {
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

        private static Task ValidateInternalAsync(DocumentLink link, IEnumerable<string> fileIndex)
        {
            if (link == null)
            {
                throw new ArgumentException("link is null");
            }

            var cleanTarget = Path.GetFileName(link.TargetPath);

            var fileName = Path.HasExtension(cleanTarget) 
                ? link.TargetPath 
                : link.TargetPath + ".md";

            if (fileIndex.Contains(fileName))
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
