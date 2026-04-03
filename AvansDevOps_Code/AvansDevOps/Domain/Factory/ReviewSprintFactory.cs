namespace AvansDevOps.Domain.Factory;

// FACTORY METHOD PATTERN: Concrete creator for ReviewSprint.
public class ReviewSprintFactory : ISprintFactory
{
    public Sprint CreateSprint(string name, DateTime startDate, DateTime endDate)
    {
        return new ReviewSprint(name, startDate, endDate);
    }
}
