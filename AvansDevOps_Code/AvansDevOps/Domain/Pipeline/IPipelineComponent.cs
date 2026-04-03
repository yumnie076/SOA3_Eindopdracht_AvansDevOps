namespace AvansDevOps.Domain.Pipeline;

// COMPOSITE PATTERN: Component interface.
// Both individual pipeline actions and composite pipeline groups
// implement this interface, allowing uniform treatment.
// This follows the Liskov Substitution Principle: clients can treat
// individual actions and composites interchangeably.
public interface IPipelineComponent
{
    string Name { get; }
    bool Execute();
    void Accept(IPipelineVisitor visitor);
}

// Visitor interface for pipeline traversal (supports reporting)
public interface IPipelineVisitor
{
    void Visit(PipelineAction action);
    void Visit(DevelopmentPipeline pipeline);
}
