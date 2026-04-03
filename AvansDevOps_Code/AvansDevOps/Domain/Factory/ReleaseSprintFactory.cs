namespace AvansDevOps.Domain.Factory;

// FACTORY METHOD PATTERN: Concrete creator for ReleaseSprint.
public class ReleaseSprintFactory : ISprintFactory
{
    public Sprint CreateSprint(string name, DateTime startDate, DateTime endDate)
    {
        return new ReleaseSprint(name, startDate, endDate);
    }
}
