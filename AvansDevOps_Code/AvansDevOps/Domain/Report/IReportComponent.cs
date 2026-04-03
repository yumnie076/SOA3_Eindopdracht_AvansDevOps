namespace AvansDevOps.Domain.Report;

// DECORATOR PATTERN: Component interface.
// Both the base report and decorators (header, footer) implement this.
// This follows the Open/Closed Principle: new decorations can be added
// without modifying the base report class.
// Also follows the Single Responsibility Principle: each decorator
// handles only one type of decoration.
public interface IReportComponent
{
    string Render();
}
