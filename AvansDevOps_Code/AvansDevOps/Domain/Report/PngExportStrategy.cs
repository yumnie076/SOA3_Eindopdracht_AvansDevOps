namespace AvansDevOps.Domain.Report;

// STRATEGY PATTERN: Concrete strategy for PNG export.
// Stub implementation - in production, this would render to an image.
public class PngExportStrategy : IReportExportStrategy
{
    public string FormatName => "PNG";

    public string Export(string content)
    {
        // Stub: simulate PNG export
        return $"[PNG IMAGE]\n{content}\n[END PNG]";
    }
}
