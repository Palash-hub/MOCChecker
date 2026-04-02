using MOCChecker.Interfaces;

namespace MOCChecker.Services
{
    public class ConsoleReportGenerator : IReportGenerator
    {
        public void GenerateReport(IEnumerable<MarkdownDocument> documents)
        {
            // Твоя задача здесь:
            // Проанализировать собранные данные с помощью LINQ и вывести в консоль:
            // 1. Общее количество просканированных заметок.
            // 2. Список битых внутренних ссылок (Broken Internal).
            // 3. Список мертвых внешних ссылок (Broken External).
            // 4. (Опционально) Найти "сиротские" заметки — файлы, на которые нет ни одной ссылки из других документов.
            throw new NotImplementedException();
        }
    }
}
