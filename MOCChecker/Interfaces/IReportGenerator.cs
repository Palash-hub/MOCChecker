namespace MOCChecker.Interfaces
{
    public interface IReportGenerator
    {
        void GenerateReport(IEnumerable<MarkdownDocument> documents);
    }
}
