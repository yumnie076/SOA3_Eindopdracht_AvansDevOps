namespace AvansDevOps.Domain.Pipeline;

// COMPOSITE PATTERN: Leaf class.
// Represents a single action in the pipeline (e.g., build, test, deploy).
public class PipelineAction : IPipelineComponent
{
    public string Name { get; }
    public PipelineActionType ActionType { get; }
    public Func<bool>? ExecutionLogic { get; set; }

    public PipelineAction(string name, PipelineActionType actionType)
    {
        Name = name;
        ActionType = actionType;
    }

    public bool Execute()
    {
        Console.WriteLine($"  Executing action: [{ActionType}] {Name}");
        // If custom execution logic is provided, use it; otherwise succeed by default
        return ExecutionLogic?.Invoke() ?? true;
    }

    public void Accept(IPipelineVisitor visitor) => visitor.Visit(this);
}

public enum PipelineActionType
{
    Sources,
    Package,
    Build,
    Test,
    Analyse,
    Deploy,
    Utility
}
