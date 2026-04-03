namespace AvansDevOps.Domain;

using AvansDevOps.Domain.Factory;
using AvansDevOps.Domain.SCM;

public class Project
{
    public string Name { get; set; }
    public Person ProductOwner { get; set; }
    public List<BacklogItem> ProductBacklog { get; } = new();
    public List<Sprint> Sprints { get; } = new();
    public Repository? Repository { get; set; }

    public Project(string name, Person productOwner)
    {
        Name = name;
        ProductOwner = productOwner;
    }

    public BacklogItem AddBacklogItem(string title, string description, int storyPoints = 0)
    {
        var item = new BacklogItem(title, description, storyPoints);
        ProductBacklog.Add(item);
        return item;
    }

    // FACTORY METHOD PATTERN: Use a factory to create sprints
    public Sprint CreateSprint(ISprintFactory factory, string name, DateTime startDate, DateTime endDate)
    {
        var sprint = factory.CreateSprint(name, startDate, endDate);
        Sprints.Add(sprint);
        return sprint;
    }

    public void SetRepository(Repository repository)
    {
        Repository = repository;
    }
}
