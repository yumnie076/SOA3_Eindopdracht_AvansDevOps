namespace AvansDevOps.Domain.Report;

// Combines STRATEGY PATTERN (export format) and DECORATOR PATTERN (headers/footers).
// The report content is built using the Decorator pattern, and then
// exported using the Strategy pattern.
public class SprintReport
{
    private IReportComponent _reportComponent;
    private IReportExportStrategy _exportStrategy;

    public SprintReport(IReportComponent reportComponent, IReportExportStrategy exportStrategy)
    {
        _reportComponent = reportComponent;
        _exportStrategy = exportStrategy;
    }

    // STRATEGY PATTERN: Allow changing export strategy at runtime
    public void SetExportStrategy(IReportExportStrategy strategy)
    {
        _exportStrategy = strategy;
    }

    // DECORATOR PATTERN: Allow changing report component (with decorators)
    public void SetReportComponent(IReportComponent component)
    {
        _reportComponent = component;
    }

    public string Generate()
    {
        string renderedContent = _reportComponent.Render();
        return _exportStrategy.Export(renderedContent);
    }

    public string GetFormatName() => _exportStrategy.FormatName;
}
