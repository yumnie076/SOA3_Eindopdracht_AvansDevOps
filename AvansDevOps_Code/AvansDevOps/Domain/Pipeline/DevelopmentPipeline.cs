namespace AvansDevOps.Domain.Pipeline;

// COMPOSITE PATTERN: Composite class.
// A pipeline contains multiple pipeline components (actions or sub-pipelines).
// Actions are executed sequentially; if any action fails, the pipeline stops.
// This follows the Single Responsibility Principle: the pipeline only manages
// execution order, while individual actions handle their own logic.
public class DevelopmentPipeline : IPipelineComponent
{
    public string Name { get; }
    private readonly List<IPipelineComponent> _components = new();

    public IReadOnlyList<IPipelineComponent> Components => _components.AsReadOnly();

    public DevelopmentPipeline(string name)
    {
        Name = name;
    }

    public void AddComponent(IPipelineComponent component)
    {
        _components.Add(component);
    }

    public void RemoveComponent(IPipelineComponent component)
    {
        _components.Remove(component);
    }

    public bool Execute()
    {
        Console.WriteLine($"Starting pipeline: {Name}");
        foreach (var component in _components)
        {
            bool success = component.Execute();
            if (!success)
            {
                Console.WriteLine($"Pipeline '{Name}' FAILED at: {component.Name}");
                return false;
            }
        }
        Console.WriteLine($"Pipeline '{Name}' completed SUCCESSFULLY.");
        return true;
    }

    public void Accept(IPipelineVisitor visitor)
    {
        visitor.Visit(this);
        foreach (var component in _components)
        {
            component.Accept(visitor);
        }
    }
}
