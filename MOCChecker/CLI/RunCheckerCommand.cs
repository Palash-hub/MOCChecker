using MOCChecker.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MOCChecker.CLI
{
    public class RunCheckerCommand : AsyncCommand<CheckerSettings>
    {
        protected override async Task<int> ExecuteAsync(CommandContext context, CheckerSettings settings, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(settings.TargetDirectory))
            {
                AnsiConsole.MarkupLine("[red]Ошибка: Необходимо указать путь. Используйте флаг --path[/]");
                return -1;
            }

            if (!Directory.Exists(settings.TargetDirectory))
            {
                AnsiConsole.MarkupLine($"[red]Ошибка: директория {settings.TargetDirectory} не существует[/]");
                return -1;
            }

            AnsiConsole.MarkupLine($"[green]Настройка окружения...[/]");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

            var validator = new ConcurrentLinkValidator(httpClient, settings.MaxConcurrentRequests);
            var extractor = new RegexLinkExtractor();
            var reporter = new ConsoleReportGenerator();
            var scanner = new LocalFileScanner();

            var app = new Application(scanner, extractor, validator, reporter);

            try
            {
                await app.RunAsync(settings.TargetDirectory, settings.ImageIgnore);
                AnsiConsole.MarkupLine("[bold green]Проверка успешно завершена![/]");
                return 0; 
            }
            catch(Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return -1;
            }
        }
    }
}
