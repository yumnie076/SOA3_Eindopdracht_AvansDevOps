namespace AvansDevOps.Domain.Report;

// DECORATOR PATTERN: Concrete decorator that adds a footer to the report.
// Footers can include version, date, confidentiality notice, etc.
public class FooterDecorator : ReportDecorator
{
    public string Version { get; }
    public DateTime GeneratedDate { get; }
    public string? ConfidentialityNotice { get; }

    public FooterDecorator(IReportComponent component, string version, DateTime generatedDate, string? confidentialityNotice = null)
        : base(component)
    {
        Version = version;
        GeneratedDate = generatedDate;
        ConfidentialityNotice = confidentialityNotice;
    }

    public override string Render()
    {
        var footer = new List<string>
        {
            "",
            "========================================",
            $"  Version: {Version}",
            $"  Generated: {GeneratedDate:dd-MM-yyyy HH:mm}",
        };

        if (!string.IsNullOrEmpty(ConfidentialityNotice))
            footer.Add($"  {ConfidentialityNotice}");

        footer.Add("========================================");

        return _wrappedComponent.Render() + "\n" + string.Join("\n", footer);
    }
}
