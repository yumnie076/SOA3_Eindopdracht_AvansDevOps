namespace AvansDevOps.Domain.Report;

// STRATEGY PATTERN: Strategy interface for report export.
// Different export formats (PDF, PNG, etc.) implement this interface.
// This follows the Open/Closed Principle: new export formats can be
// added without modifying existing code.
// Also follows the Dependency Inversion Principle: the Report class
// depends on this abstraction, not concrete export implementations.
public interface IReportExportStrategy
{
    string FormatName { get; }
    string Export(string content);
}
