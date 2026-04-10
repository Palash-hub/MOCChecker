using MOCChecker.Services;

namespace MOCChecker
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            
            var validator = new ConcurrentLinkValidator(httpClient);
            var extractor = new RegexLinkExtractor();
            var reporter = new ConsoleReportGenerator();
            var scanner = new LocalFileScanner();

            var app = new Application(scanner,extractor,validator,reporter);

            await app.RunAsync("D:\\ForVaults\\ForWork3thLVL");
        }
    }
}
