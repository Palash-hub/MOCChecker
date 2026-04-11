using Spectre.Console.Cli;

namespace MOCChecker.CLI
{
    public class CheckerSettings : CommandSettings
    {
        [CommandOption("-p|--path <DIRECTORY>")]
        public string TargetDirectory {  get; set; } = string.Empty;
        
        [CommandOption("-i|--ignore-images")]
        public bool ImageIgnore { get; set; } = false;

        [CommandOption("-t|--threads <NUMBER>")]
        public int MaxConcurrentRequests { get; set; } = 10;
    }
}
