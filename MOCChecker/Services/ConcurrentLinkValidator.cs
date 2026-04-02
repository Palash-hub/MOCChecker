using MOCChecker.Interfaces;

namespace MOCChecker.Services
{
    public class ConcurrentLinkValidator : ILinkValidator, IDisposable
    {
        // Понадобится HttpClient для проверки внешних ссылок.
        // Важно: HttpClient должен быть один на всё приложение (Singleton), 
        // чтобы не исчерпать сокеты ОС.

        public Task ValidateLinkAsync(IEnumerable<DocumentLink> links, string rootDirectory)
        {
            // Твоя задача здесь:
            // 1. Разделить логику проверки:
            //    - Если ссылка Internal: проверить наличие файла на диске (File.Exists).
            //    - Если ссылка External: сделать асинхронный HTTP HEAD или GET запрос.
            // 2. Так как внешних ссылок может быть много, запросы нужно делать параллельно
            //    с помощью Task.WhenAll().
            // 3. ОБЯЗАТЕЛЬНО: Ограничить количество одновременных HTTP-запросов 
            //    (например, не больше 10 потоков за раз), чтобы не получить бан от серверов. 
            //    Здесь идеально подойдет SemaphoreSlim.
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // Здесь нужно правильно освободить неуправляемые ресурсы (например, HttpClient)
        }
    }
}
