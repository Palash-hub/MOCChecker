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
            _scanner = scanner;
            _extractor = extractor;
            _validator = validator;
            _reporter = reporter;
        }

        public async Task RunAsync(string targetDirectory)
        {
            // Логика работы:
            // 1. Вызываем _scanner, получаем пути ко всем файлам.
            // 2. В цикле (или параллельно) скармливаем пути в _extractor, собирая все MarkdownDocument.
            // 3. Собираем все извлеченные DocumentLink в единую плоскую коллекцию (LINQ SelectMany).
            // 4. Отправляем эту коллекцию в _validator для массовой проверки.
            // 5. Передаем обновленные документы в _reporter для вывода результатов.
            throw new NotImplementedException();
        } 
    }
}
