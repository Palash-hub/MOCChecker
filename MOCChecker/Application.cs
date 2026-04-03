using MOCChecker.Interfaces;

namespace MOCChecker
{
    internal class Application
    {
        private readonly IFileScanner _scanner;
        private readonly ILinkExtractor _extractor;
        private readonly ILinkValidator _validator;
        private readonly IReportGenerator _reporter;

        public Application(
            IFileScanner scanner,
            ILinkExtractor extractor,
            ILinkValidator validator,
            IReportGenerator reporter)
        {
            _scanner = scanner ?? throw new ArgumentNullException(nameof(scanner)); ;
            _extractor = extractor ?? throw new ArgumentNullException(nameof(extractor)); ;
            _validator = validator ?? throw new ArgumentNullException(nameof(validator)); ;
            _reporter = reporter ?? throw new ArgumentNullException(nameof(reporter)); ;
        }

        public async Task RunAsync(string targetDirectory)
        {
            Console.WriteLine($"Начинаем сканирование директории: {targetDirectory}");

            var filePaths = _scanner.GetMarkdownFiles(targetDirectory);
            var documents = new List<MarkdownDocument>();
            var allLinks = new List<DocumentLink>();

            foreach (var path in filePaths)
            {
                var links = await _extractor.ExtractLinksAsync(path);
                documents.Add(new MarkdownDocument(path) { Links = links });
                allLinks.AddRange(links);
            }

            Console.WriteLine($"Извлечено {allLinks.Count} ссылок. Начинаем валидацию...");

            await _validator.ValidateLinkAsync(allLinks, targetDirectory);
            
            _reporter.GenerateReport(documents);
        } 
    }
}
