namespace AvansDevOps.Domain.Report;

// STRATEGY PATTERN: Concrete strategy for PDF export.
// Stub implementation - in production, this would use a PDF library.
public class PdfExportStrategy : IReportExportStrategy
{
    public string FormatName => "PDF";

    public string Export(string content)
    {
        // Stub: simulate PDF export
        return $"[PDF DOCUMENT]\n{content}\n[END PDF]";
    }
}
