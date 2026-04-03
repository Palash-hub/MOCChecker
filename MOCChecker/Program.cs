using MOCChecker.Services;

namespace MOCChecker
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var validator = new ConcurrentLinkValidator();
            var extractor = new RegexLinkExtractor();
            var reporter = new ConsoleReportGenerator();
            var scanner = new LocalFileScanner();

            var app = new Application(scanner,extractor,validator,reporter);

            await app.RunAsync("D:\\ForVaults\\ForWork3thLVL");
        }
    }
}
