namespace AvansDevOps.Domain.Notifications;

// OBSERVER PATTERN: Subscriber interface (Observer).
// Concrete subscribers implement this to receive notifications
// through different media (email, Slack, etc.).
// This follows the Dependency Inversion Principle: high-level modules
// depend on abstractions, not concrete notification implementations.
public interface ISubscriber
{
    void Update(string message);
}
