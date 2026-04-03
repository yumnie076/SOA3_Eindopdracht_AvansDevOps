namespace AvansDevOps.Domain.Report;

// DECORATOR PATTERN: Abstract decorator.
// Wraps an IReportComponent and delegates the Render call.
// Concrete decorators extend this to add headers, footers, etc.
public abstract class ReportDecorator : IReportComponent
{
    protected readonly IReportComponent _wrappedComponent;

    protected ReportDecorator(IReportComponent component)
    {
        _wrappedComponent = component;
    }

    public abstract string Render();
}
