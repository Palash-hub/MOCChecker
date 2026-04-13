using MOCChecker.CLI;
using Spectre.Console.Cli;

namespace MOCChecker
{
    public class Program
    {
        static async Task<int> Main(string[] args)
        {
            var app = new CommandApp<RunCheckerCommand>();

            app.Configure(config =>
            {
                config.SetApplicationName("MOCChecker");
                config.SetApplicationVersion("1.0.0");
            });

            return await app.RunAsync(args);
        }
    }
}
