namespace AvansDevOps.Domain.Factory;

// FACTORY METHOD PATTERN: Creator interface.
// Defines the factory method for creating sprints.
// This follows the Open/Closed Principle: new sprint types can be
// added by creating new factories without modifying existing code.
// It also follows the Dependency Inversion Principle: clients depend
// on the ISprintFactory abstraction, not concrete sprint classes.
public interface ISprintFactory
{
    Sprint CreateSprint(string name, DateTime startDate, DateTime endDate);
}
