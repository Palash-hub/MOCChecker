using MOCChecker.Interfaces;
using MOCChecker.Models;

namespace MOCChecker.Services
{
    public class ConsoleReportGenerator : IReportGenerator
    {
        public void GenerateReport(IEnumerable<MarkdownDocument> documents)
        {
            Console.WriteLine("=== ОТЧЕТ О ВАЛИДАЦИИ СВЯЗЕЙ ===\n");

            //List<DocumentLink> listBrokenInternal = [];
            //List<DocumentLink> listBrokenExternal = [];
            var documentsScan = documents.Count();
            var allLinks = documents.SelectMany(d => d.Links).ToList();
            Console.WriteLine($"Просканировано заметок: {documentsScan}");
            Console.WriteLine($"Найдено ссылок: {allLinks.Count}\n");

            var listBrokenExternal = allLinks
                .Where(l => l.Type == LinkType.External &&
                    (l.Status == LinkStatus.Broken || l.Status == LinkStatus.Timeout))
                .ToList();

            var listBrokenInternal = allLinks
                .Where(l => l.Type == LinkType.Internal &&
                    (l.Status == LinkStatus.Broken || l.Status == LinkStatus.Timeout))
                .ToList();

            Console.WriteLine("--- Сломанные внешние ссылки ---");
            foreach (var link in listBrokenExternal)
            {
                Console.WriteLine($"Файл: {Path.GetFileName(link.SourceFilePath)} -> URL: {link.TargetPath} ({link.Status})");
            }

            Console.WriteLine("\n--- Сломанные внутренние ссылки ---");
            foreach (var link in listBrokenInternal)
            {
                Console.WriteLine($"Файл: {Path.GetFileName(link.SourceFilePath)} -> Не найдено: {link.TargetPath}");
            }
        }
    }
}
