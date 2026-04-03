namespace AvansDevOps.Domain.Report;

// DECORATOR PATTERN: Concrete decorator that adds a header to the report.
// Headers can include company name, logo reference, project name, etc.
public class HeaderDecorator : ReportDecorator
{
    public string CompanyName { get; }
    public string ProjectName { get; }
    public string? LogoPath { get; }

    public HeaderDecorator(IReportComponent component, string companyName, string projectName, string? logoPath = null)
        : base(component)
    {
        CompanyName = companyName;
        ProjectName = projectName;
        LogoPath = logoPath;
    }

    public override string Render()
    {
        var header = new List<string>
        {
            "========================================",
            $"  {CompanyName}",
        };

        if (!string.IsNullOrEmpty(LogoPath))
            header.Add($"  [Logo: {LogoPath}]");

        header.Add($"  Project: {ProjectName}");
        header.Add("========================================");
        header.Add("");

        return string.Join("\n", header) + _wrappedComponent.Render();
    }
}
