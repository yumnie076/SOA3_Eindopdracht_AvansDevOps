namespace AvansDevOps.Domain;

using AvansDevOps.Domain.Pipeline;

// FACTORY METHOD PATTERN: Concrete product - ReleaseSprint.
// A sprint that ends with a software release via a development pipeline.
public class ReleaseSprint : Sprint
{
    public ReleaseSprint(string name, DateTime startDate, DateTime endDate)
        : base(name, startDate, endDate)
    {
    }

    public void SetPipeline(DevelopmentPipeline pipeline)
    {
        Pipeline = pipeline;
    }
}
